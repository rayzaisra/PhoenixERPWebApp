using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phoenix.ApiExtension.Extensions;
using Phoenix.Data.Models;
using Phoenix.Data.RestApi;
using Phoenix.Shared.Core.Contexts;

namespace Phoenix.Data
{
    public interface IBusinessUnitJobLevelService : IDataService<BusinessUnitJobLevel>
    {
		Task<List<BusinessUnitJobLevel>> GetDdl();
		Task<List<BusinessUnitJobLevel>> GetDdlJobTitleVacant();
		Task<List<BusinessUnitJobLevel>> GetDdlJobTitleNonVacant();
		Task<BusinessUnitJobLevel> GetDetail(string id);
	}

    public class BusinessUnitJobLevelService : IBusinessUnitJobLevelService
	{
        readonly DataContext context;
        /// <summary>
        /// And endpoint to manage Termination
        /// </summary>
        /// <param name="context">Database context</param>
        public BusinessUnitJobLevelService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(BusinessUnitJobLevel entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }
		
		public Task<int> AddRangeAsync(params BusinessUnitJobLevel[] entities)
        {
			context.PhoenixAddRange(entities);
			return context.SaveChangesAsync();
		}

        public Task<int> DeleteAsync(BusinessUnitJobLevel entity)
        {
			context.Remove(entity);
			return context.SaveChangesAsync();
		}

        public Task<int> DeleteRageAsync(params BusinessUnitJobLevel[] entities)
        {
			context.RemoveRange(entities);
			return context.SaveChangesAsync();
		}

        public async Task<int> DeleteSoftAsync(BusinessUnitJobLevel entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params BusinessUnitJobLevel[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(BusinessUnitJobLevel entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(BusinessUnitJobLevel entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<BusinessUnitJobLevel>> Get() => await context.BusinessUnitJobLevels.AsNoTracking().Where(x => x.IsDeleted != true).ToListAsync();

        public Task<List<BusinessUnitJobLevel>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<BusinessUnitJobLevel> Get(string Id) => await context.BusinessUnitJobLevels.AsNoTracking().Where(x => x.IsDeleted != true && x.Id == Id).FirstOrDefaultAsync();

		public async Task<List<BusinessUnitJobLevel>> GetDdl() {
			var que = await (
				from tb1 in context.BusinessUnitJobLevels
				join tb2 in context.JobTitles on tb1.JobTitleId equals tb2.Id
				join tb3 in context.JobLevels on tb1.JobLevelId equals tb3.Id
				join tb4 in context.JobGrades on tb1.JobGradeId equals tb4.Id
				join tb5 in context.EmployeeBasicInfos on tb1.Id equals tb5.BusinessUnitJobLevelId into tb5j
				from tb5lj in tb5j.DefaultIfEmpty()
				where tb1.IsDeleted != true
				orderby tb2.TitleName ascending
				select new BusinessUnitJobLevel
				{
					Remarks = tb1.Remarks,
					Id = tb1.Id,
					Text = tb2.TitleName,
					BusinessUnitGroup = (from tbu in context.BusinessUnits where tbu.Id == tb1.BusinessUnitGroupId select tbu.UnitName).FirstOrDefault(),
					BusinessUnitSubgroup = (from tbu in context.BusinessUnits where tbu.Id == tb1.BusinessUnitSubgroupId select tbu.UnitName).FirstOrDefault(),
					BusinessUnitDivision = (from tbu in context.BusinessUnits where tbu.Id == tb1.BusinessUnitDivisionId select tbu.UnitName).FirstOrDefault(),
					BusinessUnitDepartement = (from tbu in context.BusinessUnits where tbu.Id == tb1.BusinessUnitDepartementId select tbu.UnitName).FirstOrDefault(),
					JobGradeName = tb4.GradeName,
					JobLevel = tb3.JobName,
					Supervisor = (from tba in context.BusinessUnitJobLevels
								  join tbb in context.JobTitles on tba.JobTitleId equals tbb.Id
								  where tba.Id == tb1.ParentId
								  select tbb.TitleName).FirstOrDefault(),
					UserName = tb5lj.NameEmployee ?? "Vacant"
				}).ToListAsync();

			return que;
		}

		public async Task<List<BusinessUnitJobLevel>> GetDdlJobTitleVacant()
		{
			var que = await GetDdl();

			que = que.Where(x => x.UserName == "Vacant").ToList();
			return que;
		}

		public async Task<List<BusinessUnitJobLevel>> GetDdlJobTitleNonVacant()
		{
			var que = await GetDdl();

			que = que.Where(x => x.UserName != "Vacant").ToList();
			return que;
		}

		public async Task<BusinessUnitJobLevel> GetDetail(string id)
		{
			var que = await (
				from tb1 in context.BusinessUnitJobLevels
				join tb2 in context.JobTitles on tb1.JobTitleId equals tb2.Id
				join tb3 in context.JobLevels on tb1.JobLevelId equals tb3.Id
				join tb4 in context.JobGrades on tb1.JobGradeId equals tb4.Id
				where tb1.Id == id
				orderby tb2.TitleName ascending
				select new BusinessUnitJobLevel
				{
					Remarks = tb1.Remarks,
					Id = tb1.Id,
					JobGradeId = tb1.JobGradeId,
					JobLevelId = tb1.JobLevelId,
					JobTitleId = tb1.JobTitleId,
					ParentId = tb1.ParentId,
					BusinessUnitGroupId = tb1.BusinessUnitGroupId,
					BusinessUnitSubgroupId = tb1.BusinessUnitSubgroupId,
					BusinessUnitDivisionId = tb1.BusinessUnitDivisionId,
					BusinessUnitDepartementId = tb1.BusinessUnitDepartementId,
					BusinessUnitGroup = (from tbu in context.BusinessUnits where tbu.Id == tb1.BusinessUnitGroupId select tbu.UnitName).FirstOrDefault(),
					BusinessUnitSubgroup = (from tbu in context.BusinessUnits where tbu.Id == tb1.BusinessUnitSubgroupId select tbu.UnitName).FirstOrDefault(),
					BusinessUnitDivision = (from tbu in context.BusinessUnits where tbu.Id == tb1.BusinessUnitDivisionId select tbu.UnitName).FirstOrDefault(),
					BusinessUnitDepartement = (from tbu in context.BusinessUnits where tbu.Id == tb1.BusinessUnitDepartementId select tbu.UnitName).FirstOrDefault(),
					JobGradeName = tb4.GradeName,
					JobLevel = tb3.JobName,
					Supervisor = (from tba in context.BusinessUnitJobLevels
								  join tbb in context.JobTitles on tba.JobTitleId equals tbb.Id
								  where tba.Id == tb1.ParentId
								  select tbb.TitleName).FirstOrDefault()
				}).FirstOrDefaultAsync();
			return que;
		}
		
	}
}