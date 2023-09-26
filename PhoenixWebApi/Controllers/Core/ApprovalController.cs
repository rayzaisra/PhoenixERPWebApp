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
using DataAccess.Dto.Custom.General;

namespace PhoenixWebApi.Controllers.Core
{
    [Route("api/core/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = @"Core")]
    public class ApprovalController : ApiBaseController
    {
        [HttpPost]
        [Route("setApprovalByTemplate/{EntityId}")]
        public async Task<IActionResult> SetApprovalByTemplate(String EntityId, [FromQuery(Name = "RecordID")] String RecordId,
            [FromQuery(Name = "ApprovalTemplateId")] String ApprovalTemplateId)
        {
            var service = new Approval(AppUserData);
            var saveResult = await Task.Run<ApiResponse>(() =>
            {
                var result = new ApiResponse();
                try
                {
                    result.Status.Success = service.SetApprovalByTemplate(RecordId, EntityId);
                    if (result.Status.Success)
                    {
                        result.Status.Message = "Approval has been set";
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


        [HttpDelete]
        [Route("removeApprovalEntity/{EntityId}")]
        public async Task<IActionResult> DeleteApprovalEntity(String EntityId,
            [FromQuery(Name = "RecordID")] String RecordId, [FromQuery(Name = "ApprovalID")] String ApprovalId)
        {
            var service = new Approval(AppUserData);

            var detailResult = await Task.Run<ApiResponse>(() =>
            {
                var result = new ApiResponse();
                try
                {
                    result.Status.Success = service.removeApprovalEntity(RecordId, ApprovalId, EntityId);
                    if (result.Status.Success)
                    {
                        result.Status.Message = "Approval has been removed";
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
        [Route("GetApprovalStatus")]
        public async Task<IActionResult> GetApprovalStatus([FromBody] List<string> ApprovalIds)
        {
            var service = new Approval(AppUserData);

            var statusResult = await Task.Run<ApiResponse>(() =>
            {
                var result = new ApiResponse();
                appLogger.Debug(Newtonsoft.Json.JsonConvert.SerializeObject(ApprovalIds));
                try
                {
                    result.Data = service.GetApprovalStatus(ApprovalIds);
                    result.Status.Success = true;
                }
                catch (Exception ex)
                {
                    result.Status.Message = ex.Message;
                }
                return result;
            });

            if (statusResult == null) return new EmptyResult();
            else return Ok(statusResult);
        }

        [HttpPost]
        [Route("GetApprovalStatusGroupLevel")]
        public async Task<IActionResult> GetApprovalStatusGroupLevel([FromBody] List<string> ApprovalIds)
        {
            var service = new Approval(AppUserData);

            var statusResult = await Task.Run<ApiResponse>(() =>
            {
                var result = new ApiResponse();
                appLogger.Debug(Newtonsoft.Json.JsonConvert.SerializeObject(ApprovalIds));
                try
                {
                    result.Data = service.GetApprovalStatusGroupLevel(ApprovalIds);
                    result.Status.Success = true;
                }
                catch (Exception ex)
                {
                    result.Status.Message = ex.Message;
                }
                return result;
            });

            if (statusResult == null) return new EmptyResult();
            else return Ok(statusResult);
        }

        [HttpPost]
        [Route("SetApprovalStatus/{ApprovalDetailId}")]
        public async Task<IActionResult> SetApprovalStatus(String ApprovalDetailId, [FromQuery(Name = "ApprovalStatus")] String ApprovalStatus)
        {
            var service = new Approval(AppUserData);
            var statusResult = await Task.Run<ApiResponse>(() =>
            {
                var result = new ApiResponse();
                try
                {
                    result.Status.Success = service.SetApprovalStatus(ApprovalStatus, ApprovalDetailId);

                    if (!result.Status.Success)
                    {
                        result.Status.Message = $"Record has been {ApprovalStatus.ToUpper()}";
                    }

                }
                catch (Exception ex)
                {
                    result.Status.Message = ex.Message;
                }
                return result;
            });

            if (statusResult == null) return new EmptyResult();
            else return Ok(statusResult);
        }

        //[HttpPost]
        //[Route("WaitingApprovals/listdatatables")]
        //public async Task<IActionResult> DataGridTable([FromForm] DgRequest dgRequest, [FromQuery(Name = "entity_id")] String entityId)
        //{
        //    var Service = new Approval(AppUserData);
        //    var listResult = await Task.Run<DataTablesResponseNetCore>(() =>
        //    {
        //        return Service.GetDataApprovalDetail(dgRequest, entityId);
        //    });
        //    if (listResult == null) return new EmptyResult();
        //    else return Ok(listResult);
        //}




    }
}