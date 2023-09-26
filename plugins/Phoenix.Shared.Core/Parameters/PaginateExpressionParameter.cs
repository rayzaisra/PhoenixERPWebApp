using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Phoenix.Shared.Core.Parameters
{
    public class PaginateExpressionParameter<T> : ExpressionParameter<T>
    {
        public int limit { get; set; }
        
        public int offset { get; set; }
    }
}
