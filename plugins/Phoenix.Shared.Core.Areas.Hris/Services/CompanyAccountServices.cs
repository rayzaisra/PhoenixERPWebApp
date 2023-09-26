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
    public partial interface ICompanyAccountServices : IBaseService<CompanyAccountDto, CompanyAccount, string>
    {
        GeneralResponseList<CompanyAccountDto> ListAll(CompanyAccountSearch filter);
    }
    public partial class CompanyAccountServices : BaseService<CompanyAccountDto, CompanyAccount, string>, ICompanyAccountServices
    {
        public CompanyAccountServices(IEFRepository<CompanyAccount, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<CompanyAccountDto> ListAll(CompanyAccountSearch filter)
        {

            GeneralResponseList<CompanyAccountDto> resp = new GeneralResponseList<CompanyAccountDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<CompanyAccountDto> que = from tb1 in _repo.GetContext().Set<CompanyAccount>()
                          select new CompanyAccountDto()
                          {
                              Id = tb1.Id,
                              bankbranch_id = tb1.bankbranch_id,
                              bank_account_id = tb1.bank_account_id,
                              bank_id = tb1.bank_id,
                              branchcomapany_id = tb1.branchcomapany_id,
                              branch_name = tb1.branch_name,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "bankbranch_id", filter.Search_bankbranch_id);
                que = q.filterEquals(que, "bank_account_id", filter.Search_bank_account_id);
                que = q.filterEquals(que, "bank_id", filter.Search_bank_id);
                que = q.filterEquals(que, "branchcomapany_id", filter.Search_branchcomapany_id);
                que = q.filterContains(que, "branch_name", filter.Search_branch_name);

                

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

