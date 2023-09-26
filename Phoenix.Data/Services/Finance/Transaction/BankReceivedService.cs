using Phoenix.Data.Models.Finance;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Phoenix.Data.Models;
using Phoenix.Data.Services.Um;
using Phoenix.Data.Attributes;
using System.Data.SqlClient;
using Phoenix.Shared.Core.PrincipalHelpers;

namespace Phoenix.Data.Services.Finance
{
    public interface IBankReceivedService
    {
        Task<List<BankReceivedList>> GetBankReceivedList(FilterBankReceived search);
        Task<BankReceived> Get(string Id);
        Task<List<AccountReceivable>> GetListDetailAR(string bankReceivableId);
        Task<List<AccountReceivable>> GetListARPosted(PaymentRequestFilter Filter);
        Task<List<Journal>> GetBankReceivedJournal(string bankReceivableId);
        Task<Bank> GetAccountNumberData(string Id);
        Task<Bank> GetVendorData(string Id);
        Task<int> AddAsync(BankReceived entity);
        Task<int> EditAsync(BankReceived entity);
        Task<int> ApproveAsync(BankReceived entity);
        Task<int> RejectAsync(BankReceived entity);
        Task<BankReceived> Reconsiliation(BankReceived entity);
        Task<int> CreateJournalAsync(BankReceived entity);
        Task<int> UpdateJournaAsync(Journal entity);
        Task<List<BankReceivedDetailJournal>> GetEntryJournalList(string Id);
    }

    public class BankReceivedService : IBankReceivedService
    {
        readonly DataContext context;
        readonly GlobalFunctionApproval globalFcApproval;
        readonly IManageMenuService menuService;
        readonly FinancePeriodService financePeriod;
        readonly StatusLogService log;
        string idTemplate = "";
        /// <summary>
        /// And endpoint to manage PettyCash
        /// </summary>
        /// <param name="context">Database context</param>
        public BankReceivedService(DataContext context, GlobalFunctionApproval globalFcApproval, IManageMenuService menuService, FinancePeriodService financePeriod, StatusLogService log)
        {
            this.context = context;
            this.globalFcApproval = globalFcApproval;
            this.menuService = menuService;
            this.financePeriod = financePeriod;
            this.log = log;
        }

