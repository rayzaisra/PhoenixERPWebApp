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
    public interface ILegalEntityService : IDataService<LegalEntity>
    {
    }

    public class LegalEntityService : ILegalEntityService
    {
        readonly DataContext context;
        readonly FileService uploadFile;

        /// <summary>
        /// And endpoint to manage LegalEntity
        /// </summary>
        /// <param name="context">Database context</param>
        public LegalEntityService(DataContext context, FileService uploadFile)
        {
            this.context = context;
            this.uploadFile = uploadFile;
        }

        public async Task<int> AddAsync(LegalEntity entity)
        {
            var model = await ProcessUpload(entity);
            model.RegistrationCode = await TransID.GetTransId(context, Code.MasterClient, DateTime.Today);
            model.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(model);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params LegalEntity[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(LegalEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params LegalEntity[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(LegalEntity entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params LegalEntity[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(LegalEntity entity)
        {
            var model = await ProcessUpload(entity);
            if (string.IsNullOrEmpty(model.RegistrationCode)) model.RegistrationCode = await TransID.GetTransId(context, Code.MasterClient, DateTime.Today);
            context.PhoenixEdit(model);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(LegalEntity entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<LegalEntity>> Get()
        {
            var result = await (from a in context.LegalEntity
                                join b in context.Countrys on a.OfficeCountryId equals b.Id
                                join c in context.Provinces on a.OfficeProvinceId equals c.Id
                                join d in context.Citys on a.OfficeCityId equals d.Id
                                where !(a.IsDeleted ?? false)
                                select new LegalEntity
                                {
                                    Id = a.Id,
                                    RegistrationCode = a.RegistrationCode,
                                    LegalEntityName = a.LegalEntityName,
                                    OfficeAddress = a.OfficeAddress,
                                    OfficeCountryName = b.CountryName,
                                    OfficeProvinceName = c.ProvinceName,
                                    OfficeCityName = d.CityName,
                                    OfficeZipCode = a.OfficeZipCode,
                                    ContactName = a.ContactName,
                                    CreatedOn = a.CreatedOn
                                }).ToListAsync();
            return result;
        }

        public Task<List<LegalEntity>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<LegalEntity> Get(string Id)
        {
            var result = await context.LegalEntity.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
            return await GetFile(result);
        }


        private async Task<LegalEntity> ProcessUpload(LegalEntity entity)
        {
            var file1 = await uploadFile.Upload(entity.fileImageLegalScan, null);
            if (!string.IsNullOrEmpty(file1))
            {
                entity.ImageLegalScan = file1;
            }

            var file2 = await uploadFile.Upload(entity.fileImageTaxScan, null);
            if (!string.IsNullOrEmpty(file2))
            {
                entity.ImageTaxScan = file2;
            }

            return entity;
        }


        private async Task<LegalEntity> GetFile(LegalEntity data)
        {
            if (data.ImageLegalScan != null)
            {
                var legal = await context.Filemasters.Where(x => x.Id == data.ImageLegalScan).FirstOrDefaultAsync();
                if (legal != null) data.ImageLegalScanName = legal.Name;
            }

            if (data.ImageTaxScan != null)
            {
                var tax = await context.Filemasters.Where(x => x.Id == data.ImageTaxScan).FirstOrDefaultAsync();
                if (tax != null) data.ImageTaxScanName = tax.Name;
            }
            return data;
        }
    }
}