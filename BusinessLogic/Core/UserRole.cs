using BusinessLogic.Services.General;
using DataAccess.PhoenixERP;
using DataAccess.PhoenixERP.General;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using NLog;
using NPoco;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace BusinessLogic.Core
{
    public partial class UserRole : UserRoleService
    {
        private readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public UserRole(DataContext AppUserData) : base(AppUserData)
        {
        }


        public DataTablesResponseNetCore GetListUserRoleByRoleId(IDataTablesRequest DTRequest, string RoleId)
        {
            DataTablesResponseNetCore result = null;
            string OptionalFilters = string.Empty;
            string OptionalParameters = string.Empty;
            string OptionalQueryBuilder = string.Empty;

            try
            {
                OptionalFilters = " r.application_role_id = @0 ";
                OptionalParameters = RoleId;
                result = Context.GetListDataTablesMarvelBinder<vw_user_role>(DTRequest,
                    OptionalFilters, OptionalParameters, OptionalQueryBuilder);
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }

            return result;
        }

        public dynamic kendoLookupAssignableRoleMember(CommonTool.KendoUI.Combobox.ComboboxDataRequest dtRequest, String userId)
        {
            dynamic result = null;
            if (dtRequest != null)
            {
                var sql = Sql.Builder.Append(application_role.DefaultView);
                sql.Append(" WHERE r.is_active = 1 AND COALESCE(r.is_deleted,0)=0 ");
                //sql.Append(String.Format(" AND r.id NOT IN ({0})",
                //     " SELECT r.application_role_id FROM dbo.user_role r "
                //    + " WHERE r.is_active = 1 AND COALESCE(r.is_deleted, 0) = 0 "
                //    + " AND r.application_user_id = @0"
                //    ), userId);

                sql.Append(" ORDER BY r.role_name");

                using (var db = new PhoenixERPRepo())
                {
                    try
                    {
                        var notAssign = db.Fetch<vw_application_role>(sql);

                        sql = Sql.Builder.Append(user_role.DefaultView);
                        sql.Append(" WHERE r.is_active = 1 AND COALESCE(r.is_deleted,0)=0 ");
                        sql.Append(" AND r.application_user_id=@0 ", userId);

                        var hasAssign = db.Fetch<vw_user_role>(sql);
                        dynamic resultValidate = new ExpandoObject();
                        resultValidate = from r in notAssign
                                         join h in hasAssign on r.id equals h.application_role_id into ps
                                         from h in ps.DefaultIfEmpty()
                                         select new
                                         {
                                             r.id,
                                             r.role_name,
                                             vacant = h == null ? true : false
                                         };
                        result = new
                        {
                            items = resultValidate
                        };

                    }
                    catch (Exception ex)
                    {
                        appLogger.Error(ex);
                    }
                }
            }
            return result;
        }

        public dynamic kendoLookupAssignableUserMember(CommonTool.KendoUI.Combobox.ComboboxDataRequest dtRequest, String roleId)
        {
            dynamic result = null;
            if (dtRequest != null)
            {
                var sql = Sql.Builder.Append(application_user.DefaultView);
                sql.Append(" WHERE r.is_active = 1 ");

                #region Use filters
                if (dtRequest.Filter2 != null)
                {
                    Sql filterBuilder = Context.MultiplekendoFilterable<vw_application_user>(dtRequest.Filter2);
                    if (filterBuilder != null)
                        sql.Append(filterBuilder.SQL, filterBuilder.Arguments);
                }
                #endregion


                sql.Append(" ORDER BY r.app_fullname ");

                using (var db = new PhoenixERPRepo())
                {
                    try
                    {
                        int pageDraw = dtRequest.Page;
                        int rowPerPage = dtRequest.PageSize;

                        //var notassign = db.Page<dynamic>(pageDraw, rowPerPage, sql); 
                        var notassign = db.Fetch<vw_application_user>(sql);
                        dynamic resultValidate = new ExpandoObject();
                        if (!string.IsNullOrEmpty(roleId))
                        {
                            var sqlAssigned = Sql.Builder.Append(user_role.DefaultView);
                            sqlAssigned.Append(" WHERE r.is_active = 1 AND COALESCE(r.is_deleted,0)=0");
                            sqlAssigned.Append(" AND r.application_role_id=@0 ", roleId);
                            sqlAssigned.Append(" ORDER BY e1.app_fullname");
                            var hasAssign = db.Fetch<vw_user_role>(sqlAssigned);
                            resultValidate = from r in notassign
                                             join h in hasAssign on r.id equals h.application_user_id into ps
                                             from h in ps.DefaultIfEmpty()
                                             select new
                                             {
                                                 r.id,
                                                 r.app_username,
                                                 r.app_fullname,
                                                 r.email,
                                                 r.primary_team,
                                                 vacant = h == null ? true : false
                                             };
                        }
                        else
                        {
                            resultValidate = from r in notassign
                                             select new
                                             {
                                                 r.id,
                                                 r.app_username,
                                                 r.app_fullname,
                                                 r.email,
                                                 r.primary_team,
                                                 vacant = true
                                             };
                        }

                        result = new
                        {
                            items = resultValidate
                        };
                    }
                    catch (Exception ex)
                    {
                        appLogger.Error(ex);
                    }
                    finally
                    {
                        appLogger.Debug(db.LastCommand);
                    }
                }
            }
            return result;
        }

        public DataTablesResponseNetCore GetAssignableUserMember(IDataTablesRequest DTRequest, String RoleId)
        {
            DataTablesResponseNetCore result = null;
            string OptionalFilters = string.Empty;
            string OptionalParameters = string.Empty;
            string OptionalQueryBuilder = string.Empty;

            try
            {
                OptionalFilters = " r.id NOT IN ( SELECT m.application_user_id FROM user_role m WHERE m.application_role_id = @0 ) ";
                OptionalParameters = RoleId;
                result = Context.GetListDataTablesMarvelBinder<vw_application_user>(DTRequest,
                    OptionalFilters, OptionalParameters, OptionalQueryBuilder);
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }

            return result;
        }

        public dynamic AssignRoleToUser(string[] Ids, string userId, ref bool isComplete)
        {
            dynamic result = new ExpandoObject();
            var log = "";

            using (var scope = new TransactionScope())
            {
                try
                {
                    var total = Ids.Length;
                    int success = 0, fail = 0, exists = 0;
                    bool status = true;
                    foreach (var id in Ids)
                    {
                        var oldRecord = user_role.FirstOrDefault(" WHERE application_user_id=@0 AND application_role_id=@1 AND is_active=1 AND COALESCE(is_deleted,0)=0 ", userId,id);
                        if (oldRecord == null)
                        {
                            var record = new user_role();
                            record.application_user_id = userId;
                            record.application_role_id = id;
                            status &= Context.SaveEntity<user_role>(record, true);
                            if (status) success++;
                            else fail++;
                        }
                        else
                        {
                            appLogger.Info($"Already exists user member id {id}");
                            exists++;
                        }
                    }

                    if (status)
                    {
                        scope.Complete();
                        isComplete = true;
                    }

                    result.Success = success;
                    result.Fail = fail;

                    log = $"Assigned Role to User Id {userId}, Success:{success} | Failed:{fail}";
                    appLogger.Debug(log);
                }
                catch (Exception ex)
                {
                    appLogger.Error(ex);
                    throw;
                }
            }

            return result;
        }
        public dynamic DeleteRoleMember(string[] Ids, string userId, ref bool isComplete)
        {
            dynamic result = new ExpandoObject();
            var log = "";

            using (var scope = new TransactionScope())
            {
                try
                {
                    var total = Ids.Length;
                    int success = 0, fail = 0;
                    bool status = true;
                    foreach (var id in Ids)
                    {
                        var record = user_role.Fetch(" WHERE application_role_id=@0 AND application_user_id=@1 AND COALESCE(is_deleted,0)=0", id, userId).FirstOrDefault();
                        if (record == null) status = false;
                        else
                        {
                            status &= Context.DeleteEntity<user_role>(record.id);
                        }
                        if (status) success++;
                        else fail++;
                    }

                    if (status)
                    {
                        scope.Complete();
                        isComplete = true;
                    }

                    result.Success = success;
                    result.Fail = fail;

                    log = $"Deleted Role Member User Id {userId}, Success:{success} | Failed:{fail}";
                    appLogger.Debug(log);
                }
                catch (Exception ex)
                {
                    appLogger.Error(ex);
                    throw;
                }
            }

            return result;
        }

        public dynamic AssignMember(string[] Ids, string RoleId, ref bool isComplete)
        {
            dynamic result = new ExpandoObject();
            var log = "";

            using (var scope = new TransactionScope())
            {
                try
                {
                    var total = Ids.Length;
                    int success = 0, fail = 0, exists = 0;
                    bool status = true;
                    foreach (var id in Ids)
                    {
                        var oldRecord = user_role.FirstOrDefault(" WHERE application_user_id=@0 AND application_role_id=@1 AND is_active=1 AND COALESCE(is_deleted,0)=0 ", id, RoleId);
                        if (oldRecord == null)
                        {
                            var record = new user_role();
                            record.application_user_id = id;
                            record.application_role_id = RoleId;
                            status &= Context.SaveEntity<user_role>(record, true);
                            if (status) success++;
                            else fail++;
                        }
                        else
                        {
                            appLogger.Info($"Already exists user member id {id}");
                            exists++;
                        }
                    }

                    if (status)
                    {
                        scope.Complete();
                        isComplete = true;
                    }

                    result.Success = success;
                    result.Fail = fail;

                    log = $"Assigned Member to User Role {RoleId}, Success:{success} | Failed:{fail}";
                    appLogger.Debug(log);
                }
                catch (Exception ex)
                {
                    appLogger.Error(ex);
                    throw;
                }
            }

            return result;
        }

        public dynamic DeleteMember(string[] Ids, string RoleId, ref bool isComplete)
        {
            dynamic result = new ExpandoObject();
            var log = "";

            using (var scope = new TransactionScope())
            {
                try
                {
                    var total = Ids.Length;
                    int success = 0, fail = 0;
                    bool status = true;
                    foreach (var id in Ids)
                    {
                        var record = user_role.Fetch(" WHERE application_role_id=@0 AND application_user_id=@1 AND COALESCE(is_deleted,0)=0", RoleId, id).FirstOrDefault();
                        if (record == null) status = false;
                        else
                        {
                            status &= Context.DeleteEntity<user_role>(record.id);
                        }
                        if (status) success++;
                        else fail++;
                    }

                    if (status)
                    {
                        scope.Complete();
                        isComplete = true;
                    }

                    result.Success = success;
                    result.Fail = fail;

                    log = $"Deleted Member User Role {RoleId}, Success:{success} | Failed:{fail}";
                    appLogger.Debug(log);
                }
                catch (Exception ex)
                {
                    appLogger.Error(ex);
                    throw;
                }
            }

            return result;
        }

    }
}