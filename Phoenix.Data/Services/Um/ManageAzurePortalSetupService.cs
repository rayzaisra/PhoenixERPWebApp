using Microsoft.EntityFrameworkCore;
using Phoenix.ApiExtension.Helpers;
using Phoenix.Data.Models.Um;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Um
{
    public class ManageAzurePortalSetupService : IManageAzurePortalSetupService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage SalaryComponent
        /// </summary>
        /// <param name="context">Database context</param>
        public ManageAzurePortalSetupService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(AzurePortalSetup entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params AzurePortalSetup[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(AzurePortalSetup entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params AzurePortalSetup[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(AzurePortalSetup entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params AzurePortalSetup[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(AzurePortalSetup entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(AzurePortalSetup entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<AzurePortalSetup>> Get() => await context.AzurePortalSetups.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).ToListAsync();

        public Task<List<AzurePortalSetup>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<AzurePortalSetup> Get(string Id)
        {
            var data = await (from x in context.AzurePortalSetups.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id)
                              select new AzurePortalSetup
                              {
                                  Id = x.Id,
                                  TenantId = x.TenantId,
                                  ClientId = x.ClientId,
                                  EmailAccount = x.EmailAccount,
                                  SecurityPword = SecurityHelper.DecryptString(x.SecurityPword)
                              }).FirstOrDefaultAsync();
            return data;
        }
        public async Task<AzurePortalSetup> GetOnlyOne()
        {
            var data = await (from x in context.AzurePortalSetups.AsNoTracking().Where(x => !(x.IsDeleted ?? false))
                              select new AzurePortalSetup
                              {
                                  Id = x.Id,
                                  TenantId = x.TenantId,
                                  ClientId = x.ClientId,
                                  EmailAccount = x.EmailAccount,
                                  SecurityPword = SecurityHelper.DecryptString(x.SecurityPword)
                              }).Take(1).FirstOrDefaultAsync();
            return data;
        }
    }
}
