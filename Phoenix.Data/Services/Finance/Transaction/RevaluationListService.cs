using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.RestApi;
using Phoenix.Data.Services.Um;
using Phoenix.Shared.Core.Contexts;

namespace Phoenix.Data
{
    public interface IRevaluationListService
    {
        Task<List<RevaluationList>> Get();
        Task<RevaluationList> Get(string Id);
        Task<int> AddAsync(RevaluationList entity);
        Task<int> EditAsync(RevaluationList entity);
        Task<int> Approve(RevaluationList entity);
        Task<int> Reject(RevaluationList entity);
        Task<int> DeleteSoftAsync(RevaluationList entity);
    }

    public class RevaluationListService : IRevaluationListService
    {
        readonly DataContext context;
        readonly GlobalFunctionApproval globalFcApproval;
        readonly IManageMenuService menuService;
        readonly StatusLogService log;
        readonly FinancePeriodService financePeriod;
        private readonly IManageTransactionApprovalService trApprovalService;
        private readonly IManageTemplateApprovalService trTemplateApprovalService;

        /// <summary>
        /// And endpoint to manage RevaluationList
        /// </summary>
        /// <param name="context">Database context</param>
        public RevaluationListService(DataContext context, GlobalFunctionApproval globalFcApproval, IManageMenuService menuService, StatusLogService log, FinancePeriodService financePeriod, IManageTransactionApprovalService trApprovalService, IManageTemplateApprovalService trTemplateApprovalService)
        {
            this.context = context;
            this.globalFcApproval = globalFcApproval;
            this.menuService = menuService;
            this.log = log;
            this.financePeriod = financePeriod;
            this.trApprovalService = trApprovalService;
            this.trTemplateApprovalService = trTemplateApprovalService;
        }


        public async Task<List<RevaluationList>> Get()
        {
            var result = await (from a in context.RevaluationLists
                                join b in context.LegalEntity on a.LegalEntityId equals b.Id
                                join c in context.Currencies on a.CurrencyId equals c.Id
                                where !(a.IsDeleted ?? false)
                                select new RevaluationList
                                {
                                    Id = a.Id,
                                    Code = a.Code,
                                    LegalEntityId = a.LegalEntityId,
                                    CurrencyId = a.CurrencyId,
                                    RevaluationDate = a.RevaluationDate,
                                    ExchangeRate = a.ExchangeRate,
                                    Status = a.Status,
                                    CreatedBy = a.CreatedBy,
                                    CreatedOn = a.CreatedOn,
                                    LegalEntityName = b.LegalEntityName,
                                    CurrencyName = c.CurrencyName,
                                    CurrencyCode = c.CurrencyCode,
                                    CurrencySymbol = c.CurrencySymbol
                                }).OrderByDescending(x => x.CreatedOn).ToListAsync();
            return result;
        }

        public async Task<RevaluationList> Get(string Id)
        {
            var result = await (from a in context.RevaluationLists
                                join b in context.LegalEntity on a.LegalEntityId equals b.Id
                                join c in context.Currencies on a.CurrencyId equals c.Id
                                where !(a.IsDeleted ?? false) && a.Id == Id
                                select new RevaluationList
                                {
                                    Id = a.Id,
                                    Code = a.Code,
                                    LegalEntityId = a.LegalEntityId,
                                    CurrencyId = a.CurrencyId,
                                    RevaluationDate = a.RevaluationDate,
                                    ExchangeRate = a.ExchangeRate,
                                    Status = a.Status,
                                    CreatedBy = a.CreatedBy,
                                    CreatedOn = a.CreatedOn,
                                    LegalEntityName = b.LegalEntityName,
                                    CurrencyName = c.CurrencyName,
                                    CurrencyCode = c.CurrencyCode,
                                    CurrencySymbol = c.CurrencySymbol
                                }).FirstOrDefaultAsync();
            return result;
        }

        public async Task<int> AddAsync(RevaluationList entity)
        {
            return await AddEdit(entity, false);
        }

        public async Task<int> EditAsync(RevaluationList entity)
        {
            return await AddEdit(entity, true);
        }

