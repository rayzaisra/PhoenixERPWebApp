using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenix.WebExtension.Exceptions
{
    public static class ExceptionDigger
    {
        public static string Messages(this Exception exception)
        {
            var ms = new List<string>() { exception.Message };
            var ex = exception;
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;

                if (!ms.Contains(ex.Message))
                {
                    ms.Add(ex.Message);
                }
            }

            return String.Join("<br/>", ms.ToArray());
        }
    }
}
