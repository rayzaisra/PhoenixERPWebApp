using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models;
using Phoenix.Data.RestApi;
using Phoenix.Shared.Core.Contexts;

namespace Phoenix.Data
{
    public interface IPdrSalaryIncreaseService : IDataService<PdrSalaryIncrease>
    {
    }

    public class PdrSalaryIncreaseService : IPdrSalaryIncreaseService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage PdrSalaryIncrease
        /// </summary>
        /// <param name="context">Database context</param>
        public PdrSalaryIncreaseService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(PdrSalaryIncrease entity)
        {
            entity.Id = Guid.NewGuid().ToString();

            await context.PhoenixAddAsync(entity);
            foreach (var item in entity.PdrSalaryGrade)
            {
                PdrSalaryGrade salaryGrade = new PdrSalaryGrade()
                {
                    Id = Guid.NewGuid().ToString(),
                    JobGradeId = item,
                    PdrSalaryIncreaseId = entity.Id
                };
                await context.PhoenixAddAsync(salaryGrade);
            }
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params PdrSalaryIncrease[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(PdrSalaryIncrease entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params PdrSalaryIncrease[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(PdrSalaryIncrease entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params PdrSalaryIncrease[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(PdrSalaryIncrease entity)
        {
            context.PhoenixEdit(entity);
            context.PdrSalaryGrades.RemoveRange(context.PdrSalaryGrades.AsNoTracking().Where(x => x.PdrSalaryIncreaseId == entity.Id).ToList());
            foreach (var item in entity.PdrSalaryGrade)
            {
                PdrSalaryGrade salaryGrade = new PdrSalaryGrade()
                {
                    Id = Guid.NewGuid().ToString(),
                    JobGradeId = item,
                    PdrSalaryIncreaseId = entity.Id,
                    CreatedBy = entity.CreatedBy,
                    CreatedOn = entity.CreatedOn
                };
                await context.PhoenixAddAsync(salaryGrade);
            }
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(PdrSalaryIncrease entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PdrSalaryIncrease>> Get()
        {
            var JobGrades = await (from c in context.PdrSalaryGrades
                                   join d in context.JobGrades on c.JobGradeId equals d.Id
                                   select new { c, d }).ToListAsync();

            var result = await (from a in context.PdrSalaryIncreases
                                join b in context.PdrGrades on a.PdrGradeId equals b.Id
                                where !(a.IsDeleted ?? false)
                                select new PdrSalaryIncrease
                                {
                                    Id = a.Id,
                                    PdrGradeId = a.PdrGradeId,
                                    SalaryIncrease = a.SalaryIncrease,
                                    PdrGrade = b.Result,
                                    PdrSalaryGradeName = string.Join(", ", JobGrades.Where(x => x.c.PdrSalaryIncreaseId == a.Id).OrderBy(x => x.c.CreatedOn).Select(x => x.d.GradeName)),
                                    CreatedOn = a.CreatedOn
                                }).OrderBy(x => x.CreatedOn).ToListAsync();
            return result;
        }

        public Task<List<PdrSalaryIncrease>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<PdrSalaryIncrease> Get(string Id)
        {
            var result = await (from a in context.PdrSalaryIncreases
                                join b in context.PdrGrades on a.PdrGradeId equals b.Id
                                where !(a.IsDeleted ?? false) && a.Id == Id
                                select new PdrSalaryIncrease
                                {
                                    Id = a.Id,
                                    PdrGradeId = a.PdrGradeId,
                                    SalaryIncrease = a.SalaryIncrease,
                                    PdrGrade = b.Result,
                                    PdrSalaryGrade = context.PdrSalaryGrades.Where(x => x.PdrSalaryIncreaseId == a.Id).OrderBy(x => x.CreatedOn).Select(x => x.JobGradeId).ToList()
                                }).FirstOrDefaultAsync();
            return result;
        }
    }
}