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
using Phoenix.Data.Models.Enum;
using Phoenix.Data.RestApi;
using Phoenix.Shared.Core.Contexts;

namespace Phoenix.Data
{
    public interface IFinancePeriodService : IDataService<FinancePeriod>
    {
        Task<int> ApproveAsync(FinancePeriod entity);
        //DateTime? MaxDate();
    }

    public class FinancePeriodService : IFinancePeriodService
    {
        readonly DataContext context;
        readonly GlobalFunctionApproval globalFcApproval;
        readonly Services.Um.IManageMenuService menuService;
        readonly StatusLogService log;

        /// <summary>
        /// And endpoint to manage FinancePeriod
        /// </summary>
        /// <param name="context">Database context</param>
        public FinancePeriodService(DataContext context, GlobalFunctionApproval globalFcApproval, Services.Um.IManageMenuService menuService, StatusLogService log)
        {
            this.context = context;
            this.globalFcApproval = globalFcApproval;
            this.menuService = menuService;
            this.log = log;
        }

        public async Task<int> AddAsync(FinancePeriod entity)
        {
            return await AddEdit(entity, false);
        }

        public Task<int> AddRangeAsync(params FinancePeriod[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(FinancePeriod entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params FinancePeriod[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(FinancePeriod entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params FinancePeriod[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(FinancePeriod entity)
        {
            return await AddEdit(entity, true);
        }

        public Task<int> EditRangeAsync(FinancePeriod entity)
        {
            throw new NotImplementedException();
        }

        private async Task<int> AddEdit(FinancePeriod entity, bool isEdit)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    entity.Status = StatusTransaction.StatusName(entity.StatusApproval);
                    //entity.EndDate = entity.EndDate.Date.AddMonths(1).AddDays(-1);

                    var isAlready = false;
                    if (isEdit)
                    {
                        isAlready = context.FinancePeriods.AsNoTracking()
                                .Where(x => !(x.IsDeleted ?? false)
                                        && x.FiscalYearId == entity.FiscalYearId
                                        && x.LegalEntityId == entity.LegalEntityId
                                        && ((entity.StartDate.Date >= x.StartDate.Date && entity.StartDate.Date <= x.EndDate.Date) || (entity.EndDate.Date >= x.StartDate.Date && entity.EndDate.Date <= x.EndDate.Date))
                                        && x.Id != entity.Id).Count() > 0;
                    }
                    else
                    {
                        isAlready = context.FinancePeriods.AsNoTracking()
                              .Where(x => !(x.IsDeleted ?? false)
                                    && x.FiscalYearId == entity.FiscalYearId
                                    && x.LegalEntityId == entity.LegalEntityId
                                    && ((entity.StartDate.Date >= x.StartDate.Date && entity.StartDate.Date <= x.EndDate.Date) || (entity.EndDate.Date >= x.StartDate.Date && entity.EndDate.Date <= x.EndDate.Date))
                                    ).Count() > 0;
                    }
                    if (isAlready)
                    {
                        var legal = await context.LegalEntity.Where(x => !(x.IsDeleted ?? false) && x.Id == entity.LegalEntityId).FirstOrDefaultAsync();
                        throw new Exception($"legal entity {legal.LegalEntityName} for date {entity.StartDate.ToString("yyyy MMMM dd")} s/d {entity.EndDate.ToString("yyyy MMMM dd")} already exists, please select a legal entity in a different range period!");
                    }

                    if (isEdit)
                    {
                        context.PhoenixEdit(entity);
                    }
                    else
                    {
                        if (context.FinancialYears.Where(x => x.StartDate.Year == entity.StartDate.Year && x.LegalEntityId == entity.LegalEntityId && x.Status == StatusTransactionName.Close).Count() > 0)
                        {
                            var legal = await context.LegalEntity.Where(x => !(x.IsDeleted ?? false) && x.Id == entity.LegalEntityId).FirstOrDefaultAsync();
                            throw new Exception($"for {entity.StartDate.Year} the legal entity {legal.LegalEntityName} cannot input period because the fiscal year status is closed");
                        }

                        entity.Id = await TransID.GetTransId(context, Code.FinancePeriod, DateTime.Today);
                        entity.FinancePeriodNumber = entity.Id;
                        await context.PhoenixAddAsync(entity);
                        await context.SaveChangesAsync();
                    }

                    var isApprove = true;
                    if (entity.Status != StatusTransactionName.Draft)
                    {
                        await globalFcApproval.clearApproval(entity.Id);

                        var dataMenu = await menuService.GetByUniqueName(MenuUnique.FinancePeriod).ConfigureAwait(false);
                        if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
                        var vm = new Models.ViewModel.TransApprovalHrisVm()
                        {
                            MenuId = dataMenu.Id,
                            RefId = entity.Id,
                            DetailLink = $"{ApprovalLink.FinancePeriod}?Id={entity.Id}"
                        };
                        var subGroupId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessSubGroup(context.HttpContext);
                        var divisionId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessUnitDivisi(context.HttpContext);
                        var employeeId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(context.HttpContext);
                        isApprove = await globalFcApproval.Save(vm, subGroupId, divisionId, employeeId);
                        if (isApprove)
                        {
                            await log.AddAsync(new StatusLog() { TransactionId = entity.Id, Status = entity.Status, Description = entity.RemarksProcess });
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
                        entity.Status = StatusTransactionName.Draft;
                        context.FinancePeriods.Update(entity);
                        await log.AddAsync(new StatusLog() { TransactionId = entity.Id, Status = entity.Status, Description = entity.RemarksProcess });
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

        public async Task<int> ApproveAsync(FinancePeriod entity)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var save = await globalFcApproval.UnsafeProcessApproval(entity.Id, entity.StatusApproval, entity.RemarksProcess, entity);
                    var approval = context.TrTemplateApprovals.Where(x => x.RefId == entity.Id && !(x.IsDeleted ?? false)).FirstOrDefault();
                    if (approval != null)
                    {
                        entity.Status = StatusTransaction.StatusName(approval.StatusApproved.GetHashCode());
                        if (approval.StatusApproved == Models.Um.StatusApproved.Approved)
                        {
                            if ((entity.IsClose ?? false))
                            {
                                entity.Status = StatusTransactionName.Close;
                            }
                            else
                            {
                                entity.Status = StatusTransactionName.Draft;
                            }
                        }
                        //else if (approval.StatusApproved == Models.Um.StatusApproved.Rejected)
                        //{
                        //    entity.Status = StatusTransactionName.Draft;
                        //}
                    }
                    else
                    {
                        entity.Status = StatusTransactionName.Draft;
                    }
                    context.FinancePeriods.Update(entity);

                    await log.AddAsync(new StatusLog() { TransactionId = entity.Id, Status = entity.Status, Description = entity.RemarksProcess });
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

        public async Task<List<FinancePeriod>> Get()
        {
            var result = await (from a in context.FinancePeriods
                                join b in context.LegalEntity on a.LegalEntityId equals b.Id
                                where !(a.IsDeleted ?? false)
                                select new FinancePeriod
                                {
                                    Id = a.Id,
                                    FinancePeriodName = a.FinancePeriodName,
                                    FinancePeriodNumber = a.FinancePeriodNumber,
                                    StartDate = a.StartDate,
                                    EndDate = a.EndDate,
                                    LegalEntityName = b.LegalEntityName,
                                    Status = a.Status,
                                    CreatedBy = a.CreatedBy
                                }).ToListAsync();
            return result;
        }

        public Task<List<FinancePeriod>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<FinancePeriod> Get(string Id) => await context.FinancePeriods.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();

        //public DateTime? MaxDate()
        //{
        //    var max = context.FinancePeriods.Max(x => x.EndDate);
        //    if (max == null)
        //    {
        //        max = DateTime.Now.AddYears(-1);
        //    }
        //    return max;
        //}

        public async Task IsClosePeriod(DateTime date, string LegalEntityId, FinancePeriodCode code)
        {
            var entityName = await context.LegalEntity.AsNoTracking().Where(x => x.Id == LegalEntityId).Select(x => x.LegalEntityName).FirstOrDefaultAsync();
            var year = await context.FinancialYears.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && date.Date >= x.StartDate.Date && date.Date <= x.EndDate.Date && x.LegalEntityId == LegalEntityId).ToListAsync();
            if (year.Count() == 0)
            {
                throw new Exception("Fiscal year for " + date.ToString("MMMM dd yyyy") + " and legal entity " + entityName + " not found");
            }
            else if (year.Where(x => x.Status == StatusTransactionName.Close).Count() == 0)
            {
                var month = await context.FinancePeriods.Where(x => !(x.IsDeleted ?? false) && date.Date >= x.StartDate.Date && date.Date <= x.EndDate.Date && x.LegalEntityId == LegalEntityId).ToListAsync();
                if (month.Count() == 0)
                {
                    throw new Exception("Periodic for " + date.ToString("MMMM dd yyyy") + " and legal entity " + entityName + " not found");
                }
                else
                {
                    if (month.Where(x => x.Status == StatusTransactionName.Close).Count() > 0)
                    {
                        throw new Exception("Periodic for " + date.ToString("MMMM dd yyyy") + "and legal entity " + entityName + " Is Closed");
                    }
                    else
                    {
                        var periodNumber = month.Select(x => x.FinancePeriodNumber.ToUpper()).FirstOrDefault() ?? "";
                        if (code == FinancePeriodCode.AP)
                        {
                            if (month.Where(x => x.Ap == false).Count() > 0)
                            {
                                throw new Exception("Periodic for Account Payable with period number " + periodNumber + " is not active");
                            }
                        }
                        else if (code == FinancePeriodCode.AR)
                        {
                            if (month.Where(x => x.Ar == false).Count() > 0)
                            {
                                throw new Exception("Periodic for Account Receivable with period number " + periodNumber + " is not active");
                            }
                        }
                        else if (code == FinancePeriodCode.BP)
                        {
                            if (month.Where(x => x.Bp == false).Count() > 0)
                            {
                                throw new Exception("Periodic for Bank Payment with period number " + periodNumber + " is not active");
                            }
                        }
                        else if (code == FinancePeriodCode.BR)
                        {
                            if (month.Where(x => x.Br == false).Count() > 0)
                            {
                                throw new Exception("Periodic for Bank Received with period number " + periodNumber + " is not active");
                            }
                        }
                        else if (code == FinancePeriodCode.PCS)
                        {
                            if (month.Where(x => x.Pcs == false).Count() > 0)
                            {
                                throw new Exception("Periodic for Petty Cash Settlement with period number " + periodNumber + " is not active");
                            }
                        }
                        else if (code == FinancePeriodCode.SPR)
                        {
                            if (month.Where(x => x.Cas == false).Count() > 0)
                            {
                                throw new Exception("Periodic for Request Payment Salary with period number " + periodNumber + " is not active");
                            }
                        }
                        else if (code == FinancePeriodCode.JE)
                        {
                            if (month.Where(x => x.Je == false).Count() > 0)
                            {
                                throw new Exception("Periodic for Journal entry with period number " + periodNumber + " is not active");
                            }
                        }
                        else if (code == FinancePeriodCode.JA)
                        {
                            if (month.Where(x => x.Ja == false).Count() > 0)
                            {
                                throw new Exception("Periodic for Journal Adjusment with period number " + periodNumber + " is not active");
                            }
                        }
                        else if (code == FinancePeriodCode.AMOR)
                        {
                            if (month.Where(x => x.Amor == false).Count() > 0)
                            {
                                throw new Exception("Periodic for Amortization with period number " + periodNumber + " is not active");
                            }
                        }
                        else if (code == FinancePeriodCode.SA)
                        {
                            if (month.Where(x => x.Sa == false).Count() > 0)
                            {
                                throw new Exception("Periodic for Salary Allocation with period number " + periodNumber + " is not active");
                            }
                        }
                        else if (code == FinancePeriodCode.REV)
                        {
                            if (month.Where(x => x.Dep == false).Count() > 0)
                            {
                                throw new Exception("Periodic for Revaluation with period number " + periodNumber + " is not active");
                            }
                        }
                        else if (code == FinancePeriodCode.JC)
                        {
                            if (month.Where(x => x.Jc == false).Count() > 0)
                            {
                                throw new Exception("Periodic for Job Closing with period number " + periodNumber + " is not active");
                            }
                        }
                    }
                }
            }

            if (year.Where(x => x.Status == StatusTransactionName.Close).Count() > 0)
            {
                throw new Exception("Fiscal Year for " + date.ToString("MMMM dd yyyy") + " and legal entity " + entityName + " Is Closed");
            }
        }
    }
}