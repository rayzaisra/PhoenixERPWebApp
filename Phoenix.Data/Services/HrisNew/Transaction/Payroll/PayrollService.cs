using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.RestApi;
using Phoenix.Shared.Core.Contexts;

namespace Phoenix.Data
{
    public interface IPayrollService
    {
        Task<SalaryProcessHeader> Generate(int month, int year);
        Task<int> AddAsync(SalaryProcessHeader entity);
        Task<int> EditAsync(SalaryProcessHeader entity);
        Task<int> ApproveAsync(SalaryProcessHeader entity);
        Task<List<SalaryProcessHeader>> Get();
        Task<List<SalaryProcessHeader>> GetApprovalList();
        Task<DataTable> Get(int month, int year);
        Task<SalaryProcessHeader> Get(string Id);
        Task<int> EncryptPayroll();
        Task<int> DecryptPayroll();
    }

    public class PayrollService : IPayrollService
    {
        readonly DataContext context;
        readonly GlobalFunctionApproval globalFcApproval;
        readonly Services.Um.IManageMenuService menuService;

        /// <summary>
        /// And endpoint to manage EmployeeLoan
        /// </summary>
        /// <param name="context">Database context</param>
        public PayrollService(DataContext context, GlobalFunctionApproval globalFcApproval, Services.Um.IManageMenuService menuService)
        {
            this.context = context;
            this.globalFcApproval = globalFcApproval;
            this.menuService = menuService;
        }

        public async Task<int> AddAsync(SalaryProcessHeader entity)
        {
            return await AddEdit(entity, false);
        }

        public async Task<int> EditAsync(SalaryProcessHeader entity)
        {
            return await AddEdit(entity, true);
        }
        private async Task<int> AddEdit(SalaryProcessHeader entity, bool isEdit)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (isEdit)
                    {
                        if (string.IsNullOrEmpty(entity.Code)) entity.Code = await TransID.GetTransId(context, Code.Payroll, DateTime.Today);
                        context.PhoenixEdit(entity);
                    }
                    else
                    {
                        entity.Id = await TransID.GetTransId(context, Code.Payroll, DateTime.Today);
                        entity.Code = entity.Id;
                        await context.PhoenixAddAsync(entity);
                        await context.SaveChangesAsync();
                    }