        public async Task<List<BankReceivedList>> GetBankReceivedList(FilterBankReceived search)
        {
            var webServerUrl = string.Format("{0}://{1}", context.HttpContext.Request.Scheme, context.configuration.GetSection("AppConfiguration").GetSection("WebServerUrl").Value);
            var month = (search.EndingDate.HasValue ? search.EndingDate.Value.Month : 0);
            var year = (search.EndingDate.HasValue ? search.EndingDate.Value.Year : 0);

            return await (from tb in context.BankReceived
                          join bu in context.BusinessUnits on tb.BusinessUnitId equals bu.Id
                          join le in context.LegalEntity on tb.LegalEntityId equals le.Id
                          where !(tb.IsDeleted ?? false)
                          && (string.IsNullOrEmpty(search.Status) ? true : tb.Status == search.Status)
                          && (search.DateType == "Day" ? (search.EndingDate == null ? true : tb.PaymentRequestDate == search.EndingDate) :
                            search.DateType == "Month" ? (search.EndingDate == null ? true : (tb.PaymentRequestDate.HasValue ? tb.PaymentRequestDate.Value.Year : 0) == year && (tb.PaymentRequestDate.HasValue ? tb.PaymentRequestDate.Value.Month : 0) == month) :
                            search.DateType == "Year" ? (search.EndingDate == null ? true : (tb.PaymentRequestDate.HasValue ? tb.PaymentRequestDate.Value.Year : 0) == year) :
                            search.DateType == "Custom Range" ? (search.StartingDate == null && search.EndingDate == null ? true : tb.PaymentRequestDate >= search.StartingDate && tb.PaymentRequestDate <= search.EndingDate) : true)
                          select new BankReceivedList
                          {
                              Id = tb.Id,
                              BusinessUnitName = context.BusinessUnits.Where(x => x.Id == tb.BusinessUnitId).Select(x => x.UnitName).FirstOrDefault(),
                              PaymentTypeName = context.PaymentType.Where(x => x.Id == tb.PaymentTypeId).Select(x => x.TypeName).FirstOrDefault(),
                              Status = tb.Status,
                              LegalEntityName = le.LegalEntityName,
                              PaymentRequestDate = tb.PaymentRequestDate,
                              PaymentRequestNumber = tb.PaymentRequestNumber,
                              ReferenceNumberId = !string.IsNullOrEmpty(tb.ReferenceNumberId) ?
                                                    string.Join(",", context.AccountReceivables.Where(x => (tb.ReferenceNumberId ?? "").Contains(x.Id))
                                                    .Select(x => $"<a href='{webServerUrl}/Finance/AccountReceivable/Detail?id={x.Id}' title='View' class='edit'><span style='color:blue; text-decoration:underline; word-wrap: break-word'>{x.Id}</span></a>").ToArray())
                                                    : "",
                              AmountPay = tb.AmountPay,
                              JobName = tb.JobId == null ? tb.PurchaseRequestId : string.Join(",<br/>", context.JobDetail.Where(x => tb.JobId.Contains(x.Id)).Select(x => $"{x.Id}-{x.JobName}").ToArray()),
                              CurrencyName = string.IsNullOrEmpty(tb.ReferenceNumberId) ?
                                            context.Currencies.Where(x => x.Id == tb.CurrencyId).Select(x => x.CurrencyName).FirstOrDefault() :
                                            (from a in context.AccountReceivables
                                             join b in context.Currencies on a.CurrencyId equals b.Id
                                             where tb.ReferenceNumberId.Contains(a.Id)
                                             select b.CurrencyName).FirstOrDefault(),
                              VendorName = context.Companys.Where(x => x.Id == tb.PayToVendor).Select(x => x.CompanyName).FirstOrDefault()
                          }).ToListAsync();
        }

