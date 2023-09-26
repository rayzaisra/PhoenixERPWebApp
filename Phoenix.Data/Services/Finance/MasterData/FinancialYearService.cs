using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    public interface IFinancialYearService : IDataService<FinancialYear>
    {
        Task<int> ApproveAsync(FinancialYear entity);
        DateTime? MaxDate(string legalEntityId);
        int? GetDefaultYearOpeningBalance(string legalEntityId);
    }

    public class FinancialYearService : IFinancialYearService
    {
        readonly DataContext context;
        readonly GlobalFunctionApproval globalFcApproval;
        readonly Services.Um.IManageMenuService menuService;
        readonly StatusLogService log;

        /// <summary>
        /// And endpoint to manage FinancialYear
        /// </summary>
        /// <param name="context">Database context</param>
        public FinancialYearService(DataContext context, GlobalFunctionApproval globalFcApproval, Services.Um.IManageMenuService menuService, StatusLogService log)
        {
            this.context = context;
            this.globalFcApproval = globalFcApproval;
            this.menuService = menuService;
            this.log = log;
        }

        public async Task<int> AddAsync(FinancialYear entity)
        {
            return await AddEdit(entity, false);
        }

        public Task<int> AddRangeAsync(params FinancialYear[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(FinancialYear entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params FinancialYear[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(FinancialYear entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params FinancialYear[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(FinancialYear entity)
        {
            return await AddEdit(entity, true);
        }

        public Task<int> EditRangeAsync(FinancialYear entity)
        {
            throw new NotImplementedException();
        }

        private async Task<int> AddEdit(FinancialYear entity, bool isEdit)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var isAlready = false;
                    entity.Status = StatusTransaction.StatusName(entity.StatusApproval);

                    if (entity.StatusApproval != 0)
                    {
                        if (context.FinancePeriods.Where(x => !(x.IsDeleted ?? false) && x.StartDate.Year == entity.StartDate.Year && x.LegalEntityId == entity.LegalEntityId && !(x.Status == StatusTransactionName.Close)).Count() > 0)
                        {
                            throw new Exception("there is still an open period");
                        }
                    }

                    if (isEdit) isAlready = context.FinancialYears.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.LegalEntityId == entity.LegalEntityId && x.StartDate.Date == entity.StartDate.Date && x.Id != entity.Id).Count() > 0;
                    else isAlready = context.FinancialYears.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.LegalEntityId == entity.LegalEntityId && x.StartDate.Date == entity.StartDate.Date).Count() > 0;

                    if (isAlready)
                    {
                        var legal = await context.LegalEntity.Where(x => !(x.IsDeleted ?? false) && x.Id == entity.LegalEntityId).FirstOrDefaultAsync();
                        throw new Exception($"legal entity {legal.LegalEntityName} and year {entity.StartDate.Year.ToString()} already exists, please choose a legal entity in a different year!");
                    }

                    if (isEdit)
                    {
                        context.PhoenixEdit(entity);
                    }
                    else
                    {
                        entity.Id = await TransID.GetTransId(context, Code.FinancialYear, DateTime.Today);
                        entity.FinanceYearNumber = entity.Id;
                        await context.PhoenixAddAsync(entity);
                        await context.SaveChangesAsync();
                    }

                    //var param = new SqlParameter[]{
                    //                        new SqlParameter("@p_legal_entity", entity.LegalEntityId),
                    //                        new SqlParameter("@p_from_date", entity.StartDate),
                    //                        new SqlParameter("@p_to_date", entity.EndDate)
                    //                    };

                    //await context.Database.ExecuteSqlCommandAsync("EXECUTE fn.fin_yearly_open @p_legal_entity, @p_from_date, @p_to_date", param);

                    var isApprove = true;
                    if (entity.Status != StatusTransactionName.Draft)
                    {
                        await globalFcApproval.clearApproval(entity.Id);

                        var dataMenu = await menuService.GetByUniqueName(MenuUnique.FinancialYear).ConfigureAwait(false);
                        if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
                        var vm = new Models.ViewModel.TransApprovalHrisVm()
                        {
                            MenuId = dataMenu.Id,
                            RefId = entity.Id,
                            DetailLink = $"{ApprovalLink.FinancialYear}?Id={entity.Id}"
                        };
                        var subGroupId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessSubGroup(context.HttpContext);
                        var divisionId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessUnitDivisi(context.HttpContext);
                        var employeeId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(context.HttpContext);
                        isApprove = await globalFcApproval.Save(vm, subGroupId, divisionId, employeeId);
                        if (isApprove)
                        {
                            await log.AddAsync(new StatusLog() { TransactionId = entity.Id, Status = entity.Status, Description = entity.RemarksProcess });
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
                        entity.Status = StatusTransactionName.Draft;
                        context.FinancialYears.Update(entity);
                        await log.AddAsync(new StatusLog() { TransactionId = entity.Id, Status = entity.Status, Description = entity.RemarksProcess });
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

        public async Task<int> ApproveAsync(FinancialYear entity)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var save = await globalFcApproval.UnsafeProcessApproval(entity.Id, entity.StatusApproval, entity.RemarksProcess, entity);
                    var approval = context.TrTemplateApprovals.Where(x => x.RefId == entity.Id && !(x.IsDeleted ?? false)).FirstOrDefault();
                    if (approval != null)
                    {
                        entity.Status = StatusTransaction.StatusName(approval.StatusApproved.GetHashCode());
                        if (approval.StatusApproved == Models.Um.StatusApproved.Approved)
                        {
                            var param = new SqlParameter[]{
                                            new SqlParameter("@p_legal_entity", entity.LegalEntityId),
                                            new SqlParameter("@p_from_date", entity.StartDate.Date),
                                            new SqlParameter("@p_to_date", entity.EndDate.Date),
                                            new SqlParameter("@p_user_Login", context.GetEmployeeId()),
                                        };

                            if ((entity.IsClose ?? false))
                            {
                                entity.Status = StatusTransactionName.Close;
                                await context.Database.ExecuteSqlCommandAsync("EXECUTE fn.fin_yearly_closing @p_legal_entity, @p_from_date, @p_to_date, @p_user_Login", param);
                            }
                            else
                            {
                                entity.Status = StatusTransactionName.Draft;
                                await context.Database.ExecuteSqlCommandAsync("EXECUTE fn.fin_yearly_reopen @p_legal_entity, @p_from_date, @p_to_date, @p_user_Login", param);
                            }

                        }
                    }
                    context.FinancialYears.Update(entity);

                    await log.AddAsync(new StatusLog() { TransactionId = entity.Id, Status = entity.Status, Description = entity.RemarksProcess });
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

        public async Task<List<FinancialYear>> Get()
        {
            var result = await (from a in context.FinancialYears
                                join b in context.LegalEntity on a.LegalEntityId equals b.Id
                                where !(a.IsDeleted ?? false)
                                select new FinancialYear
                                {
                                    Id = a.Id,
                                    FinancialYearName = a.FinancialYearName,
                                    FinanceYearNumber = a.FinanceYearNumber,
                                    StartDate = a.StartDate,
                                    EndDate = a.EndDate,
                                    LegalEntityName = b.LegalEntityName,
                                    Status = a.Status
                                }).ToListAsync();
            return result;
        }

        public Task<List<FinancialYear>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<FinancialYear> Get(string Id) => await context.FinancialYears.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();

        public DateTime? MaxDate(string legalEntityId)
        {
            if (context.FinancialYears.Where(x => x.LegalEntityId == legalEntityId).Count() > 0)
            {
                return context.FinancialYears.Where(x => x.LegalEntityId == legalEntityId).Max(x => x.EndDate);
            }
            else
            {
                return DateTime.Now.AddYears(-1);
            }
        }

        public int? GetDefaultYearOpeningBalance(string legalEntityId)
        {
            if (context.FinancialYears.Where(x => x.LegalEntityId == legalEntityId).Count() > 0)
            {
                return context.FinancialYears.Where(x => x.LegalEntityId == legalEntityId).Min(x => x.StartDate).Year;
            }
            else
            {
                return null;
            }
        }
    }
}