using Phoenix.Shared.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Text;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Phoenix.Shared.Json.Results
{
    public class InvalidTokenResult
    {
        public static JsonResult Get()
        {
            BaseGeneralResponse response = new BaseGeneralResponse();
            response.Code = "401";
            response.Message = "Invalid Token";
            response.Success = false;

            return new JsonResult(response, new JsonSerializerSettings
            {
                // ContractResolver = new CamelCasePropertyNamesContractResolver()
                ContractResolver = new DefaultContractResolver()
            });
        }
    }
}