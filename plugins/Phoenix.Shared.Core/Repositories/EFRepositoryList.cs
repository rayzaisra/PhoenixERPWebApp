using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Phoenix.Shared.Core.Contexts;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Parameters;
using Phoenix.Shared.Helpers;
using Phoenix.Shared.Libraries.SnowFlake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;

namespace Phoenix.Shared.Core.Repositories
{
    public interface IEFRepositoryList<T, PK> where T : GenericEntity<PK>
    {
        IList<T> Rawlist(string RawSql);
    }
    public partial class EFRepositoryList<T, PK>
        : IEFRepositoryList<T, PK> where T :  GenericEntity<PK>
    {
        protected DbSet<T> entities;
        protected readonly EFContext context;
        public EFRepositoryList(EFContext context)
        {
            this.context = context;
            entities = context.Set<T>();

        }
        public IList<T> Rawlist(string RawSql)
        {
            IList<T> list;
            list = entities.FromSql(RawSql).ToList();
            return list;
        }
    }
}
