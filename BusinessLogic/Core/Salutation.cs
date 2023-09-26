using BusinessLogic.Services.General;
using DataAccess.PhoenixERP;
using DataAccess.PhoenixERP.General;
using NLog;
using NPoco;
using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Core
{
    public partial class Salutation : SalutationService
    {
        protected readonly Logger appLogger = LogManager.GetCurrentClassLogger();
        public Salutation(DataContext AppUserData) : base(AppUserData)
        {

        }

  
        public dynamic Lookup(String Id = null, int page = 1, int length = 100, String optionalFilters = null, String optionalParameters = null)
        {
            dynamic result = null;
            try
            {
                if (String.IsNullOrEmpty(Id))
                {
                    var filters = "";
                    var parameters = "";
                    #region Use filters
                    if (!string.IsNullOrEmpty(optionalFilters))
                    {
                        foreach (var col in salutation.DefaultViewColumns)
                        {
                            filters = optionalFilters.Replace(" " + col.Key + " ", " " + col.Value + " ");
                        }
                        filters = filters.Replace(";", "--");
                    }

                    parameters = optionalParameters;
                    int pageDraw = page;
                    int rowPerPage = length;

                    using (var db = new PhoenixERPRepo())
                    {
                        try
                        {
                            var sql = Sql.Builder.Append(salutation.DefaultView);
                            sql.Append(" WHERE r.is_active = 1 ");
                            if (!String.IsNullOrEmpty(filters))
                            {
                                if (String.IsNullOrEmpty(parameters))
                                {
                                    sql.Append(String.Format(" AND ( {0} ) ", filters));
                                }
                                else
                                {
                                    sql.Append(String.Format(" AND ( {0} ) ", filters),
                                        parameters.Split(",".ToCharArray()));
                                }
                            }

                            sql.Append(" ORDER BY r.title ");
                            result = db.Page<vw_salutation>(page, rowPerPage, sql);

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
                            var sql = Sql.Builder.Append(salutation.DefaultView);
                            sql.Append(" WHERE r.is_active = 1 ");
                            sql.Append(" AND r.id = @0 ", Id);
                            sql.Append(" ORDER BY r.title ");
                            result = db.Page<vw_salutation>(1, rowPerPage, sql);

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
