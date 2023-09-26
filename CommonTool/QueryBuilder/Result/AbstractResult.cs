using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTool.QueryBuilder.Result
{
    public abstract class AbstractResult
    {
        protected string queryJson;

        public abstract object GetQuery();

        public string GetQueryJson()
        {
            return queryJson;
        }
    }
}
