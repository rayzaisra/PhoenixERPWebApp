using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models;
using Phoenix.Data.RestApi;
using Phoenix.Shared.Core.Contexts;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models.Enum;
using Phoenix.Data.Models.Um;

namespace Phoenix.Data
{
    
    public interface IEmployeeLoanService : IDataServiceHris<EmployeeLoan>
    {
        Task<List<EmployeeLoanPaid>> GetLoanPaidList(string Id);
		Task<List<EmployeeLoan>> GetLisCoop();
		Task<int> AddEditLoanPaidAsync(List<EmployeeLoanPaid> entitys);
        Task<int> UpdateLoanPaid(EmployeeLoanPaid model);

    }

    public class EmployeeLoanService : IEmployeeLoanService
    {
        readonly DataContext context;
        readonly GlobalFunctionApproval globalFcApproval;
        readonly StatusLogService log;
        readonly Services.Um.IManageMenuService menuService;
        /// <summary>
        /// And endpoint to manage EmployeeLoan
        /// </summary>
        /// <param name="context">Database context</param>
        public EmployeeLoanService(DataContext context, GlobalFunctionApproval globalFcApproval, StatusLogService log, Services.Um.IManageMenuService menuService)
        {
            this.context = context;
            this.globalFcApproval = globalFcApproval;
            this.log = log;
            this.menuService = menuService;
        }

        public async Task<int> AddAsync(EmployeeLoan entity)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    entity.Id = Guid.NewGuid().ToString();
                    await context.PhoenixAddAsync(entity);

