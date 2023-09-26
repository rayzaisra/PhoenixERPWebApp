using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.RestApi;
using Phoenix.Data.Services.Um;
using Phoenix.Shared.Core.Contexts;
using Phoenix.Shared.Core.PrincipalHelpers;

namespace Phoenix.Data
{
    public interface ITrainingRequisitionService : IDataServiceHris<TrainingRequisition>
    {
        Task<List<TrainingRequisitionDetail>> GetTrainingDetailWithTrainingRequestId(string TrainingRequisitionId);
        Task<List<TrainingRequisitionDetail>> GetTrainingDetailJoin();
        Task<List<TrainingRequisition>> GetTrainingRequestList();
        Task<TrainingRequisitionDTO> AddWithDetail(TrainingRequisition Header, List<TrainingRequisitionDetail> Details);
        Task<TrainingRequisitionDTO> EditWithDetail(TrainingRequisition Header, List<TrainingRequisitionDetail> Details);
        Task<TrainingRequisition> Approve(TrainingRequisition entity);
        Task<TrainingRequisition> Reject(TrainingRequisition entity);
    }

    public class TrainingRequisitionService : ITrainingRequisitionService
    {
        readonly DataContext context;
        readonly GlobalFunctionApproval globalFcApproval;
        readonly IManageMenuService menuService;
        string idTemplate = "";

        /// <summary>
        /// And endpoint to manage Training Requisiotion
        /// </summary>
        /// <param name="context">Database context</param>
        public TrainingRequisitionService(DataContext context, GlobalFunctionApproval globalFcApproval, IManageMenuService menuService)
        {
            this.context = context;
            this.globalFcApproval = globalFcApproval;
            this.menuService = menuService;
        }

