using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models;
using Phoenix.Data.Models.HrisNew.Transaction.Payroll;
using Phoenix.Data.Models.ViewModel;
using Phoenix.Data.RestApi;
using Phoenix.Shared.Core.Contexts;

namespace Phoenix.Data
{
    public interface IEmployeePayrollProfileService
    {
        Task<int> AddAsync(EmployeePayrollProfile entity);
        Task<List<InfoEmployee>> Gets(string emplyeeName);
        Task<InfoEmployee> Get(string employeeId);
        Task<List<EmployeeSubcomponentAdditional>> GetAdditionalList(string employeeId);
        Task<List<EmployeeSubcomponentDeduction>> GetDeductionList(string employeeId);
        Task<EmployeeHistoryPayroll> HistoryPayroll(string employeeId);
        Task<pph21> getpph21(string ptkpId, string employeeid);
    }

    public class EmployeePayrollProfileService : IEmployeePayrollProfileService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage EmployeeSubcomponentSalary
        /// </summary>
        /// <param name="context">Database context</param>
        public EmployeePayrollProfileService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(EmployeePayrollProfile entity)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var additional = await context.EmployeeSubcomponentAdditionals.AsNoTracking().Where(x => x.AdditionalEmployeeBasicInfoId == entity.EmployeeId).ToListAsync();
                    if (additional.Count > 0)
                    {
                        context.EmployeeSubcomponentAdditionals.RemoveRange(additional);
                    }

                    var deductions = await context.EmployeeSubcomponentDeductions.AsNoTracking().Where(x => x.EmployeeBasicInfoId == entity.EmployeeId).ToListAsync();
                    if (deductions.Count > 0)
                    {
                        context.EmployeeSubcomponentDeductions.RemoveRange(deductions);
                    }

                    if (entity.EmployeeSubcomponentAdditionals != null)
                    {
                        foreach (var item in entity.EmployeeSubcomponentAdditionals)
                        {
                            item.AdditionalAmount = Math.Floor(float.Parse(item.AdditionalAmount)).ToString();
                            item.AdditionalPercent = float.Parse(item.AdditionalPercent).ToString();
                        }

                        Parallel.ForEach(entity.EmployeeSubcomponentAdditionals, (item) =>
                        {
                            item.Id = Guid.NewGuid().ToString();
                            item.AdditionalEmployeeBasicInfoId = entity.EmployeeId;
                            item.IsEncrypt = true;
                            item.AdditionalAmount = Phoenix.ApiExtension.Helpers.SecurityHelper.EncryptString(item.AdditionalAmount);
                            item.AdditionalPercent = Phoenix.ApiExtension.Helpers.SecurityHelper.EncryptString(item.AdditionalPercent);
                            context.PhoenixAddAsync(item);
                        });
                    }


                    if (entity.EmployeeSubcomponentDeductions != null)
                    {
                        foreach (var item in entity.EmployeeSubcomponentDeductions)
                        {
                            item.Amount = Math.Floor(float.Parse(item.Amount)).ToString();
                            item.Percent = float.Parse(item.Percent).ToString();
                        }

                        Parallel.ForEach(entity.EmployeeSubcomponentDeductions, (item) =>
                        {
                            item.Id = Guid.NewGuid().ToString();
                            item.EmployeeBasicInfoId = entity.EmployeeId;
                            item.IsEncrypt = true;
                            item.Percent = Phoenix.ApiExtension.Helpers.SecurityHelper.EncryptString(item.Percent);
                            item.Amount = Phoenix.ApiExtension.Helpers.SecurityHelper.EncryptString(item.Amount);
                            context.PhoenixAddAsync(item);
                        });
                    }

