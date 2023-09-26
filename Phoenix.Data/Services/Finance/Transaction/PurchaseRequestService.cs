using Phoenix.Data.Models.Finance;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Phoenix.Data.Models;
using Phoenix.Data.Attributes;
using Phoenix.Data.Services.Um;
using Phoenix.Data.Models.Um;

namespace Phoenix.Data.Services.Finance.Transaction
{
    public interface IPurchaseRequestService
    {
        Task<int> AddAsync(PurchaseRequest entity);
        Task<int> EditAsync(PurchaseRequest entity);
        Task<int> DeleteSoftAsync(PurchaseRequest entity);
        Task<int> ApproveAsync(PurchaseRequest entity);
        Task<List<PurchaseRequest>> GetPRList(string status);
        Task<PurchaseRequest> Get(string Id);
        Task<List<PurchaseRequestDetail>> GetDetail(string Id);
        Task<List<BudgetMaster>> GetBudgetByPRId(string purchaseRequestId);
        Task<dynamic> GetBudgetMasterById(string budgetMasterId);
        Task<bool> UpdateBudget(string purchaseRequestId, bool isAddBudget);
        Task<bool> UpdateAllBudget(string itemId, bool isAddBudget, decimal totalRequest, decimal totalAmount);
    }

    public class PurchaseRequestService : IPurchaseRequestService
    {
        readonly GlobalFunctionApproval globalFcApproval;
        readonly IManageMenuService menuService;
        readonly DataContext context;
        readonly StatusLogService log;
        private readonly IManageTransactionApprovalService trApprovalService;
        private readonly IManageTemplateApprovalService trTemplateApprovalService;
        readonly FileService uploadFile;

        public PurchaseRequestService(DataContext context, GlobalFunctionApproval globalFcApproval, IManageMenuService menuService, StatusLogService log, IManageTransactionApprovalService trApprovalService, IManageTemplateApprovalService trTemplateApprovalService, FileService uploadFile)
        {
            this.context = context;
            this.globalFcApproval = globalFcApproval;
            this.menuService = menuService;
            this.log = log;
            this.trApprovalService = trApprovalService;
            this.trTemplateApprovalService = trTemplateApprovalService;
            this.uploadFile = uploadFile;
        }

        public async Task<int> AddAsync(PurchaseRequest entity)
        {
            return await AddEdit(entity, false);
        }

        public async Task<int> EditAsync(PurchaseRequest entity)
        {
            return await AddEdit(entity, true);
        }

        private async Task<int> AddEdit(PurchaseRequest entity, bool isEdit)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var isCapex = entity.PurchaseRequestDetails.Where(x => (x.ItemCode ?? "").Contains("BCC")).Count() > 0;
                    var isOpex = entity.PurchaseRequestDetails.Where(x => (x.ItemCode ?? "").Contains("BCO")).Count() > 0;

                    var source = new List<string>();

                    if (isCapex) source.Add("CAPEX");
                    if (isOpex) source.Add("OPEX");

