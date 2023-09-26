using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Phoenix.Data.Attributes;
using System.Linq.Dynamic.Core;
using Microsoft.Graph;
using Phoenix.Data.Models.Enum;

namespace Phoenix.Data.Services.Finance.Transaction
{
    public interface IFixedAssetDepreciationService : IDataService<FixedAssetDepreciation>
    {
        Task<List<FixedAssetDepreciationYears>> GetByYearDetail(string Id);
        Task<List<FixedAssetDepreciationDetail>> GetByMonthDetail(string Id);
        Task<List<Journal>> GetOpeningBalance(string Id);
        Task<int> AddEditAsync(FixedAssetDepreciation entity, bool isEdit);
        Task<int> PostJournalAsync(FixedAssetDepreciationJournal entity);
        DateTime GetPostingDate(string legalEntityId);
    }
    public class FixedAssetDepreciationService : IFixedAssetDepreciationService
    {
        readonly DataContext context;
        readonly GlobalFunctionApproval globalFcApproval;
        readonly IFixedAssetService fixedAssetService;
        string idTemplate = "";

        public FixedAssetDepreciationService(DataContext context, GlobalFunctionApproval globalFcApproval, IFixedAssetService fixedAssetService)
        {
            this.context = context;
            this.globalFcApproval = globalFcApproval;
            this.fixedAssetService = fixedAssetService;
        }

        public async Task<List<FixedAssetDepreciation>> Get()
        {
            var fixedAsset = context.FixedAssetDepreciation
                .Where(fad => !(fad.IsDeleted ?? false))
                .Select(fad => new FixedAssetDepreciation
                {
                    Id = fad.Id,
                    FixedAssetID = fad.FixedAssetID,
                    AcquisitionAmount = fad.AcquisitionAmount,
                    DepreciationValue = fad.DepreciationValue,
                    AccumulationDepreciation = context.FixedAssetDepreciationDetail.Where(x => x.FixedAssetDepreciationId == fad.Id && (x.Status ?? "") == "POSTED").OrderBy(x => x.FirstPeriode).Select(x => x.AccumulationDepreciation).FirstOrDefault() ?? 0,
                    BookValue = context.FixedAssetDepreciationDetail.Where(x => x.FixedAssetDepreciationId == fad.Id && (x.Status ?? "") == "POSTED").OrderBy(x => x.FirstPeriode).Select(x => x.BookValue).FirstOrDefault() ?? fad.AcquisitionAmount,
                    Status = context.FixedAssetDepreciationDetail.Where(x => x.FixedAssetDepreciationId == fad.Id && (x.Status ?? "") == "POSTED").OrderBy(x => x.FirstPeriode).Select(x => x.Status).FirstOrDefault() ?? "",
                    AcquisitonDate = context.FixedAssetDepreciationDetail.Where(x => x.FixedAssetDepreciationId == fad.Id && (x.Status ?? "") != "POSTED" && x.FirstPeriode.Value.Year >= x.CreatedOn.Value.Year).OrderBy(x => x.FirstPeriode).Select(x => x.FirstPeriode).FirstOrDefault() ??
                                                     context.FixedAssetDepreciationDetail.Where(x => x.FixedAssetDepreciationId == fad.Id && (x.Status ?? "") == "POSTED").OrderByDescending(x => x.FirstPeriode).Select(x => x.FirstPeriode).FirstOrDefault(),
                });

            var result = await (from fad in fixedAsset
                                join fa in context.FixedAsset on fad.FixedAssetID equals fa.Id
                                //join bu in context.BusinessUnits on fa.BusinessUnitId equals bu.Id
                                join lg in context.LegalEntity on fa.LegalEntityId equals lg.Id
                                join cat in context.ItemCategories on fa.ItemCategoryId equals cat.Id
                                join curr in context.Currencies on fa.CurrencyId equals curr.Id
                                select new FixedAssetDepreciation
                                {
                                    Id = fad.Id,
                                    AcquisitionAmount = fad.AcquisitionAmount,
                                    DepreciationValue = fad.DepreciationValue,
                                    AccumulationDepreciation = fad.AccumulationDepreciation,
                                    BookValue = fad.BookValue,
                                    Status = fad.Status,
                                    AcquisitonDate = fad.AcquisitonDate.Value.AddMonths(1).AddDays((-fad.AcquisitonDate.Value.Day)),
                                    FixedAsset = new FixedAsset
                                    {
                                        FixedAssetNumber = fa.FixedAssetNumber,
                                        FixedAssetName = fa.FixedAssetName,
                                        LegalEntityName = lg.LegalEntityName,
                                        BusinessUnitName = context.BusinessUnits.Where(x => x.Id == fa.BusinessUnitId).Select(x => x.UnitName).FirstOrDefault(),//bu.UnitName,
                                        ItemCategoryName = cat.CategoryName,
                                        CurrencyName = curr.CurrencyName
                                    }
                                }).ToListAsync();
            return result;
        }