        public async Task<BankReceived> Get(string Id)
        {
            var result = await context.BankReceived.Where(x => x.Id == Id).FirstOrDefaultAsync();
            if (result != null)
            {
                result.JobName = result.JobId == null ? result.PurchaseRequestId : context.JobDetail.Where(x => x.Id == result.JobId).Select(x => $"{x.Id}-{x.JobName}").FirstOrDefault();//(result.JobId ?? result.PurchaseRequestId);
                result.LegalEntityName = await context.LegalEntity.Where(x => x.Id == result.LegalEntityId).Select(x => x.LegalEntityName).FirstOrDefaultAsync();
                result.Details = await context.BankReceivedDetail.Where(x => x.IsDeleted.Equals(false) && x.BankReceivedId == Id).ToListAsync();
                result.CurrencySymbol = context.Currencies.Where(x => x.Id == result.CurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault();
                result.CurrencyIdDefault = (context.JournalTemps.Where(x => x.ReferenceId == Id).Select(x => x.DefaultCurrencyId)).FirstOrDefault() ??
                                            context.Currencies.Where(x => !(x.IsDeleted ?? false) && x.IsDefault == true).Select(x => x.Id).FirstOrDefault();
                result.CurrencySymbolDefault = (from o in context.JournalTemps
                                                join p in context.Currencies on o.DefaultCurrencyId equals p.Id
                                                where o.ReferenceId == Id
                                                select p.CurrencySymbol).FirstOrDefault() ??
                                                context.Currencies.Where(x => !(x.IsDeleted ?? false) && x.IsDefault == true).Select(x => x.CurrencySymbol).FirstOrDefault();
            }
            return result;
        }

        public async Task<List<AccountReceivable>> GetListDetailAR(string bankReceivableId)
        {
            var result = await (from tb in context.BankReceivedDetail
                                join ar in context.AccountReceivables on tb.AccountReceivebleNumber equals ar.Id
                                //join bu in context.BusinessUnits on ar.BusinessUnitId equals bu.Id
                                //join job in context.JobDetail on tb.JobId equals job.Id
                                join ve in context.Companys on ar.ClientId equals ve.Id
                                where !(tb.IsDeleted ?? false) && tb.BankReceivedId == bankReceivableId
                                select new AccountReceivable
                                {
                                    Id = ar.Id,
                                    AccountReceivableNumber = ar.AccountReceivableNumber,
                                    TransactionDate = ar.TransactionDate,
                                    BankAccountDestination = ar.BankAccountDestination,
                                    Amount = ar.Amount,
                                    LegalEntityId = ar.LegalEntityId,
                                    LegalEntity = context.LegalEntity.Where(x => x.Id == tb.LegalEntityId).Select(x => x.LegalEntityName).FirstOrDefault(),
                                    ClientId = ar.ClientId,
                                    ClientName = ve.CompanyName,
                                    Status = "",
                                    JobId = ar.JobId,
                                    CurrencyId = ar.CurrencyId,
                                    //JobName = job.Id + " - " + job.JobName,
                                    //JobName = ar.JobId == null ? "" : string.Join(",<br/>", context.JobDetail.Where(x => tb.JobId.Contains(x.Id)).Select(x => $"{x.Id}-{x.JobName}").ToArray()),
                                    BusinessUnitId = ar.BusinessUnitId,
                                    //BusinessUnitName = bu.UnitName,
                                    BusinessUnitName = context.BusinessUnits.Where(x => x.Id == ar.BusinessUnitId).Select(x => x.UnitName).FirstOrDefault(),
                                    BrandId = ar.BrandId,
                                    //ClientBrand = context.CompanyBrands.Where(x => x.Id == tb.BrandId).Select(x => x.BrandName).FirstOrDefault(),
                                    //ClientBrand = string.Join(',', context.CompanyBrands.Where(x => tb.BrandId.Contains(x.Id)).Select(x => x.BrandName).ToArray()),
                                    AffilitionId = ar.AffilitionId,
                                    CurrencyName = context.Currencies.Where(x => x.Id == ar.CurrencyId).Select(x => x.CurrencyName).FirstOrDefault(),
                                    CurrencySymbol = context.Currencies.Where(x => x.Id == ar.CurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault()
                                }).OrderBy(x => x.CreatedOn).ToListAsync();

            foreach (var item in result)
            {
                item.ClientBrand = string.Join(',', context.CompanyBrands.Where(x => item.BrandId.Contains(x.Id)).Select(x => x.BrandName ?? "").ToArray());
                item.JobName = item.JobId == null ? "" : string.Join(",<br/>", context.JobDetail.Where(x => item.JobId.Contains(x.Id)).Select(x => $"{x.Id}-{x.JobName}").ToArray());
            };

            return result;
        }

        public async Task<List<AccountReceivable>> GetListARPosted(PaymentRequestFilter Filter)
        {
            var bankReceived = (from a in context.BankReceived
                                join b in context.BankReceivedDetail on a.Id equals b.BankReceivedId
                                where !(a.IsDeleted ?? false)
                                select new { a.Id, a.Status, b.AccountReceivebleNumber });

            var result = await (from ar in context.AccountReceivables
                                    //join bu in context.BusinessUnits on ar.BusinessUnitId equals bu.Id
                                join le in context.LegalEntity on ar.LegalEntityId equals le.Id
                                //join job in context.JobDetail on ar.JobId equals job.Id
                                join client in context.Companys on ar.ClientId equals client.Id
                                //join h in context.CompanyBrands on ar.BrandId equals h.Id
                                where !(ar.IsDeleted ?? false) && ar.Status == ARStatus.Posted
                                && (!bankReceived.Any(x => x.AccountReceivebleNumber == ar.AccountReceivableNumber && x.Status != PaymentRequestStatus.Complete && (string.IsNullOrEmpty(Filter.PaymentRequestId) ? true : x.Id != Filter.PaymentRequestId)))
                                && (string.IsNullOrEmpty(Filter.FilterLegalEntity) ? true : ar.LegalEntityId == Filter.FilterLegalEntity)
                                && (string.IsNullOrEmpty(Filter.FilterBusinessUnitId) ? true : ar.BusinessUnitId == Filter.FilterBusinessUnitId)
                                && (string.IsNullOrEmpty(Filter.VendorId) ? true : ar.ClientId == Filter.VendorId)
                                && (string.IsNullOrEmpty(Filter.CurrencyId) ? true : ar.CurrencyId == Filter.CurrencyId)
                                && (string.IsNullOrEmpty(Filter.FilterARNumber) ? true : ar.AccountReceivableNumber == Filter.FilterARNumber)
                                select new AccountReceivable
                                {
                                    Id = ar.Id,
                                    AccountReceivableNumber = ar.AccountReceivableNumber,
                                    TransactionDate = ar.TransactionDate,
                                    BankAccountDestination = ar.BankAccountDestination,
                                    Amount = ar.Amount,
                                    LegalEntityId = ar.LegalEntityId,
                                    LegalEntity = le.LegalEntityName,
                                    ClientId = ar.ClientId,
                                    ClientName = client.CompanyName,
                                    Status = "",
                                    JobId = ar.JobId,
                                    CurrencyId = ar.CurrencyId,
                                    //JobName = job.Id + " - " + job.JobName,
                                    //JobName = ar.JobId == null ? "" : string.Join(",<br/>", context.JobDetail.Where(x => ar.JobId.Contains(x.Id)).Select(x => $"{x.Id}-{x.JobName}").ToArray()),
                                    BusinessUnitId = ar.BusinessUnitId,
                                    //BusinessUnitName = bu.UnitName,
                                    BusinessUnitName = context.BusinessUnits.Where(x => x.Id == ar.BusinessUnitId).Select(x => x.UnitName).FirstOrDefault(),
                                    BrandId = ar.BrandId,
                                    //ClientBrand = h.BrandName,
                                    //ClientBrand = string.Join(',', context.CompanyBrands.Where(x => ar.BrandId.Contains(x.Id)).Select(x => x.BrandName).ToArray()),
                                    AffilitionId = ar.AffilitionId,
                                    CurrencyName = context.Currencies.Where(x => x.Id == ar.CurrencyId).Select(x => x.CurrencyName).FirstOrDefault(),
                                    AccountId = context.Journal.Where(x => x.ReferenceId == ar.Id && x.LineCode == "1").Select(x => x.AccountId).FirstOrDefault(),
                                    ExchangeRate = ar.ExchangeRate,
                                    CurrencySymbol = context.Currencies.Where(x => x.Id == ar.CurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault(),
                                    CurrencyIdDefault = context.Currencies.Where(x => !(x.IsDeleted ?? false) && x.IsDefault == true).Select(x => x.Id).FirstOrDefault(),
                                    CurrencySymbolDefault = context.Currencies.Where(x => !(x.IsDeleted ?? false) && x.IsDefault == true).Select(x => x.CurrencySymbol).FirstOrDefault()
                                }).ToListAsync();

            foreach (var item in result)
            {
                item.ClientBrand = string.Join(',', context.CompanyBrands.Where(x => item.BrandId.Contains(x.Id)).Select(x => x.BrandName ?? "").ToArray());
                item.JobName = item.JobId == null ? "" : string.Join(",<br/>", context.JobDetail.Where(x => item.JobId.Contains(x.Id)).Select(x => $"{x.Id}-{x.JobName}").ToArray());
            };

            return result;
        }


        public async Task<List<Journal>> GetBankReceivedJournal(string bankReceivableId)
        {
            var result = await (from tb in context.JournalTemps
                                join coa in context.ChartOfAccounts on tb.AccountId equals coa.Id
                                where tb.ReferenceId == bankReceivableId
                                select new Journal
                                {
                                    Id = tb.Id,
                                    ReferenceId = tb.ReferenceId,
                                    AccountId = coa.Id,
                                    CodeRec = coa.CodeRec,
                                    AccountName = coa.Name5,
                                    JobId = tb.JobId,
                                    Description = tb.Description == null ? coa.Name5 : tb.Description,
                                    JobIdName = context.JobDetail.Where(x => x.Id == tb.JobId).Select(x => $"{x.Id} - {x.JobName}").FirstOrDefault(),
                                    DivisiId = tb.DivisiId,
                                    DivisiName = context.BusinessUnits.Where(x => x.Id == tb.DivisiId).Select(x => x.UnitName).FirstOrDefault(),
                                    LegalEntityName = context.LegalEntity.Where(x => x.Id == tb.LegalId).Select(x => x.LegalEntityName).FirstOrDefault(),
                                    Credit = tb.Credit,
                                    Debit = tb.Debit,
                                    OriginalAmount = tb.OriginalAmount,
                                    LineCode = tb.LineCode,
                                    LegalId = tb.LegalId,
                                    CurrencySymbol = context.Currencies.Where(x => x.Id == tb.DefaultCurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault(),
                                    CurrencySymbolOriginal = context.Currencies.Where(x => x.Id == tb.CurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault()
                                }).OrderByDescending(x => x.Debit).ThenByDescending(x => x.Credit).ToListAsync();
            return result;
        }

        public async Task<Bank> GetAccountNumberData(string Id)
        {
            return await (from bank in context.Banks
                          join leg in context.LegalEntity on bank.LegalEntityId equals leg.Id
                          where !(bank.IsDeleted ?? false) && bank.Id == Id
                          select new Bank
                          {
                              AccountNumber = bank.AccountNumber,
                              LegalEntityId = bank.LegalEntityId,
                              LegalEntityName = leg.LegalEntityName,
                              SwiftCode = bank.SwiftCode,
                              CoaId = bank.CoaId,
                              CurrencyId = bank.CurrencyId
                          }).FirstOrDefaultAsync();
        }

        public async Task<Bank> GetVendorData(string Id)
        {
            return await (from pet in context.Companys
                          join leg in context.Banks on pet.BankId equals leg.Id
                          where !(pet.IsDeleted ?? false) && pet.Id == Id
                          select new Bank
                          {
                              AccountNumber = pet.AccountNumber,
                              LegalEntityId = leg.LegalEntityId,
                              CoaId = leg.CoaId,
                              BankName = leg.BankName
                          }).FirstOrDefaultAsync();
        }

        public async Task<int> AddAsync(BankReceived entity)
        {
            return await AddEditAsync(entity, false, false);
        }

        public async Task<int> EditAsync(BankReceived entity)
        {
            return await AddEditAsync(entity, true, false);
        }

        private async Task<int> AddEditAsync(BankReceived entity, bool isEdit, bool isJournal)
        {
            if (isJournal)
            {
                await financePeriod.IsClosePeriod(Convert.ToDateTime(entity.PaymentRequestDate), entity.LegalEntityId, Models.Enum.FinancePeriodCode.BR);
            }

            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    //dikasih validasi ini dikarenakan ada kasus di mana business unit id gk kesimpan
                    if (entity.MainServiceCategoryId != MainServiceCategoryOther.MainServiceCategoryId)
                    {
                        if (string.IsNullOrEmpty(entity.BusinessUnitId)) { throw new Exception("business unit cannot be empty."); }
                    }

                    if (entity.Details != null)
                    {
                        entity.ReferenceNumberId = string.Join(",", entity.Details.Select(x => x.AccountReceivebleNumber).ToArray());
                        entity.JobId = string.Join(",", entity.Details.Select(x => x.JobId).ToArray());

                        if (entity.Details.Count > 1)
                        {
                            entity.IsMultiple = true;
                        }
                        else
                        {
                            entity.IsMultiple = false;
                        }
                    }

                    if (string.IsNullOrEmpty(entity.Id))
                    {
                        var payId = await TransID.GetTransId(context, Code.BankReceived, DateTime.Today);
                        entity.TempHeaderId = entity.Id;
                        entity.Id = payId;
                        entity.PaymentRequestNumber = payId;
                        await context.PhoenixAddAsync(entity);
                        await context.SaveChangesAsync();
                    }
                    else
                    {
                        context.PhoenixEdit(entity);

                        var detailJournal = context.BankReceivedDetailJournal.AsNoTracking().Where(x => x.BankReceivedId == entity.Id).ToList();
                        if (detailJournal.Count > 0)
                        {
                            context.BankReceivedDetailJournal.RemoveRange(detailJournal);
                        }
                    }

                    if (entity.JournalEntry != null)
                    {
                        foreach (var item in entity.JournalEntry)
                        {
                            if (string.IsNullOrEmpty(item.Id))
                            {
                                item.Id = Guid.NewGuid().ToString();
                            }

                            item.BankReceivedId = entity.Id;
                            await context.PhoenixAddAsync(item);
                        };
                    }

                    if (entity.Details != null)
                    {
                        if (entity.Details.Count > 0)
                        {
                            var delmod = await context.BankReceivedDetail.AsNoTracking().Where(x => x.IsDeleted.Equals(false) && x.BankReceivedId == entity.Id).ToListAsync();
                            if (delmod.Count > 0)
                            {
                                context.BankReceivedDetail.RemoveRange(delmod.ToArray());
                            }

                            foreach (BankReceivedDetail data in entity.Details)
                            {
                                data.Id = Guid.NewGuid().ToString();
                                data.BankReceivedId = entity.Id;
                                //data.PairingGroupId = entity.PairingGroupId;
                                data.AccountId = await context.Journal.AsNoTracking().Where(x => x.ReferenceId == data.AccountReceivebleNumber && x.LineCode == "1").Select(x => x.AccountId).FirstOrDefaultAsync();
                                await context.PhoenixAddAsync(data);
                            }
                        }
                    }

                    if (isJournal)
                    {
                        await context.SaveChangesAsync();
                        var result = await context.Database.ExecuteSqlCommandAsync("EXECUTE fn.create_jurnal_bank_received @p_bank_received_id", parameters: new[] { new SqlParameter("@p_bank_received_id", entity.Id) });
                    }

                    if (entity.Status == PaymentRequestStatus.WaitingApproval)
                    {
                        var dataMenu = await menuService.GetByUniqueName(MenuUnique.BankReceived).ConfigureAwait(false);
                        if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
                        var vm = new Models.ViewModel.TransApprovalHrisVm()
                        {
                            MenuId = dataMenu.Id,
                            RefId = entity.Id,
                            DetailLink = $"{ApprovalLink.BankReceived}?Id={entity.Id}&isApprove=true",
                            NotificationType = NotificationTypeCode.BankReceivedNotification,
                            Module = NotificationModuleType.ModuleFinance
                        };
                        var subGroupId = ClainJwtPrincipalHelper.GetBusinessSubGroup(context.HttpContext);
                        var divisionId = ClainJwtPrincipalHelper.GetBusinessUnitDivisi(context.HttpContext);
                        var employeeId = ClainJwtPrincipalHelper.GetEmployeeId(context.HttpContext);
                        var isApprove = await globalFcApproval.Save(vm, subGroupId, divisionId, employeeId);
                        if (isApprove)
                        {
                            await log.AddAsync(new StatusLog() { TransactionId = entity.Id, Status = PaymentRequestStatus.GetStatus(entity.Status), Description = entity.LogDescription });
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
                        await log.AddAsync(new StatusLog() { TransactionId = entity.Id, Status = PaymentRequestStatus.GetStatus(entity.Status), Description = entity.LogDescription });
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

        public async Task<int> ApproveAsync(BankReceived model)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var save = await globalFcApproval.UnsafeProcessApproval(model.Id, StatusTransaction.Approved, model.LogDescription, model);
                    var approval = context.TrTemplateApprovals.AsNoTracking().Where(x => x.RefId == model.Id && !(x.IsDeleted ?? false)).FirstOrDefault();
                    if (approval != null)
                    {
                        model.Status = PaymentRequestStatus.WaitingApproval;
                        if (approval.StatusApproved == Models.Um.StatusApproved.Approved)
                        {
                            model.Status = PaymentRequestStatus.Complete;
                            await context.Database.ExecuteSqlCommandAsync("EXECUTE fn.store_data_jurnal @p_id", parameters: new[] { new SqlParameter("@p_id", model.Id) });
                            if (model.PayToType == "1")//PETTYCASH
                            {
                                var modelPettycah = await context.MasterPettyCashs.Where(x => x.IsDeleted.Equals(false) && x.Id == model.PayToPettyCash).FirstOrDefaultAsync();
                                modelPettycah.AmountInsused = (modelPettycah.AmountInsused == null ? 0 : modelPettycah.AmountInsused) + model.AmountPay;
                                context.PhoenixEdit(modelPettycah);
                            }
                        }

                        context.PhoenixEdit(model);
                        await log.AddAsync(new StatusLog() { TransactionId = model.Id, Status = PaymentRequestStatus.GetStatus(model.Status), Description = model.LogDescription });
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

        public async Task<int> RejectAsync(BankReceived model)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var save = await globalFcApproval.UnsafeProcessApproval(model.Id, StatusTransaction.Rejected, model.LogDescription, model);
                    var approval = context.TrTemplateApprovals.AsNoTracking().Where(x => x.RefId == model.Id && !(x.IsDeleted ?? false)).FirstOrDefault();
                    if (approval != null)
                    {
                        model.Status = PaymentRequestStatus.WaitingApproval;
                        if (approval.StatusApproved == Models.Um.StatusApproved.Rejected)
                        {
                            model.Status = PaymentRequestStatus.Reject;
                        }

                        context.PhoenixEdit(model);
                        await log.AddAsync(new StatusLog() { TransactionId = model.Id, Status = PaymentRequestStatus.GetStatus(model.Status), Description = model.LogDescription });
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

        public async Task<BankReceived> Reconsiliation(BankReceived entity)
        {
            entity.Status = PaymentRequestStatus.Reconsiliation;
            context.PhoenixEdit(entity);
            await log.AddAsync(new StatusLog() { TransactionId = entity.Id, Status = PaymentRequestStatus.GetStatus(PaymentRequestStatus.Reconsiliation), Description = entity.LogDescription });
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<int> CreateJournalAsync(BankReceived entity)
        {
            bool isEdit = false;
            if (!string.IsNullOrEmpty(entity.Id)) isEdit = true;
            return await AddEditAsync(entity, isEdit, true);

            //var result = await context.Database.ExecuteSqlCommandAsync("EXECUTE fn.create_jurnal_payment_request @p_payment_request_id", parameters: new[] { new SqlParameter("@p_payment_request_id", BankReceivedId) });
            //var save = await context.SaveChangesAsync();
            //return save;
        }

        public async Task<int> UpdateJournaAsync(Journal entity)
        {
            context.Database.ExecuteSqlCommand($"UPDATE fn.journal_temp SET Description={entity.Description} WHERE id={entity.Id}");
            return await context.SaveChangesAsync();
        }

        public async Task<List<BankReceivedDetailJournal>> GetEntryJournalList(string Id)
        {
            return await (from a in context.BankReceivedDetailJournal
                          join b in context.ChartOfAccounts on a.CoaId equals b.Id
                          join c in context.BankReceived on a.BankReceivedId equals c.Id
                          where a.BankReceivedId == Id
                          select new BankReceivedDetailJournal
                          {
                              Id = a.Id,
                              BankReceivedId = a.BankReceivedId,
                              CoaId = a.CoaId,
                              Description = a.Description,
                              Condition = a.Condition,
                              Debit = a.Debit,
                              Credit = a.Credit,
                              TransactionDate = a.TransactionDate,
                              //CodeRec = b.CodeRec,
                              AccountName = $"({b.CodeRec}) {b.Name5}",
                              CreatedBy = a.CreatedBy,
                              CreatedOn = a.CreatedOn
                          }).ToListAsync();
        }

    }
}
