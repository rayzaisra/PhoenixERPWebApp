using System;
using System.Collections.Generic;
using System.Text;

namespace CodeMarvel.Infrastructure.ModelShared
{
    public partial class ResponseStatus
    {
        public bool Success;

        public string Message;
    }

    public partial class ApiResponse
    {
        public ResponseStatus Status;

        public object Data;

        public ApiResponse()
        {
            Status = new ResponseStatus()
            {
                Success = false
            };
        }
    }
}
