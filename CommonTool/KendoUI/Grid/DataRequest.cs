using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTool.KendoUI.Grid
{
    public class DataRequest
    {
        public int take { get; set; }
        public int skip { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }
        public List<Column> columns { get; set; }
        public List<Group> group { get; set; }
        public List<Sort> sort { get; set; }
        public Search search { get; set; }

        public Filterable.FilterDataSource Filter { get; set; }
    }

    public class Group
    {
        public String field { get; set; }
        public String dir { get; set; }
        public List<Aggregate> aggregates { get; set; } //{ field: "UnitPrice", aggregate: "sum" }
    }

    public class Aggregate {
        public String field { get; set; }
        public String aggregate { get; set; } 
    }

    public class Column
    {
        public String field { get; set; }
        public String title { get; set; }
        public String[] aggregates { get; set; } //["min", "max", "count"]
        public bool searchable { get; set; }
        public bool sortable { get; set; }
        
    }

    public class Search
    {
        public string value { get; set; }
        public bool regex { get; set; }
    }

    public class Sort {
        public String field { get; set; }
        public String dir { get; set; }
    }

   
}
