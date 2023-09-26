using Phoenix.Shared.Core.Areas.ProjectManagement.Dtos;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Parameters;
using Phoenix.Shared.Core.Repositories;
using Phoenix.Shared.Core.Services;
using Phoenix.Shared.Responses;
using System;
using System.Linq;


namespace Phoenix.Shared.Core.Areas.ProjectManagement.Services
{
    public partial interface IClientFeedbackServices : IBaseService<ClientFeedbackDto, ClientFeedback, string>
    {
        GeneralResponseList<ClientFeedbackDto> ListAll(ClientFeedbackSearch filter);

        #region AddonService
        GeneralResponseList<ApprovalTypeDto> GetApprovalType();
        GeneralResponse<ClientFeedbackDetailDto> GetDetailClientFeedBack(ClientFeedbackParameterDto parameter);
        GeneralResponse<ClientFeedbackResponseDto> SubmitClientFeedback(ClientFeedbackParameterDto parameter);
        #endregion
    }
    public partial class ClientFeedbackServices : BaseService<ClientFeedbackDto, ClientFeedback, string>, IClientFeedbackServices
    {
        string userid = "";

        IEFRepository<ClientBriefAccount, string> _repoCientBriefAccount;

        public ClientFeedbackServices(IEFRepository<ClientFeedback, string> repo, IEFRepository<ClientBriefAccount, string> repoCientBriefAccount) : base(repo)
        {
            _repo = repo;
            userid = Helpers.Glosing.Instance.Username;

            _repoCientBriefAccount = repoCientBriefAccount;
        }

        public GeneralResponseList<ClientFeedbackDto> ListAll(ClientFeedbackSearch filter)
        {

            GeneralResponseList<ClientFeedbackDto> resp = new GeneralResponseList<ClientFeedbackDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<ClientFeedbackDto> que = from tb1 in _repo.GetContext().Set<ClientFeedback>()
                                                    select new ClientFeedbackDto()
                                                    {
                                                        Id = tb1.Id,
                                                        approval_type_id = tb1.approval_type_id,
                                                        client_feedback_file_id = tb1.client_feedback_file_id,
                                                        job_posting_requisition_id = tb1.job_posting_requisition_id,
                                                    };

                resp.RecordsTotal = que.Count();

                que = q.filterEquals(que, "approval_type_id", filter.Search_approval_type_id);
                que = q.filterEquals(que, "client_feedback_file_id", filter.Search_client_feedback_file_id);
                que = q.filterEquals(que, "job_posting_requisition_id", filter.Search_job_posting_requisition_id);



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

        #region AddonService
        public GeneralResponseList<ApprovalTypeDto> GetApprovalType()
        {
            GeneralResponseList<ApprovalTypeDto> resp = new GeneralResponseList<ApprovalTypeDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<ApprovalTypeDto> que = from tb1 in _repo.GetContext().Set<ApprovalType>()
                                                  select new ApprovalTypeDto()
                                                  {
                                                      Id = tb1.Id,
                                                      approval_type = tb1.approval_type
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

        public GeneralResponse<ClientFeedbackDetailDto> GetDetailClientFeedBack(ClientFeedbackParameterDto parameter)
        {
            GeneralResponse<ClientFeedbackDetailDto> resp = new GeneralResponse<ClientFeedbackDetailDto>();
            try
            {
                var query = from tb1 in _repo.GetContext().Set<JobDetail>()
                            join tb2 in _repo.GetContext().Set<ClientBrief>() on tb1.client_brief_id equals tb2.Id
                            join tb4 in _repo.GetContext().Set<CompanyBrand>() on tb2.external_brand_id equals tb4.Id
                            join tb5 in _repo.GetContext().Set<Company>() on tb4.company_id equals tb5.Id
                            join tb6 in _repo.GetContext().Set<ClientFeedback>() on tb1.job_number equals tb6.job_number
                            join tb7 in _repo.GetContext().Set<ApprovalType>() on tb6.approval_type_id equals tb7.Id
                            select new ClientFeedbackDetailDto()
                            {
                                ApprovalStatusId = tb6.approval_type_id,
                                Id = tb6.Id,
                                CampaignName = tb2.campaign_name,
                                ClientFeedbackFileId = tb6.client_feedback_file_id,
                                JobName = tb1.job_name,
                                JobNumber = tb1.job_number
                            };

                if (parameter.JobName != null || parameter.JobNumber == null)
                {
                    query = (from tb10 in query
                             where tb10.JobName.ToLower().Contains(parameter.JobName.ToLower())
                             select tb10);
                }
                else if (parameter.JobName == null || parameter.JobNumber != null)
                {
                    query = (from tb10 in query
                             where tb10.JobNumber.ToLower().Contains(parameter.JobNumber.ToLower())
                             select tb10);
                }

                resp.Data = query.FirstOrDefault();
                resp.Success = true;
                resp.Message = "Sukses";
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }

        public GeneralResponse<ClientFeedbackResponseDto> SubmitClientFeedback(ClientFeedbackParameterDto parameter)
        {
            GeneralResponse<ClientFeedbackResponseDto> resp = new GeneralResponse<ClientFeedbackResponseDto>();

            try
            {
                var DataClientFeedback = new ClientFeedback()
                {
                    client_feedback_file_id = parameter.FileNameId,
                    job_number = parameter.JobNumber,
                    approval_type_id = parameter.ApprovalTypeId,
                    job_posting_requisition_id = parameter.JobNumber
                };

                _repo.Create(DataClientFeedback);

                var getClientBriefId = _repo.GetContext().Set<JobDetail>().Where(x => x.job_number == parameter.JobNumber).Select(x => x.client_brief_id).FirstOrDefault();
                var ApprovalStatus = _repo.GetContext().Set<ClientBriefAccount>().Where(x => x.client_brief_id == getClientBriefId).FirstOrDefault();

                if (ApprovalStatus == null)
                {
                    var DataApprovalStatus = new ClientBriefAccount()
                    {
                        client_brief_id = getClientBriefId,
                        approval_status_id = parameter.ApprovalTypeId,
                        account_management_id = userid
                    };
                    _repoCientBriefAccount.Create(DataApprovalStatus);
                }
                else
                {
                    ApprovalStatus.approval_status_id = parameter.ApprovalTypeId;
                }

                _repo.SaveChanges();
                _repoCientBriefAccount.SaveChanges();

                var response = new ClientFeedbackResponseDto()
                {
                    ApprovalStatusId = parameter.ApprovalTypeId,
                    FileName = parameter.FileNameId,
                    JobName = parameter.JobName,
                    JobNumber = parameter.JobNumber,
                    UpdateDataResult = true
                };

                resp.Data = response;
                resp.Success = true;
                resp.Message = "Sukses";
            }
            catch (Exception ex)
            {
                var response = new ClientFeedbackResponseDto()
                {
                    ApprovalStatusId = parameter.ApprovalTypeId,
                    FileName = parameter.FileNameId,
                    JobName = parameter.JobName,
                    JobNumber = parameter.JobNumber,
                    UpdateDataResult = false
                };
                resp.Data = response;
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }
        #endregion

    }
}

