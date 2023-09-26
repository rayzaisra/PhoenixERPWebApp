using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTool.MetronicDataTables
{
    public class mDatatableResponse
    {
        public object Data = new object();
        public Meta Meta = new Meta();

        public static mDatatableResponse Create<T>(mDatatableRequest dataTableRequest, int totalRecord = 0, object data = null)
        {
            mDatatableResponse result = new mDatatableResponse();
            result.Meta.Page = dataTableRequest.Pagination.Page;
            result.Meta.Perpage = dataTableRequest.Pagination.Perpage;
            result.Meta.Total = totalRecord;
            result.Data = data;
            return result;

        }
    }
}
