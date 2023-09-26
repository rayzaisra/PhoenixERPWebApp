using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Enum;
using Phoenix.Data.Models.Finance;
using Phoenix.Data.Models.Um;
using Phoenix.Data.Services.Um;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Finance
{
    public interface IPettyCashService
    {
        Task<List<PettyCash>> Get();
        Task<PettyCash> GetById(string Id);
        Task<List<PettyCashDetail>> GetDetails(string Id);
        Task<List<MasterPettyCash>> GetMasterPettyCash(string Id);
        Task<PettyCash> GetMasterPettyCashById(string masterPettyCashId);
        Task<List<Journal>> GetJournal(string Id);
        Task<JobDetail> GetInfoPceByJobId(string jobid);
        Task<dynamic> GetPcaTasks(string pcaId);
        Task<int> AddAsync(PettyCash entity);
        Task<int> EditAsync(PettyCash entity);
        Task<int> CreateJournal(PettyCash entity);
        Task<int> Reject(PettyCash entity);
        Task<int> ApproveAsync(PettyCash entity);
        bool IsHolder();
    }

    public class PettyCashService : IPettyCashService
    {
        readonly DataContext context;
        readonly FinancePeriodService financePeriod;
        readonly GlobalFunctionApproval globalFcApproval;
        readonly Services.Um.IManageMenuService menuService;
        readonly FileService uploadFile;
        readonly StatusLogService log;
        readonly ManageTransactionApprovalService manageApproval;

        public PettyCashService(DataContext context, FinancePeriodService financePeriod,
            GlobalFunctionApproval globalFcApproval, Services.Um.IManageMenuService menuService, FileService uploadFile, StatusLogService log, ManageTransactionApprovalService manageApproval)
        {
            this.context = context;
            this.financePeriod = financePeriod;
            this.globalFcApproval = globalFcApproval;
            this.menuService = menuService;
            this.uploadFile = uploadFile;
            this.log = log;
            this.manageApproval = manageApproval;
        }

        public async Task<List<PettyCash>> Get()
        {
            var employeeId = context.GetEmployeeId();
            var approver = (from lu in context.TrUserApprovals
                            join tmp in context.TrTemplateApprovals on lu.TrTempApprovalId equals tmp.Id
                            where lu.EmployeeBasicInfoId == employeeId && !(tmp.IsDeleted ?? false) //&& lu.StatusApproved == StatusApproved.CurrentApproval 
                            select new { tmp.RefId, lu.EmployeeBasicInfoId });
            var doa = (from a in context.TmUserApps
                       join b in context.DOAs on a.Id equals b.AssignFrom
                       select new { b.AssignTo });
            var result = await (from a in context.PettyCashs
                                join c in context.MasterPettyCashs on a.RefferenceNo equals c.Id
                                join b in context.LegalEntity on a.LegalEntityId equals b.Id
                                where !(a.IsDeleted ?? false) && (c.HolderUserId == employeeId || c.DelegateUserId == employeeId ||
                                approver.Any(x => x.RefId == a.Id) || approver.Any(x => doa.Any(y => y.AssignTo == x.EmployeeBasicInfoId)))
                                select new PettyCash()
                                {
                                    Id = a.Id,
                                    RefferenceNo = a.RefferenceNo,
                                    LegalEntityName = b.LegalEntityName,
                                    BankAccountNumber = $"{c.BankAccountName} - {c.BankAccountNumber}",
                                    Status = a.Status,
                                    StatusName = PettyCashStatus.StatusName(a.Status),
                                    SettlementDate = a.SettlementDate,
                                    MasterPettyCashAmount = (context.Journal.Where(y => y.AccountId == a.CoaId && y.LegalId == a.LegalEntityId && y.TransactionDate.Value.Year == DateTime.Now.Year).Sum(y => (y.Debit - y.Credit)) ?? 0),//a.MasterPettyCashAmount,
                                    HolderName = context.EmployeeBasicInfos.Where(x => x.Id == c.HolderUserId).Select(x => x.NameEmployee).FirstOrDefault(),
                                    HolderName2 = context.EmployeeBasicInfos.Where(x => x.Id == c.DelegateUserId).Select(x => x.NameEmployee).FirstOrDefault(),
                                    DivisionName = context.BusinessUnits.Where(x => x.Id == a.DivisionId).Select(x => x.UnitName).FirstOrDefault()
                                }).ToListAsync();

            return result;
        }

        public async Task<PettyCash> GetById(string Id)
        {
            var result = await (from a in context.PettyCashs
                                join c in context.MasterPettyCashs on a.RefferenceNo equals c.Id
                                where !(a.IsDeleted ?? false) && a.Id == Id
                                select new PettyCash()
                                {
                                    Id = a.Id,
                                    RefferenceNo = a.RefferenceNo,
                                    SettlementDate = a.SettlementDate,
                                    HolderId = c.HolderUserId,
                                    DivisionId = a.DivisionId,
                                    ExchangeRate = a.ExchangeRate,
                                    Remarks = a.Remarks,
                                    Status = a.Status,
                                    MasterPettyCashAmount = (context.Journal.Where(y => y.AccountId == a.CoaId && y.LegalId == a.LegalEntityId && y.TransactionDate.Value.Year == DateTime.Now.Year).Sum(y => (y.Debit - y.Credit)) ?? 0),//a.MasterPettyCashAmount,
                                    TotalCreditAmount = a.TotalCreditAmount,
                                    EndingBalance = a.EndingBalance,
                                    CurrencyId = a.CurrencyId,
                                    LegalEntityId = a.LegalEntityId,
                                    CoaId = a.CoaId,
                                    BankAccountNumber = a.Status == PettyCashStatus.Approved ? a.BankAccountNumber : context.MasterPettyCashs.Where(x => x.Id == a.RefferenceNo).Select(x => x.BankAccountNumber).FirstOrDefault(),
                                    FileMasterId = a.FileMasterId,
                                    FilemasterName = context.Filemasters.Where(x => x.Id == a.FileMasterId).Select(x => x.Name).FirstOrDefault(),
                                    StatusName = PettyCashStatus.StatusName(a.Status),
                                    HolderName = context.EmployeeBasicInfos.Where(x => x.Id == c.HolderUserId).Select(x => x.NameEmployee).FirstOrDefault(),
                                    HolderName2 = context.EmployeeBasicInfos.Where(x => x.Id == c.DelegateUserId).Select(x => x.NameEmployee).FirstOrDefault(),
                                    DivisionName = context.BusinessUnits.Where(x => x.Id == a.DivisionId).Select(x => x.UnitName).FirstOrDefault(),
                                    CreatedBy = a.CreatedBy,
                                    CreatedOn = a.CreatedOn
                                }).FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<PettyCashDetail>> GetDetails(string Id)
        {
            var result = await (from pc in context.PettyCashs
                                join pd in context.PettyCashDetails on pc.Id equals pd.PettyCashId
                                where !(pd.IsDeleted ?? false) && pc.Id == Id
                                select new PettyCashDetail
                                {
                                    Id = pd.Id,
                                    PettyCashId = pd.PettyCashId,
                                    AccountId = pd.AccountId,
                                    TransactionDate = pd.TransactionDate,
                                    TransactionDetail = pd.TransactionDetail,
                                    Credit = pd.Credit,
                                    JobId = pd.JobId,
                                    BusinessUnitId = pd.BusinessUnitId,
                                    PcaId = pd.PcaId,
                                    PceId = pd.PceId,
                                    PcaTaskId = pd.PcaTaskId,
                                    JobName = context.JobDetail.Where(x => x.Id == pd.JobId).Select(x => $"{x.Id} - {x.JobName}").FirstOrDefault(),
                                    PcaTask = context.PcaTasks.Where(x => x.Id == pd.PcaTaskId).Select(x => x.TaskName).FirstOrDefault(),
                                    BusinessUnitName = context.BusinessUnits.Where(x => x.Id == pd.BusinessUnitId).Select(x => x.UnitName).FirstOrDefault(),
                                    AccountName = context.ChartOfAccounts.Where(x => x.Id == pd.AccountId && x.Level == "5").Select(x => $"({x.CodeRec}) {x.Name5}").FirstOrDefault()
                                }).ToListAsync();
            return result;
        }

        public async Task<List<MasterPettyCash>> GetMasterPettyCash(string Id)//for dropdownlist
        {
            var employeeId = context.GetEmployeeId();
            var result = await (from a in context.MasterPettyCashs
                                join c in context.Currencies on a.CurrencyId equals c.Id
                                join d in context.ChartOfAccounts on new { Id = a.ChartAccountId, Level = "5" } equals new { d.Id, d.Level }
                                join e in context.LegalEntity on a.LegalEntityId equals e.Id
                                join f in context.EmployeeBasicInfos on a.HolderUserId equals f.Id
                                join g in context.EmployeeBasicInfos on a.DelegateUserId equals g.Id
                                where !(a.IsDeleted ?? false) && (a.HolderUserId == employeeId || a.DelegateUserId == employeeId || context.PettyCashs.Any(x => !(x.IsDeleted ?? false) && x.RefferenceNo == a.Id && x.Id == Id))
                                select new MasterPettyCash
                                {
                                    Id = a.Id,
                                    BankAccountName = a.BankAccountName,
                                    BankAccountNumber = a.BankAccountNumber,
                                    //BeginingBalance = (a.BeginingBalance ?? 0),
                                    //AmountInsused = (a.AmountInsused ?? 0),
                                    //CurrencyId = a.CurrencyId,
                                    CurrencyCode = c.CurrencyCode,
                                    //ChartAccountId = a.ChartAccountId,
                                    ChartAccountName = d.Name5,
                                    //ChartAccountNumber = d.CodeRec,
                                    //LegalEntityId = a.LegalEntityId,
                                    LegalEntityName = e.LegalEntityName,
                                    //HolderUserId = f.NameEmployee,
                                    //DelegateUserId = g.NameEmployee,
                                    //CreatedOn = a.CreatedOn
                                }).ToListAsync();

            return result;
        }

        public async Task<PettyCash> GetMasterPettyCashById(string masterPettyCashId)
        {
            var result = await context.MasterPettyCashs.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == masterPettyCashId)
                .Select(x => new PettyCash
                {
                    CoaId = x.ChartAccountId,
                    LegalEntityId = x.LegalEntityId,
                    BankAccountNumber = x.BankAccountNumber,
                    CurrencyId = x.CurrencyId,
                    MasterPettyCashAmount = (context.Journal.Where(y => y.AccountId == x.ChartAccountId && y.LegalId == x.LegalEntityId && y.TransactionDate.Value.Year == DateTime.Now.Year).Sum(y => (y.Debit - y.Credit)) ?? 0),
                    HolderId = x.HolderUserId,//context.GetEmployeeId(),
                    HolderName = context.EmployeeBasicInfos.Where(y => y.Id == x.HolderUserId).Select(y => y.NameEmployee).FirstOrDefault(),
                    HolderName2 = context.EmployeeBasicInfos.Where(y => y.Id == x.DelegateUserId).Select(y => y.NameEmployee).FirstOrDefault(),
                    DivisionId = context.GetDivisionId(),
                    DivisionName = context.BusinessUnits.Where(z => z.Id == context.GetDivisionId()).Select(z => z.UnitName).FirstOrDefault()
                }).FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<Journal>> GetJournal(string Id)
        {
            var result = await (from a in context.JournalTemps
                                join b in context.LegalEntity on a.LegalId equals b.Id
                                where a.ReferenceId == Id
                                select new Journal
                                {
                                    TransactionDate = a.TransactionDate,
                                    AccountId = a.AccountId,
                                    AccountName = context.ChartOfAccounts.Where(x => x.Id == a.AccountId && x.Level == "5").Select(x => $"({x.CodeRec}) {x.Name5}").FirstOrDefault(),
                                    ReferenceNumber = a.ReferenceNumber,
                                    Description = a.Description,
                                    Debit = a.Debit,
                                    Credit = a.Credit,
                                    CreatedOn = a.CreatedOn,
                                    PceId = a.PceId,
                                    PcaId = a.PcaId,
                                    JobIdName = context.JobDetail.Where(x => x.Id == a.JobId).Select(x => $"{x.Id} - {x.JobName}").FirstOrDefault(),
                                    DivisiName = context.BusinessUnits.Where(x => x.Id == a.DivisiId).Select(x => x.UnitName).FirstOrDefault(),
                                    LegalEntityName = b.LegalEntityName,
                                    LineCode = a.LineCode
                                }).OrderByDescending(x => x.Debit).ThenByDescending(x => x.Credit).ToListAsync();
            return result;
        }

        public async Task<JobDetail> GetInfoPceByJobId(string jobid)
        {
            var info = await (
                from a in context.JobDetail
                join b in context.Pces on a.Id equals b.JobId
                where a.Id == jobid
                select new JobDetail()
                {
                    JobPeId = b.Code,
                    JobPaId = b.PcaId,
                    BusinessUnitDivisionId = a.BusinessUnitDivisionId
                }).FirstOrDefaultAsync();
            return info;
        }

        public async Task<dynamic> GetPcaTasks(string pcaId)
        {
            return await context.PcaTasks.Where(x => x.PcaId == pcaId).Select(x => new { Text = x.TaskName, Value = x.Id }).ToListAsync();
        }

        public async Task<int> AddAsync(PettyCash entity)
        {
            return await AddEdit(entity, false, false);
        }

        public async Task<int> EditAsync(PettyCash entity)
        {
            return await AddEdit(entity, true, false);
        }

        public async Task<int> CreateJournal(PettyCash entity)
        {
            await financePeriod.IsClosePeriod(Convert.ToDateTime(entity.SettlementDate), entity.LegalEntityId, Models.Enum.FinancePeriodCode.PCS);
            bool isEdit = false;
            if (!string.IsNullOrEmpty(entity.Id)) isEdit = true;
            return await AddEdit(entity, isEdit, true);
        }

        private async Task<int> AddEdit(PettyCash entity, bool isEdit, bool isJournal)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var file1 = await uploadFile.Upload(entity.File, null);
                    if (!string.IsNullOrEmpty(file1))
                    {
                        entity.FileMasterId = file1;
                    }

                    if (!string.IsNullOrEmpty(entity.Id))
                    {
                        var detail = context.PettyCashDetails.AsNoTracking().Where(x => x.PettyCashId == entity.Id).ToList();
                        if (detail.Count > 0)
                        {
                            context.PettyCashDetails.RemoveRange(detail);
                        }
                    }

                    if (isEdit)
                    {
                        context.PhoenixEdit(entity);
                    }
                    else
                    {
                        entity.Id = await TransID.GetTransId(context, Code.PettyCash, DateTime.Today);
                        await context.PhoenixAddAsync(entity);
                    }

                    if (entity.Detail != null)
                    {
                        // yg masuk ke detail petty cash hanya bagian credit untuk bagian debet otomatis ke isi by system
                        foreach (var item in entity.Detail)
                        {
                            item.Id = Guid.NewGuid().ToString();
                            item.PettyCashId = entity.Id;
                            context.PhoenixAdd(item);
                        }
                    }

                    await context.SaveChangesAsync();

                    var isApprove = true;
                    if (isJournal)
                    {
                        await context.Database.ExecuteSqlCommandAsync("EXECUTE fn.create_jurnal_pcs_temp @p_petty_cash_id", parameters: new[] { new SqlParameter("@p_petty_cash_id", entity.Id) });
                        var save = await context.SaveChangesAsync();
                        transaction.Commit();
                        return save;
                    }
                    else if (entity.Status != PettyCashStatus.Open)
                    {
                        var dataMenu = await menuService.GetByUniqueName(MenuUnique.PettyCash).ConfigureAwait(false);
                        if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
                        var vm = new Models.ViewModel.TransApprovalHrisVm()
                        {
                            MenuId = dataMenu.Id,
                            RefId = entity.Id,
                            DetailLink = $"{ApprovalLink.InvoiceClient}?Id={entity.Id}",
                            NotificationType = NotificationTypeCode.PettyCashNotification,
                            Module = NotificationModuleType.ModuleFinance
                        };
                        var subGroupId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessSubGroup(context.HttpContext);
                        var divisionId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessUnitDivisi(context.HttpContext);
                        var employeeId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(context.HttpContext);
                        isApprove = await globalFcApproval.Save(vm, subGroupId, divisionId, employeeId);
                        if (isApprove)
                        {
                            await log.AddAsync(new StatusLog() { TransactionId = entity.Id, Status = PettyCashStatus.StatusName(entity.Status), Description = entity.LogDescription });
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
                        context.PettyCashs.Update(entity);
                        await log.AddAsync(new StatusLog() { TransactionId = entity.Id, Status = PettyCashStatus.StatusName(entity.Status), Description = entity.LogDescription });
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

        public async Task<int> Reject(PettyCash model)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var save = await globalFcApproval.UnsafeProcessApproval(model.Id, StatusTransaction.Rejected, model.LogDescription, model);
                    var approval = context.TrTemplateApprovals.AsNoTracking().Where(x => x.RefId == model.Id && !(x.IsDeleted ?? false)).FirstOrDefault();
                    if (approval != null)
                    {
                        model.Status = PettyCashStatus.WAITING_APPROVAL;
                        if (approval.StatusApproved == Models.Um.StatusApproved.Rejected)
                        {
                            model.Status = PettyCashStatus.REVISE;
                            context.PhoenixEdit(model);
                            await log.AddAsync(new StatusLog() { TransactionId = model.Id, Status = PettyCashStatus.StatusName(model.Status), Description = model.LogDescription });
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

        public async Task<int> ApproveAsync(PettyCash model)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var save = await globalFcApproval.UnsafeProcessApproval(model.Id, StatusTransaction.Approved, model.LogDescription, model);
                    var approval = context.TrTemplateApprovals.AsNoTracking().Where(x => x.RefId == model.Id && !(x.IsDeleted ?? false)).FirstOrDefault();
                    if (approval != null)
                    {
                        model.Status = PettyCashStatus.WAITING_APPROVAL;
                        if (approval.StatusApproved == Models.Um.StatusApproved.Approved)
                        {
                            model.Status = PettyCashStatus.Approved;

                            //Add Payment Request 
                            var payId = await TransID.GetTransId(context, Code.PaymentRequest, DateTime.Today);
                            PaymentRequest pay = new PaymentRequest()
                            {
                                Id = payId,
                                PaymentRequestNumber = payId,
                                PaymentRequestDate = DateTime.Now,
                                //DueDate = model.CaPeriodTo,
                                //PaymentTypeId = model.CashAdvance,
                                //BankName ="",
                                //BankId ="",
                                //AffilicationId = model.AffiliationId,,
                                //AccountId = null,
                                CurrencyId = model.CurrencyId,
                                ExchangeRate = model.ExchangeRate,
                                LegalEntityId = model.LegalEntityId,
                                //BusinessUnitId = model.BusinessUnitId,
                                Status = PaymentRequestStatus.Open,
                                Charges = 0,
                                Amount = model.TotalCreditAmount,
                                AmountPay = model.TotalCreditAmount,
                                TotalAmountPay = model.TotalCreditAmount,
                                Vat = 0,
                                VatPercent = 0,
                                TaxPayable = 0,
                                TaxPayablePercent = 0,
                                AdvertisingTax = 0,
                                //InvoiceReceivableId="",
                                ReferenceNumberId = model.Id,
                                //PaytoName = model.VendorName,
                                Discount = 0,
                                TotalAmount = model.TotalCreditAmount,
                                //JobId = null
                                //PurchaseRequestId=null,
                                MainServiceCategoryId = MainServiceCategoryOther.MainServiceCategoryId,
                                SourceDoc = SourceDoc.PettyCash,
                                //Recon=null,
                                //TaxPayCoaId=null,
                                //PaytoBank = model.PayeeBankId,
                                PaytoCoaId = context.MapCoaTransaksi.Where(x => !(x.IsDeleted ?? false) && x.ModuleCode == SourceDoc.PettyCash && x.MainserviceCategoryId == MainServiceCategoryOther.MainServiceCategoryId && x.LineCode == "AP").Select(X => X.CoaId).FirstOrDefault(),
                                //BankChargesCoaId=null,
                                Balance = 0,
                                //IsMultiple=null,
                                //Remarks=null,
                                //PairingGroupId=null,
                                IsBalance = false,
                                //PayToPettyCash=null,
                                PayToAccountNumber = model.BankAccountNumber,
                                PayToType = "3", //Vendor
                                PayToVendor = model.LegalEntityId,
                                //AccountNumber = null,
                                //TaxPayableCoa=null,
                            };
                            await context.PhoenixAddAsync(pay);

                            var payDetail = new PaymentRequestDetail
                            {
                                Id = Guid.NewGuid().ToString(),
                                PaymentRequestId = payId,
                                CurrencyId = model.CurrencyId,
                                Qty = 1,
                                PayUnitPrice = model.TotalCreditAmount,
                                ReferenceId = model.Id,
                                AccountId = context.MapCoaTransaksi.Where(x => !(x.IsDeleted ?? false) && x.ModuleCode == SourceDoc.PettyCash && x.MainserviceCategoryId == MainServiceCategoryOther.MainServiceCategoryId && x.LineCode == "AP").Select(X => X.CoaId).FirstOrDefault(),
                                //JobId = null,
                                //Vat = null,
                                //TaxPayable = null,
                                SouceDoc = SourceDoc.PettyCash,
                                //PairingGroupId = null,
                                //TaxPayableCoaId = null
                            };

                            await context.PhoenixAddAsync(payDetail);

                            await context.Database.ExecuteSqlCommandAsync("EXECUTE fn.create_jurnal_pcs @p_petty_cash_id", parameters: new[] { new SqlParameter("@p_petty_cash_id", model.Id) });

                            context.PhoenixEdit(model);
                            await log.AddAsync(new StatusLog() { TransactionId = model.Id, Status = PettyCashStatus.StatusName(model.Status), Description = model.LogDescription });
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

        public bool IsHolder()
        {
            var employeeId = context.GetEmployeeId();
            var result = context.MasterPettyCashs.Where(x => !(x.IsDeleted ?? false) && (x.HolderUserId == employeeId || x.DelegateUserId == employeeId)).Count();
            return result > 0;
        }
    }
}
