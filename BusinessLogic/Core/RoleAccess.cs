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
using System.Text;
using System.Transactions;
using Omu.ValueInjecter;
using DataAccess;
using Newtonsoft.Json;
using CommonTool.KendoUI.Grid;

namespace BusinessLogic.Core
{
    public partial class RoleAccess : RoleAccessService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public RoleAccess(DataContext AppUserData) : base(AppUserData)
        {
        }

        public bool setRoleAccessAllEntity(String role_id, long? val)
        {
            var result = false;

            using (var db = new PhoenixERPRepo())
            {
                try
                {
                    if (!Context.IsSystemAdministrator) throw new Exception("do not have a permission, only system administrator to save an entity");

                    var sqlWhere = Sql.Builder.Append(" WHERE is_active=1 AND COALESCE(is_deleted,0)=0 ");
                    sqlWhere.Append("application_role_id=@0 ", role_id);
                    var data = db.Fetch<role_access>(sqlWhere);
                    if (data == null) throw new Exception(String.Format("Role Access of role id {0} not found !", role_id));

                    data.ForEach(o =>
                    {
                        o.access_create = val;
                        o.access_read = val;
                        o.access_update = val;
                        o.access_delete = val;
                        o.access_approve = val;
                        o.access_lock = val;
                        o.access_activate = val;
                        o.access_append = val;
                        o.access_share = val;
                        o.access_level = val;
                    });
                    db.Update(data);

                }
                catch (Exception ex)
                {
                    appLogger.Error(ex);
                }
            }
            return result;
        }


        public bool RegisterRoleAccessEntity(String AppRoleId)
        {
            bool success = false;

            using (var db = new PhoenixERPRepo())
            {
                using (var scope = db.GetTransaction())
                {
                    try
                    {
                        var qry = application_entity.DefaultView;
                        qry += " WHERE r.id NOT IN (SELECT m.application_entity_id FROM role_access m WHERE m.application_role_id=@0 ) ";
                        var ListEntity = db.Fetch<vw_application_entity>(qry, AppRoleId);
                        success = true;
                        if (ListEntity != null)
                        {

                            foreach (var item in ListEntity)
                            {
                                var record = new role_access();
                                record.application_entity_id = item.id;
                                record.application_role_id = AppRoleId;
                                record.access_create = 0;
                                record.access_read = 0;
                                record.access_update = 0;
                                record.access_delete = 0;
                                record.access_approve = 0;
                                record.access_lock = 0;
                                record.access_activate = 0;
                                record.access_append = 0;
                                record.access_share = 0;
                                record.access_level = 0;

                                success &= Context.SaveEntity<role_access>(record, true);
                            }


                        }

                        if (success)
                        {
                            scope.Complete();
                        }

                    }
                    catch (Exception ex)
                    {
                        appLogger.Error(ex);
                        throw;
                    }
                }
            }

           


            return success;
        }

