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
    public interface IMediaPlanEstimatesService : IDataService<MediaPlanHeader>
    {
        Task<List<MediaPlanHeader>> GetMediaPlanHeaders(string status);
        Task<List<MediaPlanDetail>> GetMediaPlanDetails(string Id, string option);
        Task<MediaPlanHeader> GetMediaTransactionID(string caseOption, string jobid);
        Task<MediaPlanHeader> Approve(MediaPlanHeader entity);
        Task<MediaPlanHeader> Reject(MediaPlanHeader entity);
        Task<MediaPlanHeader> Finish(MediaPlanHeader entity);
        Task<MediaPlanHeader> UploadReport(MediaPlanHeader entity);
        Task<MediaPlanDetail> UploadFile(IFormFile file);
    }
    public class MediaPlanEstimatesService : IMediaPlanEstimatesService
    {
        readonly DataContext context;
        readonly GlobalFunctionApproval globalFcApproval;
        readonly IManageMenuService menuService;
        readonly FileService uploadFile;
        string idTemplate = "";


        public MediaPlanEstimatesService(DataContext context, GlobalFunctionApproval globalFcApproval, IManageMenuService menuService, FinancePeriodService financePeriod, FileService uploadFile)
        {
            this.context = context;
            this.globalFcApproval = globalFcApproval;
            this.menuService = menuService;
            this.uploadFile = uploadFile;
        }

        public async Task<int> AddAsync(MediaPlanHeader entity)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (entity.StatusID != StatusTransaction.Draft.ToString())
                    {
                        var chechedFlag = entity.CheckedFlag;
                        for (int i = 0; i < entity.MediaPlanDetails.Count(); i++)
                        {
                            if (entity.MediaPlanDetails[i].Id == chechedFlag)
                            {
                                entity.MediaPlanDetails[i].IsChecked = true;
                            }
                            else
                            {
                                entity.MediaPlanDetails[i].IsChecked = false;
                            }
                        }

                        await globalFcApproval.UnsafeSubmitApproval<MediaPlanHeader>(false, entity.Id, int.Parse(entity.StatusID), $"{ApprovalLink.MediaPlanEstimates}?Id={entity.Id}&isApprove=true", idTemplate, MenuUnique.MediaPlan, entity);
                        
                        if (entity.MediaPlanDetails != null)
                        {
                            //remove existing data
                            var additional = await context.MediaPlanDetail.AsNoTracking().Where(x => x.PlantHeaderID == entity.Id).ToListAsync();
                            if (additional.Count > 0)
                            {
                                context.MediaPlanDetail.RemoveRange(additional);
                            }

                            //validate again, if still exist additional data after removerange
                            if (entity.MediaPlanDetails != null)
                            {
                                Parallel.ForEach(entity.MediaPlanDetails, (item) =>
                                {
                                    item.PlantHeaderID = entity.Id;
                                    context.PhoenixAddAsync(item); // save detail
                                });
                            }
                        }
                    }
                    else
                    {
                        await context.PhoenixAddAsync(entity); //save header

                        if (entity.MediaPlanDetails != null)
                        {
                            //remove existing data
                            var additional = await context.MediaPlanDetail.AsNoTracking().Where(x => x.PlantHeaderID == entity.Id).ToListAsync();
                            if (additional.Count > 0)
                            {
                                context.MediaPlanDetail.RemoveRange(additional);
                            }

                            //validate again, if still exist additional data after removerange
                            if (entity.MediaPlanDetails != null)
                            {
                                Parallel.ForEach(entity.MediaPlanDetails, (item) =>
                                {
                                    item.PlantHeaderID = entity.Id;
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

        public async Task<int> EditAsync(MediaPlanHeader entity)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (entity.StatusID != StatusTransaction.Draft.ToString())
                    {
                        await globalFcApproval.UnsafeSubmitApproval<MediaPlanHeader>(true, entity.Id, int.Parse(entity.StatusID), $"{ApprovalLink.MediaPlanEstimates}?Id={entity.Id}&isApprove=true", idTemplate, MenuUnique.MediaPlan, entity);

                        var chechedFlag = entity.CheckedFlag;
                        for (int i = 0; i < entity.MediaPlanDetails.Count(); i++)
                        {
                            if (entity.MediaPlanDetails[i].Id == chechedFlag)
                            {
                                entity.MediaPlanDetails[i].IsChecked = true;
                            }
                            else
                            {
                                entity.MediaPlanDetails[i].IsChecked = false;
                            }
                        }
                    }
                    else
                    {
                        context.PhoenixEdit(entity);
                    }
                    foreach (var item in entity.MediaPlanDetails)
                    {
                        var dtl = await context.MediaPlanDetail.AsNoTracking().Where(a => a.Id == item.Id).FirstOrDefaultAsync();
                        item.PlantHeaderID = entity.Id;
                        if (dtl == null)
                            context.PhoenixAdd(item);
                        else
                            context.PhoenixEdit(item);
                    }

                    //Parallel.ForEach(entity.MediaPlanDetails, (item) =>
                    //{
                    //    item.PlantHeaderID = entity.Id;

                    //    context.PhoenixAdd(item);

                    //});

                    //if (entity.StatusID != StatusTransaction.Draft.ToString())
                    //{
                    //    var trapprovalmodel = await context.TrTemplateApprovals.Where(x => x.RefId == entity.Id).FirstOrDefaultAsync();
                    //    if (trapprovalmodel == null)
                    //    {
                    //        var isApprove = true;
                    //        var dataMenu = await menuService.GetByUniqueName(MenuUnique.MediaPlan).ConfigureAwait(false);
                    //        if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
                    //        var vm = new Models.ViewModel.TransApprovalHrisVm()
                    //        {
                    //            MenuId = dataMenu.Id,
                    //            RefId = entity.Id,
                    //            DetailLink = $"{ApprovalLink.MediaPlanEstimates}?Id={entity.Id}&isApprove=true",
                    //            IdTemplate = idTemplate
                    //        };
                    //        var subGroupId = ClainJwtPrincipalHelper.GetBusinessSubGroup(context.HttpContext);
                    //        var divisionId = ClainJwtPrincipalHelper.GetBusinessUnitDivisi(context.HttpContext);
                    //        var employeeId = ClainJwtPrincipalHelper.GetEmployeeId(context.HttpContext);
                    //        isApprove = await globalFcApproval.Save(vm, subGroupId, divisionId, employeeId);


                    //        if (isApprove)
                    //        {
                    //            var save = await context.SaveChangesAsync();
                    //            transaction.Commit();
                    //            return save;
                    //        }
                    //        else
                    //        {
                    //            transaction.Rollback();
                    //            throw new Exception("please check the approval template that will be processed");
                    //        }

                    //    }
                    //    else
                    //    {

                    //        var save = await context.SaveChangesAsync();
                    //        transaction.Commit();
                    //        return save;
                    //    }
                    //}
                    //else
                    //{


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

        public Task<int> AddRangeAsync(params MediaPlanHeader[] entities)
        {
            throw new NotImplementedException();
        }

        private MediaOrderHeader GetDataOrder(MediaPlanHeader e, decimal gross)
        {
            var d = new MediaOrderHeader
            {
                Id = Guid.NewGuid().ToString(),
                MediaPlanNo = e.PlanNo,
                AffiliationID = e.AffiliationID,
                BrandID = e.BrandID,
                //Campaign = e.Campaign,
                ClientID = e.ClientID,
                CurrencyID = e.CurrencyID,
                Exchange = e.Exchange,
                DivisionID = e.BusinessUnitId,
                JobID = e.JobID,
                LegalEntityID = e.LegalEntityID,
                MainServiceCategoryID = e.MainServiceCategoryID,
                Revisi = e.Revisi,
                StatusID = StatusTransaction.Draft.ToString(),
                StatusDesc = MediaStatusDesc.StatusDescMedia(StatusTransaction.Draft),
                TypeID = e.TypeID,
                TypeName = e.MediaTypeName,
                Tahun = e.Tahun,
                GrossAmount = gross
                //ASFPercent = "10",
                //ASFValue =e.ASF

            };

            return d;
        }

        public async Task<MediaPlanHeader> Approve(MediaPlanHeader entity)
        {
            try
            {
                var gross = entity.MediaPlanDetails.Sum(a => a.GrossAmount);
                var planDetails = entity.MediaPlanDetails;
                var chechedFlag = entity.CheckedFlag;
                var mediaOrderModel = new MediaOrderHeader();
                entity = context.MediaPlanHeaders.Where(a => a.Id == entity.Id).FirstOrDefault();

                //foreach (var i in planDetails)
                //{
                //    if (i.Id == chechedFlag)
                //    {
                //        i.IsChecked = true;
                //        context.PhoenixEdit(i);
                //    }
                //}

                //Save data plan to order
                mediaOrderModel = GetDataOrder(entity, gross);
                mediaOrderModel.MediaOrderNumber = await TransID.GetTransId(context, Code.MediaOrderHeader, DateTime.Today);
                context.PhoenixAdd(mediaOrderModel);

                entity.StatusID = StatusTransaction.Approved.ToString();
                await globalFcApproval.ProcessApproval<MediaPlanHeader>(entity.Id, Int32.Parse(entity.StatusID), "", entity);

                context.PhoenixEdit(entity);
                var save = await context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        public async Task<MediaPlanHeader> Finish(MediaPlanHeader entity)
        {
            try
            {
                entity = context.MediaPlanHeaders.Where(a => a.Id == entity.Id).FirstOrDefault();
                entity.StatusID = "5";
                await globalFcApproval.ProcessApproval<MediaPlanHeader>(entity.Id, Int32.Parse(entity.StatusID), "", entity);

                context.PhoenixEdit(entity);
                var save = await context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        public Task<int> DeleteAsync(MediaPlanHeader entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params MediaPlanHeader[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteSoftAsync(MediaPlanHeader entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteSoftRangeAsync(params MediaPlanHeader[] entities)
        {
            throw new NotImplementedException();
        }



        public Task<int> EditRangeAsync(MediaPlanHeader entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<MediaPlanHeader>> Get()
        {
            throw new NotImplementedException();
        }

        public Task<List<MediaPlanHeader>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<MediaPlanHeader> Get(string Id)
        {
             var data =  await context.MediaPlanHeaders.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && (x.Id.Contains(Id) || x.PlanNo.ToLower().Contains(Id.ToLower()))).FirstOrDefaultAsync();
            return await GetFile(data);
        }

        private async Task<MediaPlanHeader> GetFile(MediaPlanHeader data)
        {
            if (data.ReportFileId != null)
            {
                var a = await context.Filemasters.Where(x => x.Id == data.ReportFileId).FirstOrDefaultAsync();
                if (a != null) data.ReportFileName = a.Name;
            }
            
            return data;
        }

        public async Task<List<MediaPlanDetail>> GetMediaPlanDetails(string id, string option)
        {
            if (option == "order")
            {
                return await (from a in context.MediaPlanDetail
                              where !a.IsDeleted.GetValueOrDefault() && a.PlantHeaderID == id && a.IsChecked == true
                              orderby a.CreatedOn
                              select a).ToListAsync();
            }
            else
            {
                return await (from a in context.MediaPlanDetail
                              where !a.IsDeleted.GetValueOrDefault() && a.PlantHeaderID == id
                              orderby a.CreatedOn
                              select a).ToListAsync();
            }
            //return data;
        }

        public async Task<List<MediaPlanHeader>> GetMediaPlanHeaders(string status)
        {
            if (status == "-")
                status = "";
            var data = await (from header in context.MediaPlanHeaders
                              join detail in (from dtl in context.MediaPlanDetail
                                              group dtl by new { dtl.PlantHeaderID } into gdetail
                                              select new { idHeader = gdetail.FirstOrDefault().PlantHeaderID, grossAmount = gdetail.Sum(a => a.GrossAmount), asfAmount = gdetail.Sum(a => a.ASFValue) }) on header.Id equals detail.idHeader into dtlGrouped
                              join job in context.JobDetail on header.JobID equals job.Id
                              join brand in context.CompanyBrands on job.BrandId equals brand.Id
                              join company in context.Companys on job.CompanyId equals company.Id
                              join type in context.MediaTypes on header.TypeID equals type.Id
                              where !header.IsDeleted.GetValueOrDefault() && header.StatusID.Contains(status)
                              orderby header.StatusID ascending
                              select new MediaPlanHeader
                              {
                                  Id = header.Id,
                                  PlanNo = header.PlanNo,
                                  ClientID = header.ClientID,
                                  BrandID = header.Brand,
                                  TargetAudience = header.TargetAudience,
                                  TypeID = header.TypeID,
                                  Tahun = header.Tahun,
                                  Revisi = header.Revisi,
                                  JobIdName = job.JobNumber,
                                  MediaTypeName = type.MediaTypeName,
                                  //Campaign = header.Campaign,
                                  ClientName = company.CompanyName,
                                  Brand = brand.BrandName,
                                  Gross = dtlGrouped.FirstOrDefault().grossAmount,
                                  ASF = dtlGrouped.FirstOrDefault().asfAmount,
                                  StatusName = MediaStatusDesc.StatusDescMedia(Convert.ToInt32(header.StatusID)),
                                  StatusID = header.StatusID,
                                  CreatedBy = header.CreatedBy,
                                  CreatedOn = header.CreatedOn,
                                  ModifiedBy = header.ModifiedBy,
                                  ModifiedOn = header.ModifiedOn,
                                  ApprovedBy = header.ApprovedBy,
                                  ApprovedOn = header.ApprovedOn,
                                  IsActive = header.IsActive,
                                  IsLocked = header.IsLocked,
                                  IsDefault = header.IsDefault,
                                  IsDeleted = header.IsDeleted,
                                  OwnerId = header.OwnerId,
                                  DeletedBy = header.DeletedBy,
                                  DeletedOn = header.DeletedOn,
                                  Exchange = header.Exchange
                              }).ToListAsync();

            return data;

        }

        public async Task<MediaPlanHeader> GetMediaTransactionID(string caseOption, string jobid)
        {
            MediaPlanHeader data = new MediaPlanHeader();
            if (caseOption.ToLower() == "header")
            {
                data.PlanNo = await TransID.GetTransId(context, Code.MediaPlanHeaderID, DateTime.Today);
            }
            else
            {
                var dataAsf = (from a in context.JobDetail join b in context.CompanyBrands on a.BrandId equals b.Id where a.Id == jobid select b).FirstOrDefault();
                data.ASFPercent = dataAsf.ASFValue == null ? 0 : dataAsf.ASFValue;
                data.PlanNo = await TransID.GetTransId(context, Code.MediaPlanDetailID, DateTime.Today);
            }

            return data;
        }

        public async Task<MediaPlanHeader> Reject(MediaPlanHeader entity)
        {
            try
            {
                string remarkrejected = entity.RemarkRejected;
                entity = context.MediaPlanHeaders.Where(a => a.Id == entity.Id).FirstOrDefault();
                entity.StatusID = StatusTransaction.Rejected.ToString();
                await globalFcApproval.ProcessApproval<MediaPlanHeader>(entity.Id, Int32.Parse(entity.StatusID), remarkrejected, entity);

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

        private async Task<MediaPlanHeader> ProcessUpload(MediaPlanHeader entity)
        {
            var file1 = await uploadFile.Upload(entity.reportfile, null);
            if (!string.IsNullOrEmpty(file1))
            {
                entity.ReportFileId = file1;
            }
            return entity;
        }

        public async Task<MediaPlanHeader> UploadReport(MediaPlanHeader entity)
        {
            try
            {
                var _model = await ProcessUpload(entity);
                var dataentity = context.MediaPlanHeaders.Where(a => a.Id == entity.Id).FirstOrDefault();
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
    }
}
