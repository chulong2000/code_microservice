using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace GHM.Infrastructure.ModelBinders
{
    public class ModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext.BindingSource == null || bindingContext.BindingSource.DisplayName != "Body")
                return Task.CompletedTask;

            var context = bindingContext.HttpContext;
            var initialBody = context.Request.Body; // Workaround

            context.Request.EnableBuffering();
            var buffer = new byte[Convert.ToInt32(context.Request.ContentLength)];
            var i = Task.Run(() => context.Request.Body.ReadAsync(buffer, 0, buffer.Length)).Result;
            var json = Encoding.UTF8.GetString(buffer);
            context.Request.Body = initialBody; // Workaround

            var modelType = bindingContext.ModelType;
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                DateFormatString = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern
            };
            try
            {
                var model = JObject.Parse(json);
                var properties = bindingContext.ModelMetadata.Properties;
                foreach (var property in properties)
                {
                    var value = model.Value<string>(property.Name);
                    if ((property.ModelType == typeof(DateTime) || property.ModelType == typeof(DateTime?))
                        && value != null)
                    {
                        //if (DateTime.TryParseExact(value, CultureInfo.CurrentCulture, DateTimeStyles.None,
                        //    out var parsedDate))
                        //{
                        //    model.SetValue(property.Name, parsedDate);
                        //}
                        //else
                        //{
                        //    bindingContext.ModelState.TryAddModelError(bindingContext.ModelName,
                        //        $"DateTime should be in format: {CultureInfo.CurrentCulture.DateTimeFormat.FullDateTimePattern}");
                        //}
                    }
                }

                bindingContext.Result = ModelBindingResult.Success(model);
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex);
                bindingContext.ModelState.TryAddModelError("Exception", "Binding format is invalid.");
                bindingContext.Result = ModelBindingResult.Success(null);
            }
            return Task.CompletedTask;
        }
    }
}
