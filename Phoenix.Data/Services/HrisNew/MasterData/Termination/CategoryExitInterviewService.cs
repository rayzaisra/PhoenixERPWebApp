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
    public interface ICategoryExitInterviewService : IDataService<CategoryExitInterview>
    {
        int GetAutoNumber();
    }

    public class CategoryExitInterviewService : ICategoryExitInterviewService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage CategoryExitInterview
        /// </summary>
        /// <param name="context">Database context</param>
        public CategoryExitInterviewService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(CategoryExitInterview entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params CategoryExitInterview[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(CategoryExitInterview entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params CategoryExitInterview[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(CategoryExitInterview entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params CategoryExitInterview[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(CategoryExitInterview entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(CategoryExitInterview entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CategoryExitInterview>> Get() => await context.CategoryExitInterviews.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).OrderBy(x => x.No).ToListAsync();

        public Task<List<CategoryExitInterview>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<CategoryExitInterview> Get(string Id) => await context.CategoryExitInterviews.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();

        public int GetAutoNumber()
        {
            return (context.CategoryExitInterviews.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).Max(x => x.No) ?? 0) + 1;
        }

    }
}