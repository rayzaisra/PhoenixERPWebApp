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
    public interface IInstitutionService : IDataService<Institution>
    {
    }

    public class InstitutionService : IInstitutionService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage Institution
        /// </summary>
        /// <param name="context">Database context</param>
        public InstitutionService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(Institution entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params Institution[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(Institution entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params Institution[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(Institution entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params Institution[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(Institution entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(Institution entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Institution>> Get() => await context.Institutions.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).ToListAsync();

        public Task<List<Institution>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<Institution> Get(string Id) => await context.Institutions.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
    }
}