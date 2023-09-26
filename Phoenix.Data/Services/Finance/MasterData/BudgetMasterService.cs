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
using Phoenix.Data.Models.Enum;
using Phoenix.Data.Models.ViewModel;
using Phoenix.Data.RestApi;
using Phoenix.Data.Services.Um;
using Phoenix.Shared.Core.Contexts;

namespace Phoenix.Data
{
    public interface IBudgetMasterService : IDataService<BudgetMasterHeader>
    {
        Task<List<BudgetMasterHeader>> GetList(string type);
        Task<List<VwBudgetMaster>> GetBudgetMaster(string type, string budgetMasterHeaderId);
        Task<List<BudgetMasterDetail>> GetBudgetMasterDetails(string type, string costSharingAllocationId, string budgetMasterId);
        Task<dynamic> GetMasterBudgetCode(string budgetCodeId, string type);
        Task<BudgetMaster> GetBudgetMasterById(string MasterBudgetCodeId, string Id);
        Task<int> ApproveAsync(BudgetMasterHeader entity);
        Task<int> DeleteDetailSoftAsync(string masterId);
        bool IsRequestor(string type, string Id);
    }

    public class BudgetMasterService : IBudgetMasterService
    {
        readonly DataContext context;
        readonly GlobalFunctionApproval globalFcApproval;
        readonly IManageMenuService menuService;
        readonly StatusLogService log;
        readonly ManageTransactionApprovalService manageApproval;

        /// <summary>
        /// And endpoint to manage BudgetMasterHeader
        /// </summary>
        /// <param name="context">Database context</param>
        public BudgetMasterService(DataContext context, GlobalFunctionApproval globalFcApproval, IManageMenuService menuService, StatusLogService log, ManageTransactionApprovalService manageApproval)
        {
            this.context = context;
            this.globalFcApproval = globalFcApproval;
            this.menuService = menuService;
            this.log = log;
            this.manageApproval = manageApproval;
        }

        public async Task<int> AddAsync(BudgetMasterHeader entity)
        {
            return await AddEditAsync(entity, false);
        }

        public async Task<int> EditAsync(BudgetMasterHeader entity)
        {
            return await AddEditAsync(entity, true);
        }

        private async Task<int> AddEditAsync(BudgetMasterHeader entity, bool isEdit)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (!string.IsNullOrEmpty(entity.Id))
                    {
                        //if (context.BudgetMasters.Where(x => !(x.IsDeleted ?? false) && x.BudgetMasterHeaderId == entity.Id && x.Status == StatusTransactionName.Approved).Count() == 0)
                        //{
                        //    throw new Exception($"cannot change the status of open to active, inactive and close before there is an approved master budget detail.");
                        //}

                        //if (entity.BudgetMaster == null)
                        //{

                        //}
                    }

                    //if (context.BudgetMasterHeaders.AsNoTracking().Where(x => x.Id != entity.Id && x.BudgetYear == entity.BudgetYear && x.CurrencyId == entity.CurrencyId && x.Source == entity.Source).Count() > 0)
                    //{
                    //    var currency = await context.Currencies.Where(x => x.Id == entity.CurrencyId).Select(x => x.CurrencyName).FirstOrDefaultAsync() ?? "-";
                    //    throw new Exception($"for the {entity.BudgetYear} budget with the existing {currency} currency");
                    //}

                    if (context.BudgetMasterHeaders.AsNoTracking().Where(x => x.Id != entity.Id && x.BudgetYear == entity.BudgetYear && x.Source == entity.Source).Count() > 0)
                    {
                        //var currency = await context.Currencies.Where(x => x.Id == entity.CurrencyId).Select(x => x.CurrencyName).FirstOrDefaultAsync() ?? "-";
                        throw new Exception($"the budget for {entity.BudgetYear} already exists");
                    }

