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


namespace Phoenix.Shared.Core.Areas.Hris.Services
{
    public partial interface ITrainingRegistrationServices : IBaseService<TrainingRequisitionDto, TrainingRequisition, string>
    {
        GeneralResponse<TrainingRequisitionDto> GetWithDetial(string id);
   
    }
    public partial class TrainingRegistrationServices : BaseService<TrainingRequisitionDto, TrainingRequisition, string>, ITrainingRegistrationServices
    {
        private IEFRepository<TrainingRequisitionDetial, string> _repoTrainingReqDetail;
        public TrainingRegistrationServices(IEFRepository<TrainingRequisition, string> repo, IEFRepository<TrainingRequisitionDetial, string> repoTrainingReqDetail) : base(repo)
        {
            _repoTrainingReqDetail = repoTrainingReqDetail;
            _repo = repo;
        }

        public GeneralResponse<TrainingRequisitionDto> GetWithDetial(string id)
        {
            GeneralResponse<TrainingRequisitionDto> resp = new GeneralResponse<TrainingRequisitionDto>();
            try
            {
                IQueryable<TrainingRequisitionDto> que = from tb1 in _repo.GetContext().Set<TrainingRequisition>()
                                                         join tb2 in _repo.GetContext().Set<EmployeeBasicInfo>() on tb1.employee_basic_info_id equals tb2.Id
                                                         join tbt in _repo.GetContext().Set<Training>() on tb1.training_id equals tbt.Id
                                                         join tbtap in _repo.GetContext().Set<TrTemplateApproval>() on tb1.Id equals tbtap.RefId
                                                         into tbtapau1
                                                         from tau1 in tbtapau1.DefaultIfEmpty()
                                                         where tb1.Id == id
                                                         select new TrainingRequisitionDto()
                                                         {
                                                             Id = tb1.Id,
                                                             code = tb1.code,
                                                             training_type = tbt.category,
                                                             traning_name = tbt.name,
                                                             training_id = tbt.Id,
                                                             training_cost = tbt.training_cost,
                                                             employee_basic_info_id = tb1.employee_basic_info_id,
                                                             purpose_end_date = tb1.purpose_end_date,
                                                             purpose_start_date = tb1.purpose_start_date,
                                                             notes = tb1.notes,
                                                             status = tb1.status,
                                                             statusReal = tau1.StatusApproved,
                                                             venue = tb1.venue,
                                                             detialTrainingRequester = (
                                                              from tbujl in _repo.GetContext().Set<BusinessUnitJobLevel>()
                                                              join tjg in _repo.GetContext().Set<JobGrade>() on tbujl.job_grade_id equals tjg.Id
                                                              join tjt in _repo.GetContext().Set<JobTitle>() on tbujl.job_title_id equals tjt.Id
                                                              where tbujl.Id == tb2.business_unit_job_level_id
                                                              select new detialTrainingRequester()
                                                              {
                                                                  job_grade_name = tjg.grade_name,
                                                                  job_title_name = tjt.title_name,
                                                                  request_by = tb2.name_employee
                                                              }).FirstOrDefault(),
                                                             ListParticipantTraining = (
                                                              from tbk in _repo.GetContext().Set<TrainingRequisitionDetial>()
                                                              join tbk2 in _repo.GetContext().Set<EmployeeBasicInfo>() on tbk.employee_basic_info_id equals tbk2.Id
                                                              join tbba in _repo.GetContext().Set<BusinessUnitJobLevel>() on tbk2.business_unit_job_level_id equals tbba.Id
                                                              join tbk3 in _repo.GetContext().Set<JobGrade>() on tbba.job_grade_id equals tbk3.Id
                                                              join tbk4 in _repo.GetContext().Set<JobTitle>() on tbba.job_title_id equals tbk4.Id
                                                              where tbk.training_requition_id == tb1.Id
                                                              select new participantTraining()
                                                              {
                                                                  Id = tbk.Id,
                                                                  employee_basic_info_id = tbk.employee_basic_info_id,
                                                                  name_employee = tbk2.name_employee,
                                                                  job_grade_name = tbk3.grade_name,
                                                                  job_title_name = tbk4.title_name,
                                                                  participantBusinessUnit = (
                                                                  from tbg in _repo.GetContext().Set<BusinessUnitJobLevel>()
                                                                  join tbg1 in _repo.GetContext().Set<BusinessUnit>() on tbg.business_unit_division_id equals tbg1.Id
                                                                  where tbg.Id == tbk2.business_unit_job_level_id
                                                                  select new participantBusinessUnit()
                                                                  {
                                                                      Id = tbg.Id,
                                                                      name = tbg1.unit_name
                                                                  }).FirstOrDefault()

                                                              }).ToList(),
                                                             created_on = tb1.created_on,
                                                             ApprovalReal = (from tap in _repo.GetContext().Set<TrTemplateApproval>()
                                                                             join tapd in _repo.GetContext().Set<TrUserApproval>() on tap.Id equals tapd.TrTempApprovalId
                                                                             join tapde in _repo.GetContext().Set<EmployeeBasicInfo>() on tapd.EmployeeBasicInfoId equals tapde.Id
                                                                             where tap.RefId == tb1.Id
                                                                             select new TrUserApprovalDto()
                                                                             {
                                                                                 approved_by = tapde.name_employee,
                                                                                 created_on = tapd.CreatedOn,
                                                                                 StatusApproved = tapd.StatusApproved
                                                                             }).ToList(),
                                                         };

                resp.Data = que.FirstOrDefault();
                resp.Message = "sucess";
                resp.Code = "00";
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

