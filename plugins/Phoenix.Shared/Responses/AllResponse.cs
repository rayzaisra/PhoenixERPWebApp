using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenix.Shared.Responses
{
    public class BaseGeneralResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Code { get; set; }
    }
    public class GeneralResponse<TData> : BaseGeneralResponse
    {
        public TData Data { get; set; }
    }
    public class GeneralResponseList<TData> : BaseGeneralResponse
    {
        public long Draw { get; set; }
        public IList<TData> Rows { get; set; }
        public long RecordsTotal { get; set; }
        public long RecordsFiltered { get; set; }
        public long Total { get; set; }
        public string Query { get; set; }
    }

    public class DataTableResponse<TData> : BaseGeneralResponse
    {
        public int sEcho { get; set; }
        public int iTotalRecords { get; set; }
        public int iTotalDisplayRecords { get; set; }
        public IList<TData> aaData { get; set; }
        public string sColumns { get; set; }
    }

    public class JwtResponse : BaseGeneralResponse
    {
        public string JwtCode { get; set; }

    }

    public class JwtResponseList<TData> : GeneralResponseList<TData>
    {
        public string JwtCode { get; set; }
    }
}
