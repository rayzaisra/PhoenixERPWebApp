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
    public interface IPdrGradeService : IDataService<PdrGrade>
    {
    }

    public class PdrGradeService : IPdrGradeService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage PdrGrade
        /// </summary>
        /// <param name="context">Database context</param>
        public PdrGradeService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(PdrGrade entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params PdrGrade[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(PdrGrade entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params PdrGrade[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(PdrGrade entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params PdrGrade[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(PdrGrade entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(PdrGrade entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PdrGrade>> Get() => await context.PdrGrades.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).OrderBy(x => x.CreatedOn).ToListAsync();

        public Task<List<PdrGrade>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<PdrGrade> Get(string Id) => await context.PdrGrades.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
    }
}