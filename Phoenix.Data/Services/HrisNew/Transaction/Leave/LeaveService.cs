using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.Models.ViewModel;
using Phoenix.Data.RestApi;
using Phoenix.Data.Services.Um;
using Phoenix.Shared.Core.Contexts;

namespace Phoenix.Data
{
    public interface ILeaveService : IDataServiceHris<Leave>
    {
        Task<List<Leave>> GetApprovalList();
        Task<List<InfoEmployee>> GetAssignList(string id);
        //Task<Leave> GetLeaveBalanceInfoById(string employeeId, DateTime endDate);
        Task<List<Leave>> GetAllLeaveTransaction();

        Task<bool> IsEmployeeLeave(string employeeId);
    }

    public class LeaveService : ILeaveService
    {
        readonly DataContext context;
        readonly GlobalFunctionApproval globalFcApproval;
        readonly IManageMenuService menuService;
        string idTemplate = "";

        readonly FileService uploadFile;

        /// <summary>
        /// And endpoint to manage Leave
        /// </summary>
        /// <param name="context">Database context</param>
        public LeaveService(DataContext context, GlobalFunctionApproval globalFcApproval, IManageMenuService menuService, FileService uploadFile)
        {
            this.context = context;
            this.globalFcApproval = globalFcApproval;
            this.menuService = menuService;
            this.uploadFile = uploadFile;
        }

