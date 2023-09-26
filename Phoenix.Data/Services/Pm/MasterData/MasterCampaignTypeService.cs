using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Pm.MasterData;
using Phoenix.Data.RestApi;
using Phoenix.Shared.Core.Contexts;

namespace Phoenix.Data.Services.Pm.MasterData
{
    public interface IMasterCampaignTypeService : IDataService<CampaignType>
    {
    }
    public class MasterCampaignTypeService : IMasterCampaignTypeService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage MasterOutlet
        /// </summary>
        /// <param name="context">Database context</param>
        public MasterCampaignTypeService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(CampaignType entity)
        {
            entity.IsActive = true;
            entity.IsDeleted = false;
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params CampaignType[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(CampaignType entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params CampaignType[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(CampaignType entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params CampaignType[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(CampaignType entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(CampaignType entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CampaignType>> Get()
        {
            var result = await (from a in context.CampaignType
                                where !(a.IsDeleted ?? false)
                                select new CampaignType
                                {
                                    Id = a.Id,
                                    campaignType = a.campaignType,
                                    IsDefault = a.IsDefault,
                                    IsLocked = a.IsLocked,
                                    description = a.description,
                                    BusinessUnitDivisionId = a.BusinessUnitDivisionId
                                }).ToListAsync();
            return result;
        }

        public Task<List<CampaignType>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<CampaignType> Get(string id) => await context.CampaignType.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == id).FirstOrDefaultAsync();
    }
}