                    if (entity.Status == "Draft" || string.IsNullOrEmpty(entity.Status)) entity.Status = "Open";
                    if (isEdit)
                    {
                        var status = await context.BudgetMasterHeaders.AsNoTracking().Where(x => x.Id == entity.Id && x.Source == entity.Source).Select(x => x.Status).FirstOrDefaultAsync();
                        if (status == "CLOSE")
                        {
                            if (status != entity.Status) entity.Version = +1;
                        }

                        context.PhoenixEdit(entity);
                        await context.SaveChangesAsync();
                    }
                    else
                    {
                        entity.Version = 0;
                        if (entity.Source == BudgetCodeType.CAPEX)
                        {
                            entity.Id = await TransID.GetTransId(context, Code.BudgetMasterCapex, DateTime.Today);
                        }
                        else
                        {
                            entity.Id = await TransID.GetTransId(context, Code.BudgetMasterOpex, DateTime.Today);
                        }
                        await context.PhoenixAddAsync(entity);
                        await context.SaveChangesAsync();
                    }

                    if (entity.BudgetMaster != null)
                    {
                        var master = entity.BudgetMaster;
                        var statusDetail = StatusTransaction.StatusName(master.StatusApproval);
                        if (master.Status == StatusTransactionName.Rejected && statusDetail == StatusTransactionName.WaitingApproval)
                        {
                            master.Version += 1;
                        }
                        master.Status = statusDetail;
                        master.TotalBudgetBalanceAcr = master.TotalBudget - master.TotalAcrBudget;
                        master.TotalBudgetBalanceReal = master.TotalBudget - master.TotalRealBudgetAcr;
                        master.TotalBudgetBalance = master.TotalBudget - master.TotalBudgetUsed;
                        master.TotalBudgetBalanceAllTransaction = master.TotalBudget - master.TotalBudgetUsedAllTransaction;
                        //master.TotalBudgetBalance = master.TotalBudget - master.TotalAcrBudget - master.TotalRealBudgetAcr - master.TotalBudgetUsed;

                        if (master.Id != null)
                        {
                            context.PhoenixEdit(master);

                            if (entity.BudgetMasterDetail != null)
                            {
                                var detail = context.BudgetMasterDetails.AsNoTracking().Where(x => x.BudgetMasterId == master.Id).ToList();
                                if (detail.Count > 0)
                                {
                                    context.BudgetMasterDetails.RemoveRange(detail);
                                }
                            }
                        }
                        else
                        {
                            if (entity.Source == BudgetCodeType.CAPEX)
                            {
                                master.Id = await TransID.GetTransId(context, Code.BudgetMasterCapexDetail, DateTime.Today);
                            }
                            else
                            {
                                master.Id = await TransID.GetTransId(context, Code.BudgetMasterOpexDetail, DateTime.Today);
                            }
                            master.BudgetMasterHeaderId = entity.Id;
                            master.Version = 0;
                            await context.PhoenixAddAsync(master);
                            await context.SaveChangesAsync();
                        }

                        if (entity.BudgetMasterDetail != null)
                        {
                            var masterDetail = entity.BudgetMasterDetail;
                            foreach (var item in masterDetail)
                            {
                                item.Id = Guid.NewGuid().ToString();
                                item.BudgetMasterId = master.Id;
                                await context.PhoenixAddAsync(item);
                            }
                        }

                        var isApprove = true;
                        if (master.Status != StatusTransactionName.Draft)
                        {
                            var dataMenu = await menuService.GetByUniqueName(entity.Source == BudgetCodeType.CAPEX ? MenuUnique.MasterBudgetCapex : MenuUnique.MasterBudgetOpex).ConfigureAwait(false);
                            if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
                            var vm = new Models.ViewModel.TransApprovalHrisVm()
                            {
                                MenuId = dataMenu.Id,
                                RefId = master.Id,
                                DetailLink = $"{(entity.Source == BudgetCodeType.CAPEX ? ApprovalLink.MasterBudgetCapex : ApprovalLink.MasterBudgetOpex)}?Id={entity.Id}"
                            };
                            var subGroupId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessSubGroup(context.HttpContext);
                            var divisionId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessUnitDivisi(context.HttpContext);
                            var employeeId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(context.HttpContext);
                            isApprove = await globalFcApproval.Save(vm, subGroupId, divisionId, employeeId);
                            if (isApprove)
                            {
                                await log.AddAsync(new StatusLog() { TransactionId = master.Id, Status = master.Status, Description = master.RemarksProcess });

                            }
                            else
                            {
                                transaction.Rollback();
                                throw new Exception("please check the approval template that will be processed");
                            }
                        }
                        else
                        {
                            await log.AddAsync(new StatusLog() { TransactionId = master.Id, Status = master.Status, Description = master.RemarksProcess });
                        }
                    }
                    var save = await context.SaveChangesAsync();
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

