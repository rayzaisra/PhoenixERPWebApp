using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Finance.Transaction.ServiceReceipt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Finance.Transaction
{
    public interface IServiceReceiptService : IDataService<ServiceReceipt>
    {
        Task<ServiceReceipt> GetPO(string poId);
        Task<List<ServiceReceiptDetail>> GetPODetail(string poId);
        Task<int> AddAsync(ServiceReceipt entity);
        Task<int> AddWithDetailAsync(ServiceReceipt Header, List<ServiceReceiptDetail> Details);
        Task<int> EditWithDetailAsync(ServiceReceipt Header, List<ServiceReceiptDetail> Details);
        Task<List<ServiceReceiptDetail>> GetDetail(string id);
    }

    public class ServiceReceiptService : IServiceReceiptService
    {
        readonly DataContext context;

        public ServiceReceiptService(DataContext context)
        {
            this.context = context;
        }

        public Task<int> AddAsync(ServiceReceipt entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddRangeAsync(params ServiceReceipt[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> AddWithDetailAsync(ServiceReceipt Header, List<ServiceReceiptDetail> Details)
        {
            //using (var transaction = await context.Database.BeginTransactionAsync())
            //{
                try
                {
                    Header.Id = Guid.NewGuid().ToString();
                    Header.ServiceReceiptNumber = await TransID.GetTransId(context, Code.ServiceReceipt, DateTime.Today);
                    Header.DeliveryOrderNumber = Header.ReferenceNumber;
                    Header.OrganizationId = "9EE4834225E334380667DDCF2F60F6DA";
                    await context.PhoenixAddAsync(Header);

                    if (Details.Count > 0)
                    {
                        List<ServiceReceiptDetail> srd = new List<ServiceReceiptDetail>();
                        foreach (ServiceReceiptDetail data in Details)
                        {
                            data.Id = Guid.NewGuid().ToString();
                            data.ServiceReceiptId = Header.Id;
                            srd.Add(data);
                        }

                        ServiceReceiptDetail[] srdArray = srd.ToArray();
                        await context.PhoenixAddRangeAsync(srdArray);
                    }
                    //var save = await context.SaveChangesAsync();
                    //transaction.Commit();
                    return await context.SaveChangesAsync(); ;
                }
                catch (Exception ex)
                {
                        //transaction.Rollback();
                        throw new Exception(ex.Message.ToString());
                }
            //}
        }

        public Task<int> DeleteAsync(ServiceReceipt entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params ServiceReceipt[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteSoftAsync(ServiceReceipt entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteSoftRangeAsync(params ServiceReceipt[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> EditAsync(ServiceReceipt entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> EditRangeAsync(ServiceReceipt entity)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditWithDetailAsync(ServiceReceipt Header, List<ServiceReceiptDetail> Details)
        {
            try
            {
                //Header.Id = Guid.NewGuid().ToString();
                //Header.ServiceReceiptNumber = await TransID.GetTransId(context, Code.ServiceReceipt, DateTime.Today);
                Header.DeliveryOrderNumber = Header.ReferenceNumber;
                Header.OrganizationId = "9EE4834225E334380667DDCF2F60F6DA";
                context.PhoenixEdit(Header);

                if (Details.Count > 0)
                {
                    List<ServiceReceiptDetail> srdtemp = new List<ServiceReceiptDetail>();
                    var checkAvailable = await context.ServiceReceiptDetail.Where(x => x.ServiceReceiptId == Header.Id && x.IsDeleted.Equals(false)).ToListAsync();
                    foreach (ServiceReceiptDetail data in checkAvailable)
                    {
                        srdtemp.Add(data);
                    }

                    ServiceReceiptDetail[] srdArraytemp = srdtemp.ToArray();
                    context.PhoenixDeleteRange(srdArraytemp);

                    List<ServiceReceiptDetail> srdnew = new List<ServiceReceiptDetail>();
                    foreach (ServiceReceiptDetail data in Details)
                    {
                        data.Id = Guid.NewGuid().ToString();
                        data.ServiceReceiptId = Header.Id;
                        srdnew.Add(data);
                    }

                    ServiceReceiptDetail[] srdArraynew = srdnew.ToArray();
                    context.PhoenixAddRange(srdArraynew);
                }
                //var save = await context.SaveChangesAsync();
                //transaction.Commit();
                return await context.SaveChangesAsync(); ;
            }
            catch (Exception ex)
            {
                //transaction.Rollback();
                throw new Exception(ex.Message.ToString());
            }
        }

        public async Task<List<ServiceReceipt>> Get()
        {
            var result = await context.ServiceReceipt.ToListAsync();
            return result;
        }

        public Task<List<ServiceReceipt>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceReceipt> Get(string id)
        {
            //var query = await context.ServiceReceipt.AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();

            var data = await (from t1 in context.ServiceReceipt
                              join t2 in context.BusinessUnits on t1.BusinessUnitId equals t2.Id into t1t2
                              from t2 in t1t2.DefaultIfEmpty()
                              join t3 in context.LegalEntity on t1.LegalEntityId equals t3.Id into t1t3
                              from t3 in t1t3.DefaultIfEmpty()
                              join t4 in context.Companys on t1.CompanyId equals t4.Id into t1t4
                              from t4 in t1t4.DefaultIfEmpty()
                              join t5 in context.CompanyBrands on t1.BrandId equals t5.Id into t1t5
                              from t5 in t1t5.DefaultIfEmpty()
                              where t1.Id == id && t1.IsDeleted.Equals(false)
                              select new ServiceReceipt
                              {
                                    Id = t1.Id,
                                    ServiceReceiptNumber = t1.ServiceReceiptNumber,
                                    ReferenceNumber = t1.ReferenceNumber,
                                    VendorId = t1.VendorId,
                                    VendorName = t1.VendorName,
                                    ServiceReceiptDatetime = t1.ServiceReceiptDatetime,
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
                                    BrandId = t1.BrandId,
                                    BrandName = t5.BrandName,
                                    BusinessUnitId = t1.BusinessUnitId,
                                    BusinessUnitName = t2.UnitName,
                                    LegalEntityId = t1.LegalEntityId,
                                    LegalEntityName = t3.LegalEntityName
                              }
                              ).FirstOrDefaultAsync();
            return data;
        }

        public async Task<List<ServiceReceiptDetail>> GetDetail(string id)
        {
            var query = await context.ServiceReceiptDetail.AsNoTracking().Where(x => x.IsDeleted.Equals(false) && x.ServiceReceiptId == id).ToListAsync();
            return query;
        }

        public async Task<ServiceReceipt> GetPO(string poId)
        {
            var queryAP = await(from ap in context.AccountPayables
                                where ap.PurchaseOrderId == poId
                                select new PurchaseOrder
                                {
                                    Id = ap.Id,
                                    SubTotal = ap.Amount
                                }).ToListAsync();



            var countAP = queryAP.Count();
            var totaldata = countAP;

            var total_termin = "";

            var potemp = await context.PurchaseOrders.Where(x => x.IsDeleted.Equals(false) && x.Id == poId).FirstOrDefaultAsync();
            //kalo langsung lunas 100%
            if (potemp.PercentPaymentTerm3 == 0 && potemp.PercentPaymentTerm2 == 0)
            {
                total_termin = "1 from 1 (" + potemp.PercentPaymentTerm1 + ")";
            }//kalau di bayar 2x
            else if (potemp.PercentPaymentTerm3 == 0 && potemp.PercentPaymentTerm1 != 0 && potemp.PercentPaymentTerm2 != 0)
            {
                if (totaldata == 0)
                {
                    total_termin = "1 from 2 (" + potemp.PercentPaymentTerm1 + ")";

                }
                else if (totaldata == 1)
                {
                    total_termin = "2 from 2 (" + potemp.PercentPaymentTerm2 + ")";
                }
                else
                {

                }
                total_termin = "More than 2";
            }
            else
            {

                if (totaldata == 0)
                {
                    total_termin = "1 from 3 (" + potemp.PercentPaymentTerm1 + ")";

                }
                else if (totaldata == 1)
                {
                    total_termin = "2 from 3 (" + potemp.PercentPaymentTerm2 + ")";
                }
                else if (totaldata == 2)
                {
                    total_termin = "3 from 3 (" + potemp.PercentPaymentTerm3 + ")";
                }
                else
                {
                    total_termin = "More than 3";
                }

            }


            var query = await(from po in context.PurchaseOrders
                              join pod in context.PurchaseOrderDetails on po.Id equals pod.PurchaseOrderId into p
                              from pod in p.DefaultIfEmpty()
                              join job in context.JobDetail on po.JobId equals job.Id into pj
                              from job in pj.DefaultIfEmpty()
                              join le in context.LegalEntity on po.LegalEntityId equals le.Id into l
                              from le in l.DefaultIfEmpty()
                              join company in context.Companys on po.CompanyId equals company.Id into c
                              from company in c.DefaultIfEmpty()
                              join companyBrand in context.CompanyBrands on po.CompanyBrandId equals companyBrand.Id into cb
                              from companyBrand in cb.DefaultIfEmpty()
                              join businessUnit in context.BusinessUnits on po.BusinessUnitId equals businessUnit.Id into b
                              from businessUnit in b.DefaultIfEmpty()
                              where po.Id == poId && po.IsDeleted.Equals(false)
                              select new ServiceReceipt
                              {
                                  PurchaseOrderId = po.Id,
                                  PurchaseOrderNumber = po.PurchaseOrderNumber,
                                  InvoicingTypeId = po.InvoicingType,
                                  InvoicingTypeName = po.InvoicingType,
                                  JobId = job.Id,
                                  JobName = job.JobName,
                                  CompanyId = company.Id,
                                  CompanyName = company.CompanyName,
                                  BrandId = companyBrand.Id,
                                  BrandName = companyBrand.BrandName,
                                  BusinessUnitId = po.BusinessUnitId,
                                  BusinessUnitName = businessUnit.UnitName,
                                  LegalEntityId = po.LegalEntityId,
                                  LegalEntityName = le.LegalEntityName,
                                  Term = total_termin
                              }
                              ).FirstOrDefaultAsync();
            return query;
        }

        public async Task<List<ServiceReceiptDetail>> GetPODetail(string poId)
        {
            var query = await(from pod in context.PurchaseOrderDetails
                              join item in context.Item on pod.ItemId equals item.Id
                              join itemType in context.ItemType on pod.ItemTypeId equals itemType.Id
                              join uom in context.Uom on pod.UomId equals uom.Id
                              where pod.PurchaseOrderId == poId && (itemType.TypeName == "Non Inventory" || itemType.TypeName == "Expense") && pod.IsDeleted.Equals(false)
                              select new ServiceReceiptDetail
                              {
                                  //Id = pod.Id,
                                  PurchaseOrderDetailId = pod.PurchaseOrderId,
                                  ItemId = pod.ItemId,
                                  ItemName = pod.ItemName,
                                  ItemTypeId = pod.ItemTypeId,
                                  UOMId = pod.UomId,                                                            
                                  ItemTypeName = itemType.TypeName,
                                  ItemCode = item.ItemCode,
                                  UnitName = uom.unit_name
                              }
                                ).ToListAsync();
            return query;
        }
    }
}
