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
    public partial interface IJobPostingRequisitionServices : IBaseService<JobPostingRequisitionDto, JobPostingRequisition, string>
    {
        GeneralResponseList<JobPostingRequisitionDto> ListAll(JobPostingRequisitionSearch filter);
    }
    public partial class JobPostingRequisitionServices : BaseService<JobPostingRequisitionDto, JobPostingRequisition, string>, IJobPostingRequisitionServices
    {
        public JobPostingRequisitionServices(IEFRepository<JobPostingRequisition, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<JobPostingRequisitionDto> ListAll(JobPostingRequisitionSearch filter)
        {

            GeneralResponseList<JobPostingRequisitionDto> resp = new GeneralResponseList<JobPostingRequisitionDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<JobPostingRequisitionDto> que = from tb1 in _repo.GetContext().Set<JobPostingRequisition>()
                          select new JobPostingRequisitionDto()
                          {
                              Id = tb1.Id,
                              business_unit_id = tb1.business_unit_id,
                              candidate_age_range_end = tb1.candidate_age_range_end,
                              candidate_age_range_start = tb1.candidate_age_range_start,
                              candidate_experience = tb1.candidate_experience,
                              candidate_qualification = tb1.candidate_qualification,
                              central_resource_id = tb1.central_resource_id,
                              client_brief_id = tb1.client_brief_id,
                              code = tb1.code,
                              job_post_description = tb1.job_post_description,
                              job_title = tb1.job_title,
                              notes = tb1.notes,
                              number_of_position = tb1.number_of_position,
                              reg_code = tb1.reg_code,
                              salary_end = tb1.salary_end,
                              salary_start = tb1.salary_start,
                              status = tb1.status,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "business_unit_id", filter.Search_business_unit_id);
                que = q.filterEquals(que, "candidate_age_range_end", filter.Search_candidate_age_range_end);
                que = q.filterEquals(que, "candidate_age_range_start", filter.Search_candidate_age_range_start);
                que = q.filterContains(que, "candidate_experience", filter.Search_candidate_experience);
                que = q.filterContains(que, "candidate_qualification", filter.Search_candidate_qualification);
                que = q.filterEquals(que, "central_resource_id", filter.Search_central_resource_id);
                que = q.filterEquals(que, "client_brief_id", filter.Search_client_brief_id);
                que = q.filterContains(que, "code", filter.Search_code);
                que = q.filterContains(que, "job_post_description", filter.Search_job_post_description);
                que = q.filterContains(que, "job_title", filter.Search_job_title);
                que = q.filterContains(que, "notes", filter.Search_notes);
                que = q.filterEquals(que, "number_of_position", filter.Search_number_of_position);
                que = q.filterContains(que, "reg_code", filter.Search_reg_code);
                que = q.filterEquals(que, "salary_end", filter.Search_salary_end);
                que = q.filterEquals(que, "salary_start", filter.Search_salary_start);
                que = q.filterContains(que, "status", filter.Search_status);

                

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

