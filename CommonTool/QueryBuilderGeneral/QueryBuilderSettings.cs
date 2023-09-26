using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTool.QueryBuilderGeneral
{
    using System;
    using System.Diagnostics;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    [Serializable]
    public class QueryBuilderSettings
    {
        public List<QueryBuilderFilter> filters { get; set; }
        public List<string> plugins { get; set; }

        public QueryBuilderSettings()
        {
            this.filters = new List<QueryBuilderFilter>();
            this.plugins = new List<string>();
        }
    }

    public class QueryBuilderFilter
    {
        public string id { get; set; }
        public string label { get; set; }
        public string type { get; set; }
        public List<string> operators { get; set; }
        public string input { get; set; }
        //public List<object> values { get; set; }
        public Dictionary<string, string> values { get; set; }

        public QueryBuilderFilter()
        {

        }

        public QueryBuilderFilter(string id, string label, QueryBuilderDataType type, List<QueryBuilderFilterOperators> ops, QueryBuilderInputType input, Dictionary<string, string> values)
        {
            this.id = id;
            this.label = label;
            this.type = type.ToString();
            this.operators = new List<string>();
            foreach (QueryBuilderFilterOperators op in ops)
            {
                this.operators.Add(op.ToString());
            }
            this.input = input.ToString();
            this.values = values;
        }

        public static QueryBuilderDataType GetQueryBuilderDataType(string PropertyInputType)
        {
            QueryBuilderDataType QBFilterType;

            switch (PropertyInputType)
            {
                case "bool":
                    QBFilterType = QueryBuilderDataType.boolean;
                    break;
                case "DateTime":
                    QBFilterType = QueryBuilderDataType.datetime;
                    break;
                case "Date":
                    QBFilterType = QueryBuilderDataType.date;
                    break;
                case "Time":
                    QBFilterType = QueryBuilderDataType.time;
                    break;
                case "double":
                    QBFilterType = QueryBuilderDataType.@double;
                    break;
                case "int":
                    QBFilterType = QueryBuilderDataType.integer;
                    break;
                case "enum_dropdown":
                case "dropdown":
                case "html":
                case "stringChar":
                case "stringLine":
                    QBFilterType = QueryBuilderDataType.@string;
                    break;
                default:
                    QBFilterType = QueryBuilderDataType.@string;
                    break;
            }

            return QBFilterType;
        }

        public static QueryBuilderInputType GetQueryBuilderInputType(string PropertyType)
        {
            QueryBuilderInputType QBInputType;

            switch (PropertyType)
            {
                case "Boolean":
                case "bool":
                    QBInputType = QueryBuilderInputType.radio;
                    break;

                case "enum":
                    QBInputType = QueryBuilderInputType.select;
                    break;

                case "DateTime":
                case "double":
                case "int":
                case "string":
                    QBInputType = QueryBuilderInputType.text;
                    break;
                default:
                    QBInputType = QueryBuilderInputType.text;
                    break;
            }

            return QBInputType;
        }

        public static List<QueryBuilderFilterOperators> GetQueryBuilderFilterOperator(QueryBuilderInputType? QBInputType)
        {
            List<QueryBuilderFilterOperators> QBFilterOps = new List<QueryBuilderFilterOperators>();
            switch (QBInputType)
            {
                case QueryBuilderInputType.text:
                    QBFilterOps = DefaultTextOperators;
                    break;
                case QueryBuilderInputType.textarea:
                    QBFilterOps = DefaultTextAreaOperators;
                    break;
                case QueryBuilderInputType.radio:
                    QBFilterOps = DefaultRadioOperators;
                    break;
                case QueryBuilderInputType.checkbox:
                    QBFilterOps = DefaultCheckBoxOperators;
                    break;
                case QueryBuilderInputType.select:
                    QBFilterOps = DefaultSelectOperators;
                    break;
                case null:
                    QBFilterOps = DefaultBlankOperators;
                    break;
                default:
                    QBFilterOps = DefaultAllOperators;
                    break;
            }

            return QBFilterOps;
        }

        public static List<QueryBuilderFilterOperators> DefaultTextOperators = new List<QueryBuilderFilterOperators>
        {
            QueryBuilderFilterOperators.equal,
            QueryBuilderFilterOperators.not_equal,
            QueryBuilderFilterOperators.@in,
            QueryBuilderFilterOperators.not_in,
            QueryBuilderFilterOperators.less,
            QueryBuilderFilterOperators.less_or_equal,
            QueryBuilderFilterOperators.greater,
            QueryBuilderFilterOperators.greater_or_equal,
            QueryBuilderFilterOperators.between,
            QueryBuilderFilterOperators.not_between,
            QueryBuilderFilterOperators.begins_with,
            QueryBuilderFilterOperators.not_begins_with,
            QueryBuilderFilterOperators.contains,
            QueryBuilderFilterOperators.not_contains,
            QueryBuilderFilterOperators.ends_with,
            QueryBuilderFilterOperators.not_ends_with,
            QueryBuilderFilterOperators.is_empty,
            QueryBuilderFilterOperators.is_not_empty,
            QueryBuilderFilterOperators.is_null,
            QueryBuilderFilterOperators.is_not_null
        };

        public static List<QueryBuilderFilterOperators> DefaultTextAreaOperators = new List<QueryBuilderFilterOperators>
        {
            QueryBuilderFilterOperators.equal,
            QueryBuilderFilterOperators.not_equal,
            QueryBuilderFilterOperators.@in,
            QueryBuilderFilterOperators.not_in,
            QueryBuilderFilterOperators.less,
            QueryBuilderFilterOperators.less_or_equal,
            QueryBuilderFilterOperators.greater,
            QueryBuilderFilterOperators.greater_or_equal,
            QueryBuilderFilterOperators.between,
            QueryBuilderFilterOperators.not_between,
            QueryBuilderFilterOperators.begins_with,
            QueryBuilderFilterOperators.not_begins_with,
            QueryBuilderFilterOperators.contains,
            QueryBuilderFilterOperators.not_contains,
            QueryBuilderFilterOperators.ends_with,
            QueryBuilderFilterOperators.not_ends_with,
            QueryBuilderFilterOperators.is_empty,
            QueryBuilderFilterOperators.is_not_empty,
            QueryBuilderFilterOperators.is_null,
            QueryBuilderFilterOperators.is_not_null
        };

        public static List<QueryBuilderFilterOperators> DefaultRadioOperators = new List<QueryBuilderFilterOperators>
        {
            QueryBuilderFilterOperators.equal,
            QueryBuilderFilterOperators.not_equal,
            QueryBuilderFilterOperators.is_empty,
            QueryBuilderFilterOperators.is_not_empty,
            QueryBuilderFilterOperators.is_null,
            QueryBuilderFilterOperators.is_not_null
        };


        public static List<QueryBuilderFilterOperators> DefaultCheckBoxOperators = new List<QueryBuilderFilterOperators>
        {
            QueryBuilderFilterOperators.@in,
            QueryBuilderFilterOperators.not_in,
            QueryBuilderFilterOperators.is_empty,
            QueryBuilderFilterOperators.is_not_empty,
            QueryBuilderFilterOperators.is_null,
            QueryBuilderFilterOperators.is_not_null
        };

        public static List<QueryBuilderFilterOperators> DefaultSelectOperators = new List<QueryBuilderFilterOperators>
        {
            QueryBuilderFilterOperators.equal,
            QueryBuilderFilterOperators.not_equal,
            QueryBuilderFilterOperators.is_empty,
            QueryBuilderFilterOperators.is_not_empty,
            QueryBuilderFilterOperators.is_null,
            QueryBuilderFilterOperators.is_not_null
        };

        public static List<QueryBuilderFilterOperators> DefaultBlankOperators = new List<QueryBuilderFilterOperators>();

        public static List<QueryBuilderFilterOperators> DefaultAllOperators = new List<QueryBuilderFilterOperators>
        {
            QueryBuilderFilterOperators.equal,
            QueryBuilderFilterOperators.not_equal,
            QueryBuilderFilterOperators.@in,
            QueryBuilderFilterOperators.not_in,
            QueryBuilderFilterOperators.less,
            QueryBuilderFilterOperators.less_or_equal,
            QueryBuilderFilterOperators.greater,
            QueryBuilderFilterOperators.greater_or_equal,
            QueryBuilderFilterOperators.between,
            QueryBuilderFilterOperators.not_between,
            QueryBuilderFilterOperators.begins_with,
            QueryBuilderFilterOperators.not_begins_with,
            QueryBuilderFilterOperators.contains,
            QueryBuilderFilterOperators.not_contains,
            QueryBuilderFilterOperators.ends_with,
            QueryBuilderFilterOperators.not_ends_with,
            QueryBuilderFilterOperators.is_empty,
            QueryBuilderFilterOperators.is_not_empty,
            QueryBuilderFilterOperators.is_null,
            QueryBuilderFilterOperators.is_not_null
        };


        public enum QueryBuilderDataType
        {
            @string,
            @integer,
            @double,
            @date,
            @time,
            @datetime,
            @boolean
        }

        public enum QueryBuilderFilterOperators
        {
            equal,
            not_equal,
            @in,
            not_in,
            less,
            less_or_equal,
            greater,
            greater_or_equal,
            between,
            not_between,
            begins_with,
            not_begins_with,
            contains,
            not_contains,
            ends_with,
            not_ends_with,
            is_empty,
            is_not_empty,
            is_null,
            is_not_null
        }

        public enum QueryBuilderInputType
        {
            text,
            textarea,
            radio,
            checkbox,
            select
        }
    }
}