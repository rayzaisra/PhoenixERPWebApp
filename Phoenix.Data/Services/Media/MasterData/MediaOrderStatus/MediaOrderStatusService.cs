using Phoenix.Data.Models.Media.MasterData.MediaOrderStatus;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Media.MasterData.MediaOrderStatus
{
    public interface IMediaOrderStatusService : IDataService<TmMediaOrderStatus>
    {
        Task<dynamic> GetList();
    }

    public class MediaOrderStatusService : IMediaOrderStatusService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage MasterMappingRequest
        /// </summary>
        /// <param name="context">Database context</param>
        public MediaOrderStatusService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(TmMediaOrderStatus entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params TmMediaOrderStatus[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(TmMediaOrderStatus entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params TmMediaOrderStatus[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(TmMediaOrderStatus entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params TmMediaOrderStatus[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(TmMediaOrderStatus entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(TmMediaOrderStatus entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<TmMediaOrderStatus>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<dynamic> GetList()
        {
            var sql = await (from a in context.TmMediaOrderStatuses
                             where a.IsDeleted != true
                             select new
                             {
                                 a.Id,
                                 a.MediaOrderStatusName
                             }
                            ).ToListAsync();
            return sql;
        }

        public async Task<TmMediaOrderStatus> Get(string Id) => await context.TmMediaOrderStatuses.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).AsNoTracking().FirstOrDefaultAsync();

        Task<List<TmMediaOrderStatus>> IDataService<TmMediaOrderStatus>.Get()
        {
            throw new NotImplementedException();
        }
    }
}
