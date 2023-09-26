using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Finance;
using Phoenix.Data.Models.Finance.Transaction.Bast;
using Phoenix.Data.Models.Finance.Transaction.ServiceReceipt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Finance.Transaction
{
    public interface IBastService : IDataService<Bast>
    {
        Task<List<BastDetailDTO>> GetListDetail(string bastId, string type);
        Task<int> AddWithDetailAsync(Bast Header, List<BastDetail> Details);
        Task<int> EditWithDetailAsync(Bast Header, List<BastDetail> Details);
        Task<dynamic> GetDDLGoodReceiptDetailByPO(string poId);
        Task<InventoryGoodsReceiptDetail> GetGRDetailData(string grdId);
        Task<dynamic> GetDDLServiceReceiptDetailByPO(string poId);
        Task<ServiceReceiptDetail> GetSRDetailData(string srdId);
        Task<int> EditWithDetailClosedAsync(Bast Header, List<BastDetail> Details);
        Task<List<dynamic>> getLog(string id);
        Task<int> Rejected(Bast dto);

    }

    public class BastService : IBastService
    {
        readonly DataContext context;

        public BastService(DataContext context)
        {
            this.context = context;
        }

        public Task<int> AddAsync(Bast entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddRangeAsync(params Bast[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> AddWithDetailAsync(Bast Header, List<BastDetail> Details)
        {
            Header.Id = Guid.NewGuid().ToString();
            Header.GoodsReceiptNumber = await TransID.GetTransId(context, "BAST", DateTime.Today);
            Header.OrganizationId = "9EE4834225E334380667DDCF2F60F6DA";
            Header.Status = "1";
            await context.PhoenixAddAsync(Header);

            if (Details.Count > 0)
            {
                List<BastDetail> igrd = new List<BastDetail>();
                foreach (BastDetail data in Details)
                {
                    data.Id = Guid.NewGuid().ToString();
                    data.BastId = Header.Id;
                    if (data.GoodReceiptId != null)
                    {
                        data.BastType = "GOOD";
                    }
                    else
                    {
                        data.BastType = "SERVICE";
                    }
                    igrd.Add(data);
                }

                BastDetail[] igrdArray = igrd.ToArray();
                await context.PhoenixAddRangeAsync(igrdArray);
            }

            return await context.SaveChangesAsync();
        }

        public async Task<int> Rejected(Bast dto)
        {
            dto.OrganizationId = "9EE4834225E334380667DDCF2F60F6DA";
            dto.Status = "3";
            dto.IsDeleted = false;
            context.Set<Bast>().Update(dto);

            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteAsync(Bast entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params Bast[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteSoftAsync(Bast entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteSoftRangeAsync(params Bast[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> EditAsync(Bast entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> EditRangeAsync(Bast entity)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditWithDetailAsync(Bast Header, List<BastDetail> Details)
        {
            //Header.Id = Guid.NewGuid().ToString();
            //Header.InventoryInNumber = await TransID.GetTransId(context, "INV", DateTime.Today);
            Header.OrganizationId = "9EE4834225E334380667DDCF2F60F6DA";
            context.PhoenixEdit(Header);

            if (Details.Count > 0)
            {
                List<BastDetail> igrd = new List<BastDetail>();
                foreach (BastDetail data in Details)
                {
                    //data.Id = Guid.NewGuid().ToString();
                    data.BastId = Header.Id;
                    if (data.GoodReceiptId != null)
                    {
                        data.BastType = "GOOD";
                    }
                    else
                    {
                        data.BastType = "SERVICE";
                    }
                    igrd.Add(data);
                }

                BastDetail[] igrdArray = igrd.ToArray();
                context.PhoenixEditRange(igrdArray);
            }

            return await context.SaveChangesAsync();
        }

        public async Task<int> EditWithDetailClosedAsync(Bast Header, List<BastDetail> Details)
        {
            //Header.Id = Guid.NewGuid().ToString();
            //Header.InventoryInNumber = await TransID.GetTransId(context, "INV", DateTime.Today);
            Header.OrganizationId = "9EE4834225E334380667DDCF2F60F6DA";
            Header.Status = "2";
            context.PhoenixEdit(Header);

            if (Details.Count > 0)
            {
                List<BastDetail> igrd = new List<BastDetail>();
                foreach (BastDetail data in Details)
                {
                    //data.Id = Guid.NewGuid().ToString();
                    data.BastId = Header.Id;
                    if (data.GoodReceiptId != null)
                    {
                        data.BastType = "GOOD";
                    }
                    else
                    {
                        data.BastType = "SERVICE";
                    }
                    igrd.Add(data);
                }

                BastDetail[] igrdArray = igrd.ToArray();
                context.PhoenixEditRange(igrdArray);
            }

            return await context.SaveChangesAsync();
        }

        public async Task<List<Bast>> Get()
        {
            return await context.Bast.Where(x => x.IsDeleted.Equals(false)).ToListAsync();
        }

        public Task<List<Bast>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<Bast> Get(string id)
        {
            return await context.Bast.Where(x => x.IsDeleted.Equals(false) && x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<dynamic> GetDDLGoodReceiptDetailByPO(string poId)
        {
            var query = await (from igrd in context.InventoryGoodsReceiptsDetail
                               join igr in context.InventoryGoodsReceipts on igrd.InventoryGoodsReceiptId equals igr.Id
                               where igrd.IsDeleted == false && igr.PurchaseOrderId == poId
                               select new
                               {
                                   Text = igr.GoodsReceiptNumber,
                                   Value = igrd.Id
                               }).ToListAsync();
            return query;
        }

        public async Task<dynamic> GetDDLServiceReceiptDetailByPO(string poId)
        {
            var query = await (from srd in context.ServiceReceiptDetail
                               join sr in context.ServiceReceipt on srd.ServiceReceiptId equals sr.Id
                               where srd.IsDeleted == false && sr.PurchaseOrderId == poId
                               select new
                               {
                                   Text = srd.Id + " - " + sr.ServiceReceiptNumber,
                                   Value = srd.Id
                               }).ToListAsync();
            return query;
        }

        public async Task<InventoryGoodsReceiptDetail> GetGRDetailData(string grdId)
        {
            var query = await (from grd in context.InventoryGoodsReceiptsDetail
                               join gr in context.InventoryGoodsReceipts on grd.InventoryGoodsReceiptId equals gr.Id
                               join fnl in context.MasterLocationProcurement on gr.LocationId equals fnl.Id
                               where grd.IsDeleted.Equals(false) && grd.Id.Equals(grdId)
                               select new InventoryGoodsReceiptDetail()
                               {
                                   Id = grd.Id,
                                   Amount = grd.Amount,
                                   Location = fnl.LocationStore, 
                                   InventoryGoodsReceiptId = grd.InventoryGoodsReceiptId, 
                                   InventoryId = grd.InventoryId,
                                   ItemCode = grd.ItemCode,
                                   ItemId = grd.ItemId,
                                   ItemTypeId = grd.ItemTypeId,
                                   ItemName = grd.ItemName,
                                   OwnerId = grd.OwnerId,
                                   ModifiedOn = grd.ModifiedOn,
                                   ItemTypeName = grd.ItemTypeName,
                                   PurchasedQty = grd.PurchasedQty,
                                   RemainingQty = grd.RemainingQty,
                                   ReceivedQty = grd.ReceivedQty,
                                   CreatedOn = grd.CreatedOn

                               }
                             ).FirstOrDefaultAsync();
            return query;
        }

        public async Task<List<BastDetailDTO>> GetListDetail(string bastId, string type)
        {
            //var query = await (from a in context.BastDetail
            //                   join b in context.InventoryGoodsReceipts on a.GoodReceiptId equals b.Id
            //                   where a.IsDeleted.Equals(false) && a.BastId == bastId && a.BastType == type
            //                   select new Phoenix.Data.Models.Finance.Transaction.Bast.BastDetailDTO()
            //                   {
            //                       Id = a.Id,
            //                       GoodReceiptId=a.GoodReceiptId,
            //                       CreatedOn=a.CreatedOn,
            //                       Mod
            //                   }
            //                   ).ToListAsync();
            List<BastDetailDTO> List = new List<BastDetailDTO>();
            var query = await context.BastDetail.Where(x => x.IsDeleted.Equals(false) && x.BastId == bastId && x.BastType == type).ToListAsync();
            foreach (var item in query)
            {
                var GoodReceiptDetail = await context.InventoryGoodsReceiptsDetail.Where(x => x.Id.Equals(item.GoodReceiptId)).SingleOrDefaultAsync();
                var GoodReceipt = await context.InventoryGoodsReceipts.Where(x => x.Id.Equals(GoodReceiptDetail.InventoryGoodsReceiptId)).SingleOrDefaultAsync();

                BastDetailDTO entity = new BastDetailDTO()
                {
                    Id = item.Id,
                    BastId = item.BastId,
                    GoodReceiptId = item.GoodReceiptId,
                    GoodReceiptNumber = GoodReceipt.GoodsReceiptNumber,
                    ServiceReceiptId = item.ServiceReceiptId,
                    BusinessUnitId = item.BusinessUnitId,
                    LegalEntityId = item.LegalEntityId,
                    AffiliationId = item.AffiliationId,
                    Remarks = item.Remarks,
                    DateReceipt = item.DateReceipt,
                    ItemType = item.ItemType,
                    ItemName = item.ItemName,
                    Qty = item.Qty,
                    ReceivedQty = item.ReceivedQty,
                    RemainingQty = item.RemainingQty,
                    Location = item.Location,
                    InvFaId = item.InvFaId,
                    BastType = item.BastType
                };
                List.Add(entity);
            }

            return List;
        }

        public async Task<List<dynamic>> getLog(string id)
        {
            var open = await (from a in context.Bast
                              join
                               b in context.EmployeeBasicInfos on a.CreatedBy equals b.Id
                              where a.CreatedBy != null && a.IsDeleted != null
                              select new
                              {
                                  NameEmployee = b.NameEmployee,
                                  CreatedDt = a.CreatedOn,
                                  Status = "Waiting Approvall",
                                  Remark = string.Empty
                              }
                             ).ToListAsync();
            var rejected = await (from a in context.Bast
                                  join
                                    b in context.EmployeeBasicInfos on a.RejectedBy equals b.Id
                                  where a.RejectedBy != null && a.IsDeleted != null
                                  select new
                                  {
                                      NameEmployee = b.NameEmployee,
                                      CreatedDt = a.CreatedOn,
                                      Status = "Rejected",
                                      Remark = string.Empty
                                  }
                             ).ToListAsync();
            var approved = await (from a in context.Bast
                                  join
                                    b in context.EmployeeBasicInfos on a.ApprovedBy equals b.Id
                                  where a.CreatedBy != null && a.IsDeleted != null
                                  select new
                                  {
                                      NameEmployee = b.NameEmployee,
                                      CreatedDt = a.CreatedOn,
                                      Status = "Approved",
                                      Remark = string.Empty
                                  }
                             ).ToListAsync();

            List<dynamic> result = new List<dynamic>();
            result.AddRange(open);
            result.AddRange(rejected);
            result.AddRange(approved);

            return result;


        }

        public async Task<ServiceReceiptDetail> GetSRDetailData(string srdId)
        {
            return await context.ServiceReceiptDetail.Where(x => x.IsDeleted.Equals(false) && x.Id == srdId).FirstOrDefaultAsync();
        }
    }
}