        public async Task<int> ApproveAsync(BudgetMasterHeader entity)
        {
            var master = entity.BudgetMaster;
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var save = await globalFcApproval.UnsafeProcessApproval(master.Id, master.StatusApproval, master.RemarksProcess, master);
                    var approval = context.TrTemplateApprovals.Where(x => x.RefId == master.Id && !(x.IsDeleted ?? false)).FirstOrDefault();
                    if (approval != null)
                    {
                        master.Status = StatusTransaction.StatusName(approval.StatusApproved.GetHashCode());
                    }
                    else
                    {
                        master.Status = StatusTransactionName.Draft;
                    }
                    context.BudgetMasters.Update(master);

                    await log.AddAsync(new StatusLog() { TransactionId = master.Id, Status = master.Status, Description = master.RemarksProcess });
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


        public Task<int> AddRangeAsync(params BudgetMasterHeader[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(BudgetMasterHeader entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params BudgetMasterHeader[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(BudgetMasterHeader entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params BudgetMasterHeader[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteDetailSoftAsync(string masterId)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var budgetMaster = await context.BudgetMasters.AsNoTracking().Where(x => x.Id == masterId).FirstOrDefaultAsync();
                    context.PhoenixDelete(budgetMaster);

                    var param = new SqlParameter[] { new SqlParameter("@DeleteBy", context.GetEmployeeId()), new SqlParameter("@MasterId", masterId) };
                    context.Database.ExecuteSqlCommand($"UPDATE fn.budget_master_detail SET is_deleted=1, deleted_by=@DeleteBy, deleted_on= GETDATE() WHERE budget_master_id=@MasterId", param);
                    var save = await context.SaveChangesAsync();
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

        public Task<int> EditRangeAsync(BudgetMasterHeader entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<BudgetMasterHeader>> GetList(string type)
        {
            var result = await (from a in context.BudgetMasterHeaders
                                join b in context.Currencies on a.CurrencyId equals b.Id
                                where !(a.IsDeleted ?? false) && a.Source == type
                                select new BudgetMasterHeader
                                {
                                    Id = a.Id,
                                    BudgetYear = a.BudgetYear,
                                    BudgetName = a.BudgetName,
                                    PeriodeFrom = a.PeriodeFrom,
                                    PeriodeTo = a.PeriodeTo,
                                    CurrencyId = a.CurrencyId,
                                    CurrencyName = b.CurrencyName,
                                    ExchangeRate = a.ExchangeRate,
                                    Source = a.Source,
                                    Status = a.Status,
                                    Version = a.Version
                                }
                               ).ToListAsync();
            return result;
        }

        public async Task<List<VwBudgetMaster>> GetBudgetMaster(string type, string budgetMasterHeaderId)
        {
            var param = new SqlParameter[] { new SqlParameter("@Type", type), new SqlParameter("@BudgetMasterHeaderId", budgetMasterHeaderId ?? ""), new SqlParameter("@UserLogin", context.GetEmployeeId()) };
            return await context.ExecuteStoredProcedure<VwBudgetMaster>("fn.uspGetBudgetMaster", param);
        }
        public async Task<List<BudgetMasterDetail>> GetBudgetMasterDetails(string type, string costSharingAllocationId, string budgetMasterId)
        {
            var result = new List<BudgetMasterDetail>();
            if (string.IsNullOrEmpty(budgetMasterId))
            {
                result = await (from a in context.CostSharingAllocationDetails
                                join b in context.BusinessUnits on a.BusinessUnitId equals b.Id
                                where !(a.IsDeleted ?? false) && a.CostSharingAllocationId == costSharingAllocationId
                                select new BudgetMasterDetail
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    BusinessUnitId = a.BusinessUnitId,
                                    CostSharingAllocationPercent = a.CostSharingAllocationPercent ?? 0,
                                    BusinessUnitName = b.UnitName
                                }).ToListAsync();
            }
            else
            {
                result = await (from a in context.BudgetMasterDetails
                                join b in context.BusinessUnits on a.BusinessUnitId equals b.Id
                                join c in context.BudgetMasters on a.BudgetMasterId equals c.Id
                                join d in context.BudgetMasterHeaders on c.BudgetMasterHeaderId equals d.Id
                                where !(a.IsDeleted ?? false) && d.Source == type && c.Id == budgetMasterId
                                select new BudgetMasterDetail
                                {
                                    Id = a.Id,
                                    BudgetMasterId = a.BudgetMasterId,
                                    BusinessUnitId = a.BusinessUnitId,
                                    CostSharingAllocationPercent = a.CostSharingAllocationPercent,
                                    Budget01 = a.Budget01,
                                    AcrBudget01 = a.AcrBudget01,
                                    RealBudgetAcr01 = a.RealBudgetAcr01,
                                    BalBudget01 = a.BalBudget01,
                                    UsedBudget01 = a.UsedBudget01,
                                    Budget02 = a.Budget02,
                                    AcrBudget02 = a.AcrBudget02,
                                    RealBudgetAcr02 = a.RealBudgetAcr02,
                                    BalBudget02 = a.BalBudget02,
                                    UsedBudget02 = a.UsedBudget02,
                                    Budget03 = a.Budget03,
                                    AcrBudget03 = a.AcrBudget03,
                                    RealBudgetAcr03 = a.RealBudgetAcr03,
                                    BalBudget03 = a.BalBudget03,
                                    UsedBudget03 = a.UsedBudget03,
                                    Budget04 = a.Budget04,
                                    AcrBudget04 = a.AcrBudget04,
                                    RealBudgetAcr04 = a.RealBudgetAcr04,
                                    BalBudget04 = a.BalBudget04,
                                    UsedBudget04 = a.UsedBudget04,
                                    Budget05 = a.Budget05,
                                    AcrBudget05 = a.AcrBudget05,
                                    RealBudgetAcr05 = a.RealBudgetAcr05,
                                    BalBudget05 = a.BalBudget05,
                                    UsedBudget05 = a.UsedBudget05,
                                    Budget06 = a.Budget06,
                                    AcrBudget06 = a.AcrBudget06,
                                    RealBudgetAcr06 = a.RealBudgetAcr06,
                                    BalBudget06 = a.BalBudget06,
                                    UsedBudget06 = a.UsedBudget06,
                                    Budget07 = a.Budget07,
                                    AcrBudget07 = a.AcrBudget07,
                                    RealBudgetAcr07 = a.RealBudgetAcr07,
                                    BalBudget07 = a.BalBudget07,
                                    UsedBudget07 = a.UsedBudget07,
                                    Budget08 = a.Budget08,
                                    AcrBudget08 = a.AcrBudget08,
                                    RealBudgetAcr08 = a.RealBudgetAcr08,
                                    BalBudget08 = a.BalBudget08,
                                    UsedBudget08 = a.UsedBudget08,
                                    Budget09 = a.Budget09,
                                    AcrBudget09 = a.AcrBudget09,
                                    RealBudgetAcr09 = a.RealBudgetAcr09,
                                    BalBudget09 = a.BalBudget09,
                                    UsedBudget09 = a.UsedBudget09,
                                    Budget10 = a.Budget10,
                                    AcrBudget10 = a.AcrBudget10,
                                    RealBudgetAcr10 = a.RealBudgetAcr10,
                                    BalBudget10 = a.BalBudget10,
                                    UsedBudget10 = a.UsedBudget10,
                                    Budget11 = a.Budget11,
                                    AcrBudget11 = a.AcrBudget11,
                                    RealBudgetAcr11 = a.RealBudgetAcr11,
                                    BalBudget11 = a.BalBudget11,
                                    UsedBudget11 = a.UsedBudget11,
                                    Budget12 = a.Budget12,
                                    AcrBudget12 = a.AcrBudget12,
                                    RealBudgetAcr12 = a.RealBudgetAcr12,
                                    BalBudget12 = a.BalBudget12,
                                    UsedBudget12 = a.UsedBudget12,
                                    BusinessUnitName = b.UnitName
                                }).ToListAsync();
            }
            return result;
        }

        public async Task<dynamic> GetMasterBudgetCode(string budgetCodeId, string type)
        {
            var result = await (from a in context.MasterBudgetCodes
                                join b in context.ChartOfAccounts on a.CoaId equals b.Id
                                join c in context.BusinessUnits on a.HrisDivisionId equals c.Id
                                where !(a.IsDeleted ?? false) && c.Ishris == true && a.Id == budgetCodeId && a.Source == type
                                select new BudgetMaster
                                {
                                    MasterBudgetCodeId = a.Id,
                                    HrisDivisionId = a.HrisDivisionId,
                                    HrisDivisionName = c.UnitName,
                                    AccountId = a.CoaId,
                                    CodeRec = b.CodeRec,
                                    AccountCOALv3 = b.Name3,
                                    AccountCOALv5 = b.Name5
                                }).FirstOrDefaultAsync();
            return result;
        }
        public Task<List<BudgetMasterHeader>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<BudgetMasterHeader> Get(string Id)
        {
            var result = await context.BudgetMasterHeaders.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
            if (result != null)
            {
                var user = await context.EmployeeBasicInfos.Where(x => x.Id == result.CreatedBy).FirstOrDefaultAsync();
                result.CreatedByName = user.NameEmployee;
                result.IsOwner = context.BudgetMasters.Where(x => x.BudgetMasterHeaderId == Id && x.BugetOwnerId == context.GetEmployeeId()).Count() > 0;
            }
            return result;
        }

        public async Task<BudgetMaster> GetBudgetMasterById(string MasterBudgetCodeId, string Id)
        {
            var result = new BudgetMaster();
            if (string.IsNullOrEmpty(Id))
            {
                var data = await context.MasterBudgetCodes.Where(x => x.Id == MasterBudgetCodeId).FirstOrDefaultAsync();
                if (data != null)
                {
                    result.MasterBudgetCodeId = data.Id;
                    result.AccountId = data.CoaId;
                    result.HrisDivisionId = data.HrisDivisionId;
                    result.BugetOwnerId = data.BugetOwnerId;
                    result.Status = StatusTransactionName.Draft;
                }
            }
            else
            {
                result = await context.BudgetMasters.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
            }

            if (result != null)
            {
                var coa = await context.ChartOfAccounts.Where(x => x.Id == result.AccountId).FirstOrDefaultAsync();
                if (coa != null)
                {
                    result.CodeRec = coa.CodeRec;
                    result.AccountCOALv3 = coa.Name3;
                    result.AccountCOALv5 = coa.Name5;
                }

                var bu = await context.BusinessUnits.Where(x => x.Id == result.HrisDivisionId).FirstOrDefaultAsync();
                if (bu != null)
                {
                    result.HrisDivisionName = bu.UnitName;
                }
                result.IsCurrentApproval = manageApproval.IsCurrentApproval(Id);
            }
            return result;
        }
        public Task<List<BudgetMasterHeader>> Get()
        {
            throw new NotImplementedException();
        }

        public bool IsRequestor(string type, string Id)
        {
            if (string.IsNullOrEmpty(Id)) return true;
            var employeeId = context.GetEmployeeId();
            var requestor = context.BudgetMasterHeaders.Where(x => x.Id == Id && x.CreatedBy == employeeId).Count() > 0;
            var budgetOwner = context.BudgetMasters.Where(x => x.BudgetMasterHeaderId == Id && x.BugetOwnerId == employeeId).Count() > 0;
            var approver = (from a in context.TrTemplateApprovals
                            join b in context.TrUserApprovals on a.Id equals b.TrTempApprovalId
                            join c in context.BudgetMasters on a.RefId equals c.Id
                            where b.EmployeeBasicInfoId == employeeId && c.BudgetMasterHeaderId == Id
                            select a).Count() > 0;

            return requestor || budgetOwner || approver;
        }
    }
}