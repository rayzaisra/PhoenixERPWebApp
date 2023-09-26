using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models;
using Phoenix.Data.RestApi;
using Phoenix.Shared.Core.Contexts;

namespace Phoenix.Data
{
    public interface IMasterMappingRequestService : IDataService<MasterMappingRequest>
    {
        Task<dynamic> GetList();
    }

    public class MasterMappingRequestService : IMasterMappingRequestService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage MasterMappingRequest
        /// </summary>
        /// <param name="context">Database context</param>
        public MasterMappingRequestService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(MasterMappingRequest entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params MasterMappingRequest[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(MasterMappingRequest entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params MasterMappingRequest[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(MasterMappingRequest entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params MasterMappingRequest[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(MasterMappingRequest entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(MasterMappingRequest entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<MasterMappingRequest>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<dynamic> GetList()
        {
            var sql = await (from a in context.MasterMappingRequests
                             join b in context.MasterDivisionSelfservices on a.MasterDivisionSelfserviceId equals b.Id
                             join c in context.EmployeeBasicInfos on a.EmployeeBasicInfoId equals c.Id
                             where a.IsDeleted != true
                             select new
                             {
                                 a.Id,
                                 a.ServiceName,
                                 b.BusinessUnitName,
                                 c.NameEmployee,
                                 a.Remarks,
                                 a.CreatedOn
                             }
                            ).OrderBy(x => x.CreatedOn).ToListAsync();
            return sql;
        }


        public async Task<MasterMappingRequest> Get(string Id) => await context.MasterMappingRequests.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();

        Task<List<MasterMappingRequest>> IDataService<MasterMappingRequest>.Get()
        {
            throw new NotImplementedException();
        }
    }
}