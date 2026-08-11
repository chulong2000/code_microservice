using FluentValidation;
using GHM.Infrastructure.Extensions;
using System;

namespace GHM.Infrastructure.Helpers.Validations
{
    public static class CustomValidators
    {

        /// <summary>
        /// Validate phone number.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="ruleBuilder"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, TElement> MustBePhoneNumber<T, TElement>(this IRuleBuilder<T, TElement> ruleBuilder, string message)
        {
            return ruleBuilder.Must((rootObject, element, context) =>
            {
                if (element == null)
                    return true;

                var elementValue = element?.ToString();
                return elementValue.IsPhoneNumber();
            })
            .WithMessage(message);
        }

        /// <summary>
        /// Validate number.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="ruleBuilder"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, TElement> MustBeNumber<T, TElement>(this IRuleBuilder<T, TElement> ruleBuilder, string message)
        {

            return ruleBuilder.Must((rootObject, element, context) =>
                {
                    if (element == null)
                        return true;

                    var elementValue = element?.ToString();
                    return int.TryParse(elementValue, out int number) || decimal.TryParse(elementValue,
                                                                          out decimal decimalNumber)
                                                                      || float.TryParse(elementValue,
                                                                          out float floatNumber) ||
                                                                      double.TryParse(elementValue,
                                                                          out double doubleNumber);
                })
                .WithMessage(message);
        }

        /// <summary>
        /// Validate email.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="ruleBuilder"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, TElement> MustBeEmail<T, TElement>(this IRuleBuilder<T, TElement> ruleBuilder, string message)
        {
          
            return ruleBuilder.Must((rootObject, element, context) =>
             {
                 if (element == null)
                     return true;

                 var elementValue = element?.ToString();
                 return string.IsNullOrEmpty(elementValue) || elementValue.IsEmailAddress();
             })
             .WithMessage(message);
        }

        public static IRuleBuilderOptions<T, TElement> NotNullAndEmpty<T, TElement>(this IRuleBuilder<T, TElement> ruleBuilder, string message)
        {
            return ruleBuilder.Must((rootObject, element, context) =>
         {
             if (element == null)
                 return false;

             var elementValue = element?.ToString()?.Trim();
             return !string.IsNullOrEmpty(elementValue);
         })
         .WithMessage(message);
        }

        public static IRuleBuilderOptions<T, TElement> MustBeValidDate<T, TElement>(this IRuleBuilder<T, TElement> ruleBuilder, string message)
        {
            return ruleBuilder.Must((rootObject, element, context) =>
                {
                    if (element == null)
                        return true;

                    var elementValue = element?.ToString();
                    if (string.IsNullOrEmpty(elementValue))
                        return true;

                    var isValid = DateTime.TryParse(elementValue, out var date);
                    return isValid;
                })
                .WithMessage(message);
        }
    }
}
