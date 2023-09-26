using BusinessLogic.Services.General;
using System;
using System.Collections.Generic;
using System.Text;
using NLog;
using DataAccess.PhoenixERP.General;
using DataAccess.PhoenixERP;
using Omu.ValueInjecter;
using NPoco;

namespace BusinessLogic.Core
{
    public partial class Country : CountryService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public Country(DataContext AppUserData) : base(AppUserData)
        {                
        }

        public override bool SaveEntity(countryDTO RecordDto, ref bool isNew)
        {
            bool result = false;

            if (RecordDto == null) return result;

            try
            {
                RecordDto.organization_id = Context.OrganizationId;
                result = Svc.SaveEntity(RecordDto, ref isNew);
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }

            return result;
        }

        public override vw_countryDTO Details(string Id)
        {
            vw_countryDTO result = null;

            try
            {
                using (var db = new PhoenixERPRepo())
                {
                    var sql = Sql.Builder.Append(country.DefaultView);
                    sql.Append(" WHERE r.is_active = 1 ");
                    sql.Append(" AND r.id = @0 ", Id);

                    var view = db.FirstOrDefault<vw_country>(sql);
                    if (view != null)
                    {
                        result = (vw_countryDTO)((new vw_countryDTO()).InjectFrom(view));
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
                if (String.IsNullOrEmpty(Id))
                {
                    var filters = "";
                    var parameters = "";

                    #region Use filters

                    if (!string.IsNullOrEmpty(OptionalFilters))
                    {
                        foreach (var col in country.DefaultViewColumns)
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
                            var sql = Sql.Builder.Append(country.DefaultView);
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

                            sql.Append(" ORDER BY r.country_name ");
                            result = db.Page<vw_country>(Page, rowPerPage, sql);
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
                            var sql = Sql.Builder.Append(country.DefaultView);
                            sql.Append(" WHERE r.is_active = 1 ");
                            sql.Append(" AND r.id = @0 ", Id);
                            sql.Append(" ORDER BY r.country_name ");

                            result = db.Page<vw_country>(1, rowPerPage, sql);
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
