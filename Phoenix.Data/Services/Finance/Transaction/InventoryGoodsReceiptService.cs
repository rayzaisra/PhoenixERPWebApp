using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models;
using Phoenix.Shared.Core.PrincipalHelpers;
using System.Data.SqlClient;
using System.Data;
using Phoenix.Data.Models.Finance;
using Phoenix.Data.Models.Enum;

namespace Phoenix.Data.Services.Finance.Transaction
{
    public interface IInventoryGoodsReceiptService
    {
        Task<int> AddEdit(InventoryGoodsReceipt model, bool isEdit);
        Task<List<InventoryGoodsReceipt>> Get();
        Task<InventoryGoodsReceipt> Get(string id);
        Task<List<InventoryGoodsReceiptDetail>> GetDetail(string poId, string grId);
        Task<List<InventoryGoodsReceiptDetail>> GetDetailHistory(string poId);
        Task<List<InventoryDetail>> GetInventoryDetail(string grid);
        Task<List<FixedAsset>> GetFixedAsset(string grid);
        Task<InventoryGoodsReceipt> GetPOById(string poId);
        Task<InventoryDetail> UpdateInventoryDetail(InventoryDetail model);
        Task<int> Generated(InventoryGoodsReceipt model);

    }

    public class InventoryGoodsReceiptService : IInventoryGoodsReceiptService
    {
        readonly DataContext context;
        readonly StatusLogService log;

        public InventoryGoodsReceiptService(DataContext context, StatusLogService log)
        {
            this.context = context;
            this.log = log;
        }

        public async Task<int> AddEdit(InventoryGoodsReceipt model, bool isEdit)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var employee = await context.EmployeeBasicInfos.FirstOrDefaultAsync(x => x.Id == context.GetEmployeeId());
                    model.ReceivedBy = employee.Id;
                    model.ReceivedByName = employee.NameEmployee;

                    if (isEdit)
                    {
                        model.Remark = "Data updated successfully by " + employee.NameEmployee;
                        context.PhoenixEdit(model);
                        await context.SaveChangesAsync();
                    }
                    else
                    {
                        var id = await TransID.GetTransId(context, Code.GoodsReceipt, DateTime.Today);
                        model.Id = id;
                        model.GoodsReceiptNumber = id;
                        model.Remark = "Data saved successfully by " + employee.NameEmployee;
                        await context.PhoenixAddAsync(model);
                        await context.SaveChangesAsync();
                    }

                    if (isEdit)
                    {
                        var detail = context.InventoryGoodsReceiptsDetail.AsNoTracking().Where(x => x.InventoryGoodsReceiptId == model.Id).ToList();
                        if (detail.Count > 0)
                        {
                            context.InventoryGoodsReceiptsDetail.RemoveRange(detail);
                        }
                        if (model.InventoryGoodsReceiptDetails != null)
                        {
                            List<InventoryGoodsReceiptDetail> dt = new List<InventoryGoodsReceiptDetail>();
                            foreach (InventoryGoodsReceiptDetail data in model.InventoryGoodsReceiptDetails)
                            {
                                //data.Id = Guid.NewGuid().ToString();
                                data.InventoryGoodsReceiptId = model.Id;
                                dt.Add(data);
                            }

                            InventoryGoodsReceiptDetail[] dtg = dt.ToArray();
                            await context.PhoenixAddRangeAsync(dtg);
                        }
                    }
                    else
                    {
                        if (model.InventoryGoodsReceiptDetails != null)
                        {
                            foreach (var item in model.InventoryGoodsReceiptDetails)
                            {
                                item.Id = Guid.NewGuid().ToString();
                                item.InventoryGoodsReceiptId = model.Id;
                                await context.PhoenixAddAsync(item);
                            }
                        }
                    }

                    await context.SaveChangesAsync();

                    //if (model.Status == StatusTransactionName.Close)
                    //{
                    //    var parameter = new SqlParameter("inventory_good_receipt_id", model.Id);
                    //    var message = new SqlParameter("messege", SqlDbType.NVarChar, 300) { Direction = ParameterDirection.Output };
                    //    var status_code = new SqlParameter("status_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    //    await context.Database.ExecuteSqlCommandAsync("EXECUTE fn.submit_inventory_good_receipt @inventory_good_receipt_id,@messege out,@status_code out", parameter, message, status_code);
                    //    if ((int)status_code.Value != 200)
                    //    {
                    //        throw new Exception(message.Value.ToString());
                    //    }
                    //    //await context.SaveChangesAsync();
                    //    //var data = new { StatusCode = (int)status_code.Value, Message = message.Value };
                    //    //return data;
                    //}

