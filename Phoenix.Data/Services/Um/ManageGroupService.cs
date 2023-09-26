using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models.Um;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Um
{

    public class ManageGroupService : IManageGroupService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage SalaryComponent
        /// </summary>
        /// <param name="context">Database context</param>
        public ManageGroupService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(TmGroup entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params TmGroup[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(TmGroup entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params TmGroup[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(TmGroup entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params TmGroup[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(TmGroup entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(TmGroup entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<TmGroup>> GetBySearch(string key) => await context.TmGroups.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.GroupName.Contains(key)).ToListAsync();
        public async Task<List<TmGroup>> Get() => await context.TmGroups.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).ToListAsync();

        public Task<List<TmGroup>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<TmGroup> Get(string Id) => await context.TmGroups.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).AsNoTracking().FirstOrDefaultAsync();
    }
}
