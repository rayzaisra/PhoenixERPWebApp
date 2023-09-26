using Kendo.Mvc.Extensions;
using Phoenix.Shared.Core.Areas.General.Dtos;
using Phoenix.Shared.Core.Areas.Hris.Dtos;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Parameters;
using Phoenix.Shared.Core.Repositories;
using Phoenix.Shared.Core.Services;
using Phoenix.Shared.Helpers;
using Phoenix.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Phoenix.Shared.Core.Areas.Hris.Services
{
    public partial interface IHiringRequestServices : IBaseService<HiringRequestDto, HiringRequest, string>
    {
        GeneralResponseList<HiringRequestDto> ListAll(HiringRequestSearch filter);
        GeneralResponseList<HiringRequestDto> ListAllWithDetial(HiringRequestSearch filter);
        GeneralResponseList<MppDetailDto> GetListHiringMppDetail(HiringRequestGetMppDetail dto);
        GeneralResponse<HiringRequestDto> GetListHiringMppWithID(string id);
        GeneralResponse<MppDetailDto> GetGradeDataInBusinessUnit(HiringRequestGetMppDetail dto);
        GeneralResponse<HiringRequestDto> GetWithDetil(string id);
        GeneralResponse<HiringRequestDto> GetDataRequester(string id);
        GeneralResponse<HiringRequest> CreateCustomDetail(HiringRequestNew dto);
        GeneralResponse<HiringRequest> UpdateStatusHiringRequest(HiringRequestDto dto);
        GeneralResponse<HiringRequest> DeleteWithDetail(string id);
        GeneralResponse<HiringRequest> UpdateAllDataHiringRequest(HiringRequestDto dto);
        GeneralResponse<HiringRequestDto> GetListHiringMppWithCode(string id);
    }
    public partial class HiringRequestServices : BaseService<HiringRequestDto, HiringRequest, string>, IHiringRequestServices
    {
        IEFRepository<MppDetail, string> _repoMppDetail;
        IEFRepository<HiringRequestApproval, string> _repoHiringRequestApproval;
        public HiringRequestServices(IEFRepository<HiringRequest, string> repo, IEFRepository<MppDetail, string> repoMppDetail, IEFRepository<HiringRequestApproval, string> repoHiringRequestApproval) : base(repo)
        {
            _repo = repo;
            _repoMppDetail = repoMppDetail;
            _repoHiringRequestApproval = repoHiringRequestApproval;

        }

        public GeneralResponse<HiringRequest> CreateCustomDetail(HiringRequestNew dto)
        {
            GeneralResponse<HiringRequest> resp = new GeneralResponse<HiringRequest>();
            try
            {
                HiringRequest NewParam = new HiringRequest();
                List<MppNewRealData> data = dto.dataDetailMpp;
                PropertyMapper.All(dto, NewParam);
                NewParam.status = dto.status;
                NewParam.mpp_budged = data[0].mppbudget;
                NewParam.date_fulfilment = data[0].dff;
                NewParam.detail_candidate_qualification = data[0].Description;
                NewParam.detail_candidate_responsibilites = data[0].descresponsibilites;
                NewParam.employment_status = data[0].status;
                NewParam.ttf = data[0].ttf;
                NewParam.job_grade_id = data[0].grade;
                NewParam.job_title_id = data[0].jobtitle;
                NewParam.budget_souce = data[0].budget_souce;

                if (dto.hiring_request_type_id == "Replacement")
                {
                    NewParam.replacement_employee_basic_info_id = dto.replacement_employee_basic_info_id;
                }

                if (dto.hiring_request_type_id == "Budgeted")
                {
                    MppDetail newMppDetial = new MppDetail();
                    newMppDetial = _repoMppDetail.FindByID(data[0].Id);
                    newMppDetial.mpp_id = NewParam.mpp_id;
                    newMppDetial.status = "1";
                    _repoMppDetail.Update(false, newMppDetial);
                }

                if (dto.id != null)
                {
                    var DataHiring = _repo.GetContext().Set<HiringRequest>().Where(x => x.Id == dto.id && x.is_deleted == false).Select(x => new HiringRequestDto { Id = x.Id, created_on = x.created_on, created_by = x.created_by }).FirstOrDefault();
                    NewParam.Id = dto.id;
                    NewParam.created_by = DataHiring.created_by;
                    NewParam.created_on = DataHiring.created_on;
                    _repo.Update(false, NewParam);
                }
                else
                {
                    _repo.Create(false, NewParam);
                }

                _repo.SaveChanges();

                if (dto.hiring_request_type_id == "Budgeted")
                {
                    _repoMppDetail.SaveChanges();
                }

                _repo.Dispose();
                if (dto.hiring_request_type_id == "Budgeted")
                {
                    _repoMppDetail.Dispose();
                }

                resp.Success = true;
                resp.Message = "Sucess";
                resp.Code = "00";
                resp.Data = NewParam;

            }
            catch (Exception x)
            {
                resp.Success = false;
                resp.Code = "500";
                resp.Message = x.Message.ToString();
            }

            return resp;
        }

        public GeneralResponse<HiringRequest> DeleteWithDetail(string id)
        {
            GeneralResponse<HiringRequest> resp = new GeneralResponse<HiringRequest>();
            try
            {

                IQueryable<HiringRequestApproval> que = (from tb1 in _repo.GetContext().Set<HiringRequestApproval>()
                                                         where tb1.hiring_request_id == id
                                                         select new HiringRequestApproval()
                                                         {
                                                             Id = tb1.Id,
                                                             hiring_request_id = tb1.hiring_request_id
                                                         });
                List<HiringRequestApproval> listApproval = que.ToList();



                foreach (HiringRequestApproval data in listApproval)
                {

                    _repoHiringRequestApproval.RemoveByID(false, data.Id);
                }

                _repo.RemoveByID(false, id);


                _repo.SaveChanges();
                _repoHiringRequestApproval.SaveChanges();
                _repo.Dispose();
                _repoHiringRequestApproval.Dispose();
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

        public GeneralResponse<HiringRequestDto> GetDataRequester(string id)
        {
            GeneralResponse<HiringRequestDto> resp = new GeneralResponse<HiringRequestDto>();

            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<HiringRequestDto> que = from tb1 in _repo.GetContext().Set<EmployeeBasicInfo>()
                                                   join tbul in _repo.GetContext().Set<BusinessUnitJobLevel>() on tb1.business_unit_job_level_id equals tbul.Id into aLeftJoin
                                                   from a in aLeftJoin.DefaultIfEmpty()
                                                   join tb3 in _repo.GetContext().Set<JobTitle>() on a.job_title_id equals tb3.Id into bLeftJoin
                                                   from b in bLeftJoin.DefaultIfEmpty()
                                                   join tb2 in _repo.GetContext().Set<BusinessUnit>() on a.business_unit_division_id equals tb2.Id into cLeftJoin
                                                   from c in cLeftJoin.DefaultIfEmpty()
                                                   where tb1.Id == id
                                                   select new HiringRequestDto()
                                                   {
                                                       Id = tb1.Id,
                                                       requester_name = tb1.name_employee,
                                                       job_title_name = b.title_name,
                                                       business_unit_name = c.unit_name,
                                                   };
                resp.Data = que.FirstOrDefault();
                resp.Success = true;
                resp.Code = "00";
                resp.Message = "sucess";
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }

        public GeneralResponse<MppDetailDto> GetGradeDataInBusinessUnit(HiringRequestGetMppDetail dto)
        {
            GeneralResponse<MppDetailDto> resp = new GeneralResponse<MppDetailDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<MppDetailDto> que = from tb1 in _repo.GetContext().Set<BusinessUnitJobLevel>()
                                               join
tb2 in _repo.GetContext().Set<JobGrade>() on tb1.job_grade_id equals tb2.Id
                                               //where tb1.business_unit_division_id == dto.business_unit_id && tb1.job_title_id==dto.job_tittle_id
                                               where tb1.job_title_id == dto.job_tittle_id

                                               select new MppDetailDto()
                                               {
                                                   Id = tb1.Id,
                                                   job_grade_name = tb2.grade_name,
                                                   job_grade_id = tb2.Id
                                               };



                resp.Data = que.FirstOrDefault();
                resp.Success = true;
                resp.Code = "00";
                resp.Message = "sucess";

            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }

        public GeneralResponseList<MppDetailDto> GetListHiringMppDetail(HiringRequestGetMppDetail dto)
        {
            GeneralResponseList<MppDetailDto> resp = new GeneralResponseList<MppDetailDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<MppDetailDto> que = from tb1 in _repo.GetContext().Set<MppDetail>()
                                               join tb2 in _repo.GetContext().Set<Mpp>() on tb1.mpp_id equals tb2.Id
                                               join tb3 in _repo.GetContext().Set<JobGrade>() on tb1.job_grade_id equals tb3.Id
                                               join tb4 in _repo.GetContext().Set<JobTitle>() on tb1.job_tittle_id equals tb4.Id
                                               where tb1.job_tittle_id == dto.job_tittle_id && tb1.status == "0"
                                               //where tb2.business_unit_id == dto.business_unit_id && tb1.job_tittle_id == dto.job_tittle_id && tb1.status == "0"
                                               select new MppDetailDto()
                                               {
                                                   Id = tb1.Id,
                                                   employment_status = tb1.employment_status,
                                                   job_grade_id = tb1.job_grade_id,
                                                   job_tittle_id = tb1.job_tittle_id,
                                                   ttf = tb1.ttf,
                                                   porject_name = tb1.project_name,
                                                   Description = tb1.Description,
                                                   mpp_budged = tb1.mpp_budged,
                                                   business_unit_id = tb2.business_unit_id,
                                                   mpp_id = tb2.Id,
                                                   status = tb1.status,
                                                   job_title_name = tb4.title_name,
                                                   job_grade_name = tb3.grade_name,
                                                   budget_souce = tb1.budget_souce

                                               };

                resp.RecordsTotal = que.Count();
                resp.RecordsFiltered = que.Count();
                resp.Total = resp.RecordsTotal;

                resp.Rows = que.ToList();
                resp.Success = true;
                resp.Code = "00";
                resp.Message = "sucess";

            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;

        }

        public GeneralResponse<HiringRequestDto> GetListHiringMppWithID(string id)
        {
            GeneralResponse<HiringRequestDto> resp = new GeneralResponse<HiringRequestDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<HiringRequestDto> que = from tb1 in _repo.GetContext().Set<HiringRequest>()
                                                   join tb2 in _repo.GetContext().Set<JobTitle>() on tb1.job_title_id equals tb2.Id
                                                   join tb3 in _repo.GetContext().Set<JobGrade>() on tb1.job_grade_id equals tb3.Id
                                                   join tb4 in _repo.GetContext().Set<EmployeeBasicInfo>() on tb1.requester_employee_basic_info_id equals tb4.Id
                                                   join tbu in _repo.GetContext().Set<BusinessUnitJobLevel>() on tb4.business_unit_job_level_id equals tbu.Id
                                                   join tb5 in _repo.GetContext().Set<BusinessUnit>() on tbu.business_unit_division_id equals tb5.Id
                                                   where tb1.Id == id
                                                   select new HiringRequestDto()
                                                   {
                                                       Id = tb1.Id,
                                                       employment_status = tb1.employment_status,
                                                       job_grade_id = tb1.job_grade_id,
                                                       job_title_id = tb1.job_title_id,
                                                       basic_info_grade_tittle = tb1.basic_info_grade_tittle,
                                                       replacement_employee_basic_info_id = tb1.replacement_employee_basic_info_id,
                                                       hiring_request_type_id = tb1.hiring_request_type_id,
                                                       ttf = tb1.ttf,
                                                       detail_candidate_qualification = tb1.detail_candidate_qualification,
                                                       detail_candidate_responsibilites = tb1.detail_candidate_responsibilites,
                                                       mpp_budged = tb1.mpp_budged,
                                                       requester_employee_basic_info_id = tb1.requester_employee_basic_info_id,
                                                       mpp_id = tb2.Id,
                                                       status = tb1.status,
                                                       job_title_name = tb2.title_name,
                                                       job_grade_name = tb3.grade_name,
                                                       budget_souce = tb1.budget_souce,
                                                       created_on = tb1.created_on,
                                                       date_fulfilment = tb1.date_fulfilment,
                                                       requester_name = tb4.name_employee,
                                                       business_unit_name = tb5.unit_name,
                                                       code = tb1.code,
                                                       replacedEmployee = (from tbie in _repo.GetContext().Set<EmployeeBasicInfo>()
                                                                           where tb1.replacement_employee_basic_info_id == tbie.Id
                                                                           select new ReplaceEmployee
                                                                           {
                                                                               employeebasicinfoId = tbie.Id,
                                                                               employee_replaced_name = tbie.name_employee
                                                                           }).FirstOrDefault(),
                                                       business_unit_id = tb5.Id,
                                                       departement_id = tbu.business_unit_departement_id,
                                                       departement_name = (from tbd in _repo.GetContext().Set<BusinessUnit>()
                                                                           where tbd.Id == tbu.business_unit_departement_id
                                                                           select tbd.unit_name).FirstOrDefault()

                                                   };



                resp.Data = que.FirstOrDefault();
                resp.Success = true;
                resp.Code = "00";
                resp.Message = "sucess";

            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }

        public GeneralResponse<HiringRequestDto> GetListHiringMppWithCode(string code)
        {
            GeneralResponse<HiringRequestDto> resp = new GeneralResponse<HiringRequestDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<HiringRequestDto> que = from tb1 in _repo.GetContext().Set<HiringRequest>()
                                                   join tb2 in _repo.GetContext().Set<JobTitle>() on tb1.job_title_id equals tb2.Id
                                                   join tb3 in _repo.GetContext().Set<JobGrade>() on tb1.job_grade_id equals tb3.Id
                                                   join tb4 in _repo.GetContext().Set<EmployeeBasicInfo>() on tb1.requester_employee_basic_info_id equals tb4.Id
                                                   join tbu in _repo.GetContext().Set<BusinessUnitJobLevel>() on tb4.business_unit_job_level_id equals tbu.Id
                                                   join tb5 in _repo.GetContext().Set<BusinessUnit>() on tbu.business_unit_division_id equals tb5.Id
                                                   where tb1.code.Trim() == code.Trim()
                                                   select new HiringRequestDto()
                                                   {
                                                       Id = tb1.Id,
                                                       employment_status = tb1.employment_status,
                                                       job_grade_id = tb1.job_grade_id,
                                                       job_title_id = tb1.job_title_id,
                                                       basic_info_grade_tittle = tb1.basic_info_grade_tittle,
                                                       replacement_employee_basic_info_id = tb1.replacement_employee_basic_info_id,
                                                       hiring_request_type_id = tb1.hiring_request_type_id,
                                                       ttf = tb1.ttf,
                                                       detail_candidate_qualification = tb1.detail_candidate_qualification,
                                                       detail_candidate_responsibilites = tb1.detail_candidate_responsibilites,
                                                       mpp_budged = tb1.mpp_budged,
                                                       requester_employee_basic_info_id = tb1.requester_employee_basic_info_id,
                                                       mpp_id = tb2.Id,
                                                       status = tb1.status,
                                                       job_title_name = tb2.title_name,
                                                       job_grade_name = tb3.grade_name,
                                                       budget_souce = tb1.budget_souce,
                                                       created_on = tb1.created_on,
                                                       date_fulfilment = tb1.date_fulfilment,
                                                       requester_name = tb4.name_employee,
                                                       business_unit_name = tb5.unit_name,
                                                       code = tb1.code
                                                   };



                resp.Data = que.FirstOrDefault();
                resp.Success = true;
                resp.Code = "00";
                resp.Message = "sucess";

            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }
        public GeneralResponse<HiringRequestDto> GetWithDetil(string id)
        {
            GeneralResponse<HiringRequestDto> resp = new GeneralResponse<HiringRequestDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<HiringRequestDto> que = from tb1 in _repo.GetContext().Set<HiringRequest>()
                                                   join tb2 in _repo.GetContext().Set<EmployeeBasicInfo>() on tb1.requester_employee_basic_info_id equals tb2.Id
                                                   join tbjl in _repo.GetContext().Set<BusinessUnitJobLevel>() on tb2.business_unit_job_level_id equals tbjl.Id
                                                   join tbt in _repo.GetContext().Set<JobTitle>() on tbjl.job_title_id equals tbt.Id
                                                   where tb1.Id == id
                                                   select new HiringRequestDto()
                                                   {
                                                       Id = tb1.Id,
                                                       approval_id = tb1.approval_id,
                                                       basic_info_grade_tittle = tb1.basic_info_grade_tittle,
                                                       detail_candidate_qualification = tb1.detail_candidate_qualification,
                                                       detail_candidate_responsibilites = tb1.detail_candidate_responsibilites,
                                                       job_grade_id = tb1.job_grade_id,
                                                       job_title_id = tb1.job_title_id,
                                                       code = tb1.code,
                                                       date_fulfilment = tb1.date_fulfilment,
                                                       requester_employee_basic_info_id = tb1.requester_employee_basic_info_id,
                                                       employment_status = tb1.employment_status,
                                                       hiring_request_type_id = tb1.hiring_request_type_id,
                                                       mpp_budged = tb1.mpp_budged,
                                                       requester_name = tb2.name_employee,
                                                       job_title_requester = tbt.title_name,
                                                       mpp_id = tb1.mpp_id,
                                                       ttf = tb1.ttf,
                                                       budget_souce = tb1.budget_souce,
                                                       status = tb1.status,
                                                       Jobtitile = (from tb3 in _repo.GetContext().Set<JobTitle>()
                                                                    where tb1.job_title_id == tb3.Id
                                                                    select new JobTitleDto
                                                                    {
                                                                        Id = tb3.Id,
                                                                        title_code = tb3.title_code,
                                                                        title_name = tb3.title_name
                                                                    }).FirstOrDefault(),
                                                       Approval = (from tb4 in _repo.GetContext().Set<HiringRequestApproval>()
                                                                   where tb1.Id == tb4.hiring_request_id
                                                                   select new HiringRequestApprovalDto
                                                                   {
                                                                       Id = tb4.Id,
                                                                       created_by = tb4.created_by,
                                                                       created_on = tb4.created_on
                                                                   }).ToList(),
                                                       Division = (from tb6 in _repo.GetContext().Set<BusinessUnit>()
                                                                   where tb6.Id == tbjl.business_unit_division_id
                                                                   select new BusinessUnitDto
                                                                   {
                                                                       Id = tb6.Id,
                                                                       unit_code = tb6.unit_code,
                                                                       unit_name = tb6.unit_name
                                                                   }).FirstOrDefault(),
                                                       JobGrade = (from tbg in _repo.GetContext().Set<JobGrade>()
                                                                   where tb1.job_grade_id == tbg.Id
                                                                   select new JobGradeDto
                                                                   {
                                                                       Id = tbg.Id,
                                                                       grade_code = tbg.grade_code,
                                                                       grade_name = tbg.grade_name

                                                                   }).FirstOrDefault(),
                                                       replacedEmployee = (from tebi in _repo.GetContext().Set<EmployeeBasicInfo>()
                                                                           where tb1.replacement_employee_basic_info_id == tebi.Id
                                                                           select new ReplaceEmployee
                                                                           {
                                                                               employeebasicinfoId = tebi.Id,
                                                                               employee_replaced_name = tebi.name_employee
                                                                           }).FirstOrDefault()
                                                   };

                resp.Data = que.FirstOrDefault();
                if (resp.Data.mpp_id != null)
                {
                    resp.Data.dataDetailMpp = (from a in _repo.GetContext().Set<MppDetail>()
                                               where a.mpp_id == resp.Data.mpp_id
                                               select new MppNewRealData()
                                               {
                                                   Id = a.Id,
                                                   budget_souce = a.budget_souce,
                                                   Description = a.Description,
                                                   grade = a.job_grade_id,
                                                   mppbudget = a.mpp_budged,
                                                   status = a.employment_status,
                                                   ttf = a.ttf,
                                                   jobtitle = a.job_tittle_id
                                               }).ToList();
                }

                resp.Success = true;

            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }

        public GeneralResponseList<HiringRequestDto> ListAll(HiringRequestSearch filter)
        {

            GeneralResponseList<HiringRequestDto> resp = new GeneralResponseList<HiringRequestDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<HiringRequestDto> que = from tb1 in _repo.GetContext().Set<HiringRequest>()
                                                   select new HiringRequestDto()
                                                   {
                                                       Id = tb1.Id,
                                                       approval_id = tb1.approval_id,
                                                       basic_info_grade_tittle = tb1.basic_info_grade_tittle,
                                                       detail_candidate_qualification = tb1.detail_candidate_qualification,
                                                       code = tb1.code,
                                                       date_fulfilment = tb1.date_fulfilment,
                                                       requester_employee_basic_info_id = tb1.requester_employee_basic_info_id,
                                                       employment_status = tb1.employment_status,
                                                       hiring_request_type_id = tb1.hiring_request_type_id,
                                                       mpp_budged = tb1.mpp_budged,
                                                       mpp_id = tb1.mpp_id,
                                                       ttf = tb1.ttf,
                                                       status = tb1.status
                                                   };

                resp.RecordsTotal = que.Count();

                que = q.filterEquals(que, "approval_id", filter.Search_approval_id);
                que = q.filterContains(que, "basic_info_grade_tittle", filter.Search_basic_info_grade_tittle);
                que = q.filterContains(que, "detail_candidate_qualification", filter.Search_candidate_qualification);
                que = q.filterContains(que, "code", filter.Search_code);
                que = q.filterEquals(que, "date_fulfilment", filter.Search_date_fulfilment);
                que = q.filterEquals(que, "requester_employee_basic_info_id", filter.Search_employee_basic_info_id);
                que = q.filterContains(que, "employment_status", filter.Search_employment_status);
                que = q.filterContains(que, "hiring_request_type_id", filter.Search_hiring_type);
                que = q.filterEquals(que, "mpp_budged", filter.Search_mpp_budged);
                que = q.filterEquals(que, "mpp_id", filter.Search_mpp_id);
                que = q.filterContains(que, "ttf", filter.Search_ttf);
                que = q.filterEquals(que, "status", filter.Search_status);



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

        public GeneralResponseList<HiringRequestDto> ListAllWithDetial(HiringRequestSearch filter)
        {
            GeneralResponseList<HiringRequestDto> resp = new GeneralResponseList<HiringRequestDto>();
            try
            {
                var employeeid = Glosing.Instance.EmployeeId;
                PagingHelper q = new PagingHelper();

                IQueryable<HiringRequestDto> que = from tb1 in _repo.GetContext().Set<HiringRequest>()
                                                   join tb2 in _repo.GetContext().Set<EmployeeBasicInfo>() on tb1.requester_employee_basic_info_id equals tb2.Id
                                                   //join tce in _repo.GetContext().Set<TrTemplateApproval>() on tb1.Id equals tce.RefId into tcej
                                                   //from tcelj in tcej.DefaultIfEmpty()
                                                   orderby tb1.created_on descending
                                                   select new HiringRequestDto()
                                                   {
                                                       Id = tb1.Id,
                                                       approval_id = tb1.approval_id,
                                                       basic_info_grade_tittle = tb1.basic_info_grade_tittle,
                                                       detail_candidate_qualification = tb1.detail_candidate_qualification,
                                                       code = tb1.code,
                                                       date_fulfilment = tb1.date_fulfilment,
                                                       requester_employee_basic_info_id = tb1.requester_employee_basic_info_id,
                                                       employment_status = tb1.employment_status,
                                                       hiring_request_type_id = tb1.hiring_request_type_id,
                                                       mpp_budged = tb1.mpp_budged,
                                                       mpp_id = tb1.mpp_id,
                                                       ttf = tb1.ttf,
                                                       status = tb1.status,
                                                       StatusApproved = Phoenix.Data.Models.StatusTransaction.StatusName(Convert.ToInt32(tb1.status)),
                                                       Jobtitile = (from tb3 in _repo.GetContext().Set<JobTitle>()
                                                                    where tb1.job_title_id == tb3.Id
                                                                    select new JobTitleDto
                                                                    {
                                                                        Id = tb3.Id,
                                                                        title_code = tb3.title_code,
                                                                        title_name = tb3.title_name
                                                                    }).FirstOrDefault(),
                                                       Approval = (from tb4 in _repo.GetContext().Set<TrTemplateApproval>()
                                                                   join tbb in _repo.GetContext().Set<TrUserApproval>() on tb4.Id equals tbb.TrTempApprovalId
                                                                   join tbe in _repo.GetContext().Set<EmployeeBasicInfo>() on tbb.EmployeeBasicInfoId equals tbe.Id
                                                                   where tb1.Id == tb4.RefId
                                                                   select new HiringRequestApprovalDto
                                                                   {
                                                                       Id = tb4.Id,
                                                                       created_by = tbe.name_employee,
                                                                       created_on = tbb.CreatedOn,
                                                                       StatusApproved = tbb.StatusApproved.ToString()
                                                                   }).ToList(),
                                                       Division = (from tb5 in _repo.GetContext().Set<BusinessUnitJobLevel>()
                                                                   join tb6 in _repo.GetContext().Set<BusinessUnit>() on tb5.business_unit_division_id equals tb6.Id
                                                                   where   tb2.business_unit_job_level_id == tb5.Id 
                                                                   select new BusinessUnitDto
                                                                   {
                                                                       Id = tb6.Id,
                                                                       unit_code = tb6.unit_code,
                                                                       unit_name = tb6.unit_name != null ? tb6.unit_name : tb6.unit_code,
                                                                   }).FirstOrDefault(), 
                                                       created_on = tb1.created_on,
                                                       created_by = tb1.created_by
                                                   };

                IQueryable<HiringRequestDto> que2 = from tb1 in _repo.GetContext().Set<HiringRequest>()
                                                    join tb2 in _repo.GetContext().Set<EmployeeBasicInfo>() on tb1.requester_employee_basic_info_id equals tb2.Id
                                                    join tce in _repo.GetContext().Set<TrTemplateApproval>() on tb1.Id equals tce.RefId
                                                    join tca in _repo.GetContext().Set<TrUserApproval>() on tce.Id equals tca.TrTempApprovalId
                                                    //where tca.EmployeeBasicInfoId == employeeid
                                                    orderby tb1.created_on descending
                                                    select new HiringRequestDto()
                                                    {
                                                        Id = tb1.Id,
                                                        approval_id = tb1.approval_id,
                                                        basic_info_grade_tittle = tb1.basic_info_grade_tittle,
                                                        detail_candidate_qualification = tb1.detail_candidate_qualification,
                                                        code = tb1.code,
                                                        date_fulfilment = tb1.date_fulfilment,
                                                        requester_employee_basic_info_id = tb1.requester_employee_basic_info_id,
                                                        employment_status = tb1.employment_status,
                                                        hiring_request_type_id = tb1.hiring_request_type_id,
                                                        mpp_budged = tb1.mpp_budged,
                                                        mpp_id = tb1.mpp_id,
                                                        ttf = tb1.ttf,
                                                        status = tb1.status,
                                                        StatusApproved = tce.StatusApproved.ToString(),
                                                        Jobtitile = (from tb3 in _repo.GetContext().Set<JobTitle>()
                                                                     where tb1.job_title_id == tb3.Id
                                                                     select new JobTitleDto
                                                                     {
                                                                         Id = tb3.Id,
                                                                         title_code = tb3.title_code,
                                                                         title_name = tb3.title_name
                                                                     }).FirstOrDefault(),
                                                        Approval = (from tb4 in _repo.GetContext().Set<TrTemplateApproval>()
                                                                    join tbb in _repo.GetContext().Set<TrUserApproval>() on tb4.Id equals tbb.TrTempApprovalId
                                                                    join tbe in _repo.GetContext().Set<EmployeeBasicInfo>() on tbb.EmployeeBasicInfoId equals tbe.Id
                                                                    where tb1.Id == tb4.RefId
                                                                    select new HiringRequestApprovalDto
                                                                    {
                                                                        Id = tb4.Id,
                                                                        created_by = tbe.name_employee,
                                                                        created_on = tbb.CreatedOn,
                                                                        StatusApproved = tbb.StatusApproved.ToString()
                                                                    }).ToList(),
                                                        Division = (from tb5 in _repo.GetContext().Set<BusinessUnitJobLevel>()
                                                                    join tb6 in _repo.GetContext().Set<BusinessUnit>() on tb5.business_unit_division_id equals tb6.Id
                                                                    where tb2.business_unit_job_level_id == tb5.Id
                                                                    select new BusinessUnitDto
                                                                    {
                                                                        Id = tb6.Id,
                                                                        unit_code = tb6.unit_code,
                                                                        unit_name = tb6.unit_name != null ? tb6.unit_name : tb6.unit_code,
                                                                    }).FirstOrDefault(),
                                                        created_on = tb1.created_on,
                                                        created_by = tb1.created_by
                                                    };

                resp.RecordsTotal = que.Count();

                //string businessUnitDivisionId = (from a in _repo.GetContext().Set<EmployeeBasicInfo>()
                //                                 join b in _repo.GetContext().Set<BusinessUnitJobLevel>() on a.business_unit_job_level_id equals b.Id
                //                                 join c in _repo.GetContext().Set<BusinessUnit>() on b.business_unit_division_id equals c.Id
                //                                 where a.Id == employeeid
                //                                 select c.Id).FirstOrDefault();

                //if (businessUnitDivisionId != "be2050a0-7402-4e2d-b4fb-2e4c4040e9be")
                //{
                    que = q.filterEquals(que, "approval_id", filter.Search_approval_id);
                    que = q.filterContains(que, "basic_info_grade_tittle", filter.Search_basic_info_grade_tittle);
                    que = q.filterContains(que, "detail_candidate_qualification", filter.Search_candidate_qualification);
                    que = q.filterContains(que, "code", filter.Search_code);
                    que = q.filterEquals(que, "date_fulfilment", filter.Search_date_fulfilment);
                    que = q.filterEquals(que, "requester_employee_basic_info_id", filter.Search_employee_basic_info_id);
                    que = q.filterContains(que, "employment_status", filter.Search_employment_status);
                    que = q.filterContains(que, "hiring_request_type_id", filter.Search_hiring_type);
                    que = q.filterEquals(que, "mpp_budged", filter.Search_mpp_budged);
                    que = q.filterEquals(que, "mpp_id", filter.Search_mpp_id);
                    que = q.filterContains(que, "ttf", filter.Search_ttf);
                    que = q.filterEquals(que, "status", filter.Search_status);
                    //que = q.filterEquals(que, "created_by", employeeid);
                    //que = que.Union(que2).OrderByDescending( x => x.created_on);
               // }



                resp.RecordsFiltered = que.Count();
                resp.Total = resp.RecordsTotal;

              
                if (filter.sortBy == "created_on")
                {
                    que = q.sortir(que, "created_on", filter.sortBy, filter.sortDir);
                }
                if (filter.sortBy == "hiring_request_type_id")
                {
                    que = q.sortir(que, "hiring_request_type_id", filter.sortBy, filter.sortDir);
                }

                if (filter.sortBy == "Id")
                {
                    que = q.sortir(que, "Id", filter.sortBy, filter.sortDir);
                }

                // que = q.sortir(que, "statusApproved", filter.sortBy, filter.sortDir);
                //que = q.sortir(que, "Id", filter.sortBy, filter.sortDir);
                que = q.limit(que, filter.GetLimit(), filter.GetOffset());
               

                resp.Rows = que.OrderByDescending(x => x.created_on).ToList();
                resp.Success = true;

            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }

        public GeneralResponse<HiringRequest> UpdateAllDataHiringRequest(HiringRequestDto dto)
        {
            GeneralResponse<HiringRequest> resp = new GeneralResponse<HiringRequest>();
            try
            {
                HiringRequest newData = _repo.FindByID(dto.Id);
                PropertyMapper.All(dto, newData);
                PropertyInfo pi = newData.GetType().GetProperty("updated_by");
                if (pi != null)
                {
                    pi.SetValue(newData, Glosing.Instance.Username);
                }
                PropertyInfo pd = newData.GetType().GetProperty("updated_date");
                if (pd != null)
                {
                    pd.SetValue(newData, DateTime.Now);
                }


                IQueryable<HiringRequestApproval> que = (from tb1 in _repo.GetContext().Set<HiringRequestApproval>()
                                                         where tb1.hiring_request_id == dto.Id
                                                         select new HiringRequestApproval()
                                                         {
                                                             Id = tb1.Id,
                                                             hiring_request_id = tb1.hiring_request_id
                                                         });
                List<HiringRequestApproval> listData = que.ToList();

                foreach (HiringRequestApproval data in listData)
                {

                    _repoHiringRequestApproval.RemoveByID(false, data.Id);
                }

                if (dto.Approval != null)
                {
                    foreach (HiringRequestApprovalDto data in dto.Approval)
                    {
                        HiringRequestApproval newApproval = new HiringRequestApproval();
                        newApproval.hiring_request_id = dto.Id;
                        newApproval.created_by = data.created_by;
                        newApproval.created_on = data.created_on;
                        _repoHiringRequestApproval.Create(false, newApproval);
                    }
                }


                _repo.Update(false, newData);
                _repo.SaveChanges();
                _repoHiringRequestApproval.SaveChanges();
                _repo.Dispose();
                _repoHiringRequestApproval.Dispose();


                resp.Success = true;
                resp.Code = "00";
                resp.Message = "sucess";
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }

        public GeneralResponse<HiringRequest> UpdateStatusHiringRequest(HiringRequestDto dto)
        {
            GeneralResponse<HiringRequest> resp = new GeneralResponse<HiringRequest>();
            try
            {
                HiringRequest newData = _repo.FindByID(dto.Id);
                newData.status = dto.status;
                PropertyInfo pi = newData.GetType().GetProperty("updated_by");
                if (pi != null)
                {
                    pi.SetValue(newData, Glosing.Instance.Username);
                }
                PropertyInfo pd = newData.GetType().GetProperty("updated_date");
                if (pd != null)
                {
                    pd.SetValue(newData, DateTime.Now);
                }
                _repo.Update(false, newData);
                _repo.SaveChanges();
                _repo.Dispose();



                //HiringRequestApproval app = new HiringRequestApproval();
                //app.hiring_request_id = newData.Id;
                //app.is_deleted = false;

                //_repoHiringRequestApproval.Create(false, app);


                //_repoHiringRequestApproval.SaveChanges();

                //_repoHiringRequestApproval.Dispose();
                resp.Success = true;
                resp.Code = "00";
                resp.Message = "sucess";
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

