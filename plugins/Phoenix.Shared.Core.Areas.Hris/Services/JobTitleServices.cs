using Phoenix.Shared.Core.Areas.General.Dtos;
using Phoenix.Shared.Core.Areas.Hris.Dtos;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Parameters;
using Phoenix.Shared.Core.Repositories;
using Phoenix.Shared.Core.Services;
using Phoenix.Shared.Helpers;
using Phoenix.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Phoenix.Shared.Core.Areas.Hris.Services
{
    public partial interface IJobTitleServices : IBaseService<JobTitleDto, JobTitle, string>
    {
        GeneralResponseList<JobTitleDto> ListAllFilter(JobTitleSearch filter);
        GeneralResponseList<JobTitleDto> ListTitleInsideBu(JobTitleSearch dto);
        GeneralResponse<JobTitleDto> CreateJobTitleParalel(JobTitleDto dto);
        GeneralResponseList<BusinessUnitParent> ListWithParent(BusinessUnitSearch filter);
        GeneralResponseList<BusinessUnitTree> ListWithChild(BusinessUnitSearch filter);
        GeneralResponse<List<BusinessUnitTree>> ListTree(string parent_id = null);
        List<BusinessUnitDto> ListAllNew();
        GeneralResponse<BusinessUnitParent> GetParent(string id);
        GeneralResponseList<BusinessUnitDto> ListFilter(BusinessUnitSearch filter);
    }
    public partial class JobTitleServices : BaseService<JobTitleDto, JobTitle, string>, IJobTitleServices
    {
        IEFRepository<BusinessUnitJobLevel, string> _repoBusinessUnitJobLevel;
        public JobTitleServices(IEFRepository<JobTitle, string> repo, IEFRepository<BusinessUnitJobLevel, string> repoBusinessUnitJobLevel) : base(repo)
        {
            _repo = repo;
            _repoBusinessUnitJobLevel = repoBusinessUnitJobLevel;
        }

        public GeneralResponse<JobTitleDto> CreateJobTitleParalel(JobTitleDto dto)
        {
            GeneralResponse<JobTitleDto> resp = new GeneralResponse<JobTitleDto>();
            try
            {

                JobTitle newJobtitle = new JobTitle();
                newJobtitle.title_code = dto.title_code;
                newJobtitle.title_name = dto.title_name;

                _repo.Create(false, newJobtitle);

                BusinessUnitJobLevel newBusinesUnitJobLevel = new BusinessUnitJobLevel();
                newBusinesUnitJobLevel.business_unit_id = dto.business_unit_id;
                newBusinesUnitJobLevel.job_grade_id = dto.job_grade_id;
                newBusinesUnitJobLevel.job_level_id = dto.job_level_id;
                newBusinesUnitJobLevel.job_title_id = newJobtitle.Id;
                if (Extensions.IsNullOrEmpty(dto.superior))
                {
                    
                    newBusinesUnitJobLevel.parent_id = "0";
                }
                else {
                    newBusinesUnitJobLevel.parent_id = dto.superior;
                }

               
                _repoBusinessUnitJobLevel.Create(false, newBusinesUnitJobLevel);

                _repo.SaveChanges();
                _repoBusinessUnitJobLevel.SaveChanges();
                _repo.Dispose();
                _repoBusinessUnitJobLevel.Dispose();
                resp.Success = true;
                resp.Message = "Sucess";
                resp.Code = "00";

            }
            catch (Exception x)
            {
                resp.Success = false;
                resp.Code = "500";
                resp.Message = x.Message.ToString();
            }

            return resp;
        }

        public GeneralResponseList<JobTitleDto> ListAllFilter(JobTitleSearch filter)
        {

            GeneralResponseList<JobTitleDto> resp = new GeneralResponseList<JobTitleDto>();
            try
            {
                BusinessUnitSearch newfiler = new BusinessUnitSearch();
                newfiler.page = filter.page;
                newfiler.rows = filter.rows;
                newfiler.sortBy = filter.sortBy;
                newfiler.sortDir = filter.sortDir;
                var listfill = ListFilter(newfiler);
                var allorg = ListAllNew();
                PagingHelper q = new PagingHelper();
                IQueryable<JobTitleDto> que = from tb1 in _repo.GetContext().Set<JobTitle>()
                                              orderby tb1.title_name ascending
                                              select new JobTitleDto()
                                              {
                                                  Id = tb1.Id,
                                                  title_code = tb1.title_code,
                                                  title_name = tb1.title_name,
                                                  parent = ListWithParent(newfiler),
                                                  created_on = tb1.created_on
                                              };



                resp.RecordsTotal = que.Count();


                que = q.filterContains(que, "title_code", filter.Search_title_code);
                que = q.filterContains(que, "title_name", filter.Search_title_name);

                

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

        public GeneralResponseList<JobTitleDto> ListTitleInsideBu(JobTitleSearch dto)
        {
            //1be4026b-523e-46f0-9f59-b045ec6ee7a2
           // dto.Search_title_name
            IQueryable<JobTitleDto> que = null;
            GeneralResponseList<JobTitleDto> resp = new GeneralResponseList<JobTitleDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                if (!string.IsNullOrEmpty(dto.groupId))
                {
                    if (dto.groupId == "1be4026b-523e-46f0-9f59-b045ec6ee7a2")
                    {
                        que = (from tb1 in _repo.GetContext().Set<BusinessUnitJobLevel>()
                               join tb2 in _repo.GetContext().Set<JobTitle>() on tb1.job_title_id equals tb2.Id
                               select new JobTitleDto()
                               {
                                   Id = tb2.Id,
                                   title_code = tb2.title_code,
                                   title_name = tb2.title_name,
                                   business_unit_id = tb1.business_unit_id
                               }).Distinct();
                    }else
                    {
                        que = (from tb1 in _repo.GetContext().Set<BusinessUnitJobLevel>()
                               join tb2 in _repo.GetContext().Set<JobTitle>() on tb1.job_title_id equals tb2.Id
                               where tb1.business_unit_division_id == dto.Search_business_unit_id
                               select new JobTitleDto()
                               {
                                   Id = tb2.Id,
                                   title_code = tb2.title_code,
                                   title_name = tb2.title_name,
                                   business_unit_id = tb1.business_unit_id
                               }).Distinct();
                    }
                }else
                {
                    que = (from tb1 in _repo.GetContext().Set<BusinessUnitJobLevel>()
                           join tb2 in _repo.GetContext().Set<JobTitle>() on tb1.job_title_id equals tb2.Id
                           where tb1.business_unit_division_id == dto.Search_business_unit_id
                           select new JobTitleDto()
                           {
                               Id = tb2.Id,
                               title_code = tb2.title_code,
                               title_name = tb2.title_name,
                               business_unit_id = tb1.business_unit_id
                           }).Distinct();

                }

                if (!string.IsNullOrEmpty(dto.Search_title_name))
                    que = q.filterContains(que, "title_name", dto.Search_title_name);

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

        public GeneralResponseList<BusinessUnitDto> ListFilter(BusinessUnitSearch filter)
        {

            GeneralResponseList<BusinessUnitDto> resp = new GeneralResponseList<BusinessUnitDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<BusinessUnitDto> que = from tb1 in _repo.GetContext().Set<BusinessUnitJobLevel>() join
                                                  tb2 in _repo.GetContext().Set<BusinessUnit>() on tb1.business_unit_id equals tb2.Id
                                                  join tb3 in _repo.GetContext().Set<JobTitle>() on tb1.job_title_id equals tb3.Id
                                                  select new BusinessUnitDto()
                                                  {
                                                      Id = tb1.Id,
                                                      business_unit_type_id = tb2.business_unit_type_id,
                                                      default_team = tb2.default_team,
                                                      organization_id = tb2.organization_id,
                                                      parent_unit = tb2.parent_unit,
                                                      unit_address = tb2.unit_address,
                                                      unit_code = tb2.unit_code,
                                                      unit_description = tb2.unit_description,
                                                      unit_email = tb2.unit_email,
                                                      unit_fax = tb2.unit_fax,
                                                      unit_name = tb2.unit_name,
                                                      unit_phone = tb2.unit_phone,
                                                      created_on = tb2.created_on,
                                                      job_title_name = tb3.title_name
                                                      
                                                  };

                resp.RecordsTotal = que.Count();

                que = q.filterEquals(que, "business_unit_type_id", filter.Search_business_unit_type_id);
 
                que = q.filterContains(que, "unit_address", filter.Search_unit_address);
                que = q.filterContains(que, "unit_code", filter.Search_unit_code);
       



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

        public List<BusinessUnitDto> ListAllNew()
        {
            List<BusinessUnitDto> resp = new List<BusinessUnitDto>();

            PagingHelper q = new PagingHelper();
            IQueryable<BusinessUnitDto> que = from tb1 in _repo.GetContext().Set<BusinessUnit>() 
                                              select new BusinessUnitDto()
                                              {
                                                  Id = tb1.Id,
                                                  unit_code = tb1.unit_code,
                                                  business_unit_type_id = tb1.business_unit_type_id,
                                                  unit_name = tb1.unit_name,
                                                  created_on = tb1.created_on,
                                                  parent_unit = tb1.parent_unit,
                                                  unit_description = tb1.unit_description,
                                                  default_team = tb1.default_team,
                                                  organization_id = tb1.organization_id
                                              };

            resp = que.ToList();
            return resp;
        }

        public GeneralResponse<List<BusinessUnitTree>> ListTree(string parent_id = null)
        {
            GeneralResponse<List<BusinessUnitTree>> resp = new GeneralResponse<List<BusinessUnitTree>>();
            try
            {
                resp.Data = GetNodeChildren(ListAllNew(), parent_id);
                resp.Success = true;
                resp.Message = "Sucess";
                resp.Code = "00";
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }

        public GeneralResponse<BusinessUnitParent> GetParent(string id)
        {
            GeneralResponse<BusinessUnitParent> resp = new GeneralResponse<BusinessUnitParent>();
            try
            {

                resp.Data = GetNodeParent(ListAllNew(), id);
                resp.Success = true;

            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }

        public GeneralResponseList<BusinessUnitParent> ListWithParent(BusinessUnitSearch filter)
        {
            GeneralResponseList<BusinessUnitParent> resp = new GeneralResponseList<BusinessUnitParent>();
            try
            {
                PagingHelper q = new PagingHelper();
                var listfill = ListFilter(filter);
                var allorg = ListAllNew();


                resp.Rows = listfill.Rows.Select(tb1 => new BusinessUnitParent()
                {
                    Id = tb1.Id,
                    unit_code = tb1.unit_code,
                    business_unit_type_id = tb1.business_unit_type_id,
                    unit_name = tb1.unit_name,
                    parent_unit = tb1.parent_unit,
                    created_on = tb1.created_on,
                    default_team = tb1.default_team,
                    organization_id = tb1.organization_id,
                    unit_description = tb1.unit_description,
                    job_title_name = tb1.job_title_name,
                    parent = GetNodeParent(allorg, tb1.parent_unit)
                }).ToList();

                resp.RecordsFiltered = resp.Rows.Count();
                resp.RecordsTotal = resp.Rows.Count();
                resp.Total = resp.RecordsTotal;

                resp.Success = true;

            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }

        public GeneralResponseList<BusinessUnitTree> ListWithChild(BusinessUnitSearch filter)
        {
            GeneralResponseList<BusinessUnitTree> resp = new GeneralResponseList<BusinessUnitTree>();
            try
            {
                PagingHelper q = new PagingHelper();
                var listfill = ListFilter(filter);
                var allorg = ListAllNew();


                resp.Rows = listfill.Rows.Select(tb1 => new BusinessUnitTree()
                {
                    Id = tb1.Id,
                    unit_code = tb1.unit_code,
                    business_unit_type_id = tb1.business_unit_type_id,
                    unit_name = tb1.unit_name,
                    parent_unit = tb1.parent_unit,
                    created_on = tb1.created_on,
                    default_team = tb1.default_team,
                    organization_id = tb1.organization_id,
                    unit_description = tb1.unit_description,
                    children = GetNodeChildren(allorg, tb1.parent_unit)
                }).ToList();

                resp.RecordsFiltered = resp.Rows.Count();
                resp.RecordsTotal = resp.Rows.Count();
                resp.Total = resp.RecordsTotal;

                resp.Success = true;

            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }


        #region "Private Method"
        private BusinessUnitParent GetNodeParent(List<BusinessUnitDto> list, string id)
        {
            return list.Where(x => x.Id == id).Select(x => new BusinessUnitParent
            {
                Id = x.Id,
                unit_name = x.unit_name,
                unit_code = x.unit_code,
                business_unit_type_id = x.business_unit_type_id,
                parent_unit = x.parent_unit,
                created_on = x.created_on,
                default_team = x.default_team,
                organization_id = x.organization_id,
                unit_description = x.unit_description,
                parent = GetNodeParent(list, x.parent_unit)
            }).SingleOrDefault();
        }
        private List<BusinessUnitTree> GetNodeChildren(List<BusinessUnitDto> list, string parent)
        {
            return list.Where(x => x.parent_unit == parent).Select(x => new BusinessUnitTree
            {
                Id = x.Id,
                unit_name = x.unit_name,
                unit_code = x.unit_code,
                business_unit_type_id = x.business_unit_type_id,
                parent_unit = x.parent_unit,
                created_on = x.created_on,
                default_team = x.default_team,
                organization_id = x.organization_id,
                unit_description = x.unit_description,
                children = x.parent_unit != x.Id ? GetNodeChildren(list, x.Id) : new List<BusinessUnitTree>()
            }).ToList();


        }

       


        #endregion

    }

}
public static class Extensions
{
    public static bool IsNullOrEmpty(this object obj)
    {
        return obj == null || String.IsNullOrWhiteSpace(obj.ToString());
    }
   
}

