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
    public interface IExitInterviewItemService : IDataService<ExitInterviewItem>
    {
        int GetAutoNumber(string categoryExitInterviewId);
    }

    public class ExitInterviewItemService : IExitInterviewItemService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage ExitInterviewItem
        /// </summary>
        /// <param name="context">Database context</param>
        public ExitInterviewItemService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(ExitInterviewItem entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params ExitInterviewItem[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(ExitInterviewItem entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params ExitInterviewItem[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(ExitInterviewItem entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params ExitInterviewItem[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(ExitInterviewItem entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(ExitInterviewItem entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ExitInterviewItem>> Get() {

            //return await context.ExitInterviewItems.Include(x => x.CategoryExitInterview).AsNoTracking().Where(x => !(x.IsDeleted ?? false)).OrderBy(x => x.CategoryExitInterview.No).ThenBy(x => x.No).ToListAsync();
            var que = await (from tb1 in context.ExitInterviewItems
                             join tb2 in context.CategoryExitInterviews on tb1.CategoryExitInterviewId equals tb2.Id
                             where !(tb1.IsDeleted ?? false)
                             orderby tb2.No
                             select new ExitInterviewItem()
                             {
                                 No = tb1.No,
                                 Id = tb1.Id,
                                 Type = tb1.Type,
                                 TextItem = tb1.TextItem,
                                 CategoryExitInterviewId = tb1.CategoryExitInterviewId,
                                 BusinessUnitDivisionId = tb1.BusinessUnitDivisionId,
                                 BusinessUnitDepartementId = tb1.BusinessUnitDepartementId,
                                 CategoryExitInterview = new CategoryExitInterview()
                                 {
                                     Name = tb2.Name,
                                     No = tb2.No,
                                     Id = tb2.Id

                                 }
                             }
                             ).ToListAsync();
            return que;
        } 

        public Task<List<ExitInterviewItem>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<ExitInterviewItem> Get(string Id) => await context.ExitInterviewItems.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();

        public int GetAutoNumber(string categoryExitInterviewId)
        {
            return (context.ExitInterviewItems.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.CategoryExitInterviewId == categoryExitInterviewId).Max(x => x.No) ?? 0) + 1;
        }
    }
}