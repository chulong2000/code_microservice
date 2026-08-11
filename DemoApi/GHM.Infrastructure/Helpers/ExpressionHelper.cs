using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace GHM.Infrastructure.Helpers
{

    public static class ExpressionHelper
    {
         public static string StripTagsRegex(this string source)
        {
            return Regex.Replace(source, "<.*?>", string.Empty);
        }
    }
}