using System;
using System.Collections.Generic;

namespace Phoenix.ApiExtension.Extensions
{
    public static class ExceptionGrabber
    {
        public static string GetExMessages(this Exception exception)
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

            return String.Join(" ", ms.ToArray());
        }
    }
}