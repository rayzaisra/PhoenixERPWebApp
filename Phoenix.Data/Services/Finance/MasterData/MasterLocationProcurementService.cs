using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Data
{
    public interface IMasterLocationProcurementService : IDataService<MasterLocationProcurement>
    {

    }

    public class MasterLocationProcurementService : IMasterLocationProcurementService
    {
        readonly DataContext context;

        public MasterLocationProcurementService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(MasterLocationProcurement entity)
        {
            entity.Id = await TransID.GetTransId(context, Code.MasterLocationProcurement, DateTime.Today);
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params MasterLocationProcurement[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(MasterLocationProcurement entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params MasterLocationProcurement[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(MasterLocationProcurement entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params MasterLocationProcurement[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(MasterLocationProcurement entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(MasterLocationProcurement entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<MasterLocationProcurement>> Get()
        {
            var query = await context.MasterLocationProcurement.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).OrderBy(x => x.CreatedOn).ToListAsync();
            return query;
        }

        public Task<List<MasterLocationProcurement>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<MasterLocationProcurement> Get(string Id)
        {
            var query = await context.MasterLocationProcurement.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
            return query;
        }
    }
}
