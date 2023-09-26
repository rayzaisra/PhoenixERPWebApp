using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Finance;
using Phoenix.Data.Models.Finance.Transaction;
using Phoenix.Data.Models.Media;
using Phoenix.Data.Models.Media.Transaction.MediaOrder;
using Phoenix.Data.Services.Um;
using Phoenix.Shared.Core.PrincipalHelpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Media.Transaction
{
    public interface IMediaOrderService : IDataService<MediaOrderHeader>
    {
        Task<List<MediaOrderHeader>> GetMediaOrderHeaders(string status);
        Task<List<MediaOrderDetail>> GetMediaOrderDetails(string Id);
        Task<MediaOrderHeader> GetMediaTransactionID(string jobid);
        Task<MediaOrderHeader> Approve(MediaOrderHeader entity);
        Task<MediaOrderHeader> Reject(MediaOrderHeader entity);//Cancel
        Task<MediaOrderHeader> Finish(MediaOrderHeader entity);
        Task<MediaPlanDetail> UploadFile(IFormFile file);
        Task<MediaOrderHeader> GetdataVendor(string vendorid);
        Task<MediaOrderHeader> SaveInvoice(MediaOrderHeader entity);
        Task<MediaOrderHeader> UploadReport(MediaOrderHeader entity);
    }
    public class MediaOrderService : IMediaOrderService
    {
        readonly DataContext context;
        readonly GlobalFunctionApproval globalFcApproval;
        readonly IManageMenuService menuService;
        readonly FileService uploadFile;

        public MediaOrderService(DataContext context, GlobalFunctionApproval globalFcApproval, IManageMenuService menuService, FinancePeriodService financePeriod, FileService uploadFile)
        {
            this.context = context;
            this.globalFcApproval = globalFcApproval;
            this.menuService = menuService;
            this.uploadFile = uploadFile;
        }

        public async Task<int> AddAsync(MediaOrderHeader entity)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (entity.StatusID != StatusTransaction.Draft.ToString() && !string.IsNullOrEmpty(entity.StatusID))
                    {
                        await globalFcApproval.UnsafeSubmitApproval<MediaOrderHeader>(true, entity.Id, int.Parse(entity.StatusID), $"{ApprovalLink.MediaPlanEstimates}?Id={entity.Id}&isApprove=true", "", MenuUnique.MediaPlan, entity);
                        if (entity != null)
                        {
                            //remove existing data
                            var additional = await context.MediaOrderDetails.AsNoTracking().Where(x => x.MediaOrderNumber == entity.MediaOrderNumber).ToListAsync();
                            if (additional.Count > 0)
                            {
                                context.MediaOrderDetails.RemoveRange(additional);
                            }

                            //validate again, if still exist additional data after removerange
                            if (entity.MediaOrderDetails != null)
                            {
                                Parallel.ForEach(entity.MediaOrderDetails, (item) =>
                                {
                                    item.Id = Guid.NewGuid().ToString();
                                    item.MediaOrderNumber = entity.Id;
                                    context.PhoenixAddAsync(item); // save detail
                                });
                            }
                        }
                    }
                    else
                    {
                        await context.PhoenixAddAsync(entity); //save header

                        if (entity.MediaOrderDetails != null)
                        {
                            //remove existing data
                            var additional = await context.MediaOrderDetails.AsNoTracking().Where(x => x.MediaOrderNumber == entity.MediaOrderNumber).ToListAsync();
                            if (additional.Count > 0)
                            {
                                context.MediaOrderDetails.RemoveRange(additional);
                            }

                            //validate again, if still exist additional data after removerange
                            if (entity.MediaOrderDetails != null)
                            {
                                Parallel.ForEach(entity.MediaOrderDetails, (item) =>
                                {
                                    item.Id = Guid.NewGuid().ToString();
                                    item.MediaOrderNumber = entity.Id;
                                    context.PhoenixAddAsync(item); // save detail
                                });
                            }
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

        public Task<int> AddRangeAsync(params MediaOrderHeader[] entities)
        {
            throw new NotImplementedException();
        }

        private InvoiceReceivable GetInvoiceReceive(MediaOrderHeader e)
        {
            var hdr = new InvoiceReceivable
            {

                MainServiceCategoryId = e.MainServiceCategoryID,
                BusinessUnitId = e.DivisionID,
                LegalEntityId = e.LegalEntityID,
                AffiliationId = e.AffiliationID,
                MediaOrderNumber = e.MediaOrderNumber,
                InvoiceDate = e.DateOrder,
                ValueAddedTax = e.VATValue ?? 0,
                ExchangeRate = e.Exchange ?? 0,
                NetAmount = e.SubTotal,
                TotalCostVendor = e.GrandTotal,
                Id = Guid.NewGuid().ToString(),
                InvoiceStatusId = "1",
                PoId = Guid.NewGuid().ToString()

            };
            return hdr;

        }
        private InvoiceReceivableDetail GetInvoiceReceiveDetail(MediaOrderHeader e, string invHdrID)
        {
            decimal subtotal = e.SubTotal ?? 0;
            decimal bonus = e.value_bonus_amount ?? 0;
            var dtl = new InvoiceReceivableDetail
            {
                Id = Guid.NewGuid().ToString(),
                InvoiceReceivableId = invHdrID,
                Qty = 1,
                UnitPrice = subtotal,
                //Discount = e.value_bonus_amount,
                //PceValue = Convert.ToDouble(e.GrossAmount),
                Task = e.Description,
                Amount = subtotal - bonus <= 0 ? 0 : bonus
                //organization_id = "9EE4834225E334380667DDCF2F60F6DA"

            };
            return dtl;

        }

        public async Task<MediaOrderHeader> Approve(MediaOrderHeader entity)
        {
            try
            {
                entity = context.MediaOrderHeaders.Where(a => a.Id == entity.Id).FirstOrDefault();
                entity.StatusID = StatusTransaction.Approved.ToString();
                await globalFcApproval.ProcessApproval<MediaOrderHeader>(entity.Id, Int32.Parse(entity.StatusID), "", entity);

                context.PhoenixEdit(entity);
                var save = await context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        public Task<int> DeleteAsync(MediaOrderHeader entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params MediaOrderHeader[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteSoftAsync(MediaOrderHeader entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteSoftRangeAsync(params MediaOrderHeader[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(MediaOrderHeader entity)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    
                    if (entity.StatusID != StatusTransaction.Draft.ToString() && !string.IsNullOrEmpty(entity.StatusID))
                    {
                        await globalFcApproval.UnsafeSubmitApproval<MediaOrderHeader>(true, entity.Id, int.Parse(entity.StatusID), $"{ApprovalLink.MediaPlanEstimates}?Id={entity.Id}&isApprove=true", "", MenuUnique.MediaPlan, entity);
                    }
                    else
                    {
                        context.PhoenixEdit(entity);
                    }

                    foreach (var item in entity.MediaOrderDetails)
                    {
                        var dtl = await context.MediaOrderDetails.AsNoTracking().Where(a => a.Id == item.Id).FirstOrDefaultAsync();

                        if (dtl == null)
                        {
                            item.MediaOrderNumber = entity.Id;
                            item.Id = Guid.NewGuid().ToString();
                            context.PhoenixAdd(item);
                        }
                        else
                        {
                            context.PhoenixEdit(item);
                        }
                    }

                    //if (entity.StatusID != StatusTransaction.Draft.ToString() && !string.IsNullOrEmpty(entity.StatusID))
                    //{
                    //    var trapprovalmodel = await context.TrTemplateApprovals.Where(x => x.RefId == entity.Id).FirstOrDefaultAsync();
                    //    if (trapprovalmodel == null)
                    //    {
                    //        var isApprove = true;
                    //        var dataMenu = await menuService.GetByUniqueName(MenuUnique.MediaOrder).ConfigureAwait(false);
                    //        if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
                    //        var vm = new Models.ViewModel.TransApprovalHrisVm()
                    //        {
                    //            MenuId = dataMenu.Id,
                    //            RefId = entity.Id,
                    //            DetailLink = $"{ApprovalLink.MediaPlanEstimates}?Id={entity.Id}&isApprove=true",
                    //            IdTemplate = ""
                    //        };
                    //        var subGroupId = ClainJwtPrincipalHelper.GetBusinessSubGroup(context.HttpContext);
                    //        var divisionId = ClainJwtPrincipalHelper.GetBusinessUnitDivisi(context.HttpContext);
                    //        var employeeId = ClainJwtPrincipalHelper.GetEmployeeId(context.HttpContext);
                    //        isApprove = await globalFcApproval.Save(vm, subGroupId, divisionId, employeeId);


                    //        if (!isApprove)
                    //        {
                    //            transaction.Rollback();
                    //            throw new Exception("please check the approval template that will be processed");
                    //        }
                    //    }
                    //}

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

        public Task<int> EditRangeAsync(MediaOrderHeader entity)
        {
            throw new NotImplementedException();
        }

        public async Task<MediaOrderHeader> Finish(MediaOrderHeader entity)
        {
            try
            {
                entity = context.MediaOrderHeaders.Where(a => a.Id == entity.Id).FirstOrDefault();
                entity.StatusID = "5";
                context.PhoenixEdit(entity);
                var save = await context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        public Task<List<MediaOrderHeader>> Get()
        {
            throw new NotImplementedException();
        }

        public Task<List<MediaOrderHeader>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<MediaOrderHeader> Get(string id)
        {
            var model = await context.MediaOrderHeaders.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && (x.Id.Contains(id) || x.MediaPlanNo.Contains(id))).FirstOrDefaultAsync();
            return await GetFile(model);
        }

        public async Task<MediaOrderHeader> GetdataVendor(string vendorid)
        {
            return await (from a in context.Companys
                          join b in context.CompanyBrands on a.Id equals b.CompanyId
                          where !(a.IsDeleted ?? false)
                                && a.IsClient.Equals(false)
                                && a.IsVendor.Equals(true)
                                && string.IsNullOrEmpty(vendorid) ? true : a.Id == vendorid
                          select new MediaOrderHeader
                          {
                              BrandID = b.Id,
                              VendorBrand = b.BrandName,
                              PicName = a.ContactName,
                              NPWPNumber = a.TaxRegistrationNumber,
                              Address = a.OfficeAddress,
                              TermOfPayment = b.TermOfPayment
                          }).FirstOrDefaultAsync();
        }

        public async Task<List<MediaOrderDetail>> GetMediaOrderDetails(string Id)
        {
            var data = await (from detail in context.MediaOrderDetails
                              join type in context.MediaTypes on detail.TypeID equals type.Id
                              where !detail.IsDeleted.GetValueOrDefault() && detail.MediaOrderNumber == Id
                              select new MediaOrderDetail
                              {
                                  Id = detail.Id,

                                  StartPeriode = detail.StartPeriode,
                                  EndPeriode = detail.EndPeriode,
                                  TypeID = detail.TypeID,
                                  TypeName = type.MediaTypeName,
                                  FileName = detail.FileName,
                                  FileMasterId = detail.FileMasterId,
                                  //Campaign = detail.Campaign,
                                  MediaOrderNumber = detail.MediaOrderNumber,
                                  Revision = detail.Revision,
                                  IsChecked = detail.IsChecked,
                                  CreatedBy = detail.CreatedBy,
                                  CreatedOn = detail.CreatedOn,
                                  ModifiedBy = detail.ModifiedBy,
                                  ModifiedOn = detail.ModifiedOn,
                                  ApprovedBy = detail.ApprovedBy,
                                  ApprovedOn = detail.ApprovedOn,
                                  IsActive = detail.IsActive,
                                  IsLocked = detail.IsLocked,
                                  IsDefault = detail.IsDefault,
                                  IsDeleted = detail.IsDeleted,
                                  OwnerId = detail.OwnerId,
                                  DeletedBy = detail.DeletedBy,
                                  DeletedOn = detail.DeletedOn
                              }).ToListAsync();
            return data;

        }

        public async Task<List<MediaOrderHeader>> GetMediaOrderHeaders(string status)
        {
            if (status == "-")
                status = "";
            var data = await (from hdr in context.MediaOrderHeaders
                              join type in context.MediaTypes on hdr.TypeID equals type.Id
                              join job in context.JobDetail on hdr.JobID equals job.Id
                              join comp in context.Companys on hdr.VendorID equals comp.Id into gcompany
                              from comp in gcompany.DefaultIfEmpty()
                              where !hdr.IsDeleted.GetValueOrDefault() && hdr.StatusID.Contains(status)
                              orderby hdr.CreatedOn descending
                              select new MediaOrderHeader
                              {
                                  Id = hdr.Id,
                                  MediaOrderNumber = hdr.MediaOrderNumber,
                                  MediaPlanNo = hdr.MediaPlanNo,
                                  TypeID = hdr.TypeID,
                                  TypeName = type.MediaTypeName,
                                  //Campaign = hdr.Campaign,
                                  StatusID = hdr.StatusID,
                                  StatusDesc = hdr.StatusID == "5" ? "Published" : StatusTransaction.StatusName(Convert.ToInt32(hdr.StatusID)),

                                  JobIDName = job.JobNumber + "-" + job.JobName,

                                  Address = hdr.Address,
                                  VendorBrand = hdr.VendorBrand,
                                  VendorID = hdr.VendorID,
                                  PicName = hdr.PicName,
                                  NPWPNumber = hdr.NPWPNumber,
                                  Description = hdr.Description,
                                  SubTotal = hdr.SubTotal == null ? 0 : hdr.SubTotal,
                                  VATValue = hdr.VATValue == null ? 0 : hdr.VATValue,
                                  GrandTotal = hdr.GrandTotal == null ? 0 : hdr.GrandTotal,
                                  InvoicingType = hdr.InvoicingType,
                                  PaymentMethod = hdr.PaymentMethod,
                                  Exchange = hdr.Exchange,
                                  Term1Pct = hdr.Term1Pct,
                                  Term2Pct = hdr.Term2Pct,
                                  Term3Pct = hdr.Term3Pct,
                                  Term1Amount = hdr.Term1Amount,
                                  Term2Amount = hdr.Term2Amount,
                                  Term3Amount = hdr.Term3Amount,
                                  VendorName = comp.CompanyName,


                                  DateOrder = hdr.DateOrder,
                                  CreatedBy = hdr.CreatedBy,
                                  CreatedOn = hdr.CreatedOn,
                                  ModifiedBy = hdr.ModifiedBy,
                                  ModifiedOn = hdr.ModifiedOn,
                                  ApprovedBy = hdr.ApprovedBy,
                                  ApprovedOn = hdr.ApprovedOn,
                                  IsActive = hdr.IsActive,
                                  IsLocked = hdr.IsLocked,
                                  IsDefault = hdr.IsDefault,
                                  IsDeleted = hdr.IsDeleted,
                                  OwnerId = hdr.OwnerId,
                                  DeletedBy = hdr.DeletedBy,
                                  DeletedOn = hdr.DeletedOn
                              }).ToListAsync();

            return data;
        }

        public Task<MediaOrderHeader> GetMediaTransactionID(string jobid)
        {
            throw new NotImplementedException();
        }

        public async Task<MediaOrderHeader> Reject(MediaOrderHeader entity)
        {
            try
            {
                string remarkrejected = entity.RemarkRejected;
                entity = context.MediaOrderHeaders.Where(a => a.Id == entity.Id).FirstOrDefault();
                entity.StatusID = StatusTransaction.Rejected.ToString();
                await globalFcApproval.ProcessApproval<MediaOrderHeader>(entity.Id, Int32.Parse(entity.StatusID), remarkrejected, entity);

                //var modeltrapp = await context.TrTemplateApprovals.AsNoTracking().Where(x => x.RefId == entity.Id).FirstOrDefaultAsync();
                //if (modeltrapp != null)
                //{
                //    if (modeltrapp.StatusApprovedDescription == StatusTransaction.StatusName(StatusTransaction.Rejected))
                //    {
                //        entity.StatusID = StatusTransaction.Rejected.ToString();
                //        //entity.MediaPlanDetails = entity.RemarkRejected;
                //    }
                //}
                //else
                //{
                //    entity.StatusID = StatusTransaction.Rejected.ToString();
                //}
                entity.StatusID = StatusTransaction.Draft.ToString();
                context.PhoenixEdit(entity);
                var save = await context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        public async Task<MediaPlanDetail> UploadFile(IFormFile file)
        {
            MediaPlanDetail model = new MediaPlanDetail();
            TrMediaFileMaster FileMaster = new TrMediaFileMaster();

            //Upload ke FileMaster
            var fileId = await uploadFile.Upload(file, null);
            if (!string.IsNullOrEmpty(fileId))
            {
                FileMaster.FileMasterId = fileId;
                model.FileMasterId = fileId;
                model.FileName = file.FileName;
            }
            return model;
        }

        public async Task<MediaOrderHeader> SaveInvoice(MediaOrderHeader entity)
        {
            try
            {
                var invoiceReceiv = new InvoiceReceivable();
                var invoiceReceivDetail = new InvoiceReceivableDetail();
                entity = context.MediaOrderHeaders.Where(a => a.Id == entity.Id).FirstOrDefault();
                invoiceReceiv = GetInvoiceReceive(entity);
                invoiceReceiv.InvoiceNumber = await TransID.GetTransId(context, Code.InvoiceReceived, DateTime.Today);
                invoiceReceivDetail = GetInvoiceReceiveDetail(entity, invoiceReceiv.Id);

                entity.Remark = "invoiced";
                context.PhoenixEdit(entity);
                context.PhoenixAdd(invoiceReceiv);
                context.PhoenixAdd(invoiceReceivDetail);
                var save = await context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        public async Task<MediaOrderHeader> UploadReport(MediaOrderHeader entity)
        {
            try
            {
                var _model = await ProcessUpload(entity);
                var dataentity = context.MediaOrderHeaders.Where(a => a.Id == entity.Id).FirstOrDefault();
                dataentity.ReportFileId = _model.ReportFileId;
                dataentity.isReported = true;
                context.PhoenixEdit(dataentity);
                var save = await context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        private async Task<MediaOrderHeader> ProcessUpload(MediaOrderHeader entity)
        {
            var file1 = await uploadFile.Upload(entity.reportfile, null);
            if (!string.IsNullOrEmpty(file1))
            {
                entity.ReportFileId = file1;
            }
            return entity;
        }

        private async Task<MediaOrderHeader> GetFile(MediaOrderHeader data)
        {
            if (data.ReportFileId != null)
            {
                var a = await context.Filemasters.Where(x => x.Id == data.ReportFileId).FirstOrDefaultAsync();
                if (a != null) data.ReportFileName = a.Name;
            }

            return data;
        }
    }
}
