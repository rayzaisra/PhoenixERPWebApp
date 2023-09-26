using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Phoenix.Shared.Core.Areas.ProjectManagement.Dtos;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Parameters;
using Phoenix.Shared.Core.Repositories;
using Phoenix.Shared.Core.Services;
using Phoenix.Shared.Helpers;
using Phoenix.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Phoenix.Shared.Core.Areas.ProjectManagement.Services
{
    public partial interface IPceServices : IBaseService<PceDto, PCE, string>
    {
        GeneralResponseList<PceDto> ListAll(PceSearch filter);
        GeneralResponse<PceNew> CreatePce(PceNew data);
        GeneralResponse<PceDto> UpdatePce(PceDto data);
        GeneralResponse<PceDto> GetPce(string Id);
        BaseGeneralResponse openpce(string id);
    }
    public partial class PceServices : BaseService<PceDto, PCE, string>, IPceServices
    {
        IEFRepository<spjobnumber, string> _repocode;
        public PceServices(IEFRepository<PCE, string> repo,
            IEFRepository<spjobnumber, string> repocode) : base(repo)
        {
            _repo = repo;
            _repocode = repocode;
        }
        public BaseGeneralResponse openpce(string id)
        {
            BaseGeneralResponse resp = new BaseGeneralResponse();
            try
            {

                PCE nu = _repo.FindByID(id);
                if (nu != null)
                {
                    nu.status = "open";

                    _repo.Update(nu);
                    resp.Success = true;
                }
                else
                {
                    resp.Success = true;
                    resp.Message = "no Data";
                }

            }
            catch (Exception ex)
            {

                resp.Message = ex.Message;
            }
            return resp;
        }
        public GeneralResponse<PceDto> UpdatePce(PceDto data)
        {
            GeneralResponse<PceDto> resp = new GeneralResponse<PceDto>();
            try
            {

                PCE nu = _repo.FindByID(data.Id);
                if (nu != null)
                {
                    nu.name = data.name;
                    nu.total = data.total;
                    nu.job_id = data.job_id;
                    nu.currency = data.currency;
                    int current_rev = int.Parse(nu.revision) + 1;
                    nu.revision = current_rev.ToString();
                    nu.description = data.description;
                    long grandtotal = 0;
                    for (int i1 = 0; i1 < data.detail.Count(); i1++)
                    {
                        PceTaskDto items = data.detail[i1];
                        long tasktotal = 0;
                        for (int i2 = 0; i2 < items.sub_task.Count(); i2++)
                        {
                            PceSubTask itemsub = items.sub_task[i2];
                            itemsub.total = itemsub.price * itemsub.quantity;
                            tasktotal += itemsub.total;
                        }
                        grandtotal += tasktotal;
                        items.price = tasktotal;
                    }
                    nu.total = grandtotal;
                    var detail = JsonConvert.SerializeObject(data.detail);
                    nu.detail = detail;
                    //validate total-------
                    resp.Data = data;
                    _repo.Update(nu);
                    resp.Success = true;
                }
                else
                {
                    resp.Success = true;
                    resp.Message = "no Data";
                }
                
            }
            catch (Exception ex)
            {

                resp.Message = ex.Message;
            }
            return resp;
        }
        public GeneralResponse<PceNew> CreatePce(PceNew data)
        {
            GeneralResponse<PceNew> resp = new GeneralResponse<PceNew>();
            try
            {
                var rc_code = _repocode.GetEntities().FromSql("[pm].[sgGetPceCode]").FirstOrDefault().Id;
                PCE nu = new PCE();
                nu.name = data.name;
                nu.total = data.total;
                nu.job_id = data.job_id;
                nu.code = rc_code;
                nu.revision = "0";
                nu.description = data.description;
                nu.currency = data.currency;
                long grandtotal = 0;
 
                nu.total = grandtotal;
                var detail = JsonConvert.SerializeObject(data.detail);
                nu.detail = detail;
                //validate total-------
                resp.Data = data;
                _repo.Create( nu);
                resp.Success = true;
            }
            catch (Exception ex)
            {

                resp.Message = ex.Message;
            }
            return resp;
        }
        public GeneralResponse<PceDto> GetPce(string Id)
        {
            GeneralResponse<PceDto> resp = new GeneralResponse<PceDto>();
            try
            {
                IQueryable<PceDto> que = from tb1 in _repo.GetContext().Set<PCE>()
                                              join tb2 in _repo.GetContext().Set<JobDetail>() on tb1.job_id equals tb2.Id
                                         
                                         join tb3 in _repo.GetContext().Set<Currency>() on tb1.currency equals tb3.currency_code
                                         join tb6 in _repo.GetContext().Set<JobStatus>() on tb2.job_status_id equals tb6.Id
                                         join tb3x in _repo.GetContext().Set<ClientBrief>() on tb2.client_brief_id equals tb3x.Id
                                         join tbbu in _repo.GetContext().Set<BusinessUnit>() on tb3x.business_unit_id equals tbbu.Id
                                         join tb4x in _repo.GetContext().Set<CompanyBrand>() on tb3x.external_brand_id equals tb4x.Id
                                         join tb4a in _repo.GetContext().Set<Company>() on tb4x.company_id equals tb4a.Id
                                         join tbpca in _repo.GetContext().Set<Pca>() on tb1.pca_id equals tbpca.Id
                                         join tb4 in _repo.GetContext().Set<MotherPca>() on tb1.mother_pca_id equals tb4.Id into lj1
                                             from mpca in lj1.DefaultIfEmpty()
                                             join tb5 in _repo.GetContext().Set<RateCard>() on tb1.rate_card_id equals tb5.Id into lj2
                                             from rcard in lj2.DefaultIfEmpty()
                                         
                                         where tb1.Id ==Id
                                              select new PceDto()
                                              {
                                                  Id = tb1.Id,
                                                  description = tb1.description,
                                                  job_id = tb1.job_id,
                                                  job_name=tb2.job_name,
                                                  code = tb1.code,
                                                  job_status=tb6.name,
                                                  brand_name=tb4x.brand_name,
                                                  company_name=tb4a.company_name,
                                                  pca_code=tbpca.code,
                                                  detail = (from a in _repo.GetContext().Set<PceTask>()
                                                            where a.pce_id==tb1.Id
                                                            select new PceTaskDto()
                                                            {
                                                                id=a.Id,
                                                                price=a.price,
                                                                quantity=a.quantity,
                                                                task_name=a.task_name,
                                                                total=a.total,
                                                                unit=a.unit
                                                            }).ToList(),
                                                  name = tb1.name,
                                                  revision = tb1.revision,
                                                  total = tb1.total,
                                                  currency = tb1.currency,
                                                  currency_name = tb3.currency_name,
                                                  rate_card_id = rcard.Id,
                                                  rate_card_name = rcard.name,
                                                  mother_pca_id = mpca.Id,
                                                  mother_pca_name = mpca.name,
                                                  other_fee = tb1.other_fee,
                                                  other_fee_name = tb1.other_fee_name,
                                                  other_fee_percentage = tb1.other_fee_percentage,
                                                  status = tb1.status,
                                                  subtotal = tb1.subtotal,
                                                  vat = tb1.vat
                                              };
                resp.Data = que.FirstOrDefault();
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }
        public GeneralResponseList<PceDto> ListAll(PceSearch filter)
        {

            GeneralResponseList<PceDto> resp = new GeneralResponseList<PceDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<PceDto> que = from tb1 in _repo.GetContext().Set<PCE>()
                                         join tbpca in _repo.GetContext().Set<Pca>() on tb1.pca_id equals tbpca.Id
                                         join tb2 in _repo.GetContext().Set<JobDetail>() on tb1.job_id equals tb2.job_number
                                         join tb3 in _repo.GetContext().Set<ClientBrief>() on tb2.client_brief_id equals tb3.Id
                                         join tb4 in _repo.GetContext().Set<CompanyBrand>() on tb3.external_brand_id equals tb4.Id
                                         join tb4a in _repo.GetContext().Set<Company>() on tb4.company_id equals tb4a.Id
                                         join tb6 in _repo.GetContext().Set<JobStatus>() on tb2.job_status_id equals tb6.Id
                                         join tb5 in _repo.GetContext().Set<RateCard>() on tb1.rate_card_id equals tb5.Id into lj2
                                         from rcard in lj2.DefaultIfEmpty()
                                         select new PceDto()
                          {
                                             Id = tb1.Id,
                                             description = tb1.description,
                                             job_id = tb1.job_id,
                                             code = tb1.code,
                                             detail = JsonConvert.DeserializeObject<List<PceTaskDto>>(tb1.detail),
                                             name = tb1.name,
                                             pca_code=tbpca.code,
                                             revision = tb1.revision,
                                             total = tb1.total,
                                             currency = tb1.currency,
                                             job_status=tb6.name,
                                             status=tb1.status,
                                             job_name = tb2.job_name,
                                             brand_name = tb4.brand_name,
                                             rate_card_id = tb1.rate_card_id,
                                             rate_card_name = rcard.name,
                                             company_name=tb4a.company_name,
                                             created_date=tb1.created_on
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