        public async Task<FixedAssetDepreciation> Get(string Id)
        {
            var result = await context.FixedAssetDepreciation.Where(x => x.Id == Id).FirstOrDefaultAsync();
            if (result == null)
            {
                result.FixedAsset = new FixedAsset();
            }
            else
            {
                result.FixedAsset = await fixedAssetService.Get(result.FixedAssetID);
            }

            result.Status = context.FixedAssetDepreciationDetail.Where(x => x.FixedAssetDepreciationId == Id && (x.Status ?? "") == "POSTED").OrderByDescending(x => x.FirstPeriode).Select(x => x.Status).FirstOrDefault();
            return result;
        }

        public async Task<List<FixedAssetDepreciationYears>> GetByYearDetail(string Id)
        {
            var result = new List<FixedAssetDepreciationYears>();
            if (string.IsNullOrEmpty(Id)) return result;

            var model = await Get(Id);
            int counter = 0;
            int counterYearTo = 1;

            if (model.FixedAsset.RegistrationDate == null) throw new Exception("Asset Registration Date Cannot be empty.");
            var date = model.FixedAsset.RegistrationDate.Value;
            if (date.Day > 15) { date = date.AddMonths(1).AddDays((-date.Day + 1)); }

            int lifeTime = Convert.ToInt32(model.LifeTime);
            //int batasLifeTime = 1;
            decimal AccuirdeMonth = Convert.ToDecimal(model.AcquisitionAmount);
            int Residue = Convert.ToInt32(model.Residual);
            decimal EndBookyear = 0;

            decimal DepMonth = Convert.ToDecimal(model.AcquisitionAmount);
            decimal Depreciationpermonth = (DepMonth - Residue) / lifeTime;
            string DepreciationpermonthConvert = String.Format("{0:0.00}", Depreciationpermonth);

            decimal DepreciationPerYear = 0;
            decimal rate = 0;
            decimal InitialBookValue = 0;
            int TotalMonth = 0;

            int kelipatan = lifeTime / 12;
            int sisa = lifeTime % 12;
            for (int i = 1; i <= kelipatan; i++)
            {
                FixedAssetDepreciationYears data = new FixedAssetDepreciationYears();
                DateTime firstPeriode = date.AddYears(counter);
                int yearEnd = firstPeriode.AddMonths(11).Year;
                int monthEnd = firstPeriode.AddMonths(11).Month;
                DateTime endPeriode = new DateTime(yearEnd, monthEnd, DateTime.DaysInMonth(yearEnd, monthEnd));

                InitialBookValue = EndBookyear == 0 ? AccuirdeMonth : EndBookyear;
                TotalMonth = Math.Abs(((firstPeriode.Year - endPeriode.Year) * 12) + firstPeriode.Month - endPeriode.Month) + 1;
                DepreciationPerYear = Convert.ToDecimal((Depreciationpermonth * TotalMonth));
                rate = (DepreciationPerYear / AccuirdeMonth) * 100;
                EndBookyear = InitialBookValue - DepreciationPerYear;

                data.FirstPeriode = firstPeriode;
                data.EndPeriode = endPeriode;
                data.TotalMonth = TotalMonth;
                data.InitialBookValue = InitialBookValue;
                data.Rate = Math.Round(rate, 0);
                data.DepreciationYear = DepreciationPerYear;
                data.DepreciationMonth = Convert.ToDecimal(DepreciationpermonthConvert);
                data.EndBookYear = EndBookyear;
                data.YearTo = counterYearTo;
                result.Add(data);

                counter++;
                counterYearTo++;
            }

            if (sisa > 0)
            {
                FixedAssetDepreciationYears data = new FixedAssetDepreciationYears();
                int monthFirst = date.AddMonths(12).Month;
                int yearFirst = date.AddYears(counter).Year;
                int monthEnd = 0;
                int yearEnd = 0;
                if (sisa == 1)
                {
                    monthEnd = date.AddMonths(sisa - 1).Month;
                    yearEnd = date.AddYears(counter).Year;
                }
                else
                {
                    monthEnd = date.AddMonths(sisa - 1).Month;
                    yearEnd = date.AddYears(counter + 1).Year;
                }


                InitialBookValue = EndBookyear;
                DateTime firstPeriode = new DateTime(yearFirst, monthFirst, 01);
                DateTime endPeriode = new DateTime(yearEnd, monthEnd, DateTime.DaysInMonth(yearEnd, monthEnd));
                TotalMonth = sisa;
                DepreciationPerYear = Depreciationpermonth * TotalMonth;
                rate = (DepreciationPerYear / AccuirdeMonth) * 100;
                EndBookyear = InitialBookValue - DepreciationPerYear;


                data.FirstPeriode = firstPeriode;
                data.EndPeriode = endPeriode;
                data.TotalMonth = TotalMonth;
                data.InitialBookValue = InitialBookValue;
                data.Rate = Math.Round(rate, 0);
                data.DepreciationYear = DepreciationPerYear;
                data.DepreciationMonth = Convert.ToDecimal(DepreciationpermonthConvert);
                data.EndBookYear = EndBookyear;
                data.YearTo = counterYearTo;
                result.Add(data);
            }
            return result;
        }

