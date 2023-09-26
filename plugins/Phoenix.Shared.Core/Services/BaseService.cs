using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Phoenix.Shared.Core.Contexts;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Parameters;
using Phoenix.Shared.Core.Repositories;
using Phoenix.Shared.Helpers;
using Phoenix.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Phoenix.Shared.Core.Services
{
    public partial interface IBaseService<D, T, PK> where T : BaseEntity<PK> where D : class
    {
        IList<T> GetAll();
        T GetByID(PK id);

        IList<T> SearchAll(PaginateExpressionParameter<T> parameter);
        IList<T> SearchAll(PaginateQueryParameter<T> parameter);

        T Create(D data);
        T Update(PK id, D data);

        T CreateWithOnly(D data, string[] only);
        T UpdateWithOnly(PK id, D data, string[] only);

        T CreateWithExcept(D data, string[] excepts);
        T UpdateWithExcept(PK id, D data, string[] excepts);

        void DeleteByID(PK id);

        EFContext GetContext();
        DbSet<T> GetEntities();
        Object ProcessingTransaction(TransactionProcess transactionProcess);
    }

    public partial class BaseService<D, T, PK> : IBaseService<D, T, PK> where T : BaseEntity<PK> where D : class
    {

        protected IEFRepository<T, PK> _repo;

        public BaseService(IEFRepository<T, PK> repo)
        {
            _repo = repo;
        }

        public EFContext GetContext()
        {
            return _repo.GetContext();
        }

        public DbSet<T> GetEntities()
        {
            return _repo.GetEntities();
        }
        public T Create(D data)
        {
            T newData = NewInstance();//default(T);
            PropertyMapper.All(data, newData);
            PropertyInfo pi = newData.GetType().GetProperty("created_by");
            //if (pi != null)
            //{
            //    pi.SetValue(newData, Glosing.Instance.Username);
            //}
            if (pi != null)
            {
                pi.SetValue(newData, Glosing.Instance.EmployeeId);
            }
            PropertyInfo pd = newData.GetType().GetProperty("created_date");
            if (pd != null)
            {
                pd.SetValue(newData, DateTime.Now);
            }
            return _repo.Create(newData);
        }

        public T CreateWithExcept(D data, string[] excepts)
        {
            T newData = NewInstance();//default(T);
            PropertyMapper.Except(excepts, data, newData);
            PropertyInfo pi = newData.GetType().GetProperty("created_by");
            //if (pi != null)
            //{
            //    pi.SetValue(newData, Glosing.Instance.Username);
            //}
            if (pi != null)
            {
                pi.SetValue(newData, Glosing.Instance.EmployeeId);
            }
            PropertyInfo pd = newData.GetType().GetProperty("created_date");
            if (pd != null)
            {
                pd.SetValue(newData, DateTime.Now);
            }
            return _repo.Create(newData);
        }

        public T CreateWithOnly(D data, string[] only)
        {
            T newData = NewInstance();//default(T);
            PropertyMapper.Only(only, data, newData);
            PropertyInfo pi = newData.GetType().GetProperty("created_by");
            //if (pi != null)
            //{
            //    pi.SetValue(newData, Glosing.Instance.Username);
            //}
            if (pi != null)
            {
                pi.SetValue(newData, Glosing.Instance.EmployeeId);
            }
            PropertyInfo pd = newData.GetType().GetProperty("created_date");
            if (pd != null)
            {
                pd.SetValue(newData, DateTime.Now);
            }
            return _repo.Create(newData);
        }

        public void DeleteByID(PK id)
        {
            _repo.RemoveByID(id);
        }

        public IList<T> GetAll()
        {
            ExpressionParameter<T> parameter = new ExpressionParameter<T>();
            return _repo.FindAll(parameter);
        }

        public T GetByID(PK id)
        {
            return _repo.FindByID(id);
        }

        public T Update(PK id, D data)
        {
            T newData = _repo.FindByID(id);
            PropertyMapper.All(data, newData);
            PropertyInfo pi = newData.GetType().GetProperty("updated_by");
            //if (pi != null)
            //{
            //    pi.SetValue(newData, Glosing.Instance.Username);
            //}
            if (pi != null)
            {
                pi.SetValue(newData, Glosing.Instance.EmployeeId);
            }
            PropertyInfo pd = newData.GetType().GetProperty("updated_date");
            if (pd != null)
            {
                pd.SetValue(newData, DateTime.Now);
            }
            return _repo.Update(newData);
        }

        public T UpdateWithExcept(PK id, D data, string[] excepts)
        {
            T newData = _repo.FindByID(id);
            PropertyMapper.Except(excepts, data, newData);
            PropertyMapper.All(data, newData);
            PropertyInfo pi = newData.GetType().GetProperty("updated_by");
            //if (pi != null)
            //{
            //    pi.SetValue(newData, Glosing.Instance.Username);
            //}
            if (pi != null)
            {
                pi.SetValue(newData, Glosing.Instance.EmployeeId);
            }
            PropertyInfo pd = newData.GetType().GetProperty("updated_date");
            if (pd != null)
            {
                pd.SetValue(newData, DateTime.Now);
            }
            return _repo.Update(newData);
        }

        public T UpdateWithOnly(PK id, D data, string[] only)
        {
            T newData = _repo.FindByID(id);
            PropertyMapper.Only(only, data, newData);
            PropertyMapper.All(data, newData);
            PropertyInfo pi = newData.GetType().GetProperty("updated_by");
            //if (pi != null)
            //{
            //    pi.SetValue(newData, Glosing.Instance.Username);
            //}
            if (pi != null)
            {
                pi.SetValue(newData, Glosing.Instance.EmployeeId);
            }
            PropertyInfo pd = newData.GetType().GetProperty("updated_date");
            if (pd != null)
            {
                pd.SetValue(newData, DateTime.Now);
            }
            return _repo.Update(newData);
        }

        public IList<T> SearchAll(PaginateExpressionParameter<T> parameter)
        {
            return _repo.FindAll(parameter);
        }

        public IList<T> SearchAll(PaginateQueryParameter<T> parameter)
        {
            return _repo.FindAll(parameter);
        }
        public Object ProcessingTransaction(TransactionProcess transactionProcess)
        {
            Object response = null;
            IDbContextTransaction trx = _repo.GetTransaction();
            try
            {
                response = transactionProcess();
                trx.Commit();
            }
            catch (Exception ex)
            {
                trx.Rollback();
            }
            return response;
        }
        public R ProcessingTransaction<R>(TransactionProcess transactionProcess)
        {
            R resp = default(R);
            IDbContextTransaction trx = _repo.GetTransaction();
            try
            {
                resp = (R)transactionProcess();
                trx.Commit();
            }
            catch (Exception ex)
            {
                trx.Rollback();
            }
            return resp;
        }

        public T NewInstance()
        {
            return (T)Activator.CreateInstance(typeof(T), new object[] { });
        }
    }
}
