using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Finance.Transaction.FixedAssetOutMovement;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Finance.Transaction
{
    public interface IFixedAssetService
    {
        Task<int> AddEdit(FixedAsset entity, bool isEdit);
        Task<List<FixedAsset>> Get();
        Task<FixedAsset> Get(string Id);
        Task<int> DeleteAsync(FixedAsset entity);
        Task<List<FixedAsset>> GetListFixedAssetByLocationId(string locationId);
        Task<List<FixedAssetOutMovementDetail>> GetListFixedAssetOutByLocationId(string locationId);
    }
    public class FixedAssetService : IFixedAssetService
    {
        readonly DataContext context;
        readonly FileService uploadFile;

        public FixedAssetService(DataContext context, FileService uploadFile)
        {
            this.context = context;
            this.uploadFile = uploadFile;
        }

        public async Task<List<FixedAsset>> GetListFixedAssetByLocationId(string locationId)
        {
            return await context.FixedAsset.Where(x => !(x.IsDeleted ?? false) && x.LocationId == locationId).ToListAsync();
        }

        public async Task<List<FixedAssetOutMovementDetail>> GetListFixedAssetOutByLocationId(string locationId)
        {

            var urllink = "[Link]/finance/FixedAsset/Detail?Id=";
            var query = await (from f in context.FixedAsset
                               where !(f.IsDeleted ?? false) && f.LocationId == locationId
                               select new FixedAssetOutMovementDetail()
                               {
                                   Id = f.Id,
                                   LocationId = f.LocationId,
                                   LegalEntityId = f.LegalEntityId,
                                   AccountId = f.AccountId,
                                   Brand = f.Brand,
                                   Condition = f.Condition.ToString(),
                                   ConditionName = (from x in context.InventoryDetailStatus where x.getForm == f.Condition select x.statusName).FirstOrDefault(),
                                   LegalEntityName = (from x in context.LegalEntity where x.Id == f.LegalEntityId select x.LegalEntityName).FirstOrDefault(),
                                   LocationName = (from x in context.MasterLocationProcurement where x.Id == f.LocationId select x.LocationStore).FirstOrDefault(),
                                   FixedAssetId = f.FixedAssetNumber,
                                   ItemCategoryId = f.ItemCategoryId,
                                   Qty = f.UnitCount ?? 0,
                                   QtyMax = f.UnitCount ?? 0,
                                   UnitName = f.FixedAssetName,
                                   UrlLink = urllink + f.FixedAssetNumber,
                                   RegistrationDate = f.RegistrationDate

                               }).ToListAsync();
            return query;
        }

        public async Task<List<FixedAsset>> Get()
        {
            var query = await (from fa in context.FixedAsset.Where(x => !(x.IsDeleted ?? false))
                               join ml in context.MasterLocationProcurement on fa.LocationId equals ml.Id
                               join cr in context.Currencies on fa.CurrencyId equals cr.Id
                               join st in context.InventoryDetailStatus on fa.Condition equals st.getForm into sts
                               from st in sts.DefaultIfEmpty()
                               select new FixedAsset
                               {
                                   Id = fa.Id,
                                   FixedAssetNumber = fa.FixedAssetNumber,
                                   FixedAssetNumberManual = fa.FixedAssetNumberManual,
                                   FixedAssetName = fa.FixedAssetName,
                                   UnitCount = fa.UnitCount,
                                   LocationName = ml.LocationStore,
                                   CurrentPrice = fa.CurrentPrice,
                                   EvidentNumber = fa.EvidentNumber,
                                   CurrencyId = cr.CurrencyName,
                                   ConditionName = st.statusName,
                                   AcquisitionCost = fa.AcquisitionCost,
                                   CurrentEstimationPrice = fa.CurrentEstimationPrice,
                                   Qty = fa.UnitCount
                               }).ToListAsync();

            return query;
        }
        public async Task<int> AddEdit(FixedAsset entity, bool isEdit)
        {
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

            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (isEdit)
                    {
                        context.PhoenixEdit(entity);
                        await context.SaveChangesAsync();
                    }
                    else
                    {
                        var id = await TransID.GetTransId(context, Code.FixedAsset, DateTime.Today);
                        entity.Id = id;
                        entity.FixedAssetNumber = id;
                        await context.PhoenixAddAsync(entity);
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

        public async Task<FixedAsset> Get(string Id)
        {
            var query = await (from fa in context.FixedAsset
                               join cat in context.ItemCategories on fa.ItemCategoryId equals cat.Id
                               where fa.IsDeleted.Equals(false) && fa.Id == Id
                               select new FixedAsset
                               {
                                   Id = fa.Id,
                                   CoaId = fa.CoaId,
                                   FixedAssetName = fa.FixedAssetName,
                                   FixedAssetNumber = fa.FixedAssetNumber,
                                   FixedAssetNumberManual = fa.FixedAssetNumberManual,
                                   UnitCount = fa.UnitCount,
                                   PurchaseOrderId = fa.PurchaseOrderId,
                                   InvoiceId = fa.InvoiceId,
                                   EvidentNumber = fa.EvidentNumber,
                                   CurrentPrice = fa.CurrentPrice,
                                   CurrencyId = fa.CurrencyId,
                                   ExchangeRate = fa.ExchangeRate,
                                   LocationId = fa.LocationId,
                                   AccountId = fa.AccountId,
                                   Description = fa.Description,
                                   RegistrationDate = fa.RegistrationDate,
                                   Condition = fa.Condition,
                                   StatusDepreciation = fa.StatusDepreciation,
                                   RemarksDepreciation = fa.RemarksDepreciation,
                                   GetFrom = fa.GetFrom,
                                   VendorId = fa.VendorId,
                                   DivisionId = fa.DivisionId,
                                   BusinessUnitId = fa.BusinessUnitId,
                                   LegalEntityId = fa.LegalEntityId,
                                   AffiliationId = fa.AffiliationId,
                                   CreatedBy = fa.CreatedBy,
                                   CreatedOn = fa.CreatedOn,
                                   ModifiedBy = fa.ModifiedBy,
                                   ModifiedOn = fa.ModifiedOn,
                                   EconomicPeriod = fa.EconomicPeriod,
                                   FileUpload = fa.FileUpload,
                                   FileUploadName = context.Filemasters.Where(x => x.Id == fa.FileUpload).Select(x => x.Name).FirstOrDefault(),
                                   AmortizationPeriod = fa.AmortizationPeriod,
                                   AmortizationValue = fa.AmortizationValue,
                                   ItemCategoryId = fa.ItemCategoryId,
                                   AcquisitionCost = fa.AcquisitionCost,
                                   CurrentEstimationPrice = fa.CurrentEstimationPrice,
                                   Qty = fa.UnitCount,
                                   LocationName = (from x in context.MasterLocationProcurement where x.Id == fa.LocationId select x.LocationStore).FirstOrDefault(),
                                   ConditionName = (from x in context.InventoryDetailStatus where x.getForm == fa.Condition select x.statusName).FirstOrDefault(),
                                   LegalEntityName = (from x in context.LegalEntity where x.Id == fa.LegalEntityId select x.LegalEntityName).FirstOrDefault(),
                                   LifeTimeMonth = cat.LifeTimeMonth,
                                   COADebit = cat.COADebit,
                                   COACredit = cat.COACredit
                               }).FirstOrDefaultAsync();
            return query;
        }

        public async Task<int> DeleteAsync(FixedAsset entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

    }
}
