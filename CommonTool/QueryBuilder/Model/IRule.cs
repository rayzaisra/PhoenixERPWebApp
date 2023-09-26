using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTool.QueryBuilder.Model
{
    public interface IRule
    {
        string id { get; set; }

        string field { get; set; }

        string type { get; set; }

        string input { get; set; }

        string @operator { get; set; }

        object value { get; set; }

        object data { get; set; }
    }
}
