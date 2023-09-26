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
    public partial interface IPaDetailServices : IBaseService<PaDetailDto, PaDetail, string>
    {
        GeneralResponseList<PaDetailDto> ListAll(PaDetailSearch filter);
    }
    public partial class PaDetailServices : BaseService<PaDetailDto, PaDetail, string>, IPaDetailServices
    {
        public PaDetailServices(IEFRepository<PaDetail, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<PaDetailDto> ListAll(PaDetailSearch filter)
        {

            GeneralResponseList<PaDetailDto> resp = new GeneralResponseList<PaDetailDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<PaDetailDto> que = from tb1 in _repo.GetContext().Set<PaDetail>()
                          select new PaDetailDto()
                          {

                              category = tb1.category,
                              price_per_quantity = tb1.price_per_quantity,
                              quantity = tb1.quantity,
                              remarks = tb1.remarks,
                              sub_brand = tb1.sub_brand,
                              asf=tb1.asf,
                              brand=tb1.brand,
                              category_name=tb1.category_name,
                              pph=tb1.pph,
                              ppn=tb1.ppn,
                              sub_task_id=tb1.sub_task_id,
                              task_id=tb1.task_id,
                              uom=tb1.uom
                          };
                
                resp.RecordsTotal = que.Count();
 

                

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

