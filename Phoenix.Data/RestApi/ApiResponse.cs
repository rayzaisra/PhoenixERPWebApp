using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phoenix.Data.RestApi
{
    public class ApiResponse
    {
        public ApiResponse()
        {
            Message = "OK";
            Success = true;
        }

        public ApiResponse(object result)
        {
            if (result == null)
            {
                Message = "Fail";
                Success = false;
            }
            else
            {
                var type = result.GetType();
                if (typeof(IEnumerable).IsAssignableFrom(type))
                {
                    if ((result as IEnumerable<object>).Count() == 0)
                    {
                        Data = result;
                        Message = $"Fail. Data count 0 from {type.UnderlyingSystemType.FullName}";
                        Success = false;
                        return;
                    }
                }

                Data = result;
                Message = "OK";
                Success = true;
            }
        }

        public ApiResponse(bool success, string message)
        {
            Data = null;
            Message = message;
            Success = success;
        }

        public ApiResponse(object data, bool success)
        {
            Data = data;
            Message = success ? "OK" : "Fail";
            Success = success;
        }

        public ApiResponse(object data, bool success, string message)
        {
            Data = data;
            Message = message;
            Success = success;
        }

        public bool Success { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }
    }
}