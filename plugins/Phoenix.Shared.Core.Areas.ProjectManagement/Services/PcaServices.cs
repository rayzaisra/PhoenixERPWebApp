using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Phoenix.Shared.Core.Areas.ProjectManagement.Dtos;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Entities.Finance;
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
    public partial interface IPcaServices : IBaseService<PcaDto, Pca, string>
    {
        GeneralResponseList<AccountDto> ListAccount(SearchParameter filter);
        GeneralResponseList<PcaDto> ListAll(PcaSearch filter);
        GeneralResponse<PcaNew> CreatePca(PcaNew data);
        GeneralResponse<PcaDto> UpdatePca(PcaDto data);
        GeneralResponse<PcaDto> GetPca(string Id);
      
        BaseGeneralResponse submittopce(string pca_id);
        GeneralResponse<PcaRfq> CreateRfq(PcaRfq data);
        GeneralResponse<PcaCa> CreateCa(PcaCa data);
        //BaseGeneralResponse CreateRfq(string Rfq_Id);
        
    }

    

    public partial class PcaServices : BaseService<PcaDto, Pca, string>, IPcaServices
    {
        IEFRepository<spjobnumber, string> _repocode;
        IEFRepository<PcaTask, string> _repopcatask;
        IEFRepository<PCE, string> _repopce;
        IEFRepository<Pca, string> _repopca;
        IEFRepository<PceTask, string> _repopcetask;
        IEFRepository<RequestForQuotation, string> _repopcarfq;
        IEFRepository<RequestForQuotationTask, string> _repopcarfqtask;
        IEFRepository<RequestForQuotationDetail, Guid> _repopcarfqdetail;
        IEFRepository<RfqCode, string> _reporfqcode;
        IEFRepository<PcaRefrence, string> _reporefrence;
        IEFRepository<TrTemplateApproval, string> _reportrtemplateapproval;


        public PcaServices(IEFRepository<Pca, string> repo,
            IEFRepository<PcaTask, string> repopcatask,
            IEFRepository<PcaRefrence, string> reporefrence,
            IEFRepository<PCE, string> repopce,
        IEFRepository<PceTask, string> repopcetask,
        IEFRepository<RequestForQuotation, string> repopcarfq,
            IEFRepository<RequestForQuotationTask, string> repopcarfqtask,
            IEFRepository<RfqCode, string> reporfqcode,
            IEFRepository<RequestForQuotationDetail, Guid> repopcarfqdetail,
            IEFRepository<TrTemplateApproval, string> reportrtemplateapproval,

        IEFRepository<spjobnumber, string> repocode) : base(repo)
        
        {

            _repo = repo;
            _reporefrence = reporefrence;
            _repocode = repocode;
            _repopcatask = repopcatask;
            _repopce = repopce;
            _repopcetask = repopcetask;
            _repopcarfq = repopcarfq;
            _repopcarfqdetail = repopcarfqdetail;
            _repopcarfqtask = repopcarfqtask;
            _reporfqcode = reporfqcode;
            _reportrtemplateapproval = reportrtemplateapproval;
     
            //_repopcarfq = repopcarfq;


        }





        public GeneralResponse<PcaDto> UpdatePca(PcaDto data)
        {
            GeneralResponse<PcaDto> resp = new GeneralResponse<PcaDto>();
            try
            {
 
                Pca nu = _repo.FindByID(data.Id);
                if (nu != null)
                {
                    nu.name = data.name;
                    nu.total = data.total;
                    nu.job_id = data.job_id;

                    nu.currency = data.currency;
                    int current_rev = int.Parse(nu.revision) + 1;
                    nu.revision = current_rev.ToString();

                    nu.other_fee = data.other_fee;
                    nu.other_fee_name = data.other_fee_name;
                    nu.type_of_expense_id = data.type_of_expense_id;
                    nu.other_fee_percentage = data.other_fee_percentage;
                    nu.vat = data.vat;
                    nu.mother_pca_id = data.mother_pca_id;
                    nu.rate_card_id = data.rate_card_id;

                  
                    nu.mainservice_category_id = data.mainservice_category_id;
                    nu.termofpayment1 = data.termofpayment1;
                    nu.termofpayment2 = data.termofpayment2;
                    nu.termofpayment3 = data.termofpayment3;
                    nu.shareservices_id = data.shareservices_id;
                    nu.subtotal = data.subtotal;

                    var detail = JsonConvert.SerializeObject(data.detail);
                    nu.detail = detail;

                    //nu.description = data.description;
                    long grandtotal = 0;
                    for (int i1 = 0; i1 < data.detail.Count(); i1++)
                    {
                        PcaTaskDto items = data.detail[i1];
                        long tasktotal = 0;
                        for (int i2 = 0; i2 < items.sub_task.Count(); i2++)
                        {
                            PcaSubTask itemsub = items.sub_task[i2];
                            itemsub.total = itemsub.price * itemsub.quantity;
                            tasktotal += itemsub.total;
                        }
                        grandtotal += tasktotal;
                        items.price = tasktotal;
                    }


                    nu.total = grandtotal;

                    //validate total-------



                    for (int i1 = 0; i1 < data.detail.Count(); i1++)
                    {
                        PcaTask nd = _repopcatask.FindByID(data.detail[i1].Id);

                        if (nd != null)
                        {
                            PcaTaskDto items = data.detail[i1];
                            long tasktotal = 0;

                            PropertyMapper.All(items, nd);

                            nd.departement = items.departement;
                            nd.price = items.price;

                            nd.detail = JsonConvert.SerializeObject(items.sub_task);
                            _repopcatask.Update(nd);
                        }
                        else
                        {
                            PcaTaskDto items = data.detail[i1];
                            long tasktotal = 0;
                            PcaTask newpcatask = new PcaTask();
                            PropertyMapper.All(items, newpcatask);
                            newpcatask.pca_id = nu.Id;
                            newpcatask.Id = null;
                            newpcatask.detail = JsonConvert.SerializeObject(items.sub_task);
                            _repopcatask.Create(false, newpcatask);
                        }
                    }



                    resp.Data = data;
                    _repo.Update(nu);
                    _repopcatask.SaveChanges();
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
 
        public BaseGeneralResponse submittopce(string pca_id)
        {
            BaseGeneralResponse resp = new BaseGeneralResponse();
            resp.Success = false;
            try
            {
                Pca datapca = _repo.FindByID(pca_id);
                ExpressionParameter<PCE> fvarpce = new ExpressionParameter<PCE>();
                fvarpce.where = w => w.pca_id.Equals(pca_id);
                PCE existpce = _repopce.FindFirstOrDefault(fvarpce);
                if (datapca != null)
                {
                    
                    if (existpce != null)
                    {
                        string[] GetUpdateExcepts = { "Id", "code", "pca_id", "status" };
   
                        PropertyMapper.Except(GetUpdateExcepts, datapca, existpce);
                        existpce.status = "draft";
                        ExpressionParameter<PcaTask> fparamtm = new ExpressionParameter<PcaTask>();
                        fparamtm.where = w => w.pca_id.Equals(datapca.Id); ;
                        IList<PcaTask> copy_data = _repopcatask.FindAll(fparamtm);
                        _repopce.Update(existpce);
                        var delall = _repo.Rawquery("delete from [pm].[pce_task] where pce_id='" + existpce.Id + "'");
                        foreach (PcaTask pdata in copy_data)
                        {
                            PceTask datapcetask = new PceTask();
                            PropertyMapper.All(pdata, datapcetask);
                            datapcetask.Id = null;
                            datapcetask.pce_id = existpce.Id;
                            datapcetask.price = datapcetask.total / datapcetask.quantity;
                            _repopcetask.Create(datapcetask);
                        }
                    }
                    else
                    {
                        var rc_code = _repocode.GetEntities().FromSql("[pm].[sgGetPceCode]").FirstOrDefault().Id;

                        PCE datapce = new PCE();
                        PropertyMapper.All(datapca, datapce);
                        datapce.code = rc_code;
                        datapce.pca_id = datapca.Id;
                        datapce.Id = null;
                        ExpressionParameter<PcaTask> fparamtm = new ExpressionParameter<PcaTask>();
                        fparamtm.where = w => w.pca_id.Equals(datapca.Id); ;
                        IList<PcaTask> copy_data = _repopcatask.FindAll(fparamtm);
                        datapce.status = "draft";
                        _repopce.Create(datapce);
                        foreach (PcaTask pdata in copy_data)
                        {
                            PceTask datapcetask = new PceTask();
                            PropertyMapper.All(pdata, datapcetask);
                            datapcetask.Id = null;
                            datapcetask.pce_id = datapce.Id;
                            datapcetask.price = datapcetask.total / datapcetask.quantity;
                            _repopcetask.Create(datapcetask);
                        }
                    }
                    resp.Success = true;
                    
                    
                }
            }
            catch (Exception ex)
            {

                resp.Message = ex.Message;
            }
            return resp;
        }
        public GeneralResponse<PcaNew> CreatePca(PcaNew data)
        {
            GeneralResponse<PcaNew> resp = new GeneralResponse<PcaNew>();
            
            try
            {
                var rc_code = _repocode.GetEntities().FromSql("[pm].[sgGetPcaCode]").FirstOrDefault().Id;
                Pca nu = new Pca();
                
                nu.name = data.name;
                nu.total = data.total;
                nu.job_id = data.job_id;
                nu.code = rc_code;
                nu.revision = "0";
                //nu.description = data.description;
                nu.currency = data.currency;
                nu.other_fee = data.other_fee;
                nu.other_fee_name = data.other_fee_name;
                nu.type_of_expense_id = data.type_of_expense_id;
                nu.other_fee_percentage = data.other_fee_percentage;
                nu.vat = data.vat;
                nu.mother_pca_id = data.mother_pca_id;
                nu.rate_card_id = data.rate_card_id;
                nu.status = "draft";
                nu.mainservice_category_id = data.mainservice_category_id;
                nu.termofpayment1 = data.termofpayment1;
                nu.termofpayment2 = data.termofpayment2;
                nu.termofpayment3 = data.termofpayment3;
                nu.shareservices_id = data.shareservices_id;
                nu.subtotal = data.subtotal;
                
                decimal? grandtotal = 0;
                for (int i1 = 0; i1 < data.detail.Count(); i1++)
                {
                    PcaTaskDto items = data.detail[i1];
                    long tasktotal = 0;
                    for (int i2 = 0; i2 < items.sub_task.Count(); i2++)
                    {
                        PcaSubTask itemsub = items.sub_task[i2];
                        itemsub.total = itemsub.price * itemsub.quantity;
                        tasktotal += itemsub.total;
                    }
                    grandtotal += items.total;
                    items.price = tasktotal;
                }
                //nu.total = grandtotal;
                var detail = JsonConvert.SerializeObject(data.detail);
                nu.detail = detail;
                //validate total-------
                resp.Data = data;
                _repo.Create(false, nu);

                
                if (nu.type_of_expense_id == "1128498563668643849")
                {
                    PcaRefrence nr = new PcaRefrence();

                    nr.pca_id = nu.Id;
                    nr.pca_refrence = data.pca_refrence;

                    resp.Data = data;
                    _reporefrence.Create(false, nr);
                }


                for (int i1 = 0; i1 < data.detail.Count(); i1++)
                {
                    PcaTaskDto items = data.detail[i1];
                    long tasktotal = 0;
                    PcaTask newpcatask = new PcaTask();
                    PropertyMapper.All(items, newpcatask);
                    newpcatask.pca_id = nu.Id;
                    newpcatask.Id = null;
                    newpcatask.detail = JsonConvert.SerializeObject(items.sub_task);
                    _repopcatask.Create(false, newpcatask);
                }
                _repo.SaveChanges();
                _repopcatask.SaveChanges();
                resp.Data = data;
                resp.Success = true;
            }
            catch (Exception ex)
            {

                resp.Message = ex.Message;
            }
            return resp;
        }

        public GeneralResponse<PcaCa> CreateCa(PcaCa data)
        {
            return null;
        }

        //public BaseGeneralResponse CreateRfq(string Rfq_Id)
        public GeneralResponse<PcaRfq> CreateRfq(PcaRfq data)
        {
            //BaseGeneralResponse resp = new BaseGeneralResponse();
            //RequestForQuotation newdata = new RequestForQuotation();
            GeneralResponse<PcaRfq> resp = new GeneralResponse<PcaRfq>();
            
            try
            {
                
                
                
                RequestForQuotation nu = new RequestForQuotation();
                //RequestForQuotationTask nt = new RequestForQuotationTask();
               

               
                    nu.request_for_quotation_date = DateTime.Now;
                    nu.job_id = data.job_id;
                    nu.legal_entity_id = data.legal_entity_id;
                    nu.business_unit_id = data.business_unit_id;
                    nu.is_prefered_vendor = data.is_prefered_vendor;
                    nu.job_pa_id = data.job_pa_id;
                    nu.created_by = Glosing.Instance.Username;
                    nu.owner_id = Glosing.Instance.Username;
                    var rfq_code = _repocode.GetEntities().FromSql("[pm].[sgGetRfqCode]").FirstOrDefault().Id;
                    nu.request_for_quotation_number = rfq_code;
                    nu.is_active = true;
                    _repopcarfq.Create(false, nu);
                    _repopcarfq.SaveChanges();
                    //resp.Data = data;

                 foreach (PcaRfqDetail obj in data.details)
                    {
                    RequestForQuotationDetail nd = new RequestForQuotationDetail();

                        nd.unit_price = obj.unit_price;
                        nd.item_name = obj.item_name;
                        nd.request_for_quotation_id = nu.Id;
                        nd.qty = obj.qty;
                        nd.owner_id = nu.owner_id;
                        nd.is_active = true;
                        nd.is_deleted = false;
                        nd.qty_shift = nd.qty_shift;
                        nd.qty_week = nd.qty_week;
                        nd.qty_day = obj.qty_day;
                        nd.currency_id = obj.currency_id;
                        nd.exchange_rate = obj.exchange_rate;
                        nd.item_id = obj.item_id;
                        nd.item_type = obj.item_type;
                        nd.uom_id = obj.uom_id;
                        nd.subbrand_name = obj.subbrand_name;
                        nd.description = obj.description;
                        nd.category_id = obj.category_id;
                        nd.amount = obj.amount;
                        nd.filemaster_id = obj.filemaster_id;
                        nd.task = obj.task;
                        nd.subtask = obj.subtask;
                        nd.revision_status_pca = obj.revision_status_pca;

                        _repopcarfqdetail.Create(false, nd);
                        
        

                    }

                    _repopcarfqdetail.SaveChanges();
                //_repopcarfqtask.SaveChanges();
                //_repopcarfqtask.Dispose();


                _repopcarfqdetail.Dispose();
                //_repopcarfq.Dispose();
                resp.Data = data;
           




            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }

            return resp;
        }

        public GeneralResponse<PcaDto> GetPca(string Id)
        {
            GeneralResponse<PcaDto> resp = new GeneralResponse<PcaDto>();
            try
            {

                IQueryable<PcaDto> que = from tb1 in _repo.GetContext().Set<Pca>()

                                         join tb2 in _repo.GetContext().Set<JobDetail>() on tb1.job_id equals tb2.Id
                                         join tb3 in _repo.GetContext().Set<Currency>() on tb1.currency equals tb3.currency_code
                                         join tb4 in _repo.GetContext().Set<MotherPca>() on tb1.mother_pca_id equals tb4.Id into lj1
                                         join tb6 in _repo.GetContext().Set<TypeOfExpense>() on tb1.type_of_expense_id equals tb6.Id
                                         join tb7 in _repo.GetContext().Set<ShareServices>() on tb1.shareservices_id equals tb7.Id
                                         join tb8 in _repo.GetContext().Set<MainserviceCategory>() on tb1.mainservice_category_id equals tb8.Id
                                    
                                         from mpca in lj1.DefaultIfEmpty()
                                         join tb5 in _repo.GetContext().Set<RateCard>() on tb1.rate_card_id equals tb5.Id
                                         //join tb5 in _repo.GetContext().Set<RateCard>() on tb1.rate_card_id equals tb5.Id into lj2
                                         //from rcard in lj2.DefaultIfEmpty()
                                         where tb1.Id == Id


                                         select new PcaDto()

                                         {
                                             Id = tb1.Id,
                                             //description = tb1.description,
                                             job_id = tb1.job_id,
                                             job_name = tb2.job_name,
                                             code = tb1.code,
                                             //detail = JsonConvert.DeserializeObject<List<PcaTaskDto>>(tb1.detail),
                                    
                                             detail = _repo.GetContext().Set<PcaTask>().Where(i => i.pca_id == tb1.Id).Select(i => new PcaTaskDto()
                                             {
                                                 Id = i.Id,
                                                 departement = i.departement,
                                                 sub_task = JsonConvert.DeserializeObject<List<PcaSubTask>>(i.detail),
                                                 task_name = i.task_name,
                                                 quantity = i.quantity,
                                                 unit = i.unit,
                                                 price = i.price,
                                                 pph = i.pph,
                                                 ppn = i.ppn,
                                                 asf = i.asf,
                                                 cost = i.cost,
                                                 percentage = i.percentage,
                                                 margin = i.margin,
                                                 total = i.total
                                                    
                                             }).ToList(),
                                             name = tb1.name,
                                             
                                             revision = tb1.revision,
                                             total = tb1.total,
                                             currency = tb1.currency,
                                             currency_name = tb3.currency_name,
                                             rate_card_id = tb5.Id,
                                             
                                             rate_card_name = tb5.name,
                                             mother_pca_id = mpca.Id,
                                             mother_pca_name = mpca.name,
                                             other_fee=tb1.other_fee,
                                             other_fee_name=tb1.other_fee_name,
                                             other_fee_percentage=tb1.other_fee_percentage,
                                             status=tb1.status,
                                             subtotal=tb1.subtotal,
                                             vat=tb1.vat,
                                             type_of_expense_id = tb1.type_of_expense_id,
                                             type_of_expense_name = tb6.name,
                                             mainservice_category_id =tb1.mainservice_category_id,
                                             mainservice_name = tb8.name,
                                             shareservices_id = tb1.shareservices_id,
                                             shareservice_name = tb7.shareservice_name,
                                             termofpayment1=tb1.termofpayment1,
                                             termofpayment2=tb1.termofpayment2,
                                             termofpayment3=tb1.termofpayment3
                
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

        

        public GeneralResponseList<PcaDto> ListAll(PcaSearch filter)
        {

            GeneralResponseList<PcaDto> resp = new GeneralResponseList<PcaDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<PcaDto> que = from tb1 in _repo.GetContext().Set<Pca>().Where(i => i.is_deleted == false)
                                         join tb2 in _repo.GetContext().Set<JobDetail>() on tb1.job_id equals tb2.job_number
                                         join tb3 in _repo.GetContext().Set<ClientBrief>() on tb2.client_brief_id equals tb3.Id
                                         join tb4 in _repo.GetContext().Set<CompanyBrand>() on tb3.external_brand_id equals tb4.Id
                                         join tb4a in _repo.GetContext().Set<Company>() on tb4.company_id equals tb4a.Id
                                         join tb5 in _repo.GetContext().Set<RateCard>() on tb1.rate_card_id equals tb5.Id into lj2
                                         join tb6 in _repo.GetContext().Set<TrTemplateApproval>().Where(i => i.IsDeleted == false) on tb1.Id equals tb6.RefId into pca
                                         from rcard in lj2.DefaultIfEmpty()
                                         from trTemp in pca.DefaultIfEmpty()
                                         select new PcaDto()
                          {
                              Id = tb1.Id,
                              //description = tb1.description,
                              job_id=tb1.job_id,
                              code=tb1.code,
                              detail= JsonConvert.DeserializeObject<List<PcaTaskDto>>(tb1.detail),
                              name=tb1.name,
                              status=tb1.status,
                              revision=tb1.revision,
                              StatusApproved = trTemp.StatusApproved.ToString(),
                              //StatusApproved = _repo.GetContext().Set<TrTemplateApproval>().Where(i => i.RefId == tb1.Id).ToString(),
                              total=tb1.total,
                              currency=tb1.currency,
                              job_name=tb2.job_name,
                              company_name = tb4a.company_name,
                              brand_name=tb4.brand_name,
                              rate_card_id=tb1.rate_card_id,
                              rate_card_name=rcard.name,
                              mainservice_category_id = tb1.mainservice_category_id,
                                             shareservices_id = tb1.shareservices_id,
                                             termofpayment1 = tb1.termofpayment1,
                                             termofpayment2 = tb1.termofpayment2,
                                             termofpayment3 = tb1.termofpayment3
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

        

        public GeneralResponseList<AccountDto> ListAccount(SearchParameter filter)
        {
            GeneralResponseList<AccountDto> resp = new GeneralResponseList<AccountDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<AccountDto> que = from tb1 in _repo.GetContext().Set<Account>()
                                             where tb1.account_level == 5
                                             select new AccountDto()
                                             {
                                                 account_group_id = tb1.account_group_id,
                                                 account_name = tb1.account_name,
                                                 account_number = tb1.account_number,
                                                 account_level = tb1.account_level,
                                                 account_parent_id = tb1.account_parent_id,
                                                 cost_sharing_allocation_id = tb1.cost_sharing_allocation_id,
                                                 organization_id = tb1.organization_id

                                             };

                resp.RecordsTotal = que.Count();

                if (filter.search != null)
                {
                    que = que.Where(w => w.account_number.Contains(filter.search) || w.account_name.Contains(filter.search));
                }

                resp.RecordsFiltered = que.Count();
                resp.Total = resp.RecordsTotal;

                que = q.sortir(que, "account_number", filter.sortBy, filter.sortDir);
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

