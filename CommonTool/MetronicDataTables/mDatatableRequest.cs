using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTool.MetronicDataTables
{
    public class mDatatableRequest
    {
        public mQuery Query { get; set; }
        public mPagination Pagination { get; set; }
        public mSort Sort { get; set; }
        public List<mColumnFilter> columnsFilter { get; set; }
        
    }

    public class mQuery {
        public String search_value { get; set; }
    }

    public class mPagination {
        public int Page { get; set; }
        public int Perpage { get; set; }
        public int Total { get; set; }
    }

    public class mColumnFilter
    {
        public String Field { get; set; }
        public bool Filterable { get; set; }
    }



    public class mSort
    {
        public String Field { get; set; }
        public String Sort { get; set; }
    }


    public class Meta: mPagination
    {
        public String Field { get; set; }
        public String Sort { get; set; }
    }






}
