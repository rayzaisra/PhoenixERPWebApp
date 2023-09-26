using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Um;
using Phoenix.Data.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Um
{

    public class ManageUserAppService : IManageUserAppService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage SalaryComponent
        /// </summary>
        /// <param name="context">Database context</param>
        public ManageUserAppService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(TmUserApp entity)
        {
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params TmUserApp[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(TmUserApp entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params TmUserApp[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(TmUserApp entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params TmUserApp[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(TmUserApp entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(TmUserApp entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<TmUserApp>> GetBySearch(string key)
        {
            char[] charsToTrim = { ' ' };
            return await (from x in context.TmUserApps.Where(c => c.IsDeleted.Equals(false))
                          join g in context.TmGroups on x.GroupId equals g.Id into gx
                          from ug in gx.DefaultIfEmpty()
                          select new TmUserApp
                          {
                              Id = x.Id.Trim(charsToTrim),
                              AliasName = x.AliasName,
                              GroupId = x.GroupId,
                              GroupName = ug.GroupName,
                              Email = x.Email,
                              IsMsUser = x.IsMsUser,
                              IsUserNeedTimesheet = x.IsUserNeedTimesheet,
                              IsLocked = x.IsLocked,
                              EmployeeBasicInfoId = x.EmployeeBasicInfoId
                          }).ToListAsync();
        }
        public async Task<List<TmUserApp>> GetUserForDoa(string userId)
        {
            return await (from x in context.TmUserApps.Where(c => c.IsDeleted.Equals(false) && c.Id != userId)
                          select new TmUserApp
                          {
                              Id = x.Id,
                              AliasName = x.AliasName,
                              Email = x.Email,
                              EmployeeBasicInfoId = x.EmployeeBasicInfoId
                          }).ToListAsync();
        }

        public async Task<List<TmUserApp>> GetListUserByGroupId(string groupId)
        {
            return await (from x in context.TmUserApps.Where(c => c.IsDeleted.Equals(false) && c.GroupId.Equals(groupId))
                          select new TmUserApp
                          {
                              Id = x.Id,
                              AliasName = x.AliasName
                          }).ToListAsync();
        }
        public async Task<List<TmUserApp>> Get() => await context.TmUserApps.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).ToListAsync();

        public Task<List<TmUserApp>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<TmUserApp> Get(string Id) {
            var query = await context.TmUserApps.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id.Trim() == Id).AsNoTracking().FirstOrDefaultAsync();
            return query;
        }
        public async Task<TmUserApp> GetByEmployeeBasicInfoId(string Id) => await context.TmUserApps.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.EmployeeBasicInfoId == Id).AsNoTracking().FirstOrDefaultAsync();
        public async Task<TmUserApp> CekIdIsReady(string Id) => await context.TmUserApps.AsNoTracking().Where(x => x.Id == Id).AsNoTracking().FirstOrDefaultAsync();

        public async Task<List<EmployeeBasicInfo>> GetDataEmployee(string key)
        {
            //var listEmp = GetDataEmployeeMappingDivision(key);
            var data = await (from x in context.EmployeeBasicInfos.Where(c => c.NameEmployee.Contains(key) || c.Email.Contains(key))
                              select new EmployeeBasicInfo
                              {
                                  Id = x.Id,
                                  NameEmployee = x.NameEmployee,
                                  Email = x.Email,
                                  Gender = x.Gender,
                                  IsDeleted = x.IsDeleted
                              }).Where(x => x.IsDeleted == false).ToListAsync();
            return data;
        }

        public async Task<List<EmployeeBasicInfo>> GetDataEmployeeSortByName(string key)
        {
            //var listEmp = GetDataEmployeeMappingDivision(key);
            var data = await (from x in context.EmployeeBasicInfos.Where(c => c.NameEmployee.Contains(key) || c.Email.Contains(key))
                              select new EmployeeBasicInfo
                              {
                                  Id = x.Id,
                                  NameEmployee = x.NameEmployee,
                                  Email = x.Email,
                                  Gender = x.Gender,
                                  IsDeleted = x.IsDeleted
                              }).Where(x => x.IsDeleted == false).OrderBy(x=>x.NameEmployee).ToListAsync();
            return data;
        }

        public List<EmployeeMappingDivisionViewModel> GetEmployeeAndUserAppNotMapGroup(string key)
        {
            var sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@Key",
                    SqlDbType.NVarChar) {Value = key}
            };
            var resultSet = ExecuteStoredProcedure<EmployeeMappingDivisionViewModel>("dbo.uspGetEmployeeAndUserAppNotMapGroup", sqlParameters);
            return resultSet;
        }

        public async Task<BusinessUnitJobLevel> GetBusinessUnitJobLevel(string empId)
        {
            var data = await (from x in context.BusinessUnitJobLevels.Where(x => x.IsDeleted == false)
                              join e in context.EmployeeBasicInfos.Where(x => x.Id.Equals(empId) && x.IsDeleted == false) on x.Id equals e.BusinessUnitJobLevelId
                              select new BusinessUnitJobLevel
                              {
                                  Id = x.Id,
                                  BusinessUnitGroupId = x.BusinessUnitGroupId,
                                  BusinessUnitSubgroupId = x.BusinessUnitSubgroupId,
                                  BusinessUnitDivisionId = x.BusinessUnitDivisionId,
                                  BusinessUnitDepartementId = x.BusinessUnitDepartementId,
                                  JobTitleId = x.JobTitleId
                              }
                              ).FirstOrDefaultAsync();
            return data;
        }

        public List<T> ExecuteStoredProcedure<T>(string storedProcedure,
              List<SqlParameter> parameters) where T : new()
        {
            using (var cmd =
               context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = storedProcedure;
                cmd.CommandType = CommandType.StoredProcedure;

                // set some parameters of the stored procedure
                foreach (var parameter in parameters)
                {
                    cmd.Parameters.Add(parameter);
                }

                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                using (var dataReader = cmd.ExecuteReader())
                {
                    var test = DataReaderMapToList<T>(dataReader);
                    return test;
                }
            }
        }

        private static List<T> DataReaderMapToList<T>(DbDataReader dr)
        {
            List<T> list = new List<T>();
            while (dr.Read())
            {
                var obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    if (!Equals(dr[prop.Name], DBNull.Value))
                    {
                        prop.SetValue(obj, dr[prop.Name], null);
                    }
                }
                list.Add(obj);
            }
            return list;
        }

        public async Task<List<TmUserApp>> GetByUserLogin()
        {
            var employeeId = context.GetEmployeeId();
            return await context.TmUserApps.Where(x => x.EmployeeBasicInfoId == employeeId).ToListAsync();
        }

        public async Task<string> GetUserIsLocked(string empId)
        {
            SqlParameter[] parameter = {
            new SqlParameter("@UserId", empId)
            };
            DataTable result = await context.ExecuteStoredProcedure("dbo.uspHasPastTimeSheetUser", parameter);
            var resp = result.Rows[0]["result"].ToString();
            return resp;
        }

        public async Task<bool> IsAllowOpenEncryptEmployeePayroll(string empId, string MenuUnique)
        {
            var result = false;
            var dataEmployee = await context.TmUserApps.Where(x => x.Id == empId).FirstOrDefaultAsync();
            var query = await context.UserEmployeePayrolls.Where(x => x.EmployeeBasicInfoId == dataEmployee.EmployeeBasicInfoId && x.MenuUnique == MenuUnique && x.IsDeleted == false).FirstOrDefaultAsync();
            if (query != null)
            {
                result = true;
            }

            return result;
        }

        public async Task<bool> InsertLog(string message)
        {
            var result = false;
            try
            {
                var newId = Guid.NewGuid().ToString();
                TransactionLog data = new TransactionLog()
                {
                    Route = message,
                    Id = Guid.NewGuid()
                };
                context.TransactionLogs.Add(data);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                var newId = Guid.NewGuid().ToString();
                AuditTrails data = new AuditTrails()
                {
                    OLD_VALUE = message,
                    NEW_VALUE = message
                };
                context.AuditTrails.Add(data);
                context.SaveChanges();
            }
           
            return true;
            
        }
    }
}
