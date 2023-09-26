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
    public interface ITrainingBondingService : IDataServiceHris<TrainingBonding>
    {
        Task<List<TrainingBonding>> GetTrainingBondingRequestList();
        Task<TrainingBonding> Approve(string Id);
        Task<TrainingBonding> Reject(string Id, string remarksRejected);
    }

    public class TrainingBondingService : ITrainingBondingService
    {
        readonly DataContext context;
        private readonly Services.Um.IManageUserAppService userApp;
        /// <summary>
        /// And endpoint to manage Training Requisiotion
        /// </summary>
        /// <param name="context">Database context</param>
        public TrainingBondingService(DataContext context, Services.Um.IManageUserAppService userApp)
        {
            this.context = context;
            this.userApp = userApp;
        }

        public async Task<int> AddAsync(TrainingBonding entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> EditAsync(TrainingBonding entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }


        public async Task<int> DeleteSoftAsync(TrainingBonding entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<List<TrainingBonding>> Get()
        {
            return await context.TrainingBonding.Where(x => x.IsDeleted.Equals(false)).ToListAsync();
        }

        public async Task<TrainingBonding> Get(string Id)
        {
            var trn = await (from tb in context.TrainingBonding
                             join trq in context.TrainingRequisition on tb.TrainingRequisitionId equals trq.Id
                             join tr in context.Training on trq.TrainingId equals tr.Id
                             join eb in context.EmployeeBasicInfos on tb.EmployeeBasicInfoId equals eb.Id
                             join bujl in context.BusinessUnitJobLevels on eb.BusinessUnitJobLevelId equals bujl.Id
                             join gr in context.JobGrades on bujl.JobGradeId equals gr.Id
                             join bu in context.BusinessUnits on tb.BusinessUnitId equals bu.Id
                             where tb.IsDeleted.Equals(false) && tb.Id == Id
                             select new TrainingBonding
                             {
                                 Id = tb.Id,
                                 TrainingName = tr.Name,
                                 TrainingType = tr.TrainingType,
                                 BusinessUnitId = tb.BusinessUnitId,
                                 BusinessUnitName = bu.UnitName,
                                 TrainingRequisitionId = tb.TrainingRequisitionId,
                                 Status = tb.Status,
                                 StartDate = tb.StartDate,
                                 EndDate = tb.EndDate,
                                 EmployeeBasicInfoId = tb.EmployeeBasicInfoId,
                                 EmployeeBasicInfoName = eb.NameEmployee,
                                 Code = tb.Code,
                                 GradeName = gr.GradeName,
                                 Venue = trq.Venue
                             }).FirstOrDefaultAsync();
            return trn;
        }


        public async Task<TrainingBonding> Approve(string Id)
        {
            TrainingBonding bonding = new TrainingBonding();
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
               
                var trainingBonding = await context.TrainingBonding.FindAsync(Id);

                if (trainingBonding != null)
                {
                    var trainingrequisition = await context.TrainingRequisition.FindAsync(trainingBonding.TrainingRequisitionId);
                    if (trainingrequisition == null) {
                        throw new Exception("TrainingBonding not found");
                    }
                    trainingrequisition.Status = StatusTransaction.WaitingApproval.ToString();
                    trainingBonding.Status = StatusTransaction.Approved.ToString();
                    bonding = trainingBonding;
                    var save = await context.SaveChangesAsync();
                    transaction.Commit();
                }
                else
                {
                    throw new Exception("TrainingBonding not found");
                }

            }
            return bonding;
        }

        public async Task<TrainingBonding> Reject(string Id,string remarksRejected)
        {
            TrainingBonding bonding = new TrainingBonding();
            using (var transaction = await context.Database.BeginTransactionAsync())
            {

                var trainingBonding = await context.TrainingBonding.FindAsync(Id);

                if (trainingBonding != null)
                {
                    var trainingrequisition = await context.TrainingRequisition.FindAsync(trainingBonding.TrainingRequisitionId);

                    if (trainingrequisition == null)
                    {
                        throw new Exception("TrainingBonding not found");
                    }
                    else
                    {

                        trainingrequisition.Status = StatusTransaction.Rejected.ToString();
                        trainingBonding.Status = StatusTransaction.Rejected.ToString();
                        trainingrequisition.RemarkRejected = remarksRejected;
                    }
                    bonding = trainingBonding;
                    var save = await context.SaveChangesAsync();
                    transaction.Commit();
                }
                else
                {
                    throw new Exception("TrainingBonding not found");
                }

            }
            return bonding;
        }

        public async Task<List<TrainingBonding>> GetTrainingBondingRequestList()
        {
            var group = await userApp.GetByUserLogin();
            //var HCAdmin = group.Where(x => x.GroupId == GroupAcess.HCAdmin).Count() > 0;
            //var result = new List<TrainingBonding>();
            //if (HCAdmin)
            //{
            //    result = await (from tb in context.TrainingBonding
            //                    join trq in context.TrainingRequisition on tb.TrainingRequisitionId equals trq.Id
            //                    join tr in context.Training on trq.TrainingId equals tr.Id
            //                    join eb in context.EmployeeBasicInfos on tb.EmployeeBasicInfoId equals eb.Id
            //                    join bu in context.BusinessUnits on tb.BusinessUnitId equals bu.Id
            //                    where tb.IsDeleted.Equals(false)
            //                    select new TrainingBonding
            //                    {
            //                        Id = tb.Id,
            //                        TrainingName = tr.Name,
            //                        TrainingType = tr.TrainingType,
            //                        BusinessUnitId = tb.BusinessUnitId,
            //                        BusinessUnitName = bu.UnitName,
            //                        TrainingRequisitionId = tb.TrainingRequisitionId,
            //                        Status = tb.Status,
            //                        StartDate = tb.StartDate,
            //                        EndDate = tb.EndDate,
            //                        EmployeeBasicInfoId = tb.EmployeeBasicInfoId,
            //                        EmployeeBasicInfoName = eb.NameEmployee,
            //                        Code = tb.Code,
            //                        TrainingRequestCode = trq.Code
            //                    }).ToListAsync();
            //}

            var result = new List<TrainingBonding>();

            result = await (from tb in context.TrainingBonding
                            join trq in context.TrainingRequisition on tb.TrainingRequisitionId equals trq.Id
                            join tr in context.Training on trq.TrainingId equals tr.Id
                            join eb in context.EmployeeBasicInfos on tb.EmployeeBasicInfoId equals eb.Id
                            join bu in context.BusinessUnits on tb.BusinessUnitId equals bu.Id
                            where tb.IsDeleted.Equals(false)
                            select new TrainingBonding
                            {
                                Id = tb.Id,
                                TrainingName = tr.Name,
                                TrainingType = tr.TrainingType,
                                BusinessUnitId = tb.BusinessUnitId,
                                BusinessUnitName = bu.UnitName,
                                TrainingRequisitionId = tb.TrainingRequisitionId,
                                Status = tb.Status,
                                StartDate = tb.StartDate,
                                EndDate = tb.EndDate,
                                EmployeeBasicInfoId = tb.EmployeeBasicInfoId,
                                EmployeeBasicInfoName = eb.NameEmployee,
                                Code = tb.Code,
                                TrainingRequestCode = trq.Code
                            }).ToListAsync();

            return result;
        }

        public Task<int> ApproveAsync(TrainingBonding entity)
        {
            throw new NotImplementedException();
        }
    }
}