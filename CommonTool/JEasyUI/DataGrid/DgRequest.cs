using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTool.JEasyUI.DataGrid
{
    public class DgRequest
    {
        public int PageNumber { get; set; }
        public int? PageSize { get; set; }
        public List<dgSort> Orders { get; set; }
        public List<dgGeneralFilterColumn> GeneralFilterColumns { get; set; }
        public List<dgFilterRule> FilterRules { get; set; }
        public String Search { get; set; }
    }

    public class dgPagination
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class dgSort
    {
        public String SortName { get; set; }
        public String sortOrder { get; set; }
        public bool Sortable = true;
    }

    public class dgGeneralFilterColumn
    {
        public String Name { get; set; }
        public bool Filterable { get; set; }
    }

    public class dgFilterRule
    {
        public String Field { get; set; }
        public String OP { get; set; }
        public String Value { get; set; }
    }


    public class dgGeneralSearch
    {
        public String search_value { get; set; }
    }


}
