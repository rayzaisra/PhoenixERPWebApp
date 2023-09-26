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
    public partial interface IInformationServices : IBaseService<InformationDto, Information, string>
    {
        GeneralResponseList<InformationDto> ListAll(InformationSearch filter);
    }
    public partial class InformationServices : BaseService<InformationDto, Information, string>, IInformationServices
    {
        public InformationServices(IEFRepository<Information, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<InformationDto> ListAll(InformationSearch filter)
        {

            GeneralResponseList<InformationDto> resp = new GeneralResponseList<InformationDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<InformationDto> que = from tb1 in _repo.GetContext().Set<Information>()
                          select new InformationDto()
                          {
                              Id = tb1.Id,
                              body = tb1.body,
                              date_information = tb1.date_information,
                              flag_active = tb1.flag_active,
                              title = tb1.title,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "body", filter.Search_body);
                que = q.filterEquals(que, "date_information", filter.Search_date_information);
                que = q.filterEquals(que, "flag_active", filter.Search_flag_active);
                que = q.filterContains(que, "title", filter.Search_title);

                

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

