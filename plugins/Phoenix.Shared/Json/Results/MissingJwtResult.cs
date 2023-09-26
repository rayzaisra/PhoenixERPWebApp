using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.Shared.Responses;

namespace Phoenix.Shared.Json.Results
{
    public class MissingJwtResult
    {
        public static JsonResult Get()
        {
            BaseGeneralResponse response = new BaseGeneralResponse();
            response.Code = "401";
            response.Message = "Missing JWT Token";
            response.Success = false;

            return new JsonResult(response, new JsonSerializerSettings
            {
               // ContractResolver = new CamelCasePropertyNamesContractResolver()
                ContractResolver = new DefaultContractResolver()
            });
        }

    }
}