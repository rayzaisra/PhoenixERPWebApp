using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Core;
using BusinessLogic.Services;
using CodeMarvel.Infrastructure.ModelShared;
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
    public class UserRoleController : ApiBaseController
    {
        [HttpPost]
        [Route("save")]
        public async Task<IActionResult> SaveEntity([FromBody] user_roleDTO Data)
        {
            var service = new UserRole(AppUserData);
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

        [HttpPost]
        [Route("{roleId}/listdatatables")]
        public async Task<IActionResult> ListDataTablesBinder(string roleId,
            [FromForm, ModelBinder(typeof(DataTableModelBinder))] DataTables.AspNet.AspNetCore.DataTableRequestNetCore dtRequest)
        {
            var Service = new UserRole(AppUserData);
            var listResult = await Task.Run<DataTablesResponseNetCore>(() =>
            {
                return Service.GetListUserRoleByRoleId(dtRequest, roleId);
            });
            if (listResult == null) return new EmptyResult();
            else return Ok(listResult);
        }

        [HttpPost]
        [Route("{roleId}/listdatatablesavailableassignusermember")]
        public async Task<IActionResult> ListDataTablesAvailableAssignUserMember(string roleId,
       [FromForm, ModelBinder(typeof(DataTableModelBinder))] DataTables.AspNet.AspNetCore.DataTableRequestNetCore dtRequest)
        {
            var Service = new UserRole(AppUserData);
            var listResult = await Task.Run<DataTablesResponseNetCore>(() =>
            {
                return Service.GetAssignableUserMember(dtRequest, roleId);
            });
            if (listResult == null) return new EmptyResult();
            else return Ok(listResult);
        }


        [HttpPost]
        [Route("kendoLookupAssignableUserMember")]
        public async Task<IActionResult> GetkendoLookupAssignableUserMemberBinder([FromQuery] string roleId, 
            [FromBody] CommonTool.KendoUI.Combobox.ComboboxDataRequest dtRequest)
        {
            var Service = new UserRole(AppUserData);
            var listResult = await Task.Run<dynamic>(() =>
            {
                dynamic result = null;
                result = Service.kendoLookupAssignableUserMember(dtRequest,roleId);
                return result;
            });
            if (listResult == null) return new EmptyResult();
            else return Ok(listResult);
        }


        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromBody] string[] Ids)
        {
            var service = new UserRole(AppUserData);

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

        [HttpPost]
        [Route("addmember")]
        public async Task<IActionResult> AddMember([FromQuery(Name ="roleId")]String RoleId, [FromBody] string[] Ids)
        {
            var service = new UserRole(AppUserData);
            var detailResult = await Task.Run<ApiResponse>(() =>
            {
                var result = new ApiResponse();
                bool isComplete = false;
                var data = service.AssignMember(Ids, RoleId, ref isComplete);
                result.Status.Success = true;
                result.Data = data;
                return result;
            });
            if (detailResult == null) return new EmptyResult();
            else return Ok(detailResult);
        }

        [HttpPost]
        [Route("deleteMember")]
        public async Task<IActionResult> DeleteMember([FromQuery(Name = "roleId")]String RoleId, [FromBody] string[] Ids)
        {
            var service = new UserRole(AppUserData);
            var detailResult = await Task.Run<ApiResponse>(() =>
            {
                var result = new ApiResponse();
                bool isComplete = false;
                var data = service.DeleteMember(Ids, RoleId, ref isComplete);
                result.Status.Success = true;
                result.Data = data;
                return result;
            });
            if (detailResult == null) return new EmptyResult();
            else return Ok(detailResult);
        }

    }
}