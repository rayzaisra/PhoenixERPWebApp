using Microsoft.EntityFrameworkCore;
using Phoenix.Shared.Core.Contexts;
using Phoenix.Shared.Core.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Shared.Core.Repositories
{
    public partial interface IEFRepository3<T,PK> where T : class
    {

        #region R Async
        //Task<IList<R>> FindAllAsync<R>(ExpressionParameter<T> parameter);
        //Task<IList<R>> FindAllAsync<R>(PaginateExpressionParameter<T> parameter);
        //Task<R> FindFirstOrDefaultAsync<R>(ExpressionParameter<T> parameter);
        //Task<int> CountAsync(ExpressionParameter<T> parameter); 
        #endregion
    }
    public partial class EFRepository3<T, PK>
        : IEFRepository3<T, PK> where T : class
    {

     

    }
}
