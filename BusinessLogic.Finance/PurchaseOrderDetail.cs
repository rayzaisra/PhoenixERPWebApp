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
    public partial class PurchaseOrderDetail : PurchaseOrderDetailService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();
        public PurchaseOrderDetail(DataContext AppUserData) : base(AppUserData)
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
                        foreach (var col in purchase_order.DefaultViewColumns)
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
                            var sql = Sql.Builder.Append(purchase_order_detail.DefaultView);
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
                            result = db.Page<vw_purchase_order_detail>(page, rowPerPage, sql);
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
                            var sql = Sql.Builder.Append(purchase_order_detail.DefaultView);
                            sql.Append(" WHERE r.is_active = 1 ");
                            sql.Append(" AND r.id = @0 ", Id);
                            sql.Append(" ORDER BY r.item_description DESC ");
                            result = db.Page<vw_purchase_order_detail>(1, rowPerPage, sql);
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


        public DgResponse GetDataGridEasyUI(DgRequest DTRequest, String PurchaseOrderId)
        {
            DgResponse result = null;
            try
            {
                var qry = String.Format(" r.purchase_order_id ='{0}'", PurchaseOrderId);
                DTRequest.Orders = new List<dgSort>();
                DTRequest.Orders.Add(new dgSort()
                {
                    SortName = "row_index",
                    sortOrder = "ASC"
                });
                result = Context.GetDataGridEasyUI<vw_purchase_order_detail>(DTRequest, OptionalQueryBuilder: qry);
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }

            return result;
        }


        public DataResponse GetListKendoUIGrid(DataRequest DTRequest, String purchaseOrderId)
        {
            DataResponse result = null;
            try
            {

                var qry = String.Format(" r.purchase_order_id ='{0}' ", purchaseOrderId);
                result = GetListKendoGrid(DTRequest, OptionalQueryBuilder: qry);

            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }

            return result;
        }

        public DataResponse GetListKendoUIGridFromGoodReceipt(DataRequest DTRequest, String purchaseOrderId)
        {
            DataResponse result = null;
            try
            {
                var qry = String.Format(" r.purchase_order_id ='{0}' AND (r.is_delivered = 0 OR r.is_delivered IS NULL) ", purchaseOrderId);
                qry += string.Format("AND NOT r.qty = (select fn.GetSumReceivedItemFromGoodsReceived(r.id)) ");
                result = GetListKendoGrid(DTRequest, OptionalQueryBuilder: qry);
                List<vw_purchase_order_detail> listDetail = new List<vw_purchase_order_detail>();

                using (var db = new PhoenixERPRepo())
                {
                    if (result.Data != null && ((List<vw_purchase_order_detail>)result.Data).Count() > 0)
                    {
                        // check if any in Goods Receipt detail
                        listDetail = (List<vw_purchase_order_detail>)result.Data;
                        foreach (var item in listDetail)
                        {
                            string query = string.Format(" select fn.GetSumReceivedItemFromGoodsReceived('{0}') ", item.id);
                            var sql = Sql.Builder.Append(query);

                            var total = db.Fetch<int>(sql);
                            int total_received = total[0];
                            item.total_received_qty = total_received;
                        }
                    }
                    else
                    {
                        // create new Goods receipt detail
                        qry = String.Format(" r.purchase_order_id ='{0}' AND (r.is_delivered = 0 OR r.is_delivered IS NULL) ", purchaseOrderId);
                        result = GetListKendoGrid(DTRequest, OptionalQueryBuilder: qry);
                    }

                     if (result.Data != null && ((List<vw_purchase_order_detail>)result.Data).Count() > 0)
                    {
                        // Change id => for inventory_goods_detail (Upload purpose)
                        listDetail = (List<vw_purchase_order_detail>)result.Data;
                        foreach (var item in listDetail)
                        {
                            item.inventory_goods_receipt_detail_id = Guid.NewGuid().ToString();
                        }
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
    }
}
