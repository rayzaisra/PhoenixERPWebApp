using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Enum;
using Phoenix.Data.Services.Um;

namespace Phoenix.Data
{
    public interface IRotationService : IDataServiceHris<Rotation>
    {
        Task<List<Rotation>> GetApprovalList();
        Task<Rotation> GetEmployeeSalaryinfoById(string employeeId);
        Task<List<RotationSubcomponentAdditional>> GetAdditionalList(string employeeId, string rotationId);
        Task<List<RotationSubcomponentDeduction>> GetDeductionList(string employeeId, string rotationId);
    }

    public class RotationService : IRotationService
    {
        readonly DataContext context;
        readonly GlobalFunctionApproval globalFcApproval;
        readonly Services.Um.IManageMenuService menuService;
        private readonly IManageUserAppService userApp;

        string idTemplate = "";

        /// <summary>
        /// And endpoint to manage Rotation
        /// </summary>
        /// <param name="context">Database context</param>
        public RotationService(DataContext context, GlobalFunctionApproval globalFcApproval, Services.Um.IManageMenuService menuService, IManageUserAppService userApp)
        {
            this.context = context;
            this.globalFcApproval = globalFcApproval;
            this.menuService = menuService;
            this.userApp = userApp;
        }

        public async Task<int> AddAsync(Rotation entity)
        {
            return await AddEdit(entity, false);
        }

        public async Task<int> EditAsync(Rotation entity)
        {
            return await AddEdit(entity, true);
        }

        private async Task<int> AddEdit(Rotation entity, bool isEdit)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (isEdit)
                    {
                        context.PhoenixEdit(entity);
                    }
                    else
                    {
                        entity.Id = await TransID.GetTransId(context, Code.Rotation, DateTime.Today);
                        entity.RequestCode = entity.Id;
                        await context.PhoenixAddAsync(entity);
                        await context.SaveChangesAsync();
                    }

                    if (!string.IsNullOrEmpty(entity.Id))
                    {
                        var additional = context.RotationSubcomponentAdditionals.AsNoTracking().Where(x => x.AdditionalRotationId == entity.Id).ToList();
                        if (additional.Count > 0)
                        {
                            context.RotationSubcomponentAdditionals.RemoveRange(additional);
                        }

                        var deduction = context.RotationSubcomponentDeductions.AsNoTracking().Where(x => x.RotationId == entity.Id).ToList();
                        if (deduction.Count > 0)
                        {
                            context.RotationSubcomponentDeductions.RemoveRange(deduction);
                        }
                    }

                    if (entity.ComponentAdditionalList != null)
                    {
                        Parallel.ForEach(entity.ComponentAdditionalList, (item) =>
                        {
                            item.Id = Guid.NewGuid().ToString();
                            item.AdditionalRotationId = entity.Id;
                            context.PhoenixAddAsync(item);
                        });
                    }

                    if (entity.ComponentDeductionList != null)
                    {
                        Parallel.ForEach(entity.ComponentDeductionList, (item) =>
                        {
                            item.Id = Guid.NewGuid().ToString();
                            item.RotationId = entity.Id;
                            context.PhoenixAddAsync(item);
                        });
                    }

