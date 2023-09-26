using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenix.WebExtension.Extentions
{
    public static class StringExtension
    {
        public static string ToHtmlString(this Microsoft.AspNetCore.Html.IHtmlContent content)
        {
            using (var writer = new System.IO.StringWriter())
            {
                content.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                return writer.ToString();
            }
        }

        public static string ToSafeString(this object value)
        {
            if (value == null)
                return null;

            return Convert.ToString(value).Trim();
        }
    }
}
