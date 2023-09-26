using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Phoenix.Data.Models.Um;
using Phoenix.Data.Models.ViewModel;
using Phoenix.Data.Services.Um;
using Phoenix.Data.RestApi;
using System.Collections.Generic;
using System.Linq;
using Phoenix.Data.Attributes;

namespace PhoenixWebApi.Controllers.Core
{
    /// <summary>
    /// Controller Transaksi Approval.....
    /// </summary>
    [AllowAnonymousRoleAttribute]
    [Route("api/ManageTransApproval")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = @"Um")]
    public class ManageTrApprovalController : Controller
    {
        private readonly IManageTransactionApprovalService _repo;
        private readonly IManageTemplateApprovalService _repoTemplate;
        private readonly ITransactionUserApprovalService _repoManageUserApproval;
        private readonly ITransactionConditionalApprovalService _repoManageConditionApproval;
        private readonly IDoaService _repoDoa;
        private readonly IManageUserAppService _repoUserApp;
        public ManageTrApprovalController(IManageTransactionApprovalService repo, ITransactionUserApprovalService repoManageUserApproval, ITransactionConditionalApprovalService repoManageConditionApproval,
            IManageTemplateApprovalService repoTemplate, IDoaService repoDoa, IManageUserAppService repoUserApp)
        {
            _repo = repo;
            _repoManageUserApproval = repoManageUserApproval;
            _repoManageConditionApproval = repoManageConditionApproval;
            _repoTemplate = repoTemplate;
            _repoDoa = repoDoa;
            _repoUserApp = repoUserApp;
        }

        /// <summary>
        /// Kebutuhan untuk detail form request....
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymousRole]
        [HttpGet]
        [Route("GetByRefId/{id?}")]
        public async Task<IActionResult> GetByRefId(string id)
        {
            return Ok(await _repo.GetByRefId(id).ConfigureAwait(false));
        }

        /// <summary>
        /// Kebutuhan untuk detail form request....
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymousRole]
        [Route("GetListApprovalByRefId/{id?}")]
        public async Task<IActionResult> GetListApprovalByRefId(string id)
        {
            return Ok(await _repo.GetListApprovalByRefId(id).ConfigureAwait(false));
        }

        /// <summary>
        /// Kebutuhan untuk detail form request....
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymousRole]
        [Route("IsCurrentApproval/{id?}")]
        public IActionResult IsCurrentApproval(string id)
        {
            return Ok(_repo.IsCurrentApproval(id));
        }

        /// <summary>
        /// Untuk Dashboard Approval all by EMployee Id
        /// </summary>
        /// <returns></returns>
        [AllowAnonymousRole]
        [HttpGet]
        [Route("GetListApprovalByEmpId")]
        public async Task<IActionResult> GetListApprovalByEmpId()
        {
            var empId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(HttpContext);
            return Ok(await _repo.GetListApprovalByEmpId(empId).ConfigureAwait(false));
        }

        /// <summary>
        /// kebutuhan untuk pilih template di form request..
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymousRole]
        [HttpGet]
        [Route("GetByCategory/{id?}")]
        public async Task<IActionResult> GetByCategory(int id)
        {
            return Ok(await _repo.GetByCategory(id).ConfigureAwait(false));
        }

        /// <summary>
        /// kebutuhan untuk pilih template di form request..
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymousRole]
        [HttpGet]
        [Route("SendEmail")]
        public async Task<IActionResult> SendEmail([FromBody]EmailViewModel model)
        {
            return Ok(await _repoTemplate.SendEmail(model.MailTo, model.MailFrom, model.MailSubject, model.MailBody).ConfigureAwait(false));
        }

        [AllowAnonymousRole]
        [HttpPost]
        [Route("SendEmailInvitation")]
        public async Task<IActionResult> SendEmailInvitation([FromBody] EmailViewModel model)
        {
            return Ok(await _repoTemplate.SendEmail(model.MailTo, model.MailFrom, model.MailSubject, model.MailBody).ConfigureAwait(false));
        }
        /// <summary>
        /// Save Template Approval ketika Request untuk Module HRIS..
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [AllowAnonymousRole]
        [HttpPost]
        [Route("SavePm")]
        public async Task<IActionResult> SavePm([FromBody]TransApprovalHrisVm dataVm)
        {
            if (!ModelState.IsValid)
            {
                await _repo.DeleteRefTable(dataVm.Tname, dataVm.RefId).ConfigureAwait(false);
                return BadRequest(new ApiResponse(false, "data not valid"));
            }
            try
            {

                var subGroupId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessSubGroup(this.HttpContext);
                var divisionId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessUnitDivisi(this.HttpContext);
                var jobTitleId = "0";

                var data = await _repoTemplate.GetByMenuId(subGroupId, divisionId, dataVm.MenuId, jobTitleId).ConfigureAwait(false);

                var model = new TrTemplateApproval();
                model.TemplateName = data.TemplateName;
                model.IsNotifyByWeb = data.IsNotifyByWeb;
                model.IsNotifyByEmail = data.IsNotifyByEmail;
                model.DueDate = data.DueDate;
                model.DetailLink = dataVm.DetailLink;
                model.StatusApproved = StatusApproved.WaitingApproval;
                model.Reminder = data.Reminder;
                model.ApprovalCategory = data.ApprovalCategory;
                model.ApprovalType = data.ApprovalType;
                model.FormReqName = data.TemplateName;
                model.CreatedOn = DateTime.Now;
                model.CreatedBy = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(this.HttpContext);
                model.RefId = dataVm.RefId;
                model.ForDivisionId = data.ForDivisionId;
                var tempId = Guid.NewGuid().ToString();
                model.Id = tempId;
                await _repo.AddAsync(model).ConfigureAwait(false);

                var isFirst = 0;
                foreach (var item in data.ListUserApprovals)
                {
                    var vmUser = new TrUserApproval();
                    vmUser.EmployeeBasicInfoId = item.EmployeeBasicInfoId;
                    vmUser.IndexUser = item.IndexUser;
                    vmUser.IsCondition = item.IsCondition;
                    vmUser.TrTempApprovalId = tempId;
                    vmUser.NotificationTypeId = dataVm.NotificationType;
                    vmUser.Module = dataVm.Module;
                    vmUser.Id = Guid.NewGuid().ToString();
                    if (isFirst == 0) vmUser.StatusApproved = StatusApproved.CurrentApproval;
                    else vmUser.StatusApproved = StatusApproved.WaitingApproval;
                    await _repoManageUserApproval.AddAsync(vmUser).ConfigureAwait(false);

                    //looping data conditional user approval.....
                    foreach (var con in item.ListConditionalApprovals)
                    {
                        var modelCon = new TrConditionUserApproval();
                        modelCon.Id = Guid.NewGuid().ToString();
                        modelCon.EmployeeBasicInfoId = con.EmployeeBasicInfoId;
                        modelCon.GroupConditionIndex = con.GroupConditionIndex;
                        modelCon.GroupConditionApproval = con.GroupConditionApproval;
                        modelCon.ConditionalApproval = con.ConditionalApproval;
                        if (isFirst == 0) modelCon.StatusApproved = StatusApproved.CurrentApproval;
                        else modelCon.StatusApproved = StatusApproved.WaitingApproval;
                        modelCon.TrUserApprovalId = vmUser.Id;
                        modelCon.CreatedOn = DateTime.Now;
                        modelCon.CreatedBy = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(this.HttpContext);
                        await _repoManageConditionApproval.AddAsync(modelCon).ConfigureAwait(false);
                    }
                    isFirst++;
                }
                var vm = new
                {
                    IdTemp = tempId
                };
                return Ok(new ApiResponse(vm, true, Messages.SuccessAdd));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _repo.DeleteRefTable(dataVm.Tname, dataVm.RefId).ConfigureAwait(false);
                return BadRequest(new ApiResponse(false, "Setup Approval getting error, please contact adminsitrator system"));
            }
        }

        /// <summary>
        /// Save Template Approval ketika Request untuk Module HRIS..
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [AllowAnonymousRole]
        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> Save([FromBody]TransApprovalHrisVm dataVm)
        {
            if (!ModelState.IsValid)
            {
                await _repo.DeleteRefTable(dataVm.Tname, dataVm.RefId).ConfigureAwait(false);
                return BadRequest(new ApiResponse(false, "data not valid"));
            }
            try
            {

                var subGroupId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessSubGroup(this.HttpContext);
                var divisionId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessUnitDivisi(this.HttpContext);
                //var jobTitleId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetJobTitleId(this.HttpContext);
                var jobTitleId = "0";
                var data = new TmTemplateApproval();
                if (!string.IsNullOrEmpty(dataVm.IdTemplate))
                {
                    data = await _repoTemplate.GetById(dataVm.IdTemplate).ConfigureAwait(false);
                }
                else
                {
                    data = await _repoTemplate.GetByMenuId(subGroupId, divisionId, dataVm.MenuId, jobTitleId).ConfigureAwait(false);
                }


                var model = new TrTemplateApproval();
                model.TemplateName = data.TemplateName;
                model.IsNotifyByWeb = data.IsNotifyByWeb;
                model.IsNotifyByEmail = data.IsNotifyByEmail;
                model.DueDate = data.DueDate;
                model.DetailLink = dataVm.DetailLink;
                model.StatusApproved = StatusApproved.WaitingApproval;
                model.Reminder = data.Reminder;
                model.ApprovalCategory = data.ApprovalCategory;
                model.ApprovalType = data.ApprovalType;
                model.FormReqName = data.TemplateName;
                model.CreatedOn = DateTime.Now;
                model.CreatedBy = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(this.HttpContext);
                model.RefId = dataVm.RefId;
                model.ForDivisionId = data.ForDivisionId;
                var tempId = Guid.NewGuid().ToString();
                model.Id = tempId;
                await _repo.AddAsync(model).ConfigureAwait(false);

                var isFirst = 0;
                foreach (var item in data.ListUserApprovals)
                {
                    var vmUser = new TrUserApproval();
                    vmUser.EmployeeBasicInfoId = item.EmployeeBasicInfoId;
                    vmUser.IndexUser = item.IndexUser;
                    vmUser.IsCondition = item.IsCondition;
                    vmUser.TrTempApprovalId = tempId;
                    vmUser.NotificationTypeId = dataVm.NotificationType;
                    vmUser.Module = dataVm.Module;
                    vmUser.Id = Guid.NewGuid().ToString();
                    if (isFirst == 0) {
                        vmUser.StatusApproved = StatusApproved.CurrentApproval;

                        if (!string.IsNullOrEmpty(dataVm.NotificationType) && !string.IsNullOrEmpty(dataVm.Module))
                        {
                            await _repoTemplate.AddNotification(dataVm.NotificationType, item.Email, dataVm.Module, dataVm.RefId, data.IsNotifyByWeb, data.IsNotifyByEmail, model.CreatedBy, dataVm.DetailLink);
                        }
                    }
                    else vmUser.StatusApproved = StatusApproved.WaitingApproval;
                    await _repoManageUserApproval.AddAsync(vmUser).ConfigureAwait(false);

                    //looping data conditional user approval.....
                    foreach (var con in item.ListConditionalApprovals)
                    {
                        var modelCon = new TrConditionUserApproval();
                        modelCon.Id = Guid.NewGuid().ToString();
                        modelCon.EmployeeBasicInfoId = con.EmployeeBasicInfoId;
                        modelCon.GroupConditionIndex = con.GroupConditionIndex;
                        modelCon.GroupConditionApproval = con.GroupConditionApproval;
                        modelCon.ConditionalApproval = con.ConditionalApproval;
                        if (isFirst == 0) modelCon.StatusApproved = StatusApproved.CurrentApproval;
                        else modelCon.StatusApproved = StatusApproved.WaitingApproval;
                        modelCon.TrUserApprovalId = vmUser.Id;
                        modelCon.CreatedOn = DateTime.Now;
                        modelCon.CreatedBy = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(this.HttpContext);
                        await _repoManageConditionApproval.AddAsync(modelCon).ConfigureAwait(false);
                    }
                    isFirst++;
                }
                var vm = new
                {
                    IdTemp = tempId
                };

                

                return Ok(new ApiResponse(vm, true, Messages.SuccessAdd));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _repo.DeleteRefTable(dataVm.Tname, dataVm.RefId).ConfigureAwait(false);
                return BadRequest(new ApiResponse(false, "Setup Approval getting error, please contact adminsitrator system"));
            }
        }

        /// <summary>
        /// Save Template Approval ketika Request untuk Module HRIS..
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [AllowAnonymousRole]
        [HttpPost]
        [Route("SaveFn")]
        public async Task<IActionResult> SaveFn([FromBody]TransApprovalFnVm dataVm)
        {
            if (!ModelState.IsValid)
            {
                await _repo.DeleteRefTable(dataVm.Tname, dataVm.RefId).ConfigureAwait(false);
                return BadRequest(new ApiResponse(false, "data not valid"));
            }
            try
            {

                var subGroupId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessSubGroup(this.HttpContext);
                var divisionId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessUnitDivisi(this.HttpContext);

                var data = await _repoTemplate.GetByBudgeting(subGroupId, divisionId, dataVm.MenuId, dataVm.Budget).ConfigureAwait(false);

                var model = new TrTemplateApproval();
                model.TemplateName = data.TemplateName;
                model.IsNotifyByWeb = data.IsNotifyByWeb;
                model.IsNotifyByEmail = data.IsNotifyByEmail;
                model.DueDate = data.DueDate;
                model.DetailLink = dataVm.DetailLink;
                model.StatusApproved = StatusApproved.WaitingApproval;
                model.Reminder = data.Reminder;
                model.ApprovalCategory = data.ApprovalCategory;
                model.ApprovalType = data.ApprovalType;
                model.FormReqName = data.TemplateName;
                model.CreatedOn = DateTime.Now;
                model.CreatedBy = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(this.HttpContext);
                model.RefId = dataVm.RefId;
                model.ForDivisionId = data.ForDivisionId;
                var tempId = Guid.NewGuid().ToString();
                model.Id = tempId;
                await _repo.AddAsync(model).ConfigureAwait(false);

                var isFirst = 0;
                foreach (var item in data.ListUserApprovals)
                {
                    var vmUser = new TrUserApproval();
                    vmUser.EmployeeBasicInfoId = item.EmployeeBasicInfoId;
                    vmUser.IndexUser = item.IndexUser;
                    vmUser.IsCondition = item.IsCondition;
                    vmUser.TrTempApprovalId = tempId;
                    vmUser.Id = Guid.NewGuid().ToString();

                    if (isFirst == 0) vmUser.StatusApproved = StatusApproved.CurrentApproval;
                    else vmUser.StatusApproved = StatusApproved.WaitingApproval;
                    await _repoManageUserApproval.AddAsync(vmUser).ConfigureAwait(false);

                    //looping data conditional user approval.....
                    foreach (var con in item.ListConditionalApprovals)
                    {
                        var modelCon = new TrConditionUserApproval();
                        modelCon.Id = Guid.NewGuid().ToString();
                        modelCon.EmployeeBasicInfoId = con.EmployeeBasicInfoId;
                        modelCon.GroupConditionIndex = con.GroupConditionIndex;
                        modelCon.GroupConditionApproval = con.GroupConditionApproval;
                        modelCon.ConditionalApproval = con.ConditionalApproval;
                        if (isFirst == 0) modelCon.StatusApproved = StatusApproved.CurrentApproval;
                        else modelCon.StatusApproved = StatusApproved.WaitingApproval;
                        modelCon.TrUserApprovalId = vmUser.Id;
                        modelCon.CreatedOn = DateTime.Now;
                        modelCon.CreatedBy = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(this.HttpContext);
                        await _repoManageConditionApproval.AddAsync(modelCon).ConfigureAwait(false);
                    }
                    isFirst++;
                }
                var vm = new
                {
                    IdTemp = tempId
                };
                return Ok(new ApiResponse(vm, true, Messages.SuccessAdd));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _repo.DeleteRefTable(dataVm.Tname, dataVm.RefId).ConfigureAwait(false);
                return BadRequest(new ApiResponse(false, "Setup Approval getting error, please contact adminsitrator system"));
            }
        }

        [AllowAnonymousRole]
        [HttpPost]
        [Route("UpdateStatusApproval")]
        public async Task<IActionResult> UpdateStatusApproval([FromBody]TrUpdateStatusApprovalViewModel data)
        {
            if (data.StatusApproved == 4 && string.IsNullOrEmpty(data.Remarks))
            {
                return BadRequest(new ApiResponse(false, "Remarks cannot empty if rejected approval"));
            }
            //1. Cek user/employee yang approve di conditional or di concurrent....
            //--Get Employee Basic Info Id nya..
            var empId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(HttpContext);
           
            //--Get Template Approval nya dulu...
            var dataApproval = await _repo.GetByRefId(data.RefId).ConfigureAwait(false);
            if (dataApproval == null)
            {
                dataApproval = await _repo.GetByRefId(data.Id).ConfigureAwait(false);
                if (dataApproval == null)
                {
                    return BadRequest(new ApiResponse(true, "Data not found"));
                }
            }
            if (dataApproval.StatusApproved == StatusApproved.Approved || dataApproval.StatusApproved == StatusApproved.Rejected)
            {
                return Ok(new ApiResponse(true, "Data Request Is Done From Process Approval, data request status approval is " + dataApproval.StatusApproved));
            }
            var requestempId = dataApproval.CreatedBy;
            var listEmpIdDelegation = CheckUserIsDelegationNonCoditional();
            if (listEmpIdDelegation.Count > 0)
            {
                UpdateDelegation(dataApproval, listEmpIdDelegation,data, empId);
                return Ok(new ApiResponse(true, "Update status approval success"));
            }

            foreach (var item in dataApproval.ListUserApprovals)
            {
                // Cek User ini delegation atau bukan....jika iya, ganti value empId dengan si yang mendelegate...
                // -- 
                if (item.IsCondition == true && item.StatusApproved == StatusApproved.CurrentApproval)
                {
                    // -- Cek di Conditional nya...
                    foreach (var x in item.ListConditionalApprovals)
                    {
                        if (x.EmployeeBasicInfoId == empId)
                        {
                            if (x.ConditionalApproval == "OR")
                            {
                                if (x.GroupConditionApproval == "OR")
                                {
                                    CheckAllOrCondition(empId, data, x.Id, item.Id, dataApproval.Id, dataApproval.ListUserApprovals);
                                    break;
                                }
                                else // kalau kondisi nya AND..
                                {
                                    if (data.StatusApproved == 4) // jika rejected...kelar alias close..
                                    {
                                        //update Table Conditional...
                                        var dataCon = await _repoManageConditionApproval.Get(x.Id).ConfigureAwait(false);
                                        dataCon.StatusApproved = (StatusApproved)Enum.ToObject(typeof(StatusApproved), data.StatusApproved);
                                        dataCon.CreatedBy = empId;
                                        dataCon.CreatedOn = DateTime.Now;
                                        await _repoManageConditionApproval.EditAsync(dataCon);

                                        //update table User APproval...
                                        var dataUserApp = await _repoManageUserApproval.Get(x.TrUserApprovalId).ConfigureAwait(false);
                                        dataUserApp.CreatedBy = empId;
                                        dataUserApp.CreatedOn = DateTime.Now;
                                        dataUserApp.StatusApproved = (StatusApproved)Enum.ToObject(typeof(StatusApproved), data.StatusApproved);
                                        await _repoManageUserApproval.EditAsync(dataUserApp).ConfigureAwait(false);

                                        var dataApp = await _repo.Get(dataApproval.Id);
                                        dataApp.RemarkRejected = data.Remarks;
                                        dataApp.StatusApproved = StatusApproved.Rejected;
                                        dataApp.RejectedBy = empId;
                                        dataApp.RejectedOn = DateTime.Now;
                                        await _repo.EditAsync(dataApp).ConfigureAwait(false);
                                        goto FinishedProcess;
                                    }
                                    else
                                    {
                                        CheckConditionGroupForOrAnd(item.ListConditionalApprovals, empId, data, dataApproval.ListUserApprovals, dataApproval.Id);
                                        goto FinishedProcess;
                                    }
                                }
                            }
                            else // conditional = AND
                            {
                                if (x.GroupConditionApproval == "OR")
                                {
                                    CheckConditionGroupForAndPlusOr(item.ListConditionalApprovals, empId, data, dataApproval.ListUserApprovals);
                                    goto FinishedProcess;
                                }
                                else // Contional Group = AND
                                {
                                    CheckConditionGroupForAndPlusOr(item.ListConditionalApprovals, empId, data, dataApproval.ListUserApprovals);
                                    goto FinishedProcess;
                                }
                            }
                        }
                    }
                }
                else // JIka tidak ada Conditional approval... 
                {
                    if (item.StatusApproved == StatusApproved.CurrentApproval)
                    {
                        if (item.EmployeeBasicInfoId == empId)
                        {
                            if (item.IsSpecialCase == true)
                            {
                                if (string.IsNullOrEmpty(data.Remarks))
                                {
                                    return BadRequest(new ApiResponse(false, "Remarks special case cannot empty"));
                                }
                            }

                            //update table User APproval...
                            var dataUserApp = await _repoManageUserApproval.Get(item.Id).ConfigureAwait(false);
                            //var dataUserApp = await _repoManageUserApproval.GetByTemplateSingleId(item.TrTempApprovalId).ConfigureAwait(false);
                            dataUserApp.CreatedBy = empId;
                            dataUserApp.CreatedOn = DateTime.Now;
                            dataUserApp.StatusApproved = (StatusApproved)Enum.ToObject(typeof(StatusApproved), data.StatusApproved);
                            if (item.IsSpecialCase == true) dataUserApp.RemarkSpecialCase = data.Remarks;
                            await _repoManageUserApproval.EditAsync(dataUserApp).ConfigureAwait(false);

                            if (data.StatusApproved == 4)
                            {
                                //update table tr approval header nya...and request form is Close...
                                // var dataApp = await _repo.Get(dataUserApp.TrTempApprovalId).ConfigureAwait(false);
                                var dataApp = await _repo.Get(dataUserApp.TrTempApprovalId).ConfigureAwait(false);
                                dataApp.RemarkRejected = data.Remarks;
                                dataApp.StatusApproved = StatusApproved.Rejected;
                                dataApp.RejectedBy = empId;
                                dataApp.RejectedOn = DateTime.Now;
                                await _repo.EditAsync(dataApp).ConfigureAwait(false);
                            }
                            else
                            {
                                //baca list user approval nya, masih ada atau nggak ada...
                                var dataUserConCurrentApproval = dataApproval.ListUserApprovals.Where(x =>  x.StatusApproved == StatusApproved.WaitingApproval).ToList();
                                if (dataUserConCurrentApproval.Count == 0)
                                {
                                    // var dataApp = await _repo.Get(dataUserApp.TrTempApprovalId).ConfigureAwait(false);
                                    var dataApp = await _repo.Get(dataUserApp.TrTempApprovalId).ConfigureAwait(false);
                                    dataApp.StatusApproved = StatusApproved.Approved;
                                    if (item.IsSpecialCase == true) dataUserApp.RemarkSpecialCase = data.Remarks;
                                    await _repo.EditAsync(dataApp).ConfigureAwait(false);
                                }
                                //else if (dataUserConCurrentApproval.Count == 1)
                                //{
                                //    var nextUserApproval = dataApproval.ListUserApprovals.Where(x=>x.Id.Equals(item.Id) && x.IndexUser > item.IndexUser).OrderBy(c=>c.IndexUser).First();
                                //    if(nextUserApproval.StatusApproved != StatusApproved.Approved)
                                //        await _repoManageUserApproval.UpdateNextApproval(dataUserApp.TrTempApprovalId, Convert.ToInt32(nextUserApproval.IndexUser)).ConfigureAwait(false);

                                //    var dataConcurrent = _repoManageUserApproval.GetCurrentApproval(dataUserApp.TrTempApprovalId).Result;
                                //    if (dataConcurrent != null)
                                //    {
                                //        if(dataConcurrent.StatusApproved != StatusApproved.Approved)
                                //            await _repoManageConditionApproval.UpdateNextApproval(dataConcurrent.Id).ConfigureAwait(false);
                                //    }

                                //    // var dataApp = await _repo.Get(dataUserApp.Id).ConfigureAwait(false);
                                //    var dataApp = await _repo.Get(dataUserApp.TrTempApprovalId).ConfigureAwait(false);
                                //    dataApp.StatusApproved = StatusApproved.Approved;
                                //    await _repo.EditAsync(dataApp).ConfigureAwait(false);
                                //}
                                else
                                {
                                    var nextUserApproval = dataApproval.ListUserApprovals.Where(x => x.TrTempApprovalId.Equals(item.TrTempApprovalId) && x.IndexUser > item.IndexUser).OrderBy(c => c.IndexUser).First();
                                    var NextEmployee = await _repoUserApp.GetByEmployeeBasicInfoId(nextUserApproval.EmployeeBasicInfoId).ConfigureAwait(false);
                                    var checkNotif = await _repoTemplate.CheckNotificationIsExists(dataUserApp.NotificationTypeId, NextEmployee.Email, data.RefId);
                                    if (nextUserApproval.StatusApproved != StatusApproved.Approved)
                                    {
                                        await _repoManageUserApproval.UpdateNextApproval(dataUserApp.TrTempApprovalId, Convert.ToInt32(nextUserApproval.IndexUser)).ConfigureAwait(false);
                                        
                                        if (!checkNotif)
                                        {
                                            await _repoTemplate.AddNotification(dataUserApp.NotificationTypeId, NextEmployee.Email, dataUserApp.Module, data.RefId, dataApproval.IsNotifyByWeb, dataApproval.IsNotifyByEmail, dataApproval.CreatedBy, data.DetailLink);
                                        }
                                       
                                    }
                                        
                                    var dataConcurrent = _repoManageUserApproval.GetCurrentApproval(dataUserApp.TrTempApprovalId).Result;
                                    if (dataConcurrent != null) {
                                        if (dataConcurrent.StatusApproved != StatusApproved.Approved)
                                        {
                                            await _repoManageConditionApproval.UpdateNextApproval(dataConcurrent.Id).ConfigureAwait(false);
                                            var ConcurrentEmployee = await _repoUserApp.GetByEmployeeBasicInfoId(dataConcurrent.EmployeeBasicInfoId).ConfigureAwait(false);
                                            var checkNotifConcurrent = await _repoTemplate.CheckNotificationIsExists(dataUserApp.NotificationTypeId, ConcurrentEmployee.Email, data.RefId);
                                            if (!checkNotifConcurrent)
                                            {
                                                await _repoTemplate.AddNotification(dataUserApp.NotificationTypeId, ConcurrentEmployee.Email, dataUserApp.Module, data.RefId, dataApproval.IsNotifyByWeb, dataApproval.IsNotifyByEmail, dataApproval.CreatedBy, data.DetailLink);
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    }
                }
            }

            FinishedProcess:
            var sendNotifAfterApproval = await _repoTemplate.AddNotificationAfterApproval(requestempId, data.RefId, data.StatusApproved, data.DetailLink);
            return Ok(new ApiResponse(true, "Update status approval success"));
        }

        #region Chekc Is User Delegation Or Not....
        private List<UserDoaVm> CheckUserIsDelegationNonCoditional()
        {
            return _repoDoa.IsUserInDoa().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private void UpdateDelegation(TrTemplateApproval dataApproval, List<UserDoaVm> listDelgation, TrUpdateStatusApprovalViewModel data,string empId)
        {
            // jika dia Rejected...
            if (data.StatusApproved == 4)
            {
                var dataApp = _repo.Get(dataApproval.Id).ConfigureAwait(false).GetAwaiter().GetResult();
                dataApp.RemarkRejected = data.Remarks;
                dataApp.StatusApproved = StatusApproved.Rejected;
                dataApp.RejectedBy = empId;
                dataApp.RejectedOn = DateTime.Now;
                _repo.EditAsync(dataApp).ConfigureAwait(false).GetAwaiter().GetResult();

                //tinggal cari, dia di posisi mana, struktural atau conditional...
                foreach (var userApproval in dataApproval.ListUserApprovals)
                {
                    if (empId == userApproval.EmployeeBasicInfoId)
                    {
                        var dataUserAp = _repoManageUserApproval.Get(userApproval.Id).GetAwaiter().GetResult();
                        dataUserAp.StatusApproved = StatusApproved.Rejected;
                        dataUserAp.RemarkRejected = data.Remarks;
                        _repoManageUserApproval.EditAsync(dataUserAp).GetAwaiter().GetResult();
                    }

                    // cek di other position index for delegation...
                    foreach (var delegation in listDelgation)
                        if (delegation.EmployeeId == userApproval.EmployeeBasicInfoId)
                        {
                            var dataUserAp = _repoManageUserApproval.Get(userApproval.Id).GetAwaiter().GetResult();
                            dataUserAp.StatusApproved = StatusApproved.Rejected;
                            dataUserAp.RemarkRejected = data.Remarks;
                            _repoManageUserApproval.EditAsync(dataUserAp).GetAwaiter().GetResult();
                        }
                }

                //cek di conditional...for delegation and structured..
                foreach (var userApproval in dataApproval.ListUserApprovals)
                    foreach (var condition in userApproval.ListConditionalApprovals)
                    {
                        if (empId == condition.EmployeeBasicInfoId)
                        {
                            var dataCondition = _repoManageConditionApproval.Get(condition.Id).GetAwaiter().GetResult();
                            dataCondition.StatusApproved = StatusApproved.Rejected;
                            _repoManageConditionApproval.EditAsync(dataCondition).GetAwaiter().GetResult();
                        }
                    }
            }
            else // jika StatusApproved nya Approved
            {
                // cek di struktur nya dulu....all index position ...
                foreach (var userApproval in dataApproval.ListUserApprovals)
                {
                    if (empId == userApproval.EmployeeBasicInfoId)
                    {
                        var dataUserApproval = _repoManageUserApproval.Get(userApproval.Id).GetAwaiter().GetResult();
                        dataUserApproval.StatusApproved = (StatusApproved)Enum.ToObject(typeof(StatusApproved), data.StatusApproved);
                        if (userApproval.IsSpecialCase == true)
                            dataUserApproval.RemarkSpecialCase = data.Remarks;
                        _repoManageUserApproval.EditAsync(dataUserApproval).GetAwaiter().GetResult();

                        // update next approval jika masih ada....to be Current Approval..
                        var dataNextApproval = dataApproval.ListUserApprovals.Where(x=>x.IndexUser> userApproval.IndexUser).OrderBy(c=>c.IndexUser).First();
                        if(dataNextApproval != null)
                        {
                            if (dataNextApproval.StatusApproved != StatusApproved.Approved)
                            {
                                //update next status approval...
                                dataUserApproval = _repoManageUserApproval.Get(dataNextApproval.Id).GetAwaiter().GetResult();
                                dataUserApproval.StatusApproved = StatusApproved.CurrentApproval;
                                _repoManageUserApproval.EditAsync(dataUserApproval).GetAwaiter().GetResult();
                            }
                        }
                    }

                    // cek for delegation..
                    foreach (var delegation in listDelgation)
                    {
                        if (delegation.EmployeeId == userApproval.EmployeeBasicInfoId)
                        {
                            var dataUserApproval = _repoManageUserApproval.Get(delegation.EmployeeId).GetAwaiter().GetResult();
                            dataUserApproval.StatusApproved = (StatusApproved)Enum.ToObject(typeof(StatusApproved), data.StatusApproved);
                            if (userApproval.IsSpecialCase == true)
                                dataUserApproval.RemarkSpecialCase = data.Remarks;
                            _repoManageUserApproval.EditAsync(dataUserApproval).GetAwaiter().GetResult();

                            var dataNextApproval = dataApproval.ListUserApprovals.Where(x => x.IndexUser > userApproval.IndexUser).OrderBy(c => c.IndexUser).First();
                            if (dataNextApproval != null)
                            {
                                if (dataNextApproval.StatusApproved != StatusApproved.Approved)
                                {
                                    //update next status approval...
                                    dataUserApproval = _repoManageUserApproval.Get(dataNextApproval.Id).GetAwaiter().GetResult();
                                    dataUserApproval.StatusApproved = StatusApproved.CurrentApproval;
                                    _repoManageUserApproval.EditAsync(dataUserApproval).GetAwaiter().GetResult();
                                }
                            }
                        }
                    }
                }

                //cek di conditional user group approval nya...
                foreach (var userApproval in dataApproval.ListUserApprovals)
                    foreach (var x in userApproval.ListConditionalApprovals)
                    {
                        if (empId == x.EmployeeBasicInfoId)
                        {
                            if (x.ConditionalApproval == "OR")
                            {
                                if (x.GroupConditionApproval == "OR")
                                {
                                    CheckAllOrCondition(empId, data, x.Id, userApproval.Id, dataApproval.Id, dataApproval.ListUserApprovals);
                                }
                                else // kalau kondisi nya AND..
                                {
                                    CheckConditionGroupForOrAnd(userApproval.ListConditionalApprovals, empId, data, dataApproval.ListUserApprovals, dataApproval.Id);
                                }
                            }
                            else // conditional = AND
                            {
                                if (x.GroupConditionApproval == "OR")
                                {
                                    CheckConditionGroupForAndPlusOr(userApproval.ListConditionalApprovals, empId, data, dataApproval.ListUserApprovals);
                                }
                                else // Contional Group = AND
                                {
                                    CheckConditionGroupForAndPlusOr(userApproval.ListConditionalApprovals, empId, data, dataApproval.ListUserApprovals);
                                }
                            }

                        }

                        //cek delegation nya...
                        foreach (var delegation in listDelgation)
                        {
                            if (x.EmployeeBasicInfoId == delegation.EmployeeId)
                            {
                                if (x.ConditionalApproval == "OR")
                                {
                                    if (x.GroupConditionApproval == "OR")
                                    {
                                        CheckAllOrCondition(delegation.EmployeeId, data, x.Id, userApproval.Id, dataApproval.Id, dataApproval.ListUserApprovals);
                                    }
                                    else // kalau kondisi nya AND..
                                    {
                                        CheckConditionGroupForOrAnd(userApproval.ListConditionalApprovals, delegation.EmployeeId, data, dataApproval.ListUserApprovals, dataApproval.Id);
                                    }
                                }
                                else // conditional = AND
                                {
                                    if (x.GroupConditionApproval == "OR")
                                    {
                                        CheckConditionGroupForAndPlusOr(userApproval.ListConditionalApprovals, delegation.EmployeeId, data, dataApproval.ListUserApprovals);
                                    }
                                    else // Contional Group = AND
                                    {
                                        CheckConditionGroupForAndPlusOr(userApproval.ListConditionalApprovals, delegation.EmployeeId, data, dataApproval.ListUserApprovals);
                                    }
                                }

                            }

                        }
                    }
            }

        }

        #endregion

        #region Condition OR (or + And)
        private void CheckConditionGroupForOrAnd(List<TrConditionUserApproval> listConditional, string empId, TrUpdateStatusApprovalViewModel data, List<TrUserApproval> listConcurrentUser, string trApprovalId)
        {
            //1. Cek Self Group...
            //--Cek and Insert Group Index...Gropuing By GroupIndexApproval...
            var vmListGroup = new List<VmGrouping>();
            foreach (var item in listConditional)
            {
                var isready = false;
                foreach (var g in vmListGroup)
                {
                    if (item.GroupConditionIndex == g.GroupIndex) isready = true;
                }
                if (!isready)
                {
                    vmListGroup.Add(new VmGrouping { GroupIndex = Convert.ToInt32(item.GroupConditionIndex) });
                }
            }

            //update Table Conditional...
            var conData = listConditional.Where(c => c.EmployeeBasicInfoId == empId).FirstOrDefault();
            var dataCon = _repoManageConditionApproval.Get(conData.Id).Result;
            dataCon.StatusApproved = (StatusApproved)Enum.ToObject(typeof(StatusApproved), data.StatusApproved);
            dataCon.CreatedBy = empId;
            dataCon.CreatedOn = DateTime.Now;
            _repoManageConditionApproval.EditAsync(dataCon).ConfigureAwait(false);

            //--Add List Condition to Grouping INdex and CheckIN if user already approved all....
            foreach (var item in vmListGroup)
            {
                var dataConditionList = listConditional.Where(c => c.GroupConditionIndex == item.GroupIndex && (c.StatusApproved == StatusApproved.WaitingApproval || c.StatusApproved == StatusApproved.CurrentApproval)).ToList();
                if (dataConditionList.Count == 0)// semua dalam group sudah approved..
                {
                    var dataUserConCurrentApproval = listConcurrentUser.Where(x => x.StatusApproved == StatusApproved.WaitingApproval).ToList();
                    if (dataUserConCurrentApproval.Count == 0) // jika semua user dalam Group itu sudah approved..
                    {
                        //update table User APproval...
                        var dataUserApp = _repoManageUserApproval.Get(conData.TrUserApprovalId).Result;
                        dataUserApp.CreatedBy = empId;
                        dataUserApp.CreatedOn = DateTime.Now;
                        dataUserApp.StatusApproved = (StatusApproved)Enum.ToObject(typeof(StatusApproved), data.StatusApproved);
                        _repoManageUserApproval.EditAsync(dataUserApp).ConfigureAwait(false);

                        // Update table Header (TrTemplateApproval)....
                        var dataApp = _repo.Get(trApprovalId).Result;
                        dataApp.StatusApproved = StatusApproved.Approved;
                        dataApp.RejectedBy = empId;
                        dataApp.RejectedOn = DateTime.Now;
                        _repo.EditAsync(dataApp).ConfigureAwait(false);

                        var nextIndex = listConcurrentUser.Where(x=>x.IndexUser > dataUserApp.IndexUser).OrderBy(x=>x.IndexUser).First();
                        if (nextIndex != null) {
                            if(nextIndex.StatusApproved != StatusApproved.Approved)
                                //update User Approval Concurrent next Index...
                                _repoManageUserApproval.UpdateNextApproval(trApprovalId, Convert.ToInt32(nextIndex.IndexUser)).ConfigureAwait(false);
                        }
                        var dataConcurrent = _repoManageUserApproval.GetCurrentApproval(trApprovalId).Result;
                        if (dataConcurrent != null)
                            _repoManageConditionApproval.UpdateNextApproval(dataConcurrent.Id).ConfigureAwait(false);
                        break;
                    }
                }
            }
        }

        private void CheckAllOrCondition(string empId, TrUpdateStatusApprovalViewModel data, string idConditional, string idUserApproval, string idApproval, List<TrUserApproval> listConcurrentUser)
        {
            //update Table Conditional...
            var dataCon = _repoManageConditionApproval.Get(idConditional).Result;
            dataCon.StatusApproved = (StatusApproved)Enum.ToObject(typeof(StatusApproved), data.StatusApproved);
            dataCon.CreatedBy = empId;
            dataCon.CreatedOn = DateTime.Now;
            _repoManageConditionApproval.EditAsync(dataCon).ConfigureAwait(false);

            //update table User APproval...
            var dataUserApp = _repoManageUserApproval.Get(idUserApproval).Result;
            dataUserApp.CreatedBy = empId;
            dataUserApp.CreatedOn = DateTime.Now;
            dataUserApp.StatusApproved = (StatusApproved)Enum.ToObject(typeof(StatusApproved), data.StatusApproved);
            _repoManageUserApproval.EditAsync(dataUserApp).ConfigureAwait(false);

            if (data.StatusApproved == 4)// rejected
            {
                //update table tr approval header nya...and request form is Close...
                var dataApp = _repo.Get(idApproval).Result;
                dataApp.RemarkRejected = data.Remarks;
                dataApp.StatusApproved = StatusApproved.Rejected;
                dataApp.RejectedBy = empId;
                dataApp.RejectedOn = DateTime.Now;
                _repo.EditAsync(dataApp).ConfigureAwait(false);
            }
            else
            {
                //baca list user approval nya, masih ada atau nggak ada...
                var dataUserConCurrentApproval = listConcurrentUser.Where(x => x.StatusApproved == StatusApproved.CurrentApproval || x.StatusApproved == StatusApproved.WaitingApproval).ToList();
                if (dataUserConCurrentApproval.Count == 0)
                {
                    var dataApp = _repo.Get(idApproval).Result;
                    dataApp.StatusApproved = StatusApproved.Approved;
                    dataApp.RejectedBy = empId;
                    dataApp.RejectedOn = DateTime.Now;
                    _repo.EditAsync(dataApp).ConfigureAwait(false);
                }
                else
                {
                    var nextIndex = listConcurrentUser.Where(x=>x.IndexUser > dataUserApp.IndexUser).OrderBy(x=>x.IndexUser).First();
                    if (nextIndex != null)
                    {
                        // Cek apakah ada delegation yang Approve, kalo blom approve, di update User Approval Concurrent next Index..
                        if (nextIndex.StatusApproved != StatusApproved.Approved)
                            _repoManageUserApproval.UpdateNextApproval(idApproval, Convert.ToInt32(nextIndex)).ConfigureAwait(false);
                    }
                    var dataConcurrent = _repoManageUserApproval.GetCurrentApproval(idApproval).Result;
                    if (dataConcurrent != null)
                        _repoManageConditionApproval.UpdateNextApproval(dataConcurrent.Id).ConfigureAwait(false);
                }
            }
        }
        #endregion

        #region Condition AND (or + And)
        private void CheckConditionGroupForAndPlusOr(List<TrConditionUserApproval> listConditional, string empId, TrUpdateStatusApprovalViewModel data, List<TrUserApproval> listConcurrentUser)
        {
            //cek status approved nya Reject or Approve...
            if (data.StatusApproved == 4)
            {
                //update Table Conditional...
                var conData = listConditional.Where(c => c.EmployeeBasicInfoId == empId).FirstOrDefault();
                var dataCon = _repoManageConditionApproval.Get(conData.Id).Result;
                dataCon.StatusApproved = (StatusApproved)Enum.ToObject(typeof(StatusApproved), data.StatusApproved);
                dataCon.CreatedBy = empId;
                dataCon.CreatedOn = DateTime.Now;
                _repoManageConditionApproval.EditAsync(dataCon).ConfigureAwait(false);
                //update table User APproval...
                var dataUserApp = _repoManageUserApproval.Get(conData.TrUserApprovalId).Result;
                dataUserApp.CreatedBy = empId;
                dataUserApp.CreatedOn = DateTime.Now;
                dataUserApp.StatusApproved = (StatusApproved)Enum.ToObject(typeof(StatusApproved), data.StatusApproved);
                _repoManageUserApproval.EditAsync(dataUserApp).ConfigureAwait(false);

                //update table tr approval header nya...and request form is Close...
                var dataApp = _repo.Get(dataUserApp.TrTempApprovalId).Result;
                dataApp.RemarkRejected = data.Remarks;
                dataApp.StatusApproved = StatusApproved.Rejected;
                dataApp.RejectedBy = empId;
                dataApp.RejectedOn = DateTime.Now;
                _repo.EditAsync(dataApp).ConfigureAwait(false);
            }
            else
            {
                //1. Cek Self Group...
                //--Cek and Insert Group Index...Gropuing By GroupIndexApproval...
                var vmListGroup = new List<VmGrouping>();
                foreach (var item in listConditional)
                {
                    var isready = false;
                    foreach (var g in vmListGroup)
                    {
                        if (item.GroupConditionIndex == g.GroupIndex) isready = true;
                    }
                    if (!isready)
                    {
                        vmListGroup.Add(new VmGrouping { GroupIndex = Convert.ToInt32(item.GroupConditionIndex) });
                    }
                }

                //update Table Conditional...
                var conData = listConditional.Where(c => c.EmployeeBasicInfoId == empId).FirstOrDefault();
                var dataCon = _repoManageConditionApproval.Get(conData.Id).Result;
                dataCon.StatusApproved = (StatusApproved)Enum.ToObject(typeof(StatusApproved), data.StatusApproved);
                dataCon.CreatedBy = empId;
                dataCon.CreatedOn = DateTime.Now;
                _repoManageConditionApproval.EditAsync(dataCon).ConfigureAwait(false);

                //...cek other Group ,satu satu apakah group conditional nya AND atau OR..
                var vmListTempGroup = new List<VmGroupingChekclist>();
                vmListTempGroup.Add(new VmGroupingChekclist { GroupIndex = Convert.ToInt32(conData.GroupConditionIndex), IsAllApproved = true });

                foreach (var item in vmListGroup)
                {
                    if (conData.GroupConditionIndex != item.GroupIndex) // yang di cek Group INdex nya yang berbeda dari yang nge-Prove..
                    {
                        var vmGI = vmListTempGroup.Where(c => c.GroupIndex == item.GroupIndex).FirstOrDefault();
                        if (vmGI == null)
                        {
                            //..cek group approval nya..
                            var dataCondisi = listConditional.Where(c => c.GroupConditionIndex == item.GroupIndex).FirstOrDefault();
                            if (dataCondisi.GroupConditionApproval == "OR")//jika Or..
                            {
                                var dt = listConditional.Where(c => c.GroupConditionIndex == item.GroupIndex && c.StatusApproved == StatusApproved.Approved).ToList();
                                if (dt.Count > 0)
                                    vmListTempGroup.Add(new VmGroupingChekclist { GroupIndex = item.GroupIndex, IsAllApproved = true });
                                else vmListTempGroup.Add(new VmGroupingChekclist { GroupIndex = item.GroupIndex, IsAllApproved = false });

                            }
                            else // JIka AND group conditional nya..
                            {
                                var dt = listConditional.Where(c => c.GroupConditionIndex == item.GroupIndex && c.StatusApproved != StatusApproved.Approved).ToList();
                                if (dt.Count > 0)
                                    vmListTempGroup.Add(new VmGroupingChekclist { GroupIndex = item.GroupIndex, IsAllApproved = false });
                                else vmListTempGroup.Add(new VmGroupingChekclist { GroupIndex = item.GroupIndex, IsAllApproved = true });

                            }
                        }
                    }
                }

                var result = vmListTempGroup.Where(c => c.IsAllApproved == false).ToList();
                if (result.Count == 0) // sudah approved semua groupnya..
                {
                    //update table User APproval...
                    var dataUserApp = _repoManageUserApproval.Get(conData.TrUserApprovalId).Result;
                    dataUserApp.CreatedBy = empId;
                    dataUserApp.CreatedOn = DateTime.Now;
                    dataUserApp.StatusApproved = StatusApproved.Approved;
                    _repoManageUserApproval.EditAsync(dataUserApp).ConfigureAwait(false);

                    //baca list user approval nya, masih ada atau nggak ada...
                    var dataUserConCurrentApproval = listConcurrentUser.Where(x => x.StatusApproved == StatusApproved.CurrentApproval || x.StatusApproved == StatusApproved.WaitingApproval).ToList();
                    if (dataUserConCurrentApproval.Count == 0)
                    {
                        var dataApp = _repo.Get(dataUserApp.TrTempApprovalId).Result;
                        dataApp.StatusApproved = StatusApproved.Approved;
                        dataApp.RejectedBy = empId;
                        dataApp.RejectedOn = DateTime.Now;
                        _repo.EditAsync(dataApp).ConfigureAwait(false);
                    }
                    else
                    {
                        var dataNextUserApproval = listConcurrentUser.Where(x=>x.IndexUser > dataUserApp.IndexUser).OrderBy(x=>x.IndexUser).First();
                        if (dataNextUserApproval != null)
                        {
                            if(dataNextUserApproval.StatusApproved != StatusApproved.Approved)
                                _repoManageUserApproval.UpdateNextApproval(dataUserApp.TrTempApprovalId, Convert.ToInt32(dataNextUserApproval.IndexUser)).ConfigureAwait(false);
                        }
                        var dataConcurrent = _repoManageUserApproval.GetCurrentApproval(dataUserApp.TrTempApprovalId).Result;
                        if (dataConcurrent != null)
                            _repoManageConditionApproval.UpdateNextApproval(dataConcurrent.Id).ConfigureAwait(false);
                    }
                }

            }

        }

        #endregion
        public class VmListGroupCondition
        {
            public string Id { get; set; }
            public string EmpId { get; set; }
            public int IndexGroup { get; set; }
            public string GroupConditionalApproval { get; set; }
            public StatusApproved StatusApproved { get; set; }
        }

        public class VmGrouping
        {
            public int GroupIndex { get; set; }
        }
        public class VmGroupingChekclist
        {
            public int GroupIndex { get; set; }
            public bool IsAllApproved { get; set; }
        }

        public class EmailViewModel
        {
            public string MailTo { get; set; }
            public string MailSubject { get; set; }
            public string MailFrom { get; set; }
            public string MailBody { get; set; }
        }
    }
}