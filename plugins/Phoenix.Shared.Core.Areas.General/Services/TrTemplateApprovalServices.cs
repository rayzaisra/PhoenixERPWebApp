using Phoenix.Shared.Core.Areas.General.Dtos;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Parameters;
using Phoenix.Shared.Core.Repositories;
using Phoenix.Shared.Core.Services;
using Phoenix.Shared.Responses;
using System;
using System.Linq;


namespace Phoenix.Shared.Core.Areas.General.Services
{
    public partial interface ITrTemplateApprovalServices : IBaseService<TrTemplateApprovalDto, TrTemplateApproval, string>
    {
        GeneralResponseList<TrTemplateApprovalDto> ListAll(SearchTrTemplateApproval filter);
        GeneralResponse<TrTemplateApprovalDto> GetByRefId(string RefId);
    }
    public partial class TrTemplateApprovalServices : BaseService<TrTemplateApprovalDto, TrTemplateApproval, string>, ITrTemplateApprovalServices
    {
        public TrTemplateApprovalServices(IEFRepository<TrTemplateApproval, string> repo) : base(repo)
        {
            _repo = repo;
        }

        public GeneralResponse<TrTemplateApprovalDto> GetByRefId(string RefId)
        {
            GeneralResponse<TrTemplateApprovalDto> resp = new GeneralResponse<TrTemplateApprovalDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<TrTemplateApprovalDto> que = from tb1 in _repo.GetContext().Set<TrTemplateApproval>()
                                                        join tb2 in _repo.GetContext().Set<TrUserApproval>() on tb1.Id equals tb2.TrTempApprovalId
                                                        where tb1.RefId == RefId
                                                        select new TrTemplateApprovalDto()
                                                        {
                                                            Id = tb1.Id,
                                                            RefId = tb1.RefId,
                                                            ApprovalCategory = tb1.ApprovalCategory,
                                                            ApprovalType = tb1.ApprovalType,
                                                            IsActive = tb1.IsActive,
                                                            IsDeleted = tb1.IsDeleted,
                                                            FormReqName = tb1.FormReqName,
                                                            DueDate = tb1.DueDate,
                                                            DivisionId = tb1.DivisionId,
                                                            DetailLink = tb1.DetailLink,
                                                            CreatedOn = tb1.CreatedOn,
                                                            CreatedBy = tb1.CreatedBy,
                                                            IsNotifyByEmail = tb1.IsNotifyByEmail,
                                                            IsNotifyByWeb = tb1.IsNotifyByWeb,
                                                            ModifiedBy = tb1.ModifiedBy,
                                                            ModifiedOn = tb1.ModifiedOn,
                                                            RejectedBy = tb1.RejectedBy,
                                                            RejectedOn = tb1.RejectedOn,
                                                            RemarkRejected = tb1.RemarkRejected,
                                                            Reminder = tb1.Reminder,
                                                            StatusApproved = tb1.StatusApproved,
                                                            SubGroupId = tb1.SubGroupId,
                                                            TemplateName = tb1.TemplateName,
                                                            listUserApproval = (from tb3 in _repo.GetContext().Set<TrUserApproval>()
                                                                                join tb4 in _repo.GetContext().Set<EmployeeBasicInfo>() on tb3.EmployeeBasicInfoId equals tb4.Id
                                                                                join tb5 in _repo.GetContext().Set<BusinessUnitJobLevel>() on tb4.business_unit_job_level_id equals tb5.Id
                                                                                into tb5k
                                                                                from tbk5 in tb5k.DefaultIfEmpty()
                                                                                join tb6 in _repo.GetContext().Set<JobTitle>() on tbk5.job_title_id equals tb6.Id
                                                                                into tb6k
                                                                                from tbk6 in tb6k.DefaultIfEmpty()
                                                                                where tb3.TrTempApprovalId == tb1.Id
                                                                                select new TrUserApprovalDto()
                                                                                {
                                                                                    Id = tb3.Id,
                                                                                    CreatedBy = tb3.CreatedBy,
                                                                                    CreatedOn = tb3.CreatedOn,
                                                                                    EmployeeBasicInfoId = tb3.EmployeeBasicInfoId,
                                                                                    IndexUser = tb3.IndexUser,
                                                                                    IsCondition = tb3.IsCondition,
                                                                                    StatusApproved = tb3.StatusApproved,
                                                                                    TrTempApprovalId = tb3.TrTempApprovalId,
                                                                                    job_title_id = tbk6.Id,
                                                                                    job_title_name = tbk6.title_name,
                                                                                    employee_full_name = tb4.name_employee,
                                                                                    IsSpecialCase = tb3.IsSpecialCase,
                                                                                    RemarkSpecialCase = tb3.RemarkSpecialCase

                                                                                }).ToList()
                                                        };

                resp.Data = que.FirstOrDefault();
                resp.Success = true;
                resp.Message = "sucesss";
                resp.Code = "00";
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;

        }

