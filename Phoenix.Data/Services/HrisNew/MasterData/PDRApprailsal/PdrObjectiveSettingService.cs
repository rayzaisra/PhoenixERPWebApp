using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models;
using Phoenix.Data.RestApi;
using Phoenix.Shared.Core.Contexts;

namespace Phoenix.Data
{
    public interface IPdrObjectiveSettingService : IDataService<PdrObjectiveSetting>
    {
    }

    public class PdrObjectiveSettingService : IPdrObjectiveSettingService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage PdrObjectiveSetting
        /// </summary>
        /// <param name="context">Database context</param>
        public PdrObjectiveSettingService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(PdrObjectiveSetting entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params PdrObjectiveSetting[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(PdrObjectiveSetting entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params PdrObjectiveSetting[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(PdrObjectiveSetting entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params PdrObjectiveSetting[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(PdrObjectiveSetting entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(PdrObjectiveSetting entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PdrObjectiveSetting>> Get()
        {
            var result = await (from a in context.PdrObjectiveSettings
                                join b in context.BscCorporateObjectives on a.BscCorporateObjectiveId equals b.Id
                                join c in context.JobGrades on a.JobGradeId equals c.Id into lj
                                from ljac in lj.DefaultIfEmpty()
                                where !(a.IsDeleted ?? false) && !(b.IsDeleted ?? false)
                                select new PdrObjectiveSetting
                                {
                                    Id = a.Id,
                                    CorporateObjective = a.CorporateObjective,
                                    ObjectiveItem = a.ObjectiveItem,
                                    BscCorporateObjectiveId = a.BscCorporateObjectiveId,
                                    BscCorporateObjectiveName = b.BscName,
                                    CreatedOn = a.CreatedOn,
                                    JobGradeId = a.JobGradeId,
                                    JobGradeName = ljac.GradeName
                                }).ToListAsync();

            var result2 = await context.PdrObjectiveSettings.Where(x => !(x.IsDeleted ?? false) && !result.Any(z => z.Id == x.Id)).ToListAsync();
            foreach (var item in result2)
            {
                item.JobGradeName = await context.JobGrades.Where(x => x.Id == item.JobGradeId).Select(x => x.GradeName).FirstOrDefaultAsync();
            }
            if (result == null) result = new List<PdrObjectiveSetting>();
            result.AddRange(result2);
            return result;
        }

        public Task<List<PdrObjectiveSetting>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<PdrObjectiveSetting> Get(string Id)
        {
            var result = await context.PdrObjectiveSettings.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
            result.JobGradeName = await context.JobGrades.Where(x => x.Id == result.JobGradeId).Select(x => x.GradeName).FirstOrDefaultAsync();
            return result;
        }
    }
}