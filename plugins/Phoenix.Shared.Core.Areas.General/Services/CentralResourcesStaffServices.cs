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
    public partial interface ICentralResourcesStaffServices : IBaseService<CentralResourcesStaffDto, CentralResourcesStaff, string>
    {
        GeneralResponseList<CentralResourcesStaffDto> ListAll(CentralResourcesStaffSearch filter);
        GeneralResponseList<CentralResourcesStaffList> UserListAll(CentralResourcesStaffSearch filter); 
    }
    public partial class CentralResourcesStaffServices : BaseService<CentralResourcesStaffDto, CentralResourcesStaff, string>, ICentralResourcesStaffServices
    {
        public CentralResourcesStaffServices(IEFRepository<CentralResourcesStaff, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<CentralResourcesStaffList> UserListAll(CentralResourcesStaffSearch filter)
        {

            GeneralResponseList<CentralResourcesStaffList> resp = new GeneralResponseList<CentralResourcesStaffList>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<CentralResourcesStaffList> que = from tb1 in _repo.GetContext().Set<ApplicationUser>()
                                                           join tb2 in _repo.GetContext().Set<CentralResourcesStaff>() on tb1.Id equals tb2.user_id into tb1_2
                                                           from x in tb1_2.DefaultIfEmpty()
                                                           select new CentralResourcesStaffList()
                                                            {
                                                               user_id = tb1.Id,
                                                               Id=x.Id,
                                                               central_resources_id= x.central_resources_id,
                                                               app_fullname = tb1.app_fullname,
                                                               app_username = tb1.app_username
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "central_resources_id", filter.Search_central_resources_id);
                que = q.filterEquals(que, "user_id", filter.Search_user_id);

                

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
        public GeneralResponseList<CentralResourcesStaffDto> ListAll(CentralResourcesStaffSearch filter)
        {

            GeneralResponseList<CentralResourcesStaffDto> resp = new GeneralResponseList<CentralResourcesStaffDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<CentralResourcesStaffDto> que = from tb1 in _repo.GetContext().Set<CentralResourcesStaff>()
                                                           select new CentralResourcesStaffDto()
                                                           {
                                                               Id = tb1.Id,
                                                               central_resources_id = tb1.central_resources_id,
                                                               user_id = tb1.user_id,
                                                           };

                resp.RecordsTotal = que.Count();

                que = q.filterEquals(que, "central_resources_id", filter.Search_central_resources_id);
                que = q.filterEquals(que, "user_id", filter.Search_user_id);



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

