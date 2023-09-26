using Phoenix.Shared.Core.Areas.General.Dtos;
using Phoenix.Shared.Core.Areas.Hris.Dtos;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Entities.Hris;
using Phoenix.Shared.Core.Parameters;
using Phoenix.Shared.Core.Repositories;
using Phoenix.Shared.Core.Services;
using Phoenix.Shared.Helpers;
using Phoenix.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Phoenix.Shared.Core.Areas.Hris.Services
{

    public partial interface IJobVacancyServices : IBaseService<JobVacancyDto, JobVacancy, string>
    {
        GeneralResponseList<JobVacancyDto> ListAll(JobVacancySearch filter);
        GeneralResponseList<MasterLocationDto> ListMasterLocation(SearchParameter filter);
        GeneralResponse<JobVacancyDto> GetWithDetial(string id);
        GeneralResponse<JobVacancyDto> GetDetialWithIDHiring(string id);
        GeneralResponseList<JobVacancyDto> ListPostingCode();
        GeneralResponse<JobVacancyDto> GetVancacyNameByCode(string id);
        GeneralResponse<JobVacancy> CreateDetail(JobVacancyDto dto);
        GeneralResponse<JobVacancy> UpdatePostingStatus(JobVacancyDto dto);
        GeneralResponse<JobVacancy> DeleteDetil(string id);
    }
    public partial class JobVacancyServices : BaseService<JobVacancyDto, JobVacancy, string>, IJobVacancyServices
    {
        IEFRepository<JobVacancyJobSouce, string> _repoJobVacancyJobSouce;
        IEFRepository<HiringRequest, string> _repoHiringRequest;
        public JobVacancyServices(IEFRepository<JobVacancy, string> repo, IEFRepository<JobVacancyJobSouce, string> repoJobVacancyJobSouce, IEFRepository<HiringRequest, string> repoHiringRequest) : base(repo)
        {
            _repo = repo;
            _repoJobVacancyJobSouce = repoJobVacancyJobSouce;
            _repoHiringRequest = repoHiringRequest;
        }

        #region "Private Method Business Unit"
        private BusinessUnitParent GetNodeParent(List<BusinessUnitDto> list, string id)
        {
            return list.Where(x => x.Id == id).Select(x => new BusinessUnitParent
            {
                Id = x.Id,
                unit_name = x.unit_name,
                unit_code = x.unit_code,
                business_unit_type_id = x.business_unit_type_id,
                parent_unit = x.parent_unit,
                created_on = x.created_on,
                default_team = x.default_team,
                organization_id = x.organization_id,
                unit_description = x.unit_description,
                parent = GetNodeParent(list, x.parent_unit)
            }).SingleOrDefault();
        }
        private List<BusinessUnitTree> GetNodeChildren(List<BusinessUnitDto> list, string parent)
        {
            return list.Where(x => x.parent_unit == parent).Select(x => new BusinessUnitTree
            {
                Id = x.Id,
                unit_name = x.unit_name,
                unit_code = x.unit_code,
                business_unit_type_id = x.business_unit_type_id,
                parent_unit = x.parent_unit,
                created_on = x.created_on,
                default_team = x.default_team,
                organization_id = x.organization_id,
                unit_description = x.unit_description,
                children = x.parent_unit != x.Id ? GetNodeChildren(list, x.Id) : new List<BusinessUnitTree>()
            }).ToList();
        }

        public List<BusinessUnitDto> ListAllBusinessUnit()
        {
            List<BusinessUnitDto> resp = new List<BusinessUnitDto>();

            PagingHelper q = new PagingHelper();
            IQueryable<BusinessUnitDto> que = (from tb1 in _repo.GetContext().Set<BusinessUnit>()
                                              select new BusinessUnitDto()
                                              {
                                                  Id = tb1.Id,
                                                  unit_code = tb1.unit_code,
                                                  business_unit_type_id = tb1.business_unit_type_id,
                                                  unit_name = tb1.unit_name,
                                                  created_on = tb1.created_on,
                                                  parent_unit = tb1.parent_unit,
                                                  unit_description = tb1.unit_description,
                                                  default_team = tb1.default_team,
                                                  organization_id = tb1.organization_id,
                                                  unit_email = tb1.unit_email,
                                                  unit_fax = tb1.unit_fax,
                                                  unit_phone = tb1.unit_phone
                                              }).OrderByDescending(x=>x.created_on);

            resp = que.ToList();
            return resp;
        }

        #endregion

        public GeneralResponseList<JobVacancyDto> ListAll(JobVacancySearch filter)
        {

            GeneralResponseList<JobVacancyDto> resp = new GeneralResponseList<JobVacancyDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<JobVacancyDto> que = from tb1 in _repo.GetContext().Set<JobVacancy>()
                                                join
tb2 in _repo.GetContext().Set<HiringRequest>() on tb1.hiring_request_id equals tb2.Id
                                                join tb3 in _repo.GetContext().Set<EmployeeBasicInfo>() on tb2.requester_employee_basic_info_id equals tb3.Id
                                                join tbu in _repo.GetContext().Set<BusinessUnitJobLevel>() on tb3.business_unit_job_level_id equals tbu.Id
                                                orderby tb1.code descending
                                                select new JobVacancyDto()
                                                {
                                                    Id = tb1.Id,
                                                    VacancyTitleName = (from tb4 in _repo.GetContext().Set<JobTitle>()
                                                                        where tb2.job_title_id == tb4.Id
                                                                        select new JobVacancyNameTitleDto
                                                                        {
                                                                            job_title_code = tb4.title_code,
                                                                            job_title_name = tb4.title_name

                                                                        }).FirstOrDefault(),
                                                    VacancyGradeName = (from tb5 in _repo.GetContext().Set<JobGrade>()
                                                                        where tb2.job_grade_id == tb5.Id
                                                                        select new JobVacancyNameGradeDto
                                                                        {
                                                                            job_grade_code = tb5.grade_code,
                                                                            job_grade_name = tb5.grade_name
                                                                        }).FirstOrDefault(),
                                                    JobTitleRequester = (from tb7 in _repo.GetContext().Set<BusinessUnitJobLevel>()
                                                                         join tb8 in _repo.GetContext().Set<JobTitle>() on tb7.job_title_id equals tb8.Id
                                                                         where tb3.business_unit_job_level_id == tb7.Id
                                                                         select new JobtitleRequesterDto
                                                                         {
                                                                             job_title_code = tb8.title_code,
                                                                             job_title_name = tb8.title_name

                                                                         }).FirstOrDefault(),
                                                    closing_date = tb1.closing_date,
                                                    code = tb1.code,
                                                    hiring_request_id = tb1.hiring_request_id,
                                                    master_location_id = tb1.master_location_id,
                                                    posting_status = tb1.posting_status,
                                                    created_on = tb1.created_on,
                                                    request_code = tb2.code,
                                                    BusinessUnit = GetNodeParent(ListAllBusinessUnit(), tbu.business_unit_division_id),
                                                    Applicant = (from tba in _repo.GetContext().Set<HiringApplicant>()
                                                                 join
           tb5 in _repo.GetContext().Set<JobVacancy>() on tba.job_vacancy_id equals tb5.Id
                                                                 join tb6 in _repo.GetContext().Set<CandidateRecruitment>() on tba.candidate_recruitment_id equals tb6.Id
                                                                 where tba.job_vacancy_id == tb1.Id
                                                                 select new CandidateRecruitmentSetDto
                                                                 {
                                                                     Id = tb6.Id,
                                                                     first_name = tb6.first_name,
                                                                     last_name = tb6.last_name
                                                                 }).ToList()
                                                };
                que = que.OrderByDescending(x=>x.created_on);
                resp.RecordsTotal = que.Count();

                que = q.filterEquals(que, "hiring_request_id", filter.Search_hiring_request_id);
                que = q.filterContains(que, "vacancy_name", filter.Search_vacancy_name);

                resp.RecordsFiltered = que.Count();
                resp.Total = resp.RecordsTotal;


                if (filter.sortBy == "created_on")
                {
                    que = q.sortir(que, "created_on", filter.sortBy, filter.sortDir);
                }
                if (filter.sortBy == "vacancy_name")
                {
                    que = q.sortir(que, "vacancy_name", filter.sortBy, filter.sortDir);
                }

                if (filter.sortBy == "Id")
                {
                    que = q.sortir(que, "Id", filter.sortBy, filter.sortDir);
                }

                que = q.limit(que, filter.GetLimit(), filter.GetOffset());

                resp.Rows = que.OrderByDescending(x => x.created_on).ThenByDescending(x => x.created_on).ToList();
                resp.Success = true;

            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;

        }

        public GeneralResponseList<MasterLocationDto> ListMasterLocation(SearchParameter filter)
        {
            GeneralResponseList<MasterLocationDto> resp = new GeneralResponseList<MasterLocationDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<MasterLocationDto> que = from tb1 in _repo.GetContext().Set<MasterLocation>()
                                                    select new MasterLocationDto()
                                                    {
                                                        Id = tb1.Id,
                                                        location_name = tb1.location_name,
                                                        created_on = tb1.created_on

                                                    };

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

        public GeneralResponse<JobVacancyDto> GetWithDetial(string id)
        {
            GeneralResponse<JobVacancyDto> resp = new GeneralResponse<JobVacancyDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<JobVacancyDto> que = from tb1 in _repo.GetContext().Set<JobVacancy>()
                                                join tb2 in _repo.GetContext().Set<HiringRequest>() on tb1.hiring_request_id equals tb2.Id
                                                join tb3 in _repo.GetContext().Set<JobTitle>() on tb2.job_title_id equals tb3.Id
                                                join tb4 in _repo.GetContext().Set<JobGrade>() on tb2.job_grade_id equals tb4.Id
                                                join tb5 in _repo.GetContext().Set<MasterLocation>() on tb1.master_location_id equals tb5.Id into tbj5
                                                from tb6 in tbj5.DefaultIfEmpty()
                                                where tb1.Id == id
                                                select new JobVacancyDto()
                                                {
                                                    Id = tb1.Id,
                                                    posting_status = tb1.posting_status,
                                                    code = tb1.code,
                                                    hiring_request_id = tb2.Id,
                                                    request_code = tb2.code,
                                                    master_location_id = tb1.master_location_id,
                                                    vacancy_name = tb3.title_name + " (" + tb4.grade_name + ")",
                                                    created_on = tb1.created_on,
                                                    closing_date = tb1.closing_date,
                                                    location = (from tb5 in _repo.GetContext().Set<MasterLocation>()
                                                                where tb1.master_location_id == tb5.Id
                                                                select new MasterLocationDto
                                                                {
                                                                    Id = tb5.Id,
                                                                    location_name = tb5.location_name
                                                                }).DefaultIfEmpty(new MasterLocationDto() { Id = "", location_name = "" }).FirstOrDefault(),
                                                    JobVacancyJobSource = (from tbp in _repo.GetContext().Set<JobVacancyJobSouce>()
                                                                           where tbp.job_vacancy_id == tb1.Id
                                                                           select new JobVacancyJobSourceDto
                                                                           {
                                                                               Id = tbp.Id,
                                                                               code = tbp.code,
                                                                               date_posting = tbp.date_posting,
                                                                               files = tbp.files,
                                                                               created_on = tbp.created_on,
                                                                               job_vacancy_id = tbp.job_vacancy_id,
                                                                               source = tbp.souce,
                                                                               url = tbp.url
                                                                           }).DefaultIfEmpty(new JobVacancyJobSourceDto() { Id = "", code = "", date_posting = null, files = "", created_on = null, job_vacancy_id = "", source = "Others", url = "-" }).FirstOrDefault(),
                                                    master_location_name = tb6.location_name,
                                                    template_approval_id = tb1.approval_id

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

        public GeneralResponse<JobVacancy> CreateDetail(JobVacancyDto dto)
        {
            GeneralResponse<JobVacancy> resp = new GeneralResponse<JobVacancy>();
            try
            {
                JobVacancy newVacancy = new JobVacancy();
                PropertyMapper.All(dto, newVacancy);
                _repo.Create(false, newVacancy);

                if (dto.JobVacancyJobSouceInnerDto != null)
                {
                    foreach (JobVacancyJobSouceInnerDto data in dto.JobVacancyJobSouceInnerDto)
                    {
                        JobVacancyJobSouce jobSouce = new JobVacancyJobSouce();
                        if (!data.posting_code.Equals(""))
                        {
                            jobSouce.code = data.posting_code;
                            jobSouce.souce = data.source;
                            jobSouce.files = data.files;
                            jobSouce.date_posting = data.date_posting;
                            jobSouce.url = data.url;
                            jobSouce.job_vacancy_id = newVacancy.Id;
                            _repoJobVacancyJobSouce.Create(false, jobSouce);
                        }

                    }
                }

                HiringRequest newHiring = new HiringRequest();
                newHiring = _repoHiringRequest.FindByID(newVacancy.hiring_request_id);
                newHiring.status = "3";
                _repoHiringRequest.Update(false, newHiring);


                _repo.SaveChanges();
                _repoJobVacancyJobSouce.SaveChanges();
                _repoHiringRequest.SaveChanges();
                _repo.Dispose();
                _repoJobVacancyJobSouce.Dispose();
                _repoHiringRequest.Dispose();
                resp.Data = newVacancy;
                resp.Success = true;
                resp.Message = "Sucess";
                resp.Code = "00";

            }
            catch (Exception x)
            {
                resp.Success = false;
                resp.Code = "500";
                resp.Message = x.Message.ToString();
            }

            return resp;
        }

        public GeneralResponse<JobVacancy> UpdatePostingStatus(JobVacancyDto dto)
        {
            GeneralResponse<JobVacancy> resp = new GeneralResponse<JobVacancy>();
            try
            {
                JobVacancy newVacancy = new JobVacancy();
                newVacancy = _repo.FindByID(dto.Id);
                newVacancy.posting_status = dto.posting_status;

                _repo.Update(false, newVacancy);

                _repo.SaveChanges();
                _repo.Dispose();

                resp.Success = true;
                resp.Message = "Sucess";
                resp.Code = "00";

            }
            catch (Exception x)
            {
                resp.Success = false;
                resp.Code = "500";
                resp.Message = x.Message.ToString();
            }

            return resp;
        }

        public GeneralResponse<JobVacancy> DeleteDetil(string id)
        {
            GeneralResponse<JobVacancy> resp = new GeneralResponse<JobVacancy>();
            try
            {
                JobVacancy newVacancy = new JobVacancy();
                newVacancy = _repo.FindByID(id);

                if (newVacancy.hiring_request_id != null)
                {
                    HiringRequest newHiring = new HiringRequest();
                    newHiring = _repoHiringRequest.FindByID(newVacancy.hiring_request_id);
                    newHiring.status = "1";
                    _repoHiringRequest.Update(false, newHiring);
                    _repoHiringRequest.SaveChanges();
                }

                _repo.RemoveByID(false, id);
                _repo.SaveChanges();
                _repo.Dispose();

                resp.Success = true;
                resp.Message = "Sucess";
                resp.Code = "200";

            }
            catch (Exception x)
            {
                resp.Success = false;
                resp.Code = "500";
                resp.Message = x.Message.ToString();
            }

            return resp;
        }

        public GeneralResponse<JobVacancyDto> GetDetialWithIDHiring(string id)
        {
            GeneralResponse<JobVacancyDto> resp = new GeneralResponse<JobVacancyDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<JobVacancyDto> que = from tb1 in _repo.GetContext().Set<JobVacancy>()
                                                join tb2 in _repo.GetContext().Set<HiringRequest>() on tb1.hiring_request_id equals tb2.Id
                                                join tb3 in _repo.GetContext().Set<JobTitle>() on tb2.job_title_id equals tb3.Id
                                                join tb4 in _repo.GetContext().Set<JobGrade>() on tb2.job_grade_id equals tb4.Id
                                                where tb1.code == id
                                                select new JobVacancyDto()
                                                {
                                                    Id = tb1.Id,
                                                    posting_status = tb1.posting_status,
                                                    code = tb1.code,
                                                    hiring_request_id = tb2.Id,
                                                    request_code = tb2.code,
                                                    master_location_id = tb1.master_location_id,
                                                    vacancy_name = tb3.title_name + " (" + tb4.grade_name + ")",
                                                    created_on = tb1.created_on,
                                                    closing_date = tb1.closing_date,
                                                    location = (from tb5 in _repo.GetContext().Set<MasterLocation>()
                                                                where tb1.master_location_id == tb5.Id
                                                                select new MasterLocationDto
                                                                {
                                                                    Id = tb5.Id,
                                                                    location_name = tb5.location_name
                                                                }).FirstOrDefault()
                                                };

                resp.Data = que.FirstOrDefault();
                resp.Success = true;
                JobVacancy newVacancy = new JobVacancy();
                newVacancy = _repo.FindByID(id);
                newVacancy.posting_status = "4";

                _repo.Update(false, newVacancy);

                _repo.SaveChanges();
                _repo.Dispose();
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }


        string GetStatusPostingCode(string posting_status)
        {
            string result = "";
            if (posting_status == "5")
            {
                result = "Closed";
            }else
            {
                result = "Avalaible";
            }
            return result;
        }
        public GeneralResponseList<JobVacancyDto> ListPostingCode()
        {
            GeneralResponseList<JobVacancyDto> resp = new GeneralResponseList<JobVacancyDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<JobVacancyDto> que = from tb1 in _repo.GetContext().Set<JobVacancy>()
                                                where tb1.is_deleted == false && tb1.posting_status != "5"
                                                select new JobVacancyDto()
                                                {
                                                    code = tb1.code,
                                                    vacancy_name = tb1.code + " - " + tb1.vacancy_name + " - " + GetStatusPostingCode(tb1.posting_status),
                                                    created_on = tb1.created_on
                                                };

                resp.RecordsTotal = que.Count();
                //que = q.filterEquals(que, "job_tittle_id", filter.Search_job_tittle_id);
                //que = q.filterEquals(que, "mpp_budged", filter.Search_mpp_budged);
                //que = q.filterEquals(que, "mpp_id", filter.Search_mpp_id);
                //que = q.filterContains(que, "ttf", filter.Search_ttf);
                que = que.OrderByDescending(x => x.created_on);


                resp.RecordsFiltered = que.Count();
                resp.Total = resp.RecordsTotal;

                //que = q.sortir(que, "Id", filter.sortBy, filter.sortDir);
                //que = q.limit(que, filter.GetLimit(), filter.GetOffset());

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

        public GeneralResponse<JobVacancyDto> GetVancacyNameByCode(string id)
        {
            GeneralResponse<JobVacancyDto> resp = new GeneralResponse<JobVacancyDto>();
            try
            {
                IQueryable<JobVacancyDto> que = (from tb1 in _repo.GetContext().Set<JobVacancy>()
                                                where tb1.code == id
                                                select new JobVacancyDto()
                                                {
                                                    Id = tb1.Id,
                                                    vacancy_name = tb1.vacancy_name
                                                }).OrderByDescending(x=>x.created_on);
                var data = que.FirstOrDefault();
                JobVacancy newVacancy = new JobVacancy();
                newVacancy = _repo.FindByID(data.Id);
                var ct = _repo.GetContext().Set<JobVacancy>().Where(x => x.hiring_request_id == newVacancy.hiring_request_id && x.is_deleted == false).Count();
                
                data.CountHIringRequest = ct;



                resp.Data = data;
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

