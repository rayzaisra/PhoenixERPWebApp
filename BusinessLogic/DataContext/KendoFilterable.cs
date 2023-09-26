using DataAccess;
using NPoco;
using static CommonTool.KendoUI.Filterable;

namespace BusinessLogic
{
    public partial class DataContext
    {
        public Sql MultiplekendoFilterable<T>(FilterDataSource filter)
        {
            if (filter.Filters.Count == 0) return null;

            object appEntity = DataContext.GetBaseEntity<T>();
            if (appEntity == null) return null;
            var viewColumns = ((IEntity)appEntity).GetDefaultViewColumns();
            Sql result = Sql.Builder.Append("  ");
            if (filter.IsGroup() && filter.Filters.Count > 0)
            {
                var filterGroupLogic = filter.Logic.ToUpper();
                result.Append(" AND ("); //open ex: AND (
                var totalFilteredColumn = 0;
                foreach (var f in filter.Filters)
                {
                    if (f.IsGroup() && f.Filters.Count > 0)
                    {
                        var b = MultiplekendoFilterableInner<T>(f);
                        if (b != null)
                        {
                            if (totalFilteredColumn == 0)
                                result.Append(string.Format(" {0} ", b.SQL), b.Arguments);
                            else
                                result.Append(string.Format(" {0} {1} ", filterGroupLogic, b.SQL), b.Arguments);
                            totalFilteredColumn++;
                        }
                    }
                    else
                    {
                        if (!viewColumns.ContainsKey(f.Field)) continue;
                        var b = MultiplekendoFilterableInner<T>(f);
                        if (b != null)
                        {
                            if (totalFilteredColumn == 0)
                                result.Append(string.Format(" {0} ", b.SQL), b.Arguments);
                            else
                                result.Append(string.Format(" {0} {1} ", filterGroupLogic, b.SQL), b.Arguments);
                            totalFilteredColumn++;
                        }
                    }

                }
                result.Append(" ) ");//close open ex: AND ( col1=1 )
            }
            else
            {
                if (!viewColumns.ContainsKey(filter.Field)) return null;

                switch (filter.OperatorEnum())
                {
                    case EnumOperator.isempty:
                    case EnumOperator.isnull:
                    case EnumOperator.isnotempty:
                    case EnumOperator.isnotnull:
                        result.Append(string.Format(" {0} {1} {2} ", filter.Logic, viewColumns[filter.Field], filter.OperatorToSql()));
                        break;
                    default:
                        result.Append(string.Format(" {0} {1} {2} @0", filter.Logic, viewColumns[filter.Field], filter.OperatorToSql()),
                            filter.OperatorEnum() == CommonTool.KendoUI.Filterable.EnumOperator.contains ? string.Format("%{0}%", filter.Value) : filter.Value);
                        break;
                }


            }
            return result;
        }

        private Sql MultiplekendoFilterableInner<T>(FilterDataSource filter)
        {
            object appEntity = DataContext.GetBaseEntity<T>();
            if (appEntity == null) return null;

            var viewColumns = ((IEntity)appEntity).GetDefaultViewColumns();
            Sql result = Sql.Builder.Append("  ");
            if (filter.IsGroup() && filter.Filters.Count > 0)
            {
                var filterGroupLogic = filter.Logic.ToUpper();
                result.Append(" ("); //open ex: AND (
                var totalFilteredColumn = 0;
                foreach (var f in filter.Filters)
                {
                    if (f.IsGroup() && f.Filters.Count > 0)
                    {
                        var b = MultiplekendoFilterableInner<T>(f);
                        if (b != null)
                        {
                            if (totalFilteredColumn == 0)
                                result.Append(string.Format(" {0} ", b.SQL), b.Arguments);
                            else
                                result.Append(string.Format(" {0} {1} ", filterGroupLogic, b.SQL), b.Arguments);
                            totalFilteredColumn++;
                        }
                    }
                    else
                    {
                        if (!viewColumns.ContainsKey(f.Field)) continue;
                        var b = MultiplekendoFilterableInner<T>(f);

                        if (b != null)
                        {
                            appLogger.Info("SQL :");
                            appLogger.Debug(b.SQL);
                            appLogger.Debug(string.Join(",", b.Arguments));

                            if (totalFilteredColumn == 0)
                                result.Append(string.Format(" {0} ", b.SQL), b.Arguments);
                            else
                                result.Append(string.Format(" {0} {1} ", filterGroupLogic, b.SQL), b.Arguments);
                            totalFilteredColumn++;
                        }
                    }

                }
                result.Append(" ) ");//close open ex: AND ( col1=1 )
            }
            else
            {
                if (!viewColumns.ContainsKey(filter.Field)) return null;
                switch (filter.OperatorEnum())
                {
                    case EnumOperator.isempty:
                    case EnumOperator.isnull:
                        result.Append(string.Format(" {0} {1} {2} ", filter.Logic, viewColumns[filter.Field], filter.OperatorToSql()));
                        break;
                    case EnumOperator.isnotempty:
                    case EnumOperator.isnotnull:
                        result.Append(string.Format(" {0} {1} {2} ", filter.Logic, viewColumns[filter.Field], filter.OperatorToSql()));
                        break;
                    default:
                        result.Append(string.Format(" {0} {1} {2} @0", filter.Logic, viewColumns[filter.Field], filter.OperatorToSql()),
                            filter.OperatorEnum() == CommonTool.KendoUI.Filterable.EnumOperator.contains ? string.Format("%{0}%", filter.Value) : filter.Value);
                        break;
                }


            }
            return result;
        }
    }
}
