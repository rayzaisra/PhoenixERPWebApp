using Microsoft.EntityFrameworkCore;
using Phoenix.Shared.Core.Areas.ProjectManagement.Dtos;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Parameters;
using Phoenix.Shared.Core.Repositories;
using Phoenix.Shared.Core.Services;
using Phoenix.Shared.Helpers;
using Phoenix.Shared.Libraries.SnowFlake;
using Phoenix.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Services
{
    public partial interface IClientBriefServices : IBaseService<ClientBriefDto, ClientBrief, string>
    {
        GeneralResponse<ClientBriefDetil> GetDetil(string id);
        GeneralResponseList<ClientBriefDto> ListAll(ClientBriefSearch filter);
        GeneralResponse<ClientBriefNew> CreateClientBrief(ClientBriefNew data);
        GeneralResponse<ClientBriefDetil> UpdateClientBrief(ClientBriefUpdate data);
        GeneralResponse<ClientBriefDto> Delete(ClientBriefDto data);
        GeneralResponseList<CompanyDto> CompanyList(SearchParameter data);

        GeneralResponse<ClientBriefDto> CheckUserId();
    }
    public partial class ClientBriefServices : BaseService<ClientBriefDto, ClientBrief, string>, IClientBriefServices
    {
        const int ColumnBase = 26;
        const int DigitMax = 7;
        const string Digits = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        IEFRepository<ClientBriefContent, string> _repocontent;
        IEFRepository<spClientBriefList, string> _repolist;
        IEFRepository<JobDetail, string> _repojob;
        IEFRepository<spjobnumber, string> _repojobnumber;
        IEFRepository<JobAccount, string> _repojobaccount;
        CommonHelper _commonHelper = new CommonHelper();
        readonly Phoenix.Data.DataContext _context;
        public ClientBriefServices(IEFRepository<ClientBrief, string> repo,
            IEFRepository<ClientBriefContent, string> repocontent,
            IEFRepository<JobDetail, string> repojob,
            IEFRepository<spjobnumber, string> repojobnumber,
            IEFRepository<spClientBriefList, string> repolist,
            IEFRepository<JobAccount, string> repojobaccount,
            Phoenix.Data.DataContext context) : base(repo)
        {
            _repo = repo;
            _repojobnumber = repojobnumber;
            _repocontent = repocontent;
            _context = context;
            _repojob = repojob;

            _repojobaccount = repojobaccount;

            _repolist = repolist;
        }

        public GeneralResponse<ClientBriefDto> CheckUserId() {
            GeneralResponse<ClientBriefDto> resp = new GeneralResponse<ClientBriefDto>();
            try
            {
                var subGroupId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessSubGroup(_context.HttpContext);
                var divisionId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessUnitDivisi(_context.HttpContext);
                var employeeId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(_context.HttpContext);


                if (string.IsNullOrEmpty(subGroupId) || string.IsNullOrEmpty(divisionId))
                {
                    resp.Message = "this employee doesn't have a sub group or division, please check this employee data again.";
                    resp.Success = false;
                }
                else
                {
                    resp.Message = "this employee can create client brief.";
                    resp.Success = true;
                }
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }
        public GeneralResponse<ClientBriefNew> CreateClientBrief(ClientBriefNew cbdata)
        {
            GeneralResponse<ClientBriefNew> resp = new GeneralResponse<ClientBriefNew>();
            try
            {
                if (cbdata.final_delivery != null && cbdata.deadline != null && cbdata.start_campaign != null)
                {
                    DateTime dt1 = cbdata.start_campaign ?? DateTime.Now;
                    DateTime dt2 = cbdata.deadline ?? DateTime.Now.AddDays(15);
                    DateTime dt3 = cbdata.final_delivery ?? DateTime.Now.AddMonths(1);
                    int resultFinalDelivery = DateTime.Compare(dt3, dt2);
                    int resultDeadline = DateTime.Compare(dt2, dt1);

                    if (resultDeadline < 0)
                    {
                        resp.Success = false;
                        resp.Message = "Deadline date is older than start campaign date";
                    }
                    else
                    {
                        if (resultFinalDelivery < 0)
                        {
                            resp.Success = false;
                            resp.Message = "Final delivery date is older than deadline date";
                        }
                        else
                        {
                            cbdata.Id = null;
                            string campaign_id = getCampaigncode(cbdata.business_unit_id);
                            ClientBrief newData = new ClientBrief();
                            PropertyMapper.All(cbdata, newData);
                            newData.Id = campaign_id;



                            _repo.Create(false, newData);
                            if (cbdata.content != null)
                            {
                                foreach (ClientBriefContentNew contentdata in cbdata.content)
                                {
                                    ClientBriefContent newContentData = new ClientBriefContent();
                                    contentdata.client_brief_id = newData.Id;
                                    PropertyMapper.All(contentdata, newContentData);
                                    _repocontent.Create(false, newContentData);
                                }


                            }


                            _repo.SaveChanges();
                            _repocontent.SaveChanges();

                            resp.Success = true;
                            cbdata.Id = newData.Id;
                            resp.Message = "Data has been saved successfully.";
                        }
                    }

                }
                else
                {
                    resp.Success = false;
                    resp.Message = "data is missing";
                }

            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }

            resp.Data = cbdata;
            _repo.Dispose();
            _repojob.Dispose();
            _repocontent.Dispose();
            return resp;
        }
        public GeneralResponse<ClientBriefDetil> UpdateClientBrief(ClientBriefUpdate cbdata)
        {
            GeneralResponse<ClientBriefDetil> resp = new GeneralResponse<ClientBriefDetil>();
            try
            {

                ClientBrief newData = _repo.FindByID(cbdata.Id);
                if (newData != null)
                {
                    if (cbdata.final_delivery != null && cbdata.deadline != null && cbdata.start_campaign != null)
                    {
                        DateTime dt1 = cbdata.start_campaign ?? DateTime.Now;
                        DateTime dt2 = cbdata.deadline ?? DateTime.Now.AddDays(15);
                        DateTime dt3 = cbdata.final_delivery ?? DateTime.Now.AddMonths(1);
                        int resultFinalDelivery = DateTime.Compare(dt3, dt2);
                        int resultDeadline = DateTime.Compare(dt2, dt1);

                        if (resultDeadline < 0)
                        {
                            resp.Success = false;
                            resp.Message = "Deadline date is older than start campaign date";
                        }
                        else
                        {
                            if (resultFinalDelivery < 0)
                            {
                                resp.Success = false;
                                resp.Message = "Final delivery date is older than deadline date";
                            }
                            else
                            {
                                PropertyMapper.All(cbdata, newData);
                                _repo.Update(false, newData);
                                if (cbdata.content != null)
                                {
                                    List<string> contentid = new List<string>();
                                    foreach (ClientBriefContentNew contentdata in cbdata.content)
                                    {
                                        if (contentdata.id != null)
                                        {
                                            contentid.Add(contentdata.id);
                                        }

                                    }
                                    List<ClientBriefContent> kickoutteam = (from a in _repo.GetContext().Set<ClientBriefContent>()
                                                                            where a.client_brief_id == cbdata.Id
                                                                            && !contentid.Contains(a.Id)
                                                                            select new ClientBriefContent()
                                                                            {
                                                                                Id = a.Id
                                                                            }).ToList();
                                    foreach (ClientBriefContent itemz in kickoutteam)
                                    {
                                        _repocontent.RemoveByID(false, itemz.Id);
                                    }
                                    foreach (ClientBriefContentNew contentdata in cbdata.content)
                                    {
                                        if (contentdata.id != null)
                                        {
                                            ClientBriefContent newContentData = _repocontent.FindByID(contentdata.id);
                                            if (newContentData != null)
                                            {
                                                if (contentdata.file.Contains("::"))
                                                {
                                                    string[] data = contentdata.file.Split(new string[] { "::" }, StringSplitOptions.None);
                                                    contentdata.file = data[0];
                                                }

                                                contentdata.client_brief_id = newData.Id;
                                                PropertyMapper.All(contentdata, newContentData);
                                                _repocontent.Update(false, newContentData);
                                            }

                                        }
                                        else
                                        {
                                            ClientBriefContent newContentData = new ClientBriefContent();
                                            contentdata.client_brief_id = newData.Id;
                                            PropertyMapper.All(contentdata, newContentData);
                                            _repocontent.Create(false, newContentData);
                                        }

                                    }
                                }

                                _repo.SaveChanges();
                                _repojob.SaveChanges();
                                _repocontent.SaveChanges();

                                resp.Success = true;
                                resp.Message = "Data has been updated successfully.";
                            }
                        }

                       
                    }
                    else
                    {
                        resp.Success = false;
                        resp.Message = "data is missing";
                    }



                }
                else
                {
                    resp.Success = false;
                    resp.Message = "data not found";
                }

            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }

            _repo.Dispose();
            _repojob.Dispose();
            _repocontent.Dispose();
            return resp;
        }
        public static string itvar(int index)
        {
            if (index <= 0)
                throw new IndexOutOfRangeException("index must be a positive number");

            if (index <= ColumnBase)
                return Digits[index - 1].ToString();

            var sb = new StringBuilder().Append(' ', DigitMax);
            var current = index;
            var offset = DigitMax;
            while (current > 0)
            {
                sb[--offset] = Digits[--current % ColumnBase];
                current /= ColumnBase;
            }
            return sb.ToString(offset, DigitMax - offset);
        }
        public string getCampaigncode(string seqid)
        {
            var refid = new SqlParameter("@refid", seqid);
            var result = _repojobnumber.GetEntities().FromSql("[pm].[spGetCampaignCode] @refid", refid).FirstOrDefault();
            return result.Id;
        }
        public string Nextjobnumber(string jobnumber)
        {
            long xjn = long.Parse(jobnumber.Substring(jobnumber.Length - 6));
            xjn = xjn + 1;
            var result = jobnumber.Substring(0, jobnumber.Length - 6) + String.Format("{0:000000}", xjn); ;
            return result;
        }

        public GeneralResponseList<ClientBriefDto> ListAllx(ClientBriefSearch filter)
        {
            GeneralResponseList<ClientBriefDto> resp = new GeneralResponseList<ClientBriefDto>();
            try
            {
                var PageNumber = new SqlParameter("@PageNumber", filter.page);
                var PageSize = new SqlParameter("@PageSize", filter.rows);
                var x = _repolist.Rawlist("pm.spClientBriefList @PageNumber", PageNumber);

                resp.Success = false;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;

        }

        public GeneralResponseList<CompanyDto> CompanyList(SearchParameter filter)
        {
            GeneralResponseList<CompanyDto> resp = new GeneralResponseList<CompanyDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<CompanyDto> que = from tb1 in _repo.GetContext().Set<Company>().Where(p => p.is_client == true)
                                             select new CompanyDto()
                                             {
                                                 Id = tb1.Id,
                                                 company_name = tb1.company_name
                                             };

                if (filter.search != null)
                {
                    que = que.Where(w => w.company_name.Contains(filter.search));
                }
                resp.RecordsTotal = que.Count();

                resp.RecordsFiltered = que.Count();
                resp.Total = resp.RecordsTotal;

                que = q.sortir(que, "company_name", filter.sortBy, filter.sortDir);
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

        public GeneralResponseList<ClientBriefDto> ListAll(ClientBriefSearch filter)
        {

            GeneralResponseList<ClientBriefDto> resp = new GeneralResponseList<ClientBriefDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<ClientBriefDto> que = from tb1 in _repo.GetContext().Set<ClientBrief>()
                                                 join tb2 in _repo.GetContext().Set<vClientBriefList>() on tb1.Id equals tb2.Id
                                                 //join tb3 in _repo.GetContext().Set<CompanyBrand>() on tb1.external_brand_id equals tb3.Id di comment dulu untuk update hiding combo box brand name
                                                 join tb5 in _repo.GetContext().Set<BusinessUnit>() on tb1.business_unit_id equals tb5.Id
                                                 //where tb1.created_by == Glosing.Instance.EmployeeId
                                                 select new ClientBriefDto()
                                                 {
                                                     Id = tb1.Id,
                                                     job_id = tb2.Id,
                                                     assigment_id = tb1.assigment_id,
                                                     business_unit_id = tb1.business_unit_id,
                                                     bussiness_type_id = tb1.bussiness_type_id,
                                                     campaign_progress_id = tb1.campaign_progress_id,
                                                     campaign_status_id = tb1.campaign_status_id,
                                                     campaign_type_id = tb1.campaign_type_id,
                                                     client_brief_file_id = tb1.client_brief_file_id,
                                                     deadline = tb1.deadline,
                                                     external_brand_id = tb1.external_brand_id,
                                                     final_delivery = tb1.final_delivery,
                                                     finish_campaign = tb1.finish_campaign,
                                                     job_category_id = tb1.job_category_id,
                                                     job_pe_id = tb1.job_pe_id,
                                                     nationality_id = tb1.nationality_id,
                                                     proposal_file_id = tb1.proposal_file_id,
                                                     responsible_person = tb1.responsible_person,
                                                     start_campaign = tb1.start_campaign,
                                                     job_name = _commonHelper.Csvtolist(tb2.job_name),
                                                     job_number = _commonHelper.Csvtolist(tb2.job_number),
                                                     //brand_name=tb3.brand_name, di comment dulu untuk update hiding combo box brand name
                                                     campaign_name = tb1.campaign_name,
                                                     business_unit_name = tb5.unit_name,
                                                     is_deleted = tb1.is_deleted,
                                                     created_by = tb1.created_by

                                                 };

                if (filter.group_id != Phoenix.Data.Models.GroupAcess.SuperAdmin)
                {
                    que = que.Where(x => x.created_by == Glosing.Instance.EmployeeId);
                }
                //var ListClientBrief = (from tb1 in _repo.GetContext().Set<Team>()
                //                       join tb2 in _repo.GetContext().Set<TeamMember>() on tb1.Id equals tb2.team_id
                //                       where tb2.application_user_id == Glosing.Instance.Username && tb1.is_template == false
                //                       select tb1.job_id.Substring(0,16)).Distinct().ToList();

                //que = que.Where(x => ListClientBrief.Contains(x.Id));
                resp.RecordsTotal = que.Count();


                //que = q.filterEquals(que, "business_unit_id", filter.Search_business_unit_id);
                //que = q.filterEquals(que, "bussiness_type_id", filter.Search_bussiness_type_id);


                que = q.filterContains(que, "campaign_name", filter.Search_campaign_name);
                if (filter.Search_is_deleted != null)
                    que = q.filterEquals(que, "is_deleted", Convert.ToBoolean(filter.Search_is_deleted));

                resp.RecordsFiltered = que.Count();
                resp.Total = resp.RecordsTotal;

                que = q.sortir(que, "Id", filter.sortBy, filter.sortDir = "desc");
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

        public GeneralResponse<ClientBriefDto> Delete(ClientBriefDto data)
        {
            GeneralResponse<ClientBriefDto> resp = new GeneralResponse<ClientBriefDto>();
            try
            {
                var i = Convert.ToString(data.Id);
                DateTime dt = DateTime.Now;


                //var que = _repo.GetContext().Set<ClientBrief>().Where(p => p.Id == i)
                //                                               .Where(st => dt >= st.start_campaign && dt <= st.deadline).FirstOrDefault();

                var que = _repo.GetContext().Set<JobDetail>().Where(p => p.client_brief_id == data.Id && p.is_deleted == false).FirstOrDefault();

                if (que != null)
                {
                    resp.Success = true;
                    resp.Message = "unassigned";
                }
                else
                {
                    DeleteByID(data.Id);
                    resp.Success = true;
                }



            }
            catch (Exception ex)
            {
                resp.Code = "404";
                resp.Message = ex.Message;
                resp.Success = false;
            }
            return resp;
        }
        public GeneralResponse<ClientBriefDetil> GetDetil(string id)
        {

            GeneralResponse<ClientBriefDetil> resp = new GeneralResponse<ClientBriefDetil>();
            try
            {
                IQueryable<ClientBriefDetil> que = from tb1 in _repo.GetContext().Set<ClientBrief>()
                                                   join tb2 in _repo.GetContext().Set<vClientBriefList>() on tb1.Id equals tb2.Id
                                                   //join tb3 in _repo.GetContext().Set<CompanyBrand>() on tb1.external_brand_id equals tb3.Id di comment dulu untuk update hiding combo box brand name
                                                   join tb3 in _repo.GetContext().Set<Company>() on tb1.client_id equals tb3.Id
                                                   join tb4 in _repo.GetContext().Set<BussinessType>() on tb1.bussiness_type_id equals tb4.Id
                                                   join tb5 in _repo.GetContext().Set<BusinessUnit>() on tb1.business_unit_id equals tb5.Id
                                                   join tb6 in _repo.GetContext().Set<CampaignType>() on tb1.campaign_type_id equals tb6.Id
                                                   where tb1.Id == id
                                                   select new ClientBriefDetil()
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
                                                       final_delivery = tb1.final_delivery,
                                                       finish_campaign = tb1.finish_campaign,
                                                       job_category_id = tb1.job_category_id,
                                                       client_id = tb1.client_id,
                                                       company_name = tb3.company_name,
                                                       job_pe_id = tb1.job_pe_id,
                                                       start_campaign = tb1.start_campaign,
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
                                                       content = (from a in _repo.GetContext().Set<ClientBriefContent>()
                                                                  join b in _repo.GetContext().Set<Filemaster>() on a.file equals b.Id into leftJ
                                                                  from lj in leftJ.DefaultIfEmpty()
                                                                  where a.client_brief_id == tb1.Id
                                                                  select new ClientBriefContentDto()
                                                                  {
                                                                      client_brief_id = a.client_brief_id,
                                                                      Id = a.Id,
                                                                      description = a.description,
                                                                      //file=a.file,
                                                                      file = (a.file + "::" + lj.name),
                                                                      name = a.name
                                                                  }).ToList(),
                                                       //brand_name = tb3.brand_name, di comment dulu untuk update hiding combo box brand name
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
    }
}

