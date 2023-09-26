using Phoenix.Data;
using Phoenix.Shared.Core.Areas.General.Dtos;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Parameters;
using Phoenix.Shared.Core.Repositories;
using Phoenix.Shared.Core.Services;
using Phoenix.Shared.Helpers;
using Phoenix.Shared.Responses;
using System;
using System.Linq;


namespace Phoenix.Shared.Core.Areas.General.Services
{
    public partial interface ITmTemplateApprovalServices : IBaseService<TmTemplateApprovalDto, TmTemplateApproval, string>
    {
        GeneralResponseList<TmTemplateApprovalDto> ListAll(SearchTmTemplateApproval filter);
    }
    public partial class TmTemplateApprovalServices : BaseService<TmTemplateApprovalDto, TmTemplateApproval, string>, ITmTemplateApprovalServices
    {
        public TmTemplateApprovalServices(IEFRepository<TmTemplateApproval, string> repo) : base(repo)
        {
            _repo = repo;
        }

        public GeneralResponseList<TmTemplateApprovalDto> ListAll(SearchTmTemplateApproval filter)
        {

            GeneralResponseList<TmTemplateApprovalDto> resp = new GeneralResponseList<TmTemplateApprovalDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<TmTemplateApprovalDto> que = null;
                if (!string.IsNullOrEmpty(filter.Search_Budgeted) || !string.IsNullOrEmpty(filter.Search_Unbudgeted) || !string.IsNullOrEmpty(filter.Search_Replacement))
                {
                    que = from tb1 in _repo.GetContext().Set<TmTemplateApproval>()
                          join tb2 in _repo.GetContext().Set<TmMenu>() on tb1.MenuId equals tb2.Id
                          //join tb3 in _repo.GetContext().Set<BusinessUnit>() on tb1.DivisionId equals tb3.Id
                          join tb3 in _repo.GetContext().Set<BusinessUnit>() on tb1.ForDivisionId equals tb3.Id into g
                          from result in g.DefaultIfEmpty()
                          where !(tb1.IsDeleted ?? false) && tb1.TemplateName.IndexOf(filter.Search_Budgeted, StringComparison.OrdinalIgnoreCase) >= 0
                          select new TmTemplateApprovalDto()
                          {
                              Id = tb1.Id,
                              TemplateName = tb1.TemplateName,
                              IsNotifyByEmail = tb1.IsNotifyByEmail,
                              IsNotifyByWeb = tb1.IsNotifyByWeb,
                              DueDate = tb1.DueDate,
                              Reminder = tb1.Reminder,
                              DivisionId = tb1.DivisionId,
                              SubGroupId = tb1.SubGroupId,
                              JobTitleId = tb1.JobTitleId,
                              MinBudget = tb1.MinBudget,
                              MaxBudget = tb1.MaxBudget,
                              MenuId = tb1.MenuId,
                              MenuName = tb2.MenuName,
                              ApprovalCategory = tb1.ApprovalCategory,
                              ApprovalType = tb1.ApprovalType,
                              CreatedBy = tb1.CreatedBy,
                              CreatedOn = tb1.CreatedOn,
                              IsActive = tb1.IsActive,
                              BusinessUnitName = result.unit_name,
                              IsDeleted = tb1.IsDeleted,
                              ModifiedBy = tb1.ModifiedBy,
                              ModifiedOn = tb1.ModifiedOn,
                              IsDivisionAll = tb1.IsAllDivision,
                              IsSpecialCase = tb1.IsSpecialCase,
                              listTmUserApproval = (from tb3 in _repo.GetContext().Set<TmUserApproval>()
                                                    join tb4 in _repo.GetContext().Set<EmployeeBasicInfo>() on tb3.EmployeeBasicInfoId equals tb4.Id
                                                    join tb5 in _repo.GetContext().Set<BusinessUnitJobLevel>() on tb4.business_unit_job_level_id equals tb5.Id
                                                    into tb5k
                                                    from tbk5 in tb5k.DefaultIfEmpty()
                                                    join tb6 in _repo.GetContext().Set<JobTitle>() on tbk5.job_title_id equals tb6.Id
                                                    into tb6k
                                                    from tbk6 in tb6k.DefaultIfEmpty()
                                                    where tb3.TmTempApprovalId == tb1.Id && tb3.IsDeleted == false
                                                    select new TmUserApprovalDto()
                                                    {
                                                        Id = tb3.Id,
                                                        EmployeeBasicInfoId = tb3.EmployeeBasicInfoId,
                                                        IsCondition = tb3.IsCondition,
                                                        TmTempApprovalId = tb3.TmTempApprovalId,
                                                        IndexUser = tb3.IndexUser,
                                                        CreatedBy = tb3.CreatedBy,
                                                        CreatedOn = tb3.CreatedOn,
                                                        IsActive = tb3.IsActive,
                                                        IsDeleted = tb3.IsDeleted,
                                                        ModifiedBy = tb3.ModifiedBy,
                                                        ModifiedOn = tb3.ModifiedOn,
                                                        job_title_id = tbk6.Id,
                                                        job_title_name = tbk6.title_name,
                                                        employee_full_name = tb4.name_employee

                                                    }).OrderBy(x => x.IndexUser).ToList()
                          };
                }
                else
                {

                    que = from tb1 in _repo.GetContext().Set<TmTemplateApproval>()
                          join tb2 in _repo.GetContext().Set<TmMenu>() on tb1.MenuId equals tb2.Id
                          //join tb3 in _repo.GetContext().Set<BusinessUnit>() on tb1.DivisionId equals tb3.Id
                          join tb3 in _repo.GetContext().Set<BusinessUnit>() on tb1.ForDivisionId equals tb3.Id into g
                          from result in g.DefaultIfEmpty()
                          where tb1.IsDeleted == false
                          select new TmTemplateApprovalDto()
                          {
                              Id = tb1.Id,
                              TemplateName = tb1.TemplateName,
                              IsNotifyByEmail = tb1.IsNotifyByEmail,
                              IsNotifyByWeb = tb1.IsNotifyByWeb,
                              DueDate = tb1.DueDate,
                              Reminder = tb1.Reminder,
                              DivisionId = tb1.DivisionId,
                              SubGroupId = tb1.SubGroupId,
                              JobTitleId = tb1.JobTitleId,
                              MinBudget = tb1.MinBudget,
                              MaxBudget = tb1.MaxBudget,
                              MenuId = tb1.MenuId,
                              MenuName = tb2.MenuName,
                              ApprovalCategory = tb1.ApprovalCategory,
                              ApprovalType = tb1.ApprovalType,
                              CreatedBy = tb1.CreatedBy,
                              CreatedOn = tb1.CreatedOn,
                              IsActive = tb1.IsActive,
                              BusinessUnitName = result.unit_name,
                              IsDeleted = tb1.IsDeleted,
                              ModifiedBy = tb1.ModifiedBy,
                              ModifiedOn = tb1.ModifiedOn,
                              IsDivisionAll = tb1.IsAllDivision,
                              IsSpecialCase = tb1.IsSpecialCase,
                              listTmUserApproval = (from tb3 in _repo.GetContext().Set<TmUserApproval>()
                                                    join tb4 in _repo.GetContext().Set<EmployeeBasicInfo>() on tb3.EmployeeBasicInfoId equals tb4.Id
                                                    join tb5 in _repo.GetContext().Set<BusinessUnitJobLevel>() on tb4.business_unit_job_level_id equals tb5.Id
                                                    into tb5k
                                                    from tbk5 in tb5k.DefaultIfEmpty()
                                                    join tb6 in _repo.GetContext().Set<JobTitle>() on tbk5.job_title_id equals tb6.Id
                                                    into tb6k
                                                    from tbk6 in tb6k.DefaultIfEmpty()
                                                    where tb3.TmTempApprovalId == tb1.Id && tb3.IsDeleted == false
                                                    select new TmUserApprovalDto()
                                                    {
                                                        Id = tb3.Id,
                                                        EmployeeBasicInfoId = tb3.EmployeeBasicInfoId,
                                                        IsCondition = tb3.IsCondition,
                                                        TmTempApprovalId = tb3.TmTempApprovalId,
                                                        IndexUser = tb3.IndexUser,
                                                        CreatedBy = tb3.CreatedBy,
                                                        CreatedOn = tb3.CreatedOn,
                                                        IsActive = tb3.IsActive,
                                                        IsDeleted = tb3.IsDeleted,
                                                        ModifiedBy = tb3.ModifiedBy,
                                                        ModifiedOn = tb3.ModifiedOn,
                                                        job_title_id = tbk6.Id,
                                                        job_title_name = tbk6.title_name,
                                                        employee_full_name = tb4.name_employee

                                                    }).OrderBy( x => x.IndexUser) .ToList()
                          };
                }



                resp.RecordsTotal = que.Count();
                if (!string.IsNullOrEmpty(filter.Search_MenuName))
                {
                    que = q.filterContains(que, "MenuName", filter.Search_MenuName);
                }
                if (!string.IsNullOrEmpty(filter.Search_TemplateName))
                {
                    que = q.filterEquals(que, "TemplateName", filter.Search_TemplateName.Trim());
                }
                if (!string.IsNullOrEmpty(filter.Search_BusinessUnitName))
                {

                    var tempQue = q.filterEquals(que, "BusinessUnitName", filter.Search_BusinessUnitName.Trim());
                    if (tempQue.Count() == 0)
                    {
                        que = que.Where(x => x.IsDivisionAll == true).OrderByDescending(x => x.CreatedOn);
                    }
                    else
                    {
                        que = tempQue;
                    }
                }
                if (!string.IsNullOrEmpty(filter.Search_BusinessUnitId))
                {
                    
                    var tempQue = q.filterEquals(que, "DivisionId", filter.Search_BusinessUnitId.Trim());
                    if (tempQue.Count() == 0)
                    {
                        que = que.Where(x => x.IsDivisionAll == true).OrderByDescending(x => x.CreatedOn);
                    }
                    else
                    {
                        que = tempQue;
                    }
                }

                resp.RecordsFiltered = que.Count();
                resp.Total = resp.RecordsTotal;

                que = q.sortir(que, "Id", filter.sortBy, filter.sortDir);
                que = q.limit(que, filter.GetLimit(), filter.GetOffset());

                resp.Rows = que.ToList();
                resp.Success = true;

                if (que.Where(x => x.IsSpecialCase == true).Count() > 0)
                {
                    var userSpesialCaseIsExist = true;
                    var employeeid = Glosing.Instance.EmployeeId;

                    var userSpecialCase = GetSpecialCaseUser(employeeid);
                    for (int i = 0; i < resp.Rows.Count(); i++)
                    {
                        if (resp.Rows[i].listTmUserApproval.Count() == 0)
                        {
                            userSpesialCaseIsExist = false;
                        }
                        else
                        {
                            for (int j = 0; j < resp.Rows[i].listTmUserApproval.Count(); j++)
                            {
                                if (resp.Rows[i].listTmUserApproval[j].EmployeeBasicInfoId != userSpecialCase.EmployeeId)
                                {
                                    userSpesialCaseIsExist = false;
                                }
                            }
                        }
                       
                    }
                    if (!userSpesialCaseIsExist)
                    {
                        string guid = System.Guid.NewGuid().ToString();
                        guid = guid.ToLower();
                        var query = (from tb4 in _repo.GetContext().Set<EmployeeBasicInfo>()
                                    join tb5 in _repo.GetContext().Set<BusinessUnitJobLevel>() on tb4.business_unit_job_level_id equals tb5.Id
                                    into tb5k
                                    from tbk5 in tb5k.DefaultIfEmpty()
                                    join tb6 in _repo.GetContext().Set<JobTitle>() on tbk5.job_title_id equals tb6.Id
                                    into tb6k
                                    from tbk6 in tb6k.DefaultIfEmpty()
                                    where tb4.Id == userSpecialCase.EmployeeId && tb4.is_deleted == false
                                     select new TmUserApprovalDto()
                                    {
                                        job_title_id = tbk6.Id,
                                        job_title_name = tbk6.title_name,
                                    }).FirstOrDefault();
                        var dataSpecialCase = new TmUserApprovalDto();
                        dataSpecialCase.Id = guid;
                        dataSpecialCase.IndexUser = 100;
                        dataSpecialCase.EmployeeBasicInfoId = userSpecialCase.EmployeeId;
                        dataSpecialCase.employee_full_name = userSpecialCase.EmployeeName;
                        dataSpecialCase.IsCondition = false;
                        dataSpecialCase.job_title_id = query.job_title_id;
                        dataSpecialCase.job_title_name = query.job_title_name;
                        resp.Rows[0].listTmUserApproval.Add(dataSpecialCase);
                    }
                    
                }

            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;

        }