                    var isApprove = true;
                    if (entity.Status != StatusTransaction.Draft)
                    {
                        var dataMenu = await menuService.GetByUniqueName(MenuUnique.Rotation).ConfigureAwait(false);
                        if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
                        var vm = new Models.ViewModel.TransApprovalHrisVm()
                        {
                            MenuId = dataMenu.Id,
                            RefId = entity.Id,
                            DetailLink = $"{ApprovalLink.Rotation}?Id={entity.Id}"
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

        public async Task<int> ApproveAsync(Rotation entity)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var save = await globalFcApproval.UnsafeProcessApproval(entity.Id, entity.Status, entity.RemarkRejected, entity);
                    var approval = context.TrTemplateApprovals.Where(x => x.RefId == entity.Id && !(x.IsDeleted ?? false)).FirstOrDefault();
                    if (approval != null)
                    {
                        if (approval.StatusApproved == Models.Um.StatusApproved.Approved)
                        {
                        }
                    }

                    save = await context.SaveChangesAsync();
                    transaction.Commit();
                    return save;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<int> DeleteSoftAsync(Rotation entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<List<Rotation>> GetApprovalList()
        {
            var result = await (from a in context.Rotations
                                join b in context.EmployeeBasicInfos on a.Requester equals b.Id
                                join c in context.TrTemplateApprovals on a.Id equals c.RefId
                                join d in context.TrUserApprovals on c.Id equals d.TrTempApprovalId
                                where !(a.IsDeleted ?? false) && d.StatusApproved == Models.Um.StatusApproved.CurrentApproval && d.EmployeeBasicInfoId == context.GetEmployeeId()
                                select new Rotation
                                {
                                    Id = a.Id,
                                    RequestCode = a.RequestCode,
                                    RequestDate = a.RequestDate,
                                    StatusDesc = c.StatusApprovedDescription,
                                    CreatedOn = a.CreatedOn,
                                    requestEmployee = new InfoEmployee
                                    {
                                        //EmployeeId = b.Id,
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
                                        //EmployementStatus = context.EmployementStatuss.Where(x => x.Id == b.EmployementStatusId).Select(x => x.EmploymentStatusName).FirstOrDefault(),
                                        Division = b.BusinessUnitJobLevelId == null ? "" :
                                                                        (from r in context.BusinessUnitJobLevels
                                                                         join s in context.BusinessUnits on r.BusinessUnitDivisionId equals s.Id
                                                                         join t in context.BusinessUnitTypes on s.BusinessUnitTypeId equals t.Id
                                                                         where t.BusinessUnitLevel == BusinessUnitLevelCode.Division && r.Id == b.BusinessUnitJobLevelId
                                                                         select s.UnitName).FirstOrDefault()
                                    }
                                }).OrderByDescending(x => x.CreatedOn).ToListAsync();
            return result;
        }

        public async Task<List<Rotation>> Get()
        {
            var group = await userApp.GetByUserLogin();
            var HCAdmin = group.Where(x => x.GroupId == GroupAcess.HCAdmin).Count() > 0;
            var employeeId = context.GetEmployeeId();

            var result = await (from a in context.Rotations
                                join b in context.EmployeeBasicInfos on a.Requester equals b.Id
                                join c in context.TrTemplateApprovals on a.Id equals c.RefId
                                where !(a.IsDeleted ?? false) && (HCAdmin ? true : a.CreatedBy == employeeId)
                                select new Rotation
                                {
                                    Id = a.Id,
                                    RequestCode = a.RequestCode,
                                    RequestDate = a.RequestDate,
                                    StatusDesc = c.StatusApprovedDescription,
                                    CreatedOn = a.CreatedOn,
                                    requestEmployee = new InfoEmployee
                                    {
                                        //EmployeeId = b.Id,
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
                                        //EmployementStatus = context.EmployementStatuss.Where(x => x.Id == b.EmployementStatusId).Select(x => x.EmploymentStatusName).FirstOrDefault(),
                                        Division = b.BusinessUnitJobLevelId == null ? "" :
                                                                        (from r in context.BusinessUnitJobLevels
                                                                         join s in context.BusinessUnits on r.BusinessUnitDivisionId equals s.Id
                                                                         join t in context.BusinessUnitTypes on s.BusinessUnitTypeId equals t.Id
                                                                         where t.BusinessUnitLevel == BusinessUnitLevelCode.Division && r.Id == b.BusinessUnitJobLevelId
                                                                         select s.UnitName).FirstOrDefault()
                                    }
                                }).OrderByDescending(x => x.CreatedOn).ToListAsync();

            var result2 = await (from a in context.Rotations
                                 join b in context.EmployeeBasicInfos on a.Requester equals b.Id
                                 where !(a.IsDeleted ?? false) && !context.TrTemplateApprovals.Any(x => x.RefId == a.Id) && (HCAdmin ? true : a.CreatedBy == employeeId)
                                 select new Rotation
                                 {
                                     Id = a.Id,
                                     RequestCode = a.RequestCode,
                                     RequestDate = a.RequestDate,
                                     StatusDesc = StatusTransaction.StatusName(StatusTransaction.Draft),
                                     CreatedOn = a.CreatedOn,
                                     requestEmployee = new InfoEmployee
                                     {
                                         //EmployeeId = b.Id,
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
                                         //EmployementStatus = context.EmployementStatuss.Where(x => x.Id == b.EmployementStatusId).Select(x => x.EmploymentStatusName).FirstOrDefault(),
                                         Division = b.BusinessUnitJobLevelId == null ? "" :
                                                                         (from r in context.BusinessUnitJobLevels
                                                                          join s in context.BusinessUnits on r.BusinessUnitDivisionId equals s.Id
                                                                          join t in context.BusinessUnitTypes on s.BusinessUnitTypeId equals t.Id
                                                                          where t.BusinessUnitLevel == BusinessUnitLevelCode.Division && r.Id == b.BusinessUnitJobLevelId
                                                                          select s.UnitName).FirstOrDefault()
                                     }
                                 }).OrderByDescending(x => x.CreatedOn).ToListAsync();

            if (result == null) result = new List<Rotation>();
            result.AddRange(result2);
            return result.OrderByDescending(x => x.CreatedOn).ToList();
        }

        public async Task<Rotation> Get(string Id)
        {
            var result = await (from a in context.Rotations
                                join b in context.EmployeeBasicInfos on a.Requester equals b.Id
                                join c in context.EmployeeBasicInfos on a.EmployeeBasicInfoId equals c.Id
                                join d in context.TrTemplateApprovals on a.Id equals d.RefId
                                where !(a.IsDeleted ?? false) && a.Id == Id
                                select new Rotation
                                {
                                    Id = a.Id,
                                    EmployeeBasicInfoId = a.EmployeeBasicInfoId,
                                    EffectiveDate = a.EffectiveDate,
                                    Requester = a.Requester,
                                    RequestCode = a.RequestCode,
                                    RequestDate = a.RequestDate,
                                    EndDate = a.EndDate,
                                    RotationSubgroupId = a.RotationSubgroupId,
                                    RotationDivisionId = a.RotationDivisionId,
                                    RotationDepartmentId = a.RotationDepartmentId,
                                    JobLevelId = a.JobLevelId,
                                    JobGradeId = a.JobGradeId,
                                    JobTitleId = a.JobTitleId,
                                    LocationId = a.LocationId,
                                    Notes = a.Notes,
                                    Status = d.StatusApproved.GetHashCode(),
                                    IncrementPercentage = a.IncrementPercentage,
                                    CurrentSalaryZone = a.CurrentSalaryZone,
                                    NextSalaryZone = a.NextSalaryZone,
                                    CurrentSalary = a.CurrentSalary,
                                    NextSalary = a.NextSalary,
                                    SubmitUser = a.SubmitUser,
                                    IsSynchronize = a.IsSynchronize,
                                    CreatedBy = a.CreatedBy,
                                    CreatedOn = a.CreatedOn,
                                    ModifiedBy = a.ModifiedBy,
                                    ModifiedOn = a.ModifiedOn,
                                    ApprovedBy = a.ApprovedBy,
                                    ApprovedOn = a.ApprovedOn,
                                    IsActive = a.IsActive,
                                    IsLocked = a.IsLocked,
                                    IsDefault = a.IsDefault,
                                    IsDeleted = a.IsDeleted,
                                    OwnerId = a.OwnerId,
                                    DeletedBy = a.DeletedBy,
                                    DeletedOn = a.DeletedOn,
                                    BusinessUnitDivisionId = a.BusinessUnitDivisionId,
                                    BusinessUnitDepartementId = a.BusinessUnitDepartementId,
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
                                    },
                                    infoEmployee = new InfoEmployee
                                    {
                                        EmployeeId = c.Id,
                                        EmployeeName = c.NameEmployee,
                                        DateEmployed = c.JoinDate,
                                        Phone = c.PhoneNumber,
                                        Email = c.Email,
                                        DateBirth = c.DateBirth,
                                        Age = Convert.ToDateTime(c.DateBirth).AddYears(DateTime.Now.Year - Convert.ToDateTime(c.DateBirth).Year) > DateTime.Now ?
                                                              (DateTime.Now.Year - Convert.ToDateTime(c.DateBirth).Year) - 1 : DateTime.Now.Year - Convert.ToDateTime(c.DateBirth).Year,
                                        JobTitle = c.BusinessUnitJobLevelId == null ? "" :
                                                                          (from r in context.BusinessUnitJobLevels
                                                                           join s in context.JobTitles on r.JobTitleId equals s.Id
                                                                           where r.Id == c.BusinessUnitJobLevelId
                                                                           select s.TitleName).FirstOrDefault(),
                                        Grade = c.BusinessUnitJobLevelId == null ? "" :
                                                                          (from r in context.BusinessUnitJobLevels
                                                                           join s in context.JobGrades on r.JobGradeId equals s.Id
                                                                           where r.Id == c.BusinessUnitJobLevelId
                                                                           select s.GradeName).FirstOrDefault(),
                                        EmployementStatus = context.EmployementStatuss.Where(x => x.Id == c.EmployementStatusId).Select(x => x.EmploymentStatusName).FirstOrDefault(),
                                        EmployeeContractDocument = context.EmployeeContracts.Where(x => x.EmployeeBasicInfoId == a.EmployeeBasicInfoId).Select(x => x.FilemasterId).FirstOrDefault(),
                                        EmployeeContractDocumentName = (from r in context.EmployeeContracts
                                                                        join s in context.Filemasters on r.FilemasterId equals s.Id
                                                                        where r.EmployeeBasicInfoId == a.EmployeeBasicInfoId
                                                                        select s.Name).FirstOrDefault(),
                                        EmployeeJobDescription = "",
                                        EmployeeJobDescriptionName = ""
                                    }
                                }).FirstOrDefaultAsync();

            if (result == null)
            {
                result = await (from a in context.Rotations
                                join b in context.EmployeeBasicInfos on a.Requester equals b.Id
                                join c in context.EmployeeBasicInfos on a.EmployeeBasicInfoId equals c.Id
                                where !(a.IsDeleted ?? false) && a.Id == Id && !context.TrTemplateApprovals.Any(x => x.RefId == a.Id)
                                select new Rotation
                                {
                                    Id = a.Id,
                                    EmployeeBasicInfoId = a.EmployeeBasicInfoId,
                                    EffectiveDate = a.EffectiveDate,
                                    Requester = a.Requester,
                                    RequestCode = a.RequestCode,
                                    RequestDate = a.RequestDate,
                                    EndDate = a.EndDate,
                                    RotationSubgroupId = a.RotationSubgroupId,
                                    RotationDivisionId = a.RotationDivisionId,
                                    RotationDepartmentId = a.RotationDepartmentId,
                                    JobLevelId = a.JobLevelId,
                                    JobGradeId = a.JobGradeId,
                                    JobTitleId = a.JobTitleId,
                                    LocationId = a.LocationId,
                                    Notes = a.Notes,
                                    Status = StatusTransaction.Draft,//DRAFT
                                    IncrementPercentage = a.IncrementPercentage,
                                    CurrentSalaryZone = a.CurrentSalaryZone,
                                    NextSalaryZone = a.NextSalaryZone,
                                    CurrentSalary = a.CurrentSalary,
                                    NextSalary = a.NextSalary,
                                    SubmitUser = a.SubmitUser,
                                    IsSynchronize = a.IsSynchronize,
                                    CreatedBy = a.CreatedBy,
                                    CreatedOn = a.CreatedOn,
                                    ModifiedBy = a.ModifiedBy,
                                    ModifiedOn = a.ModifiedOn,
                                    ApprovedBy = a.ApprovedBy,
                                    ApprovedOn = a.ApprovedOn,
                                    IsActive = a.IsActive,
                                    IsLocked = a.IsLocked,
                                    IsDefault = a.IsDefault,
                                    IsDeleted = a.IsDeleted,
                                    OwnerId = a.OwnerId,
                                    DeletedBy = a.DeletedBy,
                                    DeletedOn = a.DeletedOn,
                                    BusinessUnitDivisionId = a.BusinessUnitDivisionId,
                                    BusinessUnitDepartementId = a.BusinessUnitDepartementId,
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
                                    },
                                    infoEmployee = new InfoEmployee
                                    {
                                        EmployeeId = c.Id,
                                        EmployeeName = c.NameEmployee,
                                        DateEmployed = c.JoinDate,
                                        Phone = c.PhoneNumber,
                                        Email = c.Email,
                                        DateBirth = c.DateBirth,
                                        Age = Convert.ToDateTime(c.DateBirth).AddYears(DateTime.Now.Year - Convert.ToDateTime(c.DateBirth).Year) > DateTime.Now ?
                                                             (DateTime.Now.Year - Convert.ToDateTime(c.DateBirth).Year) - 1 : DateTime.Now.Year - Convert.ToDateTime(c.DateBirth).Year,
                                        JobTitle = c.BusinessUnitJobLevelId == null ? "" :
                                                                         (from r in context.BusinessUnitJobLevels
                                                                          join s in context.JobTitles on r.JobTitleId equals s.Id
                                                                          where r.Id == c.BusinessUnitJobLevelId
                                                                          select s.TitleName).FirstOrDefault(),
                                        Grade = c.BusinessUnitJobLevelId == null ? "" :
                                                                         (from r in context.BusinessUnitJobLevels
                                                                          join s in context.JobGrades on r.JobGradeId equals s.Id
                                                                          where r.Id == c.BusinessUnitJobLevelId
                                                                          select s.GradeName).FirstOrDefault(),
                                        EmployementStatus = context.EmployementStatuss.Where(x => x.Id == c.EmployementStatusId).Select(x => x.EmploymentStatusName).FirstOrDefault(),
                                        EmployeeContractDocument = context.EmployeeContracts.Where(x => x.EmployeeBasicInfoId == a.EmployeeBasicInfoId).Select(x => x.FilemasterId).FirstOrDefault(),
                                        EmployeeContractDocumentName = (from r in context.EmployeeContracts
                                                                        join s in context.Filemasters on r.FilemasterId equals s.Id
                                                                        where r.EmployeeBasicInfoId == a.EmployeeBasicInfoId
                                                                        select s.Name).FirstOrDefault(),
                                        EmployeeJobDescription = "",
                                        EmployeeJobDescriptionName = ""
                                    }
                                }).FirstOrDefaultAsync();
            }
            return result;
        }

        public async Task<Rotation> GetEmployeeSalaryinfoById(string employeeId)
        {
            var requestor = await (from b in context.EmployeeBasicInfos
                                   where b.Id == employeeId
                                   select new Rotation
                                   {
                                       infoEmployee = new InfoEmployee
                                       {
                                           EmployeeId = b.Id,
                                           EmployeeName = b.NameEmployee,
                                           DateEmployed = b.JoinDate,
                                           Phone = b.PhoneNumber,
                                           Email = b.Email
                                       }
                                   }).FirstOrDefaultAsync();

            if (requestor != null)
            {
                RequestorService requestorService = new RequestorService(context);
                requestor.infoEmployee = requestorService.GetEmployeeInfo(requestor.infoEmployee);
            }

            requestor.CurrentSalary = context.EmployeeSubcomponentAdditionals.Where(x => !(x.IsDeleted ?? false) && employeeId == x.AdditionalEmployeeBasicInfoId &&
             (x.AdditionalSalaryComponent == SalaryComponentTypeAdditional.Allowance || x.AdditionalSalaryComponent == SalaryComponentTypeAdditional.SalaryComponent)).Sum(x => Convert.ToDecimal (x.AdditionalAmount));
            return requestor;
        }

        public async Task<List<RotationSubcomponentAdditional>> GetAdditionalList(string employeeId, string rotationId)
        {
            var result = new List<RotationSubcomponentAdditional>();
            if (context.RotationSubcomponentAdditionals.Where(x => x.AdditionalRotationId == rotationId).Count() > 0)
            {
                var salaryComponent = await (from a in context.RotationSubcomponentAdditionals
                                             join b in context.SalaryMainComponents on a.AdditionalComponentId equals b.Id
                                             where a.AdditionalSalaryComponent == SalaryComponentTypeAdditional.SalaryComponent &&
                                             !(a.IsDeleted ?? false) && a.AdditionalEmployeeBasicInfoId == employeeId && a.AdditionalRotationId == rotationId
                                             select new RotationSubcomponentAdditional
                                             {
                                                 Id = a.Id,
                                                 AdditionalEmployeeBasicInfoId = a.AdditionalEmployeeBasicInfoId,
                                                 AdditionalSalaryComponent = a.AdditionalSalaryComponent,
                                                 AdditionalComponentId = a.AdditionalComponentId,
                                                 AdditionalPercent = a.AdditionalPercent,
                                                 AdditionalAmount = a.AdditionalAmount,
                                                 Type = a.Type,
                                                 IsBasicSalary = b.Id == BasicSalary.Id ? true : false,
                                                 CreatedBy = a.CreatedBy,
                                                 CreatedOn = a.CreatedOn,
                                                 ModifiedBy = a.ModifiedBy,
                                                 ModifiedOn = a.ModifiedOn,
                                                 ApprovedBy = a.ApprovedBy,
                                                 ApprovedOn = a.ApprovedOn,
                                                 IsActive = a.IsActive,
                                                 IsLocked = a.IsLocked,
                                                 IsDefault = a.IsDefault,
                                                 IsDeleted = a.IsDeleted,
                                                 OwnerId = a.OwnerId,
                                                 DeletedBy = a.DeletedBy,
                                                 DeletedOn = a.DeletedOn,
                                                 BusinessUnitDivisionId = a.BusinessUnitDivisionId,
                                                 BusinessUnitDepartementId = a.BusinessUnitDepartementId,
                                                 AdditionalComponentTypeName = b.SubComponentName
                                             }).OrderByDescending(x => x.CreatedOn).ToListAsync();

                var allowance = await (from a in context.RotationSubcomponentAdditionals
                                       join b in context.AllowanceComponents on a.AdditionalComponentId equals b.Id
                                       where a.AdditionalSalaryComponent == SalaryComponentTypeAdditional.Allowance && !(a.IsDeleted ?? false) &&
                                       a.AdditionalEmployeeBasicInfoId == employeeId && a.AdditionalRotationId == rotationId
                                       select new RotationSubcomponentAdditional
                                       {
                                           Id = a.Id,
                                           AdditionalEmployeeBasicInfoId = a.AdditionalEmployeeBasicInfoId,
                                           AdditionalSalaryComponent = a.AdditionalSalaryComponent,
                                           AdditionalComponentId = a.AdditionalComponentId,
                                           AdditionalPercent = a.AdditionalPercent,
                                           AdditionalAmount = a.AdditionalAmount,
                                           Type = a.Type,
                                           CreatedBy = a.CreatedBy,
                                           CreatedOn = a.CreatedOn,
                                           ModifiedBy = a.ModifiedBy,
                                           ModifiedOn = a.ModifiedOn,
                                           ApprovedBy = a.ApprovedBy,
                                           ApprovedOn = a.ApprovedOn,
                                           IsActive = a.IsActive,
                                           IsLocked = a.IsLocked,
                                           IsDefault = a.IsDefault,
                                           IsDeleted = a.IsDeleted,
                                           OwnerId = a.OwnerId,
                                           DeletedBy = a.DeletedBy,
                                           DeletedOn = a.DeletedOn,
                                           BusinessUnitDivisionId = a.BusinessUnitDivisionId,
                                           BusinessUnitDepartementId = a.BusinessUnitDepartementId,
                                           AdditionalComponentTypeName = b.SubComponentName
                                       }).OrderByDescending(x => x.CreatedOn).ToListAsync();

                var bpjs = await (from a in context.RotationSubcomponentAdditionals
                                  join b in context.BpjsComponents on a.AdditionalComponentId equals b.Id
                                  where a.AdditionalSalaryComponent == SalaryComponentTypeAdditional.BPJS && b.Type == BPJSType.Additional && !(a.IsDeleted ?? false) && a.AdditionalEmployeeBasicInfoId == employeeId
                                  select new RotationSubcomponentAdditional
                                  {
                                      Id = a.Id,
                                      AdditionalEmployeeBasicInfoId = a.AdditionalEmployeeBasicInfoId,
                                      AdditionalSalaryComponent = a.AdditionalSalaryComponent,
                                      AdditionalComponentId = a.AdditionalComponentId,
                                      AdditionalPercent = a.AdditionalPercent,
                                      AdditionalAmount = a.AdditionalAmount,
                                      Type = a.Type,
                                      CreatedBy = a.CreatedBy,
                                      CreatedOn = a.CreatedOn,
                                      ModifiedBy = a.ModifiedBy,
                                      ModifiedOn = a.ModifiedOn,
                                      ApprovedBy = a.ApprovedBy,
                                      ApprovedOn = a.ApprovedOn,
                                      IsActive = a.IsActive,
                                      IsLocked = a.IsLocked,
                                      IsDefault = a.IsDefault,
                                      IsDeleted = a.IsDeleted,
                                      OwnerId = a.OwnerId,
                                      DeletedBy = a.DeletedBy,
                                      DeletedOn = a.DeletedOn,
                                      BusinessUnitDivisionId = a.BusinessUnitDivisionId,
                                      BusinessUnitDepartementId = a.BusinessUnitDepartementId,
                                      AdditionalComponentTypeName = b.SubComponentName
                                  }).OrderByDescending(x => x.CreatedOn).ToListAsync();

                result.AddRange(salaryComponent);
                result.AddRange(allowance);
                result.AddRange(bpjs);
            }
            else
            {
                var salaryComponent = await (from a in context.EmployeeSubcomponentAdditionals
                                             join b in context.SalaryMainComponents on a.AdditionalComponentId equals b.Id
                                             where a.AdditionalSalaryComponent == SalaryComponentTypeAdditional.SalaryComponent && !(a.IsDeleted ?? false) && a.AdditionalEmployeeBasicInfoId == employeeId
                                             select new RotationSubcomponentAdditional
                                             {
                                                 Id = a.Id,
                                                 AdditionalEmployeeBasicInfoId = a.AdditionalEmployeeBasicInfoId,
                                                 AdditionalSalaryComponent = a.AdditionalSalaryComponent,
                                                 AdditionalComponentId = a.AdditionalComponentId,
                                                 AdditionalPercent = Convert.ToDecimal(a.AdditionalPercent),
                                                 AdditionalAmount = Convert.ToDecimal(a.AdditionalAmount),
                                                 Type = Payroll.Type.CurrentSallary,
                                                 IsBasicSalary = b.Id == BasicSalary.Id ? true : false,
                                                 CreatedBy = a.CreatedBy,
                                                 CreatedOn = a.CreatedOn,
                                                 ModifiedBy = a.ModifiedBy,
                                                 ModifiedOn = a.ModifiedOn,
                                                 ApprovedBy = a.ApprovedBy,
                                                 ApprovedOn = a.ApprovedOn,
                                                 IsActive = a.IsActive,
                                                 IsLocked = a.IsLocked,
                                                 IsDefault = a.IsDefault,
                                                 IsDeleted = a.IsDeleted,
                                                 OwnerId = a.OwnerId,
                                                 DeletedBy = a.DeletedBy,
                                                 DeletedOn = a.DeletedOn,
                                                 BusinessUnitDivisionId = a.BusinessUnitDivisionId,
                                                 BusinessUnitDepartementId = a.BusinessUnitDepartementId,
                                                 AdditionalComponentTypeName = b.SubComponentName
                                             }).OrderByDescending(x => x.CreatedOn).ToListAsync();

                var allowance = await (from a in context.EmployeeSubcomponentAdditionals
                                       join b in context.AllowanceComponents on a.AdditionalComponentId equals b.Id
                                       where a.AdditionalSalaryComponent == SalaryComponentTypeAdditional.Allowance && !(a.IsDeleted ?? false) && a.AdditionalEmployeeBasicInfoId == employeeId
                                       select new RotationSubcomponentAdditional
                                       {
                                           Id = a.Id,
                                           AdditionalEmployeeBasicInfoId = a.AdditionalEmployeeBasicInfoId,
                                           AdditionalSalaryComponent = a.AdditionalSalaryComponent,
                                           AdditionalComponentId = a.AdditionalComponentId,
                                           AdditionalPercent = Convert.ToDecimal(a.AdditionalPercent),
                                           AdditionalAmount = Convert.ToDecimal(a.AdditionalAmount),
                                           Type = Payroll.Type.CurrentSallary,
                                           CreatedBy = a.CreatedBy,
                                           CreatedOn = a.CreatedOn,
                                           ModifiedBy = a.ModifiedBy,
                                           ModifiedOn = a.ModifiedOn,
                                           ApprovedBy = a.ApprovedBy,
                                           ApprovedOn = a.ApprovedOn,
                                           IsActive = a.IsActive,
                                           IsLocked = a.IsLocked,
                                           IsDefault = a.IsDefault,
                                           IsDeleted = a.IsDeleted,
                                           OwnerId = a.OwnerId,
                                           DeletedBy = a.DeletedBy,
                                           DeletedOn = a.DeletedOn,
                                           BusinessUnitDivisionId = a.BusinessUnitDivisionId,
                                           BusinessUnitDepartementId = a.BusinessUnitDepartementId,
                                           AdditionalComponentTypeName = b.SubComponentName
                                       }).OrderByDescending(x => x.CreatedOn).ToListAsync();

                var bpjs = await (from a in context.EmployeeSubcomponentAdditionals
                                  join b in context.BpjsComponents on a.AdditionalComponentId equals b.Id
                                  where a.AdditionalSalaryComponent == SalaryComponentTypeAdditional.BPJS && b.Type == BPJSType.Additional && !(a.IsDeleted ?? false) && a.AdditionalEmployeeBasicInfoId == employeeId
                                  select new RotationSubcomponentAdditional
                                  {
                                      Id = a.Id,
                                      AdditionalEmployeeBasicInfoId = a.AdditionalEmployeeBasicInfoId,
                                      AdditionalSalaryComponent = a.AdditionalSalaryComponent,
                                      AdditionalComponentId = a.AdditionalComponentId,
                                      AdditionalPercent = Convert.ToDecimal(a.AdditionalPercent),
                                      AdditionalAmount = Convert.ToDecimal(a.AdditionalAmount),
                                      Type = Payroll.Type.CurrentSallary,
                                      CreatedBy = a.CreatedBy,
                                      CreatedOn = a.CreatedOn,
                                      ModifiedBy = a.ModifiedBy,
                                      ModifiedOn = a.ModifiedOn,
                                      ApprovedBy = a.ApprovedBy,
                                      ApprovedOn = a.ApprovedOn,
                                      IsActive = a.IsActive,
                                      IsLocked = a.IsLocked,
                                      IsDefault = a.IsDefault,
                                      IsDeleted = a.IsDeleted,
                                      OwnerId = a.OwnerId,
                                      DeletedBy = a.DeletedBy,
                                      DeletedOn = a.DeletedOn,
                                      BusinessUnitDivisionId = a.BusinessUnitDivisionId,
                                      BusinessUnitDepartementId = a.BusinessUnitDepartementId,
                                      AdditionalComponentTypeName = b.SubComponentName
                                  }).OrderByDescending(x => x.CreatedOn).ToListAsync();

                result.AddRange(salaryComponent);
                result.AddRange(allowance);
                result.AddRange(bpjs);
            }

            if (result == null)
            {
                result = new List<RotationSubcomponentAdditional>();
            }
            return result;
        }

        public async Task<List<RotationSubcomponentDeduction>> GetDeductionList(string employeeId, string rotationId)
        {
            var result = new List<RotationSubcomponentDeduction>();
            if (context.RotationSubcomponentDeductions.Where(x => x.RotationId == rotationId).Count() > 0)
            {
                var deduction = await (from a in context.RotationSubcomponentDeductions
                                       join b in context.DeductionComponents on a.ComponentId equals b.Id
                                       where a.SalaryComponent == SalaryComponentTypeDeduction.Deduction &&
                                       !(a.IsDeleted ?? false) && a.EmployeeBasicInfoId == employeeId && a.RotationId == rotationId
                                       select new RotationSubcomponentDeduction
                                       {
                                           Id = a.Id,
                                           EmployeeBasicInfoId = a.EmployeeBasicInfoId,
                                           SalaryComponent = a.SalaryComponent,
                                           ComponentId = a.ComponentId,
                                           DeductionPercent = a.DeductionPercent,
                                           DeductionAmount = a.DeductionAmount,
                                           Type = a.Type,
                                           CreatedBy = a.CreatedBy,
                                           CreatedOn = a.CreatedOn,
                                           ModifiedBy = a.ModifiedBy,
                                           ModifiedOn = a.ModifiedOn,
                                           ApprovedBy = a.ApprovedBy,
                                           ApprovedOn = a.ApprovedOn,
                                           IsActive = a.IsActive,
                                           IsLocked = a.IsLocked,
                                           IsDefault = a.IsDefault,
                                           IsDeleted = a.IsDeleted,
                                           OwnerId = a.OwnerId,
                                           DeletedBy = a.DeletedBy,
                                           DeletedOn = a.DeletedOn,
                                           BusinessUnitDivisionId = a.BusinessUnitDivisionId,
                                           BusinessUnitDepartementId = a.BusinessUnitDepartementId,
                                           ComponentTypeName = b.SubComponentName
                                       }).OrderByDescending(x => x.CreatedOn).ToListAsync();

                var bpjs = await (from a in context.RotationSubcomponentDeductions
                                  join b in context.BpjsComponents on a.ComponentId equals b.Id
                                  where a.SalaryComponent == SalaryComponentTypeDeduction.BPJS && b.Type == BPJSType.Deduction &&
                                  !(a.IsDeleted ?? false) && a.EmployeeBasicInfoId == employeeId && a.RotationId == rotationId
                                  select new RotationSubcomponentDeduction
                                  {
                                      Id = a.Id,
                                      EmployeeBasicInfoId = a.EmployeeBasicInfoId,
                                      SalaryComponent = a.SalaryComponent,
                                      ComponentId = a.ComponentId,
                                      DeductionPercent = a.DeductionPercent,
                                      DeductionAmount = a.DeductionAmount,
                                      Type = a.Type,
                                      CreatedBy = a.CreatedBy,
                                      CreatedOn = a.CreatedOn,
                                      ModifiedBy = a.ModifiedBy,
                                      ModifiedOn = a.ModifiedOn,
                                      ApprovedBy = a.ApprovedBy,
                                      ApprovedOn = a.ApprovedOn,
                                      IsActive = a.IsActive,
                                      IsLocked = a.IsLocked,
                                      IsDefault = a.IsDefault,
                                      IsDeleted = a.IsDeleted,
                                      OwnerId = a.OwnerId,
                                      DeletedBy = a.DeletedBy,
                                      DeletedOn = a.DeletedOn,
                                      BusinessUnitDivisionId = a.BusinessUnitDivisionId,
                                      BusinessUnitDepartementId = a.BusinessUnitDepartementId,
                                      ComponentTypeName = b.SubComponentName
                                  }).OrderByDescending(x => x.CreatedOn).ToListAsync();

                result.AddRange(deduction);
                result.AddRange(bpjs);

            }
            else
            {
                var deduction = await (from a in context.EmployeeSubcomponentDeductions
                                       join b in context.DeductionComponents on a.ComponentId equals b.Id
                                       where a.SalaryComponent == SalaryComponentTypeDeduction.Deduction &&
                                       !(a.IsDeleted ?? false) && a.EmployeeBasicInfoId == employeeId
                                       select new RotationSubcomponentDeduction
                                       {
                                           Id = a.Id,
                                           EmployeeBasicInfoId = a.EmployeeBasicInfoId,
                                           SalaryComponent = a.SalaryComponent,
                                           ComponentId = a.ComponentId,
                                           DeductionPercent = Convert.ToDecimal(a.Percent),
                                           DeductionAmount = Convert.ToDecimal(a.Amount),
                                           Type = Payroll.Type.CurrentSallary,
                                           CreatedBy = a.CreatedBy,
                                           CreatedOn = a.CreatedOn,
                                           ModifiedBy = a.ModifiedBy,
                                           ModifiedOn = a.ModifiedOn,
                                           ApprovedBy = a.ApprovedBy,
                                           ApprovedOn = a.ApprovedOn,
                                           IsActive = a.IsActive,
                                           IsLocked = a.IsLocked,
                                           IsDefault = a.IsDefault,
                                           IsDeleted = a.IsDeleted,
                                           OwnerId = a.OwnerId,
                                           DeletedBy = a.DeletedBy,
                                           DeletedOn = a.DeletedOn,
                                           BusinessUnitDivisionId = a.BusinessUnitDivisionId,
                                           BusinessUnitDepartementId = a.BusinessUnitDepartementId,
                                           ComponentTypeName = b.SubComponentName
                                       }).OrderByDescending(x => x.CreatedOn).ToListAsync();

                var bpjs = await (from a in context.EmployeeSubcomponentDeductions
                                  join b in context.BpjsComponents on a.ComponentId equals b.Id
                                  where a.SalaryComponent == SalaryComponentTypeDeduction.BPJS && b.Type == BPJSType.Deduction &&
                                  !(a.IsDeleted ?? false) && a.EmployeeBasicInfoId == employeeId
                                  select new RotationSubcomponentDeduction
                                  {
                                      Id = a.Id,
                                      EmployeeBasicInfoId = a.EmployeeBasicInfoId,
                                      SalaryComponent = a.SalaryComponent,
                                      ComponentId = a.ComponentId,
                                      DeductionPercent = Convert.ToDecimal(a.Percent),
                                           DeductionAmount = Convert.ToDecimal(a.Amount),
                                      Type = Payroll.Type.CurrentSallary,
                                      CreatedBy = a.CreatedBy,
                                      CreatedOn = a.CreatedOn,
                                      ModifiedBy = a.ModifiedBy,
                                      ModifiedOn = a.ModifiedOn,
                                      ApprovedBy = a.ApprovedBy,
                                      ApprovedOn = a.ApprovedOn,
                                      IsActive = a.IsActive,
                                      IsLocked = a.IsLocked,
                                      IsDefault = a.IsDefault,
                                      IsDeleted = a.IsDeleted,
                                      OwnerId = a.OwnerId,
                                      DeletedBy = a.DeletedBy,
                                      DeletedOn = a.DeletedOn,
                                      BusinessUnitDivisionId = a.BusinessUnitDivisionId,
                                      BusinessUnitDepartementId = a.BusinessUnitDepartementId,
                                      ComponentTypeName = b.SubComponentName
                                  }).OrderByDescending(x => x.CreatedOn).ToListAsync();

                result.AddRange(deduction);
                result.AddRange(bpjs);
            }

            if (result == null)
            {
                result = new List<RotationSubcomponentDeduction>();
            }
            return result;
        }
    }
}