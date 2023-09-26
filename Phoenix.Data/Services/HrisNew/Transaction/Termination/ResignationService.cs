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
using Phoenix.Data.Models.Um;
using Phoenix.Data.Models.ViewModel;
using Phoenix.Data.RestApi;
using Phoenix.Data.Services.Um;
using Phoenix.Shared.Core.Contexts;

namespace Phoenix.Data
{
    public interface IResignationService : IDataServiceHris<Termination>
    {
        Task<List<Termination>> GetApprovalList();
        Task<Termination> GetById(string id);
        bool IsRequest();
        Task<List<Termination>> GetDataResignationByEmployeeId(string id);
    }

    public class ResignationService : IResignationService
    {
        readonly DataContext context;
        readonly GlobalFunctionApproval globalFcApproval;
        readonly FileService uploadFile;
        readonly IManageMenuService menuService;

        string[] terminationType = new string[] { TerminationType.Resignation };
        string idTemplate = "";
        /// <summary>
        /// And endpoint to manage Termination
        /// </summary>
        /// <param name="context">Database context</param>
        public ResignationService(DataContext context, GlobalFunctionApproval globalFcApproval, FileService uploadFile, IManageMenuService menuService)
        {
            this.context = context;
            this.globalFcApproval = globalFcApproval;
            this.uploadFile = uploadFile;
            this.menuService = menuService;
        }

        public async Task<int> AddAsync(Termination entity)
        {
            //entity.Id = await TransID.GetTransId(context, Code.Resignation, DateTime.Today); 
            //var file = await uploadFile.Upload(entity.file, null);
            //if (!string.IsNullOrEmpty(file))
            //{
            //    entity.ResignLetter = file;
            //}
            //var dataMenu = await menuService.GetByUniqueName(MenuUnique.Resignation).ConfigureAwait(false);
            //if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
            //var dataTempate = context.TmTemplateApprovals.Where(x => x.MenuId == dataMenu.Id && x.IsDeleted == false).FirstOrDefault();
            //idTemplate = dataTempate.Id;
            //return await globalFcApproval.SubmitApproval<Termination>(false, entity.Id, entity.Status, $"{ApprovalLink.Resignation}?Id={entity.Id}&isApprove=true", idTemplate, MenuUnique.Resignation, entity);

            var isApprove = true;
            var check = await (from q in context.Terminations
                         join r in context.TrTemplateApprovals on q.Id equals r.RefId
                         where q.EmployeeBasicInfoId == entity.EmployeeBasicInfoId && q.TerminationType == entity.TerminationType && q.IsDeleted != true && r.IsDeleted != true && r.StatusApproved == StatusApproved.WaitingApproval
                         select q.Id).ToListAsync();
            
            if (check != null)
            {
                throw new Exception("The application has been submitted before and is still waiting for approval");
            }

            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                entity.Id = await TransID.GetTransId(context, Code.Resignation, DateTime.Today);
                var file = await uploadFile.Upload(entity.file, null);
                if (!string.IsNullOrEmpty(file))
                {
                    entity.ResignLetter = file;
                }
                await context.PhoenixAddAsync(entity);
                var dataMenu = await menuService.GetByUniqueName(MenuUnique.Resignation).ConfigureAwait(false);
                if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
                var dataTempate = context.TmTemplateApprovals.Where(x => x.MenuId == dataMenu.Id && x.IsDeleted == false).FirstOrDefault();
                idTemplate = dataTempate.Id;

                var vm = new Models.ViewModel.TransApprovalHrisVm()
                {
                    MenuId = dataMenu.Id,
                    RefId = entity.Id,
                    DetailLink = $"{ApprovalLink.Resignation}?Id={entity.Id}&isApprove=true",
                    Module = NotificationModuleType.ModuleHRIS,
                    NotificationType = NotificationTypeCode.ResignationNotification
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
            //var file = await uploadFile.Upload(entity.file, entity.ResignLetter);
            //if (!string.IsNullOrEmpty(file))
            //{
            //    entity.ResignLetter = file;
            //}
            //var dataMenu = await menuService.GetByUniqueName(MenuUnique.Resignation).ConfigureAwait(false);
            //if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
            //var dataTempate = context.TmTemplateApprovals.Where(x => x.MenuId == dataMenu.Id && x.IsDeleted == false).FirstOrDefault();
            //idTemplate = dataTempate.Id;
            //return await globalFcApproval.SubmitApproval<Termination>(true, entity.Id, entity.Status, $"{ApprovalLink.Resignation}?Id={entity.Id}&isApprove=true", idTemplate, MenuUnique.Resignation, entity);

            var isApprove = true;
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                var file = await uploadFile.Upload(entity.file, entity.ResignLetter);
                if (!string.IsNullOrEmpty(file))
                {
                    entity.ResignLetter = file;
                }
                context.PhoenixEdit(entity);
                var dataMenu = await menuService.GetByUniqueName(MenuUnique.Resignation).ConfigureAwait(false);
                if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
                var dataTempate = context.TmTemplateApprovals.Where(x => x.MenuId == dataMenu.Id && x.IsDeleted == false).FirstOrDefault();
                idTemplate = dataTempate.Id;

                var vm = new Models.ViewModel.TransApprovalHrisVm()
                {
                    MenuId = dataMenu.Id,
                    RefId = entity.Id,
                    DetailLink = $"{ApprovalLink.Resignation}?Id={entity.Id}&isApprove=true",
                    Module = NotificationModuleType.ModuleHRIS,
                    NotificationType = NotificationTypeCode.ResignationNotification
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

        public async Task<List<Termination>> Get()
        {
            TerminationService termination = new TerminationService(context);
            var result = await termination.GetTermination(context.GetEmployeeId(), terminationType);
            return result;
        }

        public async Task<List<Termination>> GetDataResignationByEmployeeId(string id)
        {
            TerminationService termination = new TerminationService(context);
            var result = await termination.GetTermination(id, terminationType);
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

        public bool IsRequest()
        {
            var terminationStatus = new int[] { Models.Um.StatusApproved.Rejected.GetHashCode() };
            //var result = (context.Terminations.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Requestor == context.GetEmployeeId() &&
            //            terminationType.Any(y => y == x.TerminationType) && !terminationStatus.Any(y => y == x.Status)).AsNoTracking().Count() > 0);
            var que = (from tb1 in context.Terminations
                       join tb2 in context.TrTemplateApprovals on tb1.Id equals tb2.RefId
                       where !(tb1.IsDeleted ?? false)
                       && ( tb1.Requestor == context.GetEmployeeId() || tb1.EmployeeBasicInfoId == context.GetEmployeeId())
                       select new Termination()
                       {
                           Id = tb1.Id,
                           Status = Convert.ToInt32(tb2.StatusApproved),
                           IsDeleted = tb1.IsDeleted,
                           TerminationType = tb1.TerminationType,
                           Requestor = tb1.Requestor
                       }).ToList();
            var result = (que.Where(x => terminationType.Any(y => y == x.TerminationType) && !terminationStatus.Any(y => y == x.Status)).Count() > 0);
            return result;
        }

        public async Task<Termination> GetById(string id)
        {
            TerminationService termination = new TerminationService(context);
            var data = await termination.GetTerminationById(context.GetEmployeeId(), terminationType, id);
            if (data != null)
            {
                var file = await context.Filemasters.Where(x => x.Id == data.ResignLetter).FirstOrDefaultAsync();
                if (file != null) data.ResignLetterName = file.Name;
            }
           
            return data;
        }
    }
}