        public bool UpdateRoleAccessEntity(role_accessDTO RecordDto, ref bool isNew)
        {
            bool result = false;

            if (RecordDto == null) return result;

            using (var scope = new TransactionScope())
            {
                using (var db = new PhoenixERPRepo())
                {
                    try
                    {
                        var qry = role_access.DefaultView;
                        qry += " WHERE r.application_role_id=@0 and r.id=@1";
                        var listRole = db.FirstOrDefault<vw_role_access>(qry, RecordDto.application_role_id, RecordDto.id);
                        appLogger.Debug(db.LastCommand);

                        var record = new role_access();
                        record.InjectFrom(RecordDto);

                        role_access recordEntity = (role_access)(((IBaseRecord)record).MapToEntity<role_access>(ref isNew));
                        appLogger.Debug("Record Map Entity");
                        appLogger.Debug(JsonConvert.SerializeObject(recordEntity));
                        if (listRole != null)
                        {
                            if (RecordDto.access_create.Equals(null)) RecordDto.access_create = listRole.access_create;
                            if (RecordDto.access_delete.Equals(null)) RecordDto.access_delete = listRole.access_delete;
                            if (RecordDto.access_read.Equals(null)) RecordDto.access_read = listRole.access_read;
                            if (RecordDto.access_update.Equals(null)) RecordDto.access_update = listRole.access_update;
                            if (RecordDto.access_activate.Equals(null)) RecordDto.access_activate = listRole.access_activate;
                            if (RecordDto.access_approve.Equals(null)) RecordDto.access_approve = listRole.access_approve;
                            if (RecordDto.access_append.Equals(null)) RecordDto.access_append = listRole.access_append;
                            if (RecordDto.access_level.Equals(null)) RecordDto.access_level = listRole.access_level;
                            if (RecordDto.access_lock.Equals(null)) RecordDto.access_lock = listRole.access_lock;
                            if (RecordDto.access_share.Equals(null)) RecordDto.access_share = listRole.access_share;

                            result = Svc.SaveEntity(RecordDto, ref isNew);
                        }
                    }
                    catch (Exception ex)
                    {
                        appLogger.Error(ex);
                        throw;
                    }
                    finally
                    {
                        if (result)
                        {
                            scope.Complete();
                        }
                    }
                }
            }




            return result;
        }

        public DataResponse GetEntityRoleListKendoUIGrid(DataRequest dtRequest, string roldID)
        {
            DataResponse result = null;
            try
            {
                String OptionalFilters = string.Empty;
                String OptionalParameters = string.Empty;
                String OptionalQueryBuilder = string.Empty;
                OptionalQueryBuilder = string.Format(" r.application_role_id='{0}' ", roldID);
                result = GetListKendoGrid(dtRequest, OptionalFilters, OptionalParameters, OptionalQueryBuilder);
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }

            return result;
        }

        public bool SyncronizeEntity(String roleId)
        {
            var result = false;
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            using (var db = new PhoenixERPRepo())
            {
                try
                {
                    var qry = "SELECT r.* FROM dbo.application_entity r WHERE r.is_active=1 ";
                    var sql = Sql.Builder.Append(qry);
                    sql.Append(" AND r.id NOT IN(SELECT application_entity_id FROM dbo.role_access ra WHERE ra.application_role_id = @0)", roleId);
                    var entities = db.Fetch<application_entity>(sql);
                    appLogger.Info($"TOTAL ENTITIES NOT REGISTERED {entities.Count}");
                    List<role_access> entityRecords = new List<role_access>();
                    foreach (var entity in entities)
                    {
                        entityRecords.Add(new role_access()
                        {
                            id = Guid.NewGuid().ToString(),
                            created_by = Context.AppUserId,
                            created_on = DateTime.Now,
                            is_active = true,
                            is_default = true,
                            is_locked = true,
                            application_role_id = roleId,
                            application_entity_id = entity.id
                        });
                    }
                    db.InsertBulk<role_access>(entityRecords);
                    result = true;
                }
                catch (Exception ex)
                {
                    appLogger.Debug(db.LastCommand);
                    appLogger.Error(ex);
                    result = false;
                }
                finally
                {
                    if (result)
                    {
                        scope.Complete();
                    }
                }
            }
            return result;
        }

        public DataTablesResponseNetCore GetListRoleAccessByRoleId(IDataTablesRequest DTRequest, string roleId)
        {
            DataTablesResponseNetCore result = null;
            string OptionalFilters = string.Empty;
            string OptionalParameters = string.Empty;
            string OptionalQueryBuilder = string.Empty;

            try
            {
                OptionalQueryBuilder = String.Format(" r.application_role_id = '{0}' " + roleId);
                result = Context.GetListDataTablesMarvelBinder<vw_role_access>(DTRequest,
                    OptionalFilters, OptionalParameters, OptionalQueryBuilder);
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }

            return result;
        }
    }
}