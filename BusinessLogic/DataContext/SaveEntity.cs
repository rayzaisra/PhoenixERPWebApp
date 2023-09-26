using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using NPoco;
using DataAccess;
using DataAccess.PhoenixERP;
using DataAccess.PhoenixERP.General;

namespace BusinessLogic
{
    public partial class DataContext
    {
        public bool SaveEntity<T>(T Record, bool IsNew)
        {
            bool result = false;
            string log = "";
            var appEntity = Activator.CreateInstance<T>();

            try
            {
                if (typeof(IEntity).IsAssignableFrom(typeof(T)))
                {
                    if (typeof(IBaseRecord).IsAssignableFrom(typeof(T)))
                    {
                        using (var db = new PhoenixERPRepo())
                        {
                            try
                            {
                                if (IsNew)
                                {
                                    #region Insert

                                    log = "Insert record ...";
                                    appLogger.Debug(log);

                                    if (string.IsNullOrEmpty(((IBaseRecord)Record).id) ||
                                        string.IsNullOrWhiteSpace(((IBaseRecord)Record).id))
                                    {
                                        ((IBaseRecord)Record).id = Guid.NewGuid().ToString();
                                    }

                                    if (((IBaseRecord)Record).is_active == null)
                                    {
                                        ((IBaseRecord)Record).is_active = true;
                                    }

                                    if (((IBaseRecord)Record).created_by == null)
                                    {
                                        ((IBaseRecord)Record).created_by = AppUserId;
                                    }

                                    if (((IBaseRecord)Record).created_on == null)
                                    {
                                        ((IBaseRecord)Record).created_on = DateTime.Now;
                                    }

                                    ((IBaseRecord)Record).modified_by = null;
                                    ((IBaseRecord)Record).modified_on = null;

                                    if (((IBaseRecord)Record).owner_id == null)
                                    {
                                        ((IBaseRecord)Record).owner_id = AppUserId;
                                    }

                                    #region Check role access

                                    var entityId = ((IEntity)Record).GetEntityId();
                                    var sql = Sql.Builder.Append("SELECT dbo.ufn_can_create( @0, @1 )",
                                        AppUserId, entityId);
                                    var hasAccess = db.ExecuteScalar<int>(sql);
                                    //if (hasAccess <= 0) return false;

                                    #endregion

                                    Guid audit_trail_id = Guid.NewGuid();
                                    using (var scope = db.GetTransaction())
                                    {
                                        try
                                        {
                                            db.Insert(Record);

                                            //db.Insert(new audit_trail()
                                            //{
                                            //    id = audit_trail_id.ToString(),
                                            //    created_by = AppUserId,
                                            //    created_on = DateTime.Now,
                                            //    owner_id = AppUserId,
                                            //    is_active = true,
                                            //    organization_id = "9EE4834225E334380667DDCF2F60F6DA",
                                            //    organization_name = "Nava",
                                            //    application_user_id = AppUserId,
                                            //    app_username = Username,
                                            //    app_fullname = Fullname,
                                            //    application_entity_id = ((IEntity)appEntity).GetEntityId(),
                                            //    user_action = DataAccessControl.ACTION_CREATE,
                                            //    record_id = ((IBaseRecord)Record).id,
                                            //    new_record = JsonConvert.SerializeObject(Record)
                                            //});

                                            scope.Complete();
                                            result = true;
                                        }
                                        catch (Exception ex)
                                        {
                                            appLogger.Error(ex.ToString());
                                            appLogger.Debug(db.LastCommand);
                                            throw;
                                        }
                                    }

                                    log = String.Format("Insert record = {0}", result);
                                    appLogger.Debug(log);

                                    #endregion
                                }
                                else
                                {
                                    #region Update

                                    log = "Update record ...";
                                    appLogger.Debug(log);

                                    var qry = ((IEntity)appEntity).GetDefaultView();
                                    qry += " WHERE r.id = @0 AND COALESCE(r.is_deleted, 0) = 0 ";

                                    if (!IsSystemAdministrator)
                                    {
                                        qry += " AND COALESCE(r.is_locked, 0) = 0 ";
                                        qry += " AND COALESCE(r.is_default, 0) = 0 ";
                                    }

                                    // Role access filter
                                    //qry += string.Format(" AND dbo.ufn_can_update( '{0}', '{1}', r.id, r.owner_id ) > 0 ",
                                       // AppUserId, ((IEntity)appEntity).GetEntityId());

                                    var oldRecord = db.FirstOrDefault<T>(qry, ((IBaseRecord)Record).id);
                                    appLogger.Debug(db.LastCommand);

                                    if (oldRecord != null)
                                    {
                                        qry = ((IEntity)appEntity).GetDefaultView();
                                        qry += " WHERE r.id = @0 ";

                                        dynamic oldView = db.FirstOrDefault<dynamic>(qry, ((IBaseRecord)Record).id);
                                        appLogger.Debug(db.LastCommand);

                                        log = String.Format("Old view = {0}", JsonConvert.SerializeObject(oldView));
                                        appLogger.Debug(log);

                                        //((IBaseRecord)Record).is_active = ((IBaseRecord)oldRecord).is_active;
                                        ((IBaseRecord)Record).created_by = ((IBaseRecord)oldRecord).created_by;
                                        ((IBaseRecord)Record).created_on = ((IBaseRecord)oldRecord).created_on;

                                        //if (((IBaseRecord)Record).modified_by == null)
                                        //{
                                        //    ((IBaseRecord)Record).modified_by = AppUserId;
                                        //}

                                        //if (((IBaseRecord)Record).modified_on == null)
                                        //{
                                        //    ((IBaseRecord)Record).modified_on = DateTime.Now;
                                        //}

                                        ((IBaseRecord)Record).modified_by = AppUserId;
                                        ((IBaseRecord)Record).modified_on = DateTime.Now;
                                        //((IBaseRecord)Record).owner_id = ((IBaseRecord)oldRecord).owner_id;

                                        var audit_trail_id = Guid.NewGuid();
                                        using (var scope = db.GetTransaction())
                                        {
                                            try
                                            {
                                                db.Update(Record);
                                                //db.Insert(new audit_trail()
                                                //{
                                                //    id = audit_trail_id.ToString(),
                                                //    created_by = AppUserId,
                                                //    created_on = DateTime.Now,
                                                //    owner_id = AppUserId,
                                                //    is_active = true,
                                                //    organization_id = "9EE4834225E334380667DDCF2F60F6DA",
                                                //    organization_name = "Nava",
                                                //    application_user_id = AppUserId,
                                                //    app_username = Username,
                                                //    app_fullname = Fullname,
                                                //    application_entity_id = ((IEntity)appEntity).GetEntityId(),
                                                //    user_action = DataAccessControl.ACTION_UPDATE,
                                                //    record_id = ((IBaseRecord)Record).id,
                                                //    new_record = JsonConvert.SerializeObject(Record),
                                                //    old_record = JsonConvert.SerializeObject(oldRecord),
                                                //    old_view = JsonConvert.SerializeObject(oldView)
                                                //});

                                                scope.Complete();
                                                result = true;
                                            }
                                            catch (Exception ex)
                                            {
                                                appLogger.Error(ex.ToString());
                                                appLogger.Debug(db.LastCommand);
                                                throw;
                                            }
                                        }

                                        log = String.Format("Update record = {0}", result);
                                        appLogger.Debug(log);
                                    }
                                    else
                                    {
                                        log = String.Format("Old record {0} with Id = {1} does not exists",
                                            typeof(T), ((IBaseRecord)appEntity).id);
                                        appLogger.Debug(log);
                                    }

                                    log = String.Format("Update record = {0}", result);
                                    appLogger.Debug(log);

                                    #endregion
                                }
                            }
                            catch 
                            {
                                appLogger.Debug(db.LastCommand);
                                throw;
                            }                        
                        }
                    }
                    else
                    {
                        appLogger.Error("Record is not IBaseRecord");
                    }
                }
                else
                {
                    appLogger.Error("Record is not IEntity");
                }
            }
            catch (Exception ex)
            {
                appLogger.Error(ex.ToString());

                throw;
            }

            return result;
        }
    }
}
