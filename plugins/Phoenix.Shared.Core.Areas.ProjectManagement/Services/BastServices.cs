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
    public partial interface IBastServices : IBaseService<BastDto, Bast, string>
    {
        GeneralResponseList<BastDto> ListAll(BastSearch filter);
    }
    public partial class BastServices : BaseService<BastDto, Bast, string>, IBastServices
    {
        public BastServices(IEFRepository<Bast, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<BastDto> ListAll(BastSearch filter)
        {

            GeneralResponseList<BastDto> resp = new GeneralResponseList<BastDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<BastDto> que = from tb1 in _repo.GetContext().Set<Bast>()
                          select new BastDto()
                          {
                              Id = tb1.Id,
                              bast_number = tb1.bast_number,
                              external_id = tb1.external_id,
                              po_id = tb1.po_id,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "bast_number", filter.Search_bast_number);
                que = q.filterEquals(que, "external_id", filter.Search_external_id);
                que = q.filterEquals(que, "po_id", filter.Search_po_id);

                

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

