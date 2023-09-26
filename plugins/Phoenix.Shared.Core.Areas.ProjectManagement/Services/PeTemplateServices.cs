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
    public partial interface IPeTemplateServices : IBaseService<PeTemplateDto, PeTemplate, string>
    {
        GeneralResponseList<PeTemplateDto> ListAll(PeTemplateSearch filter);
    }
    public partial class PeTemplateServices : BaseService<PeTemplateDto, PeTemplate, string>, IPeTemplateServices
    {
        public PeTemplateServices(IEFRepository<PeTemplate, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<PeTemplateDto> ListAll(PeTemplateSearch filter)
        {

            GeneralResponseList<PeTemplateDto> resp = new GeneralResponseList<PeTemplateDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<PeTemplateDto> que = from tb1 in _repo.GetContext().Set<PeTemplate>()
                          select new PeTemplateDto()
                          {
                              Id = tb1.Id,
                              pe_cost_perqty = tb1.pe_cost_perqty,
                              pe_item_qty = tb1.pe_item_qty,
                              pe_line_item = tb1.pe_line_item,
                              pe_number = tb1.pe_number,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "pe_cost_perqty", filter.Search_pe_cost_perqty);
                que = q.filterEquals(que, "pe_item_qty", filter.Search_pe_item_qty);
                que = q.filterEquals(que, "pe_line_item", filter.Search_pe_line_item);
                que = q.filterEquals(que, "pe_number", filter.Search_pe_number);

                

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

