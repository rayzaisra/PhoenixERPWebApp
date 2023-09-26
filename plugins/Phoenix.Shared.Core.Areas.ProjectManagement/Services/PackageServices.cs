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
    public partial interface IPackageServices : IBaseService<PackageDto, Package, string>
    {
        GeneralResponseList<PackageDto> ListAll(PackageSearch filter);
    }
    public partial class PackageServices : BaseService<PackageDto, Package, string>, IPackageServices
    {
        public PackageServices(IEFRepository<Package, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<PackageDto> ListAll(PackageSearch filter)
        {

            GeneralResponseList<PackageDto> resp = new GeneralResponseList<PackageDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<PackageDto> que = from tb1 in _repo.GetContext().Set<Package>()
                          select new PackageDto()
                          {
                              Id = tb1.Id,
                              description = tb1.description,
                              job_category_id = tb1.job_category_id,
                              package = tb1.package,
                              value = tb1.value,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "description", filter.Search_description);
                que = q.filterEquals(que, "job_category_id", filter.Search_job_category_id);
                que = q.filterContains(que, "package", filter.Search_package);
                que = q.filterEquals(que, "value", filter.Search_value);

                

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