        public async Task<List<FixedAssetDepreciationDetail>> GetByMonthDetail(string Id)
        {
            var result = await (from a in context.FixedAssetDepreciationDetail
                                join b in context.FixedAsset on a.FixedAssetID equals b.Id
                                join c in context.LegalEntity on b.LegalEntityId equals c.Id
                                join d in context.BusinessUnits on b.BusinessUnitId equals d.Id
                                join e in context.ItemCategories on b.ItemCategoryId equals e.Id
                                where a.FixedAssetDepreciationId == Id
                                select new FixedAssetDepreciationDetail
                                {
                                    Id = a.Id,
                                    JVNumber = a.JVNumber,
                                    FixedAssetID = a.FixedAssetID,
                                    FixedAssetDepreciationId = a.FixedAssetDepreciationId,
                                    FirstPeriode = a.FirstPeriode,
                                    BookValue = a.BookValue,
                                    DepreciationMonth = a.DepreciationMonth,
                                    AcquisitionAmount = a.AcquisitionAmount,
                                    AccumulationDepreciation = a.AccumulationDepreciation,
                                    Status = a.Status,
                                    FixedAssetName = b.FixedAssetName,
                                    ItemCategoryName = e.CategoryName,
                                    LegalEntityName = c.LegalEntityName,
                                    BusinessUnitName = d.UnitName
                                }).ToListAsync();
            return result;
        }

        public async Task<int> AddEditAsync(FixedAssetDepreciation entity, bool isEdit)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var itemcat = await (from a in context.FixedAsset
                                         join b in context.FixedAssetDepreciation on a.Id equals b.FixedAssetID
                                         join c in context.ItemCategories on a.ItemCategoryId equals c.Id
                                         where b.Id == entity.Id
                                         select c.CategoryName).FirstOrDefaultAsync();
                    if (entity.LifeTime == null)
                    {
                        throw new Exception($"Life Time (Month) cannot be empty. please check master item category for category name {itemcat}");
                    }
                    //if (string.IsNullOrEmpty(entity.AccountSetDepreceationId))
                    //{
                    //    throw new Exception($"Set Depreciation cannot be empty. please check master item category for category name {itemcat}");
                    //}
                    //if (string.IsNullOrEmpty(entity.AccountSetAccumulationDepreceationId))
                    //{
                    //    throw new Exception($"Life Time (Month) cannot be empty. please check master item category for category name {itemcat}");
                    //}

