using DataAccess.PhoenixERP;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DataAccess
{
    public static class Ext
    {
        private static NLog.Logger appLogger = NLog.LogManager.GetCurrentClassLogger();

        public static object MapToEntity<T>(this IBaseRecord source,ref bool IsNew)
        {
            string log = "";
            var id = "";

            var result = Activator.CreateInstance<T>();

            if (source == null) return result;
            if (!string.IsNullOrEmpty(source.id))
            {
                id = source.id;
            }

            using (var db = new PhoenixERPRepo())
            {
                try
                {
                    result = db.FirstOrDefault<T>(" WHERE id = @0 ", id);
                    appLogger.Debug("Record Map Entity");
                    appLogger.Debug(JsonConvert.SerializeObject(result));
                    IsNew = (result == null);
                    if (IsNew)
                    {
                        result = Activator.CreateInstance<T>();
                        log = String.Format("New {0} record", typeof(T).Name);
                        appLogger.Debug(log);
                    }
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


            // Iterate the Properties of the destination instance and  
            // populate them from their source counterparts  
            PropertyInfo[] destinationProperties = result.GetType().GetProperties();
            foreach (PropertyInfo destinationPi in destinationProperties)
            {
                PropertyInfo sourcePi = source.GetType().GetProperty(destinationPi.Name);
                if (sourcePi.GetValue(source,null) == null) continue;
                destinationPi.SetValue(result, sourcePi.GetValue(source, null), null);
            }
            return result;
            //// Iterate the Properties of the destination instance and  
            //// populate them from their source counterparts  
            //PropertyInfo[] destinationProperties = source.GetType().GetProperties();
            //foreach (PropertyInfo destinationPi in destinationProperties)
            //{
            //    PropertyInfo sourcePi = source.GetType().GetProperty(destinationPi.Name);
            //    destinationPi.SetValue(source, sourcePi.GetValue(source, null), null);
            //}
        }
    }
}
