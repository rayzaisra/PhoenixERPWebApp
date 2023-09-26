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
    public interface IInsuranceCompanyService : IDataService<InsuranceCompany>
    {
    }

    public class InsuranceCompanyService : IInsuranceCompanyService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage InsuranceCompany
        /// </summary>
        /// <param name="context">Database context</param>
        public InsuranceCompanyService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(InsuranceCompany entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params InsuranceCompany[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(InsuranceCompany entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params InsuranceCompany[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(InsuranceCompany entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params InsuranceCompany[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(InsuranceCompany entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(InsuranceCompany entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<InsuranceCompany>> Get() => await context.InsuranceCompanys.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).OrderBy(x => x.CreatedOn).ToListAsync();

        public Task<List<InsuranceCompany>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<InsuranceCompany> Get(string Id) => await context.InsuranceCompanys.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
    }
}