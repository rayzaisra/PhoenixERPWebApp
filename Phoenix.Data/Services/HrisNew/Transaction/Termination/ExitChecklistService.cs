using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phoenix.ApiExtension.Extensions;
using Phoenix.Data.Models;
using Phoenix.Data.RestApi;
using Phoenix.Shared.Core.Contexts;

namespace Phoenix.Data
{
    public interface IExitChecklistService : IDataServiceHris<Termination>
    {
        Task<List<ExitChecklist>> GetJobHandoverList(string terminationId, string employeeBasicInfoId);
        Task<List<ExitChecklist>> GetOfficeManagementList(string terminationId, string employeeBasicInfoId);
        Task<List<ExitChecklist>> GetInformationTechnologyList(string terminationId, string employeeBasicInfoId);
        Task<List<ExitChecklist>> GetFinanceList(string terminationId, string employeeBasicInfoId);
        Task<List<ExitChecklist>> GetHumanResourceList(string terminationId, string employeeBasicInfoId);
        Task<List<ExitChecklist>> GetOtherList(string terminationId, string employeeBasicInfoId);
        Task<List<ExitChecklist>> GetFinalHandoverList(string terminationId, string employeeBasicInfoId);
    }

    public class ExitChecklistService : IExitChecklistService
    {
        readonly DataContext context;
        string[] terminationType = new string[] {TerminationType.Resignation,
            TerminationType.EndOfContract, TerminationType.EndOfProbation,
            TerminationType.Death, TerminationType.ProlongedIllness,
            TerminationType.Sanction, TerminationType.Retired};

