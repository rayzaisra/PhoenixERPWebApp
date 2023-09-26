using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Services;
using DataTables.AspNet.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using PhoenixWebApi.Base;
using DataAccess.PhoenixERP.Finance;
using CodeMarvel.Infrastructure.ModelShared;
using Newtonsoft.Json;
using Phoenix.Shared.Core.Areas.General.Dtos;
using Microsoft.Extensions.Options;
using BusinessLogic.Core;

namespace PhoenixWebApi.Areas.Finance.Controllers
{
    [Route("api/finance/[controller]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = @"Finance")]
    [ApiController]
    public class PettyCashController : ApiBaseController
    {
        private readonly FileConfig _fileconfig;

        public PettyCashController(IOptions<FileConfig> settings)
        {
            _fileconfig = settings.Value;
        }

        [HttpPost]
        [Route("listdatatables")]
        public async Task<IActionResult> ListDataTablesBinder([FromForm,
            ModelBinder(typeof(DataTableModelBinder))] DataTableRequestNetCore dtRequest)
        {
            var svc = new PettyCash(AppUserData, _fileconfig);

            var listResult = await Task.Run<DataTablesResponseNetCore>(() =>
            {
                return svc.GetListDataTables(dtRequest);
            });

            if (listResult == null) return new EmptyResult();
            else return Ok(listResult);
        }

        [HttpGet]
        [Route("{Id}/details")]
        public async Task<IActionResult> Details(string Id)
        {
            var svc = new PettyCash(AppUserData, _fileconfig);

            var detailResult = await Task.Run<ApiResponse>(() =>
            {
                var result = new ApiResponse();
                var record = svc.Details(Id);

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
            var svc = new PettyCash(AppUserData, _fileconfig);
            appLogger.Debug("Delete " + JsonConvert.SerializeObject(Ids));

            var detailResult = await Task.Run<ApiResponse>(() =>
            {
                var result = new ApiResponse();

                svc.Delete(Ids, msg => result.Status.Message = msg);
                result.Status.Success = true;

                return result;
            });

            if (detailResult == null) return new EmptyResult();
            else return Ok(detailResult);
        }


        [HttpPost]
        [Route("save")]
        public async Task<IActionResult> Save([FromBody] petty_cashDTO Data)
        {
            var svc = new PettyCash(AppUserData, _fileconfig);
            
            var saveResult = await Task.Run<ApiResponse>(() =>
            {
                bool isNew = false;
                var result = new ApiResponse();

                try
                {
                    result.Status.Success = svc.SaveEntity(Data, ref isNew);

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

        [HttpPost]
        [Route("KendoGrid")]
        public async Task<IActionResult> KendoGrid(CommonTool.KendoUI.Grid.DataRequest dtRequest)
        {
            var Service = new PettyCash(AppUserData,_fileconfig);
            var listResult = await Task.Run<CommonTool.KendoUI.Grid.DataResponse>(() =>
            {
                return Service.GetListKendoUIGrid(dtRequest);
            });
            if (listResult == null) return new EmptyResult();
            else return Ok(listResult);
        }

        [HttpPost]
        [Route("savewithfiles")]
        public async Task<IActionResult> SaveEntity([FromForm] PettyCashDetailsDTO Data)
        {
            var svc = new PettyCash(AppUserData, _fileconfig);

            var files = Request.Form.Files;

            var saveResult = await Task.Run<ApiResponse>( () =>
            {
                bool isNew = false;
                var result = new ApiResponse();

                try
                {
                    result.Status.Success = svc.SavePettyCash(Data, files,ref isNew);

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
                            recordID = Data.PettyCash.id
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
           [FromQuery(Name = "filters")] string filters = null,
           [FromQuery(Name = "parameters")] string parameters = null)
        {
            var svc = new PettyCash(AppUserData, _fileconfig);

            var listResult = await Task.Run<dynamic>(() =>
            {
                dynamic result = null;

                result = svc.Lookup(length: length, optionalFilters: filters, optionalParameters: parameters);

                return result;
            });

            if (listResult == null) return new EmptyResult();
            else return Ok(listResult);
        }
    }
}