                    var employee = await context.EmployeeBasicInfos.AsNoTracking().Where(x => x.Id == entity.EmployeeId).FirstOrDefaultAsync();
                    if (employee != null)
                    {
                        employee.PtkpSettingId = entity.PtkpSettingId;
                        context.EmployeeBasicInfos.Update(employee);
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

        public async Task<List<InfoEmployee>> Gets(string emplyeeName)
        {
            return await (from b in context.EmployeeBasicInfos
                          where !(b.IsDeleted ?? false) && (emplyeeName == null ? b.NameEmployee  == b.NameEmployee : b.NameEmployee.Contains(emplyeeName))
                          select new InfoEmployee
                          {
                              EmployeeId = b.Id,
                              EmployeeName = b.NameEmployee,
                              DateEmployed = b.JoinDate,
                              //Phone = b.PhoneNumber,
                              //Email = b.Email,
                              //DateBirth = b.DateBirth,
                              //Age = Convert.ToDateTime(b.DateBirth).AddYears(DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) > DateTime.Now ?
                              //                                 (DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) - 1 : DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year,
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
                              LegalEntityId = b.LegalEntityId,
                              LegalEntityName = b.LegalEntityId == null ? "" : (from x in context.LegalEntity
                                                                                where x.Id == b.LegalEntityId && x.IsDeleted == false
                                                                                select x.LegalEntityName).FirstOrDefault(),
                              PtkpSettingId = b.PtkpSettingId,
                              PtkpSettingName = b.PtkpSettingId == null ? "" : (from y in context.PtkpSettings
                                                                                where y.Id == b.PtkpSettingId && y.IsDeleted == false
                                                                                select y.Name).FirstOrDefault(),
                              Division = b.BusinessUnitJobLevelId == null ? "" :
                                                                        (from r in context.BusinessUnitJobLevels
                                                                         join s in context.BusinessUnits on r.BusinessUnitDivisionId equals s.Id
                                                                         join t in context.BusinessUnitTypes on s.BusinessUnitTypeId equals t.Id
                                                                         where t.BusinessUnitLevel == BusinessUnitLevelCode.Division && r.Id == b.BusinessUnitJobLevelId
                                                                         select s.UnitName).FirstOrDefault()
                          }).ToListAsync();
        }

        public async Task<InfoEmployee> Get(string employeeId)
        {
            var dataEmployee = await (from b in context.EmployeeBasicInfos
                                      where !(b.IsDeleted ?? false) && b.Id == employeeId
                                      select new InfoEmployee
                                      {
                                          EmployeeId = b.Id,
                                          EmployeeName = b.NameEmployee,
                                          //DateEmployed = b.JoinDate,
                                          Phone = b.PhoneNumber,
                                          Email = b.Email,
                                          //DateBirth = b.DateBirth,
                                          //Age = Convert.ToDateTime(b.DateBirth).AddYears(DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) > DateTime.Now ?
                                          //                                 (DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) - 1 : DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year,
                                          //JobTitle = b.BusinessUnitJobLevelId == null ? "" :
                                          //                                             (from r in context.BusinessUnitJobLevels
                                          //                                              join s in context.JobTitles on r.JobTitleId equals s.Id
                                          //                                              where r.Id == b.BusinessUnitJobLevelId
                                          //                                              select s.TitleName).FirstOrDefault(),
                                          GradeId = b.BusinessUnitJobLevelId == null ? "" :
                                                                                       (from r in context.BusinessUnitJobLevels
                                                                                        join s in context.JobGrades on r.JobGradeId equals s.Id
                                                                                        where r.Id == b.BusinessUnitJobLevelId
                                                                                        select s.Id).FirstOrDefault(),
                                          Grade = b.BusinessUnitJobLevelId == null ? "" :
                                                                                       (from r in context.BusinessUnitJobLevels
                                                                                        join s in context.JobGrades on r.JobGradeId equals s.Id
                                                                                        where r.Id == b.BusinessUnitJobLevelId
                                                                                        select s.GradeName).FirstOrDefault(),
                                          //EmployementStatus = context.EmployementStatuss.Where(x => x.Id == b.EmployementStatusId).Select(x => x.EmploymentStatusName).FirstOrDefault(),
                                          Mobile = b.MobileNumber,
                                          Gender = b.Gender,
                                          PtkpSettingId = b.PtkpSettingId,
                                          LegalEntityId = b.LegalEntityId,
                                          LegalEntityName = b.LegalEntityId == null ? "" : (from x in context.LegalEntity
                                                                                            where x.Id == b.LegalEntityId && x.IsDeleted == false
                                                                                            select x.LegalEntityName).FirstOrDefault(),
                                          PtkpSettingName = b.PtkpSettingId == null ? "" : (from y in context.PtkpSettings
                                                                                            where y.Id == b.PtkpSettingId && y.IsDeleted == false
                                                                                            select y.Name).FirstOrDefault(),
                                          Division = b.BusinessUnitJobLevelId == null ? "" :
                                                                        (from r in context.BusinessUnitJobLevels
                                                                         join s in context.BusinessUnits on r.BusinessUnitDivisionId equals s.Id
                                                                         join t in context.BusinessUnitTypes on s.BusinessUnitTypeId equals t.Id
                                                                         where t.BusinessUnitLevel == BusinessUnitLevelCode.Division && r.Id == b.BusinessUnitJobLevelId
                                                                         select s.UnitName).FirstOrDefault()
                                      }).FirstOrDefaultAsync();


            //DecryptPayroll();
            if (dataEmployee != null)
            {
                await DecryptPayroll(employeeId);
                var month = DateTime.Now.Month;
                var year = DateTime.Now.Year;
                var paramType = new SqlParameter[] {
                                new SqlParameter("@employeeid", employeeId),
                                new SqlParameter("@month", month),
                                new SqlParameter("@year", year)
                            };

                var ds = await context.ExecuteStoredProcedureDataSet("uspCalculatePph21", paramType);
                var que = string.Format("select * from hr.salary_history where employee_basic_info_id = '{0}' and month = {1} and year = {2}", employeeId, month, year);
                var dt = await context.GetDataTable(que, null);
                if (dt.Rows.Count > 0)
                {
                    var pph21 = dt.Rows[0]["pph21"].ToString();
                    if (!string.IsNullOrEmpty(pph21))
                    {
                        dataEmployee.Pph21 = Convert.ToDecimal(dt.Rows[0]["pph21"].ToString());
                        dataEmployee.NetSalary = 0;
                    }
                    else
                    {
                        dataEmployee.Pph21 = 0;
                        dataEmployee.NetSalary = 0;
                    }
                }

            }


            return dataEmployee;
        }

        public async Task<List<EmployeeSubcomponentAdditional>> GetAdditionalList(string employeeId)
        {
            var UserSessionId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(context.HttpContext);

            var result = new List<EmployeeSubcomponentAdditional>();
            var salaryComponent = await (from a in context.EmployeeSubcomponentAdditionals
                                         join b in context.SalaryMainComponents on a.AdditionalComponentId equals b.Id
                                         where a.AdditionalSalaryComponent == SalaryComponentTypeAdditional.SalaryComponent && !(a.IsDeleted ?? false) && a.AdditionalEmployeeBasicInfoId == employeeId && !(b.IsDeleted ?? false)
                                         select new EmployeeSubcomponentAdditional
                                         {
                                             Id = a.Id,
                                             AdditionalEmployeeBasicInfoId = a.AdditionalEmployeeBasicInfoId,
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
                                             AdditionalComponentTypeName = b.SubComponentName,
                                             IsEncrypt = a.IsEncrypt
                                         }).OrderByDescending(x => x.CreatedOn).ToListAsync();

            var allowance = await (from a in context.EmployeeSubcomponentAdditionals
                                   join b in context.AllowanceComponents on a.AdditionalComponentId equals b.Id
                                   where a.AdditionalSalaryComponent == SalaryComponentTypeAdditional.Allowance && !(a.IsDeleted ?? false) && a.AdditionalEmployeeBasicInfoId == employeeId && !(b.IsDeleted ?? false)
                                   select new EmployeeSubcomponentAdditional
                                   {
                                       Id = a.Id,
                                       AdditionalEmployeeBasicInfoId = a.AdditionalEmployeeBasicInfoId,
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
                                       AdditionalComponentTypeName = b.SubComponentName,
                                       IsEncrypt = a.IsEncrypt
                                   }).OrderByDescending(x => x.CreatedOn).ToListAsync();

            var bpjs = await (from a in context.EmployeeSubcomponentAdditionals
                              join b in context.BpjsComponents on a.AdditionalComponentId equals b.Id
                              where a.AdditionalSalaryComponent == SalaryComponentTypeAdditional.BPJS && b.Type == BPJSType.Additional && !(a.IsDeleted ?? false) && a.AdditionalEmployeeBasicInfoId == employeeId && !(b.IsDeleted ?? false)
                              select new EmployeeSubcomponentAdditional
                              {
                                  Id = a.Id,
                                  AdditionalEmployeeBasicInfoId = a.AdditionalEmployeeBasicInfoId,
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
                                  AdditionalComponentTypeName = b.SubComponentName,
                                  IsEncrypt = a.IsEncrypt
                              }).OrderByDescending(x => x.CreatedOn).ToListAsync();

            result.AddRange(salaryComponent);
            result.AddRange(allowance);
            result.AddRange(bpjs);

            DateTime dt = DateTime.Now;
            var monthToday = dt.Month;
            var yearToday = dt.Year;

            var declarationPayrollAdd = await (from q in context.DeclarationPayrollHeader
                                               join r in context.DeclarationPayrollDetail on q.Id equals r.DeclarationPayrollHeaderId
                                               join t in context.AllowanceComponents on q.SubComponentId equals t.Id
                                               where q.IsDeleted == false && q.DeclarationType == "Additional" && r.EmployeeBasicInfoId == employeeId
                                               select new EmployeeSubcomponentAdditional()
                                               {
                                                   amountTempDouble = r.Amount,
                                                   //AdditionalAmount = r.Amount.ToString(),
                                                   AdditionalEmployeeBasicInfoId = r.EmployeeBasicInfoId,
                                                   AdditionalComponentId = q.SubComponentId,
                                                   AdditionalPercent = "0",
                                                   IsEncrypt = false,
                                                   AdditionalComponentTypeName = t.SubComponentName,
                                                   month = q.Month,
                                                   year = q.Year,
                                                   AdditionalSalaryComponent = 2
                                               }
                                               ).ToListAsync();

            List<EmployeeSubcomponentAdditional> dpa = new List<EmployeeSubcomponentAdditional>();

            foreach (var item in declarationPayrollAdd)
            {
                if (item.amountTempDouble != null && item.amountTempDouble != 0)
                {
                    var amounDes = Convert.ToDecimal(item.amountTempDouble);
                    item.AdditionalAmount = amounDes.ToString();
                }
                else
                {
                    item.AdditionalAmount = "0";
                }
            }

            dpa = declarationPayrollAdd.Where(x => x.month == monthToday && x.year == yearToday).ToList();
            if (dpa.Count == 0)
            {
                var getLastMonthYear = declarationPayrollAdd.OrderByDescending(x=>x.year).OrderByDescending(x=>x.month).FirstOrDefault();
                dpa = declarationPayrollAdd.Where(x => x.month == getLastMonthYear.month && x.year == getLastMonthYear.year).ToList();
            }

            result = result.Where(p => !dpa.Any(p2 => p2.AdditionalComponentId == p.AdditionalComponentId)).ToList();

            var basicSalary = await context.EmployeeSubcomponentAdditionals.Where(x => !(x.IsDeleted ?? false) && x.AdditionalEmployeeBasicInfoId == employeeId && x.AdditionalComponentId == "ab4cf509-88ee-4c35-8083-67e305a22719").FirstOrDefaultAsync();
            var timeSleepSalary = await context.EmployeeSubcomponentAdditionals.Where(x => !(x.IsDeleted ?? false) && x.AdditionalEmployeeBasicInfoId == employeeId && x.AdditionalComponentId == "563e6fb7-dae5-4d9c-be68-1b28f26bb1b7").FirstOrDefaultAsync();
            string basicSalaryValue = "0";
            string timeSleepAllowanceValue = "0";
            
            if (basicSalary != null)
            {
                if (basicSalary.IsEncrypt == true)
                {
                    basicSalaryValue = Phoenix.ApiExtension.Helpers.SecurityHelper.DecryptString(basicSalary.AdditionalAmount);
                }
                else
                {
                    basicSalaryValue = basicSalary.AdditionalAmount;
                }
            }

            if (timeSleepSalary != null)
            {
                if (timeSleepSalary.IsEncrypt == true)
                {
                    timeSleepAllowanceValue = Phoenix.ApiExtension.Helpers.SecurityHelper.DecryptString(timeSleepSalary.AdditionalAmount);
                }
                else
                {
                    timeSleepAllowanceValue = timeSleepSalary.AdditionalAmount;
                }
            }
            else
            {
                var existsTimesleepInDeclaration = dpa.Where(x => x.AdditionalComponentId == "563e6fb7-dae5-4d9c-be68-1b28f26bb1b7" && x.AdditionalEmployeeBasicInfoId == employeeId).FirstOrDefault();
                if (existsTimesleepInDeclaration != null)
                {
                    timeSleepAllowanceValue = dpa.Where(x => x.AdditionalComponentId == "563e6fb7-dae5-4d9c-be68-1b28f26bb1b7" && x.AdditionalEmployeeBasicInfoId == employeeId).Select(x => x.AdditionalAmount).FirstOrDefault();
                } 
            }

            var basicSalaryDec = Convert.ToDecimal(basicSalaryValue);
            var timeSleepAllowanceDec = Convert.ToDecimal(timeSleepAllowanceValue);
            var bpjsDefault = await (from q in context.BpjsComponents
                                     where q.Type == BPJSType.Additional && q.IsDefault == true && !(q.IsDeleted ?? false)
                                     select new EmployeeSubcomponentAdditional()
                                     {
                                         //AdditionalAmount = (( (basicSalaryDec + timeSleepAllowanceDec) * q.ValueCorporate)/100).ToString(),
                                         AdditionalAmount = ((basicSalaryDec * q.ValueCorporate) / 100).ToString(),
                                         AdditionalEmployeeBasicInfoId = employeeId,
                                         AdditionalComponentId = q.Id,
                                         AdditionalPercent = q.ValueCorporate.ToString(),
                                         IsEncrypt = false,
                                         AdditionalComponentTypeName = q.SubComponentName,
                                         AdditionalSalaryComponent = SalaryComponentTypeAdditional.BPJS
                                     }
                                     ).ToListAsync();

            bpjsDefault = bpjsDefault.Where(p => !result.Any(p2 => p2.AdditionalComponentId == p.AdditionalComponentId)).ToList();

            var query = await context.UserEmployeePayrolls.Where(x => x.EmployeeBasicInfoId == UserSessionId && x.MenuUnique == MenuUnique.EmployeePayroll && x.IsDeleted == false).AsNoTracking().FirstOrDefaultAsync();

            if (query != null)
            {
                foreach (var item in salaryComponent)
                {
                    if (item.IsEncrypt == true)
                    {
                        item.AdditionalAmount = Phoenix.ApiExtension.Helpers.SecurityHelper.DecryptString(item.AdditionalAmount);
                        item.AdditionalPercent = Phoenix.ApiExtension.Helpers.SecurityHelper.DecryptString(item.AdditionalPercent);
                    }
                }

                foreach (var item in allowance)
                {
                    if (item.IsEncrypt == true)
                    {
                        item.AdditionalAmount = Phoenix.ApiExtension.Helpers.SecurityHelper.DecryptString(item.AdditionalAmount);
                        item.AdditionalPercent = Phoenix.ApiExtension.Helpers.SecurityHelper.DecryptString(item.AdditionalPercent);
                    }
                }

                foreach (var item in bpjs)
                {
                    if (item.IsEncrypt == true)
                    {
                        item.AdditionalAmount = Phoenix.ApiExtension.Helpers.SecurityHelper.DecryptString(item.AdditionalAmount);
                        item.AdditionalPercent = Phoenix.ApiExtension.Helpers.SecurityHelper.DecryptString(item.AdditionalPercent);
                    }
                }
            }
            else
            {
                foreach (var item in salaryComponent)
                {
                    item.AdditionalAmount = "0";
                    item.AdditionalPercent = "0";
                }

                foreach (var item in allowance)
                {
                    item.AdditionalAmount = "0";
                    item.AdditionalPercent = "0";
                }

                foreach (var item in bpjs)
                {
                    item.AdditionalAmount = "0";
                    item.AdditionalPercent = "0";
                }
            }
           
            //result.AddRange(dpa);
            result.AddRange(bpjsDefault);
           

            return result;
        }

        public async Task<int> DecryptPayroll(string employeeId)
        {
            var save = 1;
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var dataPayroll1 = await context.EmployeeSubcomponentAdditionals.Where(x => x.IsEncrypt == true && x.IsDeleted == false && x.AdditionalEmployeeBasicInfoId == employeeId).ToListAsync();
                    foreach (var item in dataPayroll1)
                    {
                        if (item.IsEncrypt == true)
	                    {
                            item.AdditionalAmount = Phoenix.ApiExtension.Helpers.SecurityHelper.DecryptString(item.AdditionalAmount);
                            item.AdditionalPercent = Phoenix.ApiExtension.Helpers.SecurityHelper.DecryptString(item.AdditionalPercent);
                            item.IsEncrypt = false;

                            context.PhoenixEdit(item);
                        }
                       
                    }
                   

                    var dataPayroll2 = await context.EmployeeSubcomponentDeductions.Where(x => x.IsEncrypt == true && x.IsDeleted == false && x.EmployeeBasicInfoId == employeeId).ToListAsync();
                    foreach (var item in dataPayroll2)
                    {
                        if (item.IsEncrypt ==true)
                        {
                            item.Percent = Phoenix.ApiExtension.Helpers.SecurityHelper.DecryptString(item.Percent);
                            item.Amount = Phoenix.ApiExtension.Helpers.SecurityHelper.DecryptString(item.Amount);
                            item.IsEncrypt = false;

                            context.PhoenixEdit(item);
                        }
                       

                        
                    }
           
                    await context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }

            return save;
        }

