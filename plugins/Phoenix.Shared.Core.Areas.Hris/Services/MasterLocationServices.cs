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
    public partial interface IMasterLocationServices : IBaseService<MasterLocationDto, MasterLocation, string>
    {
        GeneralResponseList<MasterLocationDto> ListAll(MasterLocationSearch filter);
    }
    public partial class MasterLocationServices : BaseService<MasterLocationDto, MasterLocation, string>, IMasterLocationServices
    {
        public MasterLocationServices(IEFRepository<MasterLocation, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<MasterLocationDto> ListAll(MasterLocationSearch filter)
        {

            GeneralResponseList<MasterLocationDto> resp = new GeneralResponseList<MasterLocationDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<MasterLocationDto> que = from tb1 in _repo.GetContext().Set<MasterLocation>()
                          select new MasterLocationDto()
                          {
                              Id = tb1.Id,
                              location_name = tb1.location_name,
                              map_lat = tb1.map_lat,
                              map_long = tb1.map_long
                          };
                
                resp.RecordsTotal = que.Count();

                

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