                    for (int i = 0; i < entity.PayrollDeduction; i++)
                    {
                        EmployeeLoanPaid paid = new EmployeeLoanPaid();
                        paid.Id = Guid.NewGuid().ToString();
                        paid.EmployeeLoanId = entity.Id;
                        paid.EmployeeBasicInfoId = entity.EmployeeBasicInfoId;
                        paid.DatePaid = Convert.ToDateTime(entity.RepaymentStartDate).AddMonths(i);
                        paid.Value = entity.ValueInstallmentAmount;
                        paid.Status = LoanStatus.NotPaid;
                        await context.PhoenixAddAsync(paid);
                    }
                    //return await context.SaveChangesAsync();
                    var isApprove = true;
                    if (entity.Status == EmployeeLoanStatus.WAITING_APPROVAL)
                    {
                        await log.AddAsync(new StatusLog() { TransactionId = entity.Id, Status = EmployeeLoanStatus.StatusName(entity.Status), Description = entity.LogDescription });
                        var approval = await context.TrTemplateApprovals.AsNoTracking().Where(x => x.RefId == entity.Id).ToListAsync();
                        if (approval.Count > 0)
                        {
                            var userApproval = await context.TrUserApprovals.AsNoTracking().Where(x => approval.Any(y => y.Id == x.TrTempApprovalId)).ToListAsync();
                            var conditionUserApproval = await context.TrConditionUserApprovals.AsNoTracking().Where(x => userApproval.Any(y => y.Id == x.TrUserApprovalId)).ToListAsync();

                            context.TrConditionUserApprovals.RemoveRange(conditionUserApproval);
                            context.TrUserApprovals.RemoveRange(userApproval);
                            context.TrTemplateApprovals.RemoveRange(approval);
                        }

                        var menuUnique = "";
                        var approvalLink = "";
                        var notificationType = "";
                        if (entity.IsCorporateLoan)
                        {
                            menuUnique = MenuUnique.EmployeeLoan;
                            approvalLink = ApprovalLink.EmployeeLoan;
                            notificationType = NotificationTypeCode.EmployeeLoanNotification;
                        }
                        else
                        {
                            menuUnique = MenuUnique.EmployeeLoanCoop;
                            approvalLink = ApprovalLink.EmployeeLoanCoop;
                            notificationType = NotificationTypeCode.EmployeeLoanCoopNotification;
                        }

                        var dataMenu = await menuService.GetByUniqueName(menuUnique).ConfigureAwait(false);
                        if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
                        var dataTempate = context.TmTemplateApprovals.Where(x => x.MenuId == dataMenu.Id && x.IsDeleted == false).FirstOrDefault();
                        var vm = new Models.ViewModel.TransApprovalHrisVm()
                        {
                            IdTemplate = dataTempate.Id,
                            MenuId = dataMenu.Id,
                            RefId = entity.Id,
                            DetailLink = $"{approvalLink}?Id={entity.Id}",
                            NotificationType = notificationType,
                            Module = NotificationModuleType.ModuleHRIS
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

        public async Task<int> EditAsync(EmployeeLoan entity)
        {
            using (var transaction = await context.Database.BeginTransactionAsync()) 
            {
                try
                {
                    context.PhoenixEdit(entity);

                    var isApprove = true;
                    if (entity.Status == EmployeeLoanStatus.WAITING_APPROVAL)
                    {
                        await log.AddAsync(new StatusLog() { TransactionId = entity.Id, Status = EmployeeLoanStatus.StatusName(entity.Status), Description = entity.LogDescription });
                        var approval = await context.TrTemplateApprovals.AsNoTracking().Where(x => x.RefId == entity.Id).ToListAsync();
                        if (approval.Count > 0)
                        {
                            var userApproval = await context.TrUserApprovals.AsNoTracking().Where(x => approval.Any(y => y.Id == x.TrTempApprovalId)).ToListAsync();
                            var conditionUserApproval = await context.TrConditionUserApprovals.AsNoTracking().Where(x => userApproval.Any(y => y.Id == x.TrUserApprovalId)).ToListAsync();

                            context.TrConditionUserApprovals.RemoveRange(conditionUserApproval);
                            context.TrUserApprovals.RemoveRange(userApproval);
                            context.TrTemplateApprovals.RemoveRange(approval);
                        }
                        var menuUnique = "";
                        var approvalLink = "";
                        var notificationType = "";
                        if (entity.IsCorporateLoan)
                        {
                            menuUnique = MenuUnique.EmployeeLoan;
                            approvalLink = ApprovalLink.EmployeeLoan;
                            notificationType = NotificationTypeCode.EmployeeLoanNotification;
                        }
                        else
                        {
                            menuUnique = MenuUnique.EmployeeLoanCoop;
                            approvalLink = ApprovalLink.EmployeeLoanCoop;
                            notificationType = NotificationTypeCode.EmployeeLoanCoopNotification;
                        }
                        var dataMenu = await menuService.GetByUniqueName(menuUnique).ConfigureAwait(false);
                        if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
                        var dataTempate = context.TmTemplateApprovals.Where(x => x.MenuId == dataMenu.Id && x.IsDeleted == false).FirstOrDefault();
                        var vm = new Models.ViewModel.TransApprovalHrisVm()
                        {
                            IdTemplate = dataTempate.Id,
                            MenuId = dataMenu.Id,
                            RefId = entity.Id,
                            DetailLink = $"{approvalLink}?Id={entity.Id}",
                            NotificationType = notificationType,
                            Module = NotificationModuleType.ModuleHRIS
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

                

            //return await context.SaveChangesAsync();
        }

        public async Task<int> ApproveAsync(EmployeeLoan entity)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var save = await globalFcApproval.UnsafeProcessApproval(entity.Id, entity.StatusApproval, entity.LogDescription, entity);
                    var approval = context.TrTemplateApprovals.AsNoTracking().Where(x => x.RefId == entity.Id && !(x.IsDeleted ?? false)).FirstOrDefault();
                    if (approval != null)
                    {
                        var StatusDesc = StatusTransaction.StatusName(approval.StatusApproved.GetHashCode());
                        if (approval.StatusApproved == Models.Um.StatusApproved.Approved)
                        {
                            //entity.Status = PCEStatus.Open;

                            var data = await context.EmployeeLoans.AsNoTracking().Where(x => x.Id == entity.Id).FirstOrDefaultAsync();
                            if (data != null)
                            {
                                context.PhoenixEdit(entity);
                            }
                        }
                        else if (approval.StatusApproved == Models.Um.StatusApproved.Rejected)
                        {
                            var data = await context.EmployeeLoans.AsNoTracking().Where(x => x.Id == entity.Id).FirstOrDefaultAsync();
                            StatusDesc = StatusTransactionName.Rejected;
                            context.PhoenixEdit(entity);

                            var trApproval = await context.TrTemplateApprovals.Where(x => x.RefId == entity.Id).Where(x => !(x.IsDeleted ?? false)).FirstOrDefaultAsync();
                            trApproval.StatusApproved = StatusApproved.Rejected;
                            context.PhoenixEdit(trApproval);
                        }

                        context.EmployeeLoans.Update(entity);
                        await log.AddAsync(new StatusLog() { TransactionId = entity.Id, Status = StatusDesc, Description = entity.LogDescription });
                    }

                    save = await context.SaveChangesAsync();
                    transaction.Commit();
                    return save;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<int> DeleteSoftAsync(EmployeeLoan entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<List<EmployeeLoan>> Get()
        {
            var result = await (from a in context.EmployeeLoans
                                join b in context.EmployeeBasicInfos on a.EmployeeBasicInfoId equals b.Id
                                join c in context.LoanCategorys on a.LoanCategoryId equals c.Id
                                where !(a.IsDeleted ?? false) && c.is_Corporate_Loan == true
                                select new EmployeeLoan
                                {
                                    Id = a.Id,
                                    EmployeeBasicInfoId = a.EmployeeBasicInfoId,
                                    LoanCategoryId = a.LoanCategoryId,
                                    DateLoan = a.DateLoan,
                                    Value = a.Value,
                                    PayrollDeduction = a.PayrollDeduction,
                                    RepaymentStartDate = a.RepaymentStartDate,
                                    LoanCategoryName = c.Name,
                                    isPaid = context.EmployeeLoanPaids.Where(x => x.EmployeeLoanId == a.Id && x.Status == LoanStatus.Paid).Count() > 0,
                                    CreatedOn = a.CreatedOn,
                                    infoEmployee = new InfoEmployee
                                    {
                                        EmployeeId = b.Id,
                                        EmployeeName = b.NameEmployee,
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
                                        LegalEntityId = b.LegalEntityId,
                                        LegalEntityName  = b.LegalEntityId == null ? "" :
                                                                            (from r in context.LegalEntity
                                                                             where r.Id == b.LegalEntityId
                                                                             select r.LegalEntityName).FirstOrDefault()
                                    },
                                    ValueTotalInstallment = (a.ValueTotalInstallment ?? 0),
                                    ValueInstallmentAmount = (a.ValueInstallmentAmount ?? 0),
                                    ValueInterestAmount = (a.ValueInterestAmount ?? 0),
                                    Status = (a.Status ?? 0),
                                    StatusDesc = EmployeeLoanStatus.StatusName(a.Status)
                                }).OrderByDescending(x => x.CreatedOn).ToListAsync();

            foreach (var item in result)
            {
                item.Remaining = (from a in context.EmployeeLoans
                                  join b in context.EmployeeLoanPaids on a.Id equals b.EmployeeLoanId
                                  where a.IsDeleted == false && b.EmployeeLoanId == item.Id && b.Status == 0
                                  select b.Value).ToList().Sum();
            }
            return result;
        }

		public async Task<List<EmployeeLoan>> GetLisCoop()
		{
			var result = await (from a in context.EmployeeLoans
								join b in context.EmployeeBasicInfos on a.EmployeeBasicInfoId equals b.Id
								join c in context.LoanCategorys on a.LoanCategoryId equals c.Id
                                where !(a.IsDeleted ?? false) && c.is_Corporate_Loan == false
								select new EmployeeLoan
								{
									Id = a.Id,
									EmployeeBasicInfoId = a.EmployeeBasicInfoId,
									LoanCategoryId = a.LoanCategoryId,
									DateLoan = a.DateLoan,
									Value = a.Value,
									PayrollDeduction = a.PayrollDeduction,
									RepaymentStartDate = a.RepaymentStartDate,
									LoanCategoryName = c.Name,
									isPaid = context.EmployeeLoanPaids.Where(x => x.EmployeeLoanId == a.Id && x.Status == LoanStatus.Paid).Count() > 0,
									CreatedOn = a.CreatedOn,
									infoEmployee = new InfoEmployee
									{
										EmployeeId = b.Id,
										EmployeeName = b.NameEmployee,
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
									},
                                    Status = (a.Status ?? 0),
                                    StatusDesc = EmployeeLoanStatus.StatusName(a.Status)
                                }).OrderByDescending(x => x.CreatedOn).ToListAsync();
            foreach (var item in result)
            {
                item.Remaining = (from a in context.EmployeeLoans
                                  join b in context.EmployeeLoanPaids on a.Id equals b.EmployeeLoanId
                                  where a.IsDeleted == false && b.EmployeeLoanId == item.Id && b.Status == 0
                                  select b.Value).ToList().Sum();
            }
            return result;
		}

		public async Task<EmployeeLoan> Get(string Id)
        {
            var result = await (from a in context.EmployeeLoans
                                join b in context.EmployeeBasicInfos on a.EmployeeBasicInfoId equals b.Id
                                join c in context.LoanCategorys on a.LoanCategoryId equals c.Id
                                where !(a.IsDeleted ?? false) && a.Id == Id
                                select new EmployeeLoan
                                {
                                    Id = a.Id,
                                    EmployeeBasicInfoId = a.EmployeeBasicInfoId,
                                    LoanCategoryId = a.LoanCategoryId,
                                    DateLoan = a.DateLoan,
                                    Value = a.Value,
                                    PayrollDeduction = a.PayrollDeduction,
                                    RepaymentStartDate = a.RepaymentStartDate,
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
                                    LoanCategoryName = c.Name,
                                    InterestMonth = c.InterestMonth,
                                    InterestValue = (a.Value ?? 0) + ((((a.Value ?? 0) * a.PayrollDeduction)) / 100),
                                    ValueInstallmentAmount = a.ValueInstallmentAmount,
                                    ValueInterestAmount = a.ValueInterestAmount,
                                    ValueTotalInstallment = a.ValueTotalInstallment,
                                    Status = a.Status,
                                    StatusDesc = EmployeeLoanStatus.StatusName(a.Status),
                                    LogDescription = a.LogDescription,
                                    infoEmployee = new InfoEmployee
                                    {
                                        EmployeeId = b.Id,
                                        EmployeeName = b.NameEmployee,
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
                                    }

                                }).FirstOrDefaultAsync();
            result.Remaining = (from a in context.EmployeeLoans
                                join b in context.EmployeeLoanPaids on a.Id equals b.EmployeeLoanId
                                where a.IsDeleted == false && b.EmployeeLoanId == result.Id && b.Status == 0
                                select b.Value).ToList().Sum();

            return result;
        }

		public async Task<int> AddEditLoanPaidAsync(List<EmployeeLoanPaid> entitys)
		{
			string loanId = entitys.FirstOrDefault()?.Id;
			string employeeBasicInfoId = entitys.FirstOrDefault()?.EmployeeBasicInfoId;

			var existVal = context.EmployeeLoanPaids.Where(x => x.EmployeeLoanId == loanId && x.EmployeeBasicInfoId == employeeBasicInfoId);
			if (existVal.Count() > 0)
			{
				context.EmployeeLoanPaids.RemoveRange(existVal);
				await context.SaveChangesAsync();
			}

			context.EmployeeLoanPaids.AddRange(entitys);
			return await context.SaveChangesAsync();
		}


        public async Task<int> UpdateLoanPaid(EmployeeLoanPaid model)
        {
            string loanId = model.Id;
            string employeeBasicInfoId = model.EmployeeBasicInfoId;
            var employeeId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(context.HttpContext);
            var employee = context.EmployeeBasicInfos.Where(x => x.Id == employeeId).FirstOrDefault();
            var legalEntity = context.LegalEntity.Where(x => x.Id == employee.LegalEntityId).Select(x=>x.LegalEntityName).FirstOrDefault();
            var existVal = context.EmployeeLoanPaids.Where(x => x.Id == loanId && x.EmployeeBasicInfoId == employeeBasicInfoId).FirstOrDefault();
            if (existVal != null)
            {
                if (model.ValuePaid == model.Value)
                {
                    existVal.Status = 2;
                    existVal.EarlyPaidDate = DateTime.Now;
                    existVal.Log = "Update By " + employee.NameEmployee + " (" + legalEntity + ") - " + DateTime.Now.ToString("dd MMMM yyyy hh:mm:ss");
                    await context.SaveChangesAsync();
                    return 1;
                }
                else
                {
                    existVal.Log = "Nominal yang dibayarkan di early paid tidak sesuai.";
                }
            }
            else
            {
                existVal.Log = "Data tidak ditemukan.";
            }
            await context.SaveChangesAsync();
            return 0;
            
        }
		public async Task<List<EmployeeLoanPaid>> GetLoanPaidList(string Id)
        {
            var result = await (from a in context.EmployeeLoans
                                join c in context.EmployeeLoanPaids on a.Id equals c.EmployeeLoanId
                                where !(a.IsDeleted ?? false) && !(c.IsDeleted ?? false) && a.Id == Id
                                select new EmployeeLoanPaid
                                {
                                    Id = c.Id,
                                    EmployeeLoanId = c.EmployeeLoanId,
                                    EmployeeBasicInfoId = c.EmployeeBasicInfoId,
                                    DatePaid = c.DatePaid,
                                    Value = c.Value,
                                    Status = c.Status,
                                    EarlyPaidDate = c.EarlyPaidDate,
                                    CreatedBy = c.CreatedBy,
                                    CreatedOn = c.CreatedOn,
                                    ModifiedBy = c.ModifiedBy,
                                    ModifiedOn = c.ModifiedOn,
                                    ApprovedBy = c.ApprovedBy,
                                    ApprovedOn = c.ApprovedOn,
                                    IsActive = c.IsActive,
                                    IsLocked = c.IsLocked,
                                    IsDefault = c.IsDefault,
                                    IsDeleted = c.IsDeleted,
                                    OwnerId = c.OwnerId,
                                    DeletedBy = c.DeletedBy,
                                    DeletedOn = c.DeletedOn,
                                    DatePaidString = c.DatePaid.Value.ToString("MMMM yyyy"),
                                    Log = c.Log,
                                    ValuePaid = c.EarlyPaidDate == null ? null : c.Value
                                }).OrderBy(x => x.DatePaid).ToListAsync();
            return result;
        }

    }
}