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
using Omu.ValueInjecter;
using DataAccess.PhoenixERP.General;

namespace PhoenixWebApi.Controllers.Core
{
    [ApiController]
    [Route("api/core/[controller]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = @"Core")]
    public class RoleAccessController : ApiBaseController
    {
        [HttpPost]
        [Route("registerEntity")]
        public async Task<IActionResult> RegisterEntity([FromQuery(Name = "roleId")] String AppRoleId)
        {
            var service = new RoleAccess(AppUserData);

            var detailResult = await Task.Run<ApiResponse>(() =>
            {
                var result = new ApiResponse();
                try
                {
                    result.Status.Success = service.RegisterRoleAccessEntity(AppRoleId);
                }
                catch (Exception ex)
                {
                    result.Status.Success = false;
                    result.Status.Message = ex.Message;
                    appLogger.Error(ex);
                }
                return result;
            });
            if (detailResult == null) return new EmptyResult();
            else return Ok(detailResult);
        }

        [HttpPost]
        [Route("save")]
        public async Task<IActionResult> SaveEntity([FromBody]role_accessDTO Data)
        {
            var service = new RoleAccess(AppUserData);
            var saveResult = await Task.Run<ApiResponse>(() =>
            {

                bool isNew = false;
                var result = new ApiResponse();
                try
                {
                    result.Status.Success = service.UpdateRoleAccessEntity(Data, ref isNew);
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
        [Route("{roleId}/EntityRoleKendoGrid")]
        public async Task<IActionResult> GetDataEntityRoleKendoGridBinder(String roleId, [FromBody] CommonTool.KendoUI.Grid.DataRequest dtRequest)
        {
            var Service = new RoleAccess(AppUserData);
            var listResult = await Task.Run<CommonTool.KendoUI.Grid.DataResponse>(() =>
            {
                return Service.GetEntityRoleListKendoUIGrid(dtRequest, roleId);
            });
            if (listResult == null) return new EmptyResult();
            else return Ok(listResult);
        }

        [HttpPost]
        [Route("{approleId}/listdatatables")]
        public async Task<IActionResult> ListDataTablesBinder(string approleId,
           [FromForm, ModelBinder(typeof(DataTableModelBinder))] DataTables.AspNet.AspNetCore.DataTableRequestNetCore dtRequest)
        {
            var Service = new RoleAccess(AppUserData);
            var listResult = await Task.Run<DataTablesResponseNetCore>(() =>
            {
                return Service.GetListRoleAccessByRoleId(dtRequest, approleId);
            });
            if (listResult == null) return new EmptyResult();
            else return Ok(listResult);
        }

        [HttpPost]
        [Route("{roleId}/SyncronizeEntity")]
        public async Task<IActionResult> SyncronizeEntityByRole(string roleId)
        {
            var Service = new RoleAccess(AppUserData);
            var listResult = await Task.Run<ApiResponse>(() =>
            {
                var result = new ApiResponse();
                result.Status.Success = Service.SyncronizeEntity(roleId);
                return result;
            });
            if (listResult == null) return new EmptyResult();
            else return Ok(listResult);
        }


    }
}