        public async Task<bool> IsEmployeeLeave(string employeeId)
        {
            var result = false;

            var check = await context.Leaves.Where(x => x.EmployeeBasicInfoId == employeeId && x.StartDate.ToString("yyyyMMdd") == DateTime.Now.ToString("yyyyMMdd")).FirstOrDefaultAsync();
            if (check != null)
            {
                result = true;
            }

            return result;
        }
        public async Task<int> AddAsync(Leave entity)
        {
            entity.Id = await TransID.GetTransId(context, Code.Leave, DateTime.Today);
            if (entity.file != null)
            {
                IFormFile file = entity.file;
                var file1 = await uploadFile.Upload(file, null);
                if (!string.IsNullOrEmpty(file1))
                {
                    entity.FileUpload = file1;
                }
            }
            else
            {
                entity.FileUpload = entity.FileUpload;
            }


            try
            {
                //var dataMenu = await menuService.GetByUniqueName(MenuUnique.Leave).ConfigureAwait(false);
                //if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
                //var dataTempate = context.TmTemplateApprovals.Where(x => x.MenuId == dataMenu.Id && x.IsDeleted == false).FirstOrDefault();
                //idTemplate = dataTempate.Id;
                //return await globalFcApproval.SubmitApproval<Leave>(false, entity.Id, entity.Status, $"{ApprovalLink.Leave}?Id={entity.Id}&isApprove=true", idTemplate, MenuUnique.Leave, entity);
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        //var id = await TransID.GetTransId(context, Code.Overtime, DateTime.Today);
                        //entity.Id = id;
                        //entity.RequestCode = id;



                        await context.PhoenixAddAsync(entity);
                        var isApprove = true;
                        if (entity.Status != 0)//0: Draft
                        {

                            var dataMenu = await menuService.GetByUniqueName(MenuUnique.Leave).ConfigureAwait(false);
                            if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
                            var dataTempate = context.TmTemplateApprovals.Where(x => x.MenuId == dataMenu.Id && x.IsDeleted == false).FirstOrDefault();
                            idTemplate = dataTempate.Id;
                            var vm = new Models.ViewModel.TransApprovalHrisVm()
                            {
                                MenuId = dataMenu.Id,
                                RefId = entity.Id,
                                DetailLink = $"{ApprovalLink.Leave}?Id={entity.Id}&isApprove=true",
                                IdTemplate = idTemplate,
                                Module = NotificationModuleType.ModuleHRIS,
                                NotificationType = NotificationTypeCode.LeaveNotification
                            };



                            var subGroupId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessSubGroup(context.HttpContext);
                            var divisionId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessUnitDivisi(context.HttpContext);
                            var employeeId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(context.HttpContext);
                            isApprove = await globalFcApproval.Save(vm, subGroupId, divisionId, employeeId);

                            if (isApprove)
                            {
                                var save = await context.SaveChangesAsync();
                                transaction.Commit();
                                return save;
                            }
                            else
                            {
                                transaction.Rollback();
                                throw new Exception("please check the approval template that will be processed");
                            }
                        }
                        else
                        {
                            var save = await context.SaveChangesAsync();
                            transaction.Commit();
                            return save;
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception(ex.Message);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
           
        }

        public async Task<int> EditAsync(Leave entity)
        {
            //var dataMenu = await menuService.GetByUniqueName(MenuUnique.Leave).ConfigureAwait(false);
            //if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
            if (entity.file != null)
            {
                IFormFile file = entity.file;
                var file1 = await uploadFile.Upload(file, null);
                if (!string.IsNullOrEmpty(file1))
                {
                    entity.FileUpload = file1;
                }
            }
            else
            {
                entity.FileUpload = entity.FileUpload;
            }
            //var dataTempate = context.TmTemplateApprovals.Where(x => x.MenuId == dataMenu.Id && x.IsDeleted == false).FirstOrDefault();
            //idTemplate = dataTempate.Id;
            //return await globalFcApproval.SubmitApproval<Leave>(true, entity.Id, entity.Status, $"{ApprovalLink.Leave}?Id={entity.Id}&isApprove=true", idTemplate, MenuUnique.Leave, entity);

            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    //if (string.IsNullOrEmpty(entity.RequestCode))
                    //{
                    //    entity.RequestCode = (context.Overtimes.Where(x => x.Id == entity.Id).FirstOrDefault() ?? new Overtime()).RequestCode;
                    //}

                    context.PhoenixEdit(entity);

                    var isApprove = true;
                    if (entity.Status != 0)//0: Draft
                    {

                        var dataMenu = await menuService.GetByUniqueName(MenuUnique.Leave).ConfigureAwait(false);
                        if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
                        var dataTempate = context.TmTemplateApprovals.Where(x => x.MenuId == dataMenu.Id && x.IsDeleted == false).FirstOrDefault();
                        idTemplate = dataTempate.Id;
                        var vm = new Models.ViewModel.TransApprovalHrisVm()
                        {
                            MenuId = dataMenu.Id,
                            RefId = entity.Id,
                            DetailLink = $"{ApprovalLink.Leave}?Id={entity.Id}&isApprove=true",
                            IdTemplate = idTemplate,
                            Module = NotificationModuleType.ModuleHRIS,
                            NotificationType = NotificationTypeCode.OvertimeNotification
                        };



                        var subGroupId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessSubGroup(context.HttpContext);
                        var divisionId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessUnitDivisi(context.HttpContext);
                        var employeeId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(context.HttpContext);
                        isApprove = await globalFcApproval.Save(vm, subGroupId, divisionId, employeeId);

                        if (isApprove)
                        {
                            var save = await context.SaveChangesAsync();
                            transaction.Commit();
                            return save;
                        }
                        else
                        {
                            transaction.Rollback();
                            throw new Exception("please check the approval template that will be processed");
                        }
                    }
                    else
                    {
                        var save = await context.SaveChangesAsync();
                        transaction.Commit();
                        return save;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }

            }
        }

        public async Task<int> DeleteSoftAsync(Leave entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> ApproveAsync(Leave entity)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    context.PhoenixApprove(entity);

                    var vm = new TrUpdateStatusApprovalViewModel()
                    {
                        RefId = entity.Id,
                        StatusApproved = entity.Status ?? 0,
                        Remarks = entity.RemarkRejected
                    };

                    var employeeId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(context.HttpContext);
                    var isApprove = await globalFcApproval.UpdateStatusApproval(vm, employeeId);

                    var template = context.TrTemplateApprovals.Where(x => x.RefId == entity.Id).FirstOrDefault();

                    if (template != null)
                    {
                        if (template.StatusApproved.GetHashCode() == StatusTransaction.Approved)
                        {
                            DateTime? RequestDate;
                            var leaveRequest = await context.Leaves.AsNoTracking().Where(x => x.Id == entity.Id).FirstOrDefaultAsync();
                            if (entity.RequestDate == null)
                            {
                                RequestDate = leaveRequest.RequestDate;
                            }
                            else
                            {
                                RequestDate = entity.RequestDate;
                            }
                            decimal sisaCuti = 0;
                            decimal sisaCutiCarryOver = 0;
                            decimal totalDays = Convert.ToDecimal(entity.TotalDays);
                           
                            var leaveBalanceThisYear = await context.LeaveBalances.AsNoTracking().Where(x => x.EmployeeBasicInfoId == entity.EmployeeBasicInfoId && x.IsDeleted == false && x.Type == entity.LeaveRequestTypeId).OrderByDescending(x=>x.ExpiredDate).FirstOrDefaultAsync();
                            sisaCuti = leaveBalanceThisYear.Balance.Value;

                            var leaveBalanceLastYear = await context.LeaveBalances.AsNoTracking().Where(x => x.EmployeeBasicInfoId == entity.EmployeeBasicInfoId && x.IsDeleted == false && x.Id != leaveBalanceThisYear.Id && x.Type == entity.LeaveRequestTypeId).OrderByDescending(x => x.ExpiredDate).FirstOrDefaultAsync();
                            var leaveRequestType = await context.LeaveRequestTypes.AsNoTracking().Where(x => x.Id == entity.LeaveRequestTypeId && x.IsDeleted == false).FirstOrDefaultAsync();

                            if (leaveBalanceThisYear == null)
                            {
                                LeaveBalance ent = new LeaveBalance();
                                ent.Id = Guid.NewGuid().ToString();
                                ent.EmployeeBasicInfoId = entity.EmployeeBasicInfoId;
                                ent.Total = leaveRequestType.MaximumDurationLeave;
                                ent.Type = entity.LeaveRequestTypeId;
                                ent.Year = DateTime.Now.Year;
                                ent.ExpiredDate = new DateTime(DateTime.Now.Year, 12, 31);
                                ent.Balance = leaveRequestType.MaximumDurationLeave;
                                ent.CreatedBy = "System";
                                ent.CreatedOn = DateTime.Now;
                                ent.IsDeleted = false;

                                await context.LeaveBalances.AddAsync(ent);
                                await context.SaveChangesAsync();

                            }

                            if (leaveRequest.UseCarryOver == true)
                            {
                                sisaCutiCarryOver = leaveBalanceLastYear.Balance.Value;
                            }
                           
                            if (leaveRequestType.IsReduceBalance)
                            {

                                if (leaveRequest.UseCarryOver == true)
                                {
                                    var reduceCarryOver = sisaCutiCarryOver - entity.TotalDays;
                                    if (reduceCarryOver < 0)
                                    {
                                        leaveBalanceLastYear.Balance = 0;
                                        totalDays = totalDays - sisaCutiCarryOver;
                                    }
                                    else
                                    {
                                        leaveBalanceLastYear.Balance = reduceCarryOver;
                                        totalDays = 0;
                                    }
                                    context.PhoenixEdit(leaveBalanceLastYear);
                                }

                                if (totalDays > 0)
                                {
                                    var reduceCurrentYear = sisaCuti - totalDays;
                                    if (reduceCurrentYear < 0)
                                    {
                                        leaveBalanceThisYear.Balance = 0;
                                        totalDays = totalDays - sisaCuti;
                                    }
                                    else
                                    {
                                        leaveBalanceThisYear.Balance = reduceCurrentYear;
                                        totalDays = 0;
                                    }
                                    context.PhoenixEdit(leaveBalanceThisYear);
                                }
                               
                               

                                #region old
                                //if (RequestDate.Value.Month > defExp.Month)
                                //{
                                //    leaveBalance = leaveBalance.Where(x => x.Year == RequestDate.Value.Year).ToList();
                                //}
                                //else
                                //{
                                //    var yearUpdate = RequestDate.Value.Year - 1;
                                //    leaveBalance = leaveBalance.Where(x => x.Year <= yearUpdate).ToList();
                                //}

                                //foreach (var item in leaveBalance.OrderBy(x => x.ExpiredDate).ThenByDescending(x => x.Type))
                                //{
                                //    if (entity.TotalDays > item.Balance)
                                //    {
                                //        sisaCuti = (entity.TotalDays ?? 0) - (item.Balance ?? 0);
                                //        item.Balance = 0;
                                //        break;
                                //    }
                                //    else
                                //    {
                                //        item.Balance = entity.TotalDays - item.Balance;
                                //        break;
                                //    }

                                //    //if ((entity.TotalDays - sisaCuti) > item.Balance)
                                //    //{
                                //    //    sisaCuti = ((entity.TotalDays ?? 0) - sisaCuti) - (item.Balance ?? 0);
                                //    //    item.Balance = 0;
                                //    //}
                                //    //else
                                //    //{
                                //    //    if (sisaCuti > 0)
                                //    //    {
                                //    //        item.Balance = (item.Balance ?? 0) - sisaCuti;
                                //    //    }
                                //    //    else
                                //    //    {
                                //    //        item.Balance = (item.Balance ?? 0) - (entity.TotalDays ?? 0);
                                //    //    }
                                //    //    break;
                                //    //}
                                //}
                                //context.PhoenixEditRange(leaveBalance.ToArray());
                                //if (sisaCuti > 0)
                                //{
                                //    var leaveBalanceThisYear = await context.LeaveBalances.AsNoTracking().Where(x => x.EmployeeBasicInfoId == entity.EmployeeBasicInfoId && x.Year == RequestDate.Value.Year).ToListAsync();
                                //    foreach (var item in leaveBalanceThisYear.OrderBy(x => x.ExpiredDate).ThenByDescending(x => x.Type))
                                //    {
                                //        item.Balance = (item.Balance ?? 0) - sisaCuti;
                                //        break;
                                //    }
                                //    context.PhoenixEditRange(leaveBalanceThisYear.ToArray());
                                //}
                                #endregion
                            }
                            
                           
                        }
                    }

                    if (isApprove)
                    {
                        var save = await context.SaveChangesAsync();
                        transaction.Commit();
                        return save;
                    }
                    else
                    {
                        transaction.Rollback();
                        throw new Exception();
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<List<Leave>> Get()
        {
            var result = await (from a in context.Leaves
                                join b in context.EmployeeBasicInfos on a.EmployeeBasicInfoId equals b.Id
                                join c in context.LeaveRequestTypes on a.LeaveRequestTypeId equals c.Id
                                join d in context.TrTemplateApprovals on a.Id equals d.RefId
                                join e in context.EmployeeBasicInfos on a.AssistedJob equals e.Id into ac
                                from tb1 in ac.DefaultIfEmpty()
                                where !(a.IsDeleted ?? false) && a.EmployeeBasicInfoId == context.GetEmployeeId()
                                select new Leave
                                {
                                    Id = a.Id,
                                    EmployeeBasicInfoId = a.EmployeeBasicInfoId,
                                    StartDate = a.StartDate,
                                    EndDate = a.EndDate,
                                    Reason = a.Reason,
                                    StatusDesc = d.StatusApprovedDescription,
                                    TotalDays = a.TotalDays,
                                    LeaveRequestTypeId = a.LeaveRequestTypeId,
                                    AssistedJob = a.AssistedJob,
                                    AssistedJobName = tb1.NameEmployee,
                                    OtherLeave = a.OtherLeave,
                                    Reached = a.Reached,
                                    RequestDate = a.RequestDate,
                                    LeaveRequestType = c,
                                    UseCarryOver = a.UseCarryOver,
                                    UseCurrentYear = a.UseCurrentYear,
                                    requestEmployee = new InfoEmployee
                                    {
                                        EmployeeId = b.Id,
                                        EmployeeName = b.NameEmployee,
                                        //DateEmployed = b.JoinDate,
                                        //Phone = b.PhoneNumber,
                                        //Email = b.Email,
                                        //DateBirth = b.DateBirth,
                                        //Age = Convert.ToDateTime(b.DateBirth).AddYears(DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) > DateTime.Now ?
                                        //                      (DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) - 1 : DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year,
                                        JobTitle = b.BusinessUnitJobLevelId == null ? "" :
                                                                          (from r in context.BusinessUnitJobLevels
                                                                           join s in context.JobTitles on r.JobTitleId equals s.Id
                                                                           where r.Id == b.BusinessUnitJobLevelId
                                                                           select s.TitleName).FirstOrDefault(),
                                        Grade = b.BusinessUnitJobLevelId == null ? "" :
                                                                          (from r in context.BusinessUnitJobLevels
                                                                           join s in context.JobGrades on r.JobGradeId equals s.Id
                                                                           where r.Id == b.BusinessUnitJobLevelId
                                                                           select s.GradeName).FirstOrDefault(),
                                        //EmployementStatus = context.EmployementStatuss.Where(x => x.Id == b.EmployementStatusId).Select(x => x.EmploymentStatusName).FirstOrDefault()
                                    }
                                }).OrderByDescending(x => x.CreatedOn).ToListAsync();


            var result2 = await (from a in context.Leaves
                                 join b in context.EmployeeBasicInfos on a.EmployeeBasicInfoId equals b.Id
                                 join c in context.LeaveRequestTypes on a.LeaveRequestTypeId equals c.Id
                                 join e in context.EmployeeBasicInfos on a.AssistedJob equals e.Id into ac
                                 from tb1 in ac.DefaultIfEmpty()
                                 where !(a.IsDeleted ?? false) && a.EmployeeBasicInfoId == context.GetEmployeeId() && !context.TrTemplateApprovals.Any(x => x.RefId == a.Id)
                                 select new Leave
                                 {
                                     Id = a.Id,
                                     EmployeeBasicInfoId = a.EmployeeBasicInfoId,
                                     StartDate = a.StartDate,
                                     EndDate = a.EndDate,
                                     Reason = a.Reason,
                                     StatusDesc = StatusTransaction.StatusName(StatusTransaction.Draft),
                                     TotalDays = a.TotalDays,
                                     LeaveRequestTypeId = a.LeaveRequestTypeId,
                                     AssistedJob = a.AssistedJob,
                                     AssistedJobName = tb1.NameEmployee,
                                     OtherLeave = a.OtherLeave,
                                     Reached = a.Reached,
                                     RequestDate = a.RequestDate,
                                     LeaveRequestType = c,
                                     UseCarryOver = a.UseCarryOver,
                                     UseCurrentYear = a.UseCurrentYear,
                                     requestEmployee = new InfoEmployee
                                     {
                                         EmployeeId = b.Id,
                                         EmployeeName = b.NameEmployee,
                                         //DateEmployed = b.JoinDate,
                                         //Phone = b.PhoneNumber,
                                         //Email = b.Email,
                                         //DateBirth = b.DateBirth,
                                         //Age = Convert.ToDateTime(b.DateBirth).AddYears(DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) > DateTime.Now ?
                                         //                      (DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) - 1 : DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year,
                                         JobTitle = b.BusinessUnitJobLevelId == null ? "" :
                                                                          (from r in context.BusinessUnitJobLevels
                                                                           join s in context.JobTitles on r.JobTitleId equals s.Id
                                                                           where r.Id == b.BusinessUnitJobLevelId
                                                                           select s.TitleName).FirstOrDefault(),
                                         Grade = b.BusinessUnitJobLevelId == null ? "" :
                                                                          (from r in context.BusinessUnitJobLevels
                                                                           join s in context.JobGrades on r.JobGradeId equals s.Id
                                                                           where r.Id == b.BusinessUnitJobLevelId
                                                                           select s.GradeName).FirstOrDefault(),
                                         //EmployementStatus = context.EmployementStatuss.Where(x => x.Id == b.EmployementStatusId).Select(x => x.EmploymentStatusName).FirstOrDefault()
                                     }
                                 }).OrderByDescending(x => x.CreatedOn).ToListAsync();


            if (result == null) result = new List<Leave>();
            result.AddRange(result2);
            return result;
        }

        public async Task<List<Leave>> GetAllLeaveTransaction()
        {
            var result = await (from a in context.Leaves
                                join b in context.EmployeeBasicInfos on a.EmployeeBasicInfoId equals b.Id
                                join c in context.LeaveRequestTypes on a.LeaveRequestTypeId equals c.Id
                                join d in context.TrTemplateApprovals on a.Id equals d.RefId
                                join e in context.EmployeeBasicInfos on a.AssistedJob equals e.Id into ac
                                from tb1 in ac.DefaultIfEmpty()
                                where !(a.IsDeleted ?? false) 
                                select new Leave
                                {
                                    Id = a.Id,
                                    EmployeeBasicInfoId = a.EmployeeBasicInfoId,
                                    StartDate = a.StartDate,
                                    EndDate = a.EndDate,
                                    Reason = a.Reason,
                                    StatusDesc = d.StatusApprovedDescription,
                                    TotalDays = a.TotalDays,
                                    LeaveRequestTypeId = a.LeaveRequestTypeId,
                                    AssistedJob = a.AssistedJob,
                                    AssistedJobName = tb1.NameEmployee,
                                    OtherLeave = a.OtherLeave,
                                    Reached = a.Reached,
                                    RequestDate = a.RequestDate,
                                    LeaveRequestType = c,
                                    UseCarryOver = a.UseCarryOver,
                                    UseCurrentYear = a.UseCurrentYear,
                                    requestEmployee = new InfoEmployee
                                    {
                                        EmployeeId = b.Id,
                                        EmployeeName = b.NameEmployee,
                                        //DateEmployed = b.JoinDate,
                                        //Phone = b.PhoneNumber,
                                        //Email = b.Email,
                                        //DateBirth = b.DateBirth,
                                        //Age = Convert.ToDateTime(b.DateBirth).AddYears(DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) > DateTime.Now ?
                                        //                      (DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) - 1 : DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year,
                                        JobTitle = b.BusinessUnitJobLevelId == null ? "" :
                                                                          (from r in context.BusinessUnitJobLevels
                                                                           join s in context.JobTitles on r.JobTitleId equals s.Id
                                                                           where r.Id == b.BusinessUnitJobLevelId
                                                                           select s.TitleName).FirstOrDefault(),
                                        Grade = b.BusinessUnitJobLevelId == null ? "" :
                                                                          (from r in context.BusinessUnitJobLevels
                                                                           join s in context.JobGrades on r.JobGradeId equals s.Id
                                                                           where r.Id == b.BusinessUnitJobLevelId
                                                                           select s.GradeName).FirstOrDefault(),
                                        //EmployementStatus = context.EmployementStatuss.Where(x => x.Id == b.EmployementStatusId).Select(x => x.EmploymentStatusName).FirstOrDefault()
                                    }
                                }).OrderByDescending(x => x.CreatedOn).ToListAsync();


            var result2 = await (from a in context.Leaves
                                 join b in context.EmployeeBasicInfos on a.EmployeeBasicInfoId equals b.Id
                                 join c in context.LeaveRequestTypes on a.LeaveRequestTypeId equals c.Id
                                 join e in context.EmployeeBasicInfos on a.AssistedJob equals e.Id into ac
                                 from tb1 in ac.DefaultIfEmpty()
                                 where !(a.IsDeleted ?? false) && a.EmployeeBasicInfoId == context.GetEmployeeId() && !context.TrTemplateApprovals.Any(x => x.RefId == a.Id)
                                 select new Leave
                                 {
                                     Id = a.Id,
                                     EmployeeBasicInfoId = a.EmployeeBasicInfoId,
                                     StartDate = a.StartDate,
                                     EndDate = a.EndDate,
                                     Reason = a.Reason,
                                     StatusDesc = StatusTransaction.StatusName(StatusTransaction.Draft),
                                     TotalDays = a.TotalDays,
                                     LeaveRequestTypeId = a.LeaveRequestTypeId,
                                     AssistedJobName = tb1.NameEmployee,
                                     AssistedJob = a.AssistedJob,
                                     OtherLeave = a.OtherLeave,
                                     Reached = a.Reached,
                                     RequestDate = a.RequestDate,
                                     LeaveRequestType = c,
                                     UseCarryOver = a.UseCarryOver,
                                     UseCurrentYear = a.UseCurrentYear,
                                     requestEmployee = new InfoEmployee
                                     {
                                         EmployeeId = b.Id,
                                         EmployeeName = b.NameEmployee,
                                         //DateEmployed = b.JoinDate,
                                         //Phone = b.PhoneNumber,
                                         //Email = b.Email,
                                         //DateBirth = b.DateBirth,
                                         //Age = Convert.ToDateTime(b.DateBirth).AddYears(DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) > DateTime.Now ?
                                         //                      (DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) - 1 : DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year,
                                         JobTitle = b.BusinessUnitJobLevelId == null ? "" :
                                                                          (from r in context.BusinessUnitJobLevels
                                                                           join s in context.JobTitles on r.JobTitleId equals s.Id
                                                                           where r.Id == b.BusinessUnitJobLevelId
                                                                           select s.TitleName).FirstOrDefault(),
                                         Grade = b.BusinessUnitJobLevelId == null ? "" :
                                                                          (from r in context.BusinessUnitJobLevels
                                                                           join s in context.JobGrades on r.JobGradeId equals s.Id
                                                                           where r.Id == b.BusinessUnitJobLevelId
                                                                           select s.GradeName).FirstOrDefault(),
                                         //EmployementStatus = context.EmployementStatuss.Where(x => x.Id == b.EmployementStatusId).Select(x => x.EmploymentStatusName).FirstOrDefault()
                                     }
                                 }).OrderByDescending(x => x.CreatedOn).ToListAsync();


            if (result == null) result = new List<Leave>();
            result.AddRange(result2);
            return result;
        }

        public async Task<List<Leave>> GetApprovalList()
        {
            var result = await (from a in context.Leaves
                                join b in context.EmployeeBasicInfos on a.EmployeeBasicInfoId equals b.Id
                                join c in context.LeaveRequestTypes on a.LeaveRequestTypeId equals c.Id
                                join e in context.TrTemplateApprovals on a.Id equals e.RefId
                                join d in context.TrUserApprovals on e.Id equals d.TrTempApprovalId
                                join f in context.EmployeeBasicInfos on a.AssistedJob equals f.Id into ac
                                from tb1 in ac.DefaultIfEmpty()
                                where !(a.IsDeleted ?? false) && d.StatusApproved == Models.Um.StatusApproved.CurrentApproval && d.EmployeeBasicInfoId == context.GetEmployeeId()
                                select new Leave
                                {
                                    Id = a.Id,
                                    EmployeeBasicInfoId = a.EmployeeBasicInfoId,
                                    StartDate = a.StartDate,
                                    EndDate = a.EndDate,
                                    Reason = a.Reason,
                                    StatusDesc = e.StatusApprovedDescription,
                                    TotalDays = a.TotalDays,
                                    LeaveRequestTypeId = a.LeaveRequestTypeId,
                                    AssistedJob = a.AssistedJob,
                                    AssistedJobName = tb1.NameEmployee,
                                    OtherLeave = a.OtherLeave,
                                    Reached = a.Reached,
                                    RequestDate = a.RequestDate,
                                    LeaveRequestType = c,
                                    UseCarryOver = a.UseCarryOver,
                                    UseCurrentYear = a.UseCurrentYear,
                                    requestEmployee = new InfoEmployee
                                    {
                                        EmployeeId = b.Id,
                                        EmployeeName = b.NameEmployee,
                                        //DateEmployed = b.JoinDate,
                                        //Phone = b.PhoneNumber,
                                        //Email = b.Email,
                                        //DateBirth = b.DateBirth,
                                        //Age = Convert.ToDateTime(b.DateBirth).AddYears(DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) > DateTime.Now ?
                                        //                      (DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) - 1 : DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year,
                                        JobTitle = b.BusinessUnitJobLevelId == null ? "" :
                                                                          (from r in context.BusinessUnitJobLevels
                                                                           join s in context.JobTitles on r.JobTitleId equals s.Id
                                                                           where r.Id == b.BusinessUnitJobLevelId
                                                                           select s.TitleName).FirstOrDefault(),
                                        Grade = b.BusinessUnitJobLevelId == null ? "" :
                                                                          (from r in context.BusinessUnitJobLevels
                                                                           join s in context.JobGrades on r.JobGradeId equals s.Id
                                                                           where r.Id == b.BusinessUnitJobLevelId
                                                                           select s.GradeName).FirstOrDefault(),
                                        //EmployementStatus = context.EmployementStatuss.Where(x => x.Id == b.EmployementStatusId).Select(x => x.EmploymentStatusName).FirstOrDefault()
                                    }
                                }).OrderByDescending(x => x.CreatedOn).ToListAsync();
            return result;
        }

        public async Task<Leave> Get(string Id)
        {
            var result = await (from a in context.Leaves
                                join b in context.EmployeeBasicInfos on a.EmployeeBasicInfoId equals b.Id
                                join c in context.LeaveRequestTypes on a.LeaveRequestTypeId equals c.Id
                                join d in context.TrTemplateApprovals on a.Id equals d.RefId
                                join e in context.Filemasters on a.FileUpload equals e.Id into es
                                from e in es.DefaultIfEmpty()
                                join f in context.EmployeeBasicInfos on a.AssistedJob equals f.Id into ac
                                from tb1 in ac.DefaultIfEmpty()
                                where !(a.IsDeleted ?? false) && a.Id == Id
                                select new Leave
                                {
                                    Id = a.Id,
                                    EmployeeBasicInfoId = a.EmployeeBasicInfoId,
                                    StartDate = a.StartDate,
                                    EndDate = a.EndDate,
                                    Reason = a.Reason,
                                    Status = d.StatusApproved.GetHashCode(),
                                    StatusDesc = d.StatusApprovedDescription,
                                    TotalDays = a.TotalDays,
                                    LeaveRequestTypeId = a.LeaveRequestTypeId,
                                    AssistedJob = a.AssistedJob,
                                    AssistedJobName = tb1.NameEmployee,
                                    OtherLeave = a.OtherLeave,
                                    Reached = a.Reached,
                                    RequestDate = a.RequestDate,
                                    CreatedBy = a.CreatedBy,
                                    CreatedOn = a.CreatedOn,
                                    ModifiedBy = a.ModifiedBy,
                                    ModifiedOn = a.ModifiedOn,
                                    ApprovedBy = a.ApprovedBy,
                                    ApprovedOn = a.ApprovedOn,
                                    IsActive = a.IsActive,
                                    IsLocked = a.IsLocked,
                                    IsDefault = a.IsDefault,
                                    IsDeleted = a.IsDeleted,
                                    OwnerId = a.OwnerId,
                                    DeletedBy = a.DeletedBy,
                                    DeletedOn = a.DeletedOn,
                                    BusinessUnitDivisionId = a.BusinessUnitDivisionId,
                                    BusinessUnitDepartementId = a.BusinessUnitDepartementId,
                                    FileUpload = a.FileUpload,
                                    FileUploadName = e.Name,
                                    //Annual = 0,
                                    //Compensatory = 0,
                                    UseCarryOver = a.UseCarryOver,
                                    UseCurrentYear = a.UseCurrentYear,
                                    requestEmployee = new InfoEmployee
                                    {
                                        EmployeeId = b.Id,
                                        EmployeeName = b.NameEmployee,
                                        //DateEmployed = b.JoinDate,
                                        //Phone = b.PhoneNumber,
                                        //Email = b.Email,
                                        //DateBirth = b.DateBirth,
                                        //Age = Convert.ToDateTime(b.DateBirth).AddYears(DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) > DateTime.Now ?
                                        //                      (DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) - 1 : DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year,
                                        JobTitle = b.BusinessUnitJobLevelId == null ? "" :
                                                                          (from r in context.BusinessUnitJobLevels
                                                                           join s in context.JobTitles on r.JobTitleId equals s.Id
                                                                           where r.Id == b.BusinessUnitJobLevelId
                                                                           select s.TitleName).FirstOrDefault(),
                                        Grade = b.BusinessUnitJobLevelId == null ? "" :
                                                                          (from r in context.BusinessUnitJobLevels
                                                                           join s in context.JobGrades on r.JobGradeId equals s.Id
                                                                           where r.Id == b.BusinessUnitJobLevelId
                                                                           select s.GradeName).FirstOrDefault(),
                                        //EmployementStatus = context.EmployementStatuss.Where(x => x.Id == b.EmployementStatusId).Select(x => x.EmploymentStatusName).FirstOrDefault()
                                    }
                                }).FirstOrDefaultAsync();


            if (result == null)
            {
                result = await (from a in context.Leaves
                                join b in context.EmployeeBasicInfos on a.EmployeeBasicInfoId equals b.Id
                                join c in context.LeaveRequestTypes on a.LeaveRequestTypeId equals c.Id
                                join e in context.Filemasters on a.FileUpload equals e.Id into es
                                from e in es.DefaultIfEmpty()
                                join f in context.EmployeeBasicInfos on a.AssistedJob equals f.Id into ac
                                from tb1 in ac.DefaultIfEmpty()
                                where !(a.IsDeleted ?? false) && a.Id == Id
                                select new Leave
                                {
                                    Id = a.Id,
                                    EmployeeBasicInfoId = a.EmployeeBasicInfoId,
                                    StartDate = a.StartDate,
                                    EndDate = a.EndDate,
                                    Reason = a.Reason,
                                    Status = StatusTransaction.Draft,//DRAFT
                                    StatusDesc = StatusTransaction.StatusName(StatusTransaction.Draft),
                                    TotalDays = a.TotalDays,
                                    LeaveRequestTypeId = a.LeaveRequestTypeId,
                                    AssistedJob = a.AssistedJob,
                                    AssistedJobName = tb1.NameEmployee,
                                    OtherLeave = a.OtherLeave,
                                    UseCarryOver = a.UseCarryOver,
                                    UseCurrentYear = a.UseCurrentYear,
                                    Reached = a.Reached,
                                    RequestDate = a.RequestDate,
                                    CreatedBy = a.CreatedBy,
                                    CreatedOn = a.CreatedOn,
                                    ModifiedBy = a.ModifiedBy,
                                    ModifiedOn = a.ModifiedOn,
                                    ApprovedBy = a.ApprovedBy,
                                    ApprovedOn = a.ApprovedOn,
                                    IsActive = a.IsActive,
                                    IsLocked = a.IsLocked,
                                    IsDefault = a.IsDefault,
                                    IsDeleted = a.IsDeleted,
                                    OwnerId = a.OwnerId,
                                    DeletedBy = a.DeletedBy,
                                    DeletedOn = a.DeletedOn,
                                    BusinessUnitDivisionId = a.BusinessUnitDivisionId,
                                    BusinessUnitDepartementId = a.BusinessUnitDepartementId,
                                    FileUpload = a.FileUpload,
                                    FileUploadName = e.Name,
                                    //Annual = 0,
                                    //Compensatory = 0,
                                    requestEmployee = new InfoEmployee
                                    {
                                        EmployeeId = b.Id,
                                        EmployeeName = b.NameEmployee,
                                        //DateEmployed = b.JoinDate,
                                        //Phone = b.PhoneNumber,
                                        //Email = b.Email,
                                        //DateBirth = b.DateBirth,
                                        //Age = Convert.ToDateTime(b.DateBirth).AddYears(DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) > DateTime.Now ?
                                        //                      (DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) - 1 : DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year,
                                        JobTitle = b.BusinessUnitJobLevelId == null ? "" :
                                                                          (from r in context.BusinessUnitJobLevels
                                                                           join s in context.JobTitles on r.JobTitleId equals s.Id
                                                                           where r.Id == b.BusinessUnitJobLevelId
                                                                           select s.TitleName).FirstOrDefault(),
                                        Grade = b.BusinessUnitJobLevelId == null ? "" :
                                                                          (from r in context.BusinessUnitJobLevels
                                                                           join s in context.JobGrades on r.JobGradeId equals s.Id
                                                                           where r.Id == b.BusinessUnitJobLevelId
                                                                           select s.GradeName).FirstOrDefault(),
                                        //EmployementStatus = context.EmployementStatuss.Where(x => x.Id == b.EmployementStatusId).Select(x => x.EmploymentStatusName).FirstOrDefault()
                                    }
                                }).FirstOrDefaultAsync();
            }
            return result;
        }

        public async Task<List<InfoEmployee>> GetAssignList(string id)
        {
            var result = await (from a in context.Leaves
                                join b in context.EmployeeBasicInfos on a.AssistedJob equals b.Id
                                where a.Id == id
                                select new InfoEmployee
                                {
                                    EmployeeId = b.Id,
                                    EmployeeName = b.NameEmployee,
                                    //DateEmployed = b.JoinDate,
                                    //Phone = b.PhoneNumber,
                                    //Email = b.Email,
                                    //DateBirth = b.DateBirth,
                                    //Age = Convert.ToDateTime(b.DateBirth).AddYears(DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) > DateTime.Now ?
                                    //                          (DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) - 1 : DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year,
                                    JobTitle = b.BusinessUnitJobLevelId == null ? "" :
                                                                          (from r in context.BusinessUnitJobLevels
                                                                           join s in context.JobTitles on r.JobTitleId equals s.Id
                                                                           where r.Id == b.BusinessUnitJobLevelId
                                                                           select s.TitleName).FirstOrDefault(),
                                    Grade = b.BusinessUnitJobLevelId == null ? "" :
                                                                          (from r in context.BusinessUnitJobLevels
                                                                           join s in context.JobGrades on r.JobGradeId equals s.Id
                                                                           where r.Id == b.BusinessUnitJobLevelId
                                                                           select s.GradeName).FirstOrDefault(),
                                    //EmployementStatus = context.EmployementStatuss.Where(x => x.Id == b.EmployementStatusId).Select(x => x.EmploymentStatusName).FirstOrDefault(),
                                    Division = b.BusinessUnitJobLevelId == null ? "" :
                                                                        (from r in context.BusinessUnitJobLevels
                                                                         join s in context.BusinessUnits on r.BusinessUnitDivisionId equals s.Id
                                                                         join t in context.BusinessUnitTypes on s.BusinessUnitTypeId equals t.Id
                                                                         where t.BusinessUnitLevel == BusinessUnitLevelCode.Division && r.Id == b.BusinessUnitJobLevelId
                                                                         select s.UnitName).FirstOrDefault()
                                }).ToListAsync();
            return result;
        }
    }
}