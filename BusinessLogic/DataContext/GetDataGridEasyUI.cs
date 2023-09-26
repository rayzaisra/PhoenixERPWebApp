using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess;
using DataAccess.PhoenixERP;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using NPoco;
using CommonTool.JEasyUI.DataGrid;

namespace BusinessLogic
{
    public partial class DataContext
    {
        public DgResponse GetDataGridEasyUI<T>(DgRequest DataTableRequest,
            String OptionalFilters = "", String OptionalParameters = "", String OptionalQueryBuilder = "")
        {
            DgResponse result = new DgResponse();
            result.Data = new List<T>();
            result.Total = 0;

            var parameters = new List<String>();
            int unfilteredCount = 0;
            var log = "";
            
            try
            {
                var qry = "";
                object appEntity = DataContext.GetBaseEntity<T>();
                if (appEntity != null) qry = ((IEntity)appEntity).GetDefaultView();
                if (String.IsNullOrEmpty(qry)) return result;
                qry += " WHERE r.id IS NOT NULL AND COALESCE(r.is_deleted, 0) = 0 ";

                // Role access filter
                qry += string.Format(" AND dbo.ufn_can_read( '{0}', '{1}', r.id, r.owner_id ) > 0 ",
                    AppUserId, ((IEntity)appEntity).GetEntityId());

                if (!string.IsNullOrEmpty(OptionalQueryBuilder))
                {
                    qry += string.Format(" AND {0} ", OptionalQueryBuilder);
                }

                var sql = Sql.Builder.Append(qry);
                var viewColumns = ((IEntity)appEntity).GetDefaultViewColumns();

                if (!string.IsNullOrEmpty(DataTableRequest.Search))
                {
                    #region Set single parameter search

                    parameters.Clear();
                    parameters.Add(String.Format("%{0}%", DataTableRequest.Search));
                    var filteredColumnCount = 0;

                    foreach (dgGeneralFilterColumn col in DataTableRequest.GeneralFilterColumns)
                    {
                        if (!col.Filterable) continue;
                        if (!viewColumns.ContainsKey(col.Name)) continue;

                        if (filteredColumnCount == 0)
                        {
                            sql.Append(string.Format(" AND ( " + viewColumns[col.Name] + " LIKE '{0}' ",
                                String.Format("%{0}%", DataTableRequest.Search)));
                        }
                        else
                        {
                            sql.Append(string.Format(" OR " + viewColumns[col.Name] + " LIKE '{0}' ",
                                String.Format("%{0}%", DataTableRequest.Search)));
                        }

                        filteredColumnCount++;
                    }

                    if (filteredColumnCount > 0)
                    {
                        sql.Append(" ) ");
                    }

                    #endregion
                }

                #region Apply Filter Rules
                if (DataTableRequest.FilterRules != null)
                {

                    var filteredColumnCount = 0;
                    foreach (dgFilterRule col in DataTableRequest.FilterRules)
                    {
                        if (!viewColumns.ContainsKey(col.Field)) continue;

                        if (filteredColumnCount == 0)
                        {
                            sql.Append(string.Format(" AND ( " + viewColumns[col.Field] + " LIKE '{0}' ",
                                String.Format("%{0}%", col.Value)));
                        }
                        else
                        {
                            sql.Append(string.Format(" OR " + viewColumns[col.Field] + " LIKE '{0}' ",
                                String.Format("%{0}%", col.Value)));
                        }

                        filteredColumnCount++;
                    }

                    if (filteredColumnCount > 0)
                    {
                        sql.Append(" ) ");
                    }

                }
                #endregion

                #region Apply optional filters

                if (!String.IsNullOrEmpty(OptionalFilters) && !String.IsNullOrWhiteSpace(OptionalFilters))
                {
                    var filters = OptionalFilters;
                    foreach (var col in viewColumns)
                    {
                        filters = filters.Replace(" " + col.Key + " ", " " + col.Value + " ");
                    }

                    filters = filters.Replace(";", " -- ");

                    if (String.IsNullOrEmpty(OptionalParameters) || String.IsNullOrWhiteSpace(OptionalParameters))
                    {
                        sql.Append(String.Format(" AND ( {0} ) ", filters));
                    }
                    else
                    {
                        sql.Append(String.Format(" AND ( {0} ) ", filters),
                            OptionalParameters.Split(",".ToCharArray()));
                    }
                }

                #endregion

                #region Get unfiltered count

                using (var db = new PhoenixERPRepo())
                {
                    try
                    {
                        Sql qryUC = Sql.Builder.Append(string.Format("SELECT COUNT(*) FROM ( {0} ) AS dt ", sql.SQL));

                        unfilteredCount = db.ExecuteScalar<int>(qryUC);
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

                #region Set row order


                sql.Append(" ORDER BY ");


                if (DataTableRequest.Orders != null)
                {
                    var sortedColumns = DataTableRequest.Orders.Where(o => o.Sortable == true);
                    foreach (var col in sortedColumns)
                    {
                        log = String.Format("DataTables sorted columns = {0}", col.SortName);
                        appLogger.Debug(log);

                        if (!viewColumns.ContainsKey(col.SortName)) continue;

                        sql.Append(String.Format(" {0} {1}, ", viewColumns[col.SortName], string.IsNullOrEmpty(col.sortOrder) == true ? "ASC" : col.sortOrder));
                    }
                }
                else
                {
                    var viewOrders = ((IEntity)appEntity).GetDefaultViewOrders();
                    foreach (var col in viewOrders)
                    {
                        log = String.Format("Default sorted columns = {0}", col.Key);
                        appLogger.Debug(log);

                        sql.Append(String.Format(" {0} {1}, ", viewColumns[col.Key], col.Value));
                    }
                }

                sql.Append(" r.id ASC ");

                #endregion

                using (var db = new PhoenixERPRepo())
                {
                    long rowPerPage = DataTableRequest.PageSize ?? int.MaxValue;
                    rowPerPage = rowPerPage < 1 ? 1 : rowPerPage;
                    long pageNumber = (((DataTableRequest.PageNumber - 1) * rowPerPage) / rowPerPage) + 1;

                    try
                    {
                        appLogger.Debug(sql.SQL);
                        var rows = db.Page<T>(pageNumber, rowPerPage, sql);
                        result.Data = rows.Items;
                        result.Total = unfilteredCount;
                        result.Orders = DataTableRequest.Orders.Where(o => o.Sortable == true).ToList();
                        appLogger.Debug(Newtonsoft.Json.JsonConvert.SerializeObject(rows.Items));
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
            }
            catch (Exception ex)
            {
                appLogger.Error(ex.ToString());                
            }

            return result;
        }
    }
}
