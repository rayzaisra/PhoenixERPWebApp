using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Enum;
using Phoenix.Data.Models.HrisNew.Global;
using Phoenix.Data.RestApi;
using Phoenix.Shared.Core.Contexts;

namespace Phoenix.Data
{
    public interface IAllowanceComponentService : IDataService<AllowanceComponent>
    {
        Task<AllowanceComponentDto> GetAllowanceComponent(string Id, string employeeId);
    }

    public class AllowanceComponentService : IAllowanceComponentService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage AllowanceComponent
        /// </summary>
        /// <param name="context">Database context</param>
        public AllowanceComponentService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(AllowanceComponent entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params AllowanceComponent[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(AllowanceComponent entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params AllowanceComponent[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(AllowanceComponent entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params AllowanceComponent[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(AllowanceComponent entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(AllowanceComponent entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<AllowanceComponent>> Get()
        {
            var result = await (from a in context.AllowanceComponents
                                join b in context.JobGrades on a.JobGradeId equals b.Id
                                where !(a.IsDeleted ?? false)
                                select new AllowanceComponent
                                {
                                    Id = a.Id,
                                    SubComponentName = a.SubComponentName,
                                    Description = a.Description,
                                    Value = a.Value,
                                    JobGradeId = a.JobGradeId,
                                    JobGradeName = b.GradeName,
                                    CreatedOn = a.CreatedOn,
                                    Is_Salary_Report = a.Is_Salary_Report,
                                     IsDefault = a.IsDefault
                                }).OrderBy(x => x.CreatedOn).ToListAsync();
            return result;
        }

        public Task<List<AllowanceComponent>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<AllowanceComponent> Get(string Id) => await context.AllowanceComponents.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();

        public async Task<AllowanceComponentDto> GetAllowanceComponent(string Id, string employeeId)
        {
            AllowanceComponentDto resp = new AllowanceComponentDto();
            AllowanceComponent allowance = new AllowanceComponent();
            BpjsComponent bpjs = new BpjsComponent();
            resp = await context.AllowanceComponents.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).Select(x=> new AllowanceComponentDto() { SubComponentName = x.SubComponentName, Id =x.Id, Percent = 0, Value = x.Value }).FirstOrDefaultAsync();

            if (resp == null)
            {
                var basicSalary = await context.EmployeeSubcomponentAdditionals.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.AdditionalEmployeeBasicInfoId == employeeId && x.AdditionalComponentId == "ab4cf509-88ee-4c35-8083-67e305a22719").Select(x => x.AdditionalAmount).FirstOrDefaultAsync();
                if (string.IsNullOrEmpty(basicSalary))
                {
                    basicSalary = "0";
                }
                var basicSalaryDecimal = Convert.ToDecimal(basicSalary);
                
                resp = await context.BpjsComponents.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id && x.Type == 1).Select(x => new AllowanceComponentDto() { SubComponentName = x.SubComponentName, Id = x.Id, Percent = x.ValueCorporate, Value = (x.ValueCorporate * basicSalaryDecimal) /100 }).FirstOrDefaultAsync();

                if (Id == BasicSalary.JaminanBPJS2PercenId)
                {
                    if (basicSalaryDecimal > BasicSalary.MaxBpjsPensiun)
                    {
                        resp = await context.BpjsComponents.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).Select(x => new AllowanceComponentDto() { SubComponentName = x.SubComponentName, Id = x.Id, Percent = 2, Value = (2 * basicSalaryDecimal) / 100 }).FirstOrDefaultAsync();
                    }
                    else
                    {
                        resp = await context.BpjsComponents.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).Select(x => new AllowanceComponentDto() { SubComponentName = x.SubComponentName, Id = x.Id, Percent = 2, Value = (2 * basicSalaryDecimal) / 100 }).FirstOrDefaultAsync();
                    }
                }


            }
            //else if (Id == "1f783bb5-6855-49f7-9a30-a9416218a4bd")
            //{
            //    var totalHours = await context.Overtimes.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.EmployeeBasicInfoId == employeeId).Sum(x => x.HourOvertime);
            //}

            return resp;
        }
    }
}