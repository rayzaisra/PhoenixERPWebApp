using Phoenix.Shared.Core.Areas.ProjectManagement.Dtos;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Parameters;
using Phoenix.Shared.Core.Repositories;
using Phoenix.Shared.Core.Services;
using Phoenix.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Phoenix.Shared.Core.Areas.ProjectManagement.Services
{
    public partial interface ICompanyBrandServices : IBaseService<CompanyBrandDto, CompanyBrand, string>
    {
        GeneralResponseList<CompanyBrandList> ListAll(CompanyBrandSearch filter);
        GeneralResponse<CompanyBrandList> GetBrand(String id);
        GeneralResponse<CompanyBrandWithTeamList> GetBrandWithTeam(String id);
        bool managebrandaccount(String id, List<string> brand_account);
        bool managebrandteam(CompanyBrandTeam data);
        GeneralResponseList<CompanyList> CompanyListAll(SearchParameter filter);
        GeneralResponseList<AccountList> AccountList(SearchParameter filter);
        GeneralResponseList<DivisionList> DivisionList(SearchParameter filter);
        GeneralResponseList<MasterDepartmentList> DepartementList(string id);
        GeneralResponseList<AffilateList> AffilateList(SearchParameter filter);
        GeneralResponseList<LegalList> LegalList(SearchParameter filter);
        GeneralResponse<BrandDetailDto> CreateBrand(BrandDetailDto data);
    }
    public partial class CompanyBrandServices : BaseService<CompanyBrandDto, CompanyBrand, string>, ICompanyBrandServices
    {
        IEFRepository<BrandAccount, string> _repoba;
        IEFRepository<TmUserApp, string> _repoaccount;
        IEFRepository<BrandDetail, string> _repobranddetail;
        IEFRepository<Company, string> _repocompany;
        public CompanyBrandServices(IEFRepository<CompanyBrand, string> repo,
            IEFRepository<BrandAccount, string> repoba,
            IEFRepository<TmUserApp, string> repoaccount, IEFRepository<BrandDetail, string>repobranddetail,
            IEFRepository<Company, string>repocompany) : base(repo)
        {
            _repo = repo;
            _repoba = repoba;
            _repoaccount = repoaccount;
            _repobranddetail = repobranddetail;
            _repocompany = repocompany;
        }

        public GeneralResponse<BrandDetailDto> CreateBrand(BrandDetailDto data)
        {
            GeneralResponse<BrandDetailDto> resp = new GeneralResponse<BrandDetailDto>();

            try
            {
                

                //var cpy = _repocompany.GetContext().Set<Company>().Where(p => p.Id == data.company_id).FirstOrDefault();
                //var accnt = _repo.GetContext().Set<ApplicationUser>().Where(p => p.Id == data.account_name).FirstOrDefault();

                CompanyBrand bd = new CompanyBrand();

                bd.company_id = data.company_id;
                bd.brand_name = data.brand_name;
                bd.division_id = data.division_id;
                bd.account_name = data.account_name;
                bd.sub_brand = string.Join(",", data.subbrand1, data.subbrand2);
                bd.asf_value = data.asf_fee;

                _repo.Create(bd);

                resp.Data = data;

                resp.Success = true;

            }
            catch(Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;

            }

            return resp;
        } 

        public GeneralResponse<CompanyBrandList> GetBrand(String id)
        {
            GeneralResponse<CompanyBrandList> resp = new GeneralResponse<CompanyBrandList>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<CompanyBrandList> que = from tb1 in _repo.GetContext().Set<CompanyBrand>()
                                                   join tb2 in _repo.GetContext().Set<Company>() on tb1.company_id equals tb2.Id
                                                   where tb1.Id == id
                                                   select new CompanyBrandList()
                                                   {
                                                       Id = tb1.Id,
                                                       brand_name = tb1.brand_name,
                                                       company_id = tb1.company_id,
                                                       company_name = tb2.company_name,
                                                       sub_brand=tb1.sub_brand
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
        public GeneralResponse<CompanyBrandWithTeamList> GetBrandWithTeam(String id)
        {
            GeneralResponse<CompanyBrandWithTeamList> resp = new GeneralResponse<CompanyBrandWithTeamList>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<CompanyBrandWithTeamList> que = from tb1 in _repo.GetContext().Set<CompanyBrand>()
                                                   join tb2 in _repo.GetContext().Set<Company>() on tb1.company_id equals tb2.Id
                                                   where tb1.Id == id
                                                   select new CompanyBrandWithTeamList()
                                                   {
                                                       Id = tb1.Id,
                                                       brand_name = tb1.brand_name,
                                                       company_id = tb1.company_id,
                                                       company_name = tb2.company_name,
                                                       sub_brand = tb1.sub_brand,
                                                       team=(from a in _repo.GetContext().Set<BrandAccount>()
                                                             join b in _repo.GetContext().Set<ApplicationUser>() on a.user_id equals b.Id
                                                             where a.company_brand_id == tb1.Id
                                                             select new AccountManagementWithTeamList()
                                                             {
                                                                 Id = b.Id,
                                                                 user_id = b.Id,
                                                                 app_username = b.app_username,
                                                                 app_fullname = b.app_fullname,
                                                                 act_as=a.act_as
                                                             }).ToList()
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
        public GeneralResponseList<CompanyList> CompanyListAll(SearchParameter filter)
        {
            GeneralResponseList<CompanyList> resp = new GeneralResponseList<CompanyList>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<CompanyList> que = from tb1 in _repo.GetContext().Set<Company>().Where( p => p.is_client == true && p.is_deleted == false)
                                                   select new CompanyList()
                                                   {
                                                       id = tb1.Id,
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

        public GeneralResponseList<AccountList> AccountList(SearchParameter filter)
        {
            GeneralResponseList<AccountList> resp = new GeneralResponseList<AccountList>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<AccountList> que = from tb1 in _repoaccount.GetContext().Set<JobGrade>().Where( p => p.grade_code == "E1" || p.grade_code == "M1" )
                                              join tb2 in _repo.GetContext().Set<EmployeeBasicInfo>() on tb1.Id equals tb2.job_grade_id
                                              select new AccountList()
                                              {
                                                  Id = tb2.Id,
                                                  Name = tb2.name_employee
                                                  
                                              };
                if (filter.search != null)
                {
                    que = que.Where(w => w.Name.Contains(filter.search));
                }
                resp.RecordsTotal = que.Count();

                resp.RecordsFiltered = que.Count();
                resp.Total = resp.RecordsTotal;

                que = q.sortir(que, "AliasName", filter.sortBy, filter.sortDir);
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

        public GeneralResponseList<LegalList> LegalList(SearchParameter filter)
        {
            GeneralResponseList<LegalList> resp = new GeneralResponseList<LegalList>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<LegalList> que = from tb1 in _repo.GetContext().Set<LegalEntity>()
                                              select new LegalList()
                                              {
                                                  Id = tb1.Id,
                                                  Name = tb1.legal_entity_name

                                              };
                if (filter.search != null)
                {
                    que = que.Where(w => w.Name.Contains(filter.search));
                }
                resp.RecordsTotal = que.Count();

                resp.RecordsFiltered = que.Count();
                resp.Total = resp.RecordsTotal;

                que = q.sortir(que, "Name", filter.sortBy, filter.sortDir);
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

       

        public GeneralResponseList<DivisionList> DivisionList(SearchParameter filter)
        {
            GeneralResponseList<DivisionList> resp = new GeneralResponseList<DivisionList>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<DivisionList> que = from tb1 in _repo.GetContext().Set<BusinessUnit>().Where( b => b.business_unit_type_id == "4209D667-98F4-4E21-8A11-DF6C04FBF8BD")
                                              select new DivisionList()
                                              {
                                                  Id = tb1.Id,
                                                  Name = tb1.unit_name,

                                              };
                if (filter.search != null)
                {
                    que = que.Where(w => w.Name.Contains(filter.search));
                }
                resp.RecordsTotal = que.Count();

                resp.RecordsFiltered = que.Count();
                resp.Total = resp.RecordsTotal;

                que = q.sortir(que, "Name", filter.sortBy, filter.sortDir);
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

        public GeneralResponseList<MasterDepartmentList> DepartementList(string id)
        {
            GeneralResponseList<MasterDepartmentList> resp = new GeneralResponseList<MasterDepartmentList>();
            try
            {
                PagingHelper q = new PagingHelper();

      

                IQueryable<MasterDepartmentList> que = from tb1 in _repo.GetContext().Set<BusinessUnit>()
                                                       join tb2 in _repo.GetContext().Set<BusinessUnitType>() on tb1.business_unit_type_id equals tb2.Id
                                                       where tb1.parent_unit == id && tb2.business_unit_level == 700
                                               select new MasterDepartmentList()
                                               {
                                                   id = tb1.Id,
                                                   name = tb1.unit_name,
                                               };

                resp.RecordsTotal = que.Count();

                resp.RecordsFiltered = que.Count();
                resp.Total = resp.RecordsTotal;

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

        public GeneralResponseList<AffilateList> AffilateList(SearchParameter filter)
        {
            GeneralResponseList<AffilateList> resp = new GeneralResponseList<AffilateList>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<AffilateList> que = from tb1 in _repo.GetContext().Set<Affiliation>()
                                               select new AffilateList()
                                               {
                                                   Id = tb1.Id,
                                                   Name = tb1.affiliation_name,

                                               };
                if (filter.search != null)
                {
                    que = que.Where(w => w.Name.Contains(filter.search));
                }
                resp.RecordsTotal = que.Count();

                resp.RecordsFiltered = que.Count();
                resp.Total = resp.RecordsTotal;

                que = q.sortir(que, "Name", filter.sortBy, filter.sortDir);
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

        public GeneralResponseList<CompanyBrandList> ListAll(CompanyBrandSearch filter)
        {

            GeneralResponseList<CompanyBrandList> resp = new GeneralResponseList<CompanyBrandList>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<CompanyBrandList> que = from tb1 in _repo.GetContext().Set<CompanyBrand>()
                                                  join tb2 in _repo.GetContext().Set<Company>() on tb1.company_id equals tb2.Id
                                                  select new CompanyBrandList()
                          {
                              Id = tb1.Id,
                              brand_name = tb1.brand_name,
                              company_id = tb1.company_id,
                              company_name=tb2.company_name,
                              sub_brand=tb1.sub_brand
                          };
                
                resp.RecordsTotal = que.Count();

                que = q.filterContains(que, "brand_name", filter.search);
                que = q.filterContains(que, "company_name", filter.search);
                que = q.filterEquals(que, "company_id", filter.Search_company_id);

                

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
        public bool managebrandteam(CompanyBrandTeam data)
        {
            bool retval = false;
            try
            {
                var delall = _repo.Rawquery("delete from [pm].[brand_account] where company_brand_id='" + data.company_brand_id + "'");
                if (data.team != null)
                {
                    int urutan = 0;
                    foreach (CompanyBrandTeamlist useritem in data.team)
                    {
                        urutan++;
                        BrandAccount xuser = new BrandAccount();
                        xuser.user_id = useritem.id;
                        xuser.company_brand_id = data.company_brand_id;
                        xuser.act_as = useritem.act_as;
                        _repoba.Create(xuser);
                    }
                    retval = true;
                }
            }
            catch (Exception ex)
            {
                retval = false;
            }

            return retval;
        }
        public bool managebrandaccount(string id, List<string> brand_account)
        {
            bool retval = false;
            try
            {
                var delall = _repo.Rawquery("delete from [pm].[brand_account] where company_brand_id='" + id + "'");
                if (brand_account != null)
                {
                    int urutan = 0;
                    foreach (string user_id in brand_account)
                    {
                        urutan++;
                        BrandAccount xuser = new BrandAccount();
                        xuser.user_id = user_id;
                        xuser.company_brand_id = id;
                        xuser.act_as = "am";
                        _repoba.Create(xuser);
                    }
                }
            }
            catch (Exception ex)
            {
                retval = false;
            }
 
            return retval;
        }
    }
}

