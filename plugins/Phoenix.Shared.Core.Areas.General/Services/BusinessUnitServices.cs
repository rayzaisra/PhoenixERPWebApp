using Phoenix.Shared.Core.Areas.General.Dtos;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Parameters;
using Phoenix.Shared.Core.Repositories;
using Phoenix.Shared.Core.Services;
using Phoenix.Shared.Helpers;
using Phoenix.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Phoenix.Shared.Core.Areas.General.Services
{
    public partial interface IBusinessUnitServices : IBaseService<BusinessUnitDto, BusinessUnit, string>
    {
        GeneralResponseList<BusinessUnitDto> ListFilter(BusinessUnitSearch filter);
        GeneralResponseList<BusinessUnitParent> ListWithParent(BusinessUnitSearch filter);
        GeneralResponse<List<BusinessUnitTree>> ListTree(string parent_id = null);
        List<BusinessUnitDto> ListAll();
        GeneralResponse<BusinessUnitParent> GetParent(string id);
        GeneralResponse<BusinessUnitDto> CreateParalel(BusinessUnitNew param);
        GeneralResponse<BusinessUnit> DeletePralel(string id);
        GeneralResponseList<BusinessUnitKendoUIParent> GetTreeKenoUiFormatData(BusinessUnitSearch filter);
    }
    public partial class BusinessUnitServices : BaseService<BusinessUnitDto, BusinessUnit, string>, IBusinessUnitServices
    {
        IEFRepository<Team, string> _repoTeam;
        public BusinessUnitServices(IEFRepository<BusinessUnit, string> repo, IEFRepository<Team, string> repoTeam) : base(repo)
        {
            _repo = repo;
            _repoTeam = repoTeam;
        }

        public GeneralResponse<BusinessUnitDto> CreateParalel(BusinessUnitNew param)
        {
            GeneralResponse<BusinessUnitDto> resp = new GeneralResponse<BusinessUnitDto>();
            try
            {
                Random rnd = new Random();
                Team NewTeam = new Team();
                PropertyMapper.All(param, NewTeam);
                NewTeam.team_name = param.unit_code + rnd.Next(1, 9999);
                _repoTeam.Create(false, NewTeam);


                BusinessUnit NewBusinessUnit = new BusinessUnit();
                PropertyMapper.All(param, NewBusinessUnit);
                NewBusinessUnit.default_team = NewTeam.Id;
                _repo.Create(false, NewBusinessUnit);


                _repo.SaveChanges();
                _repoTeam.SaveChanges();
                _repo.Dispose();
                _repoTeam.Dispose();
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

        public GeneralResponse<BusinessUnit> DeletePralel(string id)
        {
            GeneralResponse<BusinessUnit> resp = new GeneralResponse<BusinessUnit>();
            try
            {

                resp.Data = _repo.FindByID(id);

                _repoTeam.RemoveByID(false, resp.Data.default_team);
                _repo.RemoveByID(false, id);


                _repoTeam.SaveChanges();
                _repo.SaveChanges();
                _repo.Dispose();
                _repoTeam.Dispose();
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

        public GeneralResponse<BusinessUnitParent> GetParent(string id)
        {
            GeneralResponse<BusinessUnitParent> resp = new GeneralResponse<BusinessUnitParent>();
            try
            {

                resp.Data = GetNodeParent(ListAll(), id);
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
                IQueryable<BusinessUnitDto> que = from tb1 in _repo.GetContext().Set<BusinessUnit>()
                                                  join tb2 in _repo.GetContext().Set<BusinessUnitType>() on tb1.business_unit_type_id equals tb2.Id

                                                  select new BusinessUnitDto()
                                                  {
                                                      Id = tb1.Id,
                                                      business_unit_type_id = tb1.business_unit_type_id,
                                                      default_team = tb1.default_team,
                                                      organization_id = tb1.organization_id,
                                                      parent_unit = tb1.parent_unit,
                                                      unit_address = tb1.unit_address,
                                                      unit_code = tb1.unit_code,
                                                      unit_description = tb1.unit_description,
                                                      unit_email = tb1.unit_email,
                                                      unit_fax = tb1.unit_fax,
                                                      unit_name = tb1.unit_name,
                                                      unit_phone = tb1.unit_phone,
                                                      created_on = tb1.created_on,
                                                      isfinance = tb1.isfinance,
                                                      business_unit_level = tb2.business_unit_level,
                                                      business_unit_type_name = tb2.business_unit_type_name,
                                                      isHris = tb1.ishris
                                                  };

                resp.RecordsTotal = que.Count();


                
                if(!string.IsNullOrEmpty(filter.Search_business_unit_type_name))
                    que = q.filterEquals(que, "business_unit_type_name", filter.Search_business_unit_type_name);
                if (filter.Search_business_unit_level != 0)
                    que = q.filterEquals(que, "business_unit_level", filter.Search_business_unit_level);
                if (!string.IsNullOrEmpty(filter.Search_parent_unit))
                    que = q.filterEquals(que, "parent_unit", filter.Search_parent_unit);
                que = q.filterContains(que, "unit_code", filter.Search_unit_code);
                que = q.filterContains(que, "unit_name", filter.Search_unit_name);
                if(filter.Seach_is_finance != null)
                    que = q.filterEquals(que, "isfinance", filter.Seach_is_finance);
                
                if (filter.search != null)
                {
                    que = que.Where(w => w.unit_code.Contains(filter.search) || w.unit_name.Contains(filter.search) );
                }

                resp.RecordsFiltered = que.Count();
                resp.Total = resp.RecordsTotal;

                que = q.sortir(que, "unit_name", filter.sortBy, filter.sortDir);
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

        public List<BusinessUnitDto> ListAll()
        {
            List<BusinessUnitDto> resp = new List<BusinessUnitDto>();

            PagingHelper q = new PagingHelper();
            IQueryable<BusinessUnitDto> que = from tb1 in _repo.GetContext().Set<BusinessUnit>()
                                              join tb2 in _repo.GetContext().Set<BusinessUnitType>() on tb1.business_unit_type_id equals tb2.Id
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
                                                  organization_id = tb1.organization_id,
                                                  unit_email = tb1.unit_email,
                                                  unit_fax = tb1.unit_fax,
                                                  unit_phone = tb1.unit_phone,
                                                  business_unit_level = tb2.business_unit_level
                                                  
                                              };

            resp = que.ToList();
            return resp;
        }

    

        public GeneralResponse<List<BusinessUnitTree>> ListTree(string parent_id = null)
        {
            GeneralResponse<List<BusinessUnitTree>> resp = new GeneralResponse<List<BusinessUnitTree>>();
            try
            {
                resp.Data = GetNodeChildren(ListAll(), parent_id);
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

        public GeneralResponseList<BusinessUnitParent> ListWithParent(BusinessUnitSearch filter)
        {
            GeneralResponseList<BusinessUnitParent> resp = new GeneralResponseList<BusinessUnitParent>();
            try
            {
                PagingHelper q = new PagingHelper();
                var listfill = ListFilter(filter);
                var allorg = ListAll();

                if (filter.Seach_is_finance == true)
                {
                    resp.Rows = listfill.Rows.Where(x => x.isfinance == true).Select(tb1 => new BusinessUnitParent()
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
                        business_unit_level = tb1.business_unit_level,

                        parent = GetNodeParent(allorg, tb1.parent_unit)
                    }).OrderBy(x => x.unit_name).ToList();
                }
                else
                {
                    resp.Rows = listfill.Rows.Where(x => x.isHris == true).Select(tb1 => new BusinessUnitParent()
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
                        business_unit_level = tb1.business_unit_level,

                        parent = GetNodeParent(allorg, tb1.parent_unit)
                    }).OrderBy(x=>x.unit_name).ToList();
                }

                if (filter.Search_parent_unit_is_not_null == true)
                {
                    resp.Rows = resp.Rows.Where(x => x.parent_unit != null).ToList();
                }
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

        public GeneralResponseList<BusinessUnitKendoUIParent> GetTreeKenoUiFormatData(BusinessUnitSearch filter)
        {
            GeneralResponseList<BusinessUnitKendoUIParent> resp = new GeneralResponseList<BusinessUnitKendoUIParent>();
            try
            {
                PagingHelper q = new PagingHelper();
                var listfill = ListFilter(filter);
                var allorg = ListAll();

                resp.Rows = listfill.Rows.Select(tb1 => new BusinessUnitKendoUIParent()
                {
                    id = tb1.Id,
                    text = tb1.unit_name,
                    expanded = true,
                    items = GetNodeChildrenKendo(allorg,tb1.parent_unit)
                }).ToList();



                resp.RecordsFiltered = resp.Rows.Count();
                resp.RecordsTotal = resp.Rows.Count();
                resp.Total = resp.RecordsTotal;

                resp.Success = true;
                resp.Message = "sucess";
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
                business_unit_level = x.business_unit_level,
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
                business_unit_level = x.business_unit_level,
                children = x.parent_unit != x.Id ? GetNodeChildren(list, x.Id) : new List<BusinessUnitTree>()
            }).ToList();
        }

        private List<BusinessUnitKendoUIParent> GetNodeChildrenKendo(List<BusinessUnitDto> list, string parent)
        {
            return list.Where(x => x.parent_unit == parent).Select(x => new BusinessUnitKendoUIParent
            {
               id = x.Id,
               text = x.unit_name,
               expanded = true,
               items = x.parent_unit != x.Id ? GetNodeChildrenKendo(list, x.Id) : new List<BusinessUnitKendoUIParent>()
            }).ToList();
        }

        #endregion
    }
}

