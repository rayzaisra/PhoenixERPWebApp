using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Net;

namespace Phoenix.ApiExtension.Exceptions
{
    public class InvalidModelStateException : Exception
    {
        public InvalidModelStateException(string message)
            : base(message)
        {

        }
        public HttpStatusCode StatusCode { get; set; }
        public string Status { get; set; }
        public ModelStateDictionary ModelState { get; set; }
    }

}