        public async Task<int> EncryptPayroll(string employeeId)
        {
            var save = 1;
            using (var transaction = await context.Database.BeginTransactionAsync()) {
                try
                {
                    var dataPayroll1 = await context.EmployeeSubcomponentAdditionals.Where(x => x.IsEncrypt == false && x.IsDeleted == false && x.AdditionalEmployeeBasicInfoId == employeeId).ToListAsync();

                    foreach (var item in dataPayroll1)
                    {
                        if (item.IsEncrypt == false)
                        {
                            item.AdditionalAmount = Phoenix.ApiExtension.Helpers.SecurityHelper.EncryptString(item.AdditionalAmount);
                            item.AdditionalPercent = Phoenix.ApiExtension.Helpers.SecurityHelper.EncryptString(item.AdditionalPercent);
                            context.PhoenixEdit(item);
                        }
                     

                    }

                    var dataPayroll2 = await context.EmployeeSubcomponentDeductions.Where(x => x.IsEncrypt == false && x.IsDeleted == false && x.EmployeeBasicInfoId == employeeId).ToListAsync();
                    foreach (var item in dataPayroll2)
                    {
                        if (item.IsEncrypt == false)
                        {
                            item.Percent = Phoenix.ApiExtension.Helpers.SecurityHelper.EncryptString(item.Percent);
                            item.Amount = Phoenix.ApiExtension.Helpers.SecurityHelper.EncryptString(item.Amount);
                            context.PhoenixEdit(item);
                        }
                       

                      
                    }

                    await context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
              



            return save;

        }

        public async Task<List<EmployeeSubcomponentDeduction>> GetDeductionList(string employeeId)
        {
            var UserSessionId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(context.HttpContext);
            var result = new List<EmployeeSubcomponentDeduction>();

            DateTime dt = DateTime.Now;
            var monthToday = dt.Month;
            var yearToday = dt.Year;

            var declarationPayrollDed= await (from q in context.DeclarationPayrollHeader
                                               join r in context.DeclarationPayrollDetail on q.Id equals r.DeclarationPayrollHeaderId
                                               join s in context.EmployeeBasicInfos on r.EmployeeBasicInfoId equals s.Id
                                               join t in context.DeductionComponents on q.SubComponentId equals t.Id
                                               where q.IsDeleted == false && q.DeclarationType == "Deductional" && s.Id == employeeId
                                               select new EmployeeSubcomponentDeduction()
                                               {
                                                   AmountDes = r.Amount,
                                                   //Amount = r.Amount.ToString(),
                                                   EmployeeBasicInfoId = employeeId,
                                                   ComponentId = q.SubComponentId,
                                                   Percent = "0",
                                                   IsEncrypt = false,
                                                   ComponentTypeName = t.SubComponentName,
                                                   month = q.Month,
                                                   year = q.Year,
                                                   SalaryComponent = SalaryComponentTypeDeduction.Deduction
                                               }
                                               ).ToListAsync();



            List<EmployeeSubcomponentDeduction> dpd = new List<EmployeeSubcomponentDeduction>();

            foreach (var item in declarationPayrollDed)
            {
                if (item.AmountDes != null && item.AmountDes != 0)
                {
                    var amounDes = Convert.ToDecimal(item.AmountDes);
                    item.Amount = amounDes.ToString();
                }
                else
                {
                    item.Amount = "0";
                }
            }
            dpd = declarationPayrollDed.Where(x => x.month == monthToday && x.year == yearToday).ToList();
            if (dpd.Count == 0)
            {
                var getLastMonthYear = declarationPayrollDed.OrderByDescending(x => x.year).OrderByDescending(x => x.month).FirstOrDefault();
                dpd = declarationPayrollDed.Where(x => x.month == getLastMonthYear.month && x.year == getLastMonthYear.year).ToList();
            }

            var deduction = await (from a in context.EmployeeSubcomponentDeductions
                                   join b in context.DeductionComponents on a.ComponentId equals b.Id
                                   where a.SalaryComponent == SalaryComponentTypeDeduction.Deduction &&
                                   !(a.IsDeleted ?? false) && a.EmployeeBasicInfoId == employeeId && !(b.IsDeleted ?? false)
                                   select new EmployeeSubcomponentDeduction
                                   {
                                       Id = a.Id,
                                       EmployeeBasicInfoId = a.EmployeeBasicInfoId,
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
                                       ComponentTypeName = b.SubComponentName,
                                       IsEncrypt = a.IsEncrypt
                                   }).OrderByDescending(x => x.CreatedOn).ToListAsync();

            deduction = deduction.Where(p => !dpd.Any(p2 => p2.ComponentId == p.ComponentId)).ToList();

            var basicSalary = await context.EmployeeSubcomponentAdditionals.Where(x => !(x.IsDeleted ?? false) && x.AdditionalEmployeeBasicInfoId == employeeId && x.AdditionalComponentId == "ab4cf509-88ee-4c35-8083-67e305a22719").FirstOrDefaultAsync();
            var timeSleepSalary = await context.EmployeeSubcomponentAdditionals.Where(x => !(x.IsDeleted ?? false) && x.AdditionalEmployeeBasicInfoId == employeeId && x.AdditionalComponentId == "563e6fb7-dae5-4d9c-be68-1b28f26bb1b7").FirstOrDefaultAsync();

            var declarationPayrollAdd = await (from q in context.DeclarationPayrollHeader
                                               join r in context.DeclarationPayrollDetail on q.Id equals r.DeclarationPayrollHeaderId
                                               join t in context.AllowanceComponents on q.SubComponentId equals t.Id
                                               where q.IsDeleted == false && q.DeclarationType == "Additional" && r.EmployeeBasicInfoId == employeeId
                                               select new EmployeeSubcomponentAdditional()
                                               {
                                                   amountTempDouble = r.Amount,
                                                   //AdditionalAmount = r.Amount.ToString(),
                                                   AdditionalEmployeeBasicInfoId = r.EmployeeBasicInfoId,
                                                   AdditionalComponentId = q.SubComponentId,
                                                   AdditionalPercent = "0",
                                                   IsEncrypt = false,
                                                   AdditionalComponentTypeName = t.SubComponentName,
                                                   month = q.Month,
                                                   year = q.Year,
                                                   AdditionalSalaryComponent = 2
                                               }
                                                 ).ToListAsync();

            List<EmployeeSubcomponentAdditional> dpa = new List<EmployeeSubcomponentAdditional>();

            foreach (var item in declarationPayrollAdd)
            {
                if (item.amountTempDouble != null && item.amountTempDouble != 0)
                {
                    var amounDes = Convert.ToDecimal(item.amountTempDouble);
                    item.AdditionalAmount = amounDes.ToString();
                }
                else
                {
                    item.AdditionalAmount = "0";
                }
            }

            dpa = declarationPayrollAdd.Where(x => x.month == monthToday && x.year == yearToday).ToList();
            if (dpa.Count == 0)
            {
                var getLastMonthYear = declarationPayrollAdd.OrderByDescending(x => x.year).OrderByDescending(x => x.month).FirstOrDefault();
                dpa = declarationPayrollAdd.Where(x => x.month == getLastMonthYear.month && x.year == getLastMonthYear.year).ToList();
            }

            var basicSalaryValue = "0";
            var timeSleepAllowanceValue = "0";

            if (basicSalary != null)
            {
                if (basicSalary.IsEncrypt == true)
                {
                    basicSalaryValue = Phoenix.ApiExtension.Helpers.SecurityHelper.DecryptString(basicSalary.AdditionalAmount);
                }
                else
                {
                    basicSalaryValue = basicSalary.AdditionalAmount;
                }
            }

            if (timeSleepSalary != null)
            {
                if (timeSleepSalary.IsEncrypt == true)
                {
                    timeSleepAllowanceValue = Phoenix.ApiExtension.Helpers.SecurityHelper.DecryptString(timeSleepSalary.AdditionalAmount);
                }
                else
                {
                    timeSleepAllowanceValue = timeSleepSalary.AdditionalAmount;
                }
            }
            else
            {
                var existsTimesleepInDeclaration = dpa.Where(x => x.AdditionalComponentId == "563e6fb7-dae5-4d9c-be68-1b28f26bb1b7" && x.AdditionalEmployeeBasicInfoId == employeeId).FirstOrDefault();
                if (existsTimesleepInDeclaration != null)
                {
                    timeSleepAllowanceValue = dpa.Where(x => x.AdditionalComponentId == "563e6fb7-dae5-4d9c-be68-1b28f26bb1b7" && x.AdditionalEmployeeBasicInfoId == employeeId).Select(x => x.AdditionalAmount).FirstOrDefault();
                }
            }

            var basicSalaryDec = Convert.ToDecimal(basicSalaryValue);
            var timeSleepAllowanceDec = Convert.ToDecimal(timeSleepAllowanceValue);
            var bpjsDefault = await (from q in context.BpjsComponents
                                     where q.Type == BPJSType.Deduction && q.IsDefault == true && !(q.IsDeleted ?? false)
                                     select new EmployeeSubcomponentDeduction()
                                     {
                                         //Amount = (((basicSalaryDec + timeSleepAllowanceDec) * q.Value) / 100).ToString(),
                                         Amount = ((basicSalaryDec * q.Value) / 100).ToString(),
                                         EmployeeBasicInfoId = employeeId,
                                         ComponentId = q.Id,
                                         Percent = q.Value.ToString(),
                                         IsEncrypt = false,
                                         ComponentTypeName = q.SubComponentName,
                                         SalaryComponent = SalaryComponentTypeDeduction.BPJS
                                     }
                                     ).ToListAsync();

            var bpjs = await (from a in context.EmployeeSubcomponentDeductions
                              join b in context.BpjsComponents on a.ComponentId equals b.Id
                              where a.SalaryComponent == SalaryComponentTypeDeduction.BPJS && b.Type == BPJSType.Deduction &&
                              !(a.IsDeleted ?? false) && a.EmployeeBasicInfoId == employeeId && !(b.IsDeleted ?? false)
                              select new EmployeeSubcomponentDeduction
                              {
                                  Id = a.Id,
                                  EmployeeBasicInfoId = a.EmployeeBasicInfoId,
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
                                  ComponentTypeName = b.SubComponentName,
                                  IsEncrypt = a.IsEncrypt
                              }).OrderByDescending(x => x.CreatedOn).ToListAsync();

            bpjsDefault = bpjsDefault.Where(p => !bpjs.Any(p2 => p2.ComponentId == p.ComponentId)).ToList();


            var query = await context.UserEmployeePayrolls.Where(x => x.EmployeeBasicInfoId == UserSessionId && x.MenuUnique == MenuUnique.EmployeePayroll && x.IsDeleted == false).AsNoTracking().FirstOrDefaultAsync();
            if (query != null)
            {
                foreach (var item in deduction)
                {
                    if (item.IsEncrypt == true)
                    {
                        item.Percent = Phoenix.ApiExtension.Helpers.SecurityHelper.DecryptString(item.Percent);
                        item.Amount = Phoenix.ApiExtension.Helpers.SecurityHelper.DecryptString(item.Amount);
                    }
                }

                foreach (var item in bpjs)
                {
                    if (item.IsEncrypt == true)
                    {
                        item.Percent = Phoenix.ApiExtension.Helpers.SecurityHelper.DecryptString(item.Percent);
                        item.Amount = Phoenix.ApiExtension.Helpers.SecurityHelper.DecryptString(item.Amount);
                    }
                }
            }
            else
            {
                foreach (var item in deduction)
                {
                    item.Percent = "0";
                    item.Amount = "0";
                }

                foreach (var item in bpjs)
                {
                    item.Percent = "0";
                    item.Amount = "0";
                }
            }
               
            result.AddRange(deduction);
            //result.AddRange(dpd);
            result.AddRange(bpjsDefault);
            result.AddRange(bpjs);
            return result;
        }

        public async Task<EmployeeHistoryPayroll> HistoryPayroll(string employeeId)
        {
            EmployeeHistoryPayroll result = new EmployeeHistoryPayroll();
            List<EmployeeHistoryPayrollHeader> header = new List<EmployeeHistoryPayrollHeader>();
            List<EmployeeHistoryPayrollDetail> detail = new List<EmployeeHistoryPayrollDetail>();

            var paramType = new SqlParameter[] {
                                new SqlParameter("@employeeid", employeeId)
                            };

            var ds = await context.ExecuteStoredProcedureDataSet("usp_history_payroll_employee", paramType);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                EmployeeHistoryPayrollHeader newObj = new EmployeeHistoryPayrollHeader();
                newObj.month_name = dr["month_name"].ToString();
                newObj.month = dr["month"].ToString();
                newObj.year = dr["year"].ToString();
                header.Add(newObj);
            }

            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                EmployeeHistoryPayrollDetail newObj = new EmployeeHistoryPayrollDetail();
                newObj.month_name = dr["month_name"].ToString();
                newObj.month = dr["month"].ToString();
                newObj.year = dr["year"].ToString();
                newObj.Basic_Salary = dr["Basic_Salary"].ToString();
                newObj.name_employee = dr["name_employee"].ToString();
                newObj.Legal_Entity = dr["Legal_Entity"].ToString();
                newObj.Division = dr["Division"].ToString();
                newObj.Join_Date = dr["Join_Date"].ToString();
                newObj.Status_Pph21 = dr["Status_Pph21"].ToString();
                newObj.Bonus = dr["Bonus"].ToString();
                newObj.THR = dr["THR"].ToString();
                newObj.Jaminan_Pensiun_BPJS = dr["Jaminan_Pensiun_BPJS"].ToString();
                newObj.Jamsostek_BPJS_TK_addition = dr["Jamsostek_BPJS_TK_addition"].ToString();
                newObj.Overtime = dr["Overtime"].ToString();
                newObj.Phone_Allowance = dr["Phone_Allowance"].ToString();
                newObj.Tambahan_Rapel = dr["Tambahan_Rapel"].ToString();
                newObj.THR_Bonus = dr["THR_Bonus"].ToString();
                newObj.Timesleep_Allowance = dr["Timesleep_Allowance"].ToString();
                newObj.Transportation_Allowance = dr["Transportation_Allowance"].ToString();
                newObj.Tunjangan_Jabatan = dr["Tunjangan_Jabatan"].ToString();
                newObj.BPJS_KS_Corp = dr["BPJS_KS_Corp"].ToString();
                newObj.Jamsostek_BPJS_TK_deduction = dr["Jamsostek_BPJS_TK_deduction"].ToString();
                newObj.Iuran_sukarela_Koperasi_Nava = dr["Iuran_sukarela_Koperasi_Nava"].ToString();
                newObj.Iuran_Wajib_Koperasi_Nava = dr["Iuran_Wajib_Koperasi_Nava"].ToString();
                newObj.Potongan_Jamsostek = dr["Potongan_Jamsostek"].ToString();
                newObj.Potongan_Pinjaman_Koperasi = dr["Potongan_Pinjaman_Koperasi"].ToString();
                newObj.BPJS_KS_Emp = dr["BPJS_KS_Emp"].ToString();
                newObj.BPJS_TK_Emp = dr["BPJS_TK_Emp"].ToString();
                newObj.Pot_Jaminan_Pensiun_BPJS = dr["Pot_Jaminan_Pensiun_BPJS"].ToString();
                newObj.Loan = dr["Loan"].ToString();
                newObj.Pph_21 = dr["Pph_21"].ToString();
                newObj.THP = dr["THP"].ToString();
                newObj.potongan_bpjs_2_persen = dr["potongan_bpjs_2_persen"].ToString();
                newObj.potongan_lain_lain = dr["potongan_lain_lain"].ToString();
                newObj.potongan_bpjs_6_persen = dr["potongan_bpjs_6_persen"].ToString();
                newObj.housing_allowance = dr["housing_allowance"].ToString();
                newObj.job_allowance = dr["job_allowance"].ToString();
                newObj.Gaji_Kotor = dr["Gaji_Kotor"].ToString();
                detail.Add(newObj);
            }

            result.Header = header;
            result.Detail = detail;

            return result;
        }

