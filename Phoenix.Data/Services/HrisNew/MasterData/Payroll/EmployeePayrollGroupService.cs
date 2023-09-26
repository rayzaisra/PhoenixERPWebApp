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
    public interface IEmployeePayrollGroupService : IDataService<EmployeePayrollGroup>
    {
        Task<List<GroupSubcomponentAdditional>> GetAdditionalList(string groupId);
        Task<List<GroupSubcomponentDeduction>> GetDeductionList(string groupId);
    }

    public class EmployeePayrollGroupService : IEmployeePayrollGroupService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage EmployeePayrollGroup
        /// </summary>
        /// <param name="context">Database context</param>
        public EmployeePayrollGroupService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(EmployeePayrollGroup entity)
        {
            return await AddEdit(entity, false);
        }

        public async Task<int> EditAsync(EmployeePayrollGroup entity)
        {
            return await AddEdit(entity, true);
        }

        private async Task<int> AddEdit(EmployeePayrollGroup entity, bool isEdit)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (isEdit)
                    {
                        context.PhoenixEdit(entity);
                    }
                    else
                    {
                        entity.Id = Guid.NewGuid().ToString();
                        await context.PhoenixAddAsync(entity);
                        await context.SaveChangesAsync();
                    }


                    var additional = await context.GroupSubcomponentAdditionals.AsNoTracking().Where(x => x.AdditionalEmployeePayrollGroupId == entity.Id).ToListAsync();
                    if (additional.Count > 0)
                    {
                        context.GroupSubcomponentAdditionals.RemoveRange(additional);
                    }

                    var deductions = await context.GroupSubcomponentDeductions.AsNoTracking().Where(x => x.EmployeePayrollGroupId == entity.Id).ToListAsync();
                    if (deductions.Count > 0)
                    {
                        context.GroupSubcomponentDeductions.RemoveRange(deductions);
                    }

                    if (entity.GroupSubcomponentAdditionals != null)
                    {
                        Parallel.ForEach(entity.GroupSubcomponentAdditionals, (item) =>
                        {
                            item.Id = Guid.NewGuid().ToString();
                            item.AdditionalEmployeePayrollGroupId = entity.Id;
                            context.PhoenixAddAsync(item);
                        });
                    }


                    if (entity.GroupSubcomponentDeductions != null)
                    {
                        Parallel.ForEach(entity.GroupSubcomponentDeductions, (item) =>
                        {
                            item.Id = Guid.NewGuid().ToString();
                            item.EmployeePayrollGroupId = entity.Id;
                            context.PhoenixAddAsync(item);
                        });
                    }

                    var save = await context.SaveChangesAsync();
                    transaction.Commit();
                    return save;

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        public Task<int> AddRangeAsync(params EmployeePayrollGroup[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(EmployeePayrollGroup entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params EmployeePayrollGroup[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(EmployeePayrollGroup entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params EmployeePayrollGroup[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> EditRangeAsync(EmployeePayrollGroup entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<EmployeePayrollGroup>> Get() => await context.EmployeePayrollGroups.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).ToListAsync();

        public Task<List<EmployeePayrollGroup>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<EmployeePayrollGroup> Get(string Id) => await context.EmployeePayrollGroups.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();

        public async Task<List<GroupSubcomponentAdditional>> GetAdditionalList(string groupId)
        {
            var result = new List<GroupSubcomponentAdditional>();
            var salaryComponent = await (from a in context.GroupSubcomponentAdditionals
                                         join b in context.SalaryMainComponents on a.AdditionalComponentId equals b.Id
                                         where a.AdditionalSalaryComponent == SalaryComponentTypeAdditional.SalaryComponent && !(a.IsDeleted ?? false) && a.AdditionalEmployeePayrollGroupId == groupId
                                         select new GroupSubcomponentAdditional
                                         {
                                             Id = a.Id,
                                             AdditionalEmployeePayrollGroupId = a.AdditionalEmployeePayrollGroupId,
                                             AdditionalSalaryComponent = a.AdditionalSalaryComponent,
                                             AdditionalComponentId = a.AdditionalComponentId,
                                             AdditionalPercent = a.AdditionalPercent,
                                             AdditionalAmount = a.AdditionalAmount,
                                             CreatedBy = a.CreatedBy,
                                             CreatedOn = a.CreatedOn,
                                             ModifiedBy = a.ModifiedBy,
                                             ModifiedOn = a.ModifiedOn,
                                             ApprovedBy = a.ApprovedBy,
                                             ApprovedOn = a.ApprovedOn,
                                             IsActive = a.IsActive,
                                             IsLocked = a.IsLocked,
                                             IsDefault = a.IsDefault,
                                             IsDeleted = a.IsDeleted,
                                             OwnerId = a.OwnerId,
                                             DeletedBy = a.DeletedBy,
                                             DeletedOn = a.DeletedOn,
                                             BusinessUnitDivisionId = a.BusinessUnitDivisionId,
                                             BusinessUnitDepartementId = a.BusinessUnitDepartementId,
                                             AdditionalComponentTypeName = b.SubComponentName
                                         }).OrderByDescending(x => x.CreatedOn).ToListAsync();

            var allowance = await (from a in context.GroupSubcomponentAdditionals
                                   join b in context.AllowanceComponents on a.AdditionalComponentId equals b.Id
                                   where a.AdditionalSalaryComponent == SalaryComponentTypeAdditional.Allowance && !(a.IsDeleted ?? false) && a.AdditionalEmployeePayrollGroupId == groupId
                                   select new GroupSubcomponentAdditional
                                   {
                                       Id = a.Id,
                                       AdditionalEmployeePayrollGroupId = a.AdditionalEmployeePayrollGroupId,
                                       AdditionalSalaryComponent = a.AdditionalSalaryComponent,
                                       AdditionalComponentId = a.AdditionalComponentId,
                                       AdditionalPercent = a.AdditionalPercent,
                                       AdditionalAmount = a.AdditionalAmount,
                                       CreatedBy = a.CreatedBy,
                                       CreatedOn = a.CreatedOn,
                                       ModifiedBy = a.ModifiedBy,
                                       ModifiedOn = a.ModifiedOn,
                                       ApprovedBy = a.ApprovedBy,
                                       ApprovedOn = a.ApprovedOn,
                                       IsActive = a.IsActive,
                                       IsLocked = a.IsLocked,
                                       IsDefault = a.IsDefault,
                                       IsDeleted = a.IsDeleted,
                                       OwnerId = a.OwnerId,
                                       DeletedBy = a.DeletedBy,
                                       DeletedOn = a.DeletedOn,
                                       BusinessUnitDivisionId = a.BusinessUnitDivisionId,
                                       BusinessUnitDepartementId = a.BusinessUnitDepartementId,
                                       AdditionalComponentTypeName = b.SubComponentName
                                   }).OrderByDescending(x => x.CreatedOn).ToListAsync();

            var bpjs = await (from a in context.GroupSubcomponentAdditionals
                              join b in context.BpjsComponents on a.AdditionalComponentId equals b.Id
                              where a.AdditionalSalaryComponent == SalaryComponentTypeAdditional.BPJS && b.Type == BPJSType.Additional && !(a.IsDeleted ?? false) && a.AdditionalEmployeePayrollGroupId == groupId
                              select new GroupSubcomponentAdditional
                              {
                                  Id = a.Id,
                                  AdditionalEmployeePayrollGroupId = a.AdditionalEmployeePayrollGroupId,
                                  AdditionalSalaryComponent = a.AdditionalSalaryComponent,
                                  AdditionalComponentId = a.AdditionalComponentId,
                                  AdditionalPercent = a.AdditionalPercent,
                                  AdditionalAmount = a.AdditionalAmount,
                                  CreatedBy = a.CreatedBy,
                                  CreatedOn = a.CreatedOn,
                                  ModifiedBy = a.ModifiedBy,
                                  ModifiedOn = a.ModifiedOn,
                                  ApprovedBy = a.ApprovedBy,
                                  ApprovedOn = a.ApprovedOn,
                                  IsActive = a.IsActive,
                                  IsLocked = a.IsLocked,
                                  IsDefault = a.IsDefault,
                                  IsDeleted = a.IsDeleted,
                                  OwnerId = a.OwnerId,
                                  DeletedBy = a.DeletedBy,
                                  DeletedOn = a.DeletedOn,
                                  BusinessUnitDivisionId = a.BusinessUnitDivisionId,
                                  BusinessUnitDepartementId = a.BusinessUnitDepartementId,
                                  AdditionalComponentTypeName = b.SubComponentName
                              }).OrderByDescending(x => x.CreatedOn).ToListAsync();

            result.AddRange(salaryComponent);
            result.AddRange(allowance);
            result.AddRange(bpjs);

            return result;
        }

        public async Task<List<GroupSubcomponentDeduction>> GetDeductionList(string groupId)
        {
            var result = new List<GroupSubcomponentDeduction>();
            var deduction = await (from a in context.GroupSubcomponentDeductions
                                   join b in context.DeductionComponents on a.ComponentId equals b.Id
                                   where a.SalaryComponent == SalaryComponentTypeDeduction.Deduction &&
                                   !(a.IsDeleted ?? false) && a.EmployeePayrollGroupId == groupId
                                   select new GroupSubcomponentDeduction
                                   {
                                       Id = a.Id,
                                       EmployeePayrollGroupId = a.EmployeePayrollGroupId,
                                       SalaryComponent = a.SalaryComponent,
                                       ComponentId = a.ComponentId,
                                       Percent = a.Percent,
                                       Amount = a.Amount,
                                       CreatedBy = a.CreatedBy,
                                       CreatedOn = a.CreatedOn,
                                       ModifiedBy = a.ModifiedBy,
                                       ModifiedOn = a.ModifiedOn,
                                       ApprovedBy = a.ApprovedBy,
                                       ApprovedOn = a.ApprovedOn,
                                       IsActive = a.IsActive,
                                       IsLocked = a.IsLocked,
                                       IsDefault = a.IsDefault,
                                       IsDeleted = a.IsDeleted,
                                       OwnerId = a.OwnerId,
                                       DeletedBy = a.DeletedBy,
                                       DeletedOn = a.DeletedOn,
                                       BusinessUnitDivisionId = a.BusinessUnitDivisionId,
                                       BusinessUnitDepartementId = a.BusinessUnitDepartementId,
                                       ComponentTypeName = b.SubComponentName
                                   }).OrderByDescending(x => x.CreatedOn).ToListAsync();

            var bpjs = await (from a in context.GroupSubcomponentDeductions
                              join b in context.BpjsComponents on a.ComponentId equals b.Id
                              where a.SalaryComponent == SalaryComponentTypeDeduction.BPJS && b.Type == BPJSType.Deduction &&
                              !(a.IsDeleted ?? false) && a.EmployeePayrollGroupId == groupId
                              select new GroupSubcomponentDeduction
                              {
                                  Id = a.Id,
                                  EmployeePayrollGroupId = a.EmployeePayrollGroupId,
                                  SalaryComponent = a.SalaryComponent,
                                  ComponentId = a.ComponentId,
                                  Percent = a.Percent,
                                  Amount = a.Amount,
                                  CreatedBy = a.CreatedBy,
                                  CreatedOn = a.CreatedOn,
                                  ModifiedBy = a.ModifiedBy,
                                  ModifiedOn = a.ModifiedOn,
                                  ApprovedBy = a.ApprovedBy,
                                  ApprovedOn = a.ApprovedOn,
                                  IsActive = a.IsActive,
                                  IsLocked = a.IsLocked,
                                  IsDefault = a.IsDefault,
                                  IsDeleted = a.IsDeleted,
                                  OwnerId = a.OwnerId,
                                  DeletedBy = a.DeletedBy,
                                  DeletedOn = a.DeletedOn,
                                  BusinessUnitDivisionId = a.BusinessUnitDivisionId,
                                  BusinessUnitDepartementId = a.BusinessUnitDepartementId,
                                  ComponentTypeName = b.SubComponentName
                              }).OrderByDescending(x => x.CreatedOn).ToListAsync();

            result.AddRange(deduction);
            result.AddRange(bpjs);
            return result;
        }
    }
}