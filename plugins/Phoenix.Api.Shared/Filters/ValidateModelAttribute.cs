using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phoenix.Api.Shared.Filters
{
    public class ValidateModelAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                GeneralResponse<nulldata> response = new GeneralResponse<nulldata>();
                string messages = string.Join("; ", context.ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                response.Code = "401";
                response.Message = messages;
                response.Success = false;

                context.Result = new JsonResult(response, new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver()
                });
            }
        }
    }

    internal class nulldata
    {
    }
}
