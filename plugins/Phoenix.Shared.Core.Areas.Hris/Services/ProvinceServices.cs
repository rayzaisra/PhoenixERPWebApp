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
    public partial interface IProvinceServices : IBaseService<ProvinceDto, Province, string>
    {
        GeneralResponseList<ProvinceDto> ListAll(ProvinceSearch filter);
    }
    public partial class ProvinceServices : BaseService<ProvinceDto, Province, string>, IProvinceServices
    {
        public ProvinceServices(IEFRepository<Province, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<ProvinceDto> ListAll(ProvinceSearch filter)
        {

            GeneralResponseList<ProvinceDto> resp = new GeneralResponseList<ProvinceDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<ProvinceDto> que = from tb1 in _repo.GetContext().Set<Province>()
                          select new ProvinceDto()
                          {
                              Id = tb1.Id,
                              code = tb1.code,
                              province = tb1.province,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "code", filter.Search_code);
                que = q.filterContains(que, "province", filter.Search_province);

                

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

