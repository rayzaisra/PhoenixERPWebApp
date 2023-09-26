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
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BusinessLogic.Core
{
    public partial class ApplicationModule :ApplicationModuleService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public ApplicationModule(DataContext AppUserData) : base(AppUserData)
        {
        }

        public Boolean RegisterApplicationModule() {
            var result = false;
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    var Modules = DataContext.GetApplicationModule();
                    var ModulesDTO = new List<application_moduleDTO>();
                    ModulesDTO = Modules
                        .Select(x => new application_moduleDTO().InjectFrom(x)).Cast<application_moduleDTO>()
                        .ToList();
                    result = true;
                    var isNew = false;
                    foreach (var module in ModulesDTO)
                    {
                        result &= Svc.SaveEntity(module, ref isNew);
                    }
                }
                catch (Exception ex)
                {
                    appLogger.Error(ex);
                }
                finally {
                    if (result)
                    {
                        scope.Complete();
                        appLogger.Debug("Completed Register Application Module");
                    }
                    else
                    {
                        appLogger.Debug("Aborted Register Application Module");
                    }
                }
            }
            return result;
        }

        public dynamic Lookup(string Id = null, int Page = 1, int Length = 100,
            string OptionalFilters = null, string OptionalParameters = null)
        {
            dynamic result = null;

            try
            {
                if (string.IsNullOrEmpty(Id))
                {
                    var filters = "";
                    var parameters = "";

                    #region Use filters

                    if (!string.IsNullOrEmpty(OptionalFilters))
                    {
                        foreach (var col in application_module.DefaultViewColumns)
                        {
                            filters = OptionalFilters.Replace(" " + col.Key + " ", " " + col.Value + " ");
                        }
                        filters = filters.Replace(";", "--");
                    }

                    parameters = OptionalParameters;
                    int pageDraw = Page;
                    int rowPerPage = Length;

                    using (var db = new PhoenixERPRepo())
                    {
                        try
                        {
                            var sql = Sql.Builder.Append(application_module.DefaultView);
                            sql.Append(" WHERE r.is_active = 1 ");

                            if (!string.IsNullOrEmpty(filters))
                            {
                                if (string.IsNullOrEmpty(parameters))
                                {
                                    sql.Append(string.Format(" AND ( {0} ) ", filters));
                                }
                                else
                                {
                                    sql.Append(string.Format(" AND ( {0} ) ", filters),
                                        parameters.Split(",".ToCharArray()));
                                }
                            }

                            sql.Append(" ORDER BY r.module_name ");
                            result = db.Page<vw_application_module>(Page, rowPerPage, sql);
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

                    #endregion
                }
                else
                {
                    #region Use Id

                    using (var db = new PhoenixERPRepo())
                    {
                        try
                        {
                            int rowPerPage = 10;
                            var sql = Sql.Builder.Append(application_module.DefaultView);
                            sql.Append(" WHERE r.is_active = 1 ");
                            sql.Append(" AND r.id = @0 ", Id);
                            sql.Append(" ORDER BY r.module_name ");

                            result = db.Page<vw_application_module>(1, rowPerPage, sql);
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

                    #endregion
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