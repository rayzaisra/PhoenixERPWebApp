using Kendo.Mvc.Extensions;
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
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;

namespace Phoenix.Shared.Core.Services
{
    public partial interface IBaseService<D, T, PK> where T : BaseEntity<PK> where D : class
    {
        GeneralResponseList<T> GetAllResponse();
        GeneralResponse<T> GetByIDResponse(PK id);
        GeneralResponse<D> GetByIDResponseDto(PK id);
        GeneralResponseList<T> SearchAllResponse(PaginateExpressionParameter<T> parameter);
        GeneralResponseList<T> SearchAllResponse(PaginateQueryParameter<T> parameter);

        GeneralResponse<T> CreateResponse(D data);
        GeneralResponse<T> UpdateResponse(PK id,D data);

        GeneralResponse<T> CreateWithOnlyResponse(D data,string[] only);
        GeneralResponse<T> UpdateWithOnlyResponse(PK id, D data,string[] only);

        GeneralResponse<T> CreateWithExceptResponse(D data, string[] excepts);
        GeneralResponse<T> UpdateWithExceptResponse(PK id, D data, string[] excepts);

        GeneralResponse<T> DeleteByIDResponse(PK id);

        GeneralResponse<Object> ProcessingTransactionResponse(TransactionProcess transactionProcess);
    }

    public partial class BaseService<D, T, PK> : IBaseService<D, T, PK> where T : BaseEntity<PK> where D : class
    {

        public GeneralResponse<T> CreateResponse(D data)
        {
            GeneralResponse<T> resp = new GeneralResponse<T>();
            try
            {
                resp.Data = Create(data);
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Code = "404";
                resp.Message = ex.Message;
                resp.Success = false;
            }
            return resp;

        }
        public D GetObject()
        {
            return (D)Activator.CreateInstance(typeof(D), new object[] { });
        }
        public GeneralResponse<D> GetByIDResponseDto(PK id)
        {

            GeneralResponse<D> resp = new GeneralResponse<D>();
            try
            {
                T sdata = GetByID(id);
                var nudata = GetObject();
                if (sdata != null)
                {
                    PropertyMapper.All(sdata, nudata);

                    resp.Data = nudata;
                    resp.Success = true;
                }
                else
                {
                    resp.Message = "Data Not Found";
                    resp.Success = false;
                }




            }
            catch (Exception ex)
            {
                resp.Code = "404";
                resp.Message = ex.Message;
                resp.Success = false;
            }
            return resp;

        }

        public GeneralResponse<T> CreateWithExceptResponse(D data, string[] excepts)
        {
            GeneralResponse<T> resp = new GeneralResponse<T>();
            try
            {
                resp.Data = CreateWithExcept(data, excepts);
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Code = "404";
                resp.Message = ex.Message;
                resp.Success = false;
            }
            return resp;
        }

        public GeneralResponse<T> CreateWithOnlyResponse(D data, string[] only)
        {
            GeneralResponse<T> resp = new GeneralResponse<T>();
            try
            {
                resp.Data = CreateWithOnly(data, only);
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Code = "404";
                resp.Message = ex.Message;
                resp.Success = false;
            }
            return resp;

        }

        public GeneralResponse<T> DeleteByIDResponse(PK id)
        {
            GeneralResponse<T> resp = new GeneralResponse<T>();
            try
            {

                DeleteByID(id);
                resp.Success = true;

            }
            catch (Exception ex)
            {
                resp.Code = "404";
                resp.Message = ex.Message;
                resp.Success = false;
            }
            return resp;

        }

        public GeneralResponseList<T> GetAllResponse()
        {
            GeneralResponseList<T> resp = new GeneralResponseList<T>();
            try
            {
                resp.Rows = GetAll();
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Code = "404";
                resp.Message = ex.Message;
                resp.Success = false;
            }
            return resp;

        }

        public GeneralResponse<T> GetByIDResponse(PK id)
        {
            GeneralResponse<T> resp = new GeneralResponse<T>();
            try
            {
                resp.Data = GetByID(id);
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Code = "404";
                resp.Message = ex.Message;
                resp.Success = false;
            }
            return resp;

        }

        public GeneralResponse<T> UpdateResponse(PK id, D data)
        {
            GeneralResponse<T> resp = new GeneralResponse<T>();
            try
            {
                resp.Data = Update(id, data);
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Code = "404";
                resp.Message = ex.Message;
                resp.Success = false;
            }
            return resp;

        }

        public GeneralResponse<T> UpdateWithExceptResponse(PK id, D data, string[] excepts)
        {
            GeneralResponse<T> resp = new GeneralResponse<T>();
            try
            {
                resp.Data = UpdateWithExcept(id, data, excepts);
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Code = "404";
                resp.Message = ex.Message;
                resp.Success = false;
            }
            return resp;

        }

        public GeneralResponse<T> UpdateWithOnlyResponse(PK id, D data, string[] only)
        {
            GeneralResponse<T> resp = new GeneralResponse<T>();
            try
            {
                resp.Data = UpdateWithOnly(id, data, only);
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Code = "404";
                resp.Message = ex.Message;
                resp.Success = false;
            }
            return resp;

        }

        public GeneralResponseList<T> SearchAllResponse(PaginateExpressionParameter<T> parameter)
        {
            GeneralResponseList<T> resp = new GeneralResponseList<T>();
            try
            {
                resp.Rows = SearchAll(parameter);
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Code = "404";
                resp.Message = ex.Message;
                resp.Success = false;
            }
            return resp;
        }

        public GeneralResponseList<T> SearchAllResponse(PaginateQueryParameter<T> parameter)
        {
            GeneralResponseList<T> resp = new GeneralResponseList<T>();
            try
            {
                resp.Rows = SearchAll(parameter);
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Code = "404";
                resp.Message = ex.Message;
                resp.Success = false;
            }
            return resp;
        }
        public GeneralResponse<Object> ProcessingTransactionResponse(TransactionProcess transactionProcess)
        {
            GeneralResponse<Object> resp = new GeneralResponse<Object>();
            IDbContextTransaction trx = _repo.GetTransaction();
            try
            {
                resp.Data = transactionProcess();
                trx.Commit();
                resp.Success = true;
            }
            catch (Exception ex)
            {
                trx.Rollback();
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }
        public GeneralResponse<R> ProcessingTransactionResponse<R>(TransactionProcess transactionProcess)
        {
            GeneralResponse<R> resp = new GeneralResponse<R>();
            IDbContextTransaction trx = _repo.GetTransaction();
            try
            {
                resp.Data = (R)transactionProcess();
                trx.Commit();
                resp.Success = true;
            }
            catch (Exception ex)
            {
                trx.Rollback();
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }
    }
}
