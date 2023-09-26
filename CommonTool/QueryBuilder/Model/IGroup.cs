using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTool.QueryBuilder.Model
{
    public interface IGroup
    {
        string condition { get; set; }

        bool not { get; set; }

        object data { get; set; }

        List<JsonRule> rules { get; set; }
    }
}
