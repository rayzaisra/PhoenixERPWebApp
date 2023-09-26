using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.RestApi;
using Phoenix.Shared.Core.Contexts;

namespace Phoenix.Data
{
    public interface IMassLeaveService : IDataServiceHris<MassLeave>
    {
        Task<List<MassLeaveDetail>> GetAssignList(string id);
        Task<List<MassLeave>> GetApprovalList();
    }

    public class MassLeaveService : IMassLeaveService
    {
        readonly DataContext context;
        readonly GlobalFunctionApproval globalFcApproval;
        readonly Services.Um.IManageMenuService menuService;

        string idTemplate = "";

        /// <summary>
        /// And endpoint to manage MassLeave
        /// </summary>
        /// <param name="context">Database context</param>
        public MassLeaveService(DataContext context, GlobalFunctionApproval globalFcApproval, Services.Um.IManageMenuService menuService)
        {
            this.context = context;
            this.globalFcApproval = globalFcApproval;
            this.menuService = menuService;
        }

        public async Task<int> AddAsync(MassLeave entity)
        {
            entity.Id = await TransID.GetTransId(context, Code.Leave, DateTime.Today);
            return await AddEdit(entity, false);
        }

        public async Task<int> EditAsync(MassLeave entity)
        {
            return await AddEdit(entity, true);
        }

        private async Task<int> AddEdit(MassLeave entity, bool isEdit)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    entity.Year = entity.LeaveFrom.Year;
                    entity.RequestBy = entity.requestEmployee.EmployeeId;
                    if (isEdit)
                    {
                        context.PhoenixEdit(entity);
                    }
                    else
                    {
                        context.PhoenixAdd(entity);
                    }

                    if (!string.IsNullOrEmpty(entity.Id))
                    {
                        var detail = context.MassLeaveDetails.AsNoTracking().Where(x => x.MassLeaveId == entity.Id).ToList();
                        if (detail.Count > 0)
                        {
                            context.MassLeaveDetails.RemoveRange(detail);
                        }
                    }

					if (entity.MassLeaveDetails != null)
					{
						if (entity.MassLeaveDetails.Count > 0)
						{
							Parallel.ForEach(entity.MassLeaveDetails, (item) =>
							{
								item.Id = Guid.NewGuid().ToString();
								item.MassLeaveId = entity.Id;
								context.PhoenixAddAsync(item);
							});
						}
					}

                    var isApprove = true;
                    if (entity.Status != 0)//0: Draft
                    {

                        var dataMenu = await menuService.GetByUniqueName(MenuUnique.MassLeave).ConfigureAwait(false);
                        if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
                        var dataTempate = context.TmTemplateApprovals.Where(x => x.MenuId == dataMenu.Id && x.IsDeleted == false).FirstOrDefault();
                        idTemplate = dataTempate.Id;
                        var vm = new Models.ViewModel.TransApprovalHrisVm()
                        {
                            MenuId = dataMenu.Id,
                            RefId = entity.Id,
                            DetailLink = $"{ApprovalLink.MassLeave}?Id={entity.Id}&isApprove=true",
                            IdTemplate = idTemplate,
                            Module = NotificationModuleType.ModuleHRIS,
                            NotificationType = NotificationTypeCode.MassLeaveNotification
                        };

                        

                        var subGroupId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessSubGroup(context.HttpContext);
                        var divisionId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessUnitDivisi(context.HttpContext);
                        var employeeId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(context.HttpContext);
                        isApprove = await globalFcApproval.Save(vm, subGroupId, divisionId, employeeId);

                        if (isApprove)
                        {
                            var save = await context.SaveChangesAsync();
                            transaction.Commit();
                            return save;
                        }
                        else
                        {
                            transaction.Rollback();
                            throw new Exception("please check the approval template that will be processed");
                        }
                    }
                    else
                    {
                        var save = await context.SaveChangesAsync();
                        transaction.Commit();
                        return save;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }


        public async Task<int> ApproveAsync(MassLeave entity)
        {
            var model = await context.MassLeaves.Where(x => x.Id == entity.Id).FirstOrDefaultAsync();
            var listDetail = GetAssignList(entity.Id);
            model.MassLeaveDetails = listDetail.Result;

           
            for (int i = 0; i < model.TotalDays; i++)
            {
                Holiday m = new Holiday();
                m.Id = Guid.NewGuid().ToString();
                m.Name = model.Reason;
                if (i == 0)
                {
                    m.Date = model.LeaveFrom;
                }
                else
                {
                    m.Date = model.LeaveFrom.AddDays(i);
                }
                m.CreatedBy = model.CreatedBy;
                m.CreatedOn = DateTime.Now;
                m.IsDeleted = false;
                context.Holidays.Add(m);
                context.SaveChanges();
                
            }

            return await globalFcApproval.ProcessApproval<MassLeave>(entity.Id, entity.Status, entity.RemarkRejected, model);
        }

