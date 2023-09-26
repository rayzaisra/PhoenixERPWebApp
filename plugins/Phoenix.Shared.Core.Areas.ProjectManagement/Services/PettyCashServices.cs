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
    public partial interface IPettyCashsServices : IBaseService<PettyCashsDto, PettyCashs, string>
    {
        GeneralResponseList<PettyCashsDto> ListAll(PettyCashsSearch filter);
    }
    public partial class PettyCashsServices : BaseService<PettyCashsDto, PettyCashs, string>, IPettyCashsServices
    {
        public PettyCashsServices(IEFRepository<PettyCashs, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<PettyCashsDto> ListAll(PettyCashsSearch filter)
        {
            throw new NotImplementedException();
        //    GeneralResponseList<PettyCashsDto> resp = new GeneralResponseList<PettyCashsDto>();
        //    try
        //    {
        //        PagingHelper q = new PagingHelper();
        //        IQueryable<PettyCashsDto> que = from tb1 in _repo.GetContext().Set<PettyCashs>()
        //                  select new PettyCashsDto()
        //                  {
        //                      Id = tb1.Id,
        //                      petty_amount = tb1.pettyyy_amount,
        //                      petty_date = tb1.petty_date,
        //                      task_id = tb1.task_id,
        //                      transfer_bank_accname = tb1.transfer_bank_accname,
        //                      transfer_bank_accno = tb1.transfer_bank_accno,
        //                      transfer_status_id = tb1.transfer_status_id,
        //                  };
                
        //        resp.RecordsTotal = que.Count();

        //                        que = q.filterEquals(que, "petty_amount", filter.Search_petty_amount);
        //        que = q.filterEquals(que, "petty_date", filter.Search_petty_date);
        //        que = q.filterEquals(que, "task_id", filter.Search_task_id);
        //        que = q.filterContains(que, "transfer_bank_accname", filter.Search_transfer_bank_accname);
        //        que = q.filterContains(que, "transfer_bank_accno", filter.Search_transfer_bank_accno);
        //        que = q.filterEquals(que, "transfer_status_id", filter.Search_transfer_status_id);

                

        //        resp.RecordsFiltered = que.Count();
        //        resp.Total = resp.RecordsTotal;

        //        que = q.sortir(que, "Id", filter.sortBy, filter.sortDir);
        //        que = q.limit(que, filter.GetLimit(), filter.GetOffset());
 
        //        resp.Rows = que.ToList();
        //        resp.Success = true;

        //    }
        //    catch (Exception ex)
        //    {
        //        resp.Success = false;
        //        resp.Message = ex.Message;
        //    }
        //    return resp;

        }
    }
}

