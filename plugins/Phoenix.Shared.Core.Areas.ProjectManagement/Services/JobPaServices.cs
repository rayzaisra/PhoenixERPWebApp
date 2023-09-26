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
    public partial interface IJobPaServices : IBaseService<JobPaDto, JobPa, string>
    {
        GeneralResponseList<PaDetailDto> Managesubtask(List<PaDetailDto> data);
        GeneralResponseList<PaTaskDto> Managetask(List<PaTaskDto> data);
        BaseGeneralResponse ManagePca(PcaForm data);
        BaseGeneralResponse managemotherpca(mpcaform data); 
        GeneralResponseList<AccountDto> ListAccount(SearchParameter filter);
        GeneralResponseList<JobPaDto> ListAll(JobPaSearch filter);
        GeneralResponse<JobDetailPca> GetPCAByJobNumber(string jobnumber);
        GeneralResponse<JobDetailPca> GetPCAByPaNumber(string jobnumber);
        GeneralResponse<mpca> GetMotherPCAByCampaignNumber(string CampaignNumber);
    }
    public partial class JobPaServices : BaseService<JobPaDto, JobPa, string>, IJobPaServices
    {
        CommonHelper _commonHelper = new CommonHelper();
        IEFRepository<PaDetail, string> _repodetil;
        IEFRepository<SubTask, string> _reposubtask;
        IEFRepository<PaTask, string> _repotask;
        IEFRepository<MotherPca, string> _repompca;
        public JobPaServices(IEFRepository<JobPa, string> repo,
            IEFRepository<PaDetail, string> repodetil,
            IEFRepository<PaTask, string> repotask,
            IEFRepository<SubTask, string> reposubtask,
            IEFRepository<MotherPca, string> repompca) : base(repo)
        {
            _repo = repo;
            _repodetil = repodetil;
            _repotask = repotask;
            _reposubtask = reposubtask;
            _repompca = repompca;
        }

        public GeneralResponse<mpca> GetMotherPCAByCampaignNumber(string CampaignNumber)
        {
            GeneralResponse<mpca> resp = new GeneralResponse<mpca>();
            try
            {
                IQueryable<mpca> que = from tb1 in _repo.GetContext().Set<ClientBrief>()
                                                   join tb2 in _repo.GetContext().Set<vClientBriefList>() on tb1.Id equals tb2.Id
                                                   join tb3 in _repo.GetContext().Set<CompanyBrand>() on tb1.external_brand_id equals tb3.Id
                                                   join tb4 in _repo.GetContext().Set<BussinessType>() on tb1.bussiness_type_id equals tb4.Id
                                                   join tb5 in _repo.GetContext().Set<BusinessUnit>() on tb1.business_unit_id equals tb5.Id
                                                   join tb6 in _repo.GetContext().Set<CampaignType>() on tb1.campaign_type_id equals tb6.Id
                                                   where tb1.Id == CampaignNumber
                                                   select new mpca()
                                                   {
                                                       Id = tb1.Id,
                                                       assigment_id = tb1.assigment_id,
                                                       business_unit_id = tb1.business_unit_id,
                                                       bussiness_type_id = tb1.bussiness_type_id,
                                                       campaign_progress_id = tb1.campaign_progress_id,
                                                       campaign_status_id = tb1.campaign_status_id,
                                                       campaign_type_id = tb1.campaign_type_id,
                                                       client_brief_file_id = tb1.client_brief_file_id,
                                                       bussiness_type_name = tb4.propose_type,
                                                       campaign_type_name = tb6.campaign_type,
                                                       deadline = tb1.deadline,
                                                       external_brand_id = tb1.external_brand_id,
                                                       final_delivery = tb1.final_delivery,
                                                       finish_campaign = tb1.finish_campaign,
                                                       job_category_id = tb1.job_category_id,
                                                       job_pe_id = tb1.job_pe_id,
                                                       start_campaign = tb1.start_campaign,
                                                       client_brief_id=tb1.Id,
                                                      
                                                       job_detil = (from a in _repo.GetContext().Set<JobDetail>()
                                                                    join b in _repo.GetContext().Set<JobStatus>()
                                                                    on a.job_status_id equals b.Id
                                                                    where a.client_brief_id == tb1.Id
                                                                    select new JobDetailDto()
                                                                    {
                                                                        Id = a.Id,
                                                                        job_number = a.job_number,
                                                                        job_description = a.job_description,
                                                                        job_name = a.job_name,
                                                                        cash_advance = a.cash_advance,
                                                                        client_brief_id = a.client_brief_id,
                                                                        company_id = a.company_id,
                                                                        deadline = a.deadline,
                                                                        final_delivery = a.final_delivery,
                                                                        job_status_detail = a.job_status_detail,
                                                                        job_status_id = a.job_status_id,
                                                                        pa = a.pa,
                                                                        start_job = a.start_job,
                                                                        finish_job = a.finish_job,
                                                                        pe = a.pe,
                                                                        pm_id = a.pm_id,
                                                                        job_status_name = b.name
                                                                    }).ToList(),
                                                       account_management = (from a in _repo.GetContext().Set<BrandAccount>()
                                                                             join b in _repo.GetContext().Set<ApplicationUser>() on a.user_id equals b.Id
                                                                             where a.company_brand_id == tb1.external_brand_id
                                                                             select new AccountManagementList()
                                                                             {
                                                                                 app_fullname = b.app_fullname,
                                                                                 Id = a.Id,
                                                                                 user_id = b.Id,
                                                                                 app_username = b.app_username
                                                                             }).ToList(),
                                                       content = (from a in _repo.GetContext().Set<ClientBriefContent>()
                                                                  where a.client_brief_id == tb1.Id
                                                                  select new ClientBriefContentDto()
                                                                  {
                                                                      client_brief_id = a.client_brief_id,
                                                                      Id = a.Id,
                                                                      description = a.description,
                                                                      file = a.file,
                                                                      name = a.name
                                                                  }).ToList(),
                                                       brand_name = tb3.brand_name,
                                                       campaign_name = tb1.campaign_name,
                                                       business_unit_name = tb5.unit_name,
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

        public GeneralResponse<JobDetailPca> GetPCAByJobNumber(string jobnumber)
        {
            GeneralResponse<JobDetailPca> resp = new GeneralResponse<JobDetailPca>();
            resp.Success = false;
            try
            {
                IQueryable<JobDetailPca> que = from tb1 in _repo.GetContext().Set<JobDetail>()
                                               join tb2 in _repo.GetContext().Set<ClientBrief>() on tb1.client_brief_id equals tb2.Id
                                               join tb3 in _repo.GetContext().Set<CompanyBrand>() on tb2.external_brand_id equals tb3.Id
                                               join tb4 in _repo.GetContext().Set<Company>() on tb3.company_id equals tb4.Id
                                               join tb5 in _repo.GetContext().Set<JobStatus>() on tb1.job_status_id equals tb5.Id
                                               join tb6 in _repo.GetContext().Set<BusinessUnit>() on tb2.business_unit_id equals tb6.Id
                                               join tb7 in _repo.GetContext().Set<ApplicationUser>() on tb1.pm_id equals tb7.Id into lj1
                                               from pmuser in lj1.DefaultIfEmpty()
                                               join tb8 in _repo.GetContext().Set<vClientBriefList>() on tb2.Id equals tb8.Id
                                               join tb9 in _repo.GetContext().Set<JobPa>() on tb1.Id equals tb9.job_detail_id into lj2
                                               from pca in lj2.DefaultIfEmpty()
                                               where tb1.job_number == jobnumber
                                               select new JobDetailPca()
                                               {
                                                   Id = tb1.Id,
                                                   job_number = tb1.job_number,
                                                   job_name = tb1.job_name,
                                                   deadline = tb1.deadline,
                                                   final_delivery = tb1.final_delivery,
                                                   client_brief_id = tb1.client_brief_id,
                                                   campaign_name = tb2.campaign_name,
                                                   job_status_id = tb1.job_status_id,
                                                   job_description = tb1.job_description,
                                                   company_name = tb4.company_name,
                                                   company_id = tb4.Id,
                                                   brand_name = tb3.brand_name,
                                                   brand_id = tb3.Id,
                                                   status_name = tb5.name,
                                                   business_unit_name = tb6.unit_name,
                                                   pm_id = tb1.pm_id,
                                                   pm = pmuser.app_fullname,
                                                   feetype = pca.feetype,
                                                   fee_percentage=pca.fee_percentage,
                                                   feetotal = pca.feetotal,
                                                   currency=pca.currency,
                                                   department = tb6.unit_name,
                                                   approval_id = pca.approval_id,
                                                   hour_per_day = pca.hour_per_day,
                                                   job_detail_id = tb1.Id,
                                                   mother_pca = pca.mother_pca,
                                                   pa_date = pca.pa_date,
                                                   pa_number = pca.pa_number,
                                                   rate_card_id = pca.rate_card_id,
                                                   
                                                   Pcatask = (from a in _repo.GetContext().Set<Task>()
                                                               join b in _repo.GetContext().Set<CentralResource>() on a.central_resources_id equals b.Id
                                                               join c in _repo.GetContext().Set<PaTask>() on a.Id equals c.task_id into c1
                                                               from cx in c1.DefaultIfEmpty()
                                                               join d in _repo.GetContext().Set<vPcatask>() on a.Id equals d.Id into d1
                                                               from dx in d1.DefaultIfEmpty()
                                                               where a.job_number == tb1.job_number
                                                               select new PaTaskList()
                                                               {
                                                                   Id = a.Id,
                                                                   task_name = a.task_name,
                                                                   total = dx.total,
                                                                   margin= cx != null ? cx.margin :null,
                                                                   central_resources_name = b.name,
                                                                   status=cx!=null? cx.status:false,
                                                                   job_detail_id=cx.job_detail_id,
                                                                   task_id=a.Id,
                                                                   PcaDetil = (from st in _repo.GetContext().Set<SubTask>()
                                                                               join st2 in _repo.GetContext().Set<PaDetail>() 
                                                                               on st.Id equals st2.sub_task_id into st2x
                                                                               from stx in st2x.DefaultIfEmpty()
                                                                               where st.task_id == a.Id
                                                                              select new PaDetailDto()
                                                                              {

                                                                                  sub_task_id = st.Id,
                                                                                  sub_task=st.sub_task,
                                                                                  category =stx.category,
                                                                                  category_name=stx.category_name,
                                                                                  asf=stx.asf,
                                                                                  brand=stx.brand,
                                                                                  pph=stx.pph,
                                                                                  ppn=stx.ppn,
                                                                                  price_per_quantity=stx.price_per_quantity,
                                                                                  quantity=stx.quantity,
                                                                                  remarks=stx.remarks,
                                                                                  sub_brand=stx.sub_brand,
                                                                                  uom=stx.uom,
                                                                                  task_id=stx.task_id,
                                                                                  
                                                                              }).ToList(),
                                                               }).ToList(),
                                                   Centralresource = (from a in _repo.GetContext().Set<BrandAccount>()
                                                                      join b in _repo.GetContext().Set<ApplicationUser>() on a.user_id equals b.Id
                                                                      where a.company_brand_id == tb2.external_brand_id
                                                                      select new Centralresourcelist()
                                                                      {
                                                                          Id = b.Id,
                                                                          name = b.app_fullname
                                                                      }).ToList(),
                                                   Accountmanager = _commonHelper.Csvtolist(tb8.account_manager)
                                               };
                resp.Data = que.FirstOrDefault();
                if (resp.Data != null)
                {
                    resp.Success = true;
                }

            }
            catch (Exception ex)
            {

                resp.Message = ex.Message;
            }
            return resp;
        }
        public GeneralResponse<JobDetailPca> GetPCAByPaNumber(string panumber)
        {
            GeneralResponse<JobDetailPca> resp = new GeneralResponse<JobDetailPca>();
            resp.Success = false;
            try
            {
                IQueryable<JobDetailPca> que = from tb1 in _repo.GetContext().Set<JobDetail>()
                                               join tb2 in _repo.GetContext().Set<ClientBrief>() on tb1.client_brief_id equals tb2.Id
                                               join tb3 in _repo.GetContext().Set<CompanyBrand>() on tb2.external_brand_id equals tb3.Id
                                               join tb4 in _repo.GetContext().Set<Company>() on tb3.company_id equals tb4.Id
                                               join tb5 in _repo.GetContext().Set<JobStatus>() on tb1.job_status_id equals tb5.Id
                                               join tb6 in _repo.GetContext().Set<BusinessUnit>() on tb2.business_unit_id equals tb6.Id
                                               join tb7 in _repo.GetContext().Set<ApplicationUser>() on tb1.pm_id equals tb7.Id into lj1
                                               from pmuser in lj1.DefaultIfEmpty()
                                               join tb8 in _repo.GetContext().Set<vClientBriefList>() on tb2.Id equals tb8.Id
                                               join tb9 in _repo.GetContext().Set<JobPa>() on tb1.Id equals tb9.job_detail_id into lj2
                                               from pca in lj2.DefaultIfEmpty()
                                               where pca.pa_number == panumber
                                               select new JobDetailPca()
                                               {
                                                   Id = tb1.Id,
                                                   job_number = tb1.job_number,
                                                   job_name = tb1.job_name,
                                                   deadline = tb1.deadline,
                                                   final_delivery = tb1.final_delivery,
                                                   client_brief_id = tb1.client_brief_id,
                                                   campaign_name = tb2.campaign_name,
                                                   job_status_id = tb1.job_status_id,
                                                   job_description = tb1.job_description,
                                                   company_name = tb4.company_name,
                                                   company_id = tb4.Id,
                                                   brand_name = tb3.brand_name,
                                                   brand_id = tb3.Id,
                                                   status_name = tb5.name,
                                                   business_unit_name = tb6.unit_name,
                                                   pm_id = tb1.pm_id,
                                                   pm = pmuser.app_fullname,
                                                   feetype = pca.feetype,
                                                   fee_percentage = pca.fee_percentage,
                                                   feetotal = pca.feetotal,
                                                   currency = pca.currency,
                                                   department = tb6.unit_name,
                                                   approval_id = pca.approval_id,
                                                   hour_per_day = pca.hour_per_day,
                                                   job_detail_id = tb1.Id,
                                                   mother_pca = pca.mother_pca,
                                                   pa_date = pca.pa_date,
                                                   pa_number = pca.pa_number,
                                                   rate_card_id = pca.rate_card_id,
                                                   Pcatask = (from a in _repo.GetContext().Set<Task>()
                                                              join b in _repo.GetContext().Set<CentralResource>() on a.central_resources_id equals b.Id
                                                              join c in _repo.GetContext().Set<PaTask>() on a.Id equals c.task_id into c1
                                                              from cx in c1.DefaultIfEmpty()
                                                              join d in _repo.GetContext().Set<vPcatask>() on a.Id equals d.Id into d1
                                                              from dx in d1.DefaultIfEmpty()
                                                              where a.job_number == tb1.job_number
                                                              select new PaTaskList()
                                                              {
                                                                  Id = a.Id,
                                                                  task_name = a.task_name,
                                                                  total = dx.total,
                                                                  margin = cx != null ? cx.margin : null,
                                                                  central_resources_name = b.name,
                                                                  status = cx != null ? cx.status : false,
                                                                  job_detail_id = cx.job_detail_id,
                                                                  task_id = a.Id,
                                                                  PcaDetil = (from st in _repo.GetContext().Set<SubTask>()
                                                                              join st2 in _repo.GetContext().Set<PaDetail>()
                                                                              on st.Id equals st2.sub_task_id into st2x
                                                                              from stx in st2x.DefaultIfEmpty()
                                                                              where st.task_id == a.Id
                                                                              select new PaDetailDto()
                                                                              {

                                                                                  sub_task_id = st.Id,
                                                                                  sub_task = st.sub_task,
                                                                                  category = stx.category,
                                                                                  category_name = stx.category_name,
                                                                                  asf = stx.asf,
                                                                                  brand = stx.brand,
                                                                                  pph = stx.pph,
                                                                                  ppn = stx.ppn,
                                                                                  price_per_quantity = stx.price_per_quantity,
                                                                                  quantity = stx.quantity,
                                                                                  remarks = stx.remarks,
                                                                                  sub_brand = stx.sub_brand,
                                                                                  uom = stx.uom,
                                                                                  task_id = stx.task_id,

                                                                              }).ToList(),
                                                              }).ToList(),
                                                   Centralresource = (from a in _repo.GetContext().Set<BrandAccount>()
                                                                      join b in _repo.GetContext().Set<ApplicationUser>() on a.user_id equals b.Id
                                                                      where a.company_brand_id == tb2.external_brand_id
                                                                      select new Centralresourcelist()
                                                                      {
                                                                          Id = b.Id,
                                                                          name = b.app_fullname
                                                                      }).ToList(),
                                                   Accountmanager = _commonHelper.Csvtolist(tb8.account_manager)
                                               };
                resp.Data = que.FirstOrDefault();
                if (resp.Data != null)
                {
                    resp.Success = true;
                }

            }
            catch (Exception ex)
            {

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
                                             where tb1.account_level==5
                                           select new AccountDto()
                                           {
                                                account_group_id=tb1.account_group_id,
                                                account_name=tb1.account_name,
                                                account_number=tb1.account_number,
                                                account_level=tb1.account_level,
                                                account_parent_id=tb1.account_parent_id,
                                                cost_sharing_allocation_id=tb1.cost_sharing_allocation_id,
                                                organization_id=tb1.organization_id
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

        public GeneralResponseList<JobPaDto> ListAll(JobPaSearch filter)
        {

            GeneralResponseList<JobPaDto> resp = new GeneralResponseList<JobPaDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<JobPaDto> que = from tb1 in _repo.GetContext().Set<JobPa>()
                                           join tb2 in _repo.GetContext().Set<JobDetail>() on tb1.job_detail_id equals tb2.Id
                                           join tb3 in _repo.GetContext().Set<ClientBrief>() on tb2.client_brief_id equals tb3.Id
                                           join tb4 in _repo.GetContext().Set<CompanyBrand>() on tb3.external_brand_id equals tb4.Id
                                           join tb5 in _repo.GetContext().Set<Company>() on tb4.company_id equals tb5.Id
                                           select new JobPaDto()
                          {
                                               company_name=tb5.company_name,
                                               campaign_name=tb3.campaign_name,
                                               job_name=tb2.job_name,
                              Id = tb1.Id,
                              approval_id = tb1.approval_id,
                              department = tb1.department,
                              hour_per_day = tb1.hour_per_day,
                              job_detail_id = tb1.job_detail_id,
                              mother_pca = tb1.mother_pca,
                              pa_date = tb1.pa_date,
                              pa_number = tb1.pa_number,
                              rate_card_id = tb1.rate_card_id,
                              currency=tb1.currency,
                              feetype=tb1.feetype,
                              fee_percentage=tb1.fee_percentage
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "approval_id", filter.Search_approval_id);
                que = q.filterEquals(que, "asf", filter.Search_asf);
                que = q.filterContains(que, "department", filter.Search_department);
                que = q.filterContains(que, "detail", filter.Search_detail);
                que = q.filterContains(que, "division", filter.Search_division);
                que = q.filterEquals(que, "hour_per_day", filter.Search_hour_per_day);
                que = q.filterEquals(que, "job_detail_id", filter.Search_job_detail_id);
                que = q.filterEquals(que, "margin", filter.Search_margin);
                que = q.filterContains(que, "mother_pca", filter.Search_mother_pca);
                que = q.filterEquals(que, "pa_cost", filter.Search_pa_cost);
                que = q.filterEquals(que, "pa_date", filter.Search_pa_date);
                que = q.filterContains(que, "pa_number", filter.Search_pa_number);
                que = q.filterEquals(que, "pph", filter.Search_pph);
                que = q.filterEquals(que, "ppn", filter.Search_ppn);
                que = q.filterEquals(que, "rate_card_id", filter.Search_rate_card_id);

                

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

        public BaseGeneralResponse managemotherpca(mpcaform data)
        {
            BaseGeneralResponse resp = new BaseGeneralResponse();
            resp.Success = false;
            try
            {
                MotherPca newdata = _repompca.GetContext().Set<MotherPca>().Where(w => w.code == data.pa_number).FirstOrDefault();
 
                if (newdata == null)
                {
                    MotherPca nu = new MotherPca();
                    PropertyMapper.All(data, nu);
                    _repompca.Create(false,nu);
 
                }
                else
                {
 
                    PropertyMapper.All(data, newdata);
                    _repompca.Update(false, newdata);
                    
                }
                _repo.SaveChanges();
                _repotask.SaveChanges();
                resp.Success = true;
            }
            catch (Exception ex)
            {

                resp.Message = ex.Message;
            }
            return resp;
        }
        public BaseGeneralResponse ManagePca(PcaForm data)
        {
            BaseGeneralResponse resp = new BaseGeneralResponse();
            resp.Success = false;
            try
            {
                JobPa newdata = _repo.GetContext().Set<JobPa>().Where(w => w.job_detail_id == data.job_detail_id).FirstOrDefault();

                if (newdata == null)
                {
                    JobPa nu = new JobPa();
                    PropertyMapper.All(data, nu);
                    _repo.Create(false, nu);
                    foreach (PaTaskMargin taskdata in data.Pcatask)
                    {
                        PaTask newtaskdata = _repotask.GetContext().Set<PaTask>().Where(w => w.task_id == taskdata.task_id).FirstOrDefault();
                        if (newtaskdata == null)
                        {
                            PaTask newtask = new PaTask();
                            PropertyMapper.All(taskdata, newtask);
                            _repotask.Create(false, newtask);
                        }
                        else
                        {
                            PropertyMapper.All(taskdata, newtaskdata);
                            _repotask.Update(false, newtaskdata);
                        }
                    }


                }
                else
                {

                    PropertyMapper.All(data, newdata);
                    _repo.Update(false, newdata);
                    foreach (PaTaskMargin taskdata in data.Pcatask)
                    {
                        PaTask newtaskdata = _repotask.GetContext().Set<PaTask>().Where(w => w.task_id == taskdata.task_id).FirstOrDefault();
                        if (newtaskdata == null)
                        {
                            PaTask newtask = new PaTask();
                            PropertyMapper.All(taskdata, newtask);
                            _repotask.Create(false, newtask);
                        }
                        else
                        {
                            PropertyMapper.All(taskdata, newtaskdata);
                            _repotask.Update(false, newtaskdata);
                        }
                    }
                }
                _repo.SaveChanges();
                _repotask.SaveChanges();
                resp.Success = true;
            }
            catch (Exception ex)
            {

                resp.Message = ex.Message;
            }
            return resp;
        }
        public GeneralResponseList<PaDetailDto> Managesubtask(List<PaDetailDto> data)
        {
            GeneralResponseList<PaDetailDto> resp = new GeneralResponseList<PaDetailDto>();
            try
            {
                var stlist = from m in data group m.task_id by m.task_id into g
                             select new {id=g.Key };
                foreach (var stitem in stlist)
                {
                    IQueryable<Pataskexist> quetask = from a in _repotask.GetContext().Set<Task>()
                                               join c in _repo.GetContext().Set<PaTask>() on a.Id equals c.task_id into c1
                                               from cx in c1.DefaultIfEmpty()
                                               where a.Id == stitem.id
                                               select new Pataskexist()
                                               {
                                                   Id=a.Id,
                                                   patask_taskname= cx.task_name,
                                                   task_name =a.task_name,
                                                   
                                               };
                    Pataskexist existtask = quetask.FirstOrDefault();
                    if (existtask != null)
                    {
                        if (existtask.patask_taskname == null)
                        {
                            PaTask newpatask = new PaTask();
                            newpatask.task_id = existtask.Id;
                            newpatask.status = true;
                            newpatask.task_name = existtask.task_name;
                            _repotask.Create(newpatask);
                        }
                    }
                }
                    foreach (PaDetailDto item in data)
                {
                    if (item.sub_task_id != null && item.sub_task_id.Trim()!="")
                    {
                        IQueryable<PaDetail> que = from tb1 in _repodetil.GetContext().Set<PaDetail>()
                                                   where tb1.sub_task_id == item.sub_task_id
                                                   select tb1;

                        PaDetail olddata= que.FirstOrDefault();
                        
                        if (olddata != null)
                        {
                            PropertyMapper.All(item, olddata);
                            _repodetil.Update(false,olddata);
                            
                        }
                        else
                        {

                            PaDetail newdata = new PaDetail();
                            PropertyMapper.All(item, newdata);
                            _repodetil.Create(false, newdata);
                        }
                    }
                    else
                    {
                        PaDetail newdata = new PaDetail();
                        PropertyMapper.All(item, newdata);

                        SubTask newsubtask = new SubTask();
                        newsubtask.sub_task = item.sub_task;
                        newsubtask.task_id = item.task_id;
                        _reposubtask.Create(false, newsubtask);
                        newdata.sub_task_id = newsubtask.Id;
                        _repodetil.Create(false, newdata);
                    }

                }
                _reposubtask.SaveChanges();
                _repodetil.SaveChanges();

                resp.Rows=data;
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }

        public GeneralResponseList<PaTaskDto> Managetask(List<PaTaskDto> data)
        {
            GeneralResponseList<PaTaskDto> resp = new GeneralResponseList<PaTaskDto>();
            try
            {
                foreach (PaTaskDto item in data)
                {
                    PaTask newdata = new PaTask();
                    PropertyMapper.All(item, newdata);
                    if (item.task_id != null)
                    {
                        IQueryable<PaTask> que = from tb1 in _repodetil.GetContext().Set<PaTask>()
                                                   where tb1.task_id == item.task_id
                                                   select new PaTask();

                        PaTask olddata = que.FirstOrDefault();
                        if (olddata != null)
                        {
                            _repotask.Update(newdata);

                        }
                        else
                        {
                            _repotask.Create(newdata);
                        }
                    }
                    else
                    {
                        _repotask.Create(newdata);
                    }
                    item.Id = newdata.Id;
                }
                resp.Rows = data;
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

