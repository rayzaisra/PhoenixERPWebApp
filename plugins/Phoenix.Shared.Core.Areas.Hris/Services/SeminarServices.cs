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
    public partial interface ISeminarServices : IBaseService<SeminarDto, Seminar, string>
    {
        GeneralResponseList<SeminarDto> ListAll(SeminarSearch filter);
    }
    public partial class SeminarServices : BaseService<SeminarDto, Seminar, string>, ISeminarServices
    {
        public SeminarServices(IEFRepository<Seminar, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<SeminarDto> ListAll(SeminarSearch filter)
        {

            GeneralResponseList<SeminarDto> resp = new GeneralResponseList<SeminarDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<SeminarDto> que = from tb1 in _repo.GetContext().Set<Seminar>()
                          select new SeminarDto()
                          {
                              Id = tb1.Id,
                              competence_id = tb1.competence_id,
                              contact_no = tb1.contact_no,
                              cover_seminar = tb1.cover_seminar,
                              end_date = tb1.end_date,
                              person_in_charge = tb1.person_in_charge,
                              seminar_address = tb1.seminar_address,
                              start_date = tb1.start_date,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "competence_id", filter.Search_competence_id);
                que = q.filterContains(que, "contact_no", filter.Search_contact_no);
                que = q.filterContains(que, "cover_seminar", filter.Search_cover_seminar);
                que = q.filterEquals(que, "end_date", filter.Search_end_date);
                que = q.filterContains(que, "person_in_charge", filter.Search_person_in_charge);
                que = q.filterContains(que, "seminar_address", filter.Search_seminar_address);
                que = q.filterEquals(que, "start_date", filter.Search_start_date);

                

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

