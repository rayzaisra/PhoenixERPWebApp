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
    public partial interface IBusinessUnitTypeServices : IBaseService<BusinessUnitTypeDto, BusinessUnitType, string>
    {
        GeneralResponseList<BusinessUnitTypeDto> ListAll(BusinessUnitTypeSearch filter);
    }
    public partial class BusinessUnitTypeServices : BaseService<BusinessUnitTypeDto, BusinessUnitType, string>, IBusinessUnitTypeServices
    {
        public BusinessUnitTypeServices(IEFRepository<BusinessUnitType, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<BusinessUnitTypeDto> ListAll(BusinessUnitTypeSearch filter)
        {

            GeneralResponseList<BusinessUnitTypeDto> resp = new GeneralResponseList<BusinessUnitTypeDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<BusinessUnitTypeDto> que = from tb1 in _repo.GetContext().Set<BusinessUnitType>()
                          select new BusinessUnitTypeDto()
                          {
                              Id = tb1.Id,
                              business_unit_type_name = tb1.business_unit_type_name,
                              organization_id = tb1.organization_id,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "business_unit_type_name", filter.Search_business_unit_type_name);
                que = q.filterEquals(que, "organization_id", filter.Search_organization_id);

                

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

