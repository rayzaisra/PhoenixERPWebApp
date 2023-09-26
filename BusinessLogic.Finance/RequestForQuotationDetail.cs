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

namespace BusinessLogic.Finance
{
    public partial class RequestForQuotationDetail : RequestForQuotationDetailService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();
        public RequestForQuotationDetail(DataContext AppUserData) : base(AppUserData)
        {
                
        }

        public override bool Delete(string Id)
        {
            bool result = false;
            try
            {
                var record = Details(Id);
                if (record != null)
                {
                    if (record.is_locked ?? false)
                    {
                        throw new Exception("Can't delete data submited");
                    }

                    result = Svc.Delete(Id);
                }

            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }

            return result;
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
                        foreach (var col in request_for_quotation_detail.DefaultViewColumns)
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
                            var sql = Sql.Builder.Append(request_for_quotation_detail.DefaultView);
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

                            sql.Append(" ORDER BY r.item_description DESC ");
                            result = db.Page<vw_request_for_quotation_detail>(page, rowPerPage, sql);
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
                            var sql = Sql.Builder.Append(request_for_quotation_detail.DefaultView);
                            sql.Append(" WHERE r.is_active = 1 ");
                            sql.Append(" AND r.id = @0 ", Id);
                            sql.Append(" ORDER BY r.item_name DESC ");
                            result = db.Page<vw_request_for_quotation_detail>(1, rowPerPage, sql);
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


        public DgResponse GetDataGridEasyUI(DgRequest DTRequest, String RequestForQuotationId)
        {
            DgResponse result = null;
            try
            {
                var qry = String.Format(" r.request_for_quotation_id ='{0}'", RequestForQuotationId);
                DTRequest.Orders = new List<dgSort>();
                DTRequest.Orders.Add(new dgSort()
                {
                    SortName = "row_index",
                    sortOrder = "ASC"
                });
                result = Context.GetDataGridEasyUI<vw_request_for_quotation_detail>(DTRequest, OptionalQueryBuilder: qry);
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }

            return result;
        }

        public DataResponse GetListKendoUIGrid(DataRequest DTRequest, String RequestForQuotationId)
        {
            DataResponse result = null;
            try
            {
                var qry = String.Format(" r.request_for_quotation_id ='{0}'", RequestForQuotationId);
                result = Context.GetListKendoUIGrid<vw_request_for_quotation_detail>(DTRequest, OptionalQueryBuilder: qry);
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
