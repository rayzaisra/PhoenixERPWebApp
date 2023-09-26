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
    public interface IEmployeeHealthInsuranceService
    {
        Task<int> AddAsync(EmployeeHealthInsurance entity);
        Task<EmployeeHealthInsurance> Get(string employeeId);
        Task<List<EmployeeHealthInsuranceFamily>> GetFamilyList(string employeeId);
        Task<EmployeeHealthInsuranceFamily> GetFamilyById(string familyId);
    }

    public class EmployeeHealthInsuranceService : IEmployeeHealthInsuranceService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage EmployeeHealthInsurance
        /// </summary>
        /// <param name="context">Database context</param>
        public EmployeeHealthInsuranceService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(EmployeeHealthInsurance entity)
        {
            if (context.EmployeeHealthInsurances.AsNoTracking().Where(x => x.EmployeeBasicInfoId == entity.EmployeeBasicInfoId).Count() > 0)
                context.PhoenixEdit(entity);
            else
                context.PhoenixAdd(entity);

            if (entity.EmployeeHealthInsuranceFamilys != null)
            {
                context.PhoenixDeleteRange(context.EmployeeHealthInsuranceFamilys.AsNoTracking().Where(x => !entity.EmployeeHealthInsuranceFamilys.Any(y => y.Id == x.Id)).ToArray());
                context.PhoenixEditRange(entity.EmployeeHealthInsuranceFamilys.Where(x => !string.IsNullOrEmpty(x.Id)).ToArray());
                entity.EmployeeHealthInsuranceFamilys.Where(x => string.IsNullOrEmpty(x.Id)).ToList().ForEach(item => { item.Id = Guid.NewGuid().ToString(); item.EmployeeBasicInfoId = entity.EmployeeBasicInfoId; });
                context.PhoenixAddRange(entity.EmployeeHealthInsuranceFamilys.Where(x => string.IsNullOrEmpty(x.CreatedBy)).ToArray());
            }
            

            return await context.SaveChangesAsync();
        }

        public async Task<EmployeeHealthInsurance> Get(string employeeId)
        {
            if (context.EmployeeHealthInsurances.Where(x => x.EmployeeBasicInfoId == employeeId).Count() > 0)
            {
                var result = await (from a in context.EmployeeHealthInsurances
                                    join b in context.EmployeeBasicInfos on a.EmployeeBasicInfoId equals b.Id
                                    where !(a.IsDeleted ?? false) && a.EmployeeBasicInfoId == employeeId
                                    select new EmployeeHealthInsurance
                                    {
                                        EmployeeBasicInfoId = a.EmployeeBasicInfoId,
                                        Endorsement = a.Endorsement,
                                        EffectiveDate = a.EffectiveDate,
                                        PlanNumber = a.PlanNumber,
                                        InsuranceCompanyId = a.InsuranceCompanyId,
                                        Remarks = a.Remarks,
                                        Inpatient = a.Inpatient,
                                        InpatientPremi = a.InpatientPremi,
                                        Outpatient = a.Outpatient,
                                        OutpatientPremi = a.OutpatientPremi,
                                        Dental = a.Dental,
                                        DentalPremi = a.DentalPremi,
                                        Maternity = a.Maternity,
                                        MaternityPremi = a.MaternityPremi,
                                        Optical = a.Optical,
                                        OpticalPremi = a.OpticalPremi,
                                        TotalMutation = a.TotalMutation,
                                        MutationDate = a.MutationDate,
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
                                        infoEmployee = new InfoEmployee
                                        {
                                            EmployeeId = b.Id,
                                            EmployeeName = b.NameEmployee,
                                            DateEmployed = b.JoinDate,
                                            Phone = b.PhoneNumber,
                                            Email = b.Email,
                                            DateBirth = b.DateBirth,
                                            Age = Convert.ToDateTime(b.DateBirth).AddYears(DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) > DateTime.Now ?
                                                  (DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) - 1 : DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year,
                                            JobTitle = b.BusinessUnitJobLevelId == null ? "" :
                                                              (from r in context.BusinessUnitJobLevels
                                                               join s in context.JobTitles on r.JobTitleId equals s.Id
                                                               where r.Id == b.BusinessUnitJobLevelId
                                                               select s.TitleName).FirstOrDefault(),
                                            Grade = b.BusinessUnitJobLevelId == null ? "" :
                                                              (from r in context.BusinessUnitJobLevels
                                                               join s in context.JobGrades on r.JobGradeId equals s.Id
                                                               where r.Id == b.BusinessUnitJobLevelId
                                                               select s.GradeName).FirstOrDefault(),
                                            EmployementStatus = context.EmployementStatuss.Where(x => x.Id == b.EmployementStatusId).Select(x => x.EmploymentStatusName).FirstOrDefault(),
                                            MarriedStatusName = context.MarriedStatuss.Where(x => x.Id == b.MarriedStatusId).Select(x => x.MarriedName).FirstOrDefault()
                                        }
                                    }
                               ).FirstOrDefaultAsync();
                return result;
            }
            else
            {
                var result = await (from b in context.EmployeeBasicInfos
                                    where !(b.IsDeleted ?? false) && b.Id == employeeId
                                    select new EmployeeHealthInsurance
                                    {
                                        EmployeeBasicInfoId = employeeId,
                                        infoEmployee = new InfoEmployee
                                        {
                                            EmployeeId = b.Id,
                                            EmployeeName = b.NameEmployee,
                                            DateEmployed = b.JoinDate,
                                            Phone = b.PhoneNumber,
                                            Email = b.Email,
                                            DateBirth = b.DateBirth,
                                            Age = Convert.ToDateTime(b.DateBirth).AddYears(DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) > DateTime.Now ?
                                                  (DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) - 1 : DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year,
                                            JobTitle = b.BusinessUnitJobLevelId == null ? "" :
                                                              (from r in context.BusinessUnitJobLevels
                                                               join s in context.JobTitles on r.JobTitleId equals s.Id
                                                               where r.Id == b.BusinessUnitJobLevelId
                                                               select s.TitleName).FirstOrDefault(),
                                            Grade = b.BusinessUnitJobLevelId == null ? "" :
                                                              (from r in context.BusinessUnitJobLevels
                                                               join s in context.JobGrades on r.JobGradeId equals s.Id
                                                               where r.Id == b.BusinessUnitJobLevelId
                                                               select s.GradeName).FirstOrDefault(),
                                            EmployementStatus = context.EmployementStatuss.Where(x => x.Id == b.EmployementStatusId).Select(x => x.EmploymentStatusName).FirstOrDefault(),
                                            MarriedStatusName = context.MarriedStatuss.Where(x => x.Id == b.MarriedStatusId).Select(x => x.MarriedName).FirstOrDefault()
                                        }
                                    }
                              ).FirstOrDefaultAsync();
                return result;
            }
        }

        public async Task<List<EmployeeHealthInsuranceFamily>> GetFamilyList(string employeeId)
        {
            var result = await (from a in context.EmployeeHealthInsuranceFamilys
                                join b in context.Familys on a.FamilyId equals b.Id
                                join c in context.FamilyStatuss on b.FamilyStatusId equals c.Id
                                where !(a.IsDeleted ?? false) && a.EmployeeBasicInfoId == employeeId
                                select new EmployeeHealthInsuranceFamily
                                {
                                    Id = a.Id,
                                    EmployeeBasicInfoId = a.EmployeeBasicInfoId,
                                    FamilyId = a.FamilyId,
                                    FamilyName = b.Name,
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
                                    FamilyStatus = c.FamilyStatusName,
                                    Gender = b.Gender,
                                    DateBirth = b.Birthday
                                }).OrderByDescending(x => x.CreatedOn).ToListAsync();
            return result;
        }

        public async Task<EmployeeHealthInsuranceFamily> GetFamilyById(string familyId)
        {
            var result = await (from b in context.Familys
                                join c in context.FamilyStatuss on b.FamilyStatusId equals c.Id
                                where !(b.IsDeleted ?? false) && b.Id == familyId
                                select new EmployeeHealthInsuranceFamily
                                {
                                    FamilyId = b.Id,
                                    FamilyName = b.Name,
                                    FamilyStatus = c.FamilyStatusName,
                                    Gender = b.Gender,
                                    DateBirth = b.Birthday
                                }).FirstOrDefaultAsync();
            return result;
        }
    }
}