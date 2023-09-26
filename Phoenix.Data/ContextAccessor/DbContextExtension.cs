using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Phoenix.Shared.Core.Contexts;
using Phoenix.Shared.Core.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Data
{
    public static class DbContextExtension
    {
        public static ClaimsPrincipal GetPrincipal(this DataContext context)
        {
            if (context.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues value))
            {
                var bearer = value.ToString().Replace("bearer ", "").Replace("Bearer ", "");
                return JwtManager.GetPrincipal(bearer);
            }
            return null;
        }

        public static string GetUserId(this DataContext context)
        {
            var principal = context.GetPrincipal();
            if (principal != null)
            {
                var claim = principal.Claims.FirstOrDefault(x => x.Type == ClaimsIdentity.DefaultNameClaimType);
                return claim.Value;
            }

            return null;
        }
        public static string GetEmployeeId(this DataContext context)
        {
            var principal = context.GetPrincipal();
            if (principal != null)
            {
                var SessionContext = JsonConvert.DeserializeObject<UserSession>(principal.Claims.ToList()[1].Value);
                if (SessionContext.EmployeeId == "IU82896372460210900")
                    SessionContext.EmployeeId = "1187584580417359872";
                return SessionContext.EmployeeId;
            }

            return null;
        }
        public static string GetSubGroupId(this DataContext context)
        {
            var principal = context.GetPrincipal();
            if (principal != null)
            {
                var SessionContext = JsonConvert.DeserializeObject<UserSession>(principal.Claims.ToList()[1].Value);
                return SessionContext.BusinessSubGroup;
            }

            return null;
        }
        public static string GetDivisionId(this DataContext context)
        {
            var principal = context.GetPrincipal();
            if (principal != null)
            {
                var SessionContext = JsonConvert.DeserializeObject<UserSession>(principal.Claims.ToList()[1].Value);
                return SessionContext.BusinessUnitDivisi;
            }

            return null;
        }
        public static string GetDepartmentId(this DataContext context)
        {
            var principal = context.GetPrincipal();
            if (principal != null)
            {
                var SessionContext = JsonConvert.DeserializeObject<UserSession>(principal.Claims.ToList()[1].Value);
                return SessionContext.BusinessUnitDepartment;
            }

            return null;
        }

        public static string GetSubGroupAccess(this DataContext context)
        {
            var principal = context.GetPrincipal();
            if (principal != null)
            {
                var SessionContext = JsonConvert.DeserializeObject<UserSession>(principal.Claims.ToList()[1].Value);
                return SessionContext.GroupAccess;
            }

            return null;
        }

        private static void SetValue<TEntity>(TEntity entity, string propName, object value)
        {
            var propertyInfo = entity.GetType()
                .GetProperty(propName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfo != null)
                propertyInfo.SetValue(entity, value, null);
        }

        public static Task<EntityEntry<TEntity>> PhoenixAddAsync<TEntity>(this DataContext context, TEntity entity) where TEntity : class
        {
            SetValue(entity, "CreatedBy", GetEmployeeId(context));
            SetValue(entity, "CreatedOn", DateTime.Now);
            SetValue(entity, "ModifiedBy", GetEmployeeId(context));
            SetValue(entity, "ModifiedOn", DateTime.Now);
            SetValue(entity, "SubGroupId", GetSubGroupId(context));
            SetValue(entity, "DivisionId", GetDivisionId(context));
            SetValue(entity, "DepartmentId", GetDepartmentId(context));
            SetValue(entity, "IsDeleted", false);

            return context.Set<TEntity>().AddAsync(entity);
        }

        public static Task<EntityEntry<TEntity>> PhoenixAddLiteAsync<TEntity>(this DataContext context, TEntity entity) where TEntity : class
        {
            SetValue(entity, "CreatedBy", GetEmployeeId(context));
            SetValue(entity, "CreatedOn", DateTime.Now);
            SetValue(entity, "ModifiedBy", GetEmployeeId(context));
            SetValue(entity, "ModifiedOn", DateTime.Now);
            SetValue(entity, "IsDeleted", false);
            return context.Set<TEntity>().AddAsync(entity);
        }


        //public static void PhoenixAddNotSaveAsync<T>(this DataContext context, T model) where T : class
        //{
        //    SetValue(model, "CreatedBy", GetEmployeeId(context));
        //    SetValue(model, "CreatedOn", DateTime.Now);
        //    SetValue(model, "ModifiedBy", GetEmployeeId(context));
        //    SetValue(model, "ModifiedOn", DateTime.Now);
        //    SetValue(model, "SubGroupId", GetSubGroupId(context));
        //    SetValue(model, "DivisionId", GetDivisionId(context));
        //    SetValue(model, "DepartmentId", GetDepartmentId(context));
        //    SetValue(model, "IsDeleted", false);
        //    //return model;
        //}

        public static Task<EntityEntry<TEntity>> PhoenixAddApprovalAsync<TEntity>(this DataContext context, TEntity entity) where TEntity : class
        {
            SetValue(entity, "CreatedBy", GetEmployeeId(context));
            SetValue(entity, "CreatedOn", DateTime.Now);
            SetValue(entity, "IsDeleted", false);
            SetValue(entity, "SubGroupId", GetSubGroupId(context));
            SetValue(entity, "DivisionId", GetDivisionId(context));
            return context.Set<TEntity>().AddAsync(entity);
        }
        public static void PhoenixAddRange<TEntity>(this DataContext context, params TEntity[] entities) where TEntity : class
        {
            foreach (var entity in entities)
            {
                SetValue(entity, "CreatedBy", GetEmployeeId(context));
                SetValue(entity, "CreatedOn", DateTime.Now);
                SetValue(entity, "ModifiedBy", GetEmployeeId(context));
                SetValue(entity, "ModifiedOn", DateTime.Now);
                SetValue(entity, "SubGroupId", GetSubGroupId(context));
                SetValue(entity, "DivisionId", GetDivisionId(context));
                SetValue(entity, "DepartmentId", GetDepartmentId(context));
                SetValue(entity, "IsDeleted", false);
            }

            context.Set<TEntity>().AddRange(entities);
        }

        public static EntityEntry<TEntity> PhoenixAdd<TEntity>(this DataContext context, TEntity entity) where TEntity : class
        {
            SetValue(entity, "CreatedBy", GetEmployeeId(context));
            SetValue(entity, "CreatedOn", DateTime.Now);
            SetValue(entity, "ModifiedBy", GetEmployeeId(context));
            SetValue(entity, "ModifiedOn", DateTime.Now);
            SetValue(entity, "SubGroupId", GetSubGroupId(context));
            SetValue(entity, "DivisionId", GetDivisionId(context));
            SetValue(entity, "DepartmentId", GetDepartmentId(context));
            SetValue(entity, "IsDeleted", false);

            return context.Set<TEntity>().Add(entity);
        }
        public static EntityEntry<TEntity> PhoenixAddApproval<TEntity>(this DataContext context, TEntity entity) where TEntity : class
        {
            SetValue(entity, "SubGroupId", GetSubGroupId(context));
            SetValue(entity, "DivisionId", GetDivisionId(context));
            return context.Set<TEntity>().Add(entity);
        }
        public static Task PhoenixAddRangeAsync<TEntity>(this DataContext context, params TEntity[] entities) where TEntity : class
        {
            foreach (var entity in entities)
            {
                SetValue(entity, "CreatedBy", GetEmployeeId(context));
                SetValue(entity, "CreatedOn", DateTime.Now);
                SetValue(entity, "ModifiedBy", GetEmployeeId(context));
                SetValue(entity, "ModifiedOn", DateTime.Now);
                SetValue(entity, "SubGroupId", GetSubGroupId(context));
                SetValue(entity, "DivisionId", GetDivisionId(context));
                SetValue(entity, "DepartmentId", GetDepartmentId(context));
                SetValue(entity, "IsDeleted", false);
            }

            return context.Set<TEntity>().AddRangeAsync(entities);
        }

        public static EntityEntry<TEntity> PhoenixEdit<TEntity>(this DataContext context, TEntity entity) where TEntity : class
        {
            SetValue(entity, "ModifiedBy", GetEmployeeId(context));
            SetValue(entity, "ModifiedOn", DateTime.Now);
            SetValue(entity, "SubGroupId", GetSubGroupId(context));
            SetValue(entity, "DivisionId", GetDivisionId(context));
            SetValue(entity, "DepartmentId", GetDepartmentId(context));
            SetValue(entity, "IsDeleted", false);

            return context.Set<TEntity>().Update(entity);
        }
        public static EntityEntry<TEntity> PhoenixEditLite<TEntity>(this DataContext context, TEntity entity) where TEntity : class
        {
            SetValue(entity, "ModifiedBy", GetEmployeeId(context));
            SetValue(entity, "ModifiedOn", DateTime.Now);
            SetValue(entity, "IsDeleted", false);

            return context.Set<TEntity>().Update(entity);
        }
        public static EntityEntry<TEntity> PhoenixEditApproval<TEntity>(this DataContext context, TEntity entity) where TEntity : class
        {
            SetValue(entity, "SubGroupId", GetSubGroupId(context));
            SetValue(entity, "DivisionId", GetDivisionId(context));
            return context.Set<TEntity>().Update(entity);
        }
        public static void PhoenixEditRange<TEntity>(this DataContext context, params TEntity[] entities) where TEntity : class
        {
            foreach (var entity in entities)
            {
                SetValue(entity, "ModifiedBy", GetEmployeeId(context));
                SetValue(entity, "ModifiedOn", DateTime.Now);
                SetValue(entity, "SubGroupId", GetSubGroupId(context));
                SetValue(entity, "DivisionId", GetDivisionId(context));
                SetValue(entity, "DepartmentId", GetDepartmentId(context));
                SetValue(entity, "IsDeleted", false);
            }

            context.Set<TEntity>().UpdateRange(entities);
        }

        public static EntityEntry<TEntity> PhoenixApprove<TEntity>(this DataContext context, TEntity entity) where TEntity : class
        {
            SetValue(entity, "ModifiedBy", GetEmployeeId(context));
            SetValue(entity, "ModifiedOn", DateTime.Now);
            SetValue(entity, "ApprovedBy", GetEmployeeId(context));
            SetValue(entity, "ApprovedOn", DateTime.Now);
            SetValue(entity, "SubGroupId", GetSubGroupId(context));
            SetValue(entity, "DivisionId", GetDivisionId(context));
            SetValue(entity, "DepartmentId", GetDepartmentId(context));
            SetValue(entity, "IsDeleted", false);

            return context.Set<TEntity>().Update(entity);
        }

        public static void PhoenixApproveRange<TEntity>(this DataContext context, params TEntity[] entities) where TEntity : class
        {
            foreach (var entity in entities)
            {
                SetValue(entity, "ModifiedBy", GetEmployeeId(context));
                SetValue(entity, "ModifiedOn", DateTime.Now);
                SetValue(entity, "ApprovedBy", GetEmployeeId(context));
                SetValue(entity, "ApprovedOn", DateTime.Now);
                SetValue(entity, "SubGroupId", GetSubGroupId(context));
                SetValue(entity, "DivisionId", GetDivisionId(context));
                SetValue(entity, "DepartmentId", GetDepartmentId(context));
                SetValue(entity, "IsDeleted", false);
            }

            context.Set<TEntity>().UpdateRange(entities);
        }

        public static EntityEntry<TEntity> PhoenixDelete<TEntity>(this DataContext context, TEntity entity) where TEntity : class
        {
            SetValue(entity, "DeletedBy", GetEmployeeId(context));
            SetValue(entity, "DeletedOn", DateTime.Now);
            SetValue(entity, "IsDeleted", true);

            return context.Set<TEntity>().Update(entity);
        }

        public static void PhoenixDeleteRange<TEntity>(this DataContext context, params TEntity[] entities) where TEntity : class
        {
            foreach (var entity in entities)
            {
                SetValue(entity, "DeletedBy", GetEmployeeId(context));
                SetValue(entity, "DeletedOn", DateTime.Now);
                SetValue(entity, "IsDeleted", true);
            }

            context.Set<TEntity>().UpdateRange(entities);
        }

        public static async Task<DataTable> GetDataTable(this DataContext context, string commandText, params SqlParameter[] parameters)
        {
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = commandText;
                if (parameters?.Length > 0)
                    command.Parameters.AddRange(parameters);

                await context.Database.OpenConnectionAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    var table = new DataTable();
                    table.Load(reader);
                    return table;
                }
            }
        }

        public static async Task<List<T>> ExecuteStoredProcedure<T>(this DataContext context, string storedProcedure, SqlParameter[] parameters) where T : new()
        {
            using (var cmd = context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = storedProcedure;
                cmd.CommandType = CommandType.StoredProcedure;

                // set some parameters of the stored procedure
                if (parameters?.Length > 0)
                {
                    foreach (var parameter in parameters)
                    {
                        cmd.Parameters.Add(parameter);
                    }
                }
                   

                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                using (var dataReader = await cmd.ExecuteReaderAsync())
                {
                    var test = await DataReaderMapToList<T>(dataReader);
                    return test;
                }
            }
        }
        public static async Task<List<T>> ExecuteStoredProcedure<T>(this DataContext context, string storedProcedure) where T : new()
        {
            using (var cmd = context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = storedProcedure;
                cmd.CommandType = CommandType.StoredProcedure;
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                using (var dataReader = await cmd.ExecuteReaderAsync())
                {
                    var test = await DataReaderMapToList<T>(dataReader);
                    return test;
                }
            }
        }

        private static async Task<List<T>> DataReaderMapToList<T>(DbDataReader dr)
        {
            List<T> list = new List<T>();
            while (await dr.ReadAsync())
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

        public static async Task<DataTable> ExecuteStoredProcedure(this DataContext context, string storedProcedure, SqlParameter[] parameters)
        {
            using (var cmd = context.Database.GetDbConnection().CreateCommand())
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

                using (var dataReader = await cmd.ExecuteReaderAsync())
                {
                    return DataReaderMapToDataTable(dataReader);
                }
            }
        }

        public static async Task<DataTable> ExecuteStoredProcedure(this DataContext context, string storedProcedure)
        {
            using (var cmd = context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = storedProcedure;
                cmd.CommandType = CommandType.StoredProcedure;
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                using (var dataReader = await cmd.ExecuteReaderAsync())
                {
                    return DataReaderMapToDataTable(dataReader);
                }
            }
        }

        public static async Task<DataSet> ExecuteStoredProcedureDataSet(this DataContext context, string storedProcedure, SqlParameter[] parameters)
        {
            using (var cmd = context.Database.GetDbConnection().CreateCommand())
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

                using (var dataReader = await cmd.ExecuteReaderAsync())
                {
                    return DataReaderMapToDataSet(dataReader);
                }
            }
        }

        public static async Task<DataSet> ExecuteStoredProcedureDataSet(this DataContext context, string storedProcedure)
        {
            using (var cmd = context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = storedProcedure;
                cmd.CommandType = CommandType.StoredProcedure;
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                using (var dataReader = await cmd.ExecuteReaderAsync())
                {
                    return DataReaderMapToDataSet(dataReader);
                }
            }
        }

        private static DataTable DataReaderMapToDataTable(DbDataReader reader)
        {
            DataTable list = new DataTable();
            while (!reader.IsClosed)
            {
                list.Load(reader);
            }
            return list;
        }

        private static DataSet DataReaderMapToDataSet(DbDataReader reader)
        {
            DataSet list = new DataSet();
            while (!reader.IsClosed)
            {
                list.Tables.Add().Load(reader);
            }
            return list;
        }

        public static async Task<List<T>> ExecuteByQuery<T>(this DataContext context, string Query) where T : new()
        {
            using (var cmd = context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = Query;
                cmd.CommandType = CommandType.Text;
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                using (var dataReader = await cmd.ExecuteReaderAsync())
                {
                    var test = await DataReaderMapToList<T>(dataReader);
                    return test;
                }
            }
        }
    }
}
