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
    public partial interface IPositionServices : IBaseService<PositionDto, Position, string>
    {
        GeneralResponseList<PositionDto> ListAll(PositionSearch filter);
    }
    public partial class PositionServices : BaseService<PositionDto, Position, string>, IPositionServices
    {
        public PositionServices(IEFRepository<Position, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<PositionDto> ListAll(PositionSearch filter)
        {

            GeneralResponseList<PositionDto> resp = new GeneralResponseList<PositionDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<PositionDto> que = from tb1 in _repo.GetContext().Set<Position>()
                          select new PositionDto()
                          {
                              Id = tb1.Id,
                              central_resource_id = tb1.central_resource_id,
                              head = tb1.head,
                              level_position_id = tb1.level_position_id,
                              position_name = tb1.position_name,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "central_resource_id", filter.Search_central_resource_id);
                que = q.filterEquals(que, "head", filter.Search_head);
                que = q.filterEquals(que, "level_position_id", filter.Search_level_position_id);
                que = q.filterContains(que, "position_name", filter.Search_position_name);

                

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

