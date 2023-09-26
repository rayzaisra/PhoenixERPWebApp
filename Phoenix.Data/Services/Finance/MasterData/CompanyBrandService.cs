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
    public interface ICompanyBrandService
    { //----------------------------Client-------------------------- 
        Task<int> ClientAddAsync(CompanyBrand entity);
        Task<int> ClientDeleteSoftAsync(CompanyBrand entity);
        Task<int> ClientEditAsync(CompanyBrand entity);
        Task<List<CompanyBrand>> ClientGet();
        Task<CompanyBrand> ClientGet(string Id);

        //----------------------------Vendor-------------------------- 
        Task<int> VendorAddAsync(CompanyBrand entity);
        Task<int> VendorDeleteSoftAsync(CompanyBrand entity);
        Task<int> VendorEditAsync(CompanyBrand entity);
        Task<List<CompanyBrand>> VendorGet();
        Task<CompanyBrand> VendorGet(string Id);
    }

    public class CompanyBrandService : ICompanyBrandService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage CompanyBrand
        /// </summary>
        /// <param name="context">Database context</param>
        public CompanyBrandService(DataContext context)
        {
            this.context = context;
        }

        //----------------------------Client-------------------------- 
        public async Task<int> ClientAddAsync(CompanyBrand entity)
        {
            entity.RegistrationCode = await TransID.GetTransId(context, Code.MasterClientBrand, DateTime.Today);
            entity.IsClient = true;
            entity.IsVendor = false;
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> ClientDeleteSoftAsync(CompanyBrand entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> ClientEditAsync(CompanyBrand entity)
        {
            if (string.IsNullOrEmpty(entity.RegistrationCode)) entity.RegistrationCode = await TransID.GetTransId(context, Code.MasterClientBrand, DateTime.Today);
            entity.IsClient = true;
            entity.IsVendor = false;
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<List<CompanyBrand>> ClientGet()
        {
            var result = await (from a in context.CompanyBrands
                                join b in context.Companys on a.CompanyId equals b.Id
                                where !(a.IsDeleted ?? false) && (a.IsClient ?? false)
                                select new CompanyBrand
                                {
                                    Id = a.Id,
                                    RegistrationCode = a.RegistrationCode,
                                    BrandName = a.BrandName,
                                    CompanyName = b.CompanyName,
                                    ASFValue = a.ASFValue,
                                    ContactName = a.ContactName,
                                    TermOfPayment = a.TermOfPayment,
                                    CreatedOn = a.CreatedOn
                                }).ToListAsync();
            return result;
        }

        public async Task<CompanyBrand> ClientGet(string Id)
        {
            var result = await context.CompanyBrands.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && (x.IsClient ?? false) && x.Id == Id).FirstOrDefaultAsync();
            return result;
        }

        //----------------------------Vendor-------------------------- 
        public async Task<int> VendorAddAsync(CompanyBrand entity)
        {
            entity.RegistrationCode = await TransID.GetTransId(context, Code.MasterVendorBrand, DateTime.Today);
            entity.IsClient = false;
            entity.IsVendor = true;
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> VendorDeleteSoftAsync(CompanyBrand entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> VendorEditAsync(CompanyBrand entity)
        {
            if (string.IsNullOrEmpty(entity.RegistrationCode)) entity.RegistrationCode = await TransID.GetTransId(context, Code.MasterVendorBrand, DateTime.Today);
            entity.IsClient = false;
            entity.IsVendor = true;
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<List<CompanyBrand>> VendorGet()
        {
            var result = await (from a in context.CompanyBrands
                                join b in context.Companys on a.CompanyId equals b.Id
                                where !(a.IsDeleted ?? false) && (a.IsVendor ?? false)
                                select new CompanyBrand
                                {
                                    Id = a.Id,
                                    RegistrationCode = a.RegistrationCode,
                                    BrandName = a.BrandName,
                                    CompanyName = b.CompanyName,
                                    ASFValue = a.ASFValue,
                                    ContactName = a.ContactName,
                                    TermOfPayment = a.TermOfPayment,
                                    CreatedOn = a.CreatedOn
                                }).ToListAsync();
            return result;
        }

        public async Task<CompanyBrand> VendorGet(string Id)
        {
            var result = await context.CompanyBrands.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && (x.IsVendor ?? false) && x.Id == Id).FirstOrDefaultAsync();
            return result;
        }
    }
}