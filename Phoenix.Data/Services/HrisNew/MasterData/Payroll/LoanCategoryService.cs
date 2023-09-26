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
    public interface ILoanCategoryService : IDataService<LoanCategory>
    {
    }

    public class LoanCategoryService : ILoanCategoryService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage LoanCategory
        /// </summary>
        /// <param name="context">Database context</param>
        public LoanCategoryService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(LoanCategory entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params LoanCategory[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(LoanCategory entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params LoanCategory[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(LoanCategory entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params LoanCategory[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(LoanCategory entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(LoanCategory entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<LoanCategory>> Get() {
            var query = await context.LoanCategorys.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).OrderBy(x => x.CreatedOn).ToListAsync();

            foreach (var item in query)
            {
                if (item.is_Corporate_Loan == true)
                {
                    item.CorporateLoanTypeDesc = "Company";
                }
                else
                {
                    item.CorporateLoanTypeDesc = "Koperasi";
                }
            }
            return query;
        }
            
            
            

        public Task<List<LoanCategory>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<LoanCategory> Get(string Id) => await context.LoanCategorys.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
    }
}