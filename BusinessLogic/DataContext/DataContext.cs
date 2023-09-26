using System;
using System.Collections.Generic;
using System.Text;
using CodeMarvel.Infrastructure.Sessions;
using DataAccess;
using DataAccess.PhoenixERP;
using DataAccess.PhoenixERP.General;
using NLog;

namespace BusinessLogic
{
    public partial class DataContext
    {
        private static readonly Logger appLogger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

        public const string DEFAULT_USER_SYSTEM_ID = "00000000-0000-0000-0000-u00000000001";

        public string AppUserId { get; private set; }

        public string Username { get; private set; }

        public string Fullname { get; private set; }

        public string OrganizationId { get; private set; }

        public string OrganizationName { get; private set; }

        public bool IsDefaultUser { get; private set; }

        public bool IsSystemAdministrator { get; private set; }

        public UserSession userSession { get; private set; }

        //partial void CommonConstruct();
        //public interface IFactory
        //{
        //    DataContext GetInstance();
        //}
        //public static IFactory Factory { get; set; }
        //public static DataContext GetInstance()
        //{
        //    if (_instance != null)
        //        return _instance;

        //    if (Factory != null)
        //        return Factory.GetInstance();
        //    else
        //        return null;
        //}
        //[ThreadStatic] static DataContext _instance;

        public DataContext(string AppUserId, string IPAddress, UserSession userSession)
        {
            this.AppUserId = AppUserId;
            this.userSession = userSession;
            using (var db = new PhoenixERPRepo())
            {
                try
                {
                    var qry = application_user.DefaultView + " WHERE r.id = @0 ";
                    var recordData = db.FirstOrDefault<vw_application_user>(qry, AppUserId);
                    if (recordData != null)
                    {
                        qry = business_unit.DefaultView + " WHERE r.id = @0 ";
                        var businessUnit = db.FirstOrDefault<vw_business_unit>(qry, recordData.business_unit_id);

                        AppUserId = recordData.id;
                        Username = recordData.app_username;
                        Fullname = recordData.app_fullname;
                        OrganizationId = businessUnit?.organization_id;
                        OrganizationName = businessUnit?.organization_name;
                        IsDefaultUser = recordData.is_default ?? false;
                        IsSystemAdministrator = recordData.is_system_administrator ?? false;
                        
                    }
                    // hardcode by dayat
                    OrganizationId = "9EE4834225E334380667DDCF2F60F6DA";
                }
                catch (Exception ex)
                {
                    appLogger.Error(ex.ToString());
                }
            }
        }
    }
}
