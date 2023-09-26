using DataAccess.PhoenixERP;
using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using NPoco;

namespace BusinessLogic
{
    public partial class DataContext
    {
        public object GetEntityDetailsById<T>(string Id)
        {
            var result = default(T);
            try
            {
                var qry = "";
                object appEntity = GetBaseEntity<T>();
                if (appEntity != null) qry = ((IEntity)appEntity).GetDefaultView();
                if (string.IsNullOrEmpty(qry)) return result;

                qry += " WHERE r.id = @0 AND COALESCE(r.is_deleted, 0) = 0 ";

                // Role access filter
               // qry += string.Format(" AND dbo.ufn_can_read( '{0}', '{1}', r.id, r.owner_id ) > 0 ",
                   // AppUserId, ((IEntity)appEntity).GetEntityId());

                using (var db = new PhoenixERPRepo())
                {
                    try
                    {
                        result = db.FirstOrDefault<T>(qry, Id);
                    }
                    catch (Exception ex)
                    {
                        appLogger.Error(ex.ToString());
                    }
                    finally
                    {
                        appLogger.Debug(db.LastCommand);
                    }
                }
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }

            return result;
        }

        public object GetEntityById<T>(string Id)
        {
            var result = default(T);

            try
            {
                using (var db = new PhoenixERPRepo())
                {
                    try
                    {
                        result = db.FirstOrDefault<T>(" WHERE id = @0 AND COALESCE(r.is_deleted, 0) = 0 ", Id);
                    }
                    catch (Exception ex)
                    {
                        appLogger.Error(ex.ToString());
                    }
                    finally
                    {
                        appLogger.Debug(db.LastCommand);
                    }
                }
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