        /// <summary>
        /// And endpoint to manage Termination
        /// </summary>
        /// <param name="context">Database context</param>
        public ExitChecklistService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(Termination entity)
        {
            List<ExitChecklist> list = new List<ExitChecklist>();
            if (entity.JobHandover != null) list.AddRange(entity.JobHandover);
            if (entity.OfficeManagement != null) list.AddRange(entity.OfficeManagement);
            if (entity.InformationTechnology != null) list.AddRange(entity.InformationTechnology);
            if (entity.Finance != null) list.AddRange(entity.Finance);
            if (entity.HumanResource != null) list.AddRange(entity.HumanResource);
            if (entity.Other != null) list.AddRange(entity.Other);
            if (entity.FinalHandover != null) list.AddRange(entity.FinalHandover);

            var exitChecklistCount = list.Where(x => x.TerminationId == entity.Id && !(x.IsDeleted ?? false)).Count();
            var doneCount = list.Where(x => x.TerminationId == entity.Id && !(x.IsDeleted ?? false) && x.IsDone == true).Count();

            //if (exitChecklistCount == 0)
            //{
            //    throw new Exception("nothing is processed in the exit checklist");
            //}

            entity.StatusChecklist = TerminationExitChecklistStatus.OnProgress;

            if (entity.JobHandover != null)
            {
                foreach (var item in entity.JobHandover)
                {
                    if (context.ExitChecklists.Where(x => x.TerminationId == entity.Id && !(x.IsDeleted ?? false) && x.Id == item.Id).Count() > 0)
                    {
                        context.PhoenixEditRange(list.Where(x => x.Id == item.Id).ToArray());
                    }
                    else
                    {
                        context.PhoenixAddRange(list.Where(x => x.Id == item.Id).ToArray());
                    }
                }
            }


            if (entity.OfficeManagement != null)
            {
                foreach (var item in entity.OfficeManagement)
                {
                    if (context.ExitChecklists.Where(x => x.TerminationId == entity.Id && !(x.IsDeleted ?? false) && x.Id == item.Id).Count() > 0)
                    {
                        context.PhoenixEditRange(list.Where(x => x.Id == item.Id).ToArray());
                    }
                    else
                    {
                        context.PhoenixAddRange(list.Where(x => x.Id == item.Id).ToArray());
                    }
                }
            }

            if (entity.InformationTechnology != null)
            {
                foreach (var item in entity.InformationTechnology)
                {
                    if (context.ExitChecklists.Where(x => x.TerminationId == entity.Id && !(x.IsDeleted ?? false) && x.Id == item.Id).Count() > 0)
                    {
                        context.PhoenixEditRange(list.Where(x => x.Id == item.Id).ToArray());
                    }
                    else
                    {
                        context.PhoenixAddRange(list.Where(x => x.Id == item.Id).ToArray());
                    }
                }
            }

            if (entity.Finance != null)
            {
                foreach (var item in entity.Finance)
                {
                    if (context.ExitChecklists.Where(x => x.TerminationId == entity.Id && !(x.IsDeleted ?? false) && x.Id == item.Id).Count() > 0)
                    {
                        context.PhoenixEditRange(list.Where(x => x.Id == item.Id).ToArray());
                    }
                    else
                    {
                        context.PhoenixAddRange(list.Where(x => x.Id == item.Id).ToArray());
                    }
                }
            }

            if (entity.HumanResource != null)
            {
                foreach (var item in entity.HumanResource)
                {
                    if (context.ExitChecklists.Where(x => x.TerminationId == entity.Id && !(x.IsDeleted ?? false) && x.Id == item.Id).Count() > 0)
                    {
                        context.PhoenixEditRange(list.Where(x => x.Id == item.Id).ToArray());
                    }
                    else
                    {
                        context.PhoenixAddRange(list.Where(x => x.Id == item.Id).ToArray());
                    }
                }
            }


            if (entity.Other != null)
            {
                foreach (var item in entity.Other)
                {
                    if (context.ExitChecklists.Where(x => x.TerminationId == entity.Id && !(x.IsDeleted ?? false) && x.Id == item.Id).Count() > 0)
                    {
                        context.PhoenixEditRange(list.Where(x => x.Id == item.Id).ToArray());
                    }
                    else
                    {
                        context.PhoenixAddRange(list.Where(x => x.Id == item.Id).ToArray());
                    }
                }
            }

            if (entity.FinalHandover != null)
            {
                foreach (var item in entity.FinalHandover)
                {
                    if (context.ExitChecklists.Where(x => x.TerminationId == entity.Id && !(x.IsDeleted ?? false) && x.Id == item.Id).Count() > 0)
                    {
                        context.PhoenixEditRange(list.Where(x => x.Id == item.Id).ToArray());
                    }
                    else
                    {
                        context.PhoenixAddRange(list.Where(x => x.Id == item.Id).ToArray());
                    }
                }
            }
            

            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> ApproveAsync(Termination entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteSoftAsync(Termination entity)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(Termination entity)
        {
            List<ExitChecklist> list = new List<ExitChecklist>();
            if (entity.JobHandover != null) list.AddRange(entity.JobHandover);
            if (entity.OfficeManagement != null) list.AddRange(entity.OfficeManagement);
            if (entity.InformationTechnology != null) list.AddRange(entity.InformationTechnology);
            if (entity.Finance != null) list.AddRange(entity.Finance);
            if (entity.HumanResource != null) list.AddRange(entity.HumanResource);
            if (entity.Other != null) list.AddRange(entity.Other);
            if (entity.FinalHandover != null) list.AddRange(entity.FinalHandover);

            var exitChecklistCount = list.Where(x => x.TerminationId == entity.Id && !(x.IsDeleted ?? false)).Count();
            var doneCount = list.Where(x => x.TerminationId == entity.Id && !(x.IsDeleted ?? false) && x.IsDone == true).Count();

            //if (exitChecklistCount == 0)
            //{
            //    throw new Exception("nothing is processed in the exit checklist");
            //}

            if (doneCount == exitChecklistCount)
            {
                entity.StatusChecklist = TerminationExitChecklistStatus.Done;
                entity.IsChecklist = true;
            }
            else if (doneCount > 0)
            {
                entity.StatusChecklist = TerminationExitChecklistStatus.OnProgress;
            }
            else
            {
                entity.StatusChecklist = TerminationExitChecklistStatus.Unfinished;
            }

            if (entity.JobHandover != null)
            {
                foreach (var item in entity.JobHandover)
                {
                    if (context.ExitChecklists.Where(x => x.TerminationId == entity.Id && !(x.IsDeleted ?? false) && x.Id == item.Id).Count() > 0)
                    {
                        context.PhoenixEditRange(list.Where(x => x.Id == item.Id).ToArray());
                    }
                    else
                    {
                        context.PhoenixAddRange(list.Where(x => x.Id == item.Id).ToArray());
                    }
                }
            }


            if (entity.OfficeManagement != null)
            {
                foreach (var item in entity.OfficeManagement)
                {
                    if (context.ExitChecklists.Where(x => x.TerminationId == entity.Id && !(x.IsDeleted ?? false) && x.Id == item.Id).Count() > 0)
                    {
                        context.PhoenixEditRange(list.Where(x => x.Id == item.Id).ToArray());
                    }
                    else
                    {
                        context.PhoenixAddRange(list.Where(x => x.Id == item.Id).ToArray());
                    }
                }
            }

            if (entity.InformationTechnology != null)
            {
                foreach (var item in entity.InformationTechnology)
                {
                    if (context.ExitChecklists.Where(x => x.TerminationId == entity.Id && !(x.IsDeleted ?? false) && x.Id == item.Id).Count() > 0)
                    {
                        context.PhoenixEditRange(list.Where(x => x.Id == item.Id).ToArray());
                    }
                    else
                    {
                        context.PhoenixAddRange(list.Where(x => x.Id == item.Id).ToArray());
                    }
                }
            }

            if (entity.Finance != null)
            {
                foreach (var item in entity.Finance)
                {
                    if (context.ExitChecklists.Where(x => x.TerminationId == entity.Id && !(x.IsDeleted ?? false) && x.Id == item.Id).Count() > 0)
                    {
                        context.PhoenixEditRange(list.Where(x => x.Id == item.Id).ToArray());
                    }
                    else
                    {
                        context.PhoenixAddRange(list.Where(x => x.Id == item.Id).ToArray());
                    }
                }
            }

            if (entity.HumanResource != null)
            {
                foreach (var item in entity.HumanResource)
                {
                    if (context.ExitChecklists.Where(x => x.TerminationId == entity.Id && !(x.IsDeleted ?? false) && x.Id == item.Id).Count() > 0)
                    {
                        context.PhoenixEditRange(list.Where(x => x.Id == item.Id).ToArray());
                    }
                    else
                    {
                        context.PhoenixAddRange(list.Where(x => x.Id == item.Id).ToArray());
                    }
                }
            }


            if (entity.Other != null)
            {
                foreach (var item in entity.Other)
                {
                    if (context.ExitChecklists.Where(x => x.TerminationId == entity.Id && !(x.IsDeleted ?? false) && x.Id == item.Id).Count() > 0)
                    {
                        context.PhoenixEditRange(list.Where(x => x.Id == item.Id).ToArray());
                    }
                    else
                    {
                        context.PhoenixAddRange(list.Where(x => x.Id == item.Id).ToArray());
                    }
                }
            }

            if (entity.FinalHandover != null)
            {
                foreach (var item in entity.FinalHandover)
                {
                    if (context.ExitChecklists.Where(x => x.TerminationId == entity.Id && !(x.IsDeleted ?? false) && x.Id == item.Id).Count() > 0)
                    {
                        context.PhoenixEditRange(list.Where(x => x.Id == item.Id).ToArray());
                    }
                    else
                    {
                        context.PhoenixAddRange(list.Where(x => x.Id == item.Id).ToArray());
                    }
                }
            }
            //if (context.ExitChecklists.Where(x => x.TerminationId == entity.Id && !(x.IsDeleted ?? false)).Count() > 0)
            //{
            //    context.PhoenixEditRange(list.ToArray());
            //}
            //else
            //{
            //    context.PhoenixAddRange(list.ToArray());
            //}

            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<List<Termination>> Get()
        {
            TerminationService termination = new TerminationService(context);
            var result = await termination.GetTerminationHC(terminationType, false);
            return result;
        }

        public async Task<Termination> Get(string terminationId)
        {
            TerminationService termination = new TerminationService(context);
            var data = await termination.GetTerminationById(null, terminationType, terminationId);
            var file = await context.Filemasters.Where(x => x.Id == data.ExitInterviewFiles).FirstOrDefaultAsync();
            if (file != null) data.ExitInterviewFilesName = file.Name;
            return data;
        }

        public async Task<List<ExitChecklist>> GetJobHandoverList(string terminationId, string employeeBasicInfoId)
        {
            return await GetExitChecklist(terminationId, employeeBasicInfoId, ChecklistCategory.JobHandover);
        }

        public async Task<List<ExitChecklist>> GetOfficeManagementList(string terminationId, string employeeBasicInfoId)
        {
            return await GetExitChecklist(terminationId, employeeBasicInfoId, ChecklistCategory.OfficeManagement);
        }

        public async Task<List<ExitChecklist>> GetInformationTechnologyList(string terminationId, string employeeBasicInfoId)
        {
            return await GetExitChecklist(terminationId, employeeBasicInfoId, ChecklistCategory.InformationTechnology);
        }

        public async Task<List<ExitChecklist>> GetFinanceList(string terminationId, string employeeBasicInfoId)
        {
            return await GetExitChecklist(terminationId, employeeBasicInfoId, ChecklistCategory.Finance);
        }

        public async Task<List<ExitChecklist>> GetHumanResourceList(string terminationId, string employeeBasicInfoId)
        {
            return await GetExitChecklist(terminationId, employeeBasicInfoId, ChecklistCategory.HumanResouce);
        }

        public async Task<List<ExitChecklist>> GetOtherList(string terminationId, string employeeBasicInfoId)
        {
            return await GetExitChecklist(terminationId, employeeBasicInfoId, ChecklistCategory.Others);
        }

        public async Task<List<ExitChecklist>> GetFinalHandoverList(string terminationId, string employeeBasicInfoId)
        {
            return await GetExitChecklist(terminationId, employeeBasicInfoId, ChecklistCategory.FinalHandover);
        }

        private async Task<List<ExitChecklist>> GetExitChecklist(string terminationId, string employeeId, string employeeChecklistCategoryId)
        {
            var result = await (from b in context.EmployeeChecklists
                                join c in context.BusinessUnits on b.BusinessUnitPicId equals c.Id into bc
                                from tb1 in bc.DefaultIfEmpty()
                                join d in context.ExitChecklists on b.Id equals d.EmployeeChecklistId into bd
                                from tb2 in bd.DefaultIfEmpty()
                                where tb2.TerminationId == terminationId && b.EmployeeBasicInfoId == employeeId &&
                                b.EmployeeChecklistCategoryId == employeeChecklistCategoryId
                                select new ExitChecklist
                                {
                                    Id = tb2.Id,
                                    TerminationId = tb2.TerminationId,
                                    EmployeeChecklistId = tb2.EmployeeChecklistId,
                                    Notes = tb2.Notes,
                                    IsDone = tb2.IsDone,
                                    EndDate = tb2.EndDate,
                                    CreatedBy = tb2.CreatedBy,
                                    CreatedOn = tb2.CreatedOn,
                                    ModifiedBy = tb2.ModifiedBy,
                                    ModifiedOn = tb2.ModifiedOn,
                                    ApprovedBy = tb2.ApprovedBy,
                                    ApprovedOn = tb2.ApprovedOn,
                                    IsActive = tb2.IsActive,
                                    IsLocked = tb2.IsLocked,
                                    IsDefault = tb2.IsDefault,
                                    IsDeleted = tb2.IsDeleted,
                                    OwnerId = tb2.OwnerId,
                                    DeletedBy = tb2.DeletedBy,
                                    DeletedOn = tb2.DeletedOn,
                                    Checklistitem = b.Checklistitem,
                                    InCharge = tb1.UnitName
                                }).ToListAsync();

            if (result.Count() == 0)
            {
                result = await (from b in context.EmployeeChecklists
                                join c in context.BusinessUnits on b.BusinessUnitPicId equals c.Id into bc
                                from tb1 in bc.DefaultIfEmpty()
                                where b.EmployeeBasicInfoId == employeeId &&
                                b.EmployeeChecklistCategoryId == employeeChecklistCategoryId
                                select new ExitChecklist
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    TerminationId = terminationId,
                                    EmployeeChecklistId = b.Id,
                                    Checklistitem = b.Checklistitem,
                                    InCharge = tb1.UnitName
                                }).ToListAsync();
            }
            return result;
        }
    }
}