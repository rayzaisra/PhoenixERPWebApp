using NPoco;
using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using DataAccess.PhoenixERP;
using DataAccess.PhoenixERP.General;

namespace BusinessLogic
{
    public partial class DataContext
    {
        public static bool SetupOrganization(string OrganizationName, string AdministratorUsername, string AdministratorPassword)
        {
            //String log = "";
            //String qry = "";
            //Sql sql = null;

            bool result = false;
            appLogger.Info("Preparing Marvel Framework");
            appLogger.Info("Updating Setup Entity ...");
            
            if(SetupEntity()) appLogger.Info("Setup Entity Has been Updated...");
            result = true;
            return result;
        }

        public static bool SetupEntity()
        {
            bool result = false;
            string log = "";
            string qry = "";

            log = "Start setup entity ...";
            appLogger.Debug(log);

            try
            {
                var app_entities = GetAppEntities();
                Console.WriteLine("Saving {0} application_entity", app_entities.Count);
                foreach (var rec in app_entities)
                {
                    #region app_entity

                    using (var db = new PhoenixERPRepo())
                    {
                        try
                        {
                            qry = "SELECT TOP 1 * FROM application_entity WHERE id = @0";
                            if (db.FirstOrDefault<application_entity>(qry, rec.id) != null)
                            {
                                Console.WriteLine("Update application_entity {0}", rec.entity_name);
                                db.Update(rec);
                            }
                            else
                            {
                                Console.WriteLine("Insert application_entity {0}", rec.entity_name);
                                db.Insert(rec);
                            }
                        }
                        catch (Exception ex)
                        {
                            appLogger.Error(ex.ToString());
                        }
                    }

                    #endregion
                }
                result = true;
            }
            catch (Exception ex)
            {
                appLogger.Error(ex.ToString());
            }
            finally
            {
                log = String.Format("Setup entity = {0}", result);
                appLogger.Debug(log);
            }

            return result;
        }
    }
}
