using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Linq;
using BusinessLogic.Services.Finance;
using NLog;
using DataAccess.PhoenixERP.Finance;
using DataAccess.PhoenixERP;
using NPoco;

namespace BusinessLogic.Services
{
    public partial class FinancialYear: FinancialYearService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public FinancialYear(DataContext AppUserData) : base(AppUserData)
        {

        }

        public override bool SaveEntity(financial_yearDTO RecordDto, ref bool isNew)
        {
            bool result = false;

            if (RecordDto == null) return result;

            try
            {
                RecordDto.start_date = RecordDto.start_date.Date;
                RecordDto.end_date = RecordDto.end_date.Date.AddDays(1).AddSeconds(-1);
                result = Svc.SaveEntity(RecordDto, ref isNew);
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }

            return result;
        }

        public dynamic Lookup(string Id = null, int page = 1, int length = 100, 
            string optionalFilters = null, string optionalParameters = null)
        {
            dynamic result = null;
            try
            {
                if (string.IsNullOrEmpty(Id))
                {
                    var filters = "";
                    var parameters = "";

                    #region Use filters

                    if (!string.IsNullOrEmpty(optionalFilters))
                    {
                        foreach (var col in financial_year.DefaultViewColumns)
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
                            var sql = Sql.Builder.Append(financial_year.DefaultView);
                            sql.Append("WHERE COALESCE(r.is_active, 0) = 1 ");
                            sql.Append("AND COALESCE(r.is_deleted, 0) = 0 ");

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

                            sql.Append("ORDER BY r.financial_year_name DESC");
                            result = db.Page<vw_financial_year>(page, rowPerPage, sql);
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
                            var sql = Sql.Builder.Append(financial_year.DefaultView);
                            sql.Append("WHERE COALESCE(r.is_active, 0) = 1 ");
                            sql.Append("AND COALESCE(r.is_deleted, 0) = 0 ");
                            sql.Append("AND r.id = @0 ", Id);
                            sql.Append("ORDER BY r.financial_year_name DESC");
                            result = db.Page<vw_financial_year>(1, rowPerPage, sql);
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