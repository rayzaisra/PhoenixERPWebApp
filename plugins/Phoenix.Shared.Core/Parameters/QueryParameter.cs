using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Phoenix.Shared.Core.Parameters
{
    public class QueryParameter<T>
    {
        public IQueryable<T> dbQuery { get; set; }
    }
}
