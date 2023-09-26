using Phoenix.Shared.Core.Areas.General.Dtos;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Parameters;
using Phoenix.Shared.Core.Repositories;
using Phoenix.Shared.Core.Services;
using Phoenix.Shared.Responses;
using System;
using System.Linq;


namespace Phoenix.Shared.Core.Areas.General.Services
{
    public partial interface ICentralResourceServices : IBaseService<CentralResourceDto, CentralResource, string>
    {
        GeneralResponseList<CentralResourceDto> ListAll(CentralResourceSearch filter);
        new GeneralResponse<CentralResourceDto> GetByIDResponse(string id);
    }
    public partial class CentralResourceServices : BaseService<CentralResourceDto, CentralResource, string>, ICentralResourceServices
    {
        public CentralResourceServices(IEFRepository<CentralResource, string> repo) : base(repo)
        {
            _repo = repo;
        }
        public new GeneralResponse<CentralResourceDto> GetByIDResponse(string id)
        {

            GeneralResponse<CentralResourceDto> resp = new GeneralResponse<CentralResourceDto>();
            try
            {

                IQueryable<CentralResourceDto> que = from tb1 in _repo.GetContext().Set<CentralResource>()
                                                     join tb2 in _repo.GetContext().Set<BusinessUnit>() on tb1.business_unit_id equals tb2.Id
                                                     where tb1.Id==id
                                                     select new CentralResourceDto()
                                                     {
                                                         Id = tb1.Id,
                                                         business_unit_id = tb1.business_unit_id,
                                                         name = tb1.name,
                                                         is_channel=tb1.is_channel,
                                                         business_unit_name = tb2.unit_name
                                                     };
 
                resp.Data = que.FirstOrDefault();
                resp.Success = true;

            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;

        }
        public GeneralResponseList<CentralResourceDto> ListAll(CentralResourceSearch filter)
        {

            GeneralResponseList<CentralResourceDto> resp = new GeneralResponseList<CentralResourceDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<CentralResourceDto> que = from tb1 in _repo.GetContext().Set<CentralResource>()
                                                     join tb2 in _repo.GetContext().Set<BusinessUnit>() on tb1.business_unit_id equals tb2.Id
                                                     select new CentralResourceDto()
                          {
                              Id = tb1.Id,
                              business_unit_id = tb1.business_unit_id,
                              name=tb1.name,
                                                         is_channel = tb1.is_channel,
                                                         business_unit_name =tb2.unit_name
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "business_unit_id", filter.Search_business_unit_id);

                

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

