using Phoenix.Shared.Core.Areas.Hris.Dtos;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Parameters;
using Phoenix.Shared.Core.Repositories;
using Phoenix.Shared.Core.Services;
using Phoenix.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Phoenix.Shared.Core.Areas.Hris.Services
{
    public partial interface IBussinesUnitJobLevelServices : IBaseService<BussinesUnitJobLevelDto, BusinessUnitJobLevel, string>
    {
        GeneralResponseList<BussinesUnitJobLevelDto> ListAll(BussinesUnitJobLevelSearch filter);
        GeneralResponseList<BusinessUnitJobLevelChart> Chart(BussinesUnitJobLevelSearch filter);
        GeneralResponseList<BussinesUnitJobLevelDto> ListAllDetail(BussinesUnitJobLevelSearch filter);
        GeneralResponseList<BusinessUnitJobLevelParent> ListWithParentDetail(BussinesUnitJobLevelSearch filter);
        GeneralResponse<List<BusinessUnitJobLevelTree>> ListTree(string parent_id = null);
        List<BussinesUnitJobLevelDto> ListAllNewDetail();
        GeneralResponse<BusinessUnitJobLevelParent> GetParent(string id);
        GeneralResponse<BussinesUnitJobLevelDto> GetDetail(string id);
        GeneralResponseList<BussinesUnitJobLevelDto> ListSupervisor(BussinesUnitJobLevelSearch filter);
    }
    public partial class BussinesUnitJobLevelServices : BaseService<BussinesUnitJobLevelDto, BusinessUnitJobLevel, string>, IBussinesUnitJobLevelServices
    {
        public BussinesUnitJobLevelServices(IEFRepository<BusinessUnitJobLevel, string> repo) : base(repo)
        {
            _repo = repo;
        }

        private BusinessUnitGroup newbug(BusinessUnitGroup businessUnitGroup)
        {
            BusinessUnitGroup x = new BusinessUnitGroup();
            if (businessUnitGroup != null)
            {
                x = businessUnitGroup;
            }
            return x;
        }
        private BusinessUnitSubgroup newbugsub(BusinessUnitSubgroup businessUnitSubGroup)
        {
            BusinessUnitSubgroup x = new BusinessUnitSubgroup();
            if (businessUnitSubGroup != null)
            {
                x = businessUnitSubGroup;
            }
            return x;
        }
        private BusinessUnitDivision newbugDiv(BusinessUnitDivision businessUnitGroupDivision)
        {
            BusinessUnitDivision x = new BusinessUnitDivision();
            if (businessUnitGroupDivision != null)
            {
                x = businessUnitGroupDivision;
            }
            return x;
        }
        private BusinessUnitDepartement newbugDepart(BusinessUnitDepartement businessUnitDepartement)
        {
            BusinessUnitDepartement x = new BusinessUnitDepartement();
            if (businessUnitDepartement != null)
            {
                x = businessUnitDepartement;
            }
            return x;
        }

        private ParentBusinessUnit newBugParent(ParentBusinessUnit param)
        {
            ParentBusinessUnit x = new ParentBusinessUnit();
            if (param != null)
            {
                x = param;
            }
            return x;
        }

        public GeneralResponseList<BussinesUnitJobLevelDto> ListSupervisor(BussinesUnitJobLevelSearch filter)
        {
            GeneralResponseList<BussinesUnitJobLevelDto> resp = new GeneralResponseList<BussinesUnitJobLevelDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                var que = (from tcc in _repo.GetContext().Set<BusinessUnitJobLevel>()
                           join tc1 in _repo.GetContext().Set<JobTitle>() on tcc.job_title_id equals tc1.Id
                           join hr in _repo.GetContext().Set<EmployeeBasicInfo>() on tcc.Id equals hr.business_unit_job_level_id
                           select new BussinesUnitJobLevelDto()
                           {
                               Id = tcc.Id,
                               remarks = tcc.remarks,
                               job_title_name = tc1.title_name,
                               username = hr.name_employee,
                               BusinessUnitDepartement = newbugDepart((from tbu in _repo.GetContext().Set<BusinessUnit>()
                                                                       where tbu.Id == tcc.business_unit_departement_id
                                                                       select new BusinessUnitDepartement()
                                                                       {
                                                                           name = tbu.unit_name,
                                                                           id = tbu.Id
                                                                       }).FirstOrDefault()),
                               BusinessUnitDivision = newbugDiv((from tbu in _repo.GetContext().Set<BusinessUnit>()
                                                                 where tbu.Id == tcc.business_unit_division_id
                                                                 select new BusinessUnitDivision()
                                                                 {
                                                                     name = tbu.unit_name,
                                                                     id = tbu.Id
                                                                 }).FirstOrDefault()),
                               BusinessUnitGroup = newbug((from tbu in _repo.GetContext().Set<BusinessUnit>()
                                                           where tbu.Id == tcc.business_unit_group_id
                                                           select new BusinessUnitGroup()
                                                           {
                                                               name = tbu.unit_name,
                                                               id = tbu.Id
                                                           }).FirstOrDefault()),
                               BusinessUnitSubgroup = newbugsub((from tbu in _repo.GetContext().Set<BusinessUnit>()
                                                                 where tbu.Id == tcc.business_unit_subgroup_id
                                                                 select new BusinessUnitSubgroup()
                                                                 {
                                                                     name = tbu.unit_name,
                                                                     id = tbu.Id
                                                                 }).FirstOrDefault())
                           });

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

        public GeneralResponseList<BussinesUnitJobLevelDto> ListAll(BussinesUnitJobLevelSearch filter)
        {

            GeneralResponseList<BussinesUnitJobLevelDto> resp = new GeneralResponseList<BussinesUnitJobLevelDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<BussinesUnitJobLevelDto> que = from tb1 in _repo.GetContext().Set<BusinessUnitJobLevel>()
                                                          join tb2 in _repo.GetContext().Set<JobTitle>() on tb1.job_title_id equals tb2.Id
                                                          join tb3 in _repo.GetContext().Set<JobLevel>() on tb1.job_level_id equals tb3.Id
                                                          join tb4 in _repo.GetContext().Set<JobGrade>() on tb1.job_grade_id equals tb4.Id
                                                          //join hr in _repo.GetContext().Set<EmployeeBasicInfo>() on tb1.Id equals hr.business_unit_job_level_id into hrj
                                                          //from hrlj in hrj.DefaultIfEmpty()
                                                          orderby tb2.title_name ascending
                                                          where tb1.is_deleted == false
                                                          select new BussinesUnitJobLevelDto()
                                                          {
                                                              Id = tb1.Id,
                                                              remarks = tb1.remarks,
                                                              //username = hrlj.name_employee,
                                                              job_grade_id = tb1.job_grade_id,
                                                              job_level_id = tb1.job_level_id,
                                                              job_title_id = tb1.job_title_id,
                                                              parent_id = tb1.parent_id,
                                                              business_unit_group_id = tb1.business_unit_group_id,
                                                              business_unit_subgroup_id = tb1.business_unit_subgroup_id,
                                                              business_unit_division_id = tb1.business_unit_division_id,
                                                              business_unit_departement_id = tb1.business_unit_departement_id,
                                                              job_grade_name = tb4.grade_name,
                                                              job_title_name = tb2.title_name,
                                                              job_level_name = tb3.job_name,
                                                              BusinessUnitGroup = newbug((from tbu in _repo.GetContext().Set<BusinessUnit>()
                                                                                          where tbu.Id == tb1.business_unit_group_id
                                                                                          select new BusinessUnitGroup()
                                                                                          {
                                                                                              name = tbu.unit_name,
                                                                                              id = tbu.Id
                                                                                          }
                                                                                   ).FirstOrDefault()),
                                                              BusinessUnitSubgroup = newbugsub((from tbu in _repo.GetContext().Set<BusinessUnit>()
                                                                                                where tbu.Id == tb1.business_unit_subgroup_id
                                                                                                select new BusinessUnitSubgroup()
                                                                                                {
                                                                                                    name = tbu.unit_name,
                                                                                                    id = tbu.Id
                                                                                                }
                                                                                   ).FirstOrDefault()),
                                                              BusinessUnitDivision = newbugDiv((from tbu in _repo.GetContext().Set<BusinessUnit>()
                                                                                                where tbu.Id == tb1.business_unit_division_id
                                                                                                select new BusinessUnitDivision()
                                                                                                {
                                                                                                    name = tbu.unit_name,
                                                                                                    id = tbu.Id
                                                                                                }
                                                                                   ).FirstOrDefault()),
                                                              BusinessUnitDepartement = newbugDepart((from tbu in _repo.GetContext().Set<BusinessUnit>()
                                                                                                      where tbu.Id == tb1.business_unit_departement_id
                                                                                                      select new BusinessUnitDepartement()
                                                                                                      {
                                                                                                          name = tbu.unit_name,
                                                                                                          id = tbu.Id
                                                                                                      }
                                                                                   ).FirstOrDefault()),
                                                              created_on = tb1.created_on,
                                                              business_unit_group_name = (from tbu in _repo.GetContext().Set<BusinessUnit>()
                                                                                          where tbu.Id == tb1.business_unit_group_id
                                                                                          select tbu.unit_name).FirstOrDefault(),
                                                              business_unit_subgroup_name = (from tbu in _repo.GetContext().Set<BusinessUnit>()
                                                                                          where tbu.Id == tb1.business_unit_subgroup_id
                                                                                          select tbu.unit_name).FirstOrDefault(),
                                                              business_unit_division_name = (from tbu in _repo.GetContext().Set<BusinessUnit>()
                                                                                          where tbu.Id == tb1.business_unit_division_id
                                                                                          select tbu.unit_name).FirstOrDefault(),
                                                              business_unit_departement_name = (from tbu in _repo.GetContext().Set<BusinessUnit>()
                                                                                          where tbu.Id == tb1.business_unit_departement_id
                                                                                          select tbu.unit_name).FirstOrDefault(),
                                                          };
                resp.RecordsTotal = que.Count();

                //if (filter.Search_business_unit_group_id != null && filter.Search_business_unit_subgroup_id == null && filter.Search_business_unit_division_id == null && filter.Search_business_unit_departement_id == null)
                //{
                //    que = que.Where(w => w.business_unit_group_id.Equals(filter.Search_business_unit_group_id));
                //}
                //else if (filter.Search_business_unit_group_id != null && filter.Search_business_unit_subgroup_id != null)
                //{
                //    que = que.Where(w => w.business_unit_group_id.Equals(filter.Search_business_unit_group_id) && w.business_unit_subgroup_id.Equals(filter.Search_business_unit_subgroup_id) || w.business_unit_subgroup_id.Equals(null));
                //}
                //else if (filter.Search_business_unit_group_id != null && filter.Search_business_unit_subgroup_id != null && filter.Search_business_unit_division_id != null)
                //{
                //    que = que.Where(w => w.business_unit_group_id.Equals(filter.Search_business_unit_group_id) && (w.business_unit_subgroup_id.Equals(filter.Search_business_unit_subgroup_id) || w.business_unit_subgroup_id.Equals(null)) && (w.business_unit_division_id.Equals(filter.Search_business_unit_division_id) || w.business_unit_division_id.Equals(null)));
                //}
                //else if (filter.Search_business_unit_group_id != null && filter.Search_business_unit_subgroup_id != null && filter.Search_business_unit_division_id != null && filter.Search_business_unit_departement_id != null)
                //{
                //    que = que.Where(w => w.business_unit_group_id.Equals(filter.Search_business_unit_group_id) && (w.business_unit_subgroup_id.Equals(filter.Search_business_unit_subgroup_id) || w.business_unit_subgroup_id.Equals(null)) && (w.business_unit_division_id.Equals(filter.Search_business_unit_division_id) || w.business_unit_division_id.Equals(null)) && w.business_unit_departement_id.Equals(filter.Search_business_unit_departement_id));
                //}


                que = q.filterContains(que, "business_unit_group_name", filter.search_group_name);
                que = q.filterContains(que, "business_unit_subgroup_name", filter.search_sub_group_name);
                que = q.filterContains(que, "business_unit_division_name", filter.search_division_name);
                que = q.filterContains(que, "business_unit_departement_name", filter.search_department_name);

                que = q.filterContains(que, "job_title_name", filter.search_title_name);
                que = q.filterContains(que, "remarks", filter.search_remarks);

             
                resp.RecordsFiltered = que.Count();
                resp.Total = resp.RecordsTotal;

                que = q.sortir(que, "Id", filter.sortBy, filter.sortDir);
                que = q.limit(que, filter.GetLimit(), filter.GetOffset());
 
                resp.Rows = que.ToList();
                if (!string.IsNullOrEmpty(filter.search_all))
                {
                    var searchAll = filter.search_all.ToLower();
                    resp.Rows = resp.Rows.Where(x => x.business_unit_group_name.ToLower().Contains(searchAll) || x.business_unit_subgroup_name.ToLower().Contains(searchAll) || x.business_unit_division_name.ToLower().Contains(searchAll) || x.business_unit_departement_name.ToLower().Contains(searchAll) || x.job_title_name.ToLower().Contains(searchAll)).ToList();
                }
                resp.Success = true;

            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;

        }

        public GeneralResponse<BussinesUnitJobLevelDto> GetDetail(string id)
        {
            GeneralResponse<BussinesUnitJobLevelDto> resp = new GeneralResponse<BussinesUnitJobLevelDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<BussinesUnitJobLevelDto> que = from tb1 in _repo.GetContext().Set<BusinessUnitJobLevel>()
                                                          join tb2 in _repo.GetContext().Set<JobTitle>() on tb1.job_title_id equals tb2.Id
                                                          join tb3 in _repo.GetContext().Set<JobLevel>() on tb1.job_level_id equals tb3.Id
                                                          join tb4 in _repo.GetContext().Set<JobGrade>() on tb1.job_grade_id equals tb4.Id
                                                          where tb1.Id == id
                                                          select new BussinesUnitJobLevelDto()
                                                          {

                                                              Id = tb1.Id,
                                                              remarks = tb1.remarks,
                                                              job_grade_id = tb1.job_grade_id,
                                                              job_level_id = tb1.job_level_id,
                                                              job_title_id = tb1.job_title_id,
                                                              parent_id = tb1.parent_id,
                                                              business_unit_group_id = tb1.business_unit_group_id,
                                                              business_unit_subgroup_id = tb1.business_unit_subgroup_id,
                                                              business_unit_division_id = tb1.business_unit_division_id,
                                                              business_unit_departement_id = tb1.business_unit_departement_id,
                                                              job_grade_name = tb4.grade_name,
                                                              job_title_name = tb2.title_name,
                                                              job_level_name = tb3.job_name,
                                                              ParentBusinessUnit = newBugParent(
                                                                                    (from tcc in _repo.GetContext().Set<BusinessUnitJobLevel>()
                                                                                     join tc1 in _repo.GetContext().Set<JobTitle>() on tcc.job_title_id equals tc1.Id
                                                                                     join hr in _repo.GetContext().Set<EmployeeBasicInfo>() on tcc.Id equals hr.business_unit_job_level_id into hrj
                                                                                     from hrlj in hrj.DefaultIfEmpty()
                                                                                     where tb1.parent_id == tcc.Id
                                                                                     select new ParentBusinessUnit()
                                                                                     {
                                                                                         id = tcc.Id,
                                                                                         name = tc1.title_name,
                                                                                          remarks = tcc.remarks,
                                                                                         username = hrlj.name_employee,
                                                                                         business_unit_departement_id = newbugDepart((from tbu in _repo.GetContext().Set<BusinessUnit>()
                                                                                                                                      where tbu.Id == tcc.business_unit_departement_id
                                                                                                                                      select new BusinessUnitDepartement()
                                                                                                                                      {
                                                                                                                                          name = tbu.unit_name,
                                                                                                                                          id = tbu.Id
                                                                                                                                      }
                                                                                   ).FirstOrDefault()),
                                                                                         business_unit_division_id = newbugDiv((from tbu in _repo.GetContext().Set<BusinessUnit>()
                                                                                                                                where tbu.Id == tcc.business_unit_division_id
                                                                                                                                select new BusinessUnitDivision()
                                                                                                                                {
                                                                                                                                    name = tbu.unit_name,
                                                                                                                                    id = tbu.Id
                                                                                                                                }
                                                                                   ).FirstOrDefault()),
                                                                                         business_unit_group_id = newbug((from tbu in _repo.GetContext().Set<BusinessUnit>()
                                                                                                                          where tbu.Id == tcc.business_unit_group_id
                                                                                                                          select new BusinessUnitGroup()
                                                                                                                          {
                                                                                                                              name = tbu.unit_name,
                                                                                                                              id = tbu.Id
                                                                                                                          }
                                                                                   ).FirstOrDefault()),
                                                                                         business_unit_subgroup_id = newbugsub((from tbu in _repo.GetContext().Set<BusinessUnit>()
                                                                                                                                where tbu.Id == tcc.business_unit_subgroup_id
                                                                                                                                select new BusinessUnitSubgroup()
                                                                                                                                {
                                                                                                                                    name = tbu.unit_name,
                                                                                                                                    id = tbu.Id
                                                                                                                                }
                                                                                   ).FirstOrDefault())
                                                                                     }).FirstOrDefault()),
                                                              BusinessUnitGroup = newbug((from tbu in _repo.GetContext().Set<BusinessUnit>()
                                                                                          where tbu.Id == tb1.business_unit_group_id
                                                                                          select new BusinessUnitGroup()
                                                                                          {
                                                                                              name = tbu.unit_name,
                                                                                              id = tbu.Id
                                                                                          }
                                                                                   ).FirstOrDefault()),
                                                              BusinessUnitSubgroup = newbugsub((from tbu in _repo.GetContext().Set<BusinessUnit>()
                                                                                                where tbu.Id == tb1.business_unit_subgroup_id
                                                                                                select new BusinessUnitSubgroup()
                                                                                                {
                                                                                                    name = tbu.unit_name,
                                                                                                    id = tbu.Id
                                                                                                }
                                                                                   ).FirstOrDefault()),
                                                              BusinessUnitDivision = newbugDiv((from tbu in _repo.GetContext().Set<BusinessUnit>()
                                                                                                where tbu.Id == tb1.business_unit_division_id
                                                                                                select new BusinessUnitDivision()
                                                                                                {
                                                                                                    name = tbu.unit_name,
                                                                                                    id = tbu.Id
                                                                                                }
                                                                                   ).FirstOrDefault()),
                                                              BusinessUnitDepartement = newbugDepart((from tbu in _repo.GetContext().Set<BusinessUnit>()
                                                                                                      where tbu.Id == tb1.business_unit_departement_id
                                                                                                      select new BusinessUnitDepartement()
                                                                                                      {
                                                                                                          name = tbu.unit_name,
                                                                                                          id = tbu.Id
                                                                                                      }
                                                                                   ).FirstOrDefault()),
                                                              created_on = tb1.created_on,

                                                          };

                resp.Data = que.FirstOrDefault();
                resp.Code = "00";
                resp.Message = "sucess";
                resp.Success = true;

            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }


        public GeneralResponseList<BusinessUnitJobLevelChart> Chart(BussinesUnitJobLevelSearch filter)
        {
            GeneralResponseList<BusinessUnitJobLevelChart> resp = new GeneralResponseList<BusinessUnitJobLevelChart>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<BusinessUnitJobLevelChart> que = from tb1 in _repo.GetContext().Set<BusinessUnitJobLevel>()
                                                            join tb2 in _repo.GetContext().Set<JobTitle>() on tb1.job_title_id equals tb2.Id
                                                            join tb3 in _repo.GetContext().Set<JobLevel>() on tb1.job_level_id equals tb3.Id
                                                            join tb4 in _repo.GetContext().Set<JobGrade>() on tb1.job_grade_id equals tb4.Id
                                                            join tb5 in _repo.GetContext().Set<EmployeeBasicInfo>() on tb1.Id equals tb5.business_unit_job_level_id into lj1
                                                            from chartbu in lj1.DefaultIfEmpty()
                                                            select new BusinessUnitJobLevelChart()
                                                            {
                                                                Id = tb1.Id,
                                                                Title = tb2.title_name,
                                                                Grade = tb4.grade_name,
                                                                parentId = tb1.parent_id,
                                                                Name = chartbu.name_employee,
                                                                business_unit_group_id = tb1.business_unit_group_id,
                                                                business_unit_subgroup_id = tb1.business_unit_subgroup_id,
                                                                business_unit_division_id = tb1.business_unit_division_id,
                                                                business_unit_departement_id = tb1.business_unit_departement_id

                                                            };

                resp.RecordsTotal = que.Count();

                //if (filter.Search_business_unit_group_id != null && filter.Search_business_unit_subgroup_id==null && filter.Search_business_unit_division_id==null && filter.Search_business_unit_departement_id==null)
                //{
                //    que = que.Where(w => w.business_unit_group_id.Equals(filter.Search_business_unit_group_id));
                //}
                //else if (filter.Search_business_unit_group_id != null && filter.Search_business_unit_subgroup_id != null)
                //{
                //    que = que.Where(w => w.business_unit_group_id.Equals(filter.Search_business_unit_group_id) && w.business_unit_subgroup_id.Equals(filter.Search_business_unit_subgroup_id) || w.business_unit_subgroup_id.Equals(null));
                //}
                //else if (filter.Search_business_unit_group_id != null && filter.Search_business_unit_subgroup_id != null && filter.Search_business_unit_division_id != null)
                //{
                //    que = que.Where(w => w.business_unit_group_id.Equals(filter.Search_business_unit_group_id) && ( w.business_unit_subgroup_id.Equals(filter.Search_business_unit_subgroup_id) || w.business_unit_subgroup_id.Equals(null)) && (w.business_unit_division_id.Equals(filter.Search_business_unit_division_id) || w.business_unit_division_id.Equals(null)));
                //}
                //else if (filter.Search_business_unit_group_id != null && filter.Search_business_unit_subgroup_id != null && filter.Search_business_unit_division_id != null && filter.Search_business_unit_departement_id != null)
                //{
                //    que = que.Where(w => w.business_unit_group_id.Equals(filter.Search_business_unit_group_id) && (w.business_unit_subgroup_id.Equals(filter.Search_business_unit_subgroup_id) || w.business_unit_subgroup_id.Equals(null)) && (w.business_unit_division_id.Equals(filter.Search_business_unit_division_id) || w.business_unit_division_id.Equals(null)) && w.business_unit_departement_id.Equals(filter.Search_business_unit_departement_id));
                //}
                que = q.filterEquals(que, "business_unit_group_id", filter.Search_business_unit_group_id);
                que = q.filterEquals(que, "business_unit_subgroup_id", filter.Search_business_unit_subgroup_id);
                que = q.filterEquals(que, "business_unit_division_id", filter.Search_business_unit_division_id);
                que = q.filterEquals(que, "business_unit_departement_id", filter.Search_business_unit_departement_id);


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

        public GeneralResponseList<BussinesUnitJobLevelDto> ListAllDetail(BussinesUnitJobLevelSearch filter)
        {
            GeneralResponseList<BussinesUnitJobLevelDto> resp = new GeneralResponseList<BussinesUnitJobLevelDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<BussinesUnitJobLevelDto> que = from tb1 in _repo.GetContext().Set<BusinessUnitJobLevel>()
                                                          join tb2 in _repo.GetContext().Set<JobGrade>() on tb1.job_grade_id equals tb2.Id
                                                          join tb5 in _repo.GetContext().Set<JobLevel>() on tb1.job_level_id equals tb5.Id
                                                          join tb3 in _repo.GetContext().Set<EmployeeBasicInfo>() on tb1.Id equals tb3.business_unit_job_level_id
                                                          select new BussinesUnitJobLevelDto()
                                                          {
                                                              Id = tb1.Id,
                                                              remarks = tb1.remarks,
                                                              business_unit_id = tb1.business_unit_id,
                                                              job_grade_id = tb1.job_grade_id,
                                                              job_level_id = tb1.job_level_id,
                                                              parent_id = tb1.parent_id,
                                                              job_grade_name = tb2.grade_name,
                                                              employee_name = tb3.name_employee,
                                                              job_level_name = tb5.job_name
                                                          };

                resp.RecordsTotal = que.Count();

                que = q.filterEquals(que, "bussines_unit_id", filter.Search_business_unit_id);
                que = q.filterEquals(que, "hrjob_grade_id", filter.Search_hrjob_grade_id);
                que = q.filterEquals(que, "hrjob_level_id", filter.Search_hrjob_level_id);
                que = q.filterEquals(que, "parent_id", filter.Search_parent_id);



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

        public GeneralResponse<BusinessUnitJobLevelParent> GetParent(string id)
        {
            GeneralResponse<BusinessUnitJobLevelParent> resp = new GeneralResponse<BusinessUnitJobLevelParent>();
            try
            {

                resp.Data = GetNodeParent(ListAllNewDetail(), id);
                resp.Success = true;

            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }

        public List<BussinesUnitJobLevelDto> ListAllNewDetail()
        {
            List<BussinesUnitJobLevelDto> resp = new List<BussinesUnitJobLevelDto>();

            PagingHelper q = new PagingHelper();
            IQueryable<BussinesUnitJobLevelDto> que = from tb1 in _repo.GetContext().Set<BusinessUnitJobLevel>()
                                                      join tb2 in _repo.GetContext().Set<JobGrade>() on tb1.job_grade_id equals tb2.Id
                                                      join tb5 in _repo.GetContext().Set<JobLevel>() on tb1.job_level_id equals tb5.Id
                                                      join tb3 in _repo.GetContext().Set<EmployeeBasicInfo>() on tb1.Id equals tb3.business_unit_job_level_id
                                                      select new BussinesUnitJobLevelDto()
                                                      {
                                                          Id = tb1.Id,
                                                          remarks = tb1.remarks,
                                                          business_unit_id = tb1.business_unit_id,
                                                          job_grade_id = tb1.job_grade_id,
                                                          job_level_id = tb1.job_level_id,
                                                          parent_id = tb1.parent_id,
                                                          job_grade_name = tb2.grade_name,
                                                          employee_name = tb3.name_employee,
                                                          job_level_name = tb5.job_name

                                                      };

            resp = que.ToList();
            return resp;
        }

        public GeneralResponseList<BusinessUnitJobLevelParent> ListWithParentDetail(BussinesUnitJobLevelSearch filter)
        {
            GeneralResponseList<BusinessUnitJobLevelParent> resp = new GeneralResponseList<BusinessUnitJobLevelParent>();
            try
            {
                PagingHelper q = new PagingHelper();
                var listfill = ListAllDetail(filter);
                var allorg = ListAllNewDetail();


                resp.Rows = listfill.Rows.Select(tb1 => new BusinessUnitJobLevelParent()
                {
                    Id = tb1.Id,
                    remarks = tb1.remarks,
                    business_unit_id = tb1.business_unit_id,
                    job_grade_id = tb1.job_grade_id,
                    job_level_id = tb1.job_level_id,
                    parent_id = tb1.parent_id,
                    job_grade_name = tb1.job_grade_name,
                    employee_name = tb1.employee_name,
                    job_level_name = tb1.job_level_name,
                    parent = GetNodeParent(allorg, tb1.parent_id)
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

        public GeneralResponse<List<BusinessUnitJobLevelTree>> ListTree(string parent_id = null)
        {
            GeneralResponse<List<BusinessUnitJobLevelTree>> resp = new GeneralResponse<List<BusinessUnitJobLevelTree>>();
            try
            {
                resp.Data = GetNodeChildren(ListAllNewDetail(), parent_id);
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


        #region "Private Method"
        private BusinessUnitJobLevelParent GetNodeParent(List<BussinesUnitJobLevelDto> list, string id)
        {
            return list.Where(x => x.Id == id).Select(x => new BusinessUnitJobLevelParent
            {
                Id = x.Id,
                remarks = x.remarks,
                business_unit_id = x.business_unit_id,
                employee_name = x.employee_name,
                job_grade_id = x.job_grade_id,
                job_grade_name = x.job_grade_name,
                job_level_id = x.job_level_id,
                parent_id = x.parent_id,
                job_level_name = x.job_level_name,
                parent = GetNodeParent(list, x.parent_id)
            }).SingleOrDefault();
        }
        private List<BusinessUnitJobLevelTree> GetNodeChildren(List<BussinesUnitJobLevelDto> list, string parent)
        {
            return list.Where(x => x.parent_id == parent).Select(x => new BusinessUnitJobLevelTree
            {
                Id = x.Id,
                remarks = x.remarks,
                business_unit_id = x.business_unit_id,
                employee_name = x.employee_name,
                job_grade_id = x.job_grade_id,
                job_grade_name = x.job_grade_name,
                job_level_id = x.job_level_id,
                parent_id = x.parent_id,
                job_level_name = x.job_level_name,
                children = x.parent_id != x.Id ? GetNodeChildren(list, x.Id) : new List<BusinessUnitJobLevelTree>()
            }).ToList();
        }
        #endregion

    }
}

