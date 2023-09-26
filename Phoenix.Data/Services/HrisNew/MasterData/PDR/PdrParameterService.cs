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
    public interface IPdrParameterService : IDataService<PdrParameter>
    {
    }

    public class PdrParameterService : IPdrParameterService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage PdrParameter
        /// </summary>
        /// <param name="context">Database context</param>
        public PdrParameterService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(PdrParameter entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params PdrParameter[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(PdrParameter entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params PdrParameter[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(PdrParameter entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params PdrParameter[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(PdrParameter entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(PdrParameter entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PdrParameter>> Get()
        {
            var result = await (from a in context.PdrParameters
                                join b in context.PdrCategorys on a.PdrCategoryId equals b.Id
                                join c in context.JobGrades on a.JobGradeId equals c.Id
                                where !(a.IsDeleted ?? false)
                                orderby b.No ascending, a.Alphabet ascending
                                select new PdrParameter
                                {
                                    Id = a.Id,
                                    PdrCategoryId = a.PdrCategoryId,
                                    Alphabet = a.Alphabet,
                                    Subject = a.Subject,
                                    Description = a.Description,
                                    KeyBehavior1 = a.KeyBehavior1,
                                    KeyBehavior2 = a.KeyBehavior2,
                                    KeyBehavior3 = a.KeyBehavior3,
                                    KeyBehavior4 = a.KeyBehavior4,
                                    KeyBehavior5 = a.KeyBehavior5,
                                    JobGradeId = a.JobGradeId,
                                    Grade = c.GradeName,
                                    PdrCategoryName = b.Name,
                                    CreatedOn = a.CreatedOn
                                }
                               ).ToListAsync();
            return result;
        }

        public Task<List<PdrParameter>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<PdrParameter> Get(string Id) => await context.PdrParameters.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
    }
}