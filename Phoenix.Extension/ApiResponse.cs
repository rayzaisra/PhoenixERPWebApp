using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Phoenix.ApiExtension
{
    public class ApiResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        public object Data { get; set; }
    }

    public class ApiErrorResponse : ApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }
    }

    public class FileResponse : ApiResponse
    {
        public string FileName { get; set; }
        public byte[] File { get; set; }
    }
}
