using Phoenix.Data;
using Phoenix.Data.Attributes;
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
using System.Reflection;

namespace Phoenix.Shared.Core.Areas.Hris.Services
{
    public partial interface IMppServices : IBaseService<MppDto, Mpp, string>
    {
        GeneralResponseList<MppDto> ListAll(MppSearch filter);
        GeneralResponse<MppDto> CreateMppWithDetail(MppNew dto);
        GeneralResponse<MppDto> GetWithDetil(string id);
        GeneralResponse<Mpp> UpdateStatusMpp(MppDto dto);
        GeneralResponse<Mpp> UpdateAllDataMpp(MppDto dto);
        GeneralResponse<Mpp> DeleteWithDetail(string id);
    }
    public partial class MppServices : BaseService<MppDto, Mpp, string>, IMppServices
    {
        private IEFRepository<MppDetail, string> _repoDetailMpp;
        private IEFRepository<MppApprovedStatus, string> _repoApprovedStatus;
        readonly DataContext context;
        public MppServices(IEFRepository<Mpp, string> repo, IEFRepository<MppDetail, string> repoDetailMpp, IEFRepository<MppApprovedStatus, string> repoApprovedStatus,
            DataContext context) : base(repo)
        {
            context = context;
            _repo = repo;
            _repoDetailMpp = repoDetailMpp;
            _repoApprovedStatus = repoApprovedStatus;
        }

