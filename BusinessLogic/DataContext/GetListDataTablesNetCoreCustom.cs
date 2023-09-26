using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess;
using DataAccess.PhoenixERP;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using NPoco;

namespace BusinessLogic
{
    public partial class DataContext
    {
        public DataTablesResponseNetCore GetListDataTablesMarvelBinderCustom<T>(IDataTablesRequest DataTableRequest,
            String OptionalFilters = "", String OptionalParameters = "", String OptionalQueryBuilder = "")
        {
            DataTablesResponseNetCore result = DataTablesResponseNetCore.Create(DataTableRequest,0,0, new List<T>());
            var parameters = new List<String>();
            int unfilteredCount = 0;
            var log = "";

            try
            {
                var qry = "";
                object appEntity = Activator.CreateInstance<T>();
                if (appEntity != null) qry = ((IEntityNoBaseEntity)appEntity).GetCustomView();
                if (String.IsNullOrEmpty(qry)) return result;
                qry += " WHERE r.id IS NOT NULL AND COALESCE(r.is_deleted, 0) = 0 ";

                // Role access filter
                qry += string.Format(" AND dbo.ufn_can_read( '{0}', '{1}', r.id, r.owner_id ) > 0 ",
                    AppUserId, ((IEntityNoBaseEntity)appEntity).GetEntityId());

                if (!string.IsNullOrEmpty(OptionalQueryBuilder))
                {
                    qry += string.Format(" AND {0} ", OptionalQueryBuilder);
                }

                #region Get unfiltered count

                using (var db = new PhoenixERPRepo())
                {
                    try
                    {
                        var qryUC = String.Format("SELECT COUNT(*) FROM ( {0} ) AS dt", qry);
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

                var sql = Sql.Builder.Append(qry);
                var viewColumns = ((IEntityNoBaseEntity)appEntity).GetCustomViewColumns();
                if (!String.IsNullOrEmpty(DataTableRequest.Search.Value))
                {
                    #region Set single parameter search

                    parameters.Clear();
                    parameters.Add(String.Format("%{0}%", DataTableRequest.Search.Value));
                    var filteredColumnCount = 0;

                    if (DataTableRequest.Search.IsRegex)
                    {
                        foreach (Column col in DataTableRequest.Columns)
                        {
                            if (!col.IsSearchable) continue;
                            if (!viewColumns.ContainsKey(col.Field)) continue;

                            if (filteredColumnCount == 0)
                            {
                                sql.Append(" AND ( " + viewColumns[col.Field] + " LIKE @0 ",
                                    String.Format("%{0}%", DataTableRequest.Search.Value));
                            }
                            else
                            {
                                sql.Append(" OR " + viewColumns[col.Field] + " LIKE @0 ",
                                    String.Format("%{0}%", DataTableRequest.Search.Value));
                            }

                            filteredColumnCount++;
                        }
                    }
                    else
                    {
                        foreach (Column col in DataTableRequest.Columns)
                        {
                            if (!col.IsSearchable) continue;
                            if (!viewColumns.ContainsKey(col.Field)) continue;

                            if (filteredColumnCount == 0)
                            {
                                sql.Append(" AND ( " + viewColumns[col.Field] + " LIKE @0 ",
                                    String.Format("%{0}%", DataTableRequest.Search.Value));
                            }
                            else
                            {
                                sql.Append(" OR " + viewColumns[col.Field] + " LIKE @0 ",
                                    String.Format("%{0}%", DataTableRequest.Search.Value));
                            }

                            filteredColumnCount++;
                        }
                    }

                    if (filteredColumnCount > 0)
                    {
                        sql.Append(" ) ");
                    }

                    #endregion
                }
                else
                {
                    #region Set multi parameters search

                    parameters.Clear();
                    var filteredColumnCount = 0;

                    var filteredColumns = DataTableRequest.Columns.Where(o => o.IsSearchable == true);
                    foreach (Column col in filteredColumns)
                    {
                        if (!col.IsSearchable || String.IsNullOrEmpty(col.Search.Value)) continue;
                        if (!viewColumns.ContainsKey(col.Field)) continue;

                        parameters.Add(String.Format("%{0}%", col.Search.Value));
                        sql.Append(" AND " + viewColumns[col.Field] + " LIKE @0 ",
                            String.Format("%{0}%", DataTableRequest.Search.Value));

                        filteredColumnCount++;
                    }

                    #endregion
                }

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

                #region Set row order

                sql.Append(" ORDER BY ");

                var sortedColumns = DataTableRequest.Columns.Where(o => o.IsSortable == true);
                if (sortedColumns != null && sortedColumns.Count() > 0)
                {
                    foreach (var col in sortedColumns)
                    {
                        log = String.Format("DataTables sorted columns = {0}", col.Field);
                        appLogger.Debug(log);

                        if (!viewColumns.ContainsKey(col.Field)) continue;

                        if (col.Sort != null)
                        {
                            sql.Append(String.Format(" {0} {1}, ", viewColumns[col.Field],
                            col.Sort.Direction == SortDirection.Descending ? "DESC" : "ASC"));
                        }                        
                    }
                }
                else
                {
                    var viewOrders = ((IEntityNoBaseEntity)appEntity).GetCustomViewOrders();
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
                    var rowPerPage = DataTableRequest.Length < 1 ? 1 : DataTableRequest.Length;
                    var pageNumber = (DataTableRequest.Start / rowPerPage) + 1;

                    try
                    {
                        var rows = db.Page<T>(pageNumber, rowPerPage, sql);
                        result = DataTablesResponseNetCore.Create(DataTableRequest, unfilteredCount,(int)rows.TotalItems, rows.Items);
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
