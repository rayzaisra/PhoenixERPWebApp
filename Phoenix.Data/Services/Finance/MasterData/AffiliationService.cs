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
    public interface IAffiliationService : IDataService<Affiliation>
    {
    }

    public class AffiliationService : IAffiliationService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage Affiliation
        /// </summary>
        /// <param name="context">Database context</param>
        public AffiliationService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(Affiliation entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params Affiliation[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(Affiliation entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params Affiliation[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(Affiliation entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params Affiliation[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(Affiliation entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(Affiliation entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Affiliation>> Get() => await context.Affiliations.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).ToListAsync();

        public Task<List<Affiliation>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<Affiliation> Get(string Id) => await context.Affiliations.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
    }
}