                    if (isEdit)
                    {
                        context.PhoenixEdit(entity);
                    }
                    else
                    {
                        entity.Id = await TransID.GetTransId(context, Code.FixedAssetDepreciation, DateTime.Today);
                        await context.PhoenixAddAsync(entity);
                    }

                    var details = await context.FixedAssetDepreciationDetail.AsNoTracking().Where(x => x.FixedAssetDepreciationId == entity.Id).ToListAsync();
                    context.FixedAssetDepreciationDetail.RemoveRange(details);


                    var fixedAsset = await context.FixedAsset.AsNoTracking().Where(x => x.Id == entity.FixedAssetID).FirstOrDefaultAsync();
                    int lifeTime = Convert.ToInt32(entity.LifeTime);
                    decimal AcquisitionAmount = Convert.ToDecimal(entity.AcquisitionAmount);

                    DateTime date = Convert.ToDateTime(fixedAsset.RegistrationDate);
                    if (date.Day > 15) { date = date.AddMonths(1).AddDays((-date.Day + 1)); }

                    decimal Depreciationpermonth = (AcquisitionAmount - Convert.ToInt32(entity.Residual)) / lifeTime;
                    string varDeptMonth = String.Format("{0:0.00}", Depreciationpermonth);
                    Depreciationpermonth = Convert.ToDecimal(varDeptMonth);
                    decimal BookVal = (AcquisitionAmount - Depreciationpermonth);
                    decimal tempBookVal = 0;
                    decimal AccumulationDepreciation = AcquisitionAmount - BookVal;
                    decimal tempAccumulationDepreciation = 0;
                    int TotalMonth = lifeTime / 12;
                    bool isOpeningBalance = false;

                    for (int i = 1; i <= lifeTime; i++)
                    {
                        FixedAssetDepreciationDetail data = new FixedAssetDepreciationDetail();
                        if (i == 1)
                        {
                            data.FirstPeriode = Convert.ToDateTime($"{date.Year}/{date.Month}/{date.Day}");
                        }
                        else
                        {
                            data.FirstPeriode = Convert.ToDateTime($"{date.Year}/{date.Month}/1");
                        }

                        date = date.AddMonths(1);

                        if (i == 1)
                        {
                            data.AcquisitionAmount = AcquisitionAmount;
                            data.DepreciationMonth = Depreciationpermonth;
                            data.BookValue = BookVal;
                            data.AccumulationDepreciation = AccumulationDepreciation;
                        }
                        else
                        {
                            tempBookVal = (BookVal - Depreciationpermonth);
                            BookVal = tempBookVal;
                            tempAccumulationDepreciation = (AcquisitionAmount - BookVal);
                            AccumulationDepreciation = tempAccumulationDepreciation;
                            data.AcquisitionAmount = AcquisitionAmount;
                            data.DepreciationMonth = Depreciationpermonth;
                            data.BookValue = BookVal;
                            data.AccumulationDepreciation = AccumulationDepreciation;
                        }

                        data.Id = Guid.NewGuid().ToString();
                        data.FixedAssetDepreciationId = entity.Id;
                        data.FixedAssetID = entity.FixedAssetID;
                        data.DepreciationMonth = entity.DepreciationValue;

                        if (data.FirstPeriode.Value.Year < DateTime.Now.Year)
                        {
                            //data.Status = "POSTED";
                            isOpeningBalance = true;
                        }
                        await context.PhoenixAddAsync(data);
                    }

