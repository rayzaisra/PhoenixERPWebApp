using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataTables.AspNet.AspNetCore
{
    public class DataTableRequestNetCore: IDataTablesRequest
    {
        public IDictionary<string, object> AdditionalParameters { get; private set; }
        public IEnumerable<IColumn> Columns { get; private set; }
        public int Draw { get; set; }
        public int Length { get; set; }
        public ISearch Search { get; set; }
        //public ISort Order { get; set; }
        public int Start { get; set; }

        public DataTableRequestNetCore(int draw, int start, int length, ISearch search, IEnumerable<IColumn> columns)
            : this(draw, start, length, search, columns, null)
        { }
        public DataTableRequestNetCore(int draw, int start, int length, ISearch search, IEnumerable<IColumn> columns, IDictionary<string, object> additionalParameters)
        {
            Draw = draw;
            Start = start;
            Length = length;
            Search = search;
            Columns = columns;
            AdditionalParameters = additionalParameters;
        }
    }
}
