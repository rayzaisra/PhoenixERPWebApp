using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Finance
{
    public interface IMasterDepreciationService : IDataService<MasterDepreciation>
    {

    }

    public class MasterDepreciationService : IMasterDepreciationService
    {
        readonly DataContext context;

        public MasterDepreciationService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(MasterDepreciation entity)
        {
            entity.Id = await TransID.GetTransId(context, Code.MasterDepreciation, DateTime.Today);
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params MasterDepreciation[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(MasterDepreciation entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params MasterDepreciation[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(MasterDepreciation entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params MasterDepreciation[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(MasterDepreciation entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(MasterDepreciation entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<MasterDepreciation>> Get()
        {
            var query = await context.MasterDepreciation.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).OrderBy(x => x.CreatedOn).ToListAsync();
            return query;
        }

        public Task<List<MasterDepreciation>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<MasterDepreciation> Get(string Id)
        {
            var query = await context.MasterDepreciation.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
            return query;
        }
    }
}
