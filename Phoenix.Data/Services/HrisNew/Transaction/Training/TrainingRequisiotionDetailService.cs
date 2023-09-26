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

namespace Phoenix.Data
{
    public interface ITrainingRequisitionDetailService : IDataServiceHris<TrainingRequisitionDetail>
    {
        Task<List<TrainingRequisitionDetail>> GetTrainingRequisitionDetailsWithTrainingRequestId(string Id);
        Task<List<TrainingRequisitionDetail>> GetEmployeeListWithFilter(string GroupId,string SubgroupId,string DivisionId,string DepartementId);
    }

    public class TrainingRequisitionDetailService : ITrainingRequisitionDetailService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage Training Requisiotion
        /// </summary>
        /// <param name="context">Database context</param>
        public TrainingRequisitionDetailService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(TrainingRequisitionDetail entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> EditAsync(TrainingRequisitionDetail entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }


        public async Task<int> DeleteSoftAsync(TrainingRequisitionDetail entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<List<TrainingRequisitionDetail>> Get()
        {
            return await context.TrainingRequisitionDetail.Where(x => x.IsDeleted.Equals(false)).ToListAsync();
        }

        public async Task<TrainingRequisitionDetail> Get(string Id)
        {
            return await context.TrainingRequisitionDetail.Where(x => x.IsDeleted.Equals(false) && x.Id==Id).FirstOrDefaultAsync();
        }

        public Task<int> ApproveAsync(TrainingRequisitionDetail entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<TrainingRequisitionDetail>> GetTrainingRequisitionDetailsWithTrainingRequestId(string Id)
        {
            var detail = await (from tb in context.TrainingRequisitionDetail
                                join eb in context.EmployeeBasicInfos on tb.EmployeeBasicInfoId equals eb.Id
                                join bujl in context.BusinessUnitJobLevels on eb.BusinessUnitJobLevelId equals bujl.Id
                                join gr in context.JobGrades on bujl.JobGradeId equals gr.Id
                                join ttl in context.JobTitles on bujl.JobTitleId equals ttl.Id
                                join bu in context.BusinessUnits on bujl.BusinessUnitDivisionId equals bu.Id                              
                                where tb.IsDeleted.Equals(false) && tb.TrainingRequisitionId == Id
                                select new TrainingRequisitionDetail
                                {
                                    Id = tb.Id,
                                    TrainingRequisitionId = tb.TrainingRequisitionId,
                                    EmployeeBasicInfoId = tb.EmployeeBasicInfoId,
                                    EmployeeBasicInfoName = eb.NameEmployee,
                                    JobGradeId = bujl.JobGradeId,
                                    JobTitleId = bujl.JobTitleId,
                                    JobGradeName = gr.GradeName,
                                    JobTitleName = ttl.TitleName,
                                    DivisionId = bujl.BusinessUnitDivisionId == null?"":bujl.BusinessUnitDivisionId,
                                    DivisionName = bu.UnitName == null?"":bu.UnitName                                
                                }).ToListAsync();

            return detail;
        }

        public async Task<List<TrainingRequisitionDetail>> GetEmployeeListWithFilter(string GroupId, string SubgroupId, string DivisionId, string DepartementId)
        {
            var employee = await (from tb in context.EmployeeBasicInfos
                                  join bujl in context.BusinessUnitJobLevels on tb.BusinessUnitJobLevelId equals bujl.Id
                                  join jt in context.JobTitles on bujl.JobTitleId equals jt.Id
                                  join gr in context.JobGrades on bujl.JobGradeId equals gr.Id
                                  join bu in context.BusinessUnits on bujl.BusinessUnitDivisionId equals bu.Id
                                  where tb.IsDeleted.Equals(false)
                                  select new TrainingRequisitionDetail
                                  {
                                      Id = tb.Id,
                                      EmployeeBasicInfoId = tb.Id,
                                      EmployeeBasicInfoName = tb.NameEmployee,
                                      JobGradeId = bujl.JobGradeId,
                                      JobTitleId = bujl.JobTitleId,
                                      JobGradeName = gr.GradeName,
                                      JobTitleName = jt.TitleName,
                                      DivisionId = bujl.BusinessUnitDivisionId == null ? "" : bujl.BusinessUnitDivisionId,
                                      DivisionName = bu.UnitName == null ? "" : bu.UnitName,
                                      GroupId = bujl.BusinessUnitGroupId == null ? "" : bujl.BusinessUnitGroupId,
                                      SubgroupId = bujl.BusinessUnitSubgroupId == null ? "" : bujl.BusinessUnitSubgroupId,
                                      DepartementId = bujl.BusinessUnitDepartementId == null ? "" : bujl.BusinessUnitDepartementId
                                  }).ToListAsync();
            List<TrainingRequisitionDetail> detail = new List<TrainingRequisitionDetail>();
            detail = employee;
            if (GroupId != "0") {
                detail = employee.Where(x => x.GroupId == GroupId).ToList();
            }
            if (SubgroupId != "0")
            {
                detail = employee.Where(x => x.GroupId == GroupId && x.SubgroupId == SubgroupId ).ToList();
            }
            if (DivisionId != "0")
            {
                detail = employee.Where(x => x.GroupId == GroupId && x.SubgroupId == SubgroupId && x.DivisionId == DivisionId).ToList();
            }
            if (DepartementId != "0")
            {
                detail = employee.Where(x => x.GroupId == GroupId && x.SubgroupId == SubgroupId && x.DivisionId == DivisionId && x.DepartementId == DepartementId).ToList();
            }

            return detail;
        }
    }
}