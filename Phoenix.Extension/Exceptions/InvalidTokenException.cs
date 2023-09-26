using System;
using System.Net;

namespace Phoenix.ApiExtension.Exceptions
{
    public class InvalidTokenException : Exception
    {
        public InvalidTokenException(string message)
            : base(message)
        {

        }
        public HttpStatusCode StatusCode { get; set; }
        public string Status { get; set; }
    }
}
