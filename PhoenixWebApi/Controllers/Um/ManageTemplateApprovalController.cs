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
    [Route("api/ManageTemplateApproval")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = @"Um")]
    public class ManageTemplateApprovalController : Controller
    {
        private readonly IManageTemplateApprovalService _repo;
        private readonly IManageUserApprovalService _repoManageUserApproval;
        private readonly IManageConditionalApprovalService _repoManageConditionApproval;
        public ManageTemplateApprovalController(IManageTemplateApprovalService repo, IManageUserApprovalService repoManageUserApproval, IManageConditionalApprovalService repoManageConditionApproval)
        {
            _repo = repo;
            _repoManageUserApproval = repoManageUserApproval;
            _repoManageConditionApproval = repoManageConditionApproval;
        }

        /// <summary>
        /// Get Template By Group Division...
        /// </summary>
        /// <returns></returns>
        [AllowAnonymousRole]
        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            var subGroupId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessSubGroup(this.HttpContext);
            var divisionId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessUnitDivisi(this.HttpContext);
            //// OPen this Remark, if not by Group...open all
            return Ok(await _repo.Get().ConfigureAwait(false));
            //return Ok(await _repo.GetBySubGroup(subGroupId, divisionId).ConfigureAwait(false));
        }

        /// <summary>
        /// Cek perubahan
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymousRole]
        [HttpGet]
        [Route("Get/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var subGroupId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessSubGroup(this.HttpContext);
            var divisionId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessUnitDivisi(this.HttpContext);
            return Ok(await _repo.Get(id).ConfigureAwait(false));
            //return Ok(await _repo.GetBySubGroup(subGroupId, divisionId).ConfigureAwait(false));
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("GetByCategory/{id}")]
        public async Task<IActionResult> GetByCategory(int id)
        {
            return Ok(await _repo.GetByCategory(id).ConfigureAwait(false));
        }

        [AllowAnonymousRole]
        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> Save([FromBody]TempApprovalViewModel data)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ApiResponse(false, "data not valid"));
            }
            var model = new TmTemplateApproval();
            model.TemplateName = data.TemplateName;
            model.IsNotifyByWeb = data.IsNotifyByWeb;
            model.IsNotifyByEmail = data.IsNotifyByEmail;
            model.DueDate = data.DueDate;
            model.Reminder = data.Reminder;
            model.ApprovalCategory = data.ApprovalCategory;
            model.ApprovalType = data.ApprovalType;
            model.MenuId = data.MenuId;
            model.JobTitleId = data.JobTitleId == "0" ? "" : data.JobTitleId;
            //model.ForDivisionId = data.ForDivisionId;
            model.MinBudget = data.MinBudget;
            model.MaxBudget = data.MaxBudget;
            model.IsSpecialCase = data.IsSpecialCase;
            List<vmModel> vm = new List<vmModel>();
            var employeeid = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(this.HttpContext);
  
            if (data.Id == "0")
            {
                var subGroupId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessSubGroup(this.HttpContext);
                var divisionId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessUnitDivisi(this.HttpContext);

                model.DivisionId = divisionId;
                model.SubGroupId = subGroupId;

                model.CreatedOn = DateTime.Now;

                if (data.IsAllDivision)
                {
                    model.IsAllDivision = data.IsAllDivision;
                    var tempId = Guid.NewGuid().ToString();
                    model.Id = tempId;
                    model.ForDivisionId = null;
                    await _repo.AddAsync(model).ConfigureAwait(false);
                    var ListDiv = await _repo.GetDivision().ConfigureAwait(false);
                    foreach (var item in data.ListUserApprovals)
                    {
                        var vmUser = new TmTemplateUserApproval();
                        vmUser.EmployeeBasicInfoId = item.EmployeeBasicInfoId;
                        vmUser.IndexUser = item.IndexUser;
                        vmUser.IsCondition = item.IsCondition;
                        vmUser.TmTempApprovalId = tempId;
                        vmUser.Id = Guid.NewGuid().ToString();
                        vmUser.IsSpecialCase = item.IsSpecialCase;
                        vmUser.CreatedOn = DateTime.Now;
                        await _repoManageUserApproval.AddAsync(vmUser).ConfigureAwait(false);
                    }

                    //if (data.IsSpecialCase)
                    //{
                    //    var userSpesialCase = await _repo.GetSpecialCaseUser(employeeid).ConfigureAwait(false);
                    //    var vmUser = new TmTemplateUserApproval();
                    //    vmUser.EmployeeBasicInfoId = userSpesialCase.EmployeeId;
                    //    vmUser.IndexUser = 100;
                    //    vmUser.IsCondition = false;
                    //    vmUser.TmTempApprovalId = tempId;
                    //    vmUser.Id = Guid.NewGuid().ToString();
                    //    vmUser.IsSpecialCase = true;
                    //    vmUser.CreatedOn = DateTime.Now;
                    //    await _repoManageUserApproval.AddAsync(vmUser).ConfigureAwait(false);
                    //}

                    vm.Add(new vmModel { IdTemp = tempId });
                }
                else
                {
                    foreach (var itemDiv in data.ForDivisionId)
                    {
                        var tempId = Guid.NewGuid().ToString();
                        model.Id = tempId;
                        model.ForDivisionId = itemDiv;
                        await _repo.AddAsync(model).ConfigureAwait(false);

                        foreach (var item in data.ListUserApprovals)
                        {
                            var vmUser = new TmTemplateUserApproval();
                            vmUser.EmployeeBasicInfoId = item.EmployeeBasicInfoId;
                            vmUser.IndexUser = item.IndexUser;
                            vmUser.IsCondition = item.IsCondition;
                            vmUser.TmTempApprovalId = tempId;
                            vmUser.Id = Guid.NewGuid().ToString();
                            vmUser.IsSpecialCase = item.IsSpecialCase;
                            vmUser.CreatedOn = DateTime.Now;
                            await _repoManageUserApproval.AddAsync(vmUser).ConfigureAwait(false);
                        }

                        //if (data.IsSpecialCase)
                        //{
                        //    var userSpesialCase = await _repo.GetSpecialCaseUser(employeeid).ConfigureAwait(false);
                        //    var vmUser = new TmTemplateUserApproval();
                        //    vmUser.EmployeeBasicInfoId = userSpesialCase.EmployeeId;
                        //    vmUser.IndexUser = 100;
                        //    vmUser.IsCondition = false;
                        //    vmUser.TmTempApprovalId = tempId;
                        //    vmUser.Id = Guid.NewGuid().ToString();
                        //    vmUser.IsSpecialCase = true;
                        //    vmUser.CreatedOn = DateTime.Now;
                        //    await _repoManageUserApproval.AddAsync(vmUser).ConfigureAwait(false);
                        //}

                        vm.Add(new vmModel { IdTemp = tempId });
                    }


                }
                //await _repo.AddAsync(model).ConfigureAwait(false);


                return Ok(new ApiResponse(vm, true, Messages.SuccessAdd));
            }
            else
            {
                model = await _repo.Get(data.Id).ConfigureAwait(false);
                model.TemplateName = data.TemplateName;
                model.IsNotifyByWeb = data.IsNotifyByWeb;
                model.IsNotifyByEmail = data.IsNotifyByEmail;
                model.DueDate = data.DueDate;
                model.Reminder = data.Reminder;
                model.ApprovalCategory = data.ApprovalCategory;
                model.ApprovalType = data.ApprovalType;
                model.ModifiedOn = DateTime.Now;
                model.IsAllDivision = data.IsAllDivision;
                model.IsSpecialCase = data.IsSpecialCase;
                await _repo.EditAsync(model).ConfigureAwait(false);

                // remark..
                var listUserApproval = await _repoManageUserApproval.GetByTemplateId(data.Id).ConfigureAwait(false);
                var userId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetUserId(this.HttpContext);
                //await _repoManageUserApproval.DeleteByTemplateIdAsync(data.Id,userId);



                foreach (var item in data.ListUserApprovals)
                {
                    var isready = listUserApproval.Where(x => x.EmployeeBasicInfoId.Equals(item.EmployeeBasicInfoId)).ToList();

                    if (isready.Count == 0) // penambahan baru
                    {
                        var vmUser = new TmTemplateUserApproval();
                        vmUser.EmployeeBasicInfoId = item.EmployeeBasicInfoId;
                        vmUser.IndexUser = item.IndexUser;
                        vmUser.IsCondition = item.IsCondition;
                        vmUser.TmTempApprovalId = data.Id;
                        vmUser.IsSpecialCase = item.IsSpecialCase;
                        vmUser.Id = Guid.NewGuid().ToString();
                        vmUser.CreatedOn = DateTime.Now;
                        await _repoManageUserApproval.AddAsync(vmUser).ConfigureAwait(false);
                    }
                }

                var listUserAppNotReady = new List<TmTemplateUserApproval>();
                var listDeff = new List<TmpUserApprovalCompareVm>();

                listUserApproval = await _repoManageUserApproval.GetByTemplateId(data.Id).ConfigureAwait(false);
                if (data.ListUserApprovals.Count > listUserApproval.Count)
                {
                    var listA = new List<TmpUserApprovalCompareVm>();
                    var listB = new List<TmpUserApprovalCompareVm>();
                    foreach (var a in data.ListUserApprovals)
                    {
                        var mod = new TmpUserApprovalCompareVm();
                        mod.EmpBasicInfoId = a.EmployeeBasicInfoId;
                        listA.Add(mod);
                    }

                    foreach (var b in listUserApproval)
                    {
                        var mod = new TmpUserApprovalCompareVm();
                        mod.EmpBasicInfoId = b.EmployeeBasicInfoId;
                        listB.Add(mod);
                    }
                    listDeff = listA.Where(x => !listB.Any(z => z.EmpBasicInfoId == x.EmpBasicInfoId)).ToList();
                }
                else
                {
                    var listA = new List<TmpUserApprovalCompareVm>();
                    var listB = new List<TmpUserApprovalCompareVm>();
                    foreach (var a in listUserApproval)
                    {
                        var mod = new TmpUserApprovalCompareVm();
                        mod.EmpBasicInfoId = a.EmployeeBasicInfoId;
                        listA.Add(mod);
                    }

                    foreach (var b in data.ListUserApprovals)
                    {
                        var mod = new TmpUserApprovalCompareVm();
                        mod.EmpBasicInfoId = b.EmployeeBasicInfoId;
                        listB.Add(mod);
                    }
                    listDeff = listA.Where(x => !listB.Any(z => z.EmpBasicInfoId == x.EmpBasicInfoId)).ToList();

                }

                if (listDeff.Count > 0)
                {
                    foreach (var a in listDeff)
                    {
                        await _repoManageUserApproval.DeleteByTemplateIdAsync(data.Id, a.EmpBasicInfoId);
                    }
                }

                //if (data.IsSpecialCase)
                //{
                //    var userSpesialCase = await _repo.GetSpecialCaseUser(employeeid).ConfigureAwait(false);
                //    var listUserApprovalIsSpecialCase = await _repoManageUserApproval.GetByTemplateId(data.Id).ConfigureAwait(false);
                //    if (listUserApprovalIsSpecialCase.Count > 0)
                //    {
                //        foreach (var item in listUserApprovalIsSpecialCase.Where(x => x.IsSpecialCase == true))
                //        {
                //            if (item.EmployeeBasicInfoId == userSpesialCase.EmployeeId)
                //            {
                //                break;
                //            }
                //            else
                //            {
                //                await _repoManageUserApproval.DeleteByTemplateIdAsync(item.TmTempApprovalId, userSpesialCase.EmployeeId);
                //                var vmUser = new TmTemplateUserApproval();
                //                vmUser.EmployeeBasicInfoId = userSpesialCase.EmployeeId;
                //                vmUser.IndexUser = 100;
                //                vmUser.IsCondition = false;
                //                vmUser.TmTempApprovalId = data.Id;
                //                vmUser.Id = Guid.NewGuid().ToString();
                //                vmUser.IsSpecialCase = true;
                //                vmUser.CreatedOn = DateTime.Now;
                //                await _repoManageUserApproval.AddAsync(vmUser).ConfigureAwait(false);
                //            }
                //        }
                //    }
                //    else
                //    {
                //        var vmUser = new TmTemplateUserApproval();
                //        vmUser.EmployeeBasicInfoId = userSpesialCase.EmployeeId;
                //        vmUser.IndexUser = 100;
                //        vmUser.IsCondition = false;
                //        vmUser.TmTempApprovalId = data.Id;
                //        vmUser.Id = Guid.NewGuid().ToString();
                //        vmUser.IsSpecialCase = true;
                //        vmUser.CreatedOn = DateTime.Now;
                //        await _repoManageUserApproval.AddAsync(vmUser).ConfigureAwait(false);
                //    }
                    
                   
                //}
                //else
                //{
                //    var userSpesialCase = await _repo.GetSpecialCaseUser(userId).ConfigureAwait(false);
                //    var listUserApprovalIsSpecialCase = await _repoManageUserApproval.GetByTemplateId(data.Id).ConfigureAwait(false);
                //    foreach (var item in listUserApprovalIsSpecialCase.Where(x => x.IsSpecialCase == true))
                //    {
                //        await _repoManageUserApproval.DeleteByTemplateIdAsync(item.TmTempApprovalId, userSpesialCase.EmployeeId);
                //    }
                //}

                vm.Add(new vmModel { IdTemp = model.Id });
                //var vm = new
                //{
                //    IdTemp = model.Id
                //};
                return Ok(new ApiResponse(vm, true, Messages.SuccessEdit));
            }
        }

        [AllowAnonymousRole]
        [HttpPost]
        [Route("SaveConditional")]
        public async Task<IActionResult> SaveConditional([FromBody]List<TempUserApprovalViewModel> data)
        {
            foreach (var du in data)
            {
                if (string.IsNullOrEmpty(du.Id) || du.Id == "0")
                {
                    return Ok(new ApiResponse(false, "data not valid"));
                }
                var modelApp = await _repoManageUserApproval.Get(du.Id).ConfigureAwait(false);
                modelApp.IsCondition = du.IsCondition;
                await _repoManageUserApproval.EditAsync(modelApp).ConfigureAwait(false);

                await _repoManageConditionApproval.DeleteByUserApprovalTempId(du.Id).ConfigureAwait(false);
                foreach (var item in du.ListConditionalApprovals)
                {
                    var model = new TmConditionUserApproval();
                    model.Id = Guid.NewGuid().ToString();
                    model.EmployeeBasicInfoId = item.EmployeeBasicInfoId;
                    model.GroupConditionIndex = item.GroupConditionIndex;
                    model.GroupConditionApproval = item.GroupConditionApproval;
                    model.ConditionalApproval = item.ConditionalApproval;
                    model.TemplateUserApprovalId = item.TemplateUserApprovalId;
                    model.CreatedOn = DateTime.Now;
                    await _repoManageConditionApproval.AddAsync(model).ConfigureAwait(false);
                }
            }
            return Ok(new ApiResponse(true, Messages.SuccessAdd));
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("Delete/{Id}")]
        public async Task<IActionResult> Delete(string Id)
        {
            var model = await _repo.Get(Id).ConfigureAwait(false);
            await _repo.DeleteSoftAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessDelete));
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("GetJobTitle")]
        public async Task<IActionResult> GetJobTitle()
        {
            var divisionId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessUnitDivisi(this.HttpContext);
            var listJobTitle = await _repo.GetJobTitlesByDivisionId(divisionId).ConfigureAwait(false);
            return Ok(listJobTitle);
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("GetDivision")]
        public async Task<IActionResult> GetDivision()
        {
            return Ok(await _repo.GetDivision().ConfigureAwait(false));
        }
    }

    public class vmModel
    {
        public string IdTemp { get; set; }
    }
}