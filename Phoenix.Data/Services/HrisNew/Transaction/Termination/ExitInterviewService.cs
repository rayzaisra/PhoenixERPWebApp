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
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.RestApi;
using Phoenix.Data.Services.Um;
using Phoenix.Shared.Core.Contexts;

namespace Phoenix.Data
{
    public interface IExitInterviewService : IDataServiceHris<Termination>
    {
    }

    public class ExitInterviewService : IExitInterviewService
    {
        readonly DataContext context;
        readonly IManageMenuService menuService;
        readonly GlobalFunctionApproval globalFcApproval;
        string[] terminationType = new string[] {TerminationType.Resignation,
            TerminationType.EndOfContract, TerminationType.EndOfProbation,
            //TerminationType.Death, TerminationType.ProlongedIllness,
            TerminationType.Sanction, TerminationType.Retired};
        string idTemplate = "";
        /// <summary>
        /// And endpoint to manage Termination
        /// </summary>
        /// <param name="context">Database context</param>
        public ExitInterviewService(DataContext context, GlobalFunctionApproval globalFcApproval, IManageMenuService menuService)
        {
            this.context = context;
            this.menuService = menuService; 
        }

        public Task<int> AddAsync(Termination entity)
        {
            throw new NotImplementedException();
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
            entity.ApprovedBy = string.Empty;//ditambahkan karena error ketika save minta diisi
            context.PhoenixEdit(entity);
            var exit = context.ExitInterviews.Where(x => x.TerminationId == entity.Id).ToList();
            if (exit.Count > 0)
            {
                context.ExitInterviews.RemoveRange(exit);
                await context.SaveChangesAsync();
            }
            //foreach (var item in entity.ExitInterviews)
            //{
            //	item.ApprovedBy = string.Empty;
            //	item.ApprovedOn = DateTime.Now;
            //	item.BusinessUnitDivisionId = string.Empty;
            //	item.BusinessUnitDepartementId = string.Empty;
            //}
            context.PhoenixAddRange(entity.ExitInterviews.ToArray());
            var emailUser = await context.EmployeeBasicInfos.Where(x => x.Id == entity.EmployeeBasicInfoId).Select(x => x.Email).FirstOrDefaultAsync();
            await globalFcApproval.AddNotification(NotificationTypeCode.ExitInterviewUser, emailUser, NotificationModuleType.ModuleHRIS, entity.Id, true, true, context.GetEmployeeId());
            return await context.SaveChangesAsync();


            //var isApprove = true;
            //using (var transaction = await context.Database.BeginTransactionAsync())
            //{
            //    entity.ApprovedBy = string.Empty;//ditambahkan karena error ketika save minta diisi
            //    context.PhoenixEdit(entity);
            //    var exit = context.ExitInterviews.Where(x => x.TerminationId == entity.Id).ToList();
            //    if (exit.Count > 0)
            //    {
            //        context.ExitInterviews.RemoveRange(exit);
            //        await context.SaveChangesAsync();
            //    }
            //    //foreach (var item in entity.ExitInterviews)
            //    //{
            //    //	item.ApprovedBy = string.Empty;
            //    //	item.ApprovedOn = DateTime.Now;
            //    //	item.BusinessUnitDivisionId = string.Empty;
            //    //	item.BusinessUnitDepartementId = string.Empty;
            //    //}
            //    context.PhoenixAddRange(entity.ExitInterviews.ToArray());

             //var dataMenu = await menuService.GetByUniqueName(MenuUnique.ExitInterView).ConfigureAwait(false);
            //    if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
            //    var dataTempate = context.TmTemplateApprovals.Where(x => x.MenuId == dataMenu.Id && x.IsDeleted == false).FirstOrDefault();
            //    idTemplate = dataTempate.Id;

            //    var vm = new Models.ViewModel.TransApprovalHrisVm()
            //    {
            //        MenuId = dataMenu.Id,
            //        RefId = entity.Id,
            //        DetailLink = $"{ApprovalLink.ExitInterview}?Id={entity.Id}",
            //        Module = NotificationModuleType.ModuleHRIS,
             //     NotificationType = NotificationTypeCode.ExitInterviewNotification
            //    };
            //    var subGroupId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessSubGroup(context.HttpContext);
            //    var divisionId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessUnitDivisi(context.HttpContext);
            //    var employeeId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(context.HttpContext);
            //    isApprove = await globalFcApproval.Save(vm, subGroupId, divisionId, employeeId);
            //    if (isApprove)
            //    {
            //        var save = await context.SaveChangesAsync();
            //        transaction.Commit();
            //        return save;
            //    }
            //    else
            //    {
            //        transaction.Rollback();
            //        throw new Exception("please check the approval template that will be processed");
            //    }

            //}
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
            data.ExitInterviews = await GetExitInterviewItemsList(terminationId);
            data.CategoryExitInterviews = await context.CategoryExitInterviews.Where(x => !(x.IsDeleted ?? false)).ToListAsync();
            return data;
        }

        private async Task<List<ExitInterview>> GetExitInterviewItemsList(string terminationId)
        {
            var result = await (from a in context.ExitInterviewItems
                                join b in context.ExitInterviews on a.Id equals b.ExitInterviewItemId
                                where b.TerminationId == terminationId
                                select new ExitInterview
                                {
                                    Id = b.Id,
                                    TerminationId = b.TerminationId,
                                    ExitInterviewItemId = b.ExitInterviewItemId,
                                    CategoryExitInterviewId = b.CategoryExitInterviewId,
                                    Result = b.Result,
                                    TextItem = a.TextItem,
                                    Type = a.Type,
                                    No = a.No,
                                    CreatedBy = b.CreatedBy,
                                    CreatedOn = b.CreatedOn,
                                    ModifiedBy = b.ModifiedBy,
                                    ModifiedOn = b.ModifiedOn,
                                    ApprovedBy = b.ApprovedBy,
                                    ApprovedOn = b.ApprovedOn,
                                    IsActive = b.IsActive,
                                    IsLocked = b.IsLocked,
                                    IsDefault = b.IsDefault,
                                    IsDeleted = b.IsDeleted,
                                    OwnerId = b.OwnerId,
                                    DeletedBy = b.DeletedBy,
                                    DeletedOn = b.DeletedOn
                                }).ToListAsync();

            if (result.Count() == 0)
            {
                result = await context.ExitInterviewItems.Where(x => !(x.IsDeleted ?? false)).Select(x => new ExitInterview
                {
                    TerminationId = terminationId,
                    ExitInterviewItemId = x.Id,
                    CategoryExitInterviewId = x.CategoryExitInterviewId,
                    TextItem = x.TextItem,
                    Type = x.Type,
                    No = x.No
                }).ToListAsync();
            }
            return result;
        }
    }
}