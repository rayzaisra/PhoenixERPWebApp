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
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Shared.Core.Repositories
{
    public delegate Object TransactionProcess();
    public partial interface IEFRepository<T, PK> where T : BaseEntity<PK>//class
    {
        EFContext GetContext();
        DbSet<T> GetEntities();
        Boolean ProcessingTransaction(TransactionProcess transactionProcess);
        IDbContextTransaction GetTransaction();
        void Dispose();
        void SaveChanges();
        #region R Sync
        int CountTotal();
        IList<T> CreateAll(params T[] items);
        IList<T> UpdateAll(params T[] items);
        void RemoveAll(params T[] items);
        T Create(T item);
        T Update(T item);
        void Remove(T item);
        IList<T> CreateAll(bool isSaveChanges, params T[] items);
        IList<T> UpdateAll(bool isSaveChanges, params T[] items);
        void RemoveAll(bool isSaveChanges, params T[] items);
        T Create(bool isSaveChanges, T item);
        T Update(bool isSaveChanges, T item);
        void Remove(bool isSaveChanges, T item);
        T FindByID(PK id);
        void RemoveByID(bool isSaveChanges, PK id);
        void RemoveByID(PK id);
        void RemoveAllByID(bool isSaveChanges, PK[] ids);
        void RemoveAllByID(PK[] ids);
        int Count(ExpressionParameter<T> parameter);
        int Count(QueryParameter<T> parameter);
        bool IsExist(ExpressionParameter<T> parameter);
        bool IsExist(QueryParameter<T> parameter);

        IList<R> FindAll<R>(ExpressionParameter<T> parameter);
        IList<R> FindAll<R>(PaginateExpressionParameter<T> parameter);
        R FindFirstOrDefault<R>(ExpressionParameter<T> parameter);
        R FindFirst<R>(ExpressionParameter<T> parameter);

        IList<T> Rawlist(string RawSql);
        bool Rawquery(string RawSql);
        IList<T> Rawlist(string RawSql, params object[] parameters);

        IList<T> FindAll(ExpressionParameter<T> parameter);
        IList<T> FindAll(PaginateExpressionParameter<T> parameter);
        T FindFirstOrDefault(ExpressionParameter<T> parameter);
        T FindFirst(ExpressionParameter<T> parameter);


        IList<R> FindAll<R>(QueryParameter<T> parameter);
        IList<R> FindAll<R>(PaginateQueryParameter<T> parameter);
        R FindFirstOrDefault<R>(QueryParameter<T> parameter);
        R FindFirst<R>(QueryParameter<T> parameter);

        IList<T> FindAll(QueryParameter<T> parameter);
        IList<T> FindAll(PaginateQueryParameter<T> parameter);
        T FindFirstOrDefault(QueryParameter<T> parameter);
        T FindFirst(QueryParameter<T> parameter);


        #endregion
    }
    public partial class EFRepository<T, PK>
        : IEFRepository<T, PK> where T : BaseEntity<PK>//class
    {
        const int DEFAULT_LIMIT = 10;
        protected readonly EFContext context;
        protected DbSet<T> entities;
        string errorMessage = string.Empty;
        protected IdWorker _snowflakeID;
        private const string _isDeletedProperty = "is_deleted";
        private const string _createdbyProperty = "created_by";
        private const string _createdonProperty = "created_on";
        private const string _modifiedbyProperty = "modified_by";
        private const string _modifiedonProperty = "modified_on";
        private const string _deletedbyProperty = "deleted_by";
        private const string _deletedonProperty = "deleted_on";

        public EFRepository(EFContext context)
        {
            this.context = context;
            entities = context.Set<T>();
            _snowflakeID = new IdWorker();
        }

        public EFContext GetContext()
        {

            return context;
        }

        public DbSet<T> GetEntities()
        {
            return entities;
        }
        public Boolean ProcessingTransaction(TransactionProcess transactionProcess)
        {

            IDbContextTransaction trx = GetTransaction();
            try
            {
                transactionProcess();
                trx.Commit();
                return true;
            }
            catch (Exception ex)
            {
                trx.Rollback();
                return false;
            }

        }


        public IDbContextTransaction GetTransaction()
        {
            return context.Database.BeginTransaction();
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public int Count(ExpressionParameter<T> parameter)
        {
            return GetQueryable(parameter).Count();
        }

        public int CountTotal()
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

        private IQueryable<T> GetQueryable(ExpressionParameter<T> parameter)
        {
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
            return dbQuery;
        }

        private IQueryable<T> GetQueryable(PaginateExpressionParameter<T> parameter)
        {
            int limit = parameter.limit == 0 ? DEFAULT_LIMIT : parameter.limit;
            return GetQueryable((ExpressionParameter<T>)parameter).Skip(parameter.offset).Take(limit);
        }

        private IQueryable<T> GetQueryable(QueryParameter<T> parameter)
        {
            IQueryable<T> dbQuery = parameter.dbQuery;
            dbQuery = dbQuery.AsNoTracking();
            return dbQuery;
        }

        private IQueryable<T> GetQueryable(PaginateQueryParameter<T> parameter)
        {
            int limit = parameter.limit == 0 ? DEFAULT_LIMIT : parameter.limit;
            return GetQueryable((QueryParameter<T>)parameter).Skip(parameter.offset).Take(limit);
        }

        public IList<R> FindAll<R>(ExpressionParameter<T> parameter)
        {
            IList<R> list;
            //using (var context = new EFContext())
            {
                //context.Configuration.ProxyCreationEnabled = false;

                list = (IList<R>)GetQueryable(parameter)
                    .ToList<Object>();
                //.Select(s=>(R)s).ToList();
                //.ToAsyncEnumerable();
            }
            return list;
        }

        public IList<R> FindAll<R>(PaginateExpressionParameter<T> parameter)
        {
            IList<R> list;
            //using (var context = new EFContext())
            {
                list = (IList<R>)GetQueryable(parameter)
                    .ToList<Object>();
                //.Select(s=>(R)s).ToList();
                //.ToAsyncEnumerable();
            }
            return list;
        }

        public R FindFirstOrDefault<R>(ExpressionParameter<T> parameter)
        {
            return (R)GetQueryable(parameter).FirstOrDefault<Object>();
        }

        public T FindByID(PK id)
        {
            //ExpressionParameter<T> parameter = new ExpressionParameter<T>();
            //parameter.where = o => o.Id.Equals(id);
            //return FindFirst(parameter);
            return entities.Find(id);
        }



        public T Create(bool isSaveChanges, T item)
        {
            try
            {
                //item.Id = _snowflakeID.NextId();
                if (item.Id == null)
                {
                    SetPrimaryKey(item, _snowflakeID.NextId() + "");
                }

                PropertyInfo pi = item.GetType().GetProperty(_createdbyProperty);
                //if (pi != null)
                //{
                //    pi.SetValue(item, Glosing.Instance.Username);
                //}
                if (pi != null)
                {
                    pi.SetValue(item, Glosing.Instance.EmployeeId);
                }
                PropertyInfo pd = item.GetType().GetProperty(_createdonProperty);
                if (pd != null)
                {
                    pd.SetValue(item, DateTime.Now);
                }
                PropertyInfo pdel = item.GetType().GetProperty(_isDeletedProperty);
                if (pdel != null)
                {
                    pdel.SetValue(item, false);
                }
                context.Entry(item).State = EntityState.Added;
                if (isSaveChanges)
                    SaveChanges();

                return item;
            }
            catch(Exception ex) {
                throw new Exception(ex.Message.ToString());
            }
        }

        public virtual void SetPrimaryKey(T entity, Object value)
        {
            /*
            var keyName = context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties
                .Select(x => x.Name).Single();

            entity.GetType().GetProperty(keyName).SetValue(entity,value);
            */
            entity.Id = (PK)value;
        }

        public T Update(bool isSaveChanges, T item)
        {
            context.Entry(item).State = EntityState.Modified;
            PropertyInfo pi = item.GetType().GetProperty(_modifiedbyProperty);
            if (pi != null)
            {
                pi.SetValue(item, Glosing.Instance.Username);
            }
            PropertyInfo pd = item.GetType().GetProperty(_modifiedonProperty);
            if (pd != null)
            {
                pd.SetValue(item, DateTime.Now);
            }
            PropertyInfo pdel = item.GetType().GetProperty(_isDeletedProperty);
            if (pdel != null)
            {
                pdel.SetValue(item, false);
            }
            if (isSaveChanges)
                SaveChanges();

            return item;
        }

        public void Remove(bool isSaveChanges, T item)
        {
            PropertyInfo pdel = item.GetType().GetProperty(_isDeletedProperty);
            if (pdel != null)
            {
                PropertyInfo pi = item.GetType().GetProperty(_deletedbyProperty);
                if (pi != null)
                {
                    pi.SetValue(item, Glosing.Instance.Username);
                }
                PropertyInfo pd = item.GetType().GetProperty(_deletedonProperty);
                if (pd != null)
                {
                    pd.SetValue(item, DateTime.Now);
                }
                context.Entry(item).State = EntityState.Modified;
                pdel.SetValue(item, true);
                if (isSaveChanges)
                    SaveChanges();
            }
            else
            {
                context.Entry(item).State = EntityState.Deleted;
                if (isSaveChanges)
                    SaveChanges();
            }

        }

        public T Create(T item)
        {
            return Create(true, item);
        }

        public IList<T> CreateAll(bool isSaveChanges, params T[] items)
        {
            foreach (T item in items)
            {
                Create(false, item);
            }
            if (isSaveChanges)
                SaveChanges();
            return items;
        }

        public IList<T> CreateAll(params T[] items)
        {
            return CreateAll(true, items);
        }


        public T Update(T item)
        {
            return Update(true, item);
        }

        public IList<T> UpdateAll(bool isSaveChanges, params T[] items)
        {
            foreach (T item in items)
            {
                Update(false, item);
            }
            if (isSaveChanges)
                SaveChanges();
            return items;
        }

        public IList<T> UpdateAll(params T[] items)
        {
            return UpdateAll(true, items);
        }

        public void Remove(T item)
        {
            Remove(true, item);
        }

        public void RemoveAll(bool isSaveChanges, params T[] items)
        {
            foreach (T item in items)
            {
                Remove(false, item);
            }
            if (isSaveChanges)
                SaveChanges();
        }

        public void RemoveAll(params T[] items)
        {
            RemoveAll(true, items);
        }

        public IList<T> FindAll(ExpressionParameter<T> parameter)
        {
            IList<T> list;
            //using (var context = new EFContext())
            {
                //context.Configuration.ProxyCreationEnabled = false;

                list = GetQueryable(parameter)
                    .ToList();
                //.Select(s=>(R)s).ToList();
                //.ToAsyncEnumerable();
            }
            return list;
        }

        public IList<T> FindAll(PaginateExpressionParameter<T> parameter)
        {
            IList<T> list;
            //using (var context = new EFContext())
            {
                //context.Configuration.ProxyCreationEnabled = false;

                list = GetQueryable(parameter)
                    .ToList();
                //.Select(s=>(R)s).ToList();
                //.ToAsyncEnumerable();
            }
            return list;
        }

        public T FindFirstOrDefault(ExpressionParameter<T> parameter)
        {
            return GetQueryable(parameter).FirstOrDefault();
        }

        public R FindFirst<R>(ExpressionParameter<T> parameter)
        {
            Object r = GetQueryable(parameter).First();
            if (r == null)
                return default(R);
            else
                return (R)r;

        }

        public T FindFirst(ExpressionParameter<T> parameter)
        {
            return GetQueryable(parameter).First();
        }

        public int Count(QueryParameter<T> parameter)
        {
            return GetQueryable(parameter).Count();
        }

        public IList<R> FindAll<R>(QueryParameter<T> parameter)
        {
            IList<R> list;
            //using (var context = new EFContext())
            {
                //context.Configuration.ProxyCreationEnabled = false;

                list = (IList<R>)GetQueryable(parameter)
                    .ToList<Object>();
                //.Select(s=>(R)s).ToList();
                //.ToAsyncEnumerable();
            }
            return list;
        }

        public IList<R> FindAll<R>(PaginateQueryParameter<T> parameter)
        {
            IList<R> list;
            //using (var context = new EFContext())
            {
                //context.Configuration.ProxyCreationEnabled = false;

                list = (IList<R>)GetQueryable(parameter)
                    .ToList<Object>();
                //.Select(s=>(R)s).ToList();
                //.ToAsyncEnumerable();
            }
            return list;
        }

        public R FindFirstOrDefault<R>(QueryParameter<T> parameter)
        {
            return (R)GetQueryable(parameter).FirstOrDefault<Object>();
        }

        public R FindFirst<R>(QueryParameter<T> parameter)
        {
            Object r = GetQueryable(parameter).First();
            if (r == null)
                return default(R);
            else
                return (R)r;
        }

        public IList<T> FindAll(QueryParameter<T> parameter)
        {
            IList<T> list;
            //using (var context = new EFContext())
            {
                //context.Configuration.ProxyCreationEnabled = false;

                list = GetQueryable(parameter)
                    .ToList();
                //.Select(s=>(R)s).ToList();
                //.ToAsyncEnumerable();
            }
            return list;
        }

        public IList<T> FindAll(PaginateQueryParameter<T> parameter)
        {
            IList<T> list;
            //using (var context = new EFContext())
            {
                //context.Configuration.ProxyCreationEnabled = false;

                list = GetQueryable(parameter)
                    .ToList();
                //.Select(s=>(R)s).ToList();
                //.ToAsyncEnumerable();
            }
            return list;
        }

        public T FindFirstOrDefault(QueryParameter<T> parameter)
        {
            return GetQueryable(parameter).FirstOrDefault();
        }

        public T FindFirst(QueryParameter<T> parameter)
        {
            return GetQueryable(parameter).First();
        }

        public bool IsExist(ExpressionParameter<T> parameter)
        {
            return Count(parameter) > 0;
        }

        public bool IsExist(QueryParameter<T> parameter)
        {
            return Count(parameter) > 0;
        }

        public void RemoveByID(bool isSaveChanges, PK id)
        {
            Remove(isSaveChanges, FindByID(id));
        }

        public void RemoveByID(PK id)
        {
            RemoveByID(true, id);
        }

        public void RemoveAllByID(bool isSaveChanges, PK[] ids)
        {
            foreach (PK id in ids)
            {
                RemoveByID(isSaveChanges, id);
            }
        }

        public void RemoveAllByID(PK[] ids)
        {
            RemoveAllByID(true, ids);
        }

        public IList<T> Rawlist(string RawSql)
        {
            IList<T> list;
            list = entities.FromSql(RawSql).ToList();
            return list;
        }
        public bool Rawquery(string RawSql)
        {
            try
            {
                context.Database.ExecuteSqlCommand(RawSql);

            }
            catch(Exception ex)
            {
                return false;
            }
            
            return true;
        }
        public IList<T> Rawlist(string RawSql,params object[] parameters)
        {
            IList<T> list;
            list = entities.FromSql(RawSql,parameters).ToList();
            return list;
        }
    }
}
