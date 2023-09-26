using System;
using System.Collections.Generic;
using System.Text;
using DataTables.AspNet.Core;
using DataAccess;

namespace BusinessLogic
{
    public class DataTableTestRequest : DataTables.AspNet.Core.IDataTablesRequest
    {
        public IDictionary<string, object> AdditionalParameters { get; private set; }
        public IEnumerable<IColumn> Columns { get; private set; }
        public int Draw { get; private set; }
        public int Length { get; private set; }
        public ISearch Search { get; private set; }
        public int Start { get; private set; }

        public DataTableTestRequest(int draw, int start, int length, ISearch search, IEnumerable<IColumn> columns)
            : this(draw, start, length, search, columns, null)
        { }
        public DataTableTestRequest(int draw, int start, int length, ISearch search, IEnumerable<IColumn> columns, IDictionary<string, object> additionalParameters)
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
