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
    public partial interface IApplicationUserServices : IBaseService<ApplicationUserDto, ApplicationUser, string>
    {
        GeneralResponseList<ApplicationUserDto> ListAll(ApplicationUserSearch filter);
    }
    public partial class ApplicationUserServices : BaseService<ApplicationUserDto, ApplicationUser, string>, IApplicationUserServices
    {
        public ApplicationUserServices(IEFRepository<ApplicationUser, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<ApplicationUserDto> ListAll(ApplicationUserSearch filter)
        {

            GeneralResponseList<ApplicationUserDto> resp = new GeneralResponseList<ApplicationUserDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<ApplicationUserDto> que = from tb1 in _repo.GetContext().Set<ApplicationUser>()
                          select new ApplicationUserDto()
                          {
                              Id = tb1.Id,
                              access_token = tb1.access_token,
                              app_fullname = tb1.app_fullname,
                              app_password = tb1.app_password,
                              app_username = tb1.app_username,
                              business_unit_id = tb1.business_unit_id,
                              email = tb1.email,
                              is_system_administrator = tb1.is_system_administrator,
                              manager_id = tb1.manager_id,
                              mime_type = tb1.mime_type,
                              organization_id = tb1.organization_id,
                              phone = tb1.phone,
                              photo_base64 = tb1.photo_base64,
                              primary_team = tb1.primary_team,
                              thumbnail_base64 = tb1.thumbnail_base64,
                              token_expiry = tb1.token_expiry,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "access_token", filter.Search_access_token);
                que = q.filterContains(que, "app_fullname", filter.Search_app_fullname);
                que = q.filterContains(que, "app_password", filter.Search_app_password);
                que = q.filterContains(que, "app_username", filter.Search_app_username);
                que = q.filterEquals(que, "business_unit_id", filter.Search_business_unit_id);
                que = q.filterContains(que, "email", filter.Search_email);
                que = q.filterEquals(que, "is_system_administrator", filter.Search_is_system_administrator);
                que = q.filterEquals(que, "manager_id", filter.Search_manager_id);
                que = q.filterContains(que, "mime_type", filter.Search_mime_type);
                que = q.filterEquals(que, "organization_id", filter.Search_organization_id);
                que = q.filterContains(que, "phone", filter.Search_phone);
                que = q.filterContains(que, "photo_base64", filter.Search_photo_base64);
                que = q.filterContains(que, "primary_team", filter.Search_primary_team);
                que = q.filterContains(que, "thumbnail_base64", filter.Search_thumbnail_base64);
                que = q.filterEquals(que, "token_expiry", filter.Search_token_expiry);

                

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