        public async Task<int> DeleteSoftAsync(MassLeave entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<List<MassLeave>> Get()
        {
            var result = await (from a in context.MassLeaves
                                join c in context.TrTemplateApprovals on a.Id equals c.RefId
                                where !(a.IsDeleted ?? false)
                                select new MassLeave
                                {
                                    Id = a.Id,
                                    Year = a.Year,
                                    LeaveFrom = a.LeaveFrom,
                                    LeaveTo = a.LeaveTo,
                                    TotalDays = a.TotalDays,
                                    Reason = a.Reason,
                                    RequestBy = a.RequestBy,
                                    RequestDate = a.RequestDate,
                                    Status = c.StatusApproved.GetHashCode(),
                                    StatusDesc = c.StatusApprovedDescription,
                                    CreatedOn = a.CreatedOn
                                }).OrderByDescending(x => x.CreatedOn).ToListAsync();

            var result2 = await (from a in context.MassLeaves
                                 where !(a.IsDeleted ?? false) && !context.TrTemplateApprovals.Any(x => x.RefId == a.Id)
                                 select new MassLeave
                                 {
                                     Id = a.Id,
                                     Year = a.Year,
                                     LeaveFrom = a.LeaveFrom,
                                     LeaveTo = a.LeaveTo,
                                     TotalDays = a.TotalDays,
                                     Reason = a.Reason,
                                     RequestBy = a.RequestBy,
                                     RequestDate = a.RequestDate,
                                     Status = StatusTransaction.Draft,
                                     StatusDesc = StatusTransaction.StatusName(StatusTransaction.Draft),
                                     CreatedOn = a.CreatedOn
                                 }).OrderByDescending(x => x.CreatedOn).ToListAsync();

            if (result == null) result = new List<MassLeave>();
            result.AddRange(result2);
            return result.OrderByDescending(x => x.CreatedOn).ToList();
        }

        public async Task<List<MassLeave>> GetApprovalList()
        {
            var result = await (from a in context.MassLeaves
                                join c in context.TrTemplateApprovals on a.Id equals c.RefId
                                join d in context.TrUserApprovals on c.Id equals d.TrTempApprovalId
                                where !(a.IsDeleted ?? false) && d.StatusApproved == Models.Um.StatusApproved.CurrentApproval && d.EmployeeBasicInfoId == context.GetEmployeeId()
                                where !(a.IsDeleted ?? false)
                                select new MassLeave
                                {
                                    Id = a.Id,
                                    Year = a.Year,
                                    LeaveFrom = a.LeaveFrom,
                                    LeaveTo = a.LeaveTo,
                                    TotalDays = a.TotalDays,
                                    Reason = a.Reason,
                                    RequestBy = a.RequestBy,
                                    RequestDate = a.RequestDate,
                                    Status = c.StatusApproved.GetHashCode(),
                                    StatusDesc = c.StatusApprovedDescription,
                                    CreatedOn = a.CreatedOn
                                }).OrderByDescending(x => x.CreatedOn).ToListAsync();
            return result;
        }

        public async Task<MassLeave> Get(string Id)
        {
            var result = await (from a in context.MassLeaves
                                join c in context.TrTemplateApprovals on a.Id equals c.RefId
                                join b in context.EmployeeBasicInfos on a.RequestBy equals b.Id
                                where !(a.IsDeleted ?? false) && a.Id == Id
                                select new MassLeave
                                {
                                    Id = a.Id,
                                    Year = a.Year,
                                    LeaveFrom = a.LeaveFrom,
                                    LeaveTo = a.LeaveTo,
                                    TotalDays = a.TotalDays,
                                    Reason = a.Reason,
                                    RequestBy = a.RequestBy,
                                    RequestDate = a.RequestDate,
                                    Status = c.StatusApproved.GetHashCode(),
                                    StatusDesc = c.StatusApprovedDescription,
                                    CreatedOn = a.CreatedOn,
                                    CreatedBy = a.CreatedBy,
                                    requestEmployee = new InfoEmployee
                                    {
                                        EmployeeId = b.Id,
                                        EmployeeName = b.NameEmployee,
                                        //DateEmployed = b.JoinDate,
                                        //Phone = b.PhoneNumber,
                                        //Email = b.Email,
                                        //DateBirth = b.DateBirth,
                                        //Age = Convert.ToDateTime(b.DateBirth).AddYears(DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) > DateTime.Now ?
                                        //                      (DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) - 1 : DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year,
                                        JobTitle = b.BusinessUnitJobLevelId == null ? "" :
                                                                          (from r in context.BusinessUnitJobLevels
                                                                           join s in context.JobTitles on r.JobTitleId equals s.Id
                                                                           where r.Id == b.BusinessUnitJobLevelId
                                                                           select s.TitleName).FirstOrDefault(),
                                        Grade = b.BusinessUnitJobLevelId == null ? "" :
                                                                          (from r in context.BusinessUnitJobLevels
                                                                           join s in context.JobGrades on r.JobGradeId equals s.Id
                                                                           where r.Id == b.BusinessUnitJobLevelId
                                                                           select s.GradeName).FirstOrDefault(),
                                        //EmployementStatus = context.EmployementStatuss.Where(x => x.Id == b.EmployementStatusId).Select(x => x.EmploymentStatusName).FirstOrDefault()
                                    }
                                }).FirstOrDefaultAsync();

            if (result == null)
            {
                result = await (from a in context.MassLeaves
                                join b in context.EmployeeBasicInfos on a.RequestBy equals b.Id
                                where !(a.IsDeleted ?? false) && a.Id == Id && !context.TrTemplateApprovals.Any(x => x.RefId == a.Id)
                                select new MassLeave
                                {
                                    Id = a.Id,
                                    Year = a.Year,
                                    LeaveFrom = a.LeaveFrom,
                                    LeaveTo = a.LeaveTo,
                                    TotalDays = a.TotalDays,
                                    Reason = a.Reason,
                                    RequestBy = a.RequestBy,
                                    RequestDate = a.RequestDate,
                                    Status = StatusTransaction.Draft,
                                    StatusDesc = StatusTransaction.StatusName(StatusTransaction.Draft),
                                    CreatedOn = a.CreatedOn,
                                     CreatedBy = a.CreatedBy,
                                    requestEmployee = new InfoEmployee
                                    {
                                        EmployeeId = b.Id,
                                        EmployeeName = b.NameEmployee,
                                        //DateEmployed = b.JoinDate,
                                        //Phone = b.PhoneNumber,
                                        //Email = b.Email,
                                        //DateBirth = b.DateBirth,
                                        //Age = Convert.ToDateTime(b.DateBirth).AddYears(DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) > DateTime.Now ?
                                        //                      (DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) - 1 : DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year,
                                        JobTitle = b.BusinessUnitJobLevelId == null ? "" :
                                                                          (from r in context.BusinessUnitJobLevels
                                                                           join s in context.JobTitles on r.JobTitleId equals s.Id
                                                                           where r.Id == b.BusinessUnitJobLevelId
                                                                           select s.TitleName).FirstOrDefault(),
                                        Grade = b.BusinessUnitJobLevelId == null ? "" :
                                                                          (from r in context.BusinessUnitJobLevels
                                                                           join s in context.JobGrades on r.JobGradeId equals s.Id
                                                                           where r.Id == b.BusinessUnitJobLevelId
                                                                           select s.GradeName).FirstOrDefault(),
                                        //EmployementStatus = context.EmployementStatuss.Where(x => x.Id == b.EmployementStatusId).Select(x => x.EmploymentStatusName).FirstOrDefault()
                                    }
                                }).FirstOrDefaultAsync();
            }

            return result;
        }

        public async Task<List<MassLeaveDetail>> GetAssignList(string id)
        {
            List<MassLeaveDetail> result = new List<MassLeaveDetail>();
            var dt = await context.GetDataTable(string.Format("EXEC uspGetMassLeaveEmployeeById '{0}'", id), null);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    MassLeaveDetail m = new MassLeaveDetail();
                    InfoEmployee x = new InfoEmployee();
                    x.EmployeeId = dt.Rows[i]["EmployeeId"].ToString();
                    x.EmployeeName = dt.Rows[i]["EmployeeName"].ToString();
                    if (!string.IsNullOrEmpty(dt.Rows[i]["DateEmployed"].ToString()))
                    {
                        x.DateEmployed = DateTime.Parse(dt.Rows[i]["DateEmployed"].ToString());
                    }
                    x.Phone = dt.Rows[i]["Phone"].ToString();
                    x.Email = dt.Rows[i]["Email"].ToString();
                    x.EmployementStatusId = dt.Rows[i]["EmployementStatusId"].ToString();
                    x.LegalEntityId = dt.Rows[i]["LegalEntityId"].ToString();
                    x.LegalEntityName = dt.Rows[i]["LegalEntityName"].ToString();
                    x.JobTitleId = dt.Rows[i]["JobTitleId"].ToString();
                    x.JobTitle = dt.Rows[i]["JobTitle"].ToString();
                    x.GradeId = dt.Rows[i]["GradeId"].ToString();
                    x.Grade = dt.Rows[i]["Grade"].ToString();
                    x.BalanceLeaveCurrentYear = Convert.ToDecimal(dt.Rows[i]["BalanceLeaveCurrentYear"].ToString());
                    x.BalanceLeaveCarryOver = Convert.ToDecimal(dt.Rows[i]["BalanceLeaveCarryOver"].ToString());
                    m.InfoEmployee = x;
                    result.Add(m);
                }
            }
            //var result = await (from a in context.MassLeaveDetails
            //                    join b in context.EmployeeBasicInfos on a.ExceptEmployee equals b.Id
            //                    join c in context.LegalEntity on b.LegalEntityId equals c.Id
            //                    where a.MassLeaveId == id
            //                    select new MassLeaveDetail
            //                    {
            //                        Id = a.Id,
            //                        MassLeaveId = a.MassLeaveId,
            //                        ExceptEmployee = a.ExceptEmployee,
            //                        CreatedBy = a.CreatedBy,
            //                        CreatedOn = a.CreatedOn,
            //                        ModifiedBy = a.ModifiedBy,
            //                        ModifiedOn = a.ModifiedOn,
            //                        ApprovedBy = a.ApprovedBy,
            //                        ApprovedOn = a.ApprovedOn,
            //                        IsActive = a.IsActive,
            //                        IsLocked = a.IsLocked,
            //                        IsDefault = a.IsDefault,
            //                        IsDeleted = a.IsDeleted,
            //                        OwnerId = a.OwnerId,
            //                        DeletedBy = a.DeletedBy,
            //                        DeletedOn = a.DeletedOn,
            //                        BusinessUnitDivisionId = a.BusinessUnitDivisionId,
            //                        BusinessUnitDepartementId = a.BusinessUnitDepartementId,
            //                        InfoEmployee = new InfoEmployee
            //                        {
            //                            EmployeeId = b.Id,
            //                            EmployeeName = b.NameEmployee,
            //                            LegalEntityName = c.LegalEntityName,
            //                            BalanceLeaveCurrentYear = from q in context.LeaveBalances
            //                                                      where q.EmployeeBasicInfoId == b.Id && q.IsDeleted == false
            //                            //DateEmployed = b.JoinDate,
            //                            //Phone = b.PhoneNumber,
            //                            //Email = b.Email,
            //                            //DateBirth = b.DateBirth,
            //                            //Age = Convert.ToDateTime(b.DateBirth).AddYears(DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) > DateTime.Now ?
            //                            //                      (DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year) - 1 : DateTime.Now.Year - Convert.ToDateTime(b.DateBirth).Year,
            //                            JobTitle = b.BusinessUnitJobLevelId == null ? "" :
            //                                                              (from r in context.BusinessUnitJobLevels
            //                                                               join s in context.JobTitles on r.JobTitleId equals s.Id
            //                                                               where r.Id == b.BusinessUnitJobLevelId
            //                                                               select s.TitleName).FirstOrDefault(),
            //                            Grade = b.BusinessUnitJobLevelId == null ? "" :
            //                                                              (from r in context.BusinessUnitJobLevels
            //                                                               join s in context.JobGrades on r.JobGradeId equals s.Id
            //                                                               where r.Id == b.BusinessUnitJobLevelId
            //                                                               select s.GradeName).FirstOrDefault(),
            //                            //EmployementStatus = context.EmployementStatuss.Where(x => x.Id == b.EmployementStatusId).Select(x => x.EmploymentStatusName).FirstOrDefault(),
            //                            Division = b.BusinessUnitJobLevelId == null ? "" :
            //                                                            (from r in context.BusinessUnitJobLevels
            //                                                             join s in context.BusinessUnits on r.BusinessUnitDivisionId equals s.Id
            //                                                             join t in context.BusinessUnitTypes on s.BusinessUnitTypeId equals t.Id
            //                                                             where t.BusinessUnitLevel == BusinessUnitLevelCode.Division && r.Id == b.BusinessUnitJobLevelId
            //                                                             select s.UnitName).FirstOrDefault()
            //                        }
            //                    }).OrderByDescending(x => x.CreatedOn).ToListAsync();
            return result;
        }
    }
}