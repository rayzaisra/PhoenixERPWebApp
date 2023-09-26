using Microsoft.AspNetCore.Mvc;
using Phoenix.Shared.Core.Areas.General.Services;
using Phoenix.Shared.Constants;
using Phoenix.Shared.Core.Filters;
using Phoenix.Shared.Responses;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Areas.General.Dtos;
using Phoenix.Shared.Helpers;
using System.Collections.Generic;
using Phoenix.Data;
using Phoenix.Data.Models;
using System.Threading.Tasks;
using Phoenix.Data.RestApi;
using System;
using Microsoft.AspNetCore.Authorization;
using Phoenix.Data.Attributes;

namespace Phoenix.Api.Areas.General.Controllers
{
    [AllowAnonymousRole]
    [Route("api/general/[controller]")]
    public class TransactionLogController : Controller
    {
        DataContext context;
        public TransactionLogController(DataContext context)
        {
            this.context = context; 
        }

        [AllowAnonymousRole]
        public async Task<IActionResult> Post([FromBody] TransactionLog log)
        {
            var userId = context.GetUserId();
            if (string.IsNullOrWhiteSpace(userId))
                return Ok(new ApiResponse(true, "Not implemented. User not login"));

            log.Id = System.Guid.NewGuid();
            log.UserId = userId;
            log.CreatedBy = "SYSTEM";
            log.CreatedDate = DateTime.Now;

            await context.PhoenixAddAsync(log);
            var result = (await context.SaveChangesAsync()) > 0;

            return Ok(new ApiResponse(result, result ? "Success" : "Fail"));
        }
    }
}