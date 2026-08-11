using GHM.Infrastructure.Extensions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace GHM.Infrastructure.Helpers
{
    public static class Common
    {
        /// <summary>
        /// Get Current Account Name
        /// </summary>
        /// <returns>string</returns>
        public static string GetCurrentAccountName()
        {
            return Environment.UserName;
        }

        public static string GetFirstName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                return string.Empty;

            fullName = fullName.Trim();
            int index = fullName.IndexOf(' ');

            // Nếu không có dấu cách, trả về toàn bộ chuỗi
            return index == -1 ? fullName : fullName.Substring(0, index);
        }

        public static string GetMiddleName(string fullName)
        {
            if (string.IsNullOrEmpty(fullName))
                return string.Empty;

            var names = fullName.Split(' ');
            if (!names.Any()) return string.Empty;

            if (names.Length > 3)
            {
                var middleName = string.Empty;
                for (var i = 1; i < names.Length - 1; i++)
                {
                    middleName += names[i] + " ";
                }
                return middleName;
            }

            if (names.Length < 3)
                return string.Empty;

            return names[1];
        }

        public static string GetLastName(string fullName)
        {
            if (string.IsNullOrEmpty(fullName))
                return string.Empty;

            var names = fullName.Split(' ');

            return names.Any() ? names.LastOrDefault() : string.Empty;
        }

        public static string GetFileName(string fileName)
        {
            var filePathArray = fileName.Split('/');
            return filePathArray.LastOrDefault();
        }

        public static int MonthDifference(this DateTime lValue, DateTime rValue)
        {
            return Math.Abs((lValue.Month - rValue.Month) + 12 * (lValue.Year - rValue.Year));
        }

        public static void ConvertToGlobalDateTime(this DateTime inDate, out DateTime outDate)
        {
            string[] formats = {"d/M/yyyy h:mm:ss tt", "d/M/yyyy h:mm tt",
                         "DD/MM/yyyy hh:mm:ss", "D/M/yyyy h:mm:ss",
                         "d/M/yyyy hh:mm tt", "d/M/yyyy hh tt",
                         "d/M/yyyy h:mm", "d/M/yyyy h:mm",
                         "dd/MM/yyyy hh:mm", "dd/MM/yyyy hh:mm"};
            DateTime.TryParseExact(inDate.ToString(), formats, new CultureInfo("en-US"), DateTimeStyles.None, out outDate);
        }

        public static string ToAlphabetId<T>(this T number, int numberOfCharacter, int numberOfDigit)
        {
            string characters = "";
            string digits = "";
            double digitValue = Math.Pow(10, numberOfDigit);
            var doubleNumber = Convert.ToDouble(number);
            // Generate charcters.				
            for (int i = 0; i < numberOfCharacter; i++)
            {
                var value = Math.Pow(26, i);
                characters = (char)(doubleNumber / (value * digitValue) % 26 + 65) + characters;
            }

            // Generate number.
            for (int i = 0; i < numberOfDigit; i++)
            {
                digits = (char)(doubleNumber / Math.Pow(10, i) % 10 + 48) + digits;
            }
            return $"{characters}{digits}";
        }

        //public static Stream GetStreamFromUrl(string url)
        //{
        //    byte[] data = null;
        //    using (var wc = new WebClient())
        //    {
        //        data = wc.DownloadData(url);
        //    }
        //    return new MemoryStream(data);
        //}

        public static int GetQuarter(this DateTime date)
        {
            return (int)((date.Month + 2) / 3);
        }

        public static Image ResizeImage(int newWidth, int newHeight, string url)
        {
            using var fileStream = new FileStream(url, FileMode.Open, FileAccess.Read);
            var imgPhoto = Image.Load(fileStream);
            imgPhoto.Mutate(x => x.Resize(newWidth, newHeight));
            return imgPhoto;
        }

        public static string ChangeFormatPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber)) return null;
            return string.Concat("84", phoneNumber[^9..]);
        }

        public static string GenerateRandomCode(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[length];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            return new String(stringChars);
        }

        public static string GenerateCode(string input)
        {
            var nameSplit = input.Split(' ');
            var output = string.Empty;
            foreach (var item in nameSplit)
            {
                output += StringExtension.GetFirstCharacter(item);
            }
            return output;
        }

        public static CheckViettel CheckPhoneNumberViettel(string phoneNumber)
        {
            try
            {
                string[] list = { "8486", "8496", "8497", "8498", "8432", "8433", "8434", "8435", "8436", "8437", "8438", "8439" };
                var phone = string.Concat("84", phoneNumber[^9..]);
                var check = list.Contains(phone[..4]);
                return new CheckViettel
                {
                    PhoneNumber = phone,
                    IsViettel = check
                };
            }
            catch
            {
                return null;
            }
        }

        public class CheckViettel
        {
            public string PhoneNumber { get; set; }
            public bool IsViettel { get; set; }
        }

        public static string ConvertDateTimeToString(DateTime dateTime)
        {
            return dateTime.ToString("yyMMddHHmmss");
        }


        public static StringBuilder AppendWithSpace(this StringBuilder sb, string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return sb;

            value = value.Trim();

            // Nếu câu chưa kết thúc bằng ., ?, ! thì tự thêm dấu .
            if (!".?!".Contains(value[^1]))
                value += ".";

            // Nếu StringBuilder chưa kết thúc bằng khoảng trắng -> thêm khoảng trắng
            if (sb.Length > 0 && !char.IsWhiteSpace(sb[^1]))
                sb.Append(' ');

            return sb.Append(value);
        }
    }
}
