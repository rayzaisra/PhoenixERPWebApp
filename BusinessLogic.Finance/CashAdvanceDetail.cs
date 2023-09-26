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
using CommonTool.JEasyUI.DataGrid;
using CommonTool.KendoUI.Grid;

namespace BusinessLogic.Services
{
    public partial class CashAdvanceDetail: CashAdvanceDetailService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public CashAdvanceDetail(DataContext AppUserData) : base(AppUserData)
        {

        }

        public override bool SaveEntity(cash_advance_detailDTO RecordDto, ref bool isNew)
        {
            bool result = false;

            if (RecordDto == null) return result;

            try
            {
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
                        foreach (var col in cash_advance_detail.DefaultViewColumns)
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
                            var sql = Sql.Builder.Append(cash_advance_detail.DefaultView);
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

                            sql.Append(" ORDER BY r.created_on DESC ");
                            result = db.Page<vw_cash_advance_detail>(page, rowPerPage, sql);
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
                            var sql = Sql.Builder.Append(cash_advance_detail.DefaultView);
                            sql.Append(" WHERE r.is_active = 1 ");
                            sql.Append(" AND r.id = @0 ", Id);
                            sql.Append(" ORDER BY r.created_on DESC ");
                            result = db.Page<vw_cash_advance_detail>(1, rowPerPage, sql);

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


        public DgResponse GetDataGridEasyUI(DgRequest DTRequest, String CashAdvanceId)
        {
            DgResponse result = null;
            try
            {
                var qry = String.Format(" r.cash_advance_id ='{0}'", CashAdvanceId);
                DTRequest.Orders = new List<dgSort>();
                DTRequest.Orders.Add(new dgSort()
                {
                    SortName = "row_index",
                    sortOrder = "ASC"
                });
                result = Context.GetDataGridEasyUI<vw_cash_advance_detail>(DTRequest, OptionalQueryBuilder: qry);
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }

            return result;
        }

        public DataResponse GetListKendoUIGrid(DataRequest DTRequest, String CashAdvanceId)
        {
            DataResponse result = null;
            try
            {
                var qry = String.Format(" r.cash_advance_id ='{0}'", CashAdvanceId);
                
                result = Context.GetListKendoUIGrid<vw_cash_advance_detail>(DTRequest, OptionalQueryBuilder: qry);
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
