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
    public partial interface IRateCardServices : IBaseService<RateCardDto, RateCard, string>
    {
        GeneralResponseList<RateCardDto> ListAll(RateCardSearch filter);
        GeneralResponse<RateCardNew> CreateRateCard(RateCardNew data);
        GeneralResponse<RateCardDto> UpdateRateCard(RateCardDto data);
        GeneralResponse<RateCardDto> GetRateCard(string Id);
    }
    public partial class RateCardServices : BaseService<RateCardDto, RateCard, string>, IRateCardServices
    {
        IEFRepository<spjobnumber, string> _repocode;
        public RateCardServices(IEFRepository<RateCard, string> repo,
            IEFRepository<spjobnumber, string> repocode) : base(repo)
        {
            _repo = repo;
            _repocode = repocode;
        }
        public GeneralResponse<RateCardDto> UpdateRateCard(RateCardDto data)
        {
            GeneralResponse<RateCardDto> resp = new GeneralResponse<RateCardDto>();
            try
            {
 
                RateCard nu = _repo.FindByID(data.Id);
                if (nu != null)
                {
                    nu.name = data.name;
                    nu.total = data.total;
                    nu.business_unit_id = data.business_unit_id;
                    nu.currency = data.currency;
                    int current_rev = int.Parse(nu.revision) + 1;
                    nu.revision = current_rev.ToString();
                    nu.description = data.description;
                    long grandtotal = 0;
                    for (int i1 = 0; i1 < data.detail.Count(); i1++)
                    {
                        RateCardTask items = data.detail[i1];
                        long tasktotal = 0;
                        for (int i2 = 0; i2 < items.sub_task.Count(); i2++)
                        {
                            RateCardSubTask itemsub = items.sub_task[i2];
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
        public GeneralResponse<RateCardNew> CreateRateCard(RateCardNew data)
        {
            GeneralResponse<RateCardNew> resp = new GeneralResponse<RateCardNew>();
            try
            {
                var rc_code = _repocode.GetEntities().FromSql("[pm].[sgGetRateCardCode]").FirstOrDefault().Id;
                RateCard nu = new RateCard();
                nu.name = data.name;
                nu.total = data.total;
                nu.business_unit_id = data.business_unit_id;
                nu.code = rc_code;
                nu.revision = "0";
                nu.description = data.description;
                nu.currency = data.currency;
                long grandtotal = 0;
                for (int i1 = 0; i1 < data.detail.Count(); i1++)
                {
                    RateCardTask items = data.detail[i1];
                    long tasktotal = 0;
                    for (int i2 = 0; i2 < items.sub_task.Count(); i2++)
                    {
                        RateCardSubTask itemsub = items.sub_task[i2];
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
                _repo.Create( nu);
                resp.Success = true;
            }
            catch (Exception ex)
            {

                resp.Message = ex.Message;
            }
            return resp;
        }
        public GeneralResponse<RateCardDto> GetRateCard(string Id)
        {
            GeneralResponse<RateCardDto> resp = new GeneralResponse<RateCardDto>();
            try
            {
                IQueryable<RateCardDto> que = from tb1 in _repo.GetContext().Set<RateCard>()
                                              join tb2 in _repo.GetContext().Set<BusinessUnit>() on tb1.business_unit_id equals tb2.Id
                                              join tb3 in _repo.GetContext().Set<Currency>() on tb1.currency equals tb3.currency_code
                                              where tb1.Id ==Id
                                              select new RateCardDto()
                                              {
                                                  Id = tb1.Id,
                                                  description = tb1.description,
                                                  business_unit_id = tb1.business_unit_id,
                                                  business_unit_name=tb2.unit_name,
                                                  code = tb1.code,
                                                  detail = JsonConvert.DeserializeObject<List<RateCardTask>>(tb1.detail),
                                                  name = tb1.name,
                                                  revision = tb1.revision,
                                                  total = tb1.total,
                                                  currency = tb1.currency,
                                                  currency_name = tb3.currency_name
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
        public GeneralResponseList<RateCardDto> ListAll(RateCardSearch filter)
        {

            GeneralResponseList<RateCardDto> resp = new GeneralResponseList<RateCardDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<RateCardDto> que = from tb1 in _repo.GetContext().Set<RateCard>()
                          select new RateCardDto()
                          {
                              Id = tb1.Id,
                              description = tb1.description,
                              business_unit_id=tb1.business_unit_id,
                              code=tb1.code,
                              detail= JsonConvert.DeserializeObject<List<RateCardTask>>(tb1.detail),
                              name=tb1.name,
                              revision=tb1.revision,
                              total=tb1.total,
                              currency=tb1.currency
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

