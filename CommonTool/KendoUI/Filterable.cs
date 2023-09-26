using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CommonTool.KendoUI
{
    public static class Filterable
    {

        public interface IFilter {
            string Field { get; set; }
            bool IgnoreCase { get; set; }
            string Operator { get; set; }
            string Value { get; set; }
            EnumOperator OperatorEnum();
            String OperatorToSql();
        }

        public interface IFilterParameter
        {
            string Logic { get; set; }
            List<FilterDataSource> Filters { get; set; }
        }

        //public class FilterParameter
        //{
        //    public string Logic { get; set; }
        //    public List<Filter> Filters = new List<Filter>();
        //}

        public class FilterDataSource: IFilter,IFilterParameter
        {
            #region IFilter
            public string Field { get; set; }
            public bool IgnoreCase { get; set; }
            public string Operator { get; set; }
            public string Value { get; set; }
            #endregion

            #region IFilterParameter
            public string Logic { get; set; }
            public List<FilterDataSource> Filters { get; set; }
            #endregion

            #region Constructor
            public FilterDataSource()
            {
                Filters = new List<FilterDataSource>();
            }
            #endregion

            public bool IsGroup()
            {
                return !string.IsNullOrEmpty(Logic);
            }

            public IFilter ToFilter()
            {
                return this;
            }

            public IFilterParameter ToFilterGroup()
            {
                return this;
            }

            public override string ToString()
            {
                return JsonConvert.SerializeObject(this);
            }

            public virtual EnumOperator OperatorEnum()
            {
                EnumOperator r = new EnumOperator();
                switch (Operator)
                {
                    case "eq":
                        r = EnumOperator.eq;
                        break;
                    case "neq":
                        r = EnumOperator.neq;
                        break;
                    case "isnull":
                        r = EnumOperator.isnull;
                        break;
                    case "isnotnull":
                        r = EnumOperator.isnotnull;
                        break;
                    case "lt":
                        r = EnumOperator.lt;
                        break;
                    case "lte":
                        r = EnumOperator.lte;
                        break;
                    case "gt":
                        r = EnumOperator.gt;
                        break;
                    case "gte":
                        r = EnumOperator.gte;
                        break;
                    case "startswith":
                        r = EnumOperator.startswith;
                        break;
                    case "endswith":
                        r = EnumOperator.endswith;
                        break;
                    case "contains":
                        r = EnumOperator.contains;
                        break;
                    case "doesnotcontain":
                        r = EnumOperator.doesnotcontain;
                        break;
                    case "isempty":
                        r = EnumOperator.isempty;
                        break;
                    case "isnotempty":
                        r = EnumOperator.isnotempty;
                        break;
                }
                return r;
            }

            public virtual String OperatorToSql()
            {
                var gen = string.Empty;
                switch (OperatorEnum())
                {
                    case EnumOperator.eq:
                        gen = "=";
                        break;
                    case EnumOperator.neq:
                        gen = "!=";
                        break;
                    case EnumOperator.isempty:
                    case EnumOperator.isnull:
                        gen = "IS NULL";
                        break;
                    case EnumOperator.isnotempty:
                    case EnumOperator.isnotnull:
                        gen = "IS NOT NULL";
                        break;
                    case EnumOperator.lt:
                        gen = "<";
                        break;
                    case EnumOperator.lte:
                        gen = "<=";
                        break;
                    case EnumOperator.gt:
                        gen = ">";
                        break;
                    case EnumOperator.gte:
                        gen = ">=";
                        break;
                    case EnumOperator.startswith:
                    case EnumOperator.endswith:
                    case EnumOperator.contains:
                        gen = "LIKE";
                        break;
                }
                return gen;
            }
        }

        public enum EnumOperator
        {
            [Description("equal to")]
            eq,
            [Description("not equal to")]
            neq,
            [Description("is equal to null")]
            isnull,
            [Description("is not equal to null")]
            isnotnull,
            [Description("less than")]
            lt,
            [Description("less than or equal to")]
            lte,
            [Description("greater than")]
            gt,
            [Description("greater than or equal to")]
            gte,
            startswith,
            endswith,
            contains,
            doesnotcontain,
            isempty,
            isnotempty

        }

    }
}
