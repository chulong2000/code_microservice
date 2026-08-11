using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace GHM.Infrastructure.Helpers
{
    /// <summary>
    /// It supports for converting from object to data type that you need as: int, long, float...
    /// </summary>
    public sealed class Converter
    {
        /// <summary>
        /// Convert object to string
        /// </summary>
        /// <param name="value"></param>
        /// <returns>string</returns>
        public static string ToString(object value)
        {
            return value?.ToString() ?? string.Empty;
        }

        /// <summary>
        /// Convert object to int
        /// </summary>
        /// <param name="value"></param>
        /// <returns>int</returns>
        public static int ToInt(object value)
        {
            try
            {
                return value == null ? 0 : int.Parse(value.ToString());
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Convert object to long
        /// </summary>
        /// <param name="value"></param>
        /// <returns>long</returns>
        public static long ToLong(object value)
        {
            try
            {
                return value == null ? 0 : long.Parse(value.ToString());
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Convert object to bool
        /// </summary>
        /// <param name="value"></param>
        /// <returns>bool</returns>
        public static bool ToBool(object value)
        {
            try
            {
                switch (value.ToString().ToLower())
                {
                    case "1":
                    case "true":
                        return true;
                    case "0":
                    case "false":
                        return false;
                }
                return bool.Parse(value.ToString());
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Convert object to decimal
        /// </summary>
        /// <param name="value"></param>
        /// <returns>decimal</returns>
        public static Decimal ToDecimal(object value)
        {
            try
            {
                return value == null ? 0 : Decimal.Parse(value.ToString());
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Convert object to float
        /// </summary>
        /// <param name="value"></param>
        /// <returns>float</returns>
        public static float ToFloat(object value)
        {
            try
            {
                return value == null ? 0 : float.Parse(value.ToString());
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Convert object to Guid
        /// </summary>
        /// <param name="value"></param>
        /// <returns>guid</returns>
        public static Guid ToGuid(object value)
        {
            try
            {
                return value == null ? Guid.Empty : new Guid(value.ToString());
            }
            catch
            {
                return Guid.Empty;
            }
        }

        /// <summary>
        /// Convert object to DateTime
        /// </summary>
        /// <param name="value"></param>
        /// <returns>datetime</returns>
        public static DateTime ToDateTime(object value)
        {
            try
            {
                return value == null ? DateTime.MinValue : DateTime.Parse(value.ToString());
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Convert object to DateTime with cultureInfo
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cultureInfo"></param>
        /// <returns>datetime</returns>
        public static DateTime ToDateTime(object value, CultureInfo cultureInfo)
        {
            try
            {
                return value == null ? DateTime.MinValue : DateTime.Parse(value.ToString(), cultureInfo);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
    }
}