using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Phoenix.ApiExtension
{
    public static class ValueExtension
    {
        public static string ToSafeString(this object value)
        {
            if (value == null)
                return null;

            return value.ToString();
        }

        public static byte[] ToByteArray(Stream stream)
        {
            if (stream is MemoryStream)
            {
                return ((MemoryStream)stream).ToArray();
            }
            else
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    stream.Position = 0;
                    stream.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }

        private static string GetValidProperty(string fieldName)
        {
            if (string.IsNullOrWhiteSpace(fieldName))
                return fieldName;

            var valid = fieldName.StartsWith("_") || char.IsLetter(fieldName.FirstOrDefault());
            if (valid)
                return fieldName;
            else
            {
                return GetValidProperty(fieldName.Substring(1));
            }
        }

        public static MemoryStream ToMemoryStream(this Stream stream)
        {
            var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            return memoryStream;
        }

        public static void SetInstanceField(this object instance, string fieldName, object value)
        {
            if (string.IsNullOrWhiteSpace(fieldName))
                return;

            //var pattern = @"[- ~!$%;:|`\\\/\,\.\{\}\(\)\[\]\<\>$*'"",&#^@]";
            //fieldName = Regex.Replace(fieldName, pattern, string.Empty);
            var pattern = "[^A-Za-z0-9-_]"; //@"[\d-]"; \\ remove digits
            fieldName = Regex.Replace(fieldName, pattern, "");

            fieldName = GetValidProperty(fieldName);
            if (string.IsNullOrWhiteSpace(fieldName))
                return;

            BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.IgnoreCase;
            PropertyInfo property = instance.GetType().GetProperty(fieldName, bindFlags);
            if (property != null)
            {
                Type propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

                if (value == null)
                    return;

                value = Convert.ChangeType(value, propertyType);

                property.SetValue(instance, value, null);
            }
        }

        public static bool Contains(this string source, string value, StringComparison comparisonType) => source?.IndexOf(value, comparisonType) >= 0;
    }
}
