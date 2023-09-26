using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models.Um;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Um
{
    public class AuditTrailsService : IAuditTrailsService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage SalaryComponent
        /// </summary>
        /// <param name="context">Database context</param>
        public AuditTrailsService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(AuditTrails entity)
        {
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params AuditTrails[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(AuditTrails entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params AuditTrails[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(AuditTrails entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params AuditTrails[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(AuditTrails entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(AuditTrails entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<AuditTrails>> GetBySearch(string key)
        {
            var data = await (from u in context.AuditTrails.AsNoTracking()
                              .Where(x => x.TABLE_NAME.Contains(key) || x.NEW_VALUE.Contains(key) || x.OLD_VALUE.Contains(key))
                              select new AuditTrails
                              {
                                  ID = u.ID,
                                  ACT_TYPE = u.ACT_TYPE,
                                  FIELD_NAME = u.FIELD_NAME,
                                  TABLE_NAME = u.TABLE_NAME,
                                  NEW_VALUE = u.NEW_VALUE,
                                  OLD_VALUE = u.OLD_VALUE,
                                  DB_USER_NAME = u.DB_USER_NAME,
                                  UPDATE_BY = u.UPDATE_BY,
                                  UPDATE_DATE = u.UPDATE_DATE
                              }
                              ).ToListAsync();
            return data;
        }
        public async Task<List<AuditTrails>> Get() => await context.AuditTrails.AsNoTracking().ToListAsync();

        public Task<List<AuditTrails>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<AuditTrails> Get(string Id) => await context.AuditTrails.AsNoTracking().Where(x => x.ID == Convert.ToInt32(Id)).AsNoTracking().FirstOrDefaultAsync();
    }
}
