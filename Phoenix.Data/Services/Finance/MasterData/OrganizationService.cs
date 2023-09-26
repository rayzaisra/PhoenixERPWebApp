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
    public interface IOrganizationService : IDataService<Organization>
    {
    }

    public class OrganizationService : IOrganizationService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage Organization
        /// </summary>
        /// <param name="context">Database context</param>
        public OrganizationService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(Organization entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params Organization[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(Organization entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params Organization[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(Organization entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params Organization[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(Organization entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(Organization entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Organization>> Get() => await context.Organizations.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).ToListAsync();

        public Task<List<Organization>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<Organization> Get(string Id) => await context.Organizations.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
    }
}