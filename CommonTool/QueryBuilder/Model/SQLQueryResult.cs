using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTool.QueryBuilder.Model
{
    public partial class SqlQueryResult
    {
        private string query;
        private List<object> parameters;

        public SqlQueryResult(string queryJson, string query, List<object> Parameters)
        {
            //this.queryJson = queryJson;
            this.query = query;
            parameters = Parameters;
        }

        public string GetQuery(bool WithParameters = false)
        {
            string qry = null;

            if (!WithParameters) return query;
            else
            { 
                return qry;
            }
        }
    }
}