                    //create Opening Balance fixed asset
                    if (isOpeningBalance)
                    {
                        var legalEntityName = await context.LegalEntity.AsNoTracking().Where(x => x.Id == fixedAsset.LegalEntityId).Select(x => x.LegalEntityName).FirstOrDefaultAsync();
                        var BusinessUnitName = await context.BusinessUnits.AsNoTracking().Where(x => x.Id == fixedAsset.BusinessUnitId).Select(x => x.UnitName).FirstOrDefaultAsync();
                        var defaultCurrencyId = await context.Currencies.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && (x.IsDefault ?? false)).Select(x => x.Id).FirstOrDefaultAsync();

                        var desc = $"Opening Balance Year : {DateTime.Now.Year}, Legal Entity : {legalEntityName}";
                        if (!string.IsNullOrEmpty(BusinessUnitName)) { desc += $" Business Unit : {BusinessUnitName}"; }
                        var journalDebit = new Journal
                        {
                            Id = Guid.NewGuid().ToString(),
                            TransactionDate = Convert.ToDateTime($"{DateTime.Now.Year}/1/1"),
                            ReferenceId = entity.Id,
                            ReferenceNumber = entity.Id,
                            ReferenceDate = Convert.ToDateTime($"{DateTime.Now.Year}/1/1"),
                            Description = desc,
                            DefaultCurrencyId = defaultCurrencyId,
                            CurrencyId = fixedAsset.CurrencyId,
                            ExchangeRate = fixedAsset.ExchangeRate,
                            OriginalAmount = entity.AcquisitionAmount,
                            Debit = entity.AcquisitionAmount * fixedAsset.ExchangeRate,
                            Credit = 0,
                            AccountId = fixedAsset.CoaId,
                            DivisiId = fixedAsset.BusinessUnitId,
                            LegalId = fixedAsset.LegalEntityId,
                            AffiliateId = fixedAsset.AffiliationId,
                            LineCode = "1",
                            TrxSource = SourceJournal.MasterOpeningBalance,
                            IsActive = true
                        };

                        var coaIdHistoricalBalance = await context.FinReportControls.AsNoTracking().Where(x => x.ItemCode == ReportControlItemCode.HistoricalBalance).Select(x => x.CoaId).FirstOrDefaultAsync();
                        var journalCredit = new Journal
                        {
                            Id = Guid.NewGuid().ToString(),
                            TransactionDate = Convert.ToDateTime($"{DateTime.Now.Year}/1/1"),
                            ReferenceId = entity.Id,
                            ReferenceNumber = entity.Id,
                            ReferenceDate = Convert.ToDateTime($"{DateTime.Now.Year}/1/1"),
                            Description = desc,
                            DefaultCurrencyId = defaultCurrencyId,
                            CurrencyId = fixedAsset.CurrencyId,
                            ExchangeRate = fixedAsset.ExchangeRate,
                            OriginalAmount = entity.AcquisitionAmount,
                            Debit = 0,
                            Credit = entity.AcquisitionAmount * fixedAsset.ExchangeRate,
                            AccountId = coaIdHistoricalBalance,
                            DivisiId = fixedAsset.BusinessUnitId,
                            LegalId = fixedAsset.LegalEntityId,
                            AffiliateId = fixedAsset.AffiliationId,
                            LineCode = "2",
                            TrxSource = SourceJournal.MasterOpeningBalance,
                            IsActive = true
                        };

                        await context.PhoenixAddAsync(journalDebit);
                        await context.PhoenixAddAsync(journalCredit);
                    }

