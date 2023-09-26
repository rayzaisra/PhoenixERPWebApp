using Phoenix.Shared.Core.Areas.Hris.Dtos;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Parameters;
using Phoenix.Shared.Core.Repositories;
using Phoenix.Shared.Core.Services;
using Phoenix.Shared.Responses;
using System;
using System.Linq;


namespace Phoenix.Shared.Core.Areas.Hris.Services
{
    public partial interface IJobInterviewServices : IBaseService<JobInterviewDto, JobInterview, string>
    {
        GeneralResponseList<JobInterviewDto> ListAll(JobInterviewSearch filter);
    }
    public partial class JobInterviewServices : BaseService<JobInterviewDto, JobInterview, string>, IJobInterviewServices
    {
        public JobInterviewServices(IEFRepository<JobInterview, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<JobInterviewDto> ListAll(JobInterviewSearch filter)
        {

            GeneralResponseList<JobInterviewDto> resp = new GeneralResponseList<JobInterviewDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<JobInterviewDto> que = from tb1 in _repo.GetContext().Set<JobInterview>()
                          select new JobInterviewDto()
                          {
                              Id = tb1.Id,
                              from_email = tb1.from_email,
                              interviewer = tb1.interviewer,
                              interview_date = tb1.interview_date,
                              interview_time = tb1.interview_time,
                              job_interview_description = tb1.job_interview_description,
                              job_posting_requisition_id = tb1.job_posting_requisition_id,
                              latitude = tb1.latitude,
                              longitude = tb1.longitude,
                              notes = tb1.notes,
                              place_of_interview = tb1.place_of_interview,
                              to_email = tb1.to_email,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "from_email", filter.Search_from_email);
                que = q.filterContains(que, "interviewer", filter.Search_interviewer);
                que = q.filterEquals(que, "interview_date", filter.Search_interview_date);
                que = q.filterEquals(que, "interview_time", filter.Search_interview_time);
                que = q.filterContains(que, "job_interview_description", filter.Search_job_interview_description);
                que = q.filterEquals(que, "job_posting_requisition_id", filter.Search_job_posting_requisition_id);
                que = q.filterContains(que, "latitude", filter.Search_latitude);
                que = q.filterContains(que, "longitude", filter.Search_longitude);
                que = q.filterContains(que, "notes", filter.Search_notes);
                que = q.filterContains(que, "place_of_interview", filter.Search_place_of_interview);
                que = q.filterContains(que, "to_email", filter.Search_to_email);

                

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

