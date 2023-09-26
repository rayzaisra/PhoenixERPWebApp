using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models.Finance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Finance.Transaction
{
    public interface IInventoryService : IDataService<Inventory>
    {
        Task<int> AddWithDetailAsync(Inventory Header, List<InventoryDetail> Details);
        Task<int> EditWithDetailAsync(Inventory Header, List<InventoryDetail> Details);
        Task<List<InventoryDetail>> GetDetailByGoodReceipt(string grId);
        Task<List<InventoryDetail>> GetDetail(string invId);
        Task<List<InventoryDetail>> GetDetailForInventoryStockCard(string invid);
        Task<Inventory> GetInvAll(string invId);
        Task<Inventory> GetDataInventoryStockCard(string invId);
        Task<int> AddDetail(InventoryDetail model);
        Task<int> EditDetail(InventoryDetail model);
        Task<Inventory> GetAsync(string id);
        Task<List<InventoryDetail>> GetDetailForGoodRequest(string invId);
    }
    public class InventoryService : IInventoryService
    {
        readonly DataContext context;
        readonly FileService uploadFile;

        public InventoryService(DataContext context, FileService uploadFile)
        {
            this.context = context;
            this.uploadFile = uploadFile;
        }

        public static int IncrementedNextSequence(int increment)
        {
            return getthenumber() + increment;
        }

        public static int getthenumber()
        {
            return 000;
        }

        public async Task<int> AddWithDetailAsync(Inventory Header, List<InventoryDetail> Details)
        {

            var file1 = await uploadFile.Upload(Header.file, null);
            if (!string.IsNullOrEmpty(file1))
            {
                Header.FileUpload = file1;
            }
            var transID = await TransID.GetTransId(context, "INV", DateTime.Today);
            Header.Id = transID;
            Header.InventoryInNumber = transID;
            await context.PhoenixAddAsync(Header);

            if (Details.Count > 0)
            {
                List<InventoryDetail> igrd = new List<InventoryDetail>();
                int counter = 1;
                foreach (InventoryDetail data in Details)
                {
                    await ProcessUpload(data);
                    data.InventoryId = Header.Id;
                    //data.Id = Header.InventoryInNumber + counter;
                    data.Id = Header.InventoryInNumber + String.Format("{0:D3}", IncrementedNextSequence(counter));
                    igrd.Add(data);
                    counter++;
                }

                InventoryDetail[] igrdArray = igrd.ToArray();
                await context.PhoenixAddRangeAsync(igrdArray);
            }

            return await context.SaveChangesAsync();
        }

        public async Task<int> EditWithDetailAsync(Inventory Header, List<InventoryDetail> Details)
        {
            var file1 = await uploadFile.Upload(Header.file, null);
            if (!string.IsNullOrEmpty(file1))
            {
                Header.FileUpload = file1;
            }
            else
            {
                Header.FileUpload = Header.FileUpload;
            }

            if (Header.InventoryInNumber == null)
            {
                Header.InventoryInNumber = await TransID.GetTransId(context, "INVT", DateTime.Today);
            }
            if (Header.InventoryName == null || Header.InventoryName == "")
            {
                Header.InventoryName = await TransID.GetTransId(context, "GR_INVENTORY", DateTime.Today);
            }

            context.PhoenixEdit(Header);


            if (Details.Count > 0)
            {
                string detailID = string.Empty;

                int counter = 1;
                foreach (InventoryDetail data in Details)
                {
                    var isExist = context.InventoryDetail.Any(x => x.Id == data.Id);
                    if (isExist == true)
                    {
                        List<InventoryDetail> igrd = new List<InventoryDetail>();
                        detailID = data.Id;
                        await ProcessUpload(data);
                        data.InventoryId = Header.Id;
                        igrd.Add(data);
                        InventoryDetail[] igrdArray = igrd.ToArray();
                        context.PhoenixEditRange(igrdArray);
                    }
                    else
                    {
                        List<InventoryDetail> igrd = new List<InventoryDetail>();
                        await ProcessUpload(data);
                        data.InventoryId = Header.Id;
                        data.Id = Header.InventoryInNumber + String.Format("{0:D3}", IncrementedNextSequence(counter));
                        igrd.Add(data);
                        InventoryDetail[] igrdArray = igrd.ToArray();
                        await context.PhoenixAddRangeAsync(igrdArray);
                    }
                    counter++;
                }



            }
            else
            {
                var isExistDataDetail = context.InventoryDetail.Any(x => x.InventoryId == Header.Id);
                if (isExistDataDetail)
                {
                    List<InventoryDetail> listDatadetail = context.InventoryDetail.Where(x => x.InventoryId == Header.Id).ToList();
                    foreach (var item in listDatadetail)
                    {
                        context.PhoenixDelete(item);
                    }
                }

            }

            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(Inventory entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Inventory>> Get()
        {
            //var query = await context.Inventory.Where(x => x.IsDeleted.Equals(false)).ToListAsync();
            List<Inventory> list = new List<Inventory>();
            list = await (from inv in context.Inventory.Where(x => !(x.IsDeleted ?? false))
                          join job in context.JobDetail on inv.JobId equals job.Id into jobs
                          from job in jobs.DefaultIfEmpty()
                          join e in context.Filemasters on inv.FileUpload equals e.Id into es
                          from e in es.DefaultIfEmpty()
                          select new Inventory
                          {
                              Id = inv.Id,
                              JobId = inv.JobId,
                              OrganizationId = inv.OrganizationId,
                              InventoryInNumber = inv.InventoryInNumber,
                              InventoryInDateTime = inv.InventoryInDateTime,
                              JobName = job.JobName,
                              InventoryGoodsReceiptId = inv.InventoryGoodsReceiptId,
                              WarehouseId = inv.WarehouseId,
                              IsRefurbished = inv.IsRefurbished,
                              InventoryOutId = inv.InventoryOutId,
                              BusinessUnitId = inv.BusinessUnitId,
                              LegalEntityId = inv.LegalEntityId,
                              AffiliationId = inv.AffiliationId,
                              FileUpload = inv.FileUpload,
                              FileUploadName = e.Name,
                              ApprovalId = inv.ApprovalId,
                              Remark = inv.Remark,
                              SKUCode = inv.SKUCode,
                              InventoryName = inv.InventoryName,
                              Brand = inv.Brand,
                              LegalEntity = inv.LegalEntity,
                              BusinessUnit = inv.BusinessUnit,
                              TotalQty = context.InventoryDetail.Where(x => !(x.IsDeleted ?? false) && x.InventoryId == inv.Id).Sum(t => t.Qty),
                              QtyOnHand = context.InventoryDetail.Where(x => !(x.IsDeleted ?? false) && x.InventoryId == inv.Id && x.Status == "3").Sum(t => t.Qty),
                              QtyOutReturn = context.InventoryDetail.Where(x => !(x.IsDeleted ?? false) && x.InventoryId == inv.Id && x.Status == "2").Sum(t => t.Qty),
                              //context.InventoryGoodsReturn.Where(x => !(x.IsDeleted ?? false) && x.InvFaId == inv.Id ).Sum(t => t.TotalQty),
                              QtyBroken = context.InventoryDetail.Where(x => !(x.IsDeleted ?? false) && x.InventoryId == inv.Id && x.Status == "1").Sum(t => t.Qty),
                              //QtyNoReturn = context.InventoryDetail.Where(x => !(x.IsDeleted ?? false) && x.InventoryId == inv.Id && x.Status == "6").Sum(t => t.Qty),
                              LeadTime = inv.LeadTime,
                              GetFrom = inv.GetFrom,
                              UomId = inv.UomId,
                              Model = inv.Model,
                              Manufacture = inv.Manufacture,
                              VendorNameId = inv.VendorNameId,
                              InventoryDetail = inv.InventoryDetail
                          }).ToListAsync();


            return list;
        }

        public Task<List<Inventory>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<Inventory> GetAsync(string id)
        {
            //var query = await context.Inventory.Where(x => x.IsDeleted.Equals(false) && x.Id == id).FirstOrDefaultAsync();
            var query = await (from inv in context.Inventory
                               join job in context.JobDetail on inv.JobId equals job.Id into jobs
                               from job in jobs.DefaultIfEmpty()
                               join e in context.Filemasters on inv.FileUpload equals e.Id into es
                               from e in es.DefaultIfEmpty()
                               where inv.IsDeleted.Equals(false) && inv.Id == id
                               select new Inventory
                               {
                                   Id = inv.Id,
                                   JobId = inv.JobId,
                                   OrganizationId = inv.OrganizationId,
                                   InventoryInNumber = inv.InventoryInNumber,
                                   InventoryInDateTime = inv.InventoryInDateTime,
                                   JobName = job.JobName,
                                   FileUpload = inv.FileUpload,
                                   FileUploadName = e.Name,
                                   InventoryGoodsReceiptId = inv.InventoryGoodsReceiptId,
                                   WarehouseId = inv.WarehouseId,
                                   IsRefurbished = inv.IsRefurbished,
                                   InventoryOutId = inv.InventoryOutId,
                                   BusinessUnitId = inv.BusinessUnitId,
                                   LegalEntityId = inv.LegalEntityId,
                                   AffiliationId = inv.AffiliationId,
                                   ApprovalId = inv.ApprovalId,
                                   Remark = inv.Remark,
                                   SKUCode = inv.SKUCode,
                                   InventoryName = inv.InventoryName,
                                   Brand = inv.Brand,
                                   LegalEntity = inv.LegalEntity,
                                   BusinessUnit = inv.BusinessUnit,
                                   TotalQty = inv.TotalQty,
                                   QtyOnHand = inv.QtyOnHand,
                                   QtyOutReturn = inv.QtyOutReturn,
                                   QtyBroken = inv.QtyBroken,
                                   LeadTime = inv.LeadTime,
                                   GetFrom = inv.GetFrom,
                                   UomId = inv.UomId,
                                   Model = inv.Model,
                                   Manufacture = inv.Manufacture,
                                   VendorNameId = inv.VendorNameId,
                                   InventoryDetail = inv.InventoryDetail
                               }).FirstOrDefaultAsync();
            return query;
        }

        public async Task<List<InventoryDetail>> GetDetailForGoodRequest(string invId)
        {
            List<InventoryDetail> list = new List<InventoryDetail>();
            list = await (from invtd in context.InventoryDetail
                          join con in context.MasterConditionProcurement on invtd.Condition equals con.Id into icon
                          from con in icon.DefaultIfEmpty()
                          join loc in context.MasterLocationProcurement on invtd.Location equals loc.Id into iloc
                          from loc in iloc.DefaultIfEmpty()
                          join invds in context.InventoryDetailStatus on invtd.Status equals invds.getForm.ToString() into iinvds
                          from invds in iinvds.DefaultIfEmpty()
                          join om in context.Uom on invtd.UomId equals om.Id into iom
                          from om in iom.DefaultIfEmpty()
                          where invtd.InventoryId == invId && (invtd.IsDeleted.Equals(false) && (invtd.Status == "3"))
                          select new InventoryDetail
                          {
                              Id = invtd.Id,
                              InventoryId = invtd.InventoryId,
                              DateReceipt = invtd.DateReceipt,
                              Manufacturer = invtd.Manufacturer,
                              Brand = invtd.Brand,
                              Model = invtd.Model,
                              ProductNumber = invtd.ProductNumber,
                              Color = invtd.Color,
                              Condition = invtd.Condition,
                              Qty = invtd.Qty,
                              UnitName = invtd.UnitName,
                              Location = invtd.Location,
                              Status = invtd.Status,
                              ItemName = invtd.ItemName,
                              ConditionName = con != null ? con.ConditionName : "",
                              LocationName = loc != null ? loc.LocationStore : "",
                              Remark = invtd.Remark,
                              FileAttachment = invtd.FileAttachment,
                              LeadTime = invtd.LeadTime,
                              status_name = invds.statusName,
                              VendorNameId = invtd.VendorNameId,
                              VendorBrandId = invtd.VendorBrandId,
                              UomId = invtd.UomId,
                              UnitOfMeasure = om.unit_name
                          }).ToListAsync();
            return list;
        }

        public async Task<List<InventoryDetail>> GetDetail(string invId)
        {
            List<InventoryDetail> list = new List<InventoryDetail>();
            try
            {
                var goodreturnList = (from x in context.InventoryGoodsReturnDetail
                                      join y in context.InventoryGoodsReturn on x.InventoryGoodsReturnId equals y.Id
                                      where y.InvFaId == invId
                                      select x).ToList();
                var requestDetail = (from z in context.GoodRequestDetail where z.InvFaDetailId == invId select z).ToList();
                //if (goodreturnList.Count() > 0 && requestDetail.Count() > 0)
                //{
                //    list = await (from invtd in context.InventoryDetail
                //                  join con in context.MasterConditionProcurement on invtd.Condition equals con.Id into icon
                //                  from con in icon.DefaultIfEmpty()
                //                  join loc in context.MasterLocationProcurement on invtd.Location equals loc.Id into iloc
                //                  from loc in iloc.DefaultIfEmpty()
                //                  join invds in context.InventoryDetailStatus on invtd.Status equals invds.getForm.ToString() into iinvds
                //                  from invds in iinvds.DefaultIfEmpty()
                //                  join om in context.Uom on invtd.UomId equals om.Id into iom
                //                  from om in iom.DefaultIfEmpty()
                //                  join goodReturn in (
                //                          from x in context.InventoryGoodsReturnDetail
                //                          join y in context.InventoryGoodsReturn on x.InventoryGoodsReturnId equals y.Id
                //                          where y.InvFaId == invId
                //                          select new
                //                          {
                //                              ItemId = x.ItemId,
                //                              ReceivedDate = y.ReceivedDate,
                //                              InventoryGoodsRequestId = y.InventoryGoodsRequestId
                //                          }
                //                      ) on invtd.ItemId equals goodReturn.ItemId into igoodReturn
                //                  from goodReturn in igoodReturn.DefaultIfEmpty()
                //                  join reqDetail in (from z in context.GoodRequestDetail where z.InvFaDetailId == invId select new { ItemId = z.ItemId, TotalUsedDays = z.TotalUsedDays }) on invtd.ItemId equals reqDetail.ItemId into ireqDetail
                //                  from reqDetail in ireqDetail.DefaultIfEmpty()
                //                  where invtd.InventoryId == invId
                //                  select new InventoryDetail
                //                  {
                //                      Id = invtd.Id,
                //                      InventoryId = invtd.InventoryId,
                //                      DateReceipt = invtd.DateReceipt,
                //                      Manufacturer = invtd.Manufacturer,
                //                      Brand = invtd.Brand,
                //                      Model = invtd.Model,
                //                      ProductNumber = invtd.ProductNumber,
                //                      Color = invtd.Color,
                //                      Condition = invtd.Condition,
                //                      Qty = invtd.Qty,
                //                      UnitName = invtd.UnitName,
                //                      Location = invtd.Location,
                //                      ConditionName = con != null ? con.ConditionName : "",
                //                      LocationName = loc != null ? loc.LocationStore : "",
                //                      Remark = invtd.Remark,
                //                      FileAttachment = invtd.FileAttachment,
                //                      status_name = invds.statusName,
                //                      VendorNameId = invtd.VendorNameId,
                //                      VendorBrandId = invtd.VendorBrandId,
                //                      //LeadTime = (DateTime.Now.Date - (goodReturn.ReceivedDate != null ? DateTime.Parse(goodReturn.ReceivedDate.ToString()).Date : DateTime.Now.Date)).Days - Convert.ToDecimal((reqDetail.TotalUsedDays == null ? 0 : reqDetail.TotalUsedDays))
                //                      LeadTime = invtd.LeadTime,
                //                      UomId = invtd.UomId,
                //                      UnitOfMeasure = om.unit_name
                //                  }).ToListAsync();
                //}
                //else if (goodreturnList.Count() > 0 && requestDetail.Count() == 0)
                //{
                //    list = await (from invtd in context.InventoryDetail
                //                  join con in context.MasterConditionProcurement on invtd.Condition equals con.Id into icon
                //                  from con in icon.DefaultIfEmpty()
                //                  join loc in context.MasterLocationProcurement on invtd.Location equals loc.Id into iloc
                //                  from loc in iloc.DefaultIfEmpty()
                //                  join invds in context.InventoryDetailStatus on invtd.Status equals invds.getForm.ToString() into iinvds
                //                  from invds in iinvds.DefaultIfEmpty()
                //                  join om in context.Uom on invtd.UomId equals om.Id into iom
                //                  from om in iom.DefaultIfEmpty()
                //                  join goodReturn in (
                //                          from x in context.InventoryGoodsReturnDetail
                //                          join y in context.InventoryGoodsReturn on x.InventoryGoodsReturnId equals y.Id
                //                          where y.InvFaId == invId
                //                          select new
                //                          {
                //                              ItemId = x.ItemId,
                //                              ReceivedDate = y.ReceivedDate,
                //                              InventoryGoodsRequestId = y.InventoryGoodsRequestId
                //                          }
                //                      ) on invtd.ItemId equals goodReturn.ItemId into igoodReturn
                //                  from goodReturn in igoodReturn.DefaultIfEmpty()
                //                  where invtd.InventoryId == invId
                //                  select new InventoryDetail
                //                  {
                //                      Id = invtd.Id,
                //                      InventoryId = invtd.InventoryId,
                //                      DateReceipt = invtd.DateReceipt,
                //                      Manufacturer = invtd.Manufacturer,
                //                      Brand = invtd.Brand,
                //                      Model = invtd.Model,
                //                      ProductNumber = invtd.ProductNumber,
                //                      Color = invtd.Color,
                //                      Condition = invtd.Condition,
                //                      Qty = invtd.Qty,
                //                      UnitName = invtd.UnitName,
                //                      Location = invtd.Location,
                //                      Status = invtd.Status,
                //                      ItemName = invtd.ItemName,
                //                      ConditionName = con != null ? con.ConditionName : "",
                //                      LocationName = loc != null ? loc.LocationStore : "",
                //                      Remark = invtd.Remark,
                //                      FileAttachment = invtd.FileAttachment,
                //                      LeadTime = invtd.LeadTime,
                //                      status_name = invds.statusName,
                //                      VendorNameId = invtd.VendorNameId,
                //                      VendorBrandId = invtd.VendorBrandId,
                //                      UomId = invtd.UomId,
                //                      UnitOfMeasure = om.unit_name
                //                      //LeadTime = (DateTime.Now.Date - (goodReturn.ReceivedDate != null ? DateTime.Parse(goodReturn.ReceivedDate.ToString()).Date : DateTime.Now.Date)).Days
                //                  }).ToListAsync();
                //}
                //else if (goodreturnList.Count() == 0 && requestDetail.Count() > 0)
                //{
                //    list = await (from invtd in context.InventoryDetail
                //                  join con in context.MasterConditionProcurement on invtd.Condition equals con.Id into icon
                //                  from con in icon.DefaultIfEmpty()
                //                  join loc in context.MasterLocationProcurement on invtd.Location equals loc.Id into iloc
                //                  from loc in iloc.DefaultIfEmpty()
                //                  join invds in context.InventoryDetailStatus on invtd.Status equals invds.getForm.ToString() into iinvds
                //                  from invds in iinvds.DefaultIfEmpty()
                //                  join om in context.Uom on invtd.UomId equals om.Id into iom
                //                  from om in iom.DefaultIfEmpty()
                //                  where invtd.InventoryId == invId
                //                  select new InventoryDetail
                //                  {
                //                      Id = invtd.Id,
                //                      InventoryId = invtd.InventoryId,
                //                      DateReceipt = invtd.DateReceipt,
                //                      Manufacturer = invtd.Manufacturer,
                //                      Brand = invtd.Brand,
                //                      Model = invtd.Model,
                //                      ProductNumber = invtd.ProductNumber,
                //                      Color = invtd.Color,
                //                      Condition = invtd.Condition,
                //                      Qty = invtd.Qty,
                //                      UnitName = invtd.UnitName,
                //                      Location = invtd.Location,
                //                      ConditionName = con != null ? con.ConditionName : "",
                //                      LocationName = loc != null ? loc.LocationStore : "",
                //                      Remark = invtd.Remark,
                //                      FileAttachment = invtd.FileAttachment,
                //                      LeadTime = invtd.LeadTime,
                //                      status_name = invds.statusName,
                //                      VendorNameId = invtd.VendorNameId,
                //                      VendorBrandId = invtd.VendorBrandId,
                //                      UomId = invtd.UomId,
                //                      UnitOfMeasure = om.unit_name
                //                  }).ToListAsync();
                //}
                //else
                //{
                //    list = await (from invtd in context.InventoryDetail
                //                  join con in context.MasterConditionProcurement on invtd.Condition equals con.Id into icon
                //                  from con in icon.DefaultIfEmpty()
                //                  join loc in context.MasterLocationProcurement on invtd.Location equals loc.Id into iloc
                //                  from loc in iloc.DefaultIfEmpty()
                //                  join invds in context.InventoryDetailStatus on invtd.Status equals invds.getForm.ToString() into iinvds
                //                  from invds in iinvds.DefaultIfEmpty()
                //                  join om in context.Uom on invtd.UomId equals om.Id into iom
                //                  from om in iom.DefaultIfEmpty()
                //                  where invtd.InventoryId == invId && (invtd.IsDeleted.Equals(false))
                //                  select new InventoryDetail
                //                  {
                //                      Id = invtd.Id,
                //                      InventoryId = invtd.InventoryId,
                //                      DateReceipt = invtd.DateReceipt,
                //                      Manufacturer = invtd.Manufacturer,
                //                      Brand = invtd.Brand,
                //                      Model = invtd.Model,
                //                      ProductNumber = invtd.ProductNumber,
                //                      Color = invtd.Color,
                //                      Condition = invtd.Condition,
                //                      Qty = invtd.Qty,
                //                      UnitName = invtd.UnitName,
                //                      Location = invtd.Location,
                //                      Status = invtd.Status,
                //                      ItemName = invtd.ItemName,
                //                      ConditionName = con != null ? con.ConditionName : "",
                //                      LocationName = loc != null ? loc.LocationStore : "",
                //                      Remark = invtd.Remark,
                //                      FileAttachment = invtd.FileAttachment,
                //                      LeadTime = invtd.LeadTime,
                //                      status_name = invds.statusName,
                //                      VendorNameId = invtd.VendorNameId,
                //                      VendorBrandId = invtd.VendorBrandId,
                //                      UomId = invtd.UomId,
                //                      UnitOfMeasure = om.unit_name
                //                  }).ToListAsync();
                //}

                //list = await context.InventoryDetail.Where(x => x.IsDeleted.Equals(false) && x.InventoryId == invId).ToListAsync();
                list = await (from invtd in context.InventoryDetail
                              join con in context.MasterConditionProcurement on invtd.Condition equals con.Id into icon
                              from con in icon.DefaultIfEmpty()
                              join loc in context.MasterLocationProcurement on invtd.Location equals loc.Id into iloc
                              from loc in iloc.DefaultIfEmpty()
                              join invds in context.InventoryDetailStatus on invtd.Status equals invds.getForm.ToString() into iinvds
                              from invds in iinvds.DefaultIfEmpty()
                              join om in context.Uom on invtd.UomId equals om.Id into iom
                              from om in iom.DefaultIfEmpty()
                              where invtd.InventoryId == invId && (invtd.IsDeleted.Equals(false))
                              select new InventoryDetail
                              {
                                  Id = invtd.Id,
                                  InventoryId = invtd.InventoryId,
                                  DateReceipt = invtd.DateReceipt,
                                  Manufacturer = invtd.Manufacturer,
                                  Brand = invtd.Brand,
                                  Model = invtd.Model,
                                  ProductNumber = invtd.ProductNumber,
                                  Color = invtd.Color,
                                  Condition = invtd.Condition,
                                  Qty = invtd.Qty,
                                  UnitName = invtd.UnitName,
                                  Location = invtd.Location,
                                  Status = invtd.Status,
                                  ItemName = invtd.ItemName,
                                  ConditionName = con != null ? con.ConditionName : "",
                                  LocationName = loc != null ? loc.LocationStore : "",
                                  Remark = invtd.Remark,
                                  FileAttachment = invtd.FileAttachment,
                                  LeadTime = invtd.LeadTime,
                                  status_name = invds.statusName,
                                  VendorNameId = invtd.VendorNameId,
                                  VendorBrandId = invtd.VendorBrandId,
                                  UomId = invtd.UomId,
                                  UnitOfMeasure = om.unit_name
                              }).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return await GetFileList(list);

        }
        public async Task<List<InventoryDetail>> GetDetailByGoodReceipt(string grId)
        {
            var getInventoryId = await context.Inventory.Where(x => x.IsDeleted.Equals(false) && x.InventoryGoodsReceiptId == grId).FirstOrDefaultAsync();

            if (getInventoryId != null)
            {
                return await context.InventoryDetail.Where(x => x.IsDeleted.Equals(false) && x.InventoryId == getInventoryId.Id).ToListAsync();
            }
            else
            {
                return new List<InventoryDetail>();
            }
            //var query = await context.InventoryDetail.Where(x => x.IsDeleted.Equals(false) && x.InventoryId==getInventoryId.Id).ToListAsync();
            //return query;
        }

        public async Task<Inventory> GetInvAll(string invId)
        {
            var query = await (from inv in context.Inventory
                               join job in context.JobDetail on inv.JobId equals job.Id into invjob
                               from job in invjob.DefaultIfEmpty()
                               where inv.Id.Contains(invId)
                               select new Inventory
                               {
                                   Id = inv.Id,
                                   BusinessUnitId = inv.BusinessUnitId,
                                   BusinessUnit = inv.BusinessUnit,
                                   LegalEntityId = inv.LegalEntityId,
                                   LegalEntity = inv.LegalEntity,
                                   JobId = job.Id,
                                   InventoryDetail = inv.InventoryDetail,
                                   JobName = job.JobName,
                               }).FirstOrDefaultAsync();
            return query;
        }

        public async Task<Inventory> GetDataInventoryStockCard(string invId)
        {
            var query = await (from inv in context.Inventory
                               join job in context.JobDetail on inv.JobId equals job.Id into invjob
                               from job in invjob.DefaultIfEmpty()
                               join e in context.Filemasters on inv.FileUpload equals e.Id into es
                               from e in es.DefaultIfEmpty()
                               where inv.Id.Contains(invId)
                               select new Inventory
                               {
                                   Id = inv.Id,
                                   BusinessUnitId = inv.BusinessUnitId,
                                   BusinessUnit = inv.BusinessUnit,
                                   LegalEntityId = inv.LegalEntityId,
                                   LegalEntity = inv.LegalEntity,
                                   JobId = job.Id,
                                   InventoryDetail = inv.InventoryDetail,
                                   JobName = job.JobName,
                                   CreatedBy = inv.CreatedBy,
                                   CreatedOn = inv.CreatedOn,
                                   FileUpload = inv.FileUpload,
                                   Brand = inv.Brand,
                                   ReOrderLevel = inv.ReOrderLevel,
                                   Remark = inv.Remark,
                                   InventoryName = inv.InventoryName,
                                   SubBrand = inv.SubBrand,
                                   QtyOnHand = context.InventoryDetail.Where(x => !(x.IsDeleted ?? false) && x.InventoryId == inv.Id && x.Status == "3").Sum(t => t.Qty),
                                   CategoryId = inv.CategoryId,
                                   LeadTime = inv.LeadTime,
                                   Status = inv.Status,
                                   StatusName = inv.StatusName,
                                   FileUploadName = e.Name,
                                   VendorNameId = inv.VendorNameId
                               }).FirstOrDefaultAsync();
            return query;
        }

        public async Task<List<InventoryDetail>> GetDetailForInventoryStockCard(string invid)
        {
            var result = new List<InventoryDetail>();
            var queryGoodReceipt = await (from q in context.Inventory
                                          join r in context.InventoryGoodsReceiptsDetail on q.Id equals r.InventoryId
                                          join s in context.InventoryGoodsReceipts on r.InventoryGoodsReceiptId equals s.Id
                                          join loc in context.MasterLocationProcurement on s.LocationId equals loc.Id into iloc
                                          from loc in iloc.DefaultIfEmpty()
                                          where r.InventoryId == invid
                                          select new InventoryDetail()
                                          {
                                              Id = r.Id,
                                              InventoryId = invid,
                                              Transaction = "Purchase Order",
                                              ReferenceNumber = r.InventoryGoodsReceiptId,
                                              InventoryName = r.InventoryName,
                                              DateReceipt = s.GoodsReceiptDatetime,
                                              Location = s.LocationId,
                                              LocationName = loc != null ? loc.LocationStore : "",
                                              Qty = r.ReceivedQty ?? 0,
                                              QtyOut = 0,
                                              Difference = r.ReceivedQty ?? 0
                                          }).ToListAsync();

            var queryGoodReturn = await (from q in context.Inventory
                                         join r in context.InventoryDetail on q.Id equals r.InventoryId
                                         join s in context.InventoryGoodsReturn on r.Id equals s.InvFaId
                                         join loc in context.MasterLocationProcurement on r.Location equals loc.Id into iloc
                                         from loc in iloc.DefaultIfEmpty()
                                         where q.Id == invid
                                         select new InventoryDetail()
                                         {
                                             Id = r.Id,
                                             InventoryId = invid,
                                             Transaction = "Goods Request",
                                             ReferenceNumber = s.Id,
                                             InventoryName = r.InventoryName,
                                             DateReceipt = s.CreatedOn,
                                             Location = r.Location,
                                             LocationName = loc != null ? loc.LocationStore : "",
                                             Qty = s.TotalQty ?? 0,
                                             QtyOut = context.InventoryGoodsReturn.Where(x => !(x.IsDeleted ?? false) && x.InvFaId == r.Id && x.Status != "3").Sum(t => t.TotalQty) ?? 0,
                                             Difference = s.TotalQty - (context.InventoryGoodsReturn.Where(x => !(x.IsDeleted ?? false) && x.InvFaId == r.Id && x.Status != "3").Sum(t => t.TotalQty)) ?? 0
                                         }).ToListAsync();
            result.AddRange(queryGoodReceipt);
            result.AddRange(queryGoodReturn);

            return result;
        }


        private async Task<InventoryDetail> ProcessUpload(InventoryDetail entity)
        {
            var file = await uploadFile.Upload(entity.FileUpload, null);
            if (!string.IsNullOrEmpty(file))
            {
                entity.FileAttachment = file;
            }
            return entity;
        }

        private async Task<InventoryDetail> GetFile(InventoryDetail data)
        {
            if (data.FileAttachment != null)
            {
                var file = await context.Filemasters.Where(x => x.Id == data.FileAttachment).FirstOrDefaultAsync();
                if (file != null) data.FileAttachmentName = file.Name;
            }

            return data;
        }

        private async Task<List<InventoryDetail>> GetFileList(List<InventoryDetail> datas)
        {
            foreach (InventoryDetail data in datas)
            {
                if (data.FileAttachment != null)
                {
                    var file = await context.Filemasters.AsNoTracking().Where(x => x.Id == data.FileAttachment).FirstOrDefaultAsync();
                    if (file != null) data.FileAttachmentName = file.Name;
                }
            }
            return datas;
        }

        public async Task<int> AddDetail(InventoryDetail model)
        {
            var data = await ProcessUpload(model);
            data.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(data);
            return await context.SaveChangesAsync();
        }


        /// <summary>
        /// Edit Detail Inventory fn.inventory_detail
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> EditDetail(InventoryDetail model)
        {
            var data = model;

            if (model.FileUpload != null)
            {
                data = await ProcessUpload(model);
            }

            context.PhoenixEdit(data);
            return await context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(Inventory entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteRageAsync(params Inventory[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteSoftAsync(Inventory entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteSoftRangeAsync(params Inventory[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> EditAsync(Inventory entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddAsync(Inventory entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddRangeAsync(params Inventory[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<Inventory> Get(string id)
        {
            throw new NotImplementedException();
        }
    }
}
