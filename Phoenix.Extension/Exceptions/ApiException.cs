using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Phoenix.ApiExtension.Exceptions
{
    public class ApiException :Exception
    {
        public ApiException(string message)
        : base(message)
        {

        }
        public HttpStatusCode StatusCode { get; set; }
        public string Status { get; set; }
    }
}
