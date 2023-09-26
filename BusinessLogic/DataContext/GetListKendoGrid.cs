using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess;
using DataAccess.PhoenixERP;
using CommonTool.KendoUI.Grid;
using NPoco;
using Newtonsoft.Json;
using static CommonTool.KendoUI.Filterable;

namespace BusinessLogic
{
    public partial class DataContext
    {
        public DataResponse GetListKendoUIGrid<T>(DataRequest DataTableRequest,String OptionalFilters = "", String OptionalParameters = "", String OptionalQueryBuilder = "")
        {
            DataResponse result = DataResponse.Create(DataTableRequest, 0, 0, new List<T>());
            var parameters = new List<String>();
            //int unfilteredCount = 0;
            var log = "";

            try
            {
                var qry = "";
                object appEntity = DataContext.GetBaseEntity<T>();
                if (appEntity != null) qry = ((IEntity)appEntity).GetDefaultView();
                if (String.IsNullOrEmpty(qry)) return result;
                //qry += " WHERE r.id IS NOT NULL AND COALESCE(r.is_deleted, 0) = 0 ";
                qry += " WHERE r.id IS NOT NULL ";

                // Role access filter
                //qry += string.Format(" AND dbo.ufn_can_read( '{0}', '{1}', r.id, r.owner_id ) > 0 ",
                //  AppUserId, ((IEntity)appEntity).GetEntityId());

                if (!string.IsNullOrEmpty(OptionalQueryBuilder))
                {
                    qry += string.Format(" AND {0} ", OptionalQueryBuilder);
                }

                var sql = Sql.Builder.Append(qry);
                var viewColumns = ((IEntity)appEntity).GetDefaultViewColumns();
                if (!String.IsNullOrEmpty(DataTableRequest.search.value))
                {
                    #region Set single parameter search

                    parameters.Clear();
                    parameters.Add(String.Format("%{0}%", DataTableRequest.search.value));
                    var filteredColumnCount = 0;

                    if (DataTableRequest.search.regex)
                    {
                        foreach (Column col in DataTableRequest.columns)
                        {
                            if (!col.searchable) continue;
                            if (!viewColumns.ContainsKey(col.field)) continue;

                            if (filteredColumnCount == 0)
                            {
                                sql.Append(" AND ( " + viewColumns[col.field] + " LIKE @0 ",
                                    String.Format("%{0}%", DataTableRequest.search.value));
                            }
                            else
                            {
                                sql.Append(" OR " + viewColumns[col.field] + " LIKE @0 ",
                                    String.Format("%{0}%", DataTableRequest.search.value));
                            }

                            filteredColumnCount++;
                        }
                    }
                    else
                    {
                        foreach (Column col in DataTableRequest.columns)
                        {
                            if (!col.searchable) continue;
                            if (!viewColumns.ContainsKey(col.field)) continue;

                            if (filteredColumnCount == 0)
                            {
                                sql.Append(" AND ( " + viewColumns[col.field] + " LIKE @0 ",
                                    String.Format("%{0}%", DataTableRequest.search.value));
                            }
                            else
                            {
                                sql.Append(" OR " + viewColumns[col.field] + " LIKE @0 ",
                                    String.Format("%{0}%", DataTableRequest.search.value));
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
                    #region Set column filterable 

                    parameters.Clear();
                    if (DataTableRequest.Filter != null)
                    {
                        Sql filterBuilder = MultiplekendoFilterable<T>(DataTableRequest.Filter);
                        if (filterBuilder != null)
                            sql.Append(filterBuilder.SQL, filterBuilder.Arguments);
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

                var sortedColumns = DataTableRequest.columns.Where(o => o.sortable == true);
                if (sortedColumns != null && sortedColumns.Count() > 0 && DataTableRequest.sort != null)
                {
                    foreach (var col in DataTableRequest.sort)
                    {
                        log = String.Format("DataTables sorted columns = {0}", col.field);
                        appLogger.Debug(log);

                        if (sortedColumns.Where(o => o.field == col.field).Count() == 0) continue;
                        if (!viewColumns.ContainsKey(col.field)) continue;

                        if (col.dir != null)
                        {
                            sql.Append(String.Format(" {0} {1}, ", viewColumns[col.field], col.dir));
                        }
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
                    var rowPerPage = DataTableRequest.pageSize;
                    var pageNumber = DataTableRequest.page;

                    try
                    {
                        var rows = db.Page<T>(pageNumber, rowPerPage, sql);
                        result = DataResponse.Create(DataTableRequest, (int)rows.TotalItems, (int)rows.TotalItems, rows.Items);
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


        //private Sql MultiplekendoFilterable<T>(FilterDataSource filter)
        //{
        //    object appEntity = DataContext.GetBaseEntity<T>();
        //    if (appEntity == null) return null;

        //    var viewColumns = ((IEntity)appEntity).GetDefaultViewColumns();
        //    Sql result = Sql.Builder.Append("  ");
        //    //var parameterizedColumnArgs = 0;
        //    if (filter.IsGroup() && filter.Filters.Count > 0)
        //    {
        //        var filterGroupLogic = filter.Logic.ToUpper();

        //        //result.Append(string.Format(" {0} ( ", filterGroupLogic)); //open ex: AND (
        //        result.Append(" AND ("); //open ex: AND (
        //        var totalFilteredColumn = 0;
        //        foreach (var f in filter.Filters)
        //        {
        //            if (f.IsGroup() && f.Filters.Count > 0)
        //            {
        //                var b = MultiplekendoFilterableInner<T>(f);
        //                if (b != null)
        //                {
        //                    if (totalFilteredColumn == 0)
        //                        result.Append(string.Format(" {0} ", b.SQL), b.Arguments);
        //                    else
        //                        result.Append(string.Format(" {0} {1} ", filterGroupLogic, b.SQL), b.Arguments);
        //                    totalFilteredColumn++;
        //                }
        //            }
        //            else
        //            {
        //                if (!viewColumns.ContainsKey(f.Field)) continue;
        //                var b = MultiplekendoFilterableInner<T>(f);
        //                appLogger.Info("SQL :");
        //                appLogger.Debug(b.SQL);
        //                appLogger.Debug(string.Join(",", b.Arguments));
        //                appLogger.Info("CLOSE SQL :");
        //                if (b != null)
        //                {
        //                    if (totalFilteredColumn == 0)
        //                        result.Append(string.Format(" {0} ", b.SQL), b.Arguments);
        //                    else
        //                        result.Append(string.Format(" {0} {1} ", filterGroupLogic, b.SQL), b.Arguments);
        //                    totalFilteredColumn++;
        //                }
        //            }

        //        }
        //        result.Append(" ) ");//close open ex: AND ( col1=1 )
        //    }
        //    else
        //    {
        //        if (!viewColumns.ContainsKey(filter.Field)) return null;

        //        switch (filter.OperatorEnum())
        //        {
        //            case EnumOperator.isempty:
        //            case EnumOperator.isnull:
        //            case EnumOperator.isnotempty:
        //            case EnumOperator.isnotnull:
        //                result.Append(string.Format(" {0} {1} {2} ", filter.Logic, viewColumns[filter.Field], filter.OperatorToSql()));
        //                break;
        //            default:
        //                result.Append(string.Format(" {0} {1} {2} @0", filter.Logic, viewColumns[filter.Field], filter.OperatorToSql()),
        //                    filter.OperatorEnum() == CommonTool.KendoUI.Filterable.EnumOperator.contains ? string.Format("%{0}%", filter.Value) : filter.Value);
        //                break;
        //        }


        //    }
        //    return result;
        //}

        //private Sql MultiplekendoFilterableInner<T>(FilterDataSource filter)
        //{
        //    object appEntity = DataContext.GetBaseEntity<T>();
        //    if (appEntity == null) return null;

        //    var viewColumns = ((IEntity)appEntity).GetDefaultViewColumns();
        //    Sql result = Sql.Builder.Append("  ");
        //    //var parameterizedColumnArgs = 0;
        //    if (filter.IsGroup() && filter.Filters.Count > 0)
        //    {
        //        var filterGroupLogic = filter.Logic.ToUpper();

        //        result.Append(" ("); //open ex: AND (
        //        var totalFilteredColumn = 0;
        //        foreach (var f in filter.Filters)
        //        {
        //            if (f.IsGroup() && f.Filters.Count > 0)
        //            {
        //                var b = MultiplekendoFilterableInner<T>(f);
        //                if (b != null)
        //                {
        //                    if (totalFilteredColumn == 0)
        //                        result.Append(string.Format(" {0} ", b.SQL), b.Arguments);
        //                    else
        //                        result.Append(string.Format(" {0} {1} ", filterGroupLogic, b.SQL), b.Arguments);
        //                    totalFilteredColumn++;
        //                }
        //            }
        //            else
        //            {
        //                if (!viewColumns.ContainsKey(f.Field)) continue;
        //                var b = MultiplekendoFilterable<T>(f);
        //                appLogger.Info("SQL :");
        //                appLogger.Debug(b.SQL);
        //                appLogger.Debug(string.Join(",", b.Arguments));
        //                appLogger.Info("CLOSE SQL :");
        //                if (b != null)
        //                {
        //                    if (totalFilteredColumn == 0)
        //                        result.Append(string.Format(" {0} ", b.SQL), b.Arguments);
        //                    else
        //                        result.Append(string.Format(" {0} {1} ", filterGroupLogic, b.SQL), b.Arguments);
        //                    totalFilteredColumn++;
        //                }
        //            }

        //        }
        //        result.Append(" ) ");//close open ex: AND ( col1=1 )
        //    }
        //    else
        //    {
        //        if (!viewColumns.ContainsKey(filter.Field)) return null;

        //        //result.Append(string.Format(" {0} {1} {2} @0", filter.Logic, viewColumns[filter.Field], filter.OperatorToSql()),
        //        //    filter.OperatorEnum() == CommonTool.KendoUI.Filterable.EnumOperator.contains ? string.Format("%{0}%", filter.Value) : filter.Value);
        //        switch (filter.OperatorEnum())
        //        {
        //            case EnumOperator.isempty:
        //            case EnumOperator.isnull:
        //            case EnumOperator.isnotempty:
        //            case EnumOperator.isnotnull:
        //                result.Append(string.Format(" {0} {1} {2} ", filter.Logic, viewColumns[filter.Field], filter.OperatorToSql()));
        //                break;
        //            default:
        //                result.Append(string.Format(" {0} {1} {2} @0", filter.Logic, viewColumns[filter.Field], filter.OperatorToSql()),
        //                    filter.OperatorEnum() == CommonTool.KendoUI.Filterable.EnumOperator.contains ? string.Format("%{0}%", filter.Value) : filter.Value);
        //                break;
        //        }


        //    }
        //    return result;
        //}
    }
}