        public async Task<int> AddAsync(TrainingRequisition entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> EditAsync(TrainingRequisition entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }


        public async Task<int> DeleteSoftAsync(TrainingRequisition entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<List<TrainingRequisition>> Get()
        {
            return await context.TrainingRequisition.Where(x => x.IsDeleted.Equals(false)).ToListAsync();
        }

        public async Task<TrainingRequisition> Get(string Id)
        {
            //var query =  await context.TrainingRequisition.Where(x => x.IsDeleted.Equals(false) && x.Id==Id).FirstOrDefaultAsync();
            var query = await (from a in context.TrainingRequisition
                               join t in context.TrTemplateApprovals on a.Id equals t.RefId into tta
                               from ttalj in tta.DefaultIfEmpty()
                               where a.IsDeleted.Equals(false) && a.Id == Id
                               select new TrainingRequisition()
                               {
                                   Id = a.Id,
                                   AddRecomendation = a.AddRecomendation,
                                   ApprovedBy = a.ApprovedBy,
                                   ApprovedOn = a.ApprovedOn,
                                   BusinessUnitDepartementId = a.BusinessUnitDepartementId,
                                   BusinessUnitDivisionId = a.BusinessUnitDivisionId,
                                   BusinessUnitId = a.BusinessUnitId,
                                   BusinessUnitName = a.BusinessUnitName,
                                   Code = a.Code,
                                   CreatedBy = a.CreatedBy,
                                   CreatedOn = a.CreatedOn,
                                   DeletedBy = a.DeletedBy,
                                   DeletedOn = a.DeletedOn,
                                   IsActive = a.IsActive,
                                   IsDefault = a.IsDefault,
                                   IsDeleted = a.IsDeleted,
                                   IsLocked = a.IsLocked,
                                   ModifiedBy = a.ModifiedBy,
                                   ModifiedOn = a.ModifiedOn,
                                   Notes = a.Notes,
                                   OwnerId = a.OwnerId,
                                   ParticipantList = a.ParticipantList,
                                   PurposeEndDate = a.PurposeEndDate,
                                   PurposeStartDate = a.PurposeStartDate,
                                   RemarkRejected = a.RemarkRejected,
                                   RequesterEmployeeBasicInfoId = a.RequesterEmployeeBasicInfoId,
                                   RequesterEmployeeBasicInfoName = a.RequesterEmployeeBasicInfoName,
                                   Status = a.Status,
                                   TrainingId = a.TrainingId,
                                   TrainingName = a.TrainingName,
                                   TrainingType = a.TrainingType,
                                   Venue = a.Venue,
                                   ApprovalId = ttalj.Id
                               }).FirstOrDefaultAsync();

            return query;
        }

        public Task<int> ApproveAsync(TrainingRequisition entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<TrainingRequisitionDetail>> GetTrainingDetailWithTrainingRequestId(string TrainingRequisitionId)
        {
            var model = await (from tb in context.TrainingRequisitionDetail
                               join tr in context.TrainingRequisition on tb.TrainingRequisitionId equals tr.Id
                               join eb in context.EmployeeBasicInfos on tb.EmployeeBasicInfoId equals eb.Id
                               join bujl in context.BusinessUnitJobLevels on eb.BusinessUnitJobLevelId equals bujl.Id
                               join jt in context.JobTitles on bujl.JobTitleId equals jt.Id
                               join jg in context.JobGrades on bujl.JobGradeId equals jg.Id
                               join bu in context.BusinessUnits on bujl.BusinessUnitDivisionId equals bu.Id
                               join t in context.TrTemplateApprovals on tr.Id equals t.RefId into tta
                               from ttalj in tta.DefaultIfEmpty()
                               where tb.IsDeleted.Equals(false) && tb.TrainingRequisitionId == TrainingRequisitionId
                               select new TrainingRequisitionDetail
                               {
                                   Id = tb.Id,
                                   EmployeeBasicInfoId = tb.EmployeeBasicInfoId,
                                   EmployeeBasicInfoName = eb.NameEmployee,
                                   JobTitleId = bujl.JobTitleId,
                                   JobTitleName = jt.TitleName,
                                   JobGradeId = bujl.JobGradeId,
                                   JobGradeName = jg.GradeName,
                                   DivisionId = bujl.BusinessUnitDivisionId,
                                   DivisionName = bu.UnitName,
                                   ApprovalId = ttalj.Id
                               }).ToListAsync();

            return model;
        }

        public async Task<List<TrainingRequisitionDetail>> GetTrainingDetailJoin()
        {
            var model = await(from tb in context.TrainingRequisitionDetail
                              join tr in context.TrainingRequisition on tb.TrainingRequisitionId equals tr.Id
                              join eb in context.EmployeeBasicInfos on tb.EmployeeBasicInfoId equals eb.Id
                              join bujl in context.BusinessUnitJobLevels on eb.BusinessUnitJobLevelId equals bujl.Id
                              join jt in context.JobTitles on bujl.JobTitleId equals jt.Id
                              join jg in context.JobGrades on bujl.JobGradeId equals jg.Id
                              join bu in context.BusinessUnits on bujl.BusinessUnitDivisionId equals bu.Id
                              where tb.IsDeleted.Equals(false)
                              select new TrainingRequisitionDetail
                              {
                                  Id = tb.Id,
                                  EmployeeBasicInfoId = tb.EmployeeBasicInfoId,
                                  EmployeeBasicInfoName = eb.NameEmployee,
                                  JobTitleId = bujl.JobTitleId,
                                  JobTitleName = jt.TitleName,
                                  JobGradeId = bujl.JobGradeId,
                                  JobGradeName = jg.GradeName,
                                  DivisionId = bujl.BusinessUnitDivisionId==null?"":bujl.BusinessUnitDivisionId,
                                  DivisionName = bu.UnitName==null?"":bu.UnitName
                              }).ToListAsync();

            return model;
        }

        public async Task<List<TrainingRequisition>> GetTrainingRequestList()
        {
            var trn = await (from tb in context.TrainingRequisition
                             join tr in context.Training on tb.TrainingId equals tr.Id
                             join eb in context.EmployeeBasicInfos on tb.RequesterEmployeeBasicInfoId equals eb.Id
                             join bu in context.BusinessUnits on tb.BusinessUnitId equals bu.Id into buj
                             from bulj in buj.DefaultIfEmpty()
                             where tb.IsDeleted.Equals(false)
                             orderby tb.Code descending
                             select new TrainingRequisition
                             {
                                 Id = tb.Id,
                                 TrainingName = tr.Name,
                                 TrainingType = tr.TrainingType,
                                 BusinessUnitId = tb.BusinessUnitId,
                                 BusinessUnitName = bulj.UnitName,
                                 TrainingId = tb.TrainingId,
                                 Code = tb.Code,
                                 AddRecomendation = tb.AddRecomendation,
                                 Notes = tb.Notes,
                                 ParticipantList = context.TrainingRequisitionDetail.Where(x=> x.IsDeleted.Equals(false) && x.TrainingRequisitionId == tb.Id).ToList(),
                                 Status = tb.Status,
                                 Venue = tb.Venue,
                                 PurposeEndDate = tb.PurposeEndDate,
                                 PurposeStartDate = tb.PurposeStartDate,
                                 RequesterEmployeeBasicInfoId = tb.RequesterEmployeeBasicInfoId,
                                 RequesterEmployeeBasicInfoName = eb.NameEmployee
                             }).ToListAsync();
            return trn;
        }

        public async Task<TrainingRequisitionDTO> AddWithDetail(TrainingRequisition Header, List<TrainingRequisitionDetail> Details)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                TrainingRequisitionDTO traning = new TrainingRequisitionDTO();
                try
                { 
                    var trainingSetting = context.Training.Where(x => x.IsDeleted.Equals(false) && x.Id == Header.TrainingId).FirstOrDefault();
                    if (trainingSetting == null)
                    {
                        throw new Exception("Training Setting Not Valid");
                    }
                    else
                    {
                        Header.Id = Guid.NewGuid().ToString();
                        Header.Code = await TransID.GetTransId(context, Code.TrainingRequest, DateTime.Today);
                       
                        if (trainingSetting.TrainingCost > TrainingStatus.TrainingBondingContraint)
                        {
                          
                            if (Details.Count > 0)
                            {
                                List<TrainingBonding> nbdn = new List<TrainingBonding>();
                                foreach (TrainingRequisitionDetail detail in Details)
                                {
                                    TrainingBonding bonding = new TrainingBonding();
                                    bonding.Id = Guid.NewGuid().ToString();
                                    bonding.TrainingRequisitionId = Header.Id;
                                    bonding.StartDate = Header.PurposeStartDate;
                                    bonding.EndDate = Header.PurposeEndDate;
                                    bonding.EmployeeBasicInfoId = detail.EmployeeBasicInfoId;
                                    bonding.BusinessUnitId = Header.BusinessUnitId;
                                    bonding.Status = StatusTransaction.WaitingApproval.ToString();
                                    bonding.Code = await TransID.GetTransId(context, Code.TrainingBonding, DateTime.Today);
                                    nbdn.Add(bonding);
                                   
                                }
                                TrainingBonding[] andbn = nbdn.ToArray();
                                await context.PhoenixAddRangeAsync(andbn);
                               
                            }

                         
                            await context.PhoenixAddAsync(Header);

                            Header.Status = TrainingStatus.WaitingBondingApproval;

                            await globalFcApproval.UnsafeSubmitApproval<TrainingRequisition>(false, Header.Id, int.Parse(Header.Status), $"{ApprovalLink.TrainingRequisition}?Id={Header.Id}&isApprove=true", idTemplate, MenuUnique.TrainingRequisition, Header, NotificationModuleType.ModuleHRIS, NotificationTypeCode.TrainingrequisitionNotification);
                            //add details
                            if (Details.Count > 0)
                            {
                                List<TrainingRequisitionDetail> newdetail = new List<TrainingRequisitionDetail>();

                                foreach (TrainingRequisitionDetail rdtl in Details)
                                {
                                    TrainingRequisitionDetail newdetailinner = new TrainingRequisitionDetail();
                                    newdetailinner.Id = Guid.NewGuid().ToString();
                                    newdetailinner.TrainingRequisitionId = Header.Id;
                                    newdetailinner.EmployeeBasicInfoId = rdtl.EmployeeBasicInfoId;
                                    newdetail.Add(newdetailinner);
                                    var emailParticipant = await context.EmployeeBasicInfos.Where(x => x.Id == newdetailinner.EmployeeBasicInfoId).Select(x => x.Email).FirstOrDefaultAsync();
                                    await globalFcApproval.AddNotification(NotificationTypeCode.TrainingrequisitionParticipant, emailParticipant, NotificationModuleType.ModuleHRIS, newdetailinner.TrainingRequisitionId, true, true, context.GetEmployeeId());
                                }
                                TrainingRequisitionDetail[] rnewdetail = newdetail.ToArray();
                                await context.PhoenixAddRangeAsync(rnewdetail);
                            }

                        } else {

                            //if training cost < paramters
                            Header.Status = TrainingStatus.WaitingApproval;
                            
                            //add header with approval
                            await globalFcApproval.UnsafeSubmitApproval<TrainingRequisition>(false, Header.Id, int.Parse(Header.Status), $"{ApprovalLink.TrainingRequisition}?Id={Header.Id}&isApprove=true", idTemplate, MenuUnique.TrainingRequisition, Header, NotificationModuleType.ModuleHRIS, NotificationTypeCode.TrainingrequisitionNotification);
                           
                            //add details
                            if (Details.Count > 0)
                            {
                                List<TrainingRequisitionDetail> newdetail = new List<TrainingRequisitionDetail>();

                                foreach (TrainingRequisitionDetail rdtl in Details)
                                {
                                    TrainingRequisitionDetail newdetailinner = new TrainingRequisitionDetail();
                                    newdetailinner.Id = Guid.NewGuid().ToString();
                                    newdetailinner.TrainingRequisitionId = Header.Id;
                                    newdetailinner.EmployeeBasicInfoId = rdtl.EmployeeBasicInfoId;
                                    newdetail.Add(newdetailinner);
                                    var emailParticipant = await context.EmployeeBasicInfos.Where(x => x.Id == newdetailinner.EmployeeBasicInfoId).Select(x => x.Email).FirstOrDefaultAsync();
                                    await globalFcApproval.AddNotification(NotificationTypeCode.TrainingrequisitionParticipant, emailParticipant, NotificationModuleType.ModuleHRIS, newdetailinner.TrainingRequisitionId, true, true, context.GetEmployeeId());
                                }
                                TrainingRequisitionDetail[] rnewdetail = newdetail.ToArray();
                                await context.PhoenixAddRangeAsync(rnewdetail);
                            }

                        }
                        //var isApprove = true;
                        
                        //var dataMenu = await menuService.GetByUniqueName(MenuUnique.TrainingRequisition).ConfigureAwait(false);
                        //if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
                        //var dataTempate = context.TmTemplateApprovals.Where(x => x.MenuId == dataMenu.Id && x.IsDeleted == false).FirstOrDefault();
                        //idTemplate = dataTempate.Id;
                        //var vm = new Models.ViewModel.TransApprovalHrisVm()
                        //{
                        //    MenuId = dataMenu.Id,
                        //    RefId = Header.Id,
                        //    DetailLink = $"{ApprovalLink.TrainingRequisition}?Id={Header.Id}&isApprove=true",
                        //    IdTemplate = idTemplate,
                        //    Module = NotificationModuleType.ModuleHRIS,
                        //    NotificationType = NotificationTypeCode.TrainingrequisitionNotification
                        //};



                        //var subGroupId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessSubGroup(context.HttpContext);
                        //var divisionId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessUnitDivisi(context.HttpContext);
                        //var employeeId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(context.HttpContext);
                        //isApprove = await globalFcApproval.Save(vm, subGroupId, divisionId, employeeId);

                        //if (isApprove)
                        //{
                        //    var save = await context.SaveChangesAsync();
                        //    transaction.Commit();
                        //    //return save;
                        //}
                        //else
                        //{
                        //    transaction.Rollback();
                        //    throw new Exception("please check the approval template that will be processed");
                        //}


                        var save = await context.SaveChangesAsync();
                        transaction.Commit();
                        traning.Header = Header;
                        traning.Details = Details;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message.ToString());
                }
                return traning;
            }
        }

        public async Task<TrainingRequisitionDTO> EditWithDetail(TrainingRequisition Header, List<TrainingRequisitionDetail> Details)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                TrainingRequisitionDTO training = new TrainingRequisitionDTO();
                try
                {

                    context.PhoenixEdit(Header);
                    if (Details != null)
                    {

                        var additional = await context.TrainingRequisitionDetail.AsNoTracking().Where(x => x.TrainingRequisitionId == Header.Id).ToListAsync();
                        if (additional.Count > 0)
                        {
                            context.TrainingRequisitionDetail.RemoveRange(additional);
                        }

                        List<TrainingRequisitionDetail> dt = new List<TrainingRequisitionDetail>();
                        foreach (TrainingRequisitionDetail data in Details)
                        {
                            data.Id = Guid.NewGuid().ToString();
                            data.TrainingRequisitionId = Header.Id;
                            dt.Add(data);
                        }

                        TrainingRequisitionDetail[] dtg = dt.ToArray();
                        await context.PhoenixAddRangeAsync(dtg);
           
                    }

                    var trapprovalmodel = await context.TrTemplateApprovals.AsNoTracking().Where(x => x.RefId == Header.Id).FirstOrDefaultAsync();
                    if (trapprovalmodel == null)
                    {
                        var isApprove = true;
                        var dataMenu = await menuService.GetByUniqueName(MenuUnique.TrainingRequisition).ConfigureAwait(false);
                        if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
                        var vm = new Models.ViewModel.TransApprovalHrisVm()
                        {
                            MenuId = dataMenu.Id,
                            RefId = Header.Id,
                            DetailLink = $"{ApprovalLink.TrainingRequisition}?Id={Header.Id}&isApprove=true",
                            //Tname = "fn.PurchaseRequest",//"SP#NAMA SP NYA",
                            IdTemplate = idTemplate,
                            NotificationType = NotificationTypeCode.TrainingrequisitionNotification,
                            Module = NotificationModuleType.ModuleHRIS
                        };
                        var subGroupId = ClainJwtPrincipalHelper.GetBusinessSubGroup(context.HttpContext);
                        var divisionId = ClainJwtPrincipalHelper.GetBusinessUnitDivisi(context.HttpContext);
                        var employeeId = ClainJwtPrincipalHelper.GetEmployeeId(context.HttpContext);
                        isApprove = await globalFcApproval.Save(vm, subGroupId, divisionId, employeeId);


                        if (isApprove)
                        {
                            var save = await context.SaveChangesAsync();
                            transaction.Commit();
                            training.Header = Header;
                            training.Details = Details;
                            return training;
                        }
                        else
                        {
                            transaction.Rollback();
                            throw new Exception("please check the approval template that will be processed");
                        }

                    }
                    else
                    {
                        var save = await context.SaveChangesAsync();
                        transaction.Commit();
                        training.Header = Header;
                        training.Details = Details;
                        return training;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message.ToString());
                }
            }
        }

