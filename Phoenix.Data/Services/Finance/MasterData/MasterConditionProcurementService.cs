using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Finance
{
    public interface IMasterConditionProcurementService : IDataService<MasterConditionProcurement>
    {

    }

    public class MasterConditionProcurementService : IMasterConditionProcurementService
    {
        readonly DataContext context;

        public MasterConditionProcurementService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(MasterConditionProcurement entity)
        {
            entity.Id = await TransID.GetTransId(context, Code.MasterConditionProcurement, DateTime.Today);
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params MasterConditionProcurement[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(MasterConditionProcurement entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params MasterConditionProcurement[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(MasterConditionProcurement entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params MasterConditionProcurement[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(MasterConditionProcurement entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(MasterConditionProcurement entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<MasterConditionProcurement>> Get()
        {
            var query = await context.MasterConditionProcurement.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).OrderBy(x => x.CreatedOn).ToListAsync();
            return query;
        }

        public Task<List<MasterConditionProcurement>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<MasterConditionProcurement> Get(string Id)
        {
            var query = await context.MasterConditionProcurement.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
            return query;
        }
    }
}
