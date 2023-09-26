using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Phoenix.ApiExtension.Exceptions
{
    public class ForbiddenAccessException : Exception
    {
        public ForbiddenAccessException()
            :base("You have no authorization to the current access.")
        {

        }

        public ForbiddenAccessException(string message)
            :base(message)
        {
            
        }

        public string Status => "Forbidden";
        public int StatusCode => StatusCodes.Status403Forbidden; //(int)HttpStatusCode.Forbidden;
    }
}
