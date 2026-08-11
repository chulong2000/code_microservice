using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace GHM.Infrastructure.ModelBinders
{
    public class DateTimeModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            //var isSimpleType = context.Metadata.ModelType.IsPrimitive
            //                   || context.Metadata.ModelType.IsEnum
            //                   || context.Metadata.ModelType == typeof(string)
            //                   || context.Metadata.ModelType == typeof(decimal);

            //if (isSimpleType)
            //{
            if (context.Metadata.ModelType == typeof(DateTime) ||
                context.Metadata.ModelType == typeof(DateTime?))
            {
                return new DateTimeModelBinder();
            }
            //}
            //else
            //{
            //    return new ModelBinder();
            //}

            return null;
        }
    }
}
