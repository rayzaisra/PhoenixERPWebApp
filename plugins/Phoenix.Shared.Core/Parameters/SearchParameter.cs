using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Phoenix.Shared.Core.Parameters
{
    public class SearchParameter
    {
        public string search { get; set; }
        public string type { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string sortDir { get; set; }
        public string sortBy { get; set; }
        public int rows { get; set; }
        public int page { get; set; }
       
        public string GetSearch()
        {
            return search == null ? "" : search;
        }
        public int GetLimit()
        {
            return rows==0?10:rows;
        }
        public int GetOffset()
        {
            page = page == 0 ? page + 1 : page;
            return (page - 1)*rows;
        }
        public string GetType()
        {
            return type == null ? "" : type;
        }
    }
}