        public async Task<int> Approve(RevaluationList model)
        {
            var yearOpen = context.FinancialYears.Where(x => int.Parse(x.FinancialYearName) < model.RevaluationDate.Value.Year && x.LegalEntityId == model.LegalEntityId && x.Status != StatusTransactionName.Close);
            if (yearOpen.Count() > 0)
            {
                var legalEntityName = await context.LegalEntity.Where(x => x.Id == model.LegalEntityId).Select(x => x.LegalEntityName).FirstOrDefaultAsync();
                throw new Exception($"cannot carry out the revaluation process because in {(yearOpen.Max(x => int.Parse(x.FinancialYearName))).ToString()} and the legal entity  {(legalEntityName ?? "")}  has not been closed.");
            }

            var yearClose = context.FinancialYears.Where(x => int.Parse(x.FinancialYearName) >= model.RevaluationDate.Value.Year && x.LegalEntityId == model.LegalEntityId && x.Status == StatusTransactionName.Close);
            if (yearClose.Count() > 0)
            {
                var legalEntityName = await context.LegalEntity.Where(x => x.Id == model.LegalEntityId).Select(x => x.LegalEntityName).FirstOrDefaultAsync();
                throw new Exception($"cannot carry out the revaluation process because in {(yearClose.Min(x => int.Parse(x.FinancialYearName))).ToString()} and the legal entity  {(legalEntityName ?? "")}  has been closed.");
            }

            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (model.RevaluationDate != null)
                    {
                        model.RevaluationDate = Convert.ToDateTime($"{model.RevaluationDate.Value.Year}/{model.RevaluationDate.Value.Month}/1").AddMonths(1).AddDays(-1);
                    }

                    var save = await globalFcApproval.UnsafeProcessApproval(model.Id, StatusTransaction.Approved, model.LogDescription, model);
                    var approval = context.TrTemplateApprovals.AsNoTracking().Where(x => x.RefId == model.Id && !(x.IsDeleted ?? false)).FirstOrDefault();
                    if (approval != null)
                    {
                        model.Status = StatusTransactionName.WaitingApproval;

                        if (approval.StatusApproved == Models.Um.StatusApproved.Approved)
                        {
                            model.Status = StatusTransactionName.Approved;
                            await financePeriod.IsClosePeriod(Convert.ToDateTime(model.RevaluationDate), model.LegalEntityId, Models.Enum.FinancePeriodCode.REV);
                            var param = new SqlParameter[] { new SqlParameter("@p_revaluation_id", model.Id) };
                            await context.Database.ExecuteSqlCommandAsync("EXECUTE fn.create_jurnal_revaluation @p_revaluation_id", param);
                        }

                        context.PhoenixEdit(model);
                        await log.AddAsync(new StatusLog() { TransactionId = model.Id, Status = model.Status, Description = model.LogDescription });
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

        public async Task<int> Reject(RevaluationList model)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (model.RevaluationDate != null)
                    {
                        model.RevaluationDate = Convert.ToDateTime($"{model.RevaluationDate.Value.Year}/{model.RevaluationDate.Value.Month}/1").AddMonths(1).AddDays(-1);
                    }

                    var save = await globalFcApproval.UnsafeProcessApproval(model.Id, StatusTransaction.Rejected, model.LogDescription, model);
                    var approval = context.TrTemplateApprovals.AsNoTracking().Where(x => x.RefId == model.Id && !(x.IsDeleted ?? false)).FirstOrDefault();
                    if (approval != null)
                    {
                        model.Status = StatusTransactionName.WaitingApproval;
                        if (approval.StatusApproved == Models.Um.StatusApproved.Rejected)
                        {
                            model.Status = StatusTransactionName.Rejected;
                        }
                        context.PhoenixEdit(model);
                        await log.AddAsync(new StatusLog() { TransactionId = model.Id, Status = model.Status, Description = model.LogDescription });
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

        private async Task<int> AddEdit(RevaluationList model, bool isEdit)
        {
            var yearOpen = context.FinancialYears.Where(x => int.Parse(x.FinancialYearName) < model.RevaluationDate.Value.Year && x.LegalEntityId == model.LegalEntityId && x.Status != StatusTransactionName.Close);
            if (yearOpen.Count() > 0)
            {
                var legalEntityName = await context.LegalEntity.Where(x => x.Id == model.LegalEntityId).Select(x => x.LegalEntityName).FirstOrDefaultAsync();
                throw new Exception($"cannot carry out the revaluation process because in {(yearOpen.Max(x => int.Parse(x.FinancialYearName))).ToString()} and the legal entity  {(legalEntityName ?? "")}  has not been closed.");
            }

            var yearClose = context.FinancialYears.Where(x => int.Parse(x.FinancialYearName) >= model.RevaluationDate.Value.Year && x.LegalEntityId == model.LegalEntityId && x.Status == StatusTransactionName.Close);
            if (yearClose.Count() > 0)
            {
                var legalEntityName = await context.LegalEntity.Where(x => x.Id == model.LegalEntityId).Select(x => x.LegalEntityName).FirstOrDefaultAsync();
                throw new Exception($"cannot carry out the revaluation process because in {(yearClose.Min(x => int.Parse(x.FinancialYearName))).ToString()} and the legal entity  {(legalEntityName ?? "")}  has been closed.");
            }

            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (model.RevaluationDate != null)
                    {
                        model.RevaluationDate = Convert.ToDateTime($"{model.RevaluationDate.Value.Year}/{model.RevaluationDate.Value.Month}/1").AddMonths(1).AddDays(-1);
                    }

                    if (context.RevaluationLists.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.LegalEntityId == model.LegalEntityId && x.CurrencyId == model.CurrencyId && x.RevaluationDate.Value.Date == model.RevaluationDate.Value.Date && x.Id != model.Id).Count() > 0)
                    {
                        var legal = await context.LegalEntity.Where(x => !(x.IsDeleted ?? false) && x.Id == model.LegalEntityId).FirstOrDefaultAsync();
                        throw new Exception($"legal entity: {legal.LegalEntityName}, currency: {model.CurrencyName}, month & year: {model.RevaluationDate.Value.ToString("MMM yyyy")} already exists");
                    }

                    if (isEdit)
                    {
                        context.PhoenixEdit(model);
                    }
                    else
                    {
                        model.Id = await TransID.GetTransId(context, Code.Revaluation, DateTime.Today);
                        model.Code = await TransID.GetTransId(context, Code.JournalVoucher, DateTime.Today);
                        await context.PhoenixAddAsync(model);
                        await context.SaveChangesAsync();
                    }

                    var isApprove = true;
                    if (model.Status != StatusTransactionName.Draft)
                    {
                        var dataMenu = await menuService.GetByUniqueName(MenuUnique.Revaluation).ConfigureAwait(false);
                        if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
                        var vm = new Models.ViewModel.TransApprovalHrisVm()
                        {
                            MenuId = dataMenu.Id,
                            RefId = model.Id,
                            DetailLink = $"{ApprovalLink.Revaluation}?Id={model.Id}",
                            NotificationType = NotificationTypeCode.RevaluationNotification,
                            Module = NotificationModuleType.ModuleFinance
                        };
                        var subGroupId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessSubGroup(context.HttpContext);
                        var divisionId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessUnitDivisi(context.HttpContext);
                        var employeeId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(context.HttpContext);
                        isApprove = await globalFcApproval.Save(vm, subGroupId, divisionId, employeeId);
                        if (isApprove)
                        {
                            await log.AddAsync(new StatusLog() { TransactionId = model.Id, Status = model.Status, Description = model.LogDescription });
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
                        context.RevaluationLists.Update(model);
                        await log.AddAsync(new StatusLog() { TransactionId = model.Id, Status = model.Status, Description = model.LogDescription });
                        var save = await context.SaveChangesAsync();
                        transaction.Commit();
                        return save;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message.ToString());
                }
            }
        }

        public async Task<int> DeleteSoftAsync(RevaluationList entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }
    }
}