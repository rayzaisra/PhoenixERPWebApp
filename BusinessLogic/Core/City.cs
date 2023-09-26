using BusinessLogic.Services.General;
using CommonTool.JEasyUI.DataGrid;
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
    public partial class City : CityService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public City(DataContext AppUserData) : base(AppUserData)
        {
        }

        public CommonTool.MetronicDataTables.mDatatableResponse GetListMetronicDataTables(CommonTool.MetronicDataTables.mDatatableRequest DTRequest)
        {
            CommonTool.MetronicDataTables.mDatatableResponse result = null;
            try
            {
                result = Context.GetListMetronicDataTablesNetCore<vw_city>(DTRequest);
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }

            return result;
        }

        public DgResponse GetDataGridEasyUI(DgRequest DTRequest)
        {
            DgResponse result = null;
            try
            {
                result = Context.GetDataGridEasyUI<vw_city>(DTRequest);
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }

            return result;
        }


        public override vw_cityDTO Details(string Id)
        {
            vw_cityDTO result = null;

            try
            {
                using (var db = new PhoenixERPRepo())
                {
                    var sql = Sql.Builder.Append(city.DefaultView);
                    sql.Append(" WHERE r.is_active = 1 ");
                    sql.Append(" AND r.id = @0 ", Id);

                    var view = db.FirstOrDefault<vw_city>(sql);
                    if (view != null)
                    {
                        result = (vw_cityDTO)((new vw_cityDTO()).InjectFrom(view));
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
                        foreach (var col in city.DefaultViewColumns)
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
                            var sql = Sql.Builder.Append(city.DefaultView);
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

                            sql.Append(" ORDER BY r.city_name ");
                            result = db.Page<vw_city>(Page, rowPerPage, sql);
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
                            var sql = Sql.Builder.Append(city.DefaultView);
                            sql.Append(" WHERE r.is_active = 1 ");
                            sql.Append(" AND r.id = @0 ", Id);
                            sql.Append(" ORDER BY r.city_name ");

                            result = db.Page<vw_city>(1, rowPerPage, sql);
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