                    var header = await context.BudgetMasterHeaders.Where(x => !(x.IsDeleted ?? false)
                                        && x.BudgetYear == entity.BudgetYear
                                        //&& x.CurrencyId == entity.CurrencyId
                                        && x.Status == "Active" && source.Count() == 0 ? true : source.Any(y => y == x.Source)).ToListAsync();
                    if (header.Count() == 0)
                    {
                        //var currency = await context.Currencies.Where(x => x.Id == entity.CurrencyId).FirstOrDefaultAsync();
                        //throw new Exception($"for the {entity.BudgetYear.ToString()} budget with {currency.CurrencyName} curreny, the status is either inactive or not input. please check the {string.Join("or", source.ToArray()).ToLower()} budget master again.");
                        throw new Exception($"for the {entity.BudgetYear.ToString()} budget, the status is either inactive or not input. please check the {string.Join("or", source.ToArray()).ToLower()} budget master again.");
                    }
                    else
                    {
                        //var currency = await context.Currencies.Where(x => x.Id == entity.CurrencyId).FirstOrDefaultAsync();
                        var master = await context.BudgetMasters.Where(x => header.Any(y => y.Id == x.BudgetMasterHeaderId) && x.HrisDivisionId == entity.RequestorDivisionId).ToListAsync();
                        if (master.Count() == 0)
                        {
                            var divisi = await context.BusinessUnits.Where(x => master.Any(z => z.HrisDivisionId == z.Id)).Select(x => x.UnitName).ToListAsync();
                            //throw new Exception($"for {string.Join(",", divisi.ToArray())} division has not been registered in the {entity.BudgetYear.ToString()} budget master in {currency.CurrencyName} currency. please check the capex or opex budget master again.");
                            throw new Exception($"for {string.Join(",", divisi.ToArray())} division has not been registered in the {entity.BudgetYear.ToString()} budget master. please check the capex or opex budget master again.");
                        }
                        //else
                        //{
                        //    var detail = await context.BudgetMasterDetails.Where(x => master.Any(z => z.Id == x.BudgetMasterId) && x.BusinessUnitId == entity.BusinessUnitId).FirstOrDefaultAsync();
                        //    if (detail == null)
                        //    {
                        //        var divisi = await context.BusinessUnits.Where(x => master.Any(z => z.HrisDivisionId == z.Id)).Select(x => x.UnitName).ToListAsync();
                        //        var businessUnit = await context.BusinessUnits.Where(x => x.Id == detail.BusinessUnitId).Select(x => x.UnitName).FirstOrDefaultAsync();
                        //        throw new Exception($"{businessUnit} business units in the detailed budget master are not registered in the {string.Join(",", divisi.ToArray())} divisions for the {entity.BudgetYear.ToString()} budget with {currency.CurrencyName} curreny. please check the capex or opex budget master again.");
                        //    }
                        //}
                    }

                    //var AffiliationId = await context.BusinessUnits.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && (x.Isfinance ?? false) && x.Id == entity.BusinessUnitId).Select(x => x.AffiliationId).FirstOrDefaultAsync();
                    //entity.AffiliationId = AffiliationId;
                    entity.Status = StatusTransaction.StatusName(entity.StatusApproval);

                    var file = await uploadFile.Upload(entity.file, entity.DocumentSpecificationFile);
                    if (!string.IsNullOrEmpty(file))
                    {
                        entity.DocumentSpecificationFile = file;
                    }

                    if (isEdit)
                    {
                        context.PhoenixEdit(entity);
                    }
                    else
                    {
                        entity.Id = await TransID.GetTransId(context, Code.PR, DateTime.Today);
                        entity.RequestNumber = entity.Id;
                        await context.PhoenixAddAsync(entity);
                        await context.SaveChangesAsync();
                    }

                    if (!string.IsNullOrEmpty(entity.Id))
                    {
                        var detail = context.PurchaseRequestDetail.AsNoTracking().Where(x => x.PurchaseRequestId == entity.Id).ToList();
                        if (detail.Count > 0)
                        {
                            context.PurchaseRequestDetail.RemoveRange(detail);
                        }
                    }

                    if (entity.PurchaseRequestDetails != null)
                    {
                        foreach (var item in entity.PurchaseRequestDetails)
                        {
                            item.Id = Guid.NewGuid().ToString();
                            item.PurchaseRequestId = entity.Id;
                            await context.PhoenixAddAsync(item);
                        };
                    }

