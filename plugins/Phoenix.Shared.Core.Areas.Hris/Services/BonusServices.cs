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
    public partial interface IBonusServices : IBaseService<BonusDto, Bonus, string>
    {
        GeneralResponseList<BonusDto> ListAll(BonusSearch filter);
    }
    public partial class BonusServices : BaseService<BonusDto, Bonus, string>, IBonusServices
    {
        public BonusServices(IEFRepository<Bonus, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<BonusDto> ListAll(BonusSearch filter)
        {

            GeneralResponseList<BonusDto> resp = new GeneralResponseList<BonusDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<BonusDto> que = from tb1 in _repo.GetContext().Set<Bonus>()
                          select new BonusDto()
                          {
                              Id = tb1.Id,
                              amount = tb1.amount,
                              bonus_description = tb1.bonus_description,
                              date_bonus = tb1.date_bonus,
                              employee_basic_info_id = tb1.employee_basic_info_id,
                              notes = tb1.notes,
                              title = tb1.title,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "amount", filter.Search_amount);
                que = q.filterContains(que, "bonus_description", filter.Search_bonus_description);
                que = q.filterEquals(que, "date_bonus", filter.Search_date_bonus);
                que = q.filterEquals(que, "employee_basic_info_id", filter.Search_employee_basic_info_id);
                que = q.filterContains(que, "notes", filter.Search_notes);
                que = q.filterContains(que, "title", filter.Search_title);

                

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

