using Microsoft.AspNetCore.Mvc;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Parameters;
using Phoenix.Shared.Core.Repositories;
using Phoenix.Shared.Core.Services;
using Phoenix.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenix.Api.Shared.Controllers
{
    public partial class BaseMasterApiController<D,T,PK> : BaseApiController<D,T,PK> where T : BaseEntity<PK> where D : BaseEntity<PK>
    {
        protected String ModeCreate;
        protected String ModeUpdate;
        protected const String ALL = "ALL";
        protected const String EXCEPT = "EXCEPT";
        protected const String ONLY = "ONLY";
        private IBaseService<D, T, PK> _service;

        public BaseMasterApiController(IBaseService<D, T, PK> service):base(service)
        {
            _service = service;
            ModeCreate = ALL;
            ModeUpdate = ALL;
        }

        //[HttpGet]
        //public GeneralResponseList<FnCurrencySample> Search(SearchParameter parameter)
        //{
        //    return _service.SearchAll(parameter);
        //}

        [HttpGet("{id}")]
        public GeneralResponse<T> Get(PK id)
        {
            GeneralResponse<T> resp = new GeneralResponse<T>();
            try
            {
                resp.Data = _service.GetByID(id);
                resp.Success = true;
                resp.Code = "200";
            }
            catch (Exception ex)
            {
                resp.Code = "500";
                resp.Success = false;
                resp.Message = "An error occured, cause of :" + ex.Message;
            }
            return resp;
        }

        [HttpPost]
        public GeneralResponse<T> Create(D dto)
        {
            GeneralResponse<T> resp = new GeneralResponse<T>();
            try
            {
                switch (ModeCreate)
                {
                    case ALL:
                        resp.Data = _service.Create(dto);
                        break;
                    case EXCEPT:
                        resp.Data = _service.CreateWithExcept(dto, GetCreateExcepts());
                        break;
                    case ONLY:
                        resp.Data = _service.CreateWithOnly(dto, GetCreateOnly());
                        break;
                    default:
                        resp.Data = _service.Create(dto);
                        break;
                }
                resp.Success = true;
                resp.Code = "200";
            }
            catch (Exception ex)
            {
                resp.Code = "500";
                resp.Success = false;
                resp.Message = "An error occured, cause of :" + ex.Message;
            }
            return resp;
        }


        protected virtual string[] GetCreateExcepts()
        {
            return new string[] { "Isdeleted" };
        }
        protected virtual string[] GetCreateOnly()
        {
            return new string[] { "Name" };
        }

        [HttpPost]
        public GeneralResponse<T> Update(D dto)
        {
            GeneralResponse<T> resp = new GeneralResponse<T>();
            try
            {
                switch (ModeUpdate)
                {
                    case ALL:
                        resp.Data = _service.Update(dto.Id, dto);
                        break;
                    case EXCEPT:
                        resp.Data = _service.UpdateWithExcept(dto.Id,dto, GetUpdateExcepts());
                        break;
                    case ONLY:
                        resp.Data = _service.UpdateWithOnly(dto.Id,dto, GetUpdateOnly());
                        break;
                    default:
                        resp.Data = _service.Update(dto.Id,dto);
                        break;
                }
                resp.Success = true;
                resp.Code = "200";
            }
            catch (Exception ex)
            {
                resp.Code = "500";
                resp.Success = false;
                resp.Message = "An error occured, cause of :" + ex.Message;
            }
            return resp;
        }

        protected virtual string[] GetUpdateExcepts()
        {
            return new string[] { "Id", "Isdeleted" };
        }

        protected virtual string[] GetUpdateOnly()
        {
            return new string[] { "Id", "Name" };
        }


        [HttpGet("{id}")]
        public GeneralResponse<T> Delete(PK id)
        {
            GeneralResponse<T> resp = new GeneralResponse<T>();
            try
            {
                _service.DeleteByID(id);
                resp.Success = true;
                resp.Code = "200";
            }
            catch (Exception ex)
            {
                resp.Code = "500";
                resp.Success = false;
                resp.Message = "An error occured, cause of :" + ex.Message;
            }
            return resp;
        }


    }
}
