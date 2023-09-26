using BusinessLogic.Services.Finance;
using DataAccess.PhoenixERP.Finance;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using Omu.ValueInjecter;
using DataAccess.PhoenixERP;
using NPoco;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using System.Linq;
using DataAccess.PhoenixERP.General;
using DataAccess.Dto.Custom.Finance;
using CommonTool.KendoUI.Grid;

namespace BusinessLogic.Finance
{
    public partial class InvoicePayableApproval : InvoicePayableApprovalService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public InvoicePayableApproval(DataContext AppUserData) : base(AppUserData)
        {
        }
        

        public List<vw_invoice_payable_approvalDTO> list(String recordId)
        {
            List<vw_invoice_payable_approvalDTO> results = null;
            using (var db = new PhoenixERPRepo())
            {
                try
                {
                    var sql = Sql.Builder.Append(invoice_payable_approval.DefaultView);
                    sql.Append(" WHERE r.record_id=@0", recordId);
                    sql.Append(" ORDER BY r.created_on DESC ");
                    var data = db.Fetch<vw_invoice_payable_approval>(sql);
                    appLogger.Debug("TESTT"+ db.LastCommand);
                    results = data.Select(x => new vw_invoice_payable_approvalDTO().InjectFrom(x)).Cast<vw_invoice_payable_approvalDTO>().ToList();
                }
                catch (Exception ex)
                {
                    appLogger.Debug(db.LastCommand);
                    appLogger.Error(ex);
                    throw;
                }
            }
            return results;
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
                        foreach (var col in invoice_payable_approval.DefaultViewColumns)
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
                            var sql = Sql.Builder.Append(invoice_payable_approval.DefaultView);
                            sql.Append(" AND COALESCE(r.is_deleted, 0) = 0 ");
                            sql.Append(" AND COALESCE(r.is_active, 0) = 1 ");

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

                            sql.Append(" ORDER BY r.status_name ");
                            result = db.Page<vw_invoice_payable_approval>(page, rowPerPage, sql);

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
                            var sql = Sql.Builder.Append(invoice_payable_approval.DefaultView);
                            sql.Append(" WHERE r.is_active = 1 ");
                            sql.Append(" AND r.id = @0 ", Id);
                            sql.Append(" ORDER BY r.status_name ");
                            result = db.Page<vw_invoice_payable_approval>(1, rowPerPage, sql);

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

        public DataResponse GetListKendoUIGrid(DataRequest DTRequest,String recordId)
        {
            DataResponse result = null;
            try
            {
                var qry = String.Format(" r.record_id ='{0}'", recordId);
                result = GetListKendoGrid(DTRequest, OptionalQueryBuilder: qry);
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
