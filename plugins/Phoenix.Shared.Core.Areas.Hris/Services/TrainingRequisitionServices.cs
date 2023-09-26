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
    public partial interface ITrainingRequisitionServices : IBaseService<TrainingRequisitionDto, TrainingRequisition, string>
    {
        GeneralResponseList<TrainingSchedule> GetTrainingSchedule();
        GeneralResponseList<TrainingRequisitionDto> ListAll(TrainingRequisitionSearch filter);
        GeneralResponseList<TrainingBondingDto> GetTrainingBonding(TrainingRequisitionSearch filter);
        GeneralResponse<TrainingRequisition> CreateWithDetail(TrainingRequisitionNew dto);
        GeneralResponse<TrainingRequisition> DeleteDetail(string id);
        GeneralResponse<TrainingRequisitionDto> GetWithDetial(string id);
        GeneralResponse<TrainingRequisition> UpdateDetial(TrainingRequisitionDto dto);
        GeneralResponse<TrainingRequisition> UpdateStatus(TrainingRequisitionDto dto);
        GeneralResponse<TrainingRequisition> ProcessPaymentandAdministration(TrainingRequisitionDto dto);
    }
    public partial class TrainingRequisitionServices : BaseService<TrainingRequisitionDto, TrainingRequisition, string>, ITrainingRequisitionServices
    {
        private IEFRepository<TrainingRequisitionDetial, string> _repoTrainingReqDetail;
        private IEFRepository<TrainingBonding, string> _repoTrainingBonding;
        public TrainingRequisitionServices(IEFRepository<TrainingRequisition, string> repo, IEFRepository<TrainingRequisitionDetial, string> repoTrainingReqDetail, IEFRepository<TrainingBonding, string> repoTrainingBonding) : base(repo)
        {
            _repoTrainingReqDetail = repoTrainingReqDetail;
            _repo = repo;
            _repoTrainingBonding = repoTrainingBonding;
        }

        public GeneralResponse<TrainingRequisition> CreateWithDetail(TrainingRequisitionNew dto)
        {
            GeneralResponse<TrainingRequisition> resp = new GeneralResponse<TrainingRequisition>();
            try
            {
                IQueryable<TrainingDto> que = (from tb1 in _repo.GetContext().Set<Training>()
                                                             where tb1.Id == dto.training_id
                                                             select new TrainingDto()
                                                             {
                                                                 Id = tb1.Id,
                                                                 category = tb1.category,
                                                                 training_type = tb1.training_type,
                                                                 bonding_detail = tb1.bonding_detail,
                                                                 training_cost = tb1.training_cost
                                                             });
                List<TrainingDto> dataTraining = que.ToList();



                Random rnd = new Random();

                TrainingRequisition NewTrainingRequisition = new TrainingRequisition();
                PropertyMapper.All(dto, NewTrainingRequisition);
                NewTrainingRequisition.code = "TRREQ" + DateTime.Now.ToString("HHmmss") + rnd.Next(1, 9999);
                NewTrainingRequisition.status = TrainingStatusDto.Draft;
                _repo.Create(false, NewTrainingRequisition);

                foreach (participantTraining data in dto.listParticipantTraining)
                {
                    TrainingRequisitionDetial newTrainingRequisitionDetial = new TrainingRequisitionDetial();
                    if (!data.employee_basic_info_id.Equals(""))
                    {
                        newTrainingRequisitionDetial.training_requition_id = NewTrainingRequisition.Id;
                        newTrainingRequisitionDetial.employee_basic_info_id = data.employee_basic_info_id;
                        _repoTrainingReqDetail.Create(false, newTrainingRequisitionDetial);
                    }

                }

                foreach (TrainingDto dt in dataTraining) {

                    if (dt.training_cost >= 15000000) {
                        TrainingBonding tb = new TrainingBonding();
                        tb.employee_basic_info_id = dto.employee_basic_info_id;
                        tb.training_req_id = NewTrainingRequisition.Id;
                        tb.start_date = DateTime.Now;
                        tb.end_date = DateTime.Now.AddDays(7);
                        _repoTrainingBonding.Create(false, tb);

                    }
                }

                _repoTrainingBonding.SaveChanges();
                _repo.SaveChanges();
                _repoTrainingReqDetail.SaveChanges();
                _repoTrainingBonding.Dispose();
                _repo.Dispose();
                _repoTrainingReqDetail.Dispose();
                resp.Code = "00";
                resp.Data = NewTrainingRequisition;
                resp.Message = "sucess";
                resp.Success = true;

            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }

        public GeneralResponse<TrainingRequisition> DeleteDetail(string id)
        {
            GeneralResponse<TrainingRequisition> resp = new GeneralResponse<TrainingRequisition>();
            try
            {
                IQueryable<TrainingRequisitionDetial> que = (from tb1 in _repo.GetContext().Set<TrainingRequisitionDetial>()
                                                where tb1.training_requition_id==id
                                                select new TrainingRequisitionDetial()
                                                {
                                                    Id = tb1.Id,
                                                    employee_basic_info_id =tb1.employee_basic_info_id
                                                });
                List<TrainingRequisitionDetial> listTrainingRequisitionDetial = que.ToList();


                foreach (TrainingRequisitionDetial data in listTrainingRequisitionDetial)
                {
                    _repoTrainingReqDetail.RemoveByID(false,data.Id);
                }

                _repo.RemoveByID(false, id);

                _repo.SaveChanges();
                _repoTrainingReqDetail.SaveChanges();
                _repo.Dispose();
                _repoTrainingReqDetail.Dispose();
                resp.Code = "00";
                resp.Message = "sucess";
                resp.Success = true;

            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
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
                                                           add_recomendation = tb1.add_recomendation,
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

        public GeneralResponseList<TrainingSchedule> GetTrainingSchedule()
        {
            GeneralResponseList<TrainingSchedule> resp = new GeneralResponseList<TrainingSchedule>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<TrainingSchedule> que = from tb1 in _repo.GetContext().Set<TrainingShareSession>()
                                                   join tb2 in _repo.GetContext().Set<TrainingRequisition>() on tb1.training_requisition_id equals tb2.Id
                                                   join tb3 in _repo.GetContext().Set<Training>() on tb2.training_id equals tb3.Id
                                                   select new TrainingSchedule()
                                                   {
                                                       className = "m-fc-event--primary",
                                                       description = tb2.notes,
                                                       title = tb3.name,
                                                       start = tb1.start_date.ToString()
                                                   };
                resp.RecordsTotal = que.Count();
                resp.RecordsFiltered = que.Count();
                resp.Total = resp.RecordsTotal;


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


        public GeneralResponseList<TrainingRequisitionDto> ListAll(TrainingRequisitionSearch filter)
        {

            GeneralResponseList<TrainingRequisitionDto> resp = new GeneralResponseList<TrainingRequisitionDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<TrainingRequisitionDto> que = from tb1 in _repo.GetContext().Set<TrainingRequisition>()
                                                         join tb3 in _repo.GetContext().Set<Training>() on tb1.training_id equals tb3.Id
                                                         join tb2 in _repo.GetContext().Set<EmployeeBasicInfo>() on tb1.employee_basic_info_id equals tb2.Id
                                                         join tbtap in _repo.GetContext().Set<TrTemplateApproval>() on tb1.Id equals tbtap.RefId
                                                         into tbtapau1
                                                         from tau1 in tbtapau1.DefaultIfEmpty()
                                                         select new TrainingRequisitionDto()
                                                          {
                                                              Id = tb1.Id,
                                                              code = tb1.code,
                                                              training_cost = tb3.training_cost,
                                                              detialTrainingRequester = (
                                                              from tbujl in _repo.GetContext().Set<BusinessUnitJobLevel>()
                                                              join tjg in _repo.GetContext().Set<JobGrade>() on tbujl.job_grade_id equals tjg.Id
                                                              join tjt in _repo.GetContext().Set<JobTitle>() on tbujl.job_title_id equals tjt.Id
                                                              where tbujl.Id == tb2.business_unit_job_level_id
                                                              select new detialTrainingRequester() {
                                                                  job_grade_name = tjg.grade_name,
                                                                  job_title_name = tjt.title_name,
                                                                  request_by = tb2.name_employee
                                                              }).FirstOrDefault(),
                                                              training_type = tb3.training_type,
                                                              training_category = tb3.category,
                                                              traning_name = tb3.name,
                                                              status = tb1.status,
                                                              purpose_start_date = tb1.purpose_start_date,
                                                              purpose_end_date = tb1.purpose_end_date,
                                                              venue = tb1.venue,
                                                              notes = tb1.notes,
                                                              add_recomendation = tb1.add_recomendation,
                                                              statusReal = tau1.StatusApproved,
                                                              ListParticipantTraining = (
                                                              from tbk in _repo.GetContext().Set<TrainingRequisitionDetial>()
                                                              join tbk2 in _repo.GetContext().Set<EmployeeBasicInfo>() on tbk.employee_basic_info_id equals tbk2.Id
                                                              join tbbk in _repo.GetContext().Set<BusinessUnitJobLevel>() on tbk2.business_unit_job_level_id equals tbbk.Id
                                                              join tbk3 in _repo.GetContext().Set<JobGrade>() on tbbk.job_grade_id equals tbk3.Id
                                                              join tbk4 in _repo.GetContext().Set<JobTitle>() on tbbk.job_title_id equals tbk4.Id
                                                              where tbk.training_requition_id == tb1.Id 
                                                              select new participantTraining()
                                                              {
                                                                  Id = tbk.Id,
                                                                  employee_basic_info_id = tbk.employee_basic_info_id,
                                                                  job_grade_name = tbk3.grade_name,
                                                                  job_title_name = tbk4.title_name,
                                                                  participantBusinessUnit = (
                                                                  from tbg in _repo.GetContext().Set<BusinessUnitJobLevel>()
                                                                  join tbg1 in _repo.GetContext().Set<BusinessUnit>() on tbg.business_unit_division_id equals tbg1.Id
                                                                  where tbg.Id == tbk2.business_unit_job_level_id
                                                                  select new participantBusinessUnit() {
                                                                      Id = tbg.Id,
                                                                      name = tbg1.unit_name
                                                                  }).FirstOrDefault()
                                  
                                                              }).ToList(),
                                                              created_on = tb1.created_on,
                                                              training_id = tb3.Id,
                                                              employee_basic_info_id = tb1.Id,
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
                
                resp.RecordsTotal = que.Count();
                que = q.filterGreaterEquals(que, "training_cost",filter.Search_training_cost);
                que = q.filterEquals(que, "status", filter.Search_status);
                que = q.filterEquals(que, "purpose_end_date", filter.Search_purpose_end_date);
                que = q.filterContains(que, "venue", filter.Search_venue);
                que = q.filterEquals(que, "purpose_start_date", filter.Search_purpose_start_date);
                que = q.filterContains(que, "training_name", filter.Search_trainingname);
                que = q.filterContains(que, "training_type", filter.Search_training_type);
                que = q.filterContains(que, "training_category", filter.Search_training_category);



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

        public GeneralResponseList<TrainingBondingDto> GetTrainingBonding(TrainingRequisitionSearch filter)
        {
            GeneralResponseList<TrainingBondingDto> resp = new GeneralResponseList<TrainingBondingDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<TrainingBondingDto> que = from tb1 in _repo.GetContext().Set<TrainingBonding>()
                                                     join tb4 in _repo.GetContext().Set<TrainingRequisition>() on tb1.training_req_id equals tb4.Id
                                                     join tb3 in _repo.GetContext().Set<Training>() on tb4.training_id equals tb3.Id
                                                     join tb2 in _repo.GetContext().Set<EmployeeBasicInfo>() on tb1.employee_basic_info_id equals tb2.Id
                                                     join tbtap in _repo.GetContext().Set<TrTemplateApproval>() on tb1.Id equals tbtap.RefId
                                                     into tbtapau1
                                                     from tau1 in tbtapau1.DefaultIfEmpty()
                                                         select new TrainingBondingDto()
                                                         {
                                                             Id = tb1.Id,
                                                             code = tb4.code,
                                                             training_cost = tb3.training_cost,
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
                                                             training_type = tb3.training_type,
                                                             training_category = tb3.category,
                                                             traning_name = tb3.name,
                                                             venue = tb4.venue,
                                                             status = tb4.status,
                                                             start_date = tb1.start_date,
                                                             end_date = tb1.end_date,
                                                             statusReal = tau1.StatusApproved,
                                                             ListParticipantTraining = (
                                                              from tbk in _repo.GetContext().Set<TrainingRequisitionDetial>()
                                                              join tbk2 in _repo.GetContext().Set<EmployeeBasicInfo>() on tbk.employee_basic_info_id equals tbk2.Id
                                                              join tbbk in _repo.GetContext().Set<BusinessUnitJobLevel>() on tbk2.business_unit_job_level_id equals tbbk.Id
                                                              join tbk3 in _repo.GetContext().Set<JobGrade>() on tbbk.job_grade_id equals tbk3.Id
                                                              join tbk4 in _repo.GetContext().Set<JobTitle>() on tbbk.job_title_id equals tbk4.Id
                                                              where tbk.training_requition_id == tb4.Id
                                                              select new participantTraining()
                                                              {
                                                                  Id = tbk.Id,
                                                                  employee_basic_info_id = tbk.employee_basic_info_id,
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
                                                             employee_basic_info_id = tb1.Id,
                                                             ApprovalReal = (from tap in _repo.GetContext().Set<TrTemplateApproval>()
                                                                             join tapd in _repo.GetContext().Set<TrUserApproval>() on tap.Id equals tapd.TrTempApprovalId
                                                                             join tapde in _repo.GetContext().Set<EmployeeBasicInfo>() on tapd.EmployeeBasicInfoId equals tapde.Id
                                                                             where tap.RefId == tb4.Id
                                                                             select new TrUserApprovalDto()
                                                                             {
                                                                                 approved_by = tapde.name_employee,
                                                                                 created_on = tapd.CreatedOn,
                                                                                 StatusApproved = tapd.StatusApproved
                                                                             }).ToList(),

                                                         };

                resp.RecordsTotal = que.Count();
                que = q.filterGreaterEquals(que, "training_cost", filter.Search_training_cost);
                que = q.filterEquals(que, "status", filter.Search_status);
                que = q.filterEquals(que, "purpose_end_date", filter.Search_purpose_end_date);
                que = q.filterContains(que, "venue", filter.Search_venue);
                que = q.filterEquals(que, "purpose_start_date", filter.Search_purpose_start_date);
                que = q.filterContains(que, "training_name", filter.Search_trainingname);
                que = q.filterContains(que, "training_type", filter.Search_training_type);
                que = q.filterContains(que, "training_category", filter.Search_training_category);



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



        public GeneralResponse<TrainingRequisition> UpdateDetial(TrainingRequisitionDto dto)
        {
            GeneralResponse<TrainingRequisition> resp = new GeneralResponse<TrainingRequisition>();
            try
            {
                TrainingRequisition NewTrainingRequisition = new TrainingRequisition();
                NewTrainingRequisition =  _repo.FindByID(dto.Id);
                
                IQueryable<TrainingRequisitionDetial> que =  from tb1 in _repo.GetContext().Set<TrainingRequisitionDetial>()
                                                             where tb1.training_requition_id == dto.Id
                                                             select new TrainingRequisitionDetial()
                                                             {
                                                                 Id = tb1.Id,
                                                                 employee_basic_info_id = tb1.employee_basic_info_id
                                                             };
                List<TrainingRequisitionDetial> listTrainingRequisitionDetial = que.ToList();
                foreach (TrainingRequisitionDetial data in listTrainingRequisitionDetial)
                {
                    _repoTrainingReqDetail.RemoveByID(false,data.Id);
                }

                foreach (participantTraining data in dto.ListParticipantTraining)
                {
                    TrainingRequisitionDetial newTrainingRequisitionDetial = new TrainingRequisitionDetial();
                    newTrainingRequisitionDetial.employee_basic_info_id = data.employee_basic_info_id;
                    newTrainingRequisitionDetial.training_requition_id = NewTrainingRequisition.Id;
                    _repoTrainingReqDetail.Create(false, newTrainingRequisitionDetial);
                }

               
                System.Reflection.PropertyInfo pi = NewTrainingRequisition.GetType().GetProperty("updated_by");
                if (pi != null)
                {
                    pi.SetValue(NewTrainingRequisition, Glosing.Instance.Username);
                }
                System.Reflection.PropertyInfo pd = NewTrainingRequisition.GetType().GetProperty("updated_date");
                if (pd != null)
                {
                    pd.SetValue(NewTrainingRequisition, DateTime.Now);
                }
                NewTrainingRequisition.venue = dto.venue;
                NewTrainingRequisition.purpose_end_date = dto.purpose_end_date;
                NewTrainingRequisition.purpose_start_date = dto.purpose_start_date;
                _repo.Update(false,NewTrainingRequisition);

                _repo.SaveChanges();
                _repoTrainingReqDetail.SaveChanges();
                _repo.Dispose();
                _repoTrainingReqDetail.Dispose();
                resp.Code = "00";
                resp.Message = "sucess";
                resp.Success = true;

            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }

        public GeneralResponse<TrainingRequisition> UpdateStatus(TrainingRequisitionDto dto)
        {
            GeneralResponse<TrainingRequisition> resp = new GeneralResponse<TrainingRequisition>();
            try
            {
                TrainingRequisition NewTrainingRequisition = new TrainingRequisition();
                NewTrainingRequisition = _repo.FindByID(dto.Id);
                
                System.Reflection.PropertyInfo pi = NewTrainingRequisition.GetType().GetProperty("updated_by");
                if (pi != null)
                {
                    pi.SetValue(NewTrainingRequisition, Glosing.Instance.EmployeeId);
                }
                System.Reflection.PropertyInfo pd = NewTrainingRequisition.GetType().GetProperty("updated_date");
                if (pd != null)
                {
                    pd.SetValue(NewTrainingRequisition, DateTime.Now);
                }
                if (dto.status != null) { 
                    NewTrainingRequisition.status = dto.status;
                }
                if (dto.add_recomendation != null) {
                    NewTrainingRequisition.add_recomendation = dto.add_recomendation;
                }
                _repo.SaveChanges();
                _repoTrainingReqDetail.SaveChanges();
                resp.Code = "00";
                resp.Message = "success";
                resp.Success = true;

            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }

        public GeneralResponse<TrainingRequisition> ProcessPaymentandAdministration(TrainingRequisitionDto dto)
        {
            GeneralResponse<TrainingRequisition> resp = new GeneralResponse<TrainingRequisition>();
            try
            {
                //update training requisition data
                TrainingRequisition NewTrainingRequisition = new TrainingRequisition();
                NewTrainingRequisition = _repo.FindByID(dto.Id);

                System.Reflection.PropertyInfo pi = NewTrainingRequisition.GetType().GetProperty("updated_by");
                if (pi != null)
                {
                    pi.SetValue(NewTrainingRequisition, Glosing.Instance.EmployeeId);
                }
                System.Reflection.PropertyInfo pd = NewTrainingRequisition.GetType().GetProperty("updated_date");
                if (pd != null)
                {
                    pd.SetValue(NewTrainingRequisition, DateTime.Now);
                }

                if (dto.status != null)
                {
                    NewTrainingRequisition.status = dto.status;
                }

                if (dto.add_recomendation != null)
                {
                    NewTrainingRequisition.add_recomendation = dto.add_recomendation;
                }
                _repo.Update(false, NewTrainingRequisition);

                //insert into AP (TBA)

                //insert into Cash Advance (TBA)

                //insert into Procurment PR (tiket and accomodation)


                //insert into traning bonding if training cost> 15 milion
                IQueryable<TrainingDto> que = from tb1 in _repo.GetContext().Set<Training>()
                                                            where tb1.Id == NewTrainingRequisition.training_id
                                                            select new TrainingDto()
                                                            {
                                                                Id = tb1.Id,
                                                                category = tb1.category,
                                                                name = tb1.name,
                                                                subject = tb1.subject,
                                                                description = tb1.description,
                                                                target_audiance = tb1.target_audiance,
                                                                bonding_detail = tb1.bonding_detail,
                                                                recruitment = tb1.recruitment,
                                                                training_cost = tb1.training_cost,
                                                                created_on = tb1.created_on
                                                            };
                IQueryable<TrainingRequisitionDetial> que2 = from tb1 in _repo.GetContext().Set<TrainingRequisitionDetial>()
                                              where tb1.training_requition_id == NewTrainingRequisition.Id
                                              select new TrainingRequisitionDetial()
                                              {
                                                  Id = tb1.Id,
                                                  employee_basic_info_id = tb1.employee_basic_info_id,
                                                  created_on = tb1.created_on
                                              };
                TrainingDto _trainingData = que.FirstOrDefault();
                List<TrainingRequisitionDetial> _trainingReqDetialData = que2.ToList();

                if (_trainingData.training_cost >= 15000000) {

                    foreach (TrainingRequisitionDetial detil in _trainingReqDetialData) {
                        TrainingBonding newTrainingBonding = new TrainingBonding();
                        newTrainingBonding.employee_basic_info_id = detil.employee_basic_info_id;
                        newTrainingBonding.training_req_id = NewTrainingRequisition.Id;
                        _repoTrainingBonding.Create(false, newTrainingBonding);
                    }

                }
               
                _repo.SaveChanges();
                if (_trainingData.training_cost >= 15000000)
                {
                    _repoTrainingBonding.SaveChanges();
                    
                }

                _repo.Dispose();
                if (_trainingData.training_cost >= 15000000)
                {
                    _repoTrainingBonding.Dispose();
                }
                resp.Code = "00";
                resp.Message = "success";
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

