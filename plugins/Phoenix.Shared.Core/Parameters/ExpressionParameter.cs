using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Phoenix.Shared.Core.Parameters
{
    public class ExpressionParameter<T>
    {
        public Expression<Func<T, bool>> where { get; set; }
        public Expression<Func<T, object>>[] includes { get; set; }
        public string orderBy { get; set; }
    }
}
