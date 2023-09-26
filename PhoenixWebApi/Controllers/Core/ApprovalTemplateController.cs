using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Core;
using BusinessLogic.Services;
using CodeMarvel.Infrastructure.ModelShared;
using CommonTool.JEasyUI.DataGrid;
using DataAccess.PhoenixERP.General;
using DataTables.AspNet.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhoenixWebApi.Base;

namespace PhoenixWebApi.Controllers.Core
{
    [Route("api/core/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = @"Core")]
    public class ApprovalTemplateController : ApiBaseController
    {

        [HttpGet]
        [Route("{Id}/details")]
        public async Task<IActionResult> Details(string Id)
        {
            var service = new ApprovalTemplate(AppUserData);
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
        [Route("deleteDetails")]
        public async Task<IActionResult> DeleteDetails([FromBody] string[] Ids)
        {
            var service = new ApprovalTemplateDetail(AppUserData);

            var detailResult = await Task.Run<ApiResponse>(() =>
            {
                var result = new ApiResponse();
                result.Status.Success = service.Delete(Ids, msg => result.Status.Message = msg);
                return result;
            });
            if (detailResult == null) return new EmptyResult();
            else return Ok(detailResult);
        }


        [HttpGet]
        [Route("{Id}/templates")]
        public async Task<IActionResult> GetTemplates(string Id)
        {
            var service = new ApprovalTemplate(AppUserData);
            var detailResult = await Task.Run<ApiResponse>(() =>
            {
                var result = new ApiResponse();
                var record = service.GetTemplates(Id);
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

        [HttpGet]
        [Route("{Id}/templates/datasource")]
        public async Task<IActionResult> GetTemplatesDatasource(string Id)
        {
            var service = new ApprovalTemplate(AppUserData);
            var detailResult = await Task.Run<ApiResponse>(() =>
            {
                var result = new ApiResponse();
                var record = service.GetTemplatesDatasource(Id);
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

        [HttpGet]
        [Route("{Id}/templates/connector")]
        public async Task<IActionResult> GetTemplatesConnector(string Id)
        {
            var service = new ApprovalTemplate(AppUserData);
            var detailResult = await Task.Run<ApiResponse>(() =>
            {
                var result = new ApiResponse();
                var record = service.GetTemplatesConnector(Id);
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


        [HttpPost]
        [Route("saveTemplate")]
        public async Task<IActionResult> SaveTemplate([FromBody] approval_template_setupDTO Data)
        {
            var service = new ApprovalTemplate(AppUserData);
            var saveResult = await Task.Run<ApiResponse>(() =>
            {
                bool isNew = false;
                var result = new ApiResponse();
                try
                {
                    result.Status.Success = service.SaveApproval(Data, ref isNew);
                    result.Data = Data;
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
        public async Task<IActionResult> GetDataKendoGridBinder([FromBody] CommonTool.KendoUI.Grid.DataRequest dtRequest)
        {
            var Service = new ApprovalTemplate(AppUserData);
            appLogger.Debug(Newtonsoft.Json.JsonConvert.SerializeObject(dtRequest));
            var listResult = await Task.Run<CommonTool.KendoUI.Grid.DataResponse>(() =>
            {
                return Service.GetListKendoUIGrid(dtRequest);
            });
            if (listResult == null) return new EmptyResult();
            else return Ok(listResult);
        }


    }
}