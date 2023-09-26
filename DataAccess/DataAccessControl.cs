using System;
using System.Collections.Generic;
using System.Text;
using NLog;
using DataAccess.PhoenixERP;
using DataAccess.PhoenixERP.General;

namespace DataAccess
{
    public partial class DataAccessControl
    {
        private static Logger appLogger = LogManager.GetCurrentClassLogger();

        public static readonly string ACTION_CREATE = "CREATE";

        public static readonly string ACTION_READ = "READ";

        public static readonly string ACTION_UPDATE = "UPDATE";

        public static readonly string ACTION_DELETE = "DELETE";

        public static readonly string ACTION_APPROVE = "APPROVE";

        public static readonly string ACTION_LOCK = "LOCK";

        public static readonly string ACTION_ACTIVATE = "ACTIVATE";

        public static readonly string ACTION_APPEND = "APPEND";

        public static readonly string ACTION_SHARE = "SHARE";

        public static readonly ulong MAX_ENTITY_COUNT = 10000;

        public static readonly ulong ACCESS_NOACCESS = 0;

        public static readonly ulong ACCESS_OWNRECORD = 1;

        public static readonly ulong ACCESS_BUSINESSUNIT = MAX_ENTITY_COUNT;

        public static readonly ulong ACCESS_PARENTCHILD = MAX_ENTITY_COUNT * ACCESS_BUSINESSUNIT;

        public static readonly ulong ACCESS_ORGANIZATION = MAX_ENTITY_COUNT * ACCESS_PARENTCHILD;

        public Dictionary<String, ulong> GetEntityAccess(String EntityId, String UserId)
        {
            var result = new Dictionary<String, ulong>();
            var qry = "";

            try
            {
                var listRoleAccess = new List<role_access>();
                using (var db = new PhoenixERPRepo())
                {
                    try
                    {
                        qry = " SELECT * FROM role_access ";
                        qry += " WHERE is_active = 1 AND application_entity_id = @0 ";
                        qry += " AND app_role_id IN ( ";
                        qry += " SELECT application_role_id FROM user_role ";
                        qry += " WHERE is_active = 1 AND application_user_id = @1 )";
                        listRoleAccess = db.Fetch<role_access>(qry, EntityId, UserId);
                        appLogger.Debug(db.LastCommand);
                    }
                    catch (Exception ex)
                    {
                        appLogger.Debug(db.LastCommand);
                        appLogger.Error(ex.ToString());
                    }
                }

                result.Add(ACTION_CREATE, ACCESS_NOACCESS);
                result.Add(ACTION_READ, ACCESS_NOACCESS);
                result.Add(ACTION_UPDATE, ACCESS_NOACCESS);
                result.Add(ACTION_DELETE, ACCESS_NOACCESS);
                result.Add(ACTION_APPROVE, ACCESS_NOACCESS);
                result.Add(ACTION_LOCK, ACCESS_NOACCESS);
                result.Add(ACTION_ACTIVATE, ACCESS_NOACCESS);
                result.Add(ACTION_APPEND, ACCESS_NOACCESS);
                result.Add(ACTION_SHARE, ACCESS_NOACCESS);

                foreach (var row in listRoleAccess)
                {
                    result[ACTION_CREATE] += (ulong)row.access_create;
                    result[ACTION_READ] += (ulong)row.access_read;
                    result[ACTION_UPDATE] += (ulong)row.access_update;
                    result[ACTION_DELETE] += (ulong)row.access_delete;
                    result[ACTION_APPROVE] += (ulong)(row.access_approve ?? 0);
                    result[ACTION_LOCK] += (ulong)(row.access_lock ?? 0);
                    result[ACTION_ACTIVATE] += (ulong)(row.access_activate ?? 0);
                    result[ACTION_APPEND] += (ulong)(row.access_append ?? 0);
                    result[ACTION_SHARE] += (ulong)(row.access_share ?? 0);
                }
            }
            catch (Exception ex)
            {
                appLogger.Error(ex.ToString());
            }

            return result;
        }
    }
}
