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
    public interface IRetiredService : IDataServiceHris<Termination>
    {
        Task<List<Termination>> GetApprovalList();
        Task<Termination> GetById(string id);
    }

    public class RetiredService : IRetiredService
    {
        readonly DataContext context;
        readonly GlobalFunctionApproval globalFcApproval;
        readonly Services.Um.IManageMenuService menuService;
        string[] terminationType = new string[] { TerminationType.Retired };
        string idTemplate = "";

        /// <summary>
        /// And endpoint to manage Termination
        /// </summary>
        /// <param name="context">Database context</param>
        public RetiredService(DataContext context, GlobalFunctionApproval globalFcApproval, Services.Um.IManageMenuService menuService)
        {
            this.context = context;
            this.globalFcApproval = globalFcApproval;
            this.menuService = menuService;
        }

        public async Task<int> AddAsync(Termination entity)
        {
            entity.Id = await TransID.GetTransId(context, Code.Retired, DateTime.Today);
            var dataMenu = await menuService.GetByUniqueName(MenuUnique.Retired).ConfigureAwait(false);
            if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
            var dataTempate = context.TmTemplateApprovals.Where(x => x.MenuId == dataMenu.Id && x.IsDeleted == false).FirstOrDefault();
            idTemplate = dataTempate.Id;
            return await globalFcApproval.SubmitApproval<Termination>(false, entity.Id, entity.Status, $"{ApprovalLink.Retired}?Id={entity.Id}&isApprove=true", idTemplate, MenuUnique.Retired, entity);
        }

        public async Task<int> EditAsync(Termination entity)
        {
            var dataMenu = await menuService.GetByUniqueName(MenuUnique.Retired).ConfigureAwait(false);
            if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
            var dataTempate = context.TmTemplateApprovals.Where(x => x.MenuId == dataMenu.Id && x.IsDeleted == false).FirstOrDefault();
            idTemplate = dataTempate.Id;
            return await globalFcApproval.SubmitApproval<Termination>(true, entity.Id, entity.Status, $"{ApprovalLink.Retired}?Id={entity.Id}&isApprove=true", idTemplate, MenuUnique.Retired, entity);
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
            return data;
        }
    }
}