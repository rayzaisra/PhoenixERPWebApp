using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.PhoenixERP;
using DataAccess;
using System.Dynamic;
using Newtonsoft.Json;
using DataAccess.PhoenixERP.General;
using NPoco;

namespace BusinessLogic
{
    public partial class DataContext
    {
        public bool DeleteEntity<T>(string Id)
        {
            var success = false;
            string log = "";
            var appEntity = Activator.CreateInstance<T>();

            try
            {
                if (String.IsNullOrEmpty(Id) || String.IsNullOrWhiteSpace(Id))
                {
                    appLogger.Error("Record Id is empty");
                    return success;
                }

                if (typeof(IEntity).IsAssignableFrom(typeof(T)))
                {
                    if (typeof(IBaseRecord).IsAssignableFrom(typeof(T)))
                    {
                        var qry = ((IEntity)appEntity).GetDefaultView();
                        qry += " WHERE r.id = @0 AND COALESCE(r.is_deleted, 0) = 0 ";

                        // Role access filter
                        //qry += string.Format(" AND dbo.ufn_can_delete( '{0}', '{1}', r.id, r.owner_id ) > 0 ",
                          //  AppUserId, ((IEntity)appEntity).GetEntityId());

                        using (var db = new PhoenixERPRepo())
                        {
                            try
                            {
                                var oldRecord = db.FirstOrDefault<T>(qry, Id);
                                appLogger.Debug(db.LastCommand);

                                if (oldRecord != null)
                                {
                                    if (!IsSystemAdministrator)
                                    {
                                        if (((IBaseRecord)oldRecord).is_default ?? false)
                                        {
                                            log = $"Default record {Id} can not be deleted ";
                                            appLogger.Debug(log);
                                            return success;
                                        }
                                        /*
                                        else if (((IBaseRecord)oldRecord).is_locked ?? false)
                                        {
                                            log = $"Locked record {Id} is not editable";
                                            appLogger.Debug(log);
                                            return Success;
                                        }
                                        */
                                    }

                                    if (((((IBaseRecord)oldRecord).is_deleted ?? false) == true) &&
                                        ((((IBaseRecord)oldRecord).is_active ?? false) == false))
                                    {
                                        log = string.Format("Old view = {0}", JsonConvert.SerializeObject(oldRecord));
                                        appLogger.Debug(log);
                                        success = true;
                                        log = string.Format("Record {0} with Id = {1} has been deleted by soft delete ",
                                            ((IEntity)appEntity).GetEntityName(), Id);
                                        appLogger.Debug(log);
                                    }
                                    else
                                    {
                                        log = string.Format("Old view = {0}", JsonConvert.SerializeObject(oldRecord));
                                        appLogger.Debug(log);

                                        ((IBaseRecord)oldRecord).is_active = false;
                                        ((IBaseRecord)oldRecord).is_deleted = true;
                                        ((IBaseRecord)oldRecord).deleted_by = AppUserId;
                                        ((IBaseRecord)oldRecord).deleted_on = DateTime.Now;
                                       
                                        Guid audit_trail_id = Guid.NewGuid();
                                        using (var scope = db.GetTransaction())
                                        {
                                            try
                                            {
                                                appLogger.Debug("Delete Masuk");
                                                db.Update(oldRecord);
                                                //db.Insert(new audit_trail()
                                                //{
                                                //    id = audit_trail_id.ToString(),
                                                //    created_by = AppUserId,
                                                //    created_on = DateTime.Now,
                                                //    owner_id = AppUserId,
                                                //    is_active = true,
                                                //    organization_id = OrganizationId,
                                                //    organization_name = OrganizationName,
                                                //    application_user_id = AppUserId,
                                                //    app_username = Username,
                                                //    app_fullname = Fullname,
                                                //    application_entity_id = ((IEntity)appEntity).GetEntityId(),
                                                //    user_action = DataAccessControl.ACTION_DELETE,
                                                //    record_id = ((IBaseRecord)oldRecord).id,
                                                //    new_record = JsonConvert.SerializeObject(oldRecord)
                                                //});

                                                scope.Complete();

                                                success = true;
                                                log = string.Format("Record {0} with Id = {1} is deleted",
                                                    ((IEntity)appEntity).GetEntityName(), Id);
                                                appLogger.Debug(log);
                                            }
                                            catch (Exception ex)
                                            {
                                                appLogger.Error(ex.ToString());
                                                appLogger.Debug(db.LastCommand);
                                                throw;
                                            }

                                        }
                                    }
                                }
                                else
                                {
                                    log = string.Format("Record {0} with Id = {1} does not exist",
                                        ((IEntity)appEntity).GetEntityName(), Id);
                                    appLogger.Error(log);
                                    appLogger.Debug(db.LastCommand);
                                }
                            }
                            catch (Exception ex)
                            {
                                appLogger.Error(ex.ToString());
                                appLogger.Debug(db.LastCommand);
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
            }

            return success;
        }
    }
}
