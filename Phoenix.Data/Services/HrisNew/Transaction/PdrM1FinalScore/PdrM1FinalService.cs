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

    public interface IPdrM1FinalService : IDataServiceHris<PdrApprailsalM1>
    {
        Task<List<PdrApprailsalM1>> GetPdrApprailsalM1List(string employeeId, DateTime? startReview);
        Task<PdrEmployee> GetDataPdrPerformance(string employeeId, DateTime? startReview);

    }


    public class PdrM1FinalService : IPdrM1FinalService
    {
        readonly DataContext context;
        readonly GlobalFunctionApproval globalFcApproval;
        readonly Services.Um.IManageMenuService menuService;

        public PdrM1FinalService(DataContext context, GlobalFunctionApproval globalFcApproval, Services.Um.IManageMenuService menuService)
        {
            this.context = context;
            this.globalFcApproval = globalFcApproval;
            this.menuService = menuService;
        }

        public async Task<List<PdrApprailsalM1>> GetPdrApprailsalM1List(string employeeId, DateTime? startReview)
        {
            var result = await (from a in context.PdrApprailsalM1s
                                join c in context.PdrApprailsalM1Headers on a.PdrApprailsalM1HeaderId equals c.Id
                                join b in context.PdrObjectiveSettings on a.PdrObjectiveSettingId equals b.Id
                                where c.EmployeeId == employeeId
                                select new PdrApprailsalM1
                                {
                                    Id = a.Id,
                                    PdrApprailsalM1HeaderId = a.PdrApprailsalM1HeaderId,
                                    PdrObjectiveSettingId = a.PdrObjectiveSettingId,
                                    Target = a.Target,
                                    Achievement = a.Achievement,
                                    Weight = a.Weight,
                                    RatingScore = a.RatingScore,
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
                                    PdrObjectiveSettingItem = b.ObjectiveItem,
                                    WeightedScore = a.RatingScore * (a.Weight / 100)
                                }).ToListAsync();

            if (result.Count == 0)
            {
                result = await (from a in context.PdrObjectiveTargets
                                join b in context.PdrObjectiveTargetDetails on a.Id equals b.PdrObjectiveTargetId
                                join c in context.PdrObjectiveSettings on b.PdrObjectiveSettingId equals c.Id
                                where a.EmployeeId == employeeId && a.Year == Convert.ToDateTime(startReview).Year
                                select new PdrApprailsalM1
                                {
                                    Id = a.Id,
                                    PdrObjectiveSettingId = b.PdrObjectiveSettingId,
                                    Target = b.Target,
                                    Achievement = 0,
                                    Weight = 0,
                                    RatingScore = 0,
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
                                    PdrObjectiveSettingItem = c.ObjectiveItem,
                                    WeightedScore = 0
                                }).ToListAsync();
            }
            return result;
        }

        public async Task<PdrEmployee> GetDataPdrPerformance(string employeeId, DateTime? startReview)
        {
            var result = new PdrEmployee();
            if (!string.IsNullOrEmpty(employeeId))
            {
                result = await (from a in context.PdrEmployees
                                join b in context.EmployeeBasicInfos on a.EmployeeBasicInfoId equals b.Id
                                join c in context.TrTemplateApprovals on a.Id equals c.RefId
                                where !(a.IsDeleted ?? false) && a.EmployeeBasicInfoId == employeeId
                                select new PdrEmployee
                                {
                                    Id = a.Id,
                                    EmployeeBasicInfoId = a.EmployeeBasicInfoId,
                                    Status = c.StatusApproved.GetHashCode(),
                                    StatusDesc = c.StatusApprovedDescription,
                                    ReviewStart = a.ReviewStart,
                                    ReviewEnd = a.ReviewEnd,
                                    DateAppraisal = a.DateAppraisal,
                                    FinalScore = a.FinalScore,
                                    Grade = a.Grade,
                                    DateSubmit = a.DateSubmit,
                                    FileResult = a.FileResult,
                                    SalaryIncrease = a.SalaryIncrease,
                                    JobGradeId = a.JobGradeId,
                                    Notes = a.Notes,
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
                                    PdrCategorys = context.PdrCategorys.Include(x => x.PdrParameters).Where(x => !(x.IsDeleted ?? false))
                                                    .Select(x => new PdrCategory
                                                    {
                                                        Id = x.Id,
                                                        No = x.No,
                                                        Name = x.Name,
                                                        Description = x.Description,
                                                        Percentage = x.Percentage,
                                                        PdrParameters = x.PdrParameters.Where(y => !(y.IsDeleted ?? false)).ToList()
                                                    }).ToList(),
                                    PdrGrades = context.PdrGrades.Where(x => !(x.IsDeleted ?? false)).ToList(),
                                    PdrRecomendation = context.PdrRecomendations.Where(x => x.PdrEmployeeId == a.Id).FirstOrDefault(),
                                    PdrDetailM2belows = context.PdrDetailM2belows.Where(x => x.PdrEmployeeId == a.Id).ToList(),
                                    PdrDevelopmentPlans = context.PdrDevelopmentPlans.Where(x => x.PdrEmployeeId == a.Id).ToList(),
                                    PdrOverallScores = context.PdrOverallScores.Where(x => x.PdrEmployeeId == a.Id).ToList(),
                                    infoEmployee = new InfoEmployee
                                    {
                                        EmployeeId = b.Id,
                                        EmployeeName = b.NameEmployee,
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
                                        EmployementStatus = context.EmployementStatuss.Where(x => x.Id == b.EmployementStatusId).Select(x => x.EmploymentStatusName).FirstOrDefault(),
                                    }
                                }).FirstOrDefaultAsync();


                if (result == null)
                {
                    result = await (from a in context.PdrEmployees
                                    join b in context.EmployeeBasicInfos on a.EmployeeBasicInfoId equals b.Id
                                    where !(a.IsDeleted ?? false) && a.EmployeeBasicInfoId == employeeId && !context.TrTemplateApprovals.Any(x => x.RefId == a.Id)
                                    select new PdrEmployee
                                    {
                                        Id = a.Id,
                                        EmployeeBasicInfoId = a.EmployeeBasicInfoId,
                                        Status = StatusTransaction.Draft,
                                        StatusDesc = StatusTransaction.StatusName(StatusTransaction.Draft),
                                        ReviewStart = a.ReviewStart,
                                        ReviewEnd = a.ReviewEnd,
                                        DateAppraisal = a.DateAppraisal,
                                        FinalScore = a.FinalScore,
                                        Grade = a.Grade,
                                        DateSubmit = a.DateSubmit,
                                        FileResult = a.FileResult,
                                        SalaryIncrease = a.SalaryIncrease,
                                        JobGradeId = a.JobGradeId,
                                        Notes = a.Notes,
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
                                        PdrCategorys = context.PdrCategorys.Include(x => x.PdrParameters).Where(x => !(x.IsDeleted ?? false))
                                                    .Select(x => new PdrCategory
                                                    {
                                                        Id = x.Id,
                                                        No = x.No,
                                                        Name = x.Name,
                                                        Description = x.Description,
                                                        Percentage = x.Percentage,
                                                        PdrParameters = x.PdrParameters.Where(y => !(y.IsDeleted ?? false)).ToList()
                                                    }).ToList(),
                                        PdrGrades = context.PdrGrades.Where(x => !(x.IsDeleted ?? false)).ToList(),
                                        PdrRecomendation = context.PdrRecomendations.Where(x => x.PdrEmployeeId == a.Id).FirstOrDefault(),
                                        PdrDetailM2belows = context.PdrDetailM2belows.Where(x => x.PdrEmployeeId == a.Id).ToList(),
                                        PdrDevelopmentPlans = context.PdrDevelopmentPlans.Where(x => x.PdrEmployeeId == a.Id).ToList(),
                                        PdrOverallScores = context.PdrOverallScores.Where(x => x.PdrEmployeeId == a.Id).ToList(),
                                        infoEmployee = new InfoEmployee
                                        {
                                            EmployeeId = b.Id,
                                            EmployeeName = b.NameEmployee,
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
                                            EmployementStatus = context.EmployementStatuss.Where(x => x.Id == b.EmployementStatusId).Select(x => x.EmploymentStatusName).FirstOrDefault(),
                                            
                                        }
                                    }).FirstOrDefaultAsync();
                }
            }
            else
            {
                result.PdrCategorys = await context.PdrCategorys.Include(x => x.PdrParameters).Where(x => !(x.IsDeleted ?? false))
                    .Select(x => new PdrCategory
                    {
                        Id = x.Id,
                        No = x.No,
                        Name = x.Name,
                        Description = x.Description,
                        Percentage = x.Percentage,
                        PdrParameters = x.PdrParameters.Where(y => !(y.IsDeleted ?? false)).ToList()
                    }).ToListAsync();
                result.PdrGrades = await context.PdrGrades.Where(x => !(x.IsDeleted ?? false)).ToListAsync();
            }
            return result;
        }
        public Task<int> AddAsync(PdrApprailsalM1 entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> ApproveAsync(PdrApprailsalM1 entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteSoftAsync(PdrApprailsalM1 entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> EditAsync(PdrApprailsalM1 entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<PdrApprailsalM1>> Get()
        {
            throw new NotImplementedException();
        }

        public Task<PdrApprailsalM1> Get(string id)
        {
            throw new NotImplementedException();
        }

      
      
    }
}
