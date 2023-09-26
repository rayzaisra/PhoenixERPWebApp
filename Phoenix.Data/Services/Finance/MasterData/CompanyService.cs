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
    public interface ICompanyService
    {
        //----------------------------Client-------------------------- 
        Task<int> ClientAddAsync(Company entity);
        Task<int> ClientDeleteSoftAsync(Company entity);
        Task<int> ClientEditAsync(Company entity);
        Task<List<Company>> ClientGet();
        Task<Company> ClientGet(string Id);

        //----------------------------Vendor-------------------------- 
        Task<int> VendorAddAsync(Company entity);
        Task<int> VendorDeleteSoftAsync(Company entity);
        Task<int> VendorEditAsync(Company entity);
        Task<List<Company>> VendorGet();
        Task<Company> VendorGet(string Id);
        Task<Company> ValidationAliasName(string name);
    }

    public class CompanyService : ICompanyService
    {
        readonly DataContext context;
        readonly FileService uploadFile;

        /// <summary>
        /// And endpoint to manage Company
        /// </summary>
        /// <param name="context">Database context</param>
        public CompanyService(DataContext context, FileService uploadFile)
        {
            this.context = context;
            this.uploadFile = uploadFile;
        }

        //----------------------------Client-------------------------- 
        public async Task<int> ClientAddAsync(Company entity)
        {
            var model = await ProcessUpload(entity);
            model.RegistrationCode = await TransID.GetTransId(context, Code.MasterClient, DateTime.Today);
            model.IsClient = true;
            //model.IsVendor = false;
            model.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(model);
            return await context.SaveChangesAsync();
        }

        public async Task<int> ClientDeleteSoftAsync(Company entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> ClientEditAsync(Company entity)
        {
            var model = await ProcessUpload(entity);
            if (string.IsNullOrEmpty(model.RegistrationCode)) model.RegistrationCode = await TransID.GetTransId(context, Code.MasterClient, DateTime.Today);
            model.IsClient = true;
            //model.IsVendor = false;
            context.PhoenixEdit(model);
            return await context.SaveChangesAsync();
        }

        public async Task<List<Company>> ClientGet()
        {
            var result = await (from a in context.Companys
                                join b in context.Countrys on a.OfficeCountryId equals b.Id
                                join c in context.Provinces on a.OfficeProvinceId equals c.Id
                                join d in context.Citys on a.OfficeCityId equals d.Id
                                where !(a.IsDeleted ?? false) && (a.IsClient ?? false)
                                select new Company
                                {
                                    Id = a.Id,
                                    RegistrationCode = a.RegistrationCode,
                                    CompanyName = a.CompanyName,
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

        public async Task<Company> ClientGet(string Id)
        {
            var result = await context.Companys.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && (x.IsClient ?? false) && x.Id == Id).FirstOrDefaultAsync();
            return await GetFile(result);
        }

        //----------------------------Vendor-------------------------- 
        public async Task<int> VendorAddAsync(Company entity)
        {
            var model = await ProcessUpload(entity);
            model.RegistrationCode = await TransID.GetTransId(context, Code.MasterVendor, DateTime.Today);
            //model.IsClient = false;
            model.IsVendor = true;
            model.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(model);
            return await context.SaveChangesAsync();
        }

        public async Task<int> VendorDeleteSoftAsync(Company entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> VendorEditAsync(Company entity)
        {
            var model = await ProcessUpload(entity);
            if (string.IsNullOrEmpty(model.RegistrationCode)) model.RegistrationCode = await TransID.GetTransId(context, Code.MasterVendor, DateTime.Today);
            //model.IsClient = false;
            model.IsVendor = true;
            context.PhoenixEdit(model);
            return await context.SaveChangesAsync();
        }

        public async Task<List<Company>> VendorGet()
        {
            var result = await (from a in context.Companys
                                join b in context.Countrys on a.OfficeCountryId equals b.Id into bj
                                from bleftj in bj.DefaultIfEmpty()
                                join c in context.Provinces on a.OfficeProvinceId equals c.Id into cj
                                from cleftj in cj.DefaultIfEmpty()
                                join d in context.Citys on a.OfficeCityId equals d.Id into dj
                                from dleftj in dj.DefaultIfEmpty()
                                where !(a.IsDeleted ?? false) && (a.IsVendor ?? false)
                                orderby a.CompanyName
                                select new Company
                                {
                                    Id = a.Id,
                                    RegistrationCode = a.RegistrationCode,
                                    CompanyName = a.CompanyName,
                                    OfficeAddress = a.OfficeAddress,
                                    OfficeCountryName = bleftj.CountryName,
                                    OfficeProvinceName = cleftj.ProvinceName,
                                    OfficeCityName = dleftj.CityName,
                                    OfficeZipCode = a.OfficeZipCode,
                                    ContactName = a.ContactName,
                                    CreatedOn = a.CreatedOn,
                                    CompanyAliasName = a.CompanyAliasName
                                }).ToListAsync();
            return result;
        }

        public async Task<Company> VendorGet(string Id)
        {
            var result = await context.Companys.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && (x.IsVendor ?? false) && x.Id == Id).FirstOrDefaultAsync();
            return await GetFile(result);
        }

        public async Task<Company> ValidationAliasName(string name)
        {
            var result = await context.Companys.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && (x.IsVendor ?? false) && x.CompanyAliasName == name).FirstOrDefaultAsync();

            return result;
        }

        private async Task<Company> ProcessUpload(Company entity)
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

        private async Task<Company> GetFile(Company data)
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