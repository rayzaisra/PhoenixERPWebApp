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
using Phoenix.Data.Services.Um;


namespace Phoenix.Data
{
    public interface ITrainingShareSessionService : IDataServiceHris<TrainingShareSession>
    {
        Task<List<TrainingShareSession>> GetTrainingShareSessionList();
        Task<TrainingShareSession> GetTrainingCompletedById(string Id);

    }
    public class TrainingShareSessionService : ITrainingShareSessionService
    {
        readonly DataContext context;
        readonly GlobalFunctionApproval globalFcApproval;
        readonly IManageMenuService menuService;
        string idTemplate = "";

        public TrainingShareSessionService(DataContext context, GlobalFunctionApproval globalFcApproval, IManageMenuService menuService)
        {
            this.context = context;
            this.globalFcApproval = globalFcApproval;
            this.menuService = menuService;
        }


        public async Task<int> AddAsync(TrainingShareSession entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> ApproveAsync(TrainingShareSession entity)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(TrainingShareSession entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> EditAsync(TrainingShareSession entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<List<TrainingShareSession>> Get()
        {
            throw new NotImplementedException();
        }

        public async Task<TrainingShareSession> Get(string id)
        {
            var trn = await (from ts in context.TrainingShareSession
                             join trs in context.TrainingRequisition on ts.TrainingRequisitionId equals trs.Id
                             join tr in context.Training on trs.TrainingId equals tr.Id
                             where ts.Id == id
                             select new TrainingShareSession
                             {
                                 Id = ts.Id,
                                 TrainingRequisitionId = trs.Id,
                                 TrainingId = tr.Id,
                                 TrainingName = tr.Name,
                                 TrainingType = tr.TrainingType,
                                 StartDate = ts.StartDate,
                                 EndDate = ts.EndDate,
                                 PurposeStartDate = trs.PurposeStartDate,
                                 PurposeEndDate = trs.PurposeEndDate,
                                 //trainingShareSession = await GetTrainingCompletedById(trs.Id)
                             }).FirstOrDefaultAsync();
            return trn;
        }

        public async Task<TrainingShareSession> GetTrainingCompletedById(string Id)
        {
            var trn = await (from ts in context.TrainingRequisition
                             join tr in context.Training on ts.TrainingId equals tr.Id
                             where ts.Id == Id && ts.Status == "5"
                             select new TrainingShareSession
                             {
                                 Id = ts.Id,
                                 TrainingId = tr.Id,
                                 TrainingRequisitionId = ts.Id,
                                 TrainingName = tr.Name,
                                 TrainingType = tr.TrainingType,
                                 PurposeStartDate = ts.PurposeStartDate,
                                 PurposeEndDate = ts.PurposeEndDate,
                                 TrainerEmployeeBasicInfoId = ts.RequesterEmployeeBasicInfoId
                             }).FirstOrDefaultAsync();
            return trn;
        }

        public async Task<List<TrainingShareSession>> GetTrainingShareSessionList()
        {
            var trn = await (from ts in context.TrainingShareSession
                             join trs in context.TrainingRequisition on ts.TrainingRequisitionId equals trs.Id
                             join tr in context.Training on trs.TrainingId equals tr.Id
                             join eb in context.EmployeeBasicInfos on ts.EmployeeBasicInfoId equals eb.Id
                             // where tb.IsDeleted.Equals(false) && tb.Status == "5"
                             select new TrainingShareSession
                             {
                                 Id = ts.Id,
                                 TrainingId = tr.Id,
                                 TrainingRequisitionId = ts.TrainingRequisitionId,
                                 TrainingName = tr.Name,
                                 TrainingType = tr.TrainingType,
                                 StartDate = ts.StartDate,
                                 EndDate = ts.EndDate,
                                 PurposeStartDate = trs.PurposeStartDate,
                                 PurposeEndDate = trs.PurposeEndDate,
                                 TrainerEmployeeBasicInfoName = eb.NameEmployee
                             }).ToListAsync();
            return trn;
        }
    }
}
