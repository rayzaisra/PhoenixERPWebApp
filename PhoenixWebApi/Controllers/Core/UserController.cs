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
using Omu.ValueInjecter;
using BusinessLogic.Services;
using BusinessLogic.Services.General;
using DataAccess.PhoenixERP.General;
using CommonTool.KendoUI.Grid;

namespace PhoenixWebApi.Controllers.Core
{
    [ApiController]
    [Route("api/core/[controller]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = @"Core")]
    public class UserController : ApiBaseController
    {
        [HttpGet]
        [Route("{Id}/imageprofile")]//{Thumbnail:bool=true}
        public async Task<IActionResult> ImageProfile(string Id, [FromQuery(Name = "thumbnail")] bool Thumbnail = true)
        {
            string mimeType = "image/jpeg";

            var result = await Task.Run<byte[]>(() =>
            {
                var service = new ApplicationUserService(AppUserData);
                var base64Img = service.GetImageProfile(Id, Thumbnail, ref mimeType);
                if (!string.IsNullOrEmpty(base64Img))
                {
                    return Convert.FromBase64String(base64Img);
                }

                return null;
            });

            if (result == null) return new EmptyResult();
            else return File(result, mimeType);
        }

        [HttpGet]
        [Route("{Id}/details")]
        public async Task<IActionResult> Details(string Id)
        {
            var service = new ApplicationUserService(AppUserData);

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
            var service = new ApplicationUserService(AppUserData);

            var detailResult = await Task.Run<ApiResponse>(() =>
            {
                var result = new ApiResponse();
                service.Delete(Ids,msg=> result.Status.Message = msg);
                result.Status.Success = true;
                return result;
            });
            if (detailResult == null) return new EmptyResult();
            else return Ok(detailResult);
        }


        [HttpPost]
        [Route("kendoGrid")]
        public async Task<IActionResult> GetDataKendoGridBinder(DataRequest dtRequest)
        {
            var Service = new ApplicationUserService(AppUserData);
            var listResult = await Task.Run<DataResponse>(() =>
            {
                return Service.GetListKendoGrid(dtRequest);
            });
            if (listResult == null) return new EmptyResult();
            else return Ok(listResult);
        }




        [HttpPost]
        [Route("listdatatables")]
        public async Task<IActionResult> ListDataTablesBinder([FromForm, ModelBinder(typeof(DataTableModelBinder))] DataTables.AspNet.AspNetCore.DataTableRequestNetCore dtRequest)
        {
            var service = new ApplicationUserService(AppUserData);

            var listResult = await Task.Run<DataTablesResponseNetCore>(() =>
            {
                return service.GetListDataTables(dtRequest);
            });
            if (listResult == null) return new EmptyResult();
            else return Ok(listResult);
        }

        //EntityBinderProviders
        [HttpPost]
        [Route("bind")]
        public async Task<IActionResult> testbind(application_userDTO Data)
        {
            var service = new ApplicationUserService(AppUserData);

            var saveResult = await Task.Run<ApiResponse>(() =>
            {
                var result = new ApiResponse();
                try
                {
                    result.Data = Data;
                    result.Status.Success = false;

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

        [HttpPost]
        [Route("save")]
        public async Task<IActionResult> SaveEntity([FromBody] application_userDTO Data)
        {
            var service = new ApplicationUserService(AppUserData);
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
        [Route("lookup")]
        public async Task<IActionResult> Lookup([FromQuery(Name = "length")] int length = 100,
                  [FromQuery(Name = "filters")] String filters = null, [FromQuery(Name = "parameters")] String parameters = null)
        {
            var Service = new ApplicationUser(AppUserData);
            var listResult = await Task.Run<dynamic>(() =>
            {
                dynamic result = null;
                result = Service.Lookup(Length: length, OptionalFilters: filters, OptionalParameters: parameters);
                return result;
            });
            if (listResult == null) return new EmptyResult();
            else return Ok(listResult);
        }

    }
}