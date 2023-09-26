using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Phoenix.Shared.Core.Areas.ProjectManagement.Dtos;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Parameters;
using Phoenix.Shared.Core.Repositories;
using Phoenix.Shared.Core.Services;
using Phoenix.Shared.Helpers;
using Phoenix.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Phoenix.Shared.Core.Areas.ProjectManagement.Services
{
    public partial interface ITimesheetNonJobServices : IBaseService<TimesheetNonJobDto, TimesheetNonJob, string>
    {
        GeneralResponseList<TimesheetNonJobDto> ListAll(TimesheetNonJobSearch filter);
        GeneralResponse<TimesheetNonJobNew> CreateTimesheetNonJob(TimesheetNonJobNew data);
        GeneralResponse<TimesheetNonJobDto> UpdateTimesheetNonJob(TimesheetNonJobDto data);
        GeneralResponse<TimesheetNonJobDto> GetTimesheetNonJob(string Id);
    }
    public partial class TimesheetNonJobServices : BaseService<TimesheetNonJobDto, TimesheetNonJob, string>, ITimesheetNonJobServices
    {
        IEFRepository<spjobnumber, string> _repocode;
        public TimesheetNonJobServices(IEFRepository<TimesheetNonJob, string> repo,
            IEFRepository<spjobnumber, string> repocode) : base(repo)
        {
            _repo = repo;
            _repocode = repocode;
        }
        public GeneralResponse<TimesheetNonJobDto> UpdateTimesheetNonJob(TimesheetNonJobDto data)
        {
            GeneralResponse<TimesheetNonJobDto> resp = new GeneralResponse<TimesheetNonJobDto>();
            try
            {
 
                TimesheetNonJob nu = _repo.FindByID(data.Id);
                if (nu != null)
                {
                    TimesheetNonJob olddata = new TimesheetNonJob();
                    PropertyMapper.All(nu, olddata);
                    nu.name = data.name;
                    nu.business_unit_id = data.business_unit_id;
                    nu.description = data.description;
                    nu.start_date = data.start_date;
                    nu.is_active = true;
                    nu.end_date = data.end_date;
                    var detail = JsonConvert.SerializeObject(data.detail);
                    nu.detail = detail;
                    nu.revision = nu.revision+1;
                    //validate total-------
                    resp.Data = data;
                    _repo.Update(nu);
                    olddata.Id = null;
                    olddata.is_active = false;
                    _repo.Create(olddata);
                    resp.Success = true;
                }
                else
                {
                    resp.Success = true;
                    resp.Message = "no Data";
                }
                
            }
            catch (Exception ex)
            {

                resp.Message = ex.Message;
            }
            return resp;
        }
        public GeneralResponse<TimesheetNonJobNew> CreateTimesheetNonJob(TimesheetNonJobNew data)
        {
            GeneralResponse<TimesheetNonJobNew> resp = new GeneralResponse<TimesheetNonJobNew>();
            try
            {
                var rc_code = _repocode.GetEntities().FromSql("[pm].[sgGetTSNonJobCode]").FirstOrDefault().Id;

                TimesheetNonJob nu = new TimesheetNonJob();
                nu.name = data.name;
                nu.code = rc_code;
                nu.business_unit_id = data.business_unit_id;
                nu.start_date = data.start_date;
                nu.is_active = true;
                nu.end_date = data.end_date;
                nu.description = data.description;
                var detail = JsonConvert.SerializeObject(data.detail);
                nu.detail = detail;
                //validate total-------
                resp.Data = data;
                _repo.Create( nu);
                resp.Success = true;
            }
            catch (Exception ex)
            {

                resp.Message = ex.Message;
            }
            return resp;
        }
        public GeneralResponse<TimesheetNonJobDto> GetTimesheetNonJob(string Id)
        {
            GeneralResponse<TimesheetNonJobDto> resp = new GeneralResponse<TimesheetNonJobDto>();
            try
            {
                IQueryable<TimesheetNonJobDto> que = from tb1 in _repo.GetContext().Set<TimesheetNonJob>()
                                              join tb2 in _repo.GetContext().Set<BusinessUnit>() on tb1.business_unit_id equals tb2.Id
                                             
                                              where tb1.Id ==Id 
                                              select new TimesheetNonJobDto()
                                              {
                                                  Id = tb1.Id,
                                                  description = tb1.description,
                                                  business_unit_id = tb1.business_unit_id,
                                                  business_unit_name=tb2.unit_name,
                                                  detail = JsonConvert.DeserializeObject<List<TimesheetNonJobTask>>(tb1.detail),
                                                  name = tb1.name,
                                                  code=tb1.code,
                                                  start_date=tb1.start_date,
                                                  end_date=tb1.end_date,
                                                  revision=tb1.revision
                                              };
                resp.Data = que.FirstOrDefault();
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }

        public GeneralResponseList<TimesheetNonJobDto> ListAll(TimesheetNonJobSearch filter)
        {

            GeneralResponseList<TimesheetNonJobDto> resp = new GeneralResponseList<TimesheetNonJobDto>();
            try
            {
                DateTime today = DateTime.Today;
                PagingHelper q = new PagingHelper();
                IQueryable<TimesheetNonJobDto> que = from tb1 in _repo.GetContext().Set<TimesheetNonJob>()
                                                     where (filter.allversion || tb1.is_active==true)
                select new TimesheetNonJobDto()
                          {
                              Id = tb1.Id,
                              description = tb1.description,
                              business_unit_id=tb1.business_unit_id,
             
                              detail= JsonConvert.DeserializeObject<List<TimesheetNonJobTask>>(tb1.detail),
                              name=tb1.name,
                              code = tb1.code,
                              start_date = tb1.start_date,
                              end_date = tb1.end_date,
                              revision = tb1.revision
                          };
                if (filter.active)
                {
                    que = que.Where(x => (x.start_date==null || x.start_date <= today) && (x.end_date==null || x.end_date >= today));

                }

                
                resp.RecordsTotal = que.Count();

                resp.RecordsFiltered = que.Count();
                resp.Total = resp.RecordsTotal;

                que = q.sortir(que, "Id", filter.sortBy, filter.sortDir);
                que = q.limit(que, filter.GetLimit(), filter.GetOffset());
 
                resp.Rows = que.ToList();
                resp.Success = true;

            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;

        }
    }
}