        public VmUser GetSpecialCaseUser(string id)
        {
            VmUser query = new VmUser();
            var cekReportsTo = _repo.GetContext().Set<EmployeeBasicInfo>().Where(x => x.Id == id && x.is_deleted == false).Select(x => x.reports_to).FirstOrDefault();
            if (string.IsNullOrEmpty(cekReportsTo))
            {
                var defaultData = _repo.GetContext().Set<EmployeeBasicInfo>().Where(x => x.Id == "1225624597257392128" && x.is_deleted == false).Select(x => x.reports_to).FirstOrDefault();
                query = (from q in _repo.GetContext().Set<BusinessUnitJobLevel>().Where(x => x.is_deleted.Equals(false))
                         join r in _repo.GetContext().Set<EmployeeBasicInfo>().Where(x => x.is_deleted.Equals(false)) on q.parent_id equals r.business_unit_job_level_id
                         where r.Id == defaultData
                         select new VmUser()
                         {
                             EmployeeId = r.Id,
                             EmployeeName = r.name_employee
                         }
                              ).FirstOrDefault();
            }
            else
            {
                query = (from q in _repo.GetContext().Set<BusinessUnitJobLevel>().Where(x => x.is_deleted.Equals(false))
                         join r in _repo.GetContext().Set<EmployeeBasicInfo>().Where(x => x.is_deleted.Equals(false)) on q.Id equals r.business_unit_job_level_id
                         where r.business_unit_job_level_id == cekReportsTo
                         select new VmUser()
                         {
                             EmployeeId = r.Id,
                             EmployeeName = r.name_employee
                         }
                               ).FirstOrDefault();
            }

            return query;
        }

        public class VmUser
        {
            public string EmployeeId { get; set; }
            public string EmployeeName { get; set; }
        }

    }
}

