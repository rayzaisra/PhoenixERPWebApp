using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTool.DataTables.Marvel
{
    public class DataTableRequest
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }

        public DataTableOrder[] Order { get; set; }
        public DataTableColumn[] Columns { get; set; }
        public DataTableSearch Search { get; set; }
    }


    public class DataTableColumn
    {
        public string Data { get; set; }
        public int OrderNumber { get; set; }
        public string Name { get; set; }
        public bool Searchable { get; set; }
        public bool Orderable { get; set; }

        public DataTableSearch Search { get; set; }

    }

    public class DataTableSearch
    {
        public string Value { get; set; }
        public bool Regex { get; set; }
    }
    public class DataTableOrder
    {
        public int Column { get; set; }
        public string Dir { get; set; }
        public OrderDirection SortDirection { get; set; }
        public enum OrderDirection
        {
            Ascendant = 0,
            Descendant = 1
        }
    }
}
