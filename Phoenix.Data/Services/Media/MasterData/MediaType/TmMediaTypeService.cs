using Phoenix.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Phoenix.Data.Services.Media
{
    public interface ITmMediaTypeService : IDataService<TmMediaType>
    {
    }

    public class TmMediaServiceService : ITmMediaTypeService
    {
        readonly DataContext context;

        public TmMediaServiceService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(TmMediaType entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params TmMediaType[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(TmMediaType entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params TmMediaType[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(TmMediaType entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params TmMediaType[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(TmMediaType entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(TmMediaType entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<TmMediaType>> Get() => await context.MediaTypes.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).ToListAsync();

        public Task<List<TmMediaType>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<TmMediaType> Get(string id) => await context.MediaTypes.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == id).AsNoTracking().FirstOrDefaultAsync();
    }
}