                    var isApprove = true;
                    if (entity.Status != StatusTransaction.Draft)
                    {
                        var dataMenu = await menuService.GetByUniqueName(MenuUnique.Payroll).ConfigureAwait(false);
                        if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
                        var vm = new Models.ViewModel.TransApprovalHrisVm()
                        {
                            MenuId = dataMenu.Id,
                            RefId = entity.Id,
                            DetailLink = $"{ApprovalLink.Payroll}?Id={entity.Id}",
                            Module = NotificationModuleType.ModuleHRIS,
                            NotificationType = NotificationTypeCode.PayrollNotification
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

        public async Task<int> ApproveAsync(SalaryProcessHeader entity)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var save = await globalFcApproval.UnsafeProcessApproval(entity.Id, entity.Status, entity.RemarkRejected, entity);
                    var approval = context.TrTemplateApprovals.Where(x => x.RefId == entity.Id && !(x.IsDeleted ?? false)).FirstOrDefault();
                    if (approval != null)
                    {
                        if (approval.StatusApproved == Models.Um.StatusApproved.Approved)
                        {
                            //proses update utang / loan ketika proses generate payroll di approval terakhir
                            var loan = await (from a in context.EmployeeLoanPaids
                                              join b in context.SalaryProcesss on a.EmployeeBasicInfoId equals b.EmployeeBasicInfoId
                                              where Convert.ToDateTime(a.DatePaid).Year == entity.Year && Convert.ToDateTime(a.DatePaid).Month == entity.Month
                                              select a).ToListAsync();

                            Parallel.ForEach(loan, (item) =>
                            {
                                item.Status = LoanStatus.Paid;
                                item.EarlyPaidDate = DateTime.Now;
                            });

                            context.PhoenixEditRange(loan.ToArray());
                        }
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

        public async Task<List<SalaryProcessHeader>> GetApprovalList()
        {
            var result = await (from a in context.SalaryProcessHeaders
                                join b in context.EmployeeBasicInfos on a.CreatedBy equals b.Id
                                join c in context.TrTemplateApprovals on a.Id equals c.RefId
                                join d in context.TrUserApprovals on c.Id equals d.TrTempApprovalId
                                where !(a.IsDeleted ?? false) && !(c.IsDeleted ?? false) && a.IsTmp == false &&
                                d.StatusApproved == Models.Um.StatusApproved.CurrentApproval && d.EmployeeBasicInfoId == context.GetEmployeeId()
                                select new SalaryProcessHeader
                                {
                                    Id = a.Id,
                                    Code = a.Code,
                                    Year = a.Year,
                                    Month = a.Month,
                                    Status = c.StatusApproved.GetHashCode(),
                                    StatusDesc = c.StatusApprovedDescription,
                                    RequestBy = b.NameEmployee,
                                    CreatedOn = a.CreatedOn
                                }).OrderByDescending(x => x.CreatedOn).ToListAsync();
            return result;
        }

        public async Task<List<SalaryProcessHeader>> Get()
        {
            var result = await (from a in context.SalaryProcessHeaders
                                join b in context.EmployeeBasicInfos on a.CreatedBy equals b.Id
                                join c in context.TrTemplateApprovals on a.Id equals c.RefId
                                where !(a.IsDeleted ?? false) && !(c.IsDeleted ?? false) && a.IsTmp == false
                                select new SalaryProcessHeader
                                {
                                    Id = a.Id,
                                    Code = a.Code,
                                    Year = a.Year,
                                    Month = a.Month,
                                    Status = c.StatusApproved.GetHashCode(),
                                    StatusDesc = c.StatusApprovedDescription,
                                    RequestBy = b.NameEmployee,
                                    CreatedOn = a.CreatedOn
                                }).ToListAsync();


            var result2 = await (from a in context.SalaryProcessHeaders
                                 join b in context.EmployeeBasicInfos on a.CreatedBy equals b.Id
                                 where !(a.IsDeleted ?? false) && a.IsTmp == false && !context.TrTemplateApprovals.Any(x => x.RefId == a.Id)
                                 select new SalaryProcessHeader
                                 {
                                     Id = a.Id,
                                     Code = a.Code,
                                     Year = a.Year,
                                     Month = a.Month,
                                     Status = StatusTransaction.Draft,
                                     StatusDesc = StatusTransaction.StatusName(StatusTransaction.Draft),
                                     RequestBy = b.NameEmployee,
                                     CreatedOn = a.CreatedOn
                                 }).ToListAsync();

            if (result == null) result = new List<SalaryProcessHeader>();
            result.AddRange(result2);

            return result.OrderByDescending(x => x.CreatedOn).ToList();
        }


        public async Task<DataTable> Get(int month, int year)
        {
            var data = await context.EmployeeSubcomponentAdditionals.Where(x => x.AdditionalComponentId == "ab4cf509-88ee-4c35-8083-67e305a22719" && x.IsDeleted == false).ToListAsync();
            foreach (var item in data)
            {
                var paramType1 = new SqlParameter[] {
                                new SqlParameter("@employeeid", item.AdditionalEmployeeBasicInfoId),
                                new SqlParameter("@month", month),
                                new SqlParameter("@year", year)
                            };
                var res = await context.ExecuteStoredProcedure("uspCalculatePph21", paramType1);
            }


            var paramType = new SqlParameter[] {
                                new SqlParameter("@Month", month),
                                new SqlParameter("@Year", year)
                            };

            var result = await context.ExecuteStoredProcedure("uspGetGeneratePayrollList_NEW", paramType);
            return result;
        }

        public async Task<int> DecryptPayroll()
        {
            var save = 1;
            try
            {
                var dataPayroll1 = await context.EmployeeSubcomponentAdditionals.Where(x => x.IsEncrypt == true && x.IsDeleted == false).ToListAsync();
                foreach (var item in dataPayroll1)
                {
                    item.AdditionalAmount = Phoenix.ApiExtension.Helpers.SecurityHelper.DecryptString(item.AdditionalAmount);
                    item.AdditionalPercent = Phoenix.ApiExtension.Helpers.SecurityHelper.DecryptString(item.AdditionalPercent);

                    var commandText = @"UPDATE hr.employee_subcomponent_additional set additional_percent = @1, additional_amount = @2, is_encrypt = 0 where id = @3 ";
                    var percent = new SqlParameter("@1", item.AdditionalPercent);
                    var amount = new SqlParameter("@2", item.AdditionalAmount);
                    var id = new SqlParameter("@3", item.Id);

                   await context.Database.ExecuteSqlCommandAsync(commandText, percent, amount, id);

                }

                var dataPayroll2 = await context.EmployeeSubcomponentDeductions.Where(x => x.IsEncrypt == true && x.IsDeleted == false).ToListAsync();
                foreach (var item in dataPayroll2)
                {
                    item.Percent = Phoenix.ApiExtension.Helpers.SecurityHelper.DecryptString(item.Percent);
                    item.Amount = Phoenix.ApiExtension.Helpers.SecurityHelper.DecryptString(item.Amount);

                    var commandText = @"UPDATE hr.employee_subcomponent_deduction set deduction_percent = @1, deduction_amount = @2, is_encrypt = 0 where id = @3";
                    var percent = new SqlParameter("@1", item.Percent);
                    var amount = new SqlParameter("@2", item.Amount);
                    var id = new SqlParameter("@3", item.Id);

                    await context.Database.ExecuteSqlCommandAsync(commandText, percent, amount, id);

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return save;

        }

        public async Task<int> EncryptPayroll()
        {
            var save = 1;
            try
            {
                var dataPayroll1 = await context.EmployeeSubcomponentAdditionals.Where(x => x.IsEncrypt == false && x.IsDeleted == false).ToListAsync();

                foreach (var item in dataPayroll1)
                {
                    item.AdditionalAmount = Phoenix.ApiExtension.Helpers.SecurityHelper.EncryptString(item.AdditionalAmount);
                    item.AdditionalPercent = Phoenix.ApiExtension.Helpers.SecurityHelper.EncryptString(item.AdditionalPercent);

                    var commandText = @"UPDATE hr.employee_subcomponent_additional set additional_percent = @1, additional_amount = @2, is_encrypt = 1 where id = @3 ";
                    var percent = new SqlParameter("@1", item.AdditionalPercent);
                    var amount = new SqlParameter("@2", item.AdditionalAmount);
                    var id = new SqlParameter("@3", item.Id);

                    await context.Database.ExecuteSqlCommandAsync(commandText, percent, amount, id);
                }

                var dataPayroll2 = await context.EmployeeSubcomponentDeductions.Where(x => x.IsEncrypt == false && x.IsDeleted == false).ToListAsync();
                foreach (var item in dataPayroll2)
                {
                    item.Percent = Phoenix.ApiExtension.Helpers.SecurityHelper.EncryptString(item.Percent);
                    item.Amount = Phoenix.ApiExtension.Helpers.SecurityHelper.EncryptString(item.Amount);

                    var commandText = @"UPDATE hr.employee_subcomponent_deduction set deduction_percent = @1, deduction_amount = @2, is_encrypt = 1 where id = @3";
                    var percent = new SqlParameter("@1", item.Percent);
                    var amount = new SqlParameter("@2", item.Amount);
                    var id = new SqlParameter("@3", item.Id);

                    await context.Database.ExecuteSqlCommandAsync(commandText, percent, amount, id);

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
                

            return save;

        }

        public async Task<SalaryProcessHeader> Generate(int month, int year)
        {
            var result = await (from a in context.SalaryProcessHeaders
                                join b in context.EmployeeBasicInfos on a.CreatedBy equals b.Id
                                join c in context.TrTemplateApprovals on a.Id equals c.RefId
                                where !(a.IsDeleted ?? false) && !(c.IsDeleted ?? false) && a.IsTmp == false
                                select new SalaryProcessHeader
                                {
                                    Id = a.Id,
                                    Code = a.Code,
                                    Year = a.Year,
                                    Month = a.Month,
                                    Status = c.StatusApproved.GetHashCode(),
                                    StatusDesc = c.StatusApprovedDescription,
                                    RequestBy = b.NameEmployee,
                                    CreatedOn = a.CreatedOn
                                }).ToListAsync();

            var result2 = await (from a in context.SalaryProcessHeaders
                                 join b in context.EmployeeBasicInfos on a.CreatedBy equals b.Id
                                 where !(a.IsDeleted ?? false) && a.IsTmp == false && !context.TrTemplateApprovals.Any(x => x.RefId == a.Id)
                                 select new SalaryProcessHeader
                                 {
                                     Id = a.Id,
                                     Code = a.Code,
                                     Year = a.Year,
                                     Month = a.Month,
                                     Status = StatusTransaction.Draft,
                                     StatusDesc = StatusTransaction.StatusName(StatusTransaction.Draft),
                                     RequestBy = b.NameEmployee,
                                     CreatedOn = a.CreatedOn
                                 }).ToListAsync();

            if (result == null) result = new List<SalaryProcessHeader>();
            result.AddRange(result2);



            if (result.Where(x => x.Month == month && x.Year == year && x.Status == StatusTransaction.Approved).Count() > 0)
            {
                throw new Exception("for " + new DateTime(year, month, 1).ToString("MMMM", CultureInfo.InvariantCulture) + " " + year.ToString() + " is already approved.");
            }

            if (result.Where(x => x.Month == month && x.Year == year  && (x.Status == StatusTransaction.Draft ||  x.Status == StatusTransaction.WaitingApproval)).Count() > 0)
            {
                throw new Exception("for " + new DateTime(year, month, 1).ToString("MMMM", CultureInfo.InvariantCulture) + " " + year.ToString() + " there is already a transaction");
            }

            //if (context.SalaryProcessHeaders.Where(x => x.Month == month && x.Year == year && (x.Status == StatusTransaction.Draft || x.Status == StatusTransaction.Rejected || x.Status == StatusTransaction.Approved)).Count() > 0)
            //    throw new Exception("for " + new DateTime(year, month, 1).ToString("MMMM", CultureInfo.InvariantCulture) + " " + year.ToString() + " there is already a transaction");


            var decrypt = await DecryptPayroll();

            var paramType = new SqlParameter[] {
                                new SqlParameter("@Month", month),
                                new SqlParameter("@Year", year),
                                new SqlParameter("@RequestBy", context.GetEmployeeId())
                            };
            var process = await context.SalaryProcessHeaders.FromSql("EXEC uspGeneratePayroll @Month, @Year, @RequestBy", paramType).FirstOrDefaultAsync();

           

            return process;
        }
        public async Task<SalaryProcessHeader> Get(string Id)
        {
            var result = await context.SalaryProcessHeaders.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
            var approval = await context.TrTemplateApprovals.Where(x => x.RefId == Id && !(x.IsDeleted ?? false)).FirstOrDefaultAsync();
            if (approval == null) { result.Status = StatusTransaction.Draft; } else { result.Status = approval.StatusApproved.GetHashCode(); }
            return result;
        }
    }
}