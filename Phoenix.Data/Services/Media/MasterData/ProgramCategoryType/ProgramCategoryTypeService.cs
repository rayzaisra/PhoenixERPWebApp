using Phoenix.Data.Models.Media;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Phoenix.Data.Services.Media
{
    public interface IProgramCategoryTypeService : IDataService<TmProgramCategoryType>
    {
    }
    public class ProgramCategoryTypeService : IProgramCategoryTypeService
    {
        readonly DataContext context;

        public ProgramCategoryTypeService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(TmProgramCategoryType entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params TmProgramCategoryType[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(TmProgramCategoryType entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params TmProgramCategoryType[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(TmProgramCategoryType entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params TmProgramCategoryType[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(TmProgramCategoryType entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(TmProgramCategoryType entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<TmProgramCategoryType>> Get() => await context.TmProgramCategoryTypes.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).ToListAsync();

        public Task<List<TmProgramCategoryType>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<TmProgramCategoryType> Get(string id) => await context.TmProgramCategoryTypes.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == id).AsNoTracking().FirstOrDefaultAsync();
    }
}
