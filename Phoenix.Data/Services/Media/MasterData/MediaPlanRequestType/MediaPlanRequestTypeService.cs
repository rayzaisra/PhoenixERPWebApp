using Phoenix.Data.Models.Media;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Phoenix.Data.Services.Media
{
    public interface IMediaPlanRequestTypeService : IDataService<TmMediaPlanRequestType>
    {
    }
    public class MediaPlanRequestTypeService : IMediaPlanRequestTypeService
    {
        readonly DataContext context;

        public MediaPlanRequestTypeService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(TmMediaPlanRequestType entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params TmMediaPlanRequestType[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(TmMediaPlanRequestType entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params TmMediaPlanRequestType[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(TmMediaPlanRequestType entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params TmMediaPlanRequestType[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(TmMediaPlanRequestType entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(TmMediaPlanRequestType entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<TmMediaPlanRequestType>> Get() => await context.TmMediaPlanRequestTypes.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).ToListAsync();

        public Task<List<TmMediaPlanRequestType>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<TmMediaPlanRequestType> Get(string id) => await context.TmMediaPlanRequestTypes.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == id).AsNoTracking().FirstOrDefaultAsync();
    }
}
