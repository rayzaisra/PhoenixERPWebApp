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
    public interface IBscCorporateObjectiveService : IDataService<BscCorporateObjective>
    {
    }

    public class BscCorporateObjectiveService : IBscCorporateObjectiveService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage BscCorporateObjective
        /// </summary>
        /// <param name="context">Database context</param>
        public BscCorporateObjectiveService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(BscCorporateObjective entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params BscCorporateObjective[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(BscCorporateObjective entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params BscCorporateObjective[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(BscCorporateObjective entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params BscCorporateObjective[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(BscCorporateObjective entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(BscCorporateObjective entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<BscCorporateObjective>> Get()
        {
            var result = await (from a in context.BscCorporateObjectives
                                join b in context.BscCorporateObjectives on a.Id equals b.ParentId
                                where !(a.IsDeleted ?? false) && !(b.IsDeleted ?? false)
                                select new BscCorporateObjective
                                {
                                    Id = a.Id,
                                    BscCategory = a.BscCategory,
                                    BscName = a.BscName,
                                    ParentId = a.ParentId,
                                    Year = a.Year,
                                    ParentName = b.BscName,
                                    CreatedOn = a.CreatedOn
                                }).ToListAsync();

            var result2 = await context.BscCorporateObjectives.Where(x => !(x.IsDeleted ?? false) && !result.Any(z => z.Id == x.Id)).ToListAsync();
            if (result == null) result = new List<BscCorporateObjective>();
            result.AddRange(result2);
            return result;
        }

        public Task<List<BscCorporateObjective>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<BscCorporateObjective> Get(string Id) => await context.BscCorporateObjectives.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
    }
}