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
    public interface ILegalDocumentMasterCounterpartService : IDataService<LegalDocumentMasterCounterpart>
    {
        Task<dynamic> GetList();
    }

    public class LegalDocumentMasterCounterpartService : ILegalDocumentMasterCounterpartService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage LegalDocumentMasterCounterpart
        /// </summary>
        /// <param name="context">Database context</param>
        public LegalDocumentMasterCounterpartService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(LegalDocumentMasterCounterpart entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params LegalDocumentMasterCounterpart[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(LegalDocumentMasterCounterpart entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params LegalDocumentMasterCounterpart[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(LegalDocumentMasterCounterpart entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params LegalDocumentMasterCounterpart[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(LegalDocumentMasterCounterpart entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(LegalDocumentMasterCounterpart entity)
        {
            throw new NotImplementedException();
        }

        public async Task<dynamic> GetList()
        {
            var result = await (from a in context.LegalDocumentMasterCounterparts
                                join b in context.LegalCategorys on a.LegalCategoryId equals b.Id
                                join c in context.EmployeeBasicInfos on a.EmployeeBasicInfoId equals c.Id
                                where !(a.IsDeleted ?? false)
                                select new
                                {
                                    a.Id,
                                    LegalCategory = b.Category,
                                    FullName = c.NameEmployee,
                                    a.CreatedOn
                                }).OrderByDescending(x => x.CreatedOn).ToListAsync();
            return result;
        }

        public Task<List<LegalDocumentMasterCounterpart>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<LegalDocumentMasterCounterpart> Get(string Id) => await context.LegalDocumentMasterCounterparts.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();

        public Task<List<LegalDocumentMasterCounterpart>> Get()
        {
            throw new NotImplementedException();
        }
    }
}