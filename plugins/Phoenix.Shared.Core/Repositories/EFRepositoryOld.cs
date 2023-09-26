using Microsoft.EntityFrameworkCore;
using Phoenix.Shared.Core.Contexts;
using Phoenix.Shared.Core.Parameters;
using Phoenix.Shared.Libraries.SnowFlake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Shared.Core.Repositories
{
    public interface IEFRepository2<T,PK> where T : class
    {
        EFContext GetContext();
        Expression<Func<T, bool>> AsExpression(Expression<Func<T, bool>> where);
        IList<T> FindAll(params Expression<Func<T, object>>[] navigationProperties);
        IList<T> FindAll(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties);
        IList<T> FindAll(int limit, int offset, Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties);
        T FindFirstOrDefault(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties);
        int Count(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties);

        int CountTotal();
        IList<T> AddAll(params T[] items);
        IList<T> UpdateAll(params T[] items);
        void RemoveAll(params T[] items);
        T Add(T item);
        T Update(T item);
        void Remove(T item);
        T FindByID(PK id);



        Task<IList<T>> FindAllAsync(params Expression<Func<T, object>>[] navigationProperties);
        Task<IList<T>> FindAllAsync(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties);
        Task<IList<T>> FindAllAsync(int limit, int offset, Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties);
        Task<T> FindFirstOrDefaultAsync(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties);
        Task<int> CountAsync(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties);

        Task<int> CountTotalAsync();
        Task<IList<T>> AddAllAsync(params T[] items);
        Task<IList<T>> UpdateAllAsync(params T[] items);
        Task RemoveAllAsync(params T[] items);
        Task<T> AddAsync(T item);
        Task<T> UpdateAsync(T item);
        Task RemoveAsync(T item);
        Task<T> FindByIDAsync(PK id);



        IList<Object> FindAll(IQueryable<Object> dbQuery);
        IList<Object> FindAll(int limit, int offset, IQueryable<Object> dbQuery);
        int Count(IQueryable<Object> dbQuery);
        Object FindFirstOrDefault(IQueryable<Object> dbQuery);
        Task<IList<Object>> FindAllAsync(IQueryable<Object> dbQuery);
        Task<IList<Object>> FindAllAsync(int limit, int offset,IQueryable<Object> dbQuery);
        Task<int> CountAsync(IQueryable<Object> dbQuery);
        Task<Object> FindFirstOrDefaultAsync(IQueryable<Object> dbQuery);


        IList<X> FindAll<X>(IQueryable<Object> dbQuery);
        IList<X> FindAll<X>(int limit, int offset, IQueryable<Object> dbQuery);
        X FindFirstOrDefault<X>(IQueryable<Object> dbQuery);
        Task<IList<X>> FindAllAsync<X>(IQueryable<Object> dbQuery);
        Task<IList<X>> FindAllAsync<X>(int limit, int offset, IQueryable<Object> dbQuery);
        Task<X> FindFirstOrDefaultAsync<X>(IQueryable<Object> dbQuery);

        IList<T> FindAll(ExpressionParameter<T> parameter);
        IList<T> FindAll(PaginateExpressionParameter<T> parameter);
        T FindFirstOrDefault(ExpressionParameter<T> parameter);
        int Count(ExpressionParameter<T> parameter);

        Task<IList<T>> FindAllAsync(ExpressionParameter<T> parameter);
        Task<IList<T>> FindAllAsync(PaginateExpressionParameter<T> parameter);
        Task<T> FindFirstOrDefaultAsync(ExpressionParameter<T> parameter);
        Task<int> CountAsync(ExpressionParameter<T> parameter);
    }
    public partial class EFRepository2<T,PK> 
        : IEFRepository2<T,PK> where T : class
    {

        protected readonly EFContext context;
        protected DbSet<T> entities;
        string errorMessage = string.Empty;
        protected IdWorker _snowflakeID;

        public EFRepository2(EFContext context)
        {
            this.context = context;
            entities = context.Set<T>();
            _snowflakeID = new IdWorker();
        }

        public EFContext GetContext() {
            return context;
        }

        public virtual IList<T> FindAll(params Expression<Func<T, object>>[] navigationProperties)
        {
            IList<T> list;
            //using (var context = new EFContext())
            {
                //context.Configuration.ProxyCreationEnabled = false;
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<T, object>(navigationProperty);

                list = dbQuery
                    .AsNoTracking()
                    .ToList<T>();

            }
            return list;
        }

        public virtual IList<T> FindAll(Expression<Func<T, bool>> where,
            params Expression<Func<T, object>>[] navigationProperties)
        {
            IList<T> list;
            //using (var context = new EFContext())
            {
                //context.Configuration.ProxyCreationEnabled = false;
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<T, object>(navigationProperty);

                list = dbQuery
                    .AsNoTracking()
                    .Where(where)
                    .ToList<T>();
            }
            return list;
        }

        public virtual IList<T> FindAll(int limit, int offset, Expression<Func<T, bool>> where,
            params Expression<Func<T, object>>[] navigationProperties)
        {
            IList<T> list;
            //using (var context = new EFContext())
            {
                //context.Configuration.ProxyCreationEnabled = false;
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<T, object>(navigationProperty);

                list = dbQuery
                    .AsNoTracking()
                    .Where(where)
                    .Skip(offset).Take(limit)
                    .ToList<T>();
            }
            return list;
        }

        

        public virtual int Count(Expression<Func<T, bool>> where,
            params Expression<Func<T, object>>[] navigationProperties)
        {
            int total = 0;
            //using (var context = new EFContext())
            {
                //context.Configuration.ProxyCreationEnabled = false;
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<T, object>(navigationProperty);

                total = dbQuery
                    .AsNoTracking()
                    .Where(where)
                    .Count();
            }
            return total;
        }

        public virtual int CountTotal()
        {
            int total = 0;
            //using (var context = new EFContext())
            {
                //context.Configuration.ProxyCreationEnabled = false;
                IQueryable<T> dbQuery = context.Set<T>();

                total = dbQuery
                    .AsNoTracking()
                    .Count();
            }
            return total;
        }

        public virtual T FindFirstOrDefault(Expression<Func<T, bool>> where,
            params Expression<Func<T, object>>[] navigationProperties)
        {
            T item = null;
            //using (var context = new EFContext())
            {
                //context.Configuration.ProxyCreationEnabled = false;
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<T, object>(navigationProperty);

                item = dbQuery
                    .AsNoTracking() //Don't track any changes for the selected item
                    .FirstOrDefault(where); //Apply where clause
            }
            return item;
        }

        public virtual IList<T> AddAll(params T[] items)
        {
            //using (var context = new EFContext())
            {
                foreach (T item in items)
                {
                    //context.Set<T>().Add(item);
                    context.Entry(item).State = EntityState.Added;
                }
                context.SaveChanges();
            }
            return items;
        }

        public virtual IList<T> UpdateAll(params T[] items)
        {
            //using (var context = new EFContext())
            {
                foreach (T item in items)
                {
                    context.Entry(item).State = EntityState.Modified;
                }
                context.SaveChanges();
            }
            return items;
        }

        public virtual void RemoveAll(params T[] items)
        {
            //using (var context = new EFContext())
            {
                foreach (T item in items)
                {
                    context.Entry(item).State = EntityState.Deleted;
                }
                context.SaveChanges();
            }
        }

        public virtual T Add(T item)
        {
            //using (var context = new EFContext())
            {
                context.Entry(item).State = EntityState.Added;
                context.SaveChanges();
            }
            return item;
        }

        public virtual T Update(T item)
        {
            //using (var context = new EFContext())
            {
                context.Entry(item).State = EntityState.Modified;
                context.SaveChanges();
            }
            return item;
        }

        public virtual void Remove(T item)
        {
            //using (var context = new EFContext())
            {
                context.Entry(item).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public T FindByID(PK id)
        {
            return entities.Find(id);
        }


        public async Task<IList<T>> FindAllAsync(params Expression<Func<T, object>>[] navigationProperties)
        {
            IList<T> list;
            //using (var context = new EFContext())
            {
                //context.Configuration.ProxyCreationEnabled = false;
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<T, object>(navigationProperty);

                list = await dbQuery
                    .AsNoTracking()
                    .ToListAsync<T>();
            }
            return list;
        }

        public async Task<IList<T>> FindAllAsync(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties)
        {
            IList<T> list;
            //using (var context = new EFContext())
            {
                //context.Configuration.ProxyCreationEnabled = false;
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<T, object>(navigationProperty);

                list = await dbQuery
                    .AsNoTracking()
                    .Where(where)
                    .ToAsyncEnumerable()
                    .ToList();
                    //.ToAsyncEnumerable();
            }
            return list;
        }

        public async Task<IList<T>> FindAllAsync(int limit, int offset, Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties)
        {
            IList<T> list;
            //using (var context = new EFContext())
            {
                //context.Configuration.ProxyCreationEnabled = false;
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<T, object>(navigationProperty);

                list = await dbQuery
                    .AsNoTracking()
                    .Where(where)
                    .Skip(offset).Take(limit)
                    .ToAsyncEnumerable()
                    .ToList();
                //.AsQueryable()
                //.ToListAsync<T>();
            }
            return list;
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties)
        {
            int total = 0;
            //using (var context = new EFContext())
            {
                //context.Configuration.ProxyCreationEnabled = false;
                IQueryable<T> dbQuery = context.Set<T>();
                //Apply eager loading
                foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<T, object>(navigationProperty);

                total = await dbQuery
                    .Where(where)
                    .AsNoTracking()
                    //.AsNoTracking()
                    //.Where(where)

                    .CountAsync();
            }
            return total;
        }

        public async Task<int> CountTotalAsync()
        {
            int total = 0;
            //using (var context = new EFContext())
            {
                //context.Configuration.ProxyCreationEnabled = false;
                IQueryable<T> dbQuery = context.Set<T>();

                total = await dbQuery
                    .AsNoTracking()
                    .CountAsync();
            }
            return total;
        }

        public async Task<T> FindFirstOrDefaultAsync(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties)
        {
            T item = null;
            //using (var context = new EFContext())
            {
                //context.Configuration.ProxyCreationEnabled = false;
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<T, object>(navigationProperty);

                item = await dbQuery
                    .AsNoTracking() //Don't track any changes for the selected item
                    .Where(where)
                    .AsQueryable()
                    .FirstOrDefaultAsync();
            }
            return item;
        }

        public async Task<IList<T>> AddAllAsync(params T[] items)
        {
            //using (var context = new EFContext())
            {
                foreach (T item in items)
                {
                    //context.Set<T>().Add(item);
                    context.Entry(item).State = EntityState.Added;
                }
                await context.SaveChangesAsync();
            }
            return items;
        }

        public async Task<IList<T>> UpdateAllAsync(params T[] items)
        {
            //using (var context = new EFContext())
            {
                foreach (T item in items)
                {
                    context.Entry(item).State = EntityState.Modified;
                }
                await context.SaveChangesAsync();
            }
            return items;
        }

        public async Task RemoveAllAsync(params T[] items)
        {
            //using (var context = new EFContext())
            {
                foreach (T item in items)
                {
                    context.Entry(item).State = EntityState.Deleted;
                }
                await context.SaveChangesAsync();
            }
        }

        public async Task<T> AddAsync(T item)
        {
            //using (var context = new EFContext())
            {
                context.Entry(item).State = EntityState.Added;
                await context.SaveChangesAsync();
            }
            return item;
        }

        public async Task<T> UpdateAsync(T item)
        {
            //using (var context = new EFContext())
            {
                context.Entry(item).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
            return item;
        }

        public async Task RemoveAsync(T item)
        {
            //using (var context = new EFContext())
            {
                context.Entry(item).State = EntityState.Deleted;
                await context.SaveChangesAsync();
            }
        }

        public async Task<T> FindByIDAsync(PK id)
        {
            return await entities.FindAsync(id);
        }

        public Expression<Func<T, bool>> AsExpression(Expression<Func<T, bool>> where)
        {
            return where;
        }

        public IList<Object> FindAll(IQueryable<Object> dbQuery)
        {
            return dbQuery
                .AsNoTracking()
                .ToList();
        }

        public IList<Object> FindAll(int limit, int offset, IQueryable<Object> dbQuery)
        {
            return dbQuery
                    .AsNoTracking()
                    .Skip(offset)
                    .Take(limit)
                    .ToList();
        }

        public int Count(IQueryable<Object> dbQuery)
        {
            return dbQuery
                    .AsNoTracking()
                    .Count();
        }

        public Object FindFirstOrDefault(IQueryable<Object> dbQuery)
        {
            return dbQuery
                    .AsNoTracking()
                    .FirstOrDefault();
        }

        public async Task<IList<Object>> FindAllAsync(IQueryable<Object> dbQuery)
        {
            return await dbQuery
                   .AsNoTracking()
                   .ToListAsync();
        }

        public async Task<IList<Object>> FindAllAsync(int limit, int offset, IQueryable<Object> dbQuery)
        {
            return await dbQuery
                            .AsNoTracking()
                            .Skip(offset)
                            .Take(limit)
                            .ToListAsync();
        }

        public async Task<int> CountAsync(IQueryable<Object> dbQuery)
        {
            return await dbQuery
                            .AsNoTracking()
                            .CountAsync();
        }

        public async Task<Object> FindFirstOrDefaultAsync(IQueryable<Object> dbQuery)
        {
            return await dbQuery
                            .AsNoTracking()
                            .FirstOrDefaultAsync();
        }

        public IList<X> FindAll<X>(IQueryable<object> dbQuery)
        {
            return dbQuery
                    .AsNoTracking()
                    .Select(s => (X)s).ToList();
        }

        public IList<X> FindAll<X>(int limit, int offset, IQueryable<object> dbQuery)
        {
            return (IList<X>)dbQuery
                   .AsNoTracking()
                   .Skip(offset)
                   .Take(limit)
                   .Select(s => (X)s).ToList();
        }

        public X FindFirstOrDefault<X>(IQueryable<object> dbQuery)
        {
            return (X)dbQuery
                    .AsNoTracking()
                    .FirstOrDefault();
        }

        public async Task<IList<X>> FindAllAsync<X>(IQueryable<object> dbQuery)
        {
            var list = await dbQuery
                   .AsNoTracking()
                   .ToListAsync();
            return list.Select(s => (X)s).ToList();
        }

        public async Task<IList<X>> FindAllAsync<X>(int limit, int offset, IQueryable<object> dbQuery)
        {
            var list = await dbQuery
                   .AsNoTracking()
                   .Take(limit)
                   .Skip(offset)
                   .ToListAsync();
            return list.Select(s => (X)s).ToList();
        }

        public async Task<X> FindFirstOrDefaultAsync<X>(IQueryable<object> dbQuery)
        {
            var vl = await dbQuery
                            .AsNoTracking()
                            .FirstOrDefaultAsync();
            return (X)vl;
        }

        public IList<T> FindAll(ExpressionParameter<T> parameter)
        {
            IList<T> list;
            //using (var context = new EFContext())
            {
                //context.Configuration.ProxyCreationEnabled = false;
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                if (parameter.includes != null)
                {
                    foreach (Expression<Func<T, object>> navigationProperty in parameter.includes)
                        dbQuery = dbQuery.Include<T, object>(navigationProperty);
                }
                dbQuery = dbQuery.AsNoTracking();
                if(parameter.where!=null)
                    dbQuery = dbQuery.Where(parameter.where);
                if(!String.IsNullOrEmpty(parameter.orderBy))
                    dbQuery = dbQuery.OrderBy(parameter.orderBy);
                list = dbQuery
                    .ToList<T>();
            }
            return list;
        }

        public IList<T> FindAll(PaginateExpressionParameter<T> parameter)
        {

            IList<T> list;
            //using (var context = new EFContext())
            {
                //context.Configuration.ProxyCreationEnabled = false;
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                if (parameter.includes != null)
                {
                    foreach (Expression<Func<T, object>> navigationProperty in parameter.includes)
                        dbQuery = dbQuery.Include<T, object>(navigationProperty);
                }

                dbQuery = dbQuery.AsNoTracking();
                if (parameter.where != null)
                    dbQuery = dbQuery.Where(parameter.where);
                if (!String.IsNullOrEmpty(parameter.orderBy))
                    dbQuery = dbQuery.OrderBy(parameter.orderBy);
                dbQuery =  dbQuery.Skip(parameter.offset).Take(parameter.limit);
                list = dbQuery
                    .ToList<T>();
            }
            return list;
        }

        public T FindFirstOrDefault(ExpressionParameter<T> parameter)
        {
            T item = null;
            //using (var context = new EFContext())
            {
                //context.Configuration.ProxyCreationEnabled = false;
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                if (parameter.includes != null)
                {
                    foreach (Expression<Func<T, object>> navigationProperty in parameter.includes)
                        dbQuery = dbQuery.Include<T, object>(navigationProperty);
                }

                item = dbQuery
                    .AsNoTracking() //Don't track any changes for the selected item
                    .FirstOrDefault(parameter.where); //Apply where clause
            }
            return item;
        }

        public int Count(ExpressionParameter<T> parameter)
        {
            int total = 0;
            //using (var context = new EFContext())
            {
                //context.Configuration.ProxyCreationEnabled = false;
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                if (parameter.includes != null)
                {
                    foreach (Expression<Func<T, object>> navigationProperty in parameter.includes)
                        dbQuery = dbQuery.Include<T, object>(navigationProperty);
                }

                total = dbQuery
                    .AsNoTracking()
                    .Where(parameter.where)
                    .Count();
            }
            return total;
        }

        public async Task<IList<T>> FindAllAsync(ExpressionParameter<T> parameter)
        {
            IList<T> list;
            //using (var context = new EFContext())
            {
                //context.Configuration.ProxyCreationEnabled = false;
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                if (parameter.includes != null)
                {
                    foreach (Expression<Func<T, object>> navigationProperty in parameter.includes)
                        dbQuery = dbQuery.Include<T, object>(navigationProperty);
                }
                dbQuery = dbQuery.AsNoTracking();
                if (parameter.where != null)
                    dbQuery = dbQuery.Where(parameter.where);
                if (!String.IsNullOrEmpty(parameter.orderBy))
                    dbQuery = dbQuery.OrderBy(parameter.orderBy);
                list = await dbQuery
                    .ToAsyncEnumerable()
                    .ToList();
                //.ToAsyncEnumerable();
            }
            return list;
        }

        public async Task<IList<T>> FindAllAsync(PaginateExpressionParameter<T> parameter)
        {
            IList<T> list;
            //using (var context = new EFContext())
            {
                //context.Configuration.ProxyCreationEnabled = false;
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                if (parameter.includes != null)
                {
                    foreach (Expression<Func<T, object>> navigationProperty in parameter.includes)
                        dbQuery = dbQuery.Include<T, object>(navigationProperty);
                }
                dbQuery = dbQuery.AsNoTracking();
                if (parameter.where != null)
                    dbQuery = dbQuery.Where(parameter.where);
                if (!String.IsNullOrEmpty(parameter.orderBy))
                    dbQuery = dbQuery.OrderBy(parameter.orderBy);
                dbQuery = dbQuery.Skip(parameter.offset).Take(parameter.limit);
                list = await dbQuery
                    .ToAsyncEnumerable()
                    .ToList();
                //.ToAsyncEnumerable();
            }
            return list;
        }

        public async Task<T> FindFirstOrDefaultAsync(ExpressionParameter<T> parameter)
        {
            T item = null;
            //using (var context = new EFContext())
            {
                //context.Configuration.ProxyCreationEnabled = false;
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                if (parameter.includes != null)
                {
                    foreach (Expression<Func<T, object>> navigationProperty in parameter.includes)
                        dbQuery = dbQuery.Include<T, object>(navigationProperty);
                }

                dbQuery = dbQuery.AsNoTracking();
                if (parameter.where != null)
                    dbQuery = dbQuery.Where(parameter.where);

                item = await dbQuery
                    //.AsQueryable()
                    .FirstOrDefaultAsync();
            }
            return item;
        }

        public async Task<int> CountAsync(ExpressionParameter<T> parameter)
        {
            int total = 0;
            //using (var context = new EFContext())
            {
                //context.Configuration.ProxyCreationEnabled = false;
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                if (parameter.includes != null)
                {
                    foreach (Expression<Func<T, object>> navigationProperty in parameter.includes)
                        dbQuery = dbQuery.Include<T, object>(navigationProperty);
                }

                dbQuery = dbQuery.AsNoTracking();
                if (parameter.where != null)
                    dbQuery = dbQuery.Where(parameter.where);

                total = await dbQuery
                    //.AsQueryable()
                    .CountAsync();
            }
            return total;
        }



        /* rest of code omitted */
    }
}
