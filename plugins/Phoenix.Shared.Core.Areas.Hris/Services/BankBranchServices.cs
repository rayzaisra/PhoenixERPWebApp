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
    public partial interface IBankBranchServices : IBaseService<BankBranchDto, BankBranch, string>
    {
        GeneralResponseList<BankBranchDto> ListAll(BankBranchSearch filter);
    }
    public partial class BankBranchServices : BaseService<BankBranchDto, BankBranch, string>, IBankBranchServices
    {
        public BankBranchServices(IEFRepository<BankBranch, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<BankBranchDto> ListAll(BankBranchSearch filter)
        {

            GeneralResponseList<BankBranchDto> resp = new GeneralResponseList<BankBranchDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<BankBranchDto> que = from tb1 in _repo.GetContext().Set<BankBranch>()
                          select new BankBranchDto()
                          {
                              Id = tb1.Id,
                              adress = tb1.adress,
                              bank_branch_id = tb1.bank_branch_id,
                              bank_id = tb1.bank_id,
                              branch_code = tb1.branch_code,
                              phone = tb1.phone,
                          };
                
                resp.RecordsTotal = que.Count();

                que = q.filterContains(que, "adress", filter.Search_adress);
                que = q.filterEquals(que, "bank_branch_id", filter.Search_bank_branch_id);
                que = q.filterEquals(que, "bank_id", filter.Search_bank_id);
                que = q.filterContains(que, "branch_code", filter.Search_branch_code);
                que = q.filterContains(que, "phone", filter.Search_phone);

                

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

