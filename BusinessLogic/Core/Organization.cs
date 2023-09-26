using System;
using DataAccess;
using DataAccess.PhoenixERP;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using Omu.ValueInjecter;
using NLog;
using System.Transactions;
using System.Dynamic;
using System.Linq;
using BusinessLogic.Services.General;
using NPoco;
using DataAccess.PhoenixERP.General;

namespace BusinessLogic.Core
{
    public partial class Organization : OrganizationService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public Organization(DataContext AppUserData) : base(AppUserData)
        {
        }

        public static bool SetupOrganization(string OrganizationName, string AdminUsername, string AdminPassword)
        {
            bool result = false;

            try
            {
                using(var db = new PhoenixERPRepo())
                {
                    try
                    {
                        #region Validation

                        var sql = Sql.Builder.Append("SELECT TOP 1 * FROM dbo.organization r");
                        sql.Append("WHERE r.organization_name = @0", OrganizationName);
                        var org = db.FirstOrDefault<organization>(sql);
                        appLogger.Debug(db.LastCommand);

                        if(org != null)
                        {
                            return false; 
                        }

                        Sql.Builder.Append("SELECT TOP 1 * FROM dbo.application_user r");
                        sql.Append("WHERE r.app_username = @0", AdminUsername);
                        var admin = db.FirstOrDefault<application_user>(sql);
                        appLogger.Debug(db.LastCommand);

                        if(admin != null)
                        {
                            return false;
                        }

                        #endregion

                        #region Setup

                        using(var tx = db.GetTransaction())
                        {
                            try
                            {
                                org = new organization()
                                {
                                    id = CommonTool.GUIDHash.ConvertToMd5HashGUID(OrganizationName).ToString("N"),
                                    is_active = true,
                                    is_default = true
                                };

                                admin = new application_user()
                                {
                                    id = CommonTool.GUIDHash.ConvertToMd5HashGUID(AdminUsername).ToString("N"),
                                    is_active = true,
                                    is_default = true,
                                    is_system_administrator = true,
                                    organization_id = org.id,
                                    app_username = AdminUsername,
                                    app_fullname = AdminUsername,
                                    app_password = CommonTool.PasswordHash.CreateHash(AdminPassword)
                                };

                                org.created_by = admin.id;
                                org.created_on = DateTime.Now;
                                org.owner_id = admin.id;

                                admin.created_by = admin.id;
                                admin.created_on = DateTime.Now;
                                admin.owner_id = admin.id;

                                var bu = new business_unit()
                                {
                                    id = Guid.NewGuid().ToString("N"),
                                    created_by = admin.id,
                                    created_on = DateTime.Now,
                                    is_active = true,
                                    is_default = true,
                                    owner_id = admin.id,
                                    organization_id = org.id,
                                    unit_name = org.organization_name
                                };

                                var tim = new team()
                                {
                                    id = Guid.NewGuid().ToString("N"),
                                    created_by = admin.id,
                                    created_on = DateTime.Now,
                                    is_active = true,
                                    is_default = true,
                                    owner_id = admin.id,
                                    organization_id = org.id,
                                    team_name = org.organization_name
                                };

                                admin.business_unit_id = bu.id;
                                bu.default_team = tim.id;
                                tim.team_leader = admin.id;

                                tx.Complete();
                                result = true;
                            }
                            catch (Exception ex)
                            {
                                appLogger.Error(ex.ToString());                                
                            }
                        }

                        #endregion
                    }
                    catch (Exception ex)
                    {
                        appLogger.Error(ex.ToString());                        
                    }
                }
            }
            catch (Exception ex)
            {
                appLogger.Error(ex.ToString());                
            }

            return result;
        }

        public dynamic kendoLookup(CommonTool.KendoUI.Combobox.ComboboxDataRequest dtRequest)
        {
            dynamic result = null;
            if (dtRequest != null)
            {
                #region Use filters
                var sql = Sql.Builder.Append(organization.DefaultView);
                sql.Append(" WHERE r.is_active = 1 ");
                if (dtRequest.Filter2 != null)
                {
                    Sql filterBuilder = Context.MultiplekendoFilterable<vw_organization>(dtRequest.Filter2);
                    if (filterBuilder != null)
                        sql.Append(filterBuilder.SQL, filterBuilder.Arguments);
                }
                #endregion
                sql.Append(" ORDER BY r.organization_name");
                using (var db = new PhoenixERPRepo())
                {
                    try
                    {
                        int pageDraw = dtRequest.Page;
                        int rowPerPage = dtRequest.PageSize;
                        result = db.Page<vw_organization>(pageDraw, rowPerPage, sql);
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
    }
}
