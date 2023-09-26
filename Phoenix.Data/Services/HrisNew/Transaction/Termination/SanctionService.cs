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
using Phoenix.Data.Models.ViewModel;
using Phoenix.Data.RestApi;
using Phoenix.Shared.Core.Contexts;

namespace Phoenix.Data
{
    public interface ISanctionService : IDataServiceHris<Termination>
    {
        Task<List<Termination>> GetApprovalList();
        Task<Termination> GetById(string id);
        Task<Termination> GetLastSanctionById(string employeeId);
    }

    public class SanctionService : ISanctionService
    {
        readonly DataContext context;
        readonly GlobalFunctionApproval globalFcApproval;
        readonly FileService uploadFile;
        readonly Services.Um.IManageMenuService menuService;
        string[] terminationType = new string[] { TerminationType.Sanction };
        string idTemplate = "";

        /// <summary>
        /// And endpoint to manage Sanction
        /// </summary>
        /// <param name="context">Database context</param>
        public SanctionService(DataContext context, GlobalFunctionApproval globalFcApproval, FileService uploadFile, Services.Um.IManageMenuService menuService)
        {
            this.context = context;
            this.globalFcApproval = globalFcApproval;
            this.uploadFile = uploadFile;
            this.menuService = menuService;
        }

        public async Task<int> AddAsync(Termination entity)
        {
            var isApprove = true;
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                var model = await ProcessUpload(entity);

                entity.Id = await TransID.GetTransId(context, Code.Sanction, DateTime.Today);
                var dataMenu = await menuService.GetByUniqueName(MenuUnique.Sanction).ConfigureAwait(false);
                if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
                var dataTempate = context.TmTemplateApprovals.Where(x => x.MenuId == dataMenu.Id && x.IsDeleted == false).FirstOrDefault();
                idTemplate = dataTempate.Id;
                await context.PhoenixAddAsync(model);
               
                //return await globalFcApproval.SubmitApproval<Termination>(false, entity.Id, entity.Status, $"{ApprovalLink.Sanction}?Id={entity.Id}&isApprove=true", idTemplate, MenuUnique.Sanction, entity);
                var vm = new Models.ViewModel.TransApprovalHrisVm()
                {
                    MenuId = dataMenu.Id,
                    RefId = entity.Id,
                    DetailLink = $"{ApprovalLink.Sanction}?Id={entity.Id}&isApprove=true",
                    Module = NotificationModuleType.ModuleHRIS,
                    NotificationType = NotificationTypeCode.SanctionNotification
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
               
        }

        public async Task<int> EditAsync(Termination entity)
        {
            var isApprove = true;
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                var model = await ProcessUpload(entity);
                var dataMenu = await menuService.GetByUniqueName(MenuUnique.Sanction).ConfigureAwait(false);
                if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
                var dataTempate = context.TmTemplateApprovals.Where(x => x.MenuId == dataMenu.Id && x.IsDeleted == false).FirstOrDefault();
                idTemplate = dataTempate.Id;
                context.PhoenixEdit(model);
                return await globalFcApproval.SubmitApproval<Termination>(false, entity.Id, entity.Status, $"{ApprovalLink.Sanction}?Id={entity.Id}&isApprove=true", idTemplate, MenuUnique.Sanction, entity);
                var vm = new Models.ViewModel.TransApprovalHrisVm()
                {
                    MenuId = dataMenu.Id,
                    RefId = entity.Id,
                    DetailLink = $"{ApprovalLink.Sanction}?Id={entity.Id}&isApprove=true",
                    Module = NotificationModuleType.ModuleHRIS,
                    NotificationType = NotificationTypeCode.SanctionNotification
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
        }

        public async Task<int> ApproveAsync(Termination entity)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    context.PhoenixApprove(entity);

                    var vm = new TrUpdateStatusApprovalViewModel()
                    {
                        RefId = entity.Id,
                        StatusApproved = entity.Status ?? 0,
                        Remarks = entity.RemarkRejected
                    };

                    var employeeId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(context.HttpContext);
                    var isApprove = await globalFcApproval.UpdateStatusApproval(vm, employeeId);

                    if (isApprove)
                    {
                        if (context.TrTemplateApprovals.Where(x => x.StatusApproved == Models.Um.StatusApproved.Approved && x.RefId == entity.Id).Count() > 0)
                        {
                            entity.IsHcProcess = true;
                            context.PhoenixEdit(entity);
                        }
                        var save = await context.SaveChangesAsync();

                        transaction.Commit();
                        return save;
                    }
                    else
                    {
                        transaction.Rollback();
                        throw new Exception();
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        private async Task<Termination> ProcessUpload(Termination entity)
        {
            var file1 = await uploadFile.Upload(entity.fileInterviewWarning, null);
            if (!string.IsNullOrEmpty(file1))
            {
                entity.InterviewWarningFile = file1;
            }

            var file2 = await uploadFile.Upload(entity.fileStatementOfPolice, null);
            if (!string.IsNullOrEmpty(file2))
            {
                entity.StatementOfPoliceReport = file2;
            }

            return entity;
        }

        public async Task<List<Termination>> Get()
        {
            TerminationService termination = new TerminationService(context);
            var result = await termination.GetTermination(context.GetEmployeeId(), terminationType);
            return result;
        }


        public async Task<List<Termination>> GetApprovalList()
        {
            var status = new int[] { Models.Um.StatusApproved.CurrentApproval.GetHashCode() };
            TerminationService termination = new TerminationService(context);
            var result = await termination.GetTerminationApproval(terminationType, status);
            return result;
        }

        public async Task<int> DeleteSoftAsync(Termination entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<Termination> Get(string Id) => await context.Terminations.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();

        public async Task<Termination> GetById(string id)
        {
            TerminationService termination = new TerminationService(context);
            var data = await termination.GetTerminationById(context.GetEmployeeId(), terminationType, id);
            if (data == null)
                data = new Termination();
            else
                await GetFile(data);
            return data;
        }

        public async Task<Termination> GetLastSanctionById(string employeeId)
        {
            var data = await context.Terminations.Where(x => x.EmployeeBasicInfoId == employeeId && x.TerminationType == TerminationType.Sanction && !(x.IsDeleted ?? false)).OrderByDescending(x => x.RequestDate).FirstOrDefaultAsync();
            if (data == null)
                data = new Termination();
            else
                await GetFile(data);
            return data;
        }

        private async Task<Termination> GetFile(Termination data)
        {
            if (data.InterviewWarningFile != null)
            {
                var InterviewWarning = await context.Filemasters.Where(x => x.Id == data.InterviewWarningFile).FirstOrDefaultAsync();
                if (InterviewWarning != null) data.InterviewWarningFileName = InterviewWarning.Name;
            }

            if (data.StatementOfPoliceReport != null)
            {
                var StatementOfPoliceReport = await context.Filemasters.Where(x => x.Id == data.StatementOfPoliceReport).FirstOrDefaultAsync();
                if (StatementOfPoliceReport != null) data.StatementOfPoliceReportName = StatementOfPoliceReport.Name;
            }
            return data;
        }
    }
}