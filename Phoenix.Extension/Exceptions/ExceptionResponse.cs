using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Phoenix.ApiExtension.Exceptions
{
    public class ExceptionResponse : ApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }
    }
}
