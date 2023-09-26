using Phoenix.Data.Models.Media.MasterData.MediaPlanStatus;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Media.MasterData.MediaPlanStatus
{
    public interface IMediaPlanStatusService : IDataService<TmMediaPlanStatus>
    {
        Task<dynamic> GetList();
    }

    public class MediaPlanStatusService : IMediaPlanStatusService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage MasterMappingRequest
        /// </summary>
        /// <param name="context">Database context</param>
        public MediaPlanStatusService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(TmMediaPlanStatus entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params TmMediaPlanStatus[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(TmMediaPlanStatus entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params TmMediaPlanStatus[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(TmMediaPlanStatus entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params TmMediaPlanStatus[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(TmMediaPlanStatus entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(TmMediaPlanStatus entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<TmMediaPlanStatus>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<dynamic> GetList()
        {
            var sql = await (from a in context.TmMediaPlanStatuses
                             where a.IsDeleted != true
                             select new
                             {
                                 a.Id,
                                 a.MediaPlanStatusName
                             }
                            ).ToListAsync();
            return sql;
        }

        public async Task<TmMediaPlanStatus> Get(string Id) => await context.TmMediaPlanStatuses.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).AsNoTracking().FirstOrDefaultAsync();

        Task<List<TmMediaPlanStatus>> IDataService<TmMediaPlanStatus>.Get()
        {
            throw new NotImplementedException();
        }
    }
}