        public GeneralResponse<MppDto> CreateMppWithDetail(MppNew dto)
        {
            GeneralResponse<MppDto> resp = new GeneralResponse<MppDto>();
            Mpp NewMpp = new Mpp();

            try
            {
                Random rnd = new Random();

                decimal? total_budget = 0;
                foreach (MppNewRealData data in dto.dataDetailMpp)
                {
                    if (!data.mppbudget.Equals(""))
                    {
                        total_budget = total_budget + data.mppbudget;
                    }
                }
                PropertyMapper.All(dto, NewMpp);
                NewMpp.total_budget = total_budget;
                var MppId = "";
                if (dto.code != null)
                {
                    var dataMpp = _repo.GetContext().Set<Mpp>().Where(x => x.code == dto.code && x.is_deleted == false).Select(x => new MppDto { Id = x.Id, created_on = x.created_on, created_by = x.created_by }).FirstOrDefault();
                    MppId = dataMpp.Id;
                    NewMpp.code = dto.code;
                    NewMpp.total_budget = total_budget;
                    NewMpp.created_on = dataMpp.created_on;
                    NewMpp.created_by = dataMpp.created_by;
                    NewMpp.is_active = true;
                    NewMpp.is_deleted = false;
                    NewMpp.Id = MppId;

                    _repo.Update(NewMpp);
                }
                else
                {
                    NewMpp.code = "MPP" + DateTime.Now.ToString("HHmmss") + rnd.Next(1, 9999);
                    _repo.Create(false, NewMpp);
                }


                if (dto.code != null)
                {
                    IQueryable<MppDetailDto> que = (from tb1 in _repo.GetContext().Set<Mpp>()
                                                    join tbu in _repo.GetContext().Set<MppDetail>() on tb1.Id equals tbu.mpp_id
                                                    where tbu.mpp_id == MppId
                                                    select new MppDetailDto()
                                                    {
                                                        Id = tbu.Id,
                                                        mpp_id = tbu.mpp_id
                                                    });
                    List<MppDetailDto> listMppdetail = que.ToList();

                    foreach (MppDetailDto data in listMppdetail)
                    {

                        _repoDetailMpp.RemoveByID(false, data.Id);
                    }

                    foreach (MppNewRealData data in dto.dataDetailMpp)
                    {
                        MppDetail newMppDetil = new MppDetail();
                        newMppDetil.mpp_id = NewMpp.Id;
                        newMppDetil.ttf = data.ttf;
                        newMppDetil.employment_status = data.status;
                        newMppDetil.mpp_budged = data.mppbudget;
                        newMppDetil.job_tittle_id = data.jobtitle;
                        newMppDetil.job_grade_id = data.grade;
                        newMppDetil.Description = data.Description;
                        newMppDetil.status = "0";
                        newMppDetil.project_name = data.project;
                        newMppDetil.budget_souce = data.budget_souce;
                        _repoDetailMpp.Create(false, newMppDetil);
                    }


                }
                else
                {
                    foreach (MppNewRealData data in dto.dataDetailMpp)
                    {
                        MppDetail mppDetail = new MppDetail();
                        if (!data.status.Equals(""))
                        {
                            mppDetail.mpp_id = NewMpp.Id;
                            mppDetail.ttf = data.ttf;
                            mppDetail.employment_status = data.status;
                            mppDetail.mpp_budged = data.mppbudget;
                            mppDetail.job_tittle_id = data.jobtitle;
                            mppDetail.job_grade_id = data.grade;
                            mppDetail.Description = data.Description;
                            mppDetail.status = "0";
                            mppDetail.project_name = data.project;
                            mppDetail.budget_souce = data.budget_souce;
                            _repoDetailMpp.Create(false, mppDetail);
                        }
                    }
                }

                MppDto mppforDto = new MppDto();
                PropertyMapper.All(NewMpp, mppforDto);

                // var template = _repo.GetContext().Set<TrTemplateApproval>().Where(x => x.RefId == NewMpp.Id).ToList();
                // var templateUser = (from b in _repo.GetContext().Set<TrUserApproval>()
                //                     join c in _repo.GetContext().Set<TrTemplateApproval>() on b.TrTempApprovalId equals c.Id
                //                     where c.RefId == NewMpp.Id
                //                     select b).ToList();

                // var templateCondition = (from a in _repo.GetContext().Set<Data.Models.Um.TrConditionUserApproval>()
                //                          join b in _repo.GetContext().Set<TrUserApproval>() on a.TrUserApprovalId equals b.Id
                //                          join c in _repo.GetContext().Set<TrTemplateApproval>() on b.TrTempApprovalId equals c.Id
                //                          where c.RefId == NewMpp.Id
                //                          select a).ToList();

                // _repo.GetContext().Set<Data.Models.Um.TrConditionUserApproval>().RemoveRange(templateCondition.ToArray());
                // _repo.GetContext().Set<TrUserApproval>().RemoveRange(templateUser.ToArray());
                // _repo.GetContext().Set<TrTemplateApproval>().RemoveRange(template.ToArray());

                // var dataMenu = _repo.GetContext().Set<TmMenu>().Where(x => !(x.IsDeleted ?? false) && x.MenuUnique.Equals(Data.Models.MenuUnique.RECMPP)).FirstOrDefault();
                // var dataTempate = _repo.GetContext().Set<TmTemplateApproval>().Where(x => x.MenuId == dataMenu.Id && x.IsDeleted == false).FirstOrDefault();
                // if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }

                // var vm = new Phoenix.Data.Models.ViewModel.TransApprovalHrisVm()
                // {
                //     IdTemplate = dataTempate.Id,
                //     MenuId = dataMenu.Id,
                //     RefId = NewMpp.Id,
                //     DetailLink = $"{Phoenix.Data.Models.ApprovalLink.MPP}?Id={NewMpp.Id}",
                //     NotificationType = Phoenix.Data.Models.NotificationTypeCode.MPPNotification,
                //     Module = Phoenix.Data.Models.NotificationModuleType.ModuleHRIS
                // };
                //var subGroupId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessSubGroup(this.context.HttpContext);
                // var divisionId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessUnitDivisi(context.HttpContext);
                // var employeeId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(context.HttpContext);

                _repo.SaveChanges();
                _repoDetailMpp.SaveChanges();
                _repo.Dispose();
                _repoDetailMpp.Dispose();
                resp.Data = mppforDto;
                resp.Success = true;
                resp.Message = "Sucess";
                resp.Code = "00";

            }
            catch (Exception x)
            {
                //_repo.RemoveByID(NewMpp.Id);
                //var ListDetailMpp = _repoDetailMpp.Rawlist("Select id from mpp_detail where mpp_id={0}", NewMpp.Id);
                //foreach (var item in ListDetailMpp) {
                //    _repoDetailMpp.RemoveByID(item.Id);
                //}
                resp.Success = false;
                resp.Code = "500";
                resp.Message = x.Message.ToString();
            }

            return resp;
        }

