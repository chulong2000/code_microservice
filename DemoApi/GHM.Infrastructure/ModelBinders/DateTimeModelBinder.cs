using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace GHM.Infrastructure.ModelBinders
{
    public class DateTimeModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException(nameof(bindingContext));

            // Try to fetch the value of the argument by name
            var modelName = bindingContext.ModelName;

            var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);
            if (valueProviderResult == ValueProviderResult.None)
                return Task.CompletedTask;

            var dateStr = valueProviderResult.FirstValue;
            if (string.IsNullOrEmpty(dateStr))
                return Task.CompletedTask;

            //string[] formats = { "dd/MM/yyyy", "dd/M/yyyy", "d/M/yyyy", "d/MM/yyyy",
            //    "dd/MM/yy", "dd/M/yy", "d/M/yy", "d/MM/yy",
            //    // 12 hour
            //    "dd/MM/yyyy hh:mm", "dd/M/yyyy hh:mm", "d/M/yyyy hh:mm", "d/MM/yyyy hh:mm",
            //    "dd/MM/yy hh:mm", "dd/M/yy hh:mm", "d/M/yy hh:mm", "d/MM/yy hh:mm",
            //    // 24 hour
            //    "dd/MM/yyyy HH:mm", "dd/M/yyyy HH:mm", "d/M/yyyy HH:mm", "d/MM/yyyy HH:mm",
            //    "dd/MM/yy HH:mm", "dd/M/yy HH:mm", "d/M/yy HH:mm", "d/MM/yy HH:mm",
            //};
            bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);
            if (!DateTime.TryParse(dateStr, out var date))
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName,
                    $"DateTime should be in format: {CultureInfo.CurrentCulture.DateTimeFormat.FullDateTimePattern}");
                return Task.CompletedTask;
            }

            bindingContext.Result = ModelBindingResult.Success(date);
            return Task.CompletedTask;
        }
    }
}