                    await context.SaveChangesAsync();
                    var isApprove = true;
                    if (entity.Status != StatusTransactionName.Draft)
                    {
                        var dataMenu = await menuService.GetByUniqueName(MenuUnique.PurchaseRequest).ConfigureAwait(false);
                        if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
                        var vm = new Models.ViewModel.TransApprovalHrisVm()
                        {
                            MenuId = dataMenu.Id,
                            RefId = entity.Id,
                            DetailLink = $"{ApprovalLink.PurchaseRequest}?Id={entity.Id}",
                            NotificationType = NotificationTypeCode.PRNotification,
                            Module = NotificationModuleType.ModuleFinance
                        };
                        var subGroupId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessSubGroup(context.HttpContext);
                        var divisionId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessUnitDivisi(context.HttpContext);
                        var employeeId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(context.HttpContext);
                        isApprove = await ProcessApprovalFlow(vm, subGroupId, (entity.RequestorDivisionId ?? divisionId), employeeId, entity.BudgetOwnerId);
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
                        context.PurchaseRequest.Update(entity);
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

        public async Task<bool> ProcessApprovalFlow(Models.ViewModel.TransApprovalHrisVm dataVm, string subGroupId, string divisionId, string employeeId, string budgetOwnerId)
        {
            if (!string.IsNullOrEmpty(dataVm.Tname) && !string.IsNullOrEmpty(dataVm.RefId))
            {
                if (!string.IsNullOrEmpty(dataVm.MenuId) || !string.IsNullOrEmpty(dataVm.DetailLink))
                {
                    await trApprovalService.DeleteRefTable(dataVm.Tname, dataVm.RefId).ConfigureAwait(false);
                    return await Task.FromResult(false);
                }
            }

            if (!string.IsNullOrEmpty(dataVm.RefId))
            {
                var trApproval = context.TrTemplateApprovals.AsNoTracking().Where(x => x.RefId == dataVm.RefId && !(x.IsDeleted ?? false));//&& x.StatusApproved == StatusApproved.Rejected);
                if (trApproval != null)
                {
                    foreach (var item in trApproval)
                    {
                        context.Remove(item);
                    }
                }
            }

            try
            {
                var jobTitleId = "0";
                var data = new TmTemplateApproval();
                if (!string.IsNullOrEmpty(dataVm.IdTemplate))
                {
                    data = await trTemplateApprovalService.GetById(dataVm.IdTemplate).ConfigureAwait(false);
                }
                else
                {
                    if (string.IsNullOrEmpty(divisionId))
                    {
                        throw new Exception("this employee doesn't have a division, please check this employee data again.");
                    }

                    data = await trTemplateApprovalService.GetByMenuId(subGroupId, divisionId, dataVm.MenuId, jobTitleId).ConfigureAwait(false);

                    if (data == null)
                    {
                        throw new Exception("please check the approval template that will be processed");
                    }
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
                model.CreatedBy = employeeId;
                model.DivisionId = divisionId;
                model.IsDeleted = false;
                model.RefId = dataVm.RefId;
                var tempId = Guid.NewGuid().ToString();
                model.Id = tempId;

                await context.PhoenixAddAsync(model);
                //await _repo.AddAsync(model).ConfigureAwait(false);

                var prDetail = await context.PurchaseRequestDetail.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.PurchaseRequestId == dataVm.RefId).GroupBy(x => x.ItemId).Select(x => new { x.First().ItemId, total = x.Sum(y => y.AmountExchange) }).ToListAsync();
                var budget = await context.BudgetMasters.AsNoTracking().Where(x => prDetail.Any(z => z.ItemId == x.Id)).ToListAsync();

                //Special case ini untuk sisa nilai budget PO di bawah 0
                bool isSpecialCase = false;
                foreach (var item in budget)
                {
                    var get = prDetail.FirstOrDefault(x => x.ItemId == item.Id);
                    if (get != null)
                    {
                        item.TotalBudgetUsedAllTransaction += (get.total ?? 0);
                        item.TotalBudgetBalanceAllTransaction = item.TotalBudget - item.TotalBudgetUsedAllTransaction;
                        if (item.TotalBudgetBalanceAllTransaction <= 0)
                        {
                            isSpecialCase = true;
                            break;
                        }
                    }
                };

                //Start Custome Approval Add new OwnerId
                var ownerApprover = data.ListUserApprovals.OrderBy(x => x.IndexUser).FirstOrDefault();
                data.ListUserApprovals.ForEach(x => x.IndexUser += 1);

                if (ownerApprover != null)
                {
                    var item = new TmTemplateUserApproval();
                    item.Id = Guid.NewGuid().ToString();
                    item.IndexUser = (ownerApprover.IndexUser - 1);
                    item.IsCondition = ownerApprover.IsCondition;
                    item.EmployeeBasicInfoId = budgetOwnerId;
                    item.IsSpecialCase = isSpecialCase;
                    item.ListConditionalApprovals = new List<TmConditionUserApproval>();
                    data.ListUserApprovals.Add(item);
                }
                //End Custome Approval Add new OwnerId

                var isFirst = 0;
                var maxIndexUser = "0";
                if (data.ListUserApprovals.Count() > 2)
                {
                    maxIndexUser = data.ListUserApprovals.Max(x => x.IndexUser).ToString();//for special case harus ada lebih dari 1 user
                }
                var list = data.ListUserApprovals.Where(x => (isSpecialCase == true ? true : x.IndexUser.ToString() != maxIndexUser) && (x.EmployeeBasicInfoId ?? "") != "").OrderBy(x => x.IndexUser);
                foreach (var item in list)
                {
                    var vmUser = new TrUserApproval();
                    vmUser.EmployeeBasicInfoId = item.EmployeeBasicInfoId;
                    vmUser.IndexUser = item.IndexUser;
                    vmUser.IsCondition = item.IsCondition;
                    vmUser.IsSpecialCase = item.IsSpecialCase;
                    vmUser.TrTempApprovalId = tempId;
                    vmUser.Id = Guid.NewGuid().ToString();
                    vmUser.NotificationTypeId = dataVm.NotificationType;
                    vmUser.Module = dataVm.Module;
                    if (isFirst == 0)
                    {
                        vmUser.StatusApproved = StatusApproved.CurrentApproval;

                        //notifikasi hanya muncul untuk yg sedang melakukan approval
                        if (!string.IsNullOrEmpty(dataVm.NotificationType) && !string.IsNullOrEmpty(dataVm.Module))
                        {
                            var notif = await context.TmUserApps.AsNoTracking().Where(x => x.EmployeeBasicInfoId == item.EmployeeBasicInfoId && x.IsDeleted == false).FirstOrDefaultAsync();
                            if (notif != null)
                            {
                                await globalFcApproval.AddNotification(dataVm.NotificationType, notif.Id, dataVm.Module, dataVm.RefId, data.IsNotifyByWeb, data.IsNotifyByEmail, employeeId);
                            }
                        }
                    }
                    else
                    {
                        vmUser.StatusApproved = StatusApproved.WaitingApproval;
                    }
                    await context.PhoenixAddAsync(vmUser);
                    //await _repoManageUserApproval.AddAsync(vmUser).ConfigureAwait(false);

                    //looping data conditional user approval.....
                    if (item.ListConditionalApprovals != null)
                    {
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
                            modelCon.CreatedBy = employeeId;
                            await context.PhoenixAddAsync(modelCon);
                            //await _repoManageConditionApproval.AddAsync(modelCon).ConfigureAwait(false);
                        }
                    }
                    await context.SaveChangesAsync();
                    isFirst++;
                }

                var vm = new
                {
                    IdTemp = tempId
                };
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (!string.IsNullOrEmpty(dataVm.RefId) && !string.IsNullOrEmpty(dataVm.Tname))
                {
                    await trApprovalService.DeleteRefTable(dataVm.Tname, dataVm.RefId).ConfigureAwait(false);
                }
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> ApproveAsync(PurchaseRequest entity)
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
                            if (context.RequestForQuotation.AsNoTracking().Where(x => x.PurchaseRequestId == entity.Id).Count() == 0)
                            {
                                var rfqId = await TransID.GetTransId(context, Code.RFQ, DateTime.Today);
                                var RFQ = new RequestForQuotation()
                                {
                                    Id = rfqId,
                                    RequestForQuotationNumber = rfqId,
                                    PurchaseRequestId = entity.Id,
                                    AffiliationId = entity.AffiliationId,
                                    LegalEntityId = entity.LegalEntityId,
                                    BusinessUnitId = entity.BusinessUnitId,
                                    RequestForQuotationDate = DateTime.Now,
                                    Status = StatusTransactionName.Draft,
                                    MainserviceCategoryId = MainServiceCategoryOther.MainServiceCategoryId,
                                    //IsPreferedVendor = entity.IsPrefered,
                                    DueDate = entity.DueDate,
                                    //CurrencyId = entity.CurrencyId,
                                    CurrencyId = (from x in context.BudgetMasters
                                                  join y in context.BudgetMasterHeaders on x.BudgetMasterHeaderId equals y.Id
                                                  join z in context.Currencies on y.CurrencyId equals z.Id
                                                  join o in context.PurchaseRequestDetail on x.Id equals o.ItemId
                                                  where o.PurchaseRequestId == entity.Id
                                                  select z.Id).FirstOrDefault(),
                                    Remarks = entity.RemarksProcess,
                                    CreatedBy = entity.CreatedBy,
                                    CreatedOn = DateTime.Now,
                                    ModifiedBy = context.GetEmployeeId(),
                                    ModifiedOn = DateTime.Now,
                                    InternalOrProject = "Internal"
                                };
                                await context.RequestForQuotation.AddAsync(RFQ);
                                await context.SaveChangesAsync();


                                foreach (var item in entity.PurchaseRequestDetails)
                                {
                                    //var RFQTask = new RequestForQuotationTask
                                    //{
                                    //    Id = Guid.NewGuid().ToString(),
                                    //    RequestForQuotationId = RFQ.Id,
                                    //    Task = item.Name,
                                    //    TaskId = item.Id,
                                    //    ItemTypeId = item.ItemTypeId,
                                    //    ItemId = item.ItemId,
                                    //    ItemCode = item.ItemCode,
                                    //    ItemName = item.ItemName,
                                    //    Quantity = item.Qty,
                                    //    UomId = item.UomId,
                                    //    UnitPrice = item.UnitPrice,
                                    //    TotalPrice = item.UnitPrice * item.Qty,
                                    //    CreatedBy = entity.CreatedBy,
                                    //    CreatedOn = DateTime.Now,
                                    //    ModifiedBy = context.GetEmployeeId(),
                                    //    ModifiedOn = DateTime.Now
                                    //};
                                    //context.RequestForQuotationTask.Add(RFQTask);

                                    var RFQTask = new RequestForQuotationTask
                                    {
                                        Id = Guid.NewGuid().ToString(),
                                        RequestForQuotationId = RFQ.Id,
                                        Task = item.Name,
                                        TaskId = item.Id,
                                        ItemTypeId = item.ItemTypeId,
                                        ItemId = item.ItemId,
                                        ItemCode = item.ItemCode,
                                        ItemName = item.ItemName,
                                        Quantity = item.Qty,
                                        UomId = item.UomId,
                                        UnitPrice = item.UnitPrice,
                                        TotalPrice = item.Amount,
                                        TotalRequest = item.Amount,
                                        TotalExchange = item.AmountExchange,
                                        CurrencyId = entity.CurrencyId,
                                        ExchangeRate = entity.ExchangeRate,
                                        Operator = entity.Operator,
                                    };
                                    context.PhoenixAdd(RFQTask);
                                    await UpdateAllBudget(item.ItemId, true, 0, (item.AmountExchange ?? 0));
                                };

                                await UpdateBudget(entity.Id, true);
                            }
                        }

                        context.PurchaseRequest.Update(entity);
                        await log.AddAsync(new StatusLog() { TransactionId = entity.Id, Status = entity.Status, Description = entity.RemarksProcess });
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

        public async Task<int> DeleteSoftAsync(PurchaseRequest entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<List<PurchaseRequest>> GetPRList(string status)
        {
            //var businessUnit = await (from buj in context.BusinessUnitJobLevels
            //                          join bu in context.BusinessUnits on buj.BusinessUnitDivisionId equals bu.Id
            //                          join but in context.BusinessUnitTypes on bu.BusinessUnitTypeId equals but.Id
            //                          where but.BusinessUnitLevel == BusinessUnitLevelCode.Division && (bu.Ishris ?? false)
            //                          select new { BusinessUnitJobLevelId = buj.Id, bu.UnitName }).ToListAsync();

            var model = await (from tb in context.PurchaseRequest
                                   //join bu in context.BusinessUnits on tb.BusinessUnitId equals bu.Id
                                   //join but in context.BusinessUnitTypes on bu.BusinessUnitTypeId equals but.Id
                               join usr in context.EmployeeBasicInfos on tb.CreatedBy equals usr.Id
                               join le in context.LegalEntity on tb.LegalEntityId equals le.Id
                               where !(tb.IsDeleted ?? false) && //but.BusinessUnitLevel == BusinessUnitLevelCode.Division && (bu.Isfinance ?? false) &&
                               (string.IsNullOrEmpty(status) ? true : (tb.Status ?? StatusTransactionName.Draft) == status)
                               select new PurchaseRequest
                               {
                                   Id = tb.Id,
                                   RequestNumber = tb.RequestNumber,
                                   RequestDatetime = tb.RequestDatetime,
                                   BusinessUnitName = context.BusinessUnits.Where(x => x.Id == tb.BusinessUnitId).Select(x => x.UnitName).FirstOrDefault(),
                                   Requestor = usr.NameEmployee,
                                   Status = tb.Status,
                                   RequestorDivisionName = context.BusinessUnits.Where(x => x.Id == tb.RequestorDivisionId).Select(x => x.UnitName).FirstOrDefault(),//businessUnit.Where(x => x.BusinessUnitJobLevelId == usr.BusinessUnitJobLevelId).Select(x => x.UnitName).FirstOrDefault(),
                                   ModifiedOn = tb.ModifiedOn,
                                   LegalEntityName = le.LegalEntityName,
                                   CurrencySymbol = context.Currencies.Where(x => x.Id == tb.CurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault(),
                                   GrandTotal = tb.GrandTotal
                               }).OrderByDescending(x => x.ModifiedOn).ToListAsync();
            return model;
        }

        public async Task<PurchaseRequest> Get(string Id)
        {
            var result = await context.PurchaseRequest.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
            if (result != null)
            {
                result.CurrencySymbol = context.Currencies.Where(x => x.Id == result.CurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault();
                var employee = await context.EmployeeBasicInfos.Where(x => x.Id == result.CreatedBy).FirstOrDefaultAsync();
                if (employee != null)
                {
                    result.Requestor = employee.NameEmployee;
                    //var businessUnitId = await (from buj in context.BusinessUnitJobLevels
                    //                            join bu in context.BusinessUnits on buj.BusinessUnitDivisionId equals bu.Id
                    //                            join but in context.BusinessUnitTypes on bu.BusinessUnitTypeId equals but.Id
                    //                            where but.BusinessUnitLevel == BusinessUnitLevelCode.Division &&
                    //                            buj.Id == employee.BusinessUnitJobLevelId && (bu.Ishris ?? false)
                    //                            select bu.Id).Select(x => x).FirstOrDefaultAsync();
                    //result.RequestorDivisionId = businessUnitId;
                }

                if (!string.IsNullOrEmpty(result.DocumentSpecificationFile))
                {
                    result.DocumentSpecificationFileName = await context.Filemasters.Where(x => x.Id == result.DocumentSpecificationFile).Select(x => x.Name).FirstOrDefaultAsync();
                }
                else
                {
                    result.DocumentSpecificationFileName = "";
                }
            }
            return result;
        }

        public async Task<List<PurchaseRequestDetail>> GetDetail(string Id)
        {
            var result = await (from a in context.PurchaseRequestDetail
                                join b in context.Uom on a.UomId equals b.Id
                                join c in context.ItemType on a.ItemTypeId equals c.Id
                                join d in context.PurchaseRequest on a.PurchaseRequestId equals d.Id
                                where a.PurchaseRequestId == Id && !(a.IsDeleted ?? false)
                                select new PurchaseRequestDetail
                                {
                                    Id = a.Id,
                                    PurchaseRequestId = a.PurchaseRequestId,
                                    ItemTypeId = a.ItemTypeId,
                                    ItemId = a.ItemId,
                                    ItemCode = a.ItemCode,
                                    ItemName = a.ItemName,
                                    Name = a.Name,
                                    UomId = a.UomId,
                                    Qty = a.Qty,
                                    UnitPrice = a.UnitPrice,
                                    Amount = a.Amount,
                                    AmountExchange = a.AmountExchange,
                                    ItemTypeName = c.TypeName,
                                    UomName = b.unit_symbol,
                                    CreatedBy = a.CreatedBy,
                                    CreatedOn = a.CreatedOn,
                                    CurrencySymbolExchange = (from x in context.BudgetMasters
                                                              join y in context.BudgetMasterHeaders on x.BudgetMasterHeaderId equals y.Id
                                                              join z in context.Currencies on y.CurrencyId equals z.Id
                                                              where x.Id == a.ItemId
                                                              select z.CurrencySymbol).FirstOrDefault(),
                                    CurrencySymbol = context.Currencies.Where(x => x.Id == d.CurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault(),
                                    TotalBudgetUsed = context.BudgetMasters.Where(x => x.Id == a.ItemId).Select(x => x.TotalBudgetUsedAllTransaction).FirstOrDefault(),
                                    TotalBudgetBalance = context.BudgetMasters.Where(x => x.Id == a.ItemId).Select(x => x.TotalBudgetBalanceAllTransaction).FirstOrDefault()
                                }).ToListAsync();
            return result;
        }

        public async Task<List<BudgetMaster>> GetBudgetByPRId(string purchaseRequestId)
        {
            var model = await (from prd in context.PurchaseRequestDetail
                               join tb in context.BudgetMasters on prd.ItemId equals tb.Id
                               join coa in context.ChartOfAccounts on tb.AccountId equals coa.Id
                               where !(tb.IsDeleted ?? false) && prd.PurchaseRequestId == purchaseRequestId
                               select new BudgetMaster
                               {
                                   Id = tb.Id,
                                   CodeRec = coa.CodeRec,
                                   AccountCOALv5 = coa.Name5,
                                   TotalBudget = tb.TotalBudget,
                                   TotalAcrBudget = tb.TotalAcrBudget,
                                   TotalRealBudgetAcr = tb.TotalRealBudgetAcr,
                                   TotalBudgetUsed = tb.TotalBudgetUsed,
                                   TotalBudgetBalanceAcr = tb.TotalBudgetBalanceAcr,
                                   TotalBudgetBalanceReal = tb.TotalBudgetBalanceReal,
                                   TotalBudgetBalance = tb.TotalBudgetBalance//Balance Used
                               }).ToListAsync();
            return model;
        }

        public async Task<dynamic> GetBudgetMasterById(string budgetMasterId)
        {
            var result = await (from a in context.BudgetMasters
                                join b in context.MasterBudgetCodes on a.MasterBudgetCodeId equals b.Id
                                join c in context.BudgetMasterHeaders on a.BudgetMasterHeaderId equals c.Id
                                join d in context.Currencies on c.CurrencyId equals d.Id
                                where a.Id == budgetMasterId
                                select new { b.BudgetCode, b.BudgetName, a.BugetOwnerId, TotalBudgetUsed = a.TotalBudgetUsedAllTransaction, TotalBudgetBalance = a.TotalBudgetBalanceAllTransaction, d.CurrencySymbol }).FirstOrDefaultAsync();
            return result;
        }

        public async Task<bool> UpdateBudget(string purchaseRequestId, bool isAddBudget)
        {
            var prDetail = await context.PurchaseRequestDetail.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.PurchaseRequestId == purchaseRequestId).GroupBy(x => x.ItemId).Select(x => new { x.First().ItemId, total = x.Sum(y => y.AmountExchange) }).ToListAsync();
            var budget = await context.BudgetMasters.AsNoTracking().Where(x => prDetail.Any(z => z.ItemId == x.Id)).ToListAsync();
            var budgetDetail = await context.BudgetMasterDetails.AsNoTracking().Where(x => budget.Any(z => z.Id == x.BudgetMasterId)).ToListAsync();

            //update budget Capex or Opex
            foreach (var item in budget)
            {
                var get = prDetail.FirstOrDefault(x => x.ItemId == item.Id);
                if (get != null)
                {
                    if (isAddBudget)
                    {
                        item.TotalAcrBudget += (get.total ?? 0);
                    }
                    else
                    {
                        item.TotalAcrBudget -= (get.total ?? 0);
                    }
                    item.TotalBudgetBalanceAcr = item.TotalBudget - item.TotalAcrBudget;
                    //if (item.TotalAcrBudget > item.TotalBudget)
                    //{
                    //    throw new Exception("the total budget for this year has exceeded the specified capacity. please check that the input budget cannot exceed the total budget that was set.");
                    //}
                }
                context.PhoenixEdit(item);
            };

            //update budget Capex or Opex detail
            //var message = "the total budget for this month has already exceeded the specified capacity. please double check the input budget must not exceed the total budget that has been set.";
            foreach (var item in budgetDetail)
            {
                var get = prDetail.FirstOrDefault(x => x.ItemId == item.BudgetMasterId);
                if (get != null)
                {
                    var month = DateTime.Now.Month;
                    if (isAddBudget)
                    {
                        if (month == 1) { item.AcrBudget01 += (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.AcrBudget01 > item.Budget01) throw new Exception(message); }
                        if (month == 2) { item.AcrBudget02 += (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.AcrBudget02 > item.Budget02) throw new Exception(message); }
                        if (month == 3) { item.AcrBudget03 += (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.AcrBudget03 > item.Budget03) throw new Exception(message); }
                        if (month == 4) { item.AcrBudget04 += (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.AcrBudget04 > item.Budget04) throw new Exception(message); }
                        if (month == 5) { item.AcrBudget05 += (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.AcrBudget05 > item.Budget05) throw new Exception(message); }
                        if (month == 6) { item.AcrBudget06 += (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.AcrBudget06 > item.Budget06) throw new Exception(message); }
                        if (month == 7) { item.AcrBudget07 += (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.AcrBudget07 > item.Budget07) throw new Exception(message); }
                        if (month == 8) { item.AcrBudget08 += (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.AcrBudget08 > item.Budget08) throw new Exception(message); }
                        if (month == 9) { item.AcrBudget09 += (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.AcrBudget09 > item.Budget09) throw new Exception(message); }
                        if (month == 10) { item.AcrBudget10 += (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.AcrBudget10 > item.Budget10) throw new Exception(message); }
                        if (month == 11) { item.AcrBudget11 += (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.AcrBudget11 > item.Budget11) throw new Exception(message); }
                        if (month == 12) { item.AcrBudget12 += (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.AcrBudget12 > item.Budget12) throw new Exception(message); }
                    }
                    else
                    {
                        if (month == 1) { item.AcrBudget01 -= (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.AcrBudget01 > item.Budget01) throw new Exception(message); }
                        if (month == 2) { item.AcrBudget02 -= (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.AcrBudget02 > item.Budget02) throw new Exception(message); }
                        if (month == 3) { item.AcrBudget03 -= (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.AcrBudget03 > item.Budget03) throw new Exception(message); }
                        if (month == 4) { item.AcrBudget04 -= (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.AcrBudget04 > item.Budget04) throw new Exception(message); }
                        if (month == 5) { item.AcrBudget05 -= (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.AcrBudget05 > item.Budget05) throw new Exception(message); }
                        if (month == 6) { item.AcrBudget06 -= (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.AcrBudget06 > item.Budget06) throw new Exception(message); }
                        if (month == 7) { item.AcrBudget07 -= (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.AcrBudget07 > item.Budget07) throw new Exception(message); }
                        if (month == 8) { item.AcrBudget08 -= (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.AcrBudget08 > item.Budget08) throw new Exception(message); }
                        if (month == 9) { item.AcrBudget09 -= (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.AcrBudget09 > item.Budget09) throw new Exception(message); }
                        if (month == 10) { item.AcrBudget10 -= (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.AcrBudget10 > item.Budget10) throw new Exception(message); }
                        if (month == 11) { item.AcrBudget11 -= (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.AcrBudget11 > item.Budget11) throw new Exception(message); }
                        if (month == 12) { item.AcrBudget12 -= (get.total ?? 0) * (item.CostSharingAllocationPercent / 100); } //if (item.AcrBudget12 > item.Budget12) throw new Exception(message); }
                    }
                    context.PhoenixEdit(item);
                }
            };
            return true;
        }

        public async Task<bool> UpdateAllBudget(string itemId, bool isAddBudget, decimal totalRequest, decimal totalAmount)
        {
            var budget = await context.BudgetMasters.AsNoTracking().Where(x => x.Id == itemId).FirstOrDefaultAsync();
            if (budget != null)
            {
                if (isAddBudget == true)
                {
                    budget.TotalBudgetUsedAllTransaction = (budget.TotalBudgetUsedAllTransaction - totalRequest) + totalAmount;
                }
                else
                {
                    budget.TotalBudgetUsedAllTransaction = (budget.TotalBudgetUsedAllTransaction - totalRequest);
                }

                budget.TotalBudgetBalanceAllTransaction = (budget.TotalBudget) - budget.TotalBudgetUsedAllTransaction;

            }
            context.Database.ExecuteSqlCommand($"UPDATE fn.budget_master SET total_budget_used_all_transaction={budget.TotalBudgetUsedAllTransaction}, total_budget_balance_all_transaction={budget.TotalBudgetBalanceAllTransaction} WHERE Id={itemId}");
            return true;
        }
    }
}
