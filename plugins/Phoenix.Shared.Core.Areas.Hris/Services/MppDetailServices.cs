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
    public partial interface IMppDetailServices : IBaseService<MppDetailDto, MppDetail, string>
    {
        GeneralResponseList<MppDetailDto> ListAll(MppDetailSearch filter);
        GeneralResponse<MppDetailDto> GetWithName(string id);
        GeneralResponseList<FiscalYearDto> ListFiscalYear(MppDetailSearch filter, string userId);
    }
    public partial class MppDetailServices : BaseService<MppDetailDto, MppDetail, string>, IMppDetailServices
    {
        private IEFRepository<Mpp, string> _repoMpp;
        public MppDetailServices(IEFRepository<MppDetail, string> repo, IEFRepository<Mpp, string> repoMpp) : base(repo)
        {
            _repo = repo;
            _repoMpp = repoMpp;
        }

        public GeneralResponse<MppDetailDto> GetWithName(string id)
        {
            GeneralResponse<MppDetailDto> resp = new GeneralResponse<MppDetailDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<MppDetailDto> que = from tb1 in _repo.GetContext().Set<MppDetail>() join
                                               tb2 in _repo.GetContext().Set<JobGrade>() on tb1.job_grade_id equals tb2.Id
                                               join tb3 in _repo.GetContext().Set<JobTitle>() on tb1.job_tittle_id equals tb3.Id
                                               where tb1.Id == id
                                               select new MppDetailDto()
                                               {
                                                   Id = tb1.Id,
                                                   employment_status = tb1.employment_status,
                                                   job_tittle_id = tb1.job_tittle_id,
                                                   mpp_budged = tb1.mpp_budged,
                                                   mpp_id = tb1.mpp_id,
                                                   ttf = tb1.ttf,
                                                   job_grade_name = tb2.grade_name,
                                                   job_title_name = tb3.title_name,
                                                   Description = tb1.Description,
                                                   porject_name = tb1.project_name,
                                                   budget_souce = tb1.budget_souce,
                                                   status = tb1.status,
                                                   job_grade_id = tb1.job_grade_id
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

        public GeneralResponseList<MppDetailDto> ListAll(MppDetailSearch filter)
        {

            GeneralResponseList<MppDetailDto> resp = new GeneralResponseList<MppDetailDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<MppDetailDto> que = from tb1 in _repo.GetContext().Set<MppDetail>()
                          select new MppDetailDto()
                          {
                              Id = tb1.Id,
                              employment_status = tb1.employment_status,
                              job_tittle_id = tb1.job_tittle_id,
                              mpp_budged = tb1.mpp_budged,
                              mpp_id = tb1.mpp_id,
                              ttf = tb1.ttf,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "employment_status", filter.Search_employment_status);
                que = q.filterEquals(que, "job_tittle_id", filter.Search_job_tittle_id);
                que = q.filterEquals(que, "mpp_budged", filter.Search_mpp_budged);
                que = q.filterEquals(que, "mpp_id", filter.Search_mpp_id);
                que = q.filterContains(que, "ttf", filter.Search_ttf);

                

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

        public GeneralResponseList<FiscalYearDto> ListFiscalYear(MppDetailSearch filter, string userId)
        {

            GeneralResponseList<FiscalYearDto> resp = new GeneralResponseList<FiscalYearDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                var legalEntity = _repo.GetContext().Set<EmployeeBasicInfo>().Where(x => x.Id == userId && x.is_deleted == false).Select(x => x.legal_entity_id).FirstOrDefault();
                if (legalEntity == null)
                {
                    legalEntity = "37233a65-4dec-4888-989d-7bead019c991";
                }
                IQueryable<FiscalYearDto> que = from tb1 in _repo.GetContext().Set<FinancialYear>()
                                                where tb1.is_deleted == false && tb1.IsClose == false && tb1.LegalEntityId == legalEntity
                                                select new FiscalYearDto()
                                                {
                                                    Id = tb1.Id,
                                                    EndDate = tb1.EndDate,
                                                    FinancialYearName = tb1.FinancialYearName,
                                                    FinancialYearNumber = tb1.FinanceYearNumber,
                                                    IsClose = tb1.IsClose,
                                                    LegalEntityId = tb1.LegalEntityId,
                                                    StartDate = tb1.StartDate,
                                                    Status = tb1.Status
                                                };

                resp.RecordsTotal = que.Count();

                que = q.filterContains(que, "FinancialYearNumber", filter.Search_Fiscal_Year);
                //que = q.filterEquals(que, "job_tittle_id", filter.Search_job_tittle_id);
                //que = q.filterEquals(que, "mpp_budged", filter.Search_mpp_budged);
                //que = q.filterEquals(que, "mpp_id", filter.Search_mpp_id);
                //que = q.filterContains(que, "ttf", filter.Search_ttf);



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
    }
}

