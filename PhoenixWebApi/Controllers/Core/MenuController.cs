using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using CodeMarvel.Infrastructure.ModelShared;
using DataAccess.PhoenixERP;
using DataTables.AspNet.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhoenixWebApi.Base;
using BusinessLogic.Core;
using BusinessLogic.Services;
using BusinessLogic.Services.General;
using Omu.ValueInjecter;
using DataAccess.PhoenixERP.General;
using Newtonsoft.Json;

namespace PhoenixWebApi.Controllers.Core
{
    [ApiController]
    [Route("api/core/[controller]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = @"Core")]
    public class MenuController : ApiBaseController
    {
        [HttpPost]
        [Route("save")]
        public async Task<IActionResult> SaveEntity([FromBody] application_menuDTO Data)
        {
            appLogger.Debug(JsonConvert.SerializeObject(Data));
            var service = new Menu(AppUserData);
            var saveResult = await Task.Run<ApiResponse>(() =>
            {

                bool isNew = false;
                var result = new ApiResponse();
                try
                {
                    result.Status.Success = service.SaveEntity(Data, ref isNew);
                    if (result.Status.Success)
                    {
                        if (isNew)
                        {
                            result.Status.Message = "The data has been added.";
                        }
                        else
                        {
                            result.Status.Message = "The data has been updated.";
                        }
                        result.Data = new
                        {
                            recordID = Data.id
                        };
                    }
                    else
                    {
                        result.Status.Message = "Record is not saved !";
                    }
                }
                catch (Exception ex)
                {
                    result.Status.Message = ex.Message;
                }
                return result;
            });

            if (saveResult == null) return new EmptyResult();
            else return Ok(saveResult);
        }


        [HttpGet]
        [Route("BuildMenu")]
        public async Task<IActionResult> BuildMenu()
        {
            
            var service = new Menu(AppUserData);
            var saveResult = await Task.Run<ApiResponse>(() =>
            {

                var result = new ApiResponse();
                try
                {
                    result.Status.Success = true;
                    result.Data = service.GenerateMenu();
                }
                catch (Exception ex)
                {
                    result.Status.Message = ex.Message;
                }
                return result;
            });

            if (saveResult == null) return new EmptyResult();
            else return Ok(saveResult);
        }


        [HttpGet]
        [Route("orderMenu")]
        public async Task<IActionResult> OrderMenu([FromQuery(Name= "recordId")] String recordId,
            [FromQuery(Name = "targetId")] String targetId,
            [FromQuery(Name = "newOrderIndex")] int newOrderIndex)
        {
            var service = new Menu(AppUserData);
            var saveResult = await Task.Run<ApiResponse>(() =>
            {
                var result = new ApiResponse();
                try
                {
                    result.Status.Success = service.OrderMenu(recordId,targetId,newOrderIndex);
                }
                catch (Exception ex)
                {
                    result.Status.Success = false;
                    result.Status.Message = ex.Message;
                }
                return result;
            });

            if (saveResult == null) return new EmptyResult();
            else return Ok(saveResult);
        }

        [HttpGet]
        [Route("treelist/{ApplicationModuleId}")]
        public async Task<IActionResult> ListDataBycategoryId(string ApplicationModuleId)
        {
            var service = new ApplicationMenuService(AppUserData);
            var detailResult = await Task.Run<ApiResponse>(() =>
            {
                var result = new ApiResponse();
                var Data = service.GetDetailMenuByModuleId(ApplicationModuleId);
                try
                {
                    if (Data != null)
                    {
                        result.Status.Success = true;
                        result.Data = Data;
                    }
                    else
                    {
                        result.Status.Message = "Record not found";
                    }
                }
                catch (Exception ex)
                {
                    result.Status.Message = ex.Message;
                }
                return result;
            });
            if (detailResult == null) return new EmptyResult();
            else return Ok(detailResult);
        }

        [HttpGet]
        [Route("{Id}/details")]
        public async Task<IActionResult> Details(string Id)
        {
            var service = new Menu(AppUserData);
            var detailResult = await Task.Run<ApiResponse>(() =>
            {
                var result = new ApiResponse();
                var record = service.Details(Id);
                try
                {
                    if (record != null)
                    {
                        result.Status.Success = true;
                        result.Data = record;
                    }
                    else
                    {
                        result.Status.Message = "Record not found";
                    }
                }
                catch (Exception ex)
                {
                    result.Status.Message = ex.Message;
                }
                return result;
            });
            if (detailResult == null) return new EmptyResult();
            else return Ok(detailResult);
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromBody] string[] Ids)
        {
            var service = new ApplicationMenuService(AppUserData);

            var detailResult = await Task.Run<ApiResponse>(() =>
            {
                var result = new ApiResponse();
                service.Delete(Ids, msg => result.Status.Message = msg);
                result.Status.Success = true;
                return result;
            });
            if (detailResult == null) return new EmptyResult();
            else return Ok(detailResult);
        }
    }
}