        public async Task<pph21> getpph21(string ptkpId, string employeeId)
        {
            pph21 resp = new pph21();
            var result = "0";
            var emp = await context.EmployeeBasicInfos.Where(x => x.Id == employeeId).FirstOrDefaultAsync();
            if (emp != null)
            {
                emp.PtkpSettingId = ptkpId;
                await context.SaveChangesAsync();
            }
            var basicSalary = await context.EmployeeSubcomponentAdditionals.Where(x => !(x.IsDeleted ?? false) && x.AdditionalEmployeeBasicInfoId == employeeId && x.AdditionalComponentId == "ab4cf509-88ee-4c35-8083-67e305a22719").FirstOrDefaultAsync();
            var basicSalaryValue = "0";
            if (basicSalary != null)
            {
                if (basicSalary.IsEncrypt == true)
                {
                    basicSalaryValue = Phoenix.ApiExtension.Helpers.SecurityHelper.DecryptString(basicSalary.AdditionalAmount);
                }
                else
                {
                    basicSalaryValue = basicSalary.AdditionalAmount;
                }
            }

            if (basicSalaryValue != "0")
            {
                DecryptPayroll(employeeId);
                var month = DateTime.Now.Month;
                var year = DateTime.Now.Year;
                var paramType = new SqlParameter[] {
                                new SqlParameter("@employeeid", employeeId),
                                new SqlParameter("@month", month),
                                new SqlParameter("@year", year)
                            };

                var ds = await context.ExecuteStoredProcedureDataSet("uspCalculatePph21", paramType);
                var que = string.Format("select * from hr.salary_history where employee_basic_info_id = '{0}' and month = {1} and year = {2}", employeeId, month, year);
                var dt = await context.GetDataTable(que,null);
                if (dt.Rows.Count > 0)
                {
                    var pph21 = dt.Rows[0]["pph21"].ToString();
                    if (!string.IsNullOrEmpty(pph21))
                    {
                        result = dt.Rows[0]["pph21"].ToString();
                    }
                    else
                    {
                        result = "0";
                    }
                }
                
            }
            else
            {
                result = "0";
            }
            resp.result = result;

            return resp;
        }
    }
}

public class pph21
{
    public string result { get; set; }
}