                    var save = await context.SaveChangesAsync();
                    transaction.Commit();
                    return save;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message.ToString());
                }
            }
        }

        public async Task<int> DeleteSoftAsync(FixedAssetDepreciation entity)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    context.PhoenixDelete(entity);
                    var detail = await context.FixedAssetDepreciationDetail.Where(x => x.FixedAssetDepreciationId == entity.Id).ToListAsync();
                    context.PhoenixDeleteRange(detail.ToArray());
                    var save = await context.SaveChangesAsync();
                    transaction.Commit();
                    return save;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message.ToString());
                }
            }
        }

        public async Task<int> PostJournalAsync(FixedAssetDepreciationJournal entity)
        {
            var entityName = await context.LegalEntity.AsNoTracking().Where(x => x.Id == entity.LegalEntityId).Select(x => x.LegalEntityName).FirstOrDefaultAsync();
            var year = await context.FinancialYears.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && entity.PostingDate >= x.StartDate.Date && entity.PostingDate <= x.EndDate.Date && x.LegalEntityId == entity.LegalEntityId).ToListAsync();
            if (year.Count() == 0)
            {
                throw new Exception("Fiscal year for " + entity.PostingDate.Value.ToString("MMMM dd yyyy") + " and legal entity " + entityName + " not found");
            }

            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    //var selected = from a in context.FixedAssetDepreciationDetail
                    //               where entity.Selected.Any(x => x == a.FixedAssetDepreciationId)
                    //               group a by a.FixedAssetDepreciationId into g
                    //               select new { FixedAssetDepreciationId = g.Key, FirstPeriode = g.Where(x => x.Status != "POSTED").Min(x => x.FirstPeriode) };

                    var details = await (from a in context.FixedAssetDepreciationDetail
                                         join b in context.FixedAsset on a.FixedAssetID equals b.Id
                                         join c in context.ItemCategories on b.ItemCategoryId equals c.Id
                                         //join e in selected on new { a.FixedAssetDepreciationId, a.FirstPeriode } equals new { e.FixedAssetDepreciationId, e.FirstPeriode }
                                         where (a.Status ?? "") != "POSTED" && a.FirstPeriode.Value.Month == entity.PostingDate.Value.Month && a.FirstPeriode.Value.Year == entity.PostingDate.Value.Year && b.LegalEntityId == entity.LegalEntityId
                                         select new
                                         {
                                             a.Id,
                                             a.FirstPeriode,
                                             a.DepreciationMonth,
                                             b.CurrencyId,
                                             b.ExchangeRate,
                                             b.LegalEntityId,
                                             b.AffiliationId,
                                             b.BusinessUnitId,
                                             c.COADebit,
                                             c.COACredit
                                         }).ToListAsync();

                    var jvNumber = string.Empty;
                    if (entity.PostingDate != null)
                    {
                        var detail = await context.FixedAssetDepreciationDetail.Where(x => (x.Status ?? "") == "POSTED" && x.FirstPeriode.Value.Month == entity.PostingDate.Value.Month && x.FirstPeriode.Value.Year == entity.PostingDate.Value.Year && context.FixedAsset.Any(y => y.Id == x.FixedAssetID && y.LegalEntityId == entity.LegalEntityId)).FirstOrDefaultAsync();
                        if (detail != null) { if (!string.IsNullOrEmpty(detail.JVNumber)) jvNumber = detail.JVNumber; else jvNumber = await TransID.GetTransId(context, Code.JournalVoucher, DateTime.Today); }
                        else { jvNumber = await TransID.GetTransId(context, Code.JournalVoucher, DateTime.Today); }
                    }

                    foreach (var item in details)
                    {
                        if (!string.IsNullOrEmpty(item.COADebit) && !string.IsNullOrEmpty(item.COACredit))
                        {
                            //var date = item.FirstPeriode.Value.AddDays((-item.FirstPeriode.Value.Day) + DateTime.DaysInMonth(item.FirstPeriode.Value.Year, item.FirstPeriode.Value.Month));
                            var date = new DateTime(item.FirstPeriode.Value.Year, item.FirstPeriode.Value.Month, DateTime.DaysInMonth(item.FirstPeriode.Value.Year, item.FirstPeriode.Value.Month));
                            var jornalDebit = new Journal()
                            {
                                Id = Guid.NewGuid().ToString(),
                                //Description = "",
                                Debit = item.DepreciationMonth * item.ExchangeRate,
                                Credit = 0,
                                ReferenceId = item.Id,
                                ReferenceNumber = jvNumber,
                                AccountId = item.COADebit,
                                ReferenceDate = date,
                                TransactionDate = date,
                                CurrencyId = item.CurrencyId,
                                ExchangeRate = item.ExchangeRate,
                                LegalId = item.LegalEntityId,
                                AffiliateId = item.AffiliationId,
                                DivisiId = item.BusinessUnitId,
                                LineCode = "1",
                                TrxSource = SourceJournal.JournalFixedAssetDepreciation,
                                //IsJobClosed = "N",
                                OriginalAmount = item.DepreciationMonth,
                                DefaultCurrencyId = context.Currencies.AsNoTracking().Where(z => z.IsDefault == true).Select(z => z.Id).FirstOrDefault()
                            };
                            await context.PhoenixAddAsync(jornalDebit);

                            var jornalCredit = new Journal()
                            {
                                Id = Guid.NewGuid().ToString(),
                                //Description = "",
                                Debit = 0,
                                Credit = item.DepreciationMonth * item.ExchangeRate,
                                ReferenceId = item.Id,
                                ReferenceNumber = jvNumber,
                                AccountId = item.COACredit,
                                ReferenceDate = date,
                                TransactionDate = date,
                                CurrencyId = item.CurrencyId,
                                ExchangeRate = item.ExchangeRate,
                                LegalId = item.LegalEntityId,
                                AffiliateId = item.AffiliationId,
                                DivisiId = item.BusinessUnitId,
                                LineCode = "2",
                                TrxSource = SourceJournal.JournalFixedAssetDepreciation,
                                //IsJobClosed = "N",
                                OriginalAmount = item.DepreciationMonth,
                                DefaultCurrencyId = context.Currencies.AsNoTracking().Where(z => z.IsDefault == true).Select(z => z.Id).FirstOrDefault()
                            };
                            await context.PhoenixAddAsync(jornalCredit);
                            await context.Database.ExecuteSqlCommandAsync("update fn.fixed_asset_depreciation_detail set Status='POSTED', jv_number=@JvNumber where id=@Id", new SqlParameter("@JvNumber", jvNumber), new SqlParameter("@Id", item.Id));
                        }
                    }

                    var save = await context.SaveChangesAsync();
                    transaction.Commit();
                    return save;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message.ToString());
                }
            }
        }

        public DateTime GetPostingDate(string legalEntityId)
        {
            var date = context.FixedAssetDepreciationDetail.Where(x => !(x.IsDeleted ?? false) && (x.Status ?? "") != "POSTED" && context.FixedAsset.Any(y => y.Id == x.FixedAssetID && y.LegalEntityId == legalEntityId) && x.FirstPeriode.Value.Year >= x.CreatedOn.Value.Year).Min(x => x.FirstPeriode) ?? DateTime.Now;
            //return date.AddDays((-date.Day) + DateTime.DaysInMonth(date.Year, date.Month));
            return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
        }

        public async Task<List<Journal>> GetOpeningBalance(string Id)
        {
            var result = await (from tb in context.Journal
                                join coa in context.ChartOfAccounts on tb.AccountId equals coa.Id
                                where tb.ReferenceId == Id
                                select new Journal
                                {
                                    AccountId = coa.Id,
                                    CodeRec = coa.CodeRec,
                                    AccountName = coa.Name5,
                                    Description = tb.Description,
                                    DivisiName = context.BusinessUnits.Where(x => x.Id == tb.DivisiId).Select(x => x.UnitName).FirstOrDefault(),
                                    LegalEntityName = context.LegalEntity.Where(x => x.Id == tb.LegalId).Select(x => x.LegalEntityName).FirstOrDefault(),
                                    Credit = tb.Credit,
                                    Debit = tb.Debit,
                                    LineCode = tb.LineCode,
                                }).OrderByDescending(x => x.Debit).ThenByDescending(x => x.Credit).ToListAsync();
            return result;
        }

        #region Not Implement 

        public Task<int> AddRangeAsync(params FixedAssetDepreciation[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> EditAsync(FixedAssetDepreciation entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> EditRangeAsync(FixedAssetDepreciation entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<FixedAssetDepreciation>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(FixedAssetDepreciation entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params FixedAssetDepreciation[] entities)
        {
            throw new NotImplementedException();
        }


        public Task<int> DeleteSoftRangeAsync(params FixedAssetDepreciation[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddAsync(FixedAssetDepreciation entity)
        {
            throw new NotImplementedException();
        }

        #endregion  Not Implement 

    }
}