        public GeneralResponseList<TrTemplateApprovalDto> ListAll(SearchTrTemplateApproval filter)
        {

            GeneralResponseList<TrTemplateApprovalDto> resp = new GeneralResponseList<TrTemplateApprovalDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<TrTemplateApprovalDto> que = from tb1 in _repo.GetContext().Set<TrTemplateApproval>()
                                                        join tb2 in _repo.GetContext().Set<TrUserApproval>() on tb1.Id equals tb2.TrTempApprovalId
                                                        where !(tb1.IsDeleted ?? false)
                                                        select new TrTemplateApprovalDto()
                                                        {
                                                            Id = tb1.Id,
                                                            RefId = tb1.RefId,
                                                            ApprovalCategory = tb1.ApprovalCategory,
                                                            ApprovalType = tb1.ApprovalType,
                                                            IsActive = tb1.IsActive,
                                                            IsDeleted = tb1.IsDeleted,
                                                            FormReqName = tb1.FormReqName,
                                                            DueDate = tb1.DueDate,
                                                            DivisionId = tb1.DivisionId,
                                                            DetailLink = tb1.DetailLink,
                                                            CreatedOn = tb1.CreatedOn,
                                                            CreatedBy = tb1.CreatedBy,
                                                            IsNotifyByEmail = tb1.IsNotifyByEmail,
                                                            IsNotifyByWeb = tb1.IsNotifyByWeb,
                                                            ModifiedBy = tb1.ModifiedBy,
                                                            ModifiedOn = tb1.ModifiedOn,
                                                            RejectedBy = tb1.RejectedBy,
                                                            RejectedOn = tb1.RejectedOn,
                                                            RemarkRejected = tb1.RemarkRejected,
                                                            Reminder = tb1.Reminder,
                                                            StatusApproved = tb1.StatusApproved,
                                                            SubGroupId = tb1.SubGroupId,
                                                            TemplateName = tb1.TemplateName,
                                                            listUserApproval = (from tb3 in _repo.GetContext().Set<TrUserApproval>()
                                                                                join tb4 in _repo.GetContext().Set<EmployeeBasicInfo>() on tb3.EmployeeBasicInfoId equals tb4.Id
                                                                                join tb5 in _repo.GetContext().Set<BusinessUnitJobLevel>() on tb4.business_unit_job_level_id equals tb5.Id
                                                                                  into tb5k
                                                                                from tbk5 in tb5k.DefaultIfEmpty()
                                                                                join tb6 in _repo.GetContext().Set<JobTitle>() on tbk5.job_title_id equals tb6.Id
                                                                                into tb6k
                                                                                from tbk6 in tb6k.DefaultIfEmpty()
                                                                                where tb1.Id == tb3.TrTempApprovalId
                                                                                select new TrUserApprovalDto()
                                                                                {
                                                                                    Id = tb3.Id,
                                                                                    CreatedBy = tb3.CreatedBy,
                                                                                    CreatedOn = tb3.CreatedOn,
                                                                                    EmployeeBasicInfoId = tb3.EmployeeBasicInfoId,
                                                                                    IndexUser = tb3.IndexUser,
                                                                                    IsCondition = tb3.IsCondition,
                                                                                    StatusApproved = tb3.StatusApproved,
                                                                                    TrTempApprovalId = tb3.TrTempApprovalId,
                                                                                    job_title_id = tbk6.Id,
                                                                                    job_title_name = tbk6.title_name,
                                                                                    employee_full_name = tb4.name_employee,
                                                                                    IsSpecialCase = tb3.IsSpecialCase,
                                                                                    RemarkSpecialCase = tb3.RemarkSpecialCase

                                                                                }).ToList()
                                                        };

                resp.RecordsTotal = que.Count();

                que = q.filterEquals(que, "RefId", filter.Search_RefId);


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