        public GeneralResponseList<MppDto> ListAll(MppSearch filter)
        {

            GeneralResponseList<MppDto> resp = new GeneralResponseList<MppDto>();
            try
            {
                BusinessUnitDto bu = new BusinessUnitDto();

                PagingHelper q = new PagingHelper();
                IQueryable<MppDto> que = from tb1 in _repo.GetContext().Set<Mpp>()
                                         join tba in _repo.GetContext().Set<TrTemplateApproval>() on tb1.Id equals tba.RefId into g
                                         from result in g.DefaultIfEmpty()
                                         join tbf in _repo.GetContext().Set<FinancialYear>() on tb1.fiscal_year equals tbf.FinanceYearNumber
                                         orderby tb1.created_on descending
                                         select new MppDto()
                                         {
                                             Id = tb1.Id,
                                             approval_id = tb1.approval_id,
                                             code = tb1.code,
                                             created_date = tb1.created_date,
                                             created_on = tb1.created_on,
                                             fiscal_year = tb1.fiscal_year,
                                             fiscal_year_number = tbf.FinanceYearNumber,
                                             fiscal_year_name = tbf.FinancialYearName,
                                             business_unit_id = tb1.business_unit_id,
                                             status = result.StatusApproved.ToString(),
                                             total_budget = tb1.total_budget,
                                             updated_date = tb1.updated_date,
                                             JobTitle = (from tb2 in _repo.GetContext().Set<MppDetail>()
                                                         join tb4 in _repo.GetContext().Set<JobTitle>() on tb2.job_tittle_id equals tb4.Id
                                                         where tb2.mpp_id == tb1.Id
                                                         select new MppDetailDto()
                                                         {
                                                             job_tittle_id = tb4.Id,
                                                             job_title_name = tb4.title_name
                                                         }).ToList(),
                                             ApprovalReal = (from tap in _repo.GetContext().Set<TrTemplateApproval>()
                                                             join tapd in _repo.GetContext().Set<TrUserApproval>() on tap.Id equals tapd.TrTempApprovalId
                                                             join tapde in _repo.GetContext().Set<EmployeeBasicInfo>() on tapd.EmployeeBasicInfoId equals tapde.Id
                                                             where tap.RefId == tb1.Id
                                                             select new TrUserApprovalDto()
                                                             {
                                                                 approved_by = tapde.name_employee,
                                                                 created_on = tapd.CreatedOn,
                                                                 StatusApproved = tapd.StatusApproved
                                                             }).ToList(),
                                             Approval = (from tb7 in _repo.GetContext().Set<MppApprovedStatus>()
                                                         join tbb in _repo.GetContext().Set<ApplicationUser>() on tb7.created_by equals tbb.Id
                                                         where tb1.Id == tb7.id_mpp && tb7.id_mpp == tb1.Id
                                                         select new MppApprovedStatusDto()
                                                         {
                                                             approved_by = tbb.app_username,
                                                             created_on = tb7.created_on
                                                         }).ToList(),
                                            BusinessUnit = ((from tb8 in _repo.GetContext().Set<BusinessUnit>()
                                                            where tb1.business_unit_id == tb8.Id
                                                            select new BusinessUnitDto()
                                                            {
                                                                Id = tb8.Id,
                                                                unit_name = tb8.unit_name
                                                            }).FirstOrDefault() ?? new BusinessUnitDto())
                                         };

                resp.RecordsTotal = que.Count();

                que = q.filterEquals(que, "approval_id", filter.Search_approval_id);
                que = q.filterContains(que, "code", filter.Search_code);
                //que = q.filterEquals(que, "created_date", filter.Search_created_date);
                que = q.filterContains(que, "fiscal_year", filter.Search_fiscal_year);
                que = q.filterContains(que, "business_unit_id", filter.Search_business_unit_id);
                que = q.filterContains(que, "status", filter.Search_status);
                que = q.filterEquals(que, "total_budget", filter.Search_total_budget);
                //que = q.filterEquals(que, "updated_date", filter.Search_updated_date);

                resp.RecordsFiltered = que.Count();
                resp.Total = resp.RecordsTotal;

                //que = q.sortir(que, "Id", filter.sortBy, filter.sortDir);
                que = q.limit(que, filter.GetLimit(), filter.GetOffset());

                resp.Rows = que.OrderByDescending(x => x.created_on).ToList();
                resp.Success = true;

            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;

        }

        public GeneralResponse<MppDto> GetWithDetil(string id)
        {
            GeneralResponse<MppDto> resp = new GeneralResponse<MppDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<MppDto> que = from tb1 in _repo.GetContext().Set<Mpp>()
                                         join tbu in _repo.GetContext().Set<BusinessUnit>() on tb1.business_unit_id equals tbu.Id
                                         join tbf in _repo.GetContext().Set<FinancialYear>() on tb1.fiscal_year equals tbf.FinanceYearNumber
                                         where tb1.Id == id
                                         select new MppDto()
                                         {
                                             Id = tb1.Id,
                                             approval_id = tb1.approval_id,
                                             code = tb1.code,
                                             created_date = tb1.created_date,
                                             fiscal_year = tb1.fiscal_year,
                                             fiscal_year_name = tbf.FinancialYearName,
                                             fiscal_year_number = tbf.FinanceYearNumber,
                                             business_unit_id = tb1.business_unit_id,
                                             business_unit_name = tbu.unit_name,
                                             status = tb1.status,
                                             total_budget = tb1.total_budget,
                                             updated_date = tb1.updated_date,
                                             mppDetail = (from tb2 in _repo.GetContext().Set<MppDetail>()
                                                          join tb3 in _repo.GetContext().Set<JobTitle>() on tb2.job_tittle_id equals tb3.Id
                                                          where tb1.Id == tb2.mpp_id
                                                          select new MppDetailDto()
                                                          {
                                                              Id = tb2.Id,
                                                              mpp_id = tb2.mpp_id,
                                                              employment_status = tb2.employment_status,
                                                              ttf = tb2.ttf,
                                                              mpp_budged = tb2.mpp_budged,
                                                              job_tittle_id = tb2.job_tittle_id,
                                                              job_grade_id = tb2.job_grade_id,
                                                              job_title_name = tb3.title_name,
                                                              porject_name = tb2.project_name,
                                                              Description = tb2.Description,
                                                              status = tb2.status,
                                                              budget_souce = tb2.budget_souce
                                                          }).ToList()
                                         };
                resp.Data = que.FirstOrDefault();
                resp.Success = true;
                resp.Code = "00";
                resp.Message = "sucess";

            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }
        private string[] GetUpdateOnly()
        {
            return new string[] { "status" };
        }
        public GeneralResponse<Mpp> UpdateStatusMpp(MppDto dto)
        {

            GeneralResponse<Mpp> resp = new GeneralResponse<Mpp>();
            try
            {
                Mpp newData = _repo.FindByID(dto.Id);
                dto.created_by = newData.created_by;
                dto.created_on = newData.created_on;
                PropertyMapper.All(dto, newData);

                PropertyInfo pi = newData.GetType().GetProperty("updated_by");
                if (pi != null)
                {
                    pi.SetValue(newData, Glosing.Instance.EmployeeId);
                }
                PropertyInfo pd = newData.GetType().GetProperty("updated_date");
                if (pd != null)
                {
                    pd.SetValue(newData, DateTime.Now);
                }
                _repo.Update(false, newData);


                //MppApprovedStatus app = new MppApprovedStatus();
                //app.id_mpp = newData.Id;
                //app.created_on = newData.created_on;
                //app.created_by = newData.created_by;
                //app.created_date = newData.created_on;
                //app.is_deleted = false;

                //_repoApprovedStatus.Create(false, app);

                _repo.SaveChanges();
                //_repoApprovedStatus.SaveChanges();
                _repo.Dispose();
                //_repoApprovedStatus.Dispose();
                resp.Success = true;
                resp.Code = "00";
                resp.Message = "sucess";
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }

        public GeneralResponse<Mpp> UpdateAllDataMpp(MppDto dto)
        {
            GeneralResponse<Mpp> resp = new GeneralResponse<Mpp>();
            try
            {
                Mpp newData = _repo.FindByID(dto.Id);
                PropertyMapper.All(dto, newData);
                PropertyInfo pi = newData.GetType().GetProperty("updated_by");
                if (pi != null)
                {
                    pi.SetValue(newData, Glosing.Instance.EmployeeId);
                }
                PropertyInfo pd = newData.GetType().GetProperty("updated_date");
                if (pd != null)
                {
                    pd.SetValue(newData, DateTime.Now);
                }


                IQueryable<MppDetailDto> que = (from tb1 in _repo.GetContext().Set<Mpp>()
                                                join tbu in _repo.GetContext().Set<MppDetail>() on tb1.Id equals tbu.mpp_id
                                                where tbu.mpp_id == dto.Id
                                                select new MppDetailDto()
                                                {
                                                    Id = tbu.Id,
                                                    mpp_id = tbu.mpp_id
                                                });
                List<MppDetailDto> listMppdetail = que.ToList();

                foreach (MppDetailDto data in listMppdetail)
                {

                    _repoDetailMpp.RemoveByID(false, data.Id);
                }

                foreach (MppNewRealData data in dto.dataDetailMpp)
                {
                    MppDetail newMppDetil = new MppDetail();
                    newMppDetil.mpp_id = dto.Id;
                    newMppDetil.ttf = data.ttf;
                    newMppDetil.employment_status = data.status;
                    newMppDetil.mpp_budged = data.mppbudget;
                    newMppDetil.job_tittle_id = data.jobtitle;
                    newMppDetil.job_grade_id = data.grade;
                    newMppDetil.Description = data.Description;
                    newMppDetil.status = "0";
                    newMppDetil.project_name = data.project;
                    newMppDetil.budget_souce = data.budget_souce;
                    _repoDetailMpp.Create(false, newMppDetil);
                }


                _repo.Update(false, newData);
                _repo.SaveChanges();
                _repoDetailMpp.SaveChanges();
                _repo.Dispose();
                _repoDetailMpp.Dispose();
                resp.Success = true;
                resp.Code = "00";
                resp.Message = "sucess";
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }

        public GeneralResponse<Mpp> DeleteWithDetail(string id)
        {
            GeneralResponse<Mpp> resp = new GeneralResponse<Mpp>();
            try
            {

                IQueryable<MppDetailDto> que = (from tb1 in _repo.GetContext().Set<Mpp>()
                                                join tbu in _repo.GetContext().Set<MppDetail>() on tb1.Id equals tbu.mpp_id
                                                where tbu.mpp_id == id
                                                select new MppDetailDto()
                                                {
                                                    Id = tbu.Id,
                                                    mpp_id = tbu.mpp_id
                                                });
                List<MppDetailDto> listMppdetail = que.ToList();



                foreach (MppDetailDto data in listMppdetail)
                {

                    _repoDetailMpp.RemoveByID(false, data.Id);
                }

                _repo.RemoveByID(false, id);


                _repo.SaveChanges();
                _repoDetailMpp.SaveChanges();
                _repo.Dispose();
                _repoDetailMpp.Dispose();
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
    }
}