        public async Task<TrainingRequisition> Approve(TrainingRequisition entity)
        {
            try
            {
                entity.Status = TrainingStatus.Approved;
                await globalFcApproval.ProcessApproval<TrainingRequisition>(entity.Id, Int32.Parse(entity.Status), entity.RemarkRejected, entity);

                var modeltrapp = await context.TrTemplateApprovals.Where(x => x.RefId == entity.Id).FirstOrDefaultAsync();
                if (modeltrapp != null)
                {
                    if (modeltrapp.StatusApprovedDescription == StatusTransaction.StatusName(StatusTransaction.Approved))
                    {
                        entity.Status = TrainingStatus.Approved;
             
                    }
                    else
                    {
                        entity.Status = TrainingStatus.WaitingApproval;
                    }
                }
                else
                {
                    entity.Status = TrainingStatus.WaitingApproval;
                }

                context.PhoenixEdit(entity);
                var save = await context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

        }

        public async Task<TrainingRequisition> Reject(TrainingRequisition entity)
        {
            try
            {
                entity.Status = TrainingStatus.Reject;
                await globalFcApproval.ProcessApproval<TrainingRequisition>(entity.Id, Int32.Parse(entity.Status), entity.RemarkRejected, entity);

                var modeltrapp = await context.TrTemplateApprovals.AsNoTracking().Where(x => x.RefId == entity.Id).FirstOrDefaultAsync();
                if (modeltrapp != null)
                {
                    if (modeltrapp.StatusApprovedDescription == StatusTransaction.StatusName(StatusTransaction.Rejected))
                    {
                        entity.Status = PaymentRequestStatus.Reject;
                        entity.Notes = entity.RemarkRejected;
                    }
                }
                else
                {
                    entity.Status = PaymentRequestStatus.Reject;
                }

                context.PhoenixEdit(entity);
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