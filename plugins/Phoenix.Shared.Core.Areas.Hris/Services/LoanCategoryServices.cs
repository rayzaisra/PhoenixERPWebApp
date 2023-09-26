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
    public partial interface ILoanCategoryServices : IBaseService<LoanCategoryDto, LoanCategory, string>
    {
        GeneralResponseList<LoanCategoryDto> ListAll(LoanCategorySearch filter);
        GeneralResponseList<LoanCategoryDto> GetWithDetil(string id);
    }
    public partial class LoanCategoryServices : BaseService<LoanCategoryDto, LoanCategory, string>, ILoanCategoryServices
    {
        public LoanCategoryServices(IEFRepository<LoanCategory, string> repo) : base(repo)
        {
            _repo = repo;
        }

        public GeneralResponseList<LoanCategoryDto> ListAll(LoanCategorySearch filter)
        {

            GeneralResponseList<LoanCategoryDto> resp = new GeneralResponseList<LoanCategoryDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<LoanCategoryDto> que = from tb1 in _repo.GetContext().Set<LoanCategory>()
                                            select new LoanCategoryDto()
                                            {
                                                Id = tb1.Id,
                                                name = tb1.name,
                                                code = tb1.code,
                                                description = tb1.description,
                                                interest_month = tb1.interest_month
                                            };

                resp.RecordsTotal = que.Count();

                que = q.filterContains(que, "name", filter.Search_name);
                que = q.filterEquals(que, "interest_month", filter.Search_interest_month);



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

        public GeneralResponseList<LoanCategoryDto> GetWithDetil(string id)
        {
            GeneralResponseList<LoanCategoryDto> resp = new GeneralResponseList<LoanCategoryDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<LoanCategoryDto> que = from tb1 in _repo.GetContext().Set<LoanCategory>()
                                                         //where tb1.salary_component == id
                                                     select new LoanCategoryDto()
                                                     {
                                                         Id = tb1.Id,
                                                         name = tb1.name,
                                                         interest_month = tb1.interest_month,

                                                     };
                //resp.RecordsTotal = que.Count();
                //resp.Data = que.FirstOrDefault();
                resp.RecordsFiltered = que.Count();
                resp.Total = resp.RecordsTotal;

                //que = q.sortir(que, "Id", filter.sortBy, filter.sortDir);
                //que = q.limit(que, filter.GetLimit(), filter.GetOffset());
                que = q.filterContains(que, "name", id);
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
    }
}

