using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTool.KendoUI.Combobox
{
    public class ComboboxDataRequest
    {
        public int PageSize { get; set; }
        public int Page { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public FilterParameter Filter { get; set; }
        public Filterable.FilterDataSource Filter2 { get; set; }
    }

    public class FilterParameter {
        public string Logic { get; set; }
        public List<Filter> Filters = new List<Filter>();
    }

    public class Filter {
        public string Field { get; set; }
        public bool IgnoreCase { get; set; }
        public string Operator { get; set; }
        public object Value { get; set; }
    }

    public static class Helper {
        public static String OperatorConvert(String op) {
            string result = "AND";
            switch (op) {
                case "eq":
                    result = "=";
                    break;
                case "contains":
                    result = "LIKE";
                    break;
            }
            return result;
        } 


    }
}

