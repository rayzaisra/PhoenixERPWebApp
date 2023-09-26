using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenix.Shared.Core.Parameters
{
    public class PaginateQueryParameter<T> : QueryParameter<T>
    {
        public int limit { get; set; }

        public int offset { get; set; }
    }
}
