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


namespace PhoenixWebApi.Controllers.Core
{


    [ApiController]
    [Route("api/core/[controller]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = @"Core")]
    public class MenuCategoryController : ApiBaseController
    {
        [HttpPost]
        [Route("save")]
        public async Task<IActionResult> SaveEntity([FromBody] application_menu_categoryDTO Data)
        {
            var service = new ApplicationMenuCategory(AppUserData);
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
            var Service = new ApplicationMenuCategory(AppUserData);
            var listResult = await Task.Run<dynamic>(() =>
            {
                dynamic result = null;
                appLogger.Debug("MASUK LOOKUP");
                result = Service.Lookup(Length: length, OptionalFilters: filters, OptionalParameters: parameters);
                return result;
            });
            if (listResult == null) return new EmptyResult();
            else return Ok(listResult);
        }

        [HttpPost]
        [Route("listdatatables")]
        public async Task<IActionResult> ListDataTablesBinder([FromForm, ModelBinder(typeof(DataTableModelBinder))] DataTables.AspNet.AspNetCore.DataTableRequestNetCore dtRequest)
        {
            var Service = new ApplicationMenuCategoryService(AppUserData);
            var listResult = await Task.Run<DataTablesResponseNetCore>(() =>
            {
                return Service.GetListDataTables(dtRequest);
            });
            if (listResult == null) return new EmptyResult();
            else return Ok(listResult);
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromBody] string[] Ids)
        {
            var service = new ApplicationMenuCategoryService(AppUserData);

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

        [HttpGet]
        [Route("{Id}/details")]
        public async Task<IActionResult> Details(string Id)
        {
            var service = new ApplicationMenuCategory(AppUserData);
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
    }
}