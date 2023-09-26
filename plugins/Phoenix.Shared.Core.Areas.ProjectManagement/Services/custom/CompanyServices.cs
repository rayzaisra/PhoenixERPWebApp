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
    public partial interface ICompanyServices : IBaseService<CompanyDto, Company, string>
    {
        GeneralResponseList<CompanyDto> ListAll(CompanySearch filter);
    }
    public partial class CompanyServices : BaseService<CompanyDto, Company, string>, ICompanyServices
    {
        public CompanyServices(IEFRepository<Company, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<CompanyDto> ListAll(CompanySearch filter)
        {

            GeneralResponseList<CompanyDto> resp = new GeneralResponseList<CompanyDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<CompanyDto> que = from tb1 in _repo.GetContext().Set<Company>()
                          select new CompanyDto()
                          {
                              Id = tb1.Id,
                              company_name = tb1.company_name,
                              email = tb1.email,
                              facebook = tb1.facebook,
                              fax = tb1.fax,
                              instagram = tb1.instagram,
                              is_client = tb1.is_client,
                              is_vendor = tb1.is_vendor,
                              mobile_phone = tb1.mobile_phone,
                              organization_id = tb1.organization_id,
                              payment_term_day = tb1.payment_term_day,
                              phone = tb1.phone,
                              registration_code = tb1.registration_code,
                              shipping_term_day = tb1.shipping_term_day,
                              tax_number = tb1.tax_number,
                              twitter = tb1.twitter,
                              website = tb1.website,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "company_name", filter.Search_company_name);
                que = q.filterContains(que, "email", filter.Search_email);
                que = q.filterContains(que, "facebook", filter.Search_facebook);
                que = q.filterContains(que, "fax", filter.Search_fax);
                que = q.filterContains(que, "instagram", filter.Search_instagram);
                que = q.filterEquals(que, "is_client", filter.Search_is_client);
                que = q.filterEquals(que, "is_vendor", filter.Search_is_vendor);
                que = q.filterContains(que, "mobile_phone", filter.Search_mobile_phone);
                que = q.filterEquals(que, "organization_id", filter.Search_organization_id);
                que = q.filterEquals(que, "payment_term_day", filter.Search_payment_term_day);
                que = q.filterContains(que, "phone", filter.Search_phone);
                que = q.filterContains(que, "registration_code", filter.Search_registration_code);
                que = q.filterEquals(que, "shipping_term_day", filter.Search_shipping_term_day);
                que = q.filterContains(que, "tax_number", filter.Search_tax_number);
                que = q.filterContains(que, "twitter", filter.Search_twitter);
                que = q.filterContains(que, "website", filter.Search_website);

                

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