                    //if (model.IsGenerated == true)
                    //{
                    //    var updateIGR = await context.InventoryGoodsReceipts.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
                    //    updateIGR.IsGenerated = true;
                    //    context.PhoenixEdit(updateIGR);
                    //    await context.Database.ExecuteSqlCommandAsync("EXECUTE fn.proc_generate_aset_backup2 @p_gr_id", parameters: new[] { new SqlParameter("@p_gr_id", model.Id) });
                    //}

                    await log.AddAsync(new StatusLog() { TransactionId = model.Id, Status = model.Status, Description = model.Remark });
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

        public async Task<int> Generated(InventoryGoodsReceipt model)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var employee = await context.EmployeeBasicInfos.FirstOrDefaultAsync(x => x.Id == context.GetEmployeeId());
                    model.Remark = "Data generated successfully by " + employee.NameEmployee;
                    var updateIGR = await context.InventoryGoodsReceipts.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
                    updateIGR.IsGenerated = true;
                    context.PhoenixEdit(updateIGR);

                    await context.Database.ExecuteSqlCommandAsync("EXECUTE fn.proc_generate_aset_backup2 @p_gr_id", parameters: new[] { new SqlParameter("@p_gr_id", model.Id) });

                    await log.AddAsync(new StatusLog() { TransactionId = model.Id, Status = model.Status, Description = model.Remark });
                    var save = await context.SaveChangesAsync();
                    transaction.Commit();
                    return save;
                }
                catch (Exception ex)
                {
                    await log.AddAsync(new StatusLog() { TransactionId = model.Id, Status = model.Status, Description = "Data failed generated." });
                    var save = await context.SaveChangesAsync();
                    transaction.Rollback();
                    throw new Exception(ex.Message.ToString());
                }
            }
        }
        public async Task<InventoryDetail> UpdateInventoryDetail(InventoryDetail model)
        {
            InventoryDetail result = new InventoryDetail();
            await context.Database.ExecuteSqlCommandAsync("EXECUTE fn.updateInventoryCondition @inventory_detail_id,@statusitem,@condition", parameters: new[] { new SqlParameter("@inventory_detail_id", model.Id), new SqlParameter("@statusitem", model.Condition), new SqlParameter("@condition", model.ConditionName) });
            return result;
        }

        public async Task<List<InventoryGoodsReceipt>> Get()
        {
            var result = await context.InventoryGoodsReceipts.Where(x => !(x.IsDeleted ?? false)).ToListAsync();
            return result;
        }

        public async Task<InventoryGoodsReceipt> Get(string id)
        {
            var data = await (from t1 in context.InventoryGoodsReceipts
                              join t2 in context.BusinessUnits on t1.BusinessUnitId equals t2.Id into t2j
                              from t2lj in t2j.DefaultIfEmpty()
                              join t3 in context.LegalEntity on t1.LegalEntityId equals t3.Id
                              join t4 in context.Companys on t1.CompanyId equals t4.Id
                              join t6 in context.Affiliations on t1.AffiliationId equals t6.Id
                              join po in context.PurchaseOrders on t1.PurchaseOrderId equals po.Id
                              join emp in context.EmployeeBasicInfos on po.PicId equals emp.Id
                              where t1.Id == id && !(t1.IsDeleted ?? false)
                              select new InventoryGoodsReceipt
                              {
                                  Id = t1.Id,
                                  AffiliationId = t1.AffiliationId,
                                  GoodsReceiptNumber = t1.GoodsReceiptNumber,
                                  ReferenceNumber = t1.ReferenceNumber,
                                  VendorId = t1.VendorId,
                                  VendorName = t1.VendorName,
                                  GoodsReceiptDatetime = t1.GoodsReceiptDatetime,
                                  PurchaseOrderId = t1.PurchaseOrderId,
                                  PurchaseOrderNumber = t1.PurchaseOrderNumber,
                                  ReceivedBy = t1.ReceivedBy,
                                  ReceivedByName = t1.ReceivedByName,
                                  InvoicingTypeId = t1.InvoicingTypeId,
                                  InvoicingTypeName = t1.InvoicingTypeName,
                                  JobId = t1.JobId,
                                  JobName = t1.JobName,
                                  Term = t1.Term,
                                  CompanyId = t1.CompanyId,
                                  CompanyName = t4.CompanyName,
                                  CompanyBrandId = t1.CompanyBrandId,
                                  BrandName = t1.BrandName,
                                  BusinessUnitId = t1.BusinessUnitId,
                                  BusinessUnitName = t2lj.UnitName,
                                  LegalEntityId = t1.LegalEntityId,
                                  LegalEntityName = t3.LegalEntityName,
                                  LocationId = t1.LocationId,
                                  AffiliationName = t6.AffiliationName,
                                  IsGenerated = t1.IsGenerated,
                                  Status = t1.Status,
                                  PicId = po.PicId,
                                  PicName = emp.NameEmployee,
                                  CreatedBy = t1.CreatedBy,
                                  CreatedOn = t1.CreatedOn,
                                  ModifiedBy = t1.ModifiedBy,
                                  ModifiedOn = t1.ModifiedOn,
                                  ApprovedBy = t1.ApprovedBy,
                                  ApprovedOn = t1.ApprovedOn,
                                  IsActive = t1.IsActive,
                                  IsDefault = t1.IsDefault,
                                  IsDeleted = t1.IsDeleted
                              }).FirstOrDefaultAsync();
            if (data != null)
            {
                data.IsNonInventory = context.InventoryGoodsReceiptsDetail.Where(x => x.ItemTypeId != ItemTypeId.NonInventory).Count() == 0;
                if (data.IsNonInventory) data.IsGenerated = true;
            }
            return data;
        }

        public async Task<List<InventoryGoodsReceiptDetail>> GetDetail(string poId, string grId)
        {
            if (string.IsNullOrEmpty(grId))
            {
                var query = await (from a in context.PurchaseOrderDetails
                                   join b in context.Uom on a.UomId equals b.Id
                                   join c in context.ItemType on a.ItemTypeId equals c.Id
                                   where a.PurchaseOrderId == poId && !(a.IsDeleted ?? false)
                                   select new InventoryGoodsReceiptDetail
                                   {
                                       Id = Guid.NewGuid().ToString(),
                                       PurchaseOrderDetailId = a.Id,
                                       ItemId = a.ItemId,
                                       ItemName = a.ItemName,
                                       ItemTypeId = a.ItemTypeId,
                                       UomId = a.UomId,
                                       PurchasedQty = a.Qty,
                                       UnitPrice = a.UnitPrice,
                                       Amount = a.Amount,
                                       ItemTypeName = c.TypeName,
                                       ItemCode = a.ItemCode,
                                       UnitName = b.unit_name,
                                       ReceivedQty = 0,
                                       RemainingQty = 0,
                                       Task = a.Task,
                                       TaskId = a.TaskId

                                   }).ToListAsync();

                var lastGrNumber = await context.InventoryGoodsReceipts.Where(x => !(x.IsDeleted ?? false) && x.Id != grId && x.PurchaseOrderId == poId && x.Status == StatusTransactionName.Close).MaxAsync(x => x.GoodsReceiptNumber);
                var lastGoodReceivedDetails = await (from a in context.InventoryGoodsReceipts
                                                     join b in context.InventoryGoodsReceiptsDetail on a.Id equals b.InventoryGoodsReceiptId
                                                     where a.GoodsReceiptNumber == lastGrNumber
                                                     select new { b.PurchaseOrderDetailId, b.RemainingQty }
                                                    ).ToListAsync();

                foreach (var item in query)
                {
                    var qty = (lastGoodReceivedDetails.Where(x => x.PurchaseOrderDetailId == item.PurchaseOrderDetailId).Select(x => x.RemainingQty).FirstOrDefault() ?? 0);
                    if (qty > 0)
                    {
                        item.PurchasedQty = qty;
                    }

                    item.LastRemainingQty = qty;
                    //item.ReceivedQty = qty;
                }

                return query;
            }
            else
            {
                var query = await context.InventoryGoodsReceiptsDetail.Where(x => x.InventoryGoodsReceiptId == grId && !(x.IsDeleted ?? false)).ToListAsync();
                foreach (var item in query)
                {
                    item.LastRemainingQty = item.RemainingQty;// + item.ReceivedQty;
                }
                return query;
            }
        }

        public async Task<List<InventoryGoodsReceiptDetail>> GetDetailHistory(string poId)
        {
            var query = await (from igrd in context.InventoryGoodsReceiptsDetail
                               join igr in context.InventoryGoodsReceipts on igrd.InventoryGoodsReceiptId equals igr.Id
                               where igr.PurchaseOrderId == poId && igrd.ReceivedQty != 0
                               select new InventoryGoodsReceiptDetail
                               {
                                   Id = igrd.Id,
                                   ModifiedOn = igrd.ModifiedOn,
                                   ItemName = igrd.ItemName,
                                   ItemCode = igrd.ItemCode,
                                   ItemTypeName = igrd.ItemTypeName,
                                   ReceivedQty = igrd.ReceivedQty,
                                   InventoryGoodsReceiptId = igr.GoodsReceiptNumber,
                                   Task = igrd.Task,
                                   TaskId = igrd.TaskId
                               }).OrderByDescending(x => x.ModifiedOn).ToListAsync();
            return query;
        }


        public async Task<List<InventoryDetail>> GetInventoryDetail(string grid)
        {
            var query = await (from a in context.Inventory
                               join b in context.InventoryDetail on a.Id equals b.InventoryId
                               where !(a.IsDeleted ?? false) && b.GoodReceiptId == grid
                               select new InventoryDetail
                               {
                                   Id = b.Id,
                                   InventoryId = b.InventoryId,
                                   ItemId = b.ItemId,
                                   ItemName = b.ItemName,
                                   ItemCategoryId = b.ItemCategoryId,
                                   ItemCategoryName = b.ItemCategoryName,
                                   UomId = b.UomId,
                                   UnitName = b.UnitName,
                                   Brand = b.Brand,
                                   Subbrand = b.Subbrand,
                                   Description = b.Description,
                                   Qty = b.Qty,
                                   Remark = b.Remark,
                                   FileAttachment = b.FileAttachment,
                                   GoodReceiptDetailId = b.GoodReceiptDetailId,
                                   DateReceipt = b.DateReceipt,
                                   Manufacturer = b.Manufacturer,
                                   Model = b.Model,
                                   ProductNumber = b.ProductNumber,
                                   Color = b.Color,
                                   Condition = b.Condition,
                                   SkuCode = b.SkuCode,
                                   Location = b.Location,
                                   InventoryInNumber = a.InventoryInNumber,
                                   InventoryName = a.InventoryName,
                                   //ConditionName = context.MasterConditionProcurement.Where(x => x.Id == b.Condition).Select(x => x.ConditionName).FirstOrDefault()
                                   ConditionName = b.Condition
                               }).ToListAsync();

            return query;
        }


        public async Task<List<FixedAsset>> GetFixedAsset(string grid)
        {
            var query = await context.FixedAsset.Where(x => !(x.IsDeleted ?? false) && x.GoodReceivedId == grid).ToListAsync();
            return query;
        }

        public async Task<InventoryGoodsReceipt> GetPOById(string poId)
        {
            var termin = context.InvoiceReceivables.Where(x => x.PoId == poId).Max(x => x.PaymentTo);
            var query = await (from po in context.PurchaseOrders
                               join le in context.LegalEntity on po.LegalEntityId equals le.Id
                               join com in context.Companys on po.CompanyId equals com.Id
                               join bu in context.BusinessUnits on po.BusinessUnitId equals bu.Id into buj
                               from bujo in buj.DefaultIfEmpty()
                               join aff in context.Affiliations on po.AffiliationId equals aff.Id into affj
                               from affjo in affj.DefaultIfEmpty()
                               join emp in context.EmployeeBasicInfos on po.PicId equals emp.Id
                               where po.Id == poId
                               select new InventoryGoodsReceipt
                               {
                                   PurchaseOrderId = po.Id,
                                   PurchaseOrderNumber = po.PurchaseOrderNumber,
                                   InvoicingTypeId = po.InvoicingType,
                                   InvoicingTypeName = po.InvoicingType,
                                   JobId = po.JobId,
                                   CompanyId = com.Id,
                                   CompanyName = com.CompanyName,
                                   CompanyBrandId = po.CompanyBrandId,
                                   BusinessUnitId = po.BusinessUnitId,
                                   BusinessUnitName = bujo.UnitName,
                                   LegalEntityId = po.LegalEntityId,
                                   LegalEntityName = le.LegalEntityName,
                                   Term = termin,
                                   AffiliationId = po.AffiliationId,
                                   AffiliationName = affjo.AffiliationName,
                                   PicId = po.PicId,
                                   PicName = emp.NameEmployee,
                                   IsGenerated = false
                               }).FirstOrDefaultAsync();
            if (query != null)
            {
                query.IsNonInventory = context.PurchaseOrderDetails.Where(x => x.ItemTypeId != ItemTypeId.NonInventory).Count() == 0;
                if (query.IsNonInventory) query.IsGenerated = true;
                query.JobName = context.JobDetail.Where(x => x.Id == query.JobId).Select(x => x.JobName).FirstOrDefault();
                query.BrandName = context.CompanyBrands.Where(x => x.Id == query.CompanyBrandId).Select(x => x.BrandName).FirstOrDefault();
            }

            var a = await context.InventoryGoodsReceiptsDetail.Where(x => x.PurchaseOrderDetailId == poId && !(x.IsDeleted ?? false)).ToListAsync();

            return query;
        }
    }
}
