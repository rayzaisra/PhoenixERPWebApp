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
    public partial interface IOrganizationServices : IBaseService<OrganizationDto, Organization, string>
    {
        GeneralResponseList<OrganizationDto> ListAll(OrganizationSearch filter);
    }
    public partial class OrganizationServices : BaseService<OrganizationDto, Organization, string>, IOrganizationServices
    {
        public OrganizationServices(IEFRepository<Organization, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<OrganizationDto> ListAll(OrganizationSearch filter)
        {

            GeneralResponseList<OrganizationDto> resp = new GeneralResponseList<OrganizationDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<OrganizationDto> que = from tb1 in _repo.GetContext().Set<Organization>()
                          select new OrganizationDto()
                          {
                              Id = tb1.Id,
                              is_default_organization = tb1.is_default_organization,
                              organization_name = tb1.organization_name,
                              parent_organization_id = tb1.parent_organization_id,
                              smtp_from_address = tb1.smtp_from_address,
                              smtp_password = tb1.smtp_password,
                              smtp_port = tb1.smtp_port,
                              smtp_server = tb1.smtp_server,
                              smtp_username = tb1.smtp_username,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "is_default_organization", filter.Search_is_default_organization);
                que = q.filterContains(que, "organization_name", filter.Search_organization_name);
                que = q.filterEquals(que, "parent_organization_id", filter.Search_parent_organization_id);
                que = q.filterContains(que, "smtp_from_address", filter.Search_smtp_from_address);
                que = q.filterContains(que, "smtp_password", filter.Search_smtp_password);
                que = q.filterEquals(que, "smtp_port", filter.Search_smtp_port);
                que = q.filterContains(que, "smtp_server", filter.Search_smtp_server);
                que = q.filterContains(que, "smtp_username", filter.Search_smtp_username);

                

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

