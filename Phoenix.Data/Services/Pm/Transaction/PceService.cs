using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Finance.Master;
using Phoenix.Data.Models.Um;
using Phoenix.Data.RestApi;
using Phoenix.Shared.Core.Contexts;

namespace Phoenix.Data
{
    public interface IPceService : IDataService<Pce>
    {
        Task<List<PceTask>> GetTaskList(string PceId);
        Task<int> ApproveAsync(Pce entity);
        Task<int> SubmitToClientAsync(Pce entity);
        Task<dynamic> GetInvoiceDetailList(string pceId);
        Task<List<Pce>> GetList(string status, bool isPM);
        //Task<Pce> GetByJobId(string jobId);
        Task<dynamic> Print(string pceId);
    }

    public class PceService : IPceService
    {
        readonly DataContext context;
        readonly GlobalFunctionApproval globalFcApproval;
        readonly Services.Um.IManageMenuService menuService;
        readonly StatusLogService log;

        /// <summary>
        /// And endpoint to manage Pce
        /// </summary>
        /// <param name="context">Database context</param>
        public PceService(DataContext context, GlobalFunctionApproval globalFcApproval, Services.Um.IManageMenuService menuService, StatusLogService log)
        {
            this.context = context;
            this.globalFcApproval = globalFcApproval;
            this.menuService = menuService;
            this.log = log;
        }

        public async Task<int> SubmitToClientAsync(Pce entity)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (entity.Status == StatusTransactionName.Draft || entity.Status == StatusTransactionName.Rejected)
                    {
                        entity.TotalUsedInvoice = 0;
                        entity.TotalBalanceInvoice = entity.Total;
                    }

                    if (entity.Status == PCEStatus.Revise)
                    {
                        var trApproval = context.TrTemplateApprovals.AsNoTracking().Where(x => x.RefId == entity.Id && !(x.IsDeleted ?? false));
                        if (trApproval != null)
                        {
                            foreach (var item in trApproval)
                            {
                                item.IsDeleted = true;
                                context.TrTemplateApprovals.Update(item);
                            }
                        }

                        context.PhoenixEdit(entity);

                        var dataMenu = await menuService.GetByUniqueName(entity.IsPM ? MenuUnique.PMPCE : MenuUnique.PCE).ConfigureAwait(false);
                        var dataTempate = context.TmTemplateApprovals.Where(x => x.MenuId == dataMenu.Id && x.IsDeleted == false).FirstOrDefault();
                        if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
                        var vm = new Models.ViewModel.TransApprovalHrisVm()
                        {
                            IdTemplate = dataTempate.Id,
                            MenuId = dataMenu.Id,
                            RefId = entity.Id,
                            DetailLink = $"{ApprovalLink.PCE}?Id={entity.Id}",
                            NotificationType = NotificationTypeCode.PCENotification,
                            Module = NotificationModuleType.ModulePM
                        };
                        var subGroupId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessSubGroup(context.HttpContext);
                        var divisionId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessUnitDivisi(context.HttpContext);
                        var employeeId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(context.HttpContext);
                        var isApprove = await globalFcApproval.Save(vm, subGroupId, divisionId, employeeId);
                        if (isApprove)
                        {
                            await log.AddAsync(new StatusLog() { TransactionId = entity.Id, Status = entity.Status, Description = entity.RemarkRejected });
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
                        context.PhoenixEdit(entity);
                        await log.AddAsync(new StatusLog() { TransactionId = entity.Id, Status = entity.Status, Description = entity.RemarkRejected });
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

        public async Task<int> AddAsync(Pce entity)
        {
            return await AddEdit(entity, false);
        }

        public async Task<int> EditAsync(Pce entity)
        {
            return await AddEdit(entity, true);
        }

        private async Task<int> AddEdit(Pce entity, bool isEdit)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    entity.Status = StatusTransaction.StatusName(entity.StatusApproval);

                    if (entity.Status == StatusTransactionName.Draft || entity.Status == StatusTransactionName.Rejected)
                    {
                        entity.TotalUsedInvoice = 0;
                        entity.TotalBalanceInvoice = entity.Total;
                    }

                    if (isEdit)
                    {
                        context.PhoenixEdit(entity);
                    }
                    else
                    {
                        entity.Id = await TransID.GetTransId(context, Code.PCE, DateTime.Today);
                        entity.Code = entity.Id;

                        await context.PhoenixAddAsync(entity);
                        await context.SaveChangesAsync();
                    }

                    if (entity.Status == StatusTransactionName.Revised)
                    {
                        await globalFcApproval.clearApproval(entity.PcaId); //Clear Approval PCA

                        var pca = await context.Pcas.AsNoTracking().Where(x => x.Id == entity.PcaId).FirstOrDefaultAsync();
                        if (pca != null) pca.Status = entity.Status;
                        context.PhoenixEdit(pca);

                        context.Pces.Update(entity);
                        await log.AddAsync(new StatusLog() { TransactionId = entity.Id, Status = entity.Status, Description = entity.RemarkRejected });
                        await log.AddAsync(new StatusLog() { TransactionId = entity.PcaId, Status = entity.Status, Description = entity.RemarkRejected });
                        var save = await context.SaveChangesAsync();
                        transaction.Commit();
                        return save;
                    }
                    else if (entity.Status != StatusTransactionName.Draft)
                    {
                        await globalFcApproval.clearApproval(entity.Id);

                        var isApprove = true;
                        var dataMenu = await menuService.GetByUniqueName(entity.IsPM ? MenuUnique.PMPCE : MenuUnique.PCE).ConfigureAwait(false);
                        if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
                        var vm = new Models.ViewModel.TransApprovalHrisVm()
                        {
                            MenuId = dataMenu.Id,
                            RefId = entity.Id,
                            DetailLink = $"{ApprovalLink.PCE}?Id={entity.Id}",
                            NotificationType = NotificationTypeCode.PCENotification,
                            Module = entity.IsPM ? NotificationModuleType.ModulePM : NotificationModuleType.ModuleFinance
                        };
                        var subGroupId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessSubGroup(context.HttpContext);
                        var divisionId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessUnitDivisi(context.HttpContext);
                        var employeeId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(context.HttpContext);
                        isApprove = await globalFcApproval.Save(vm, subGroupId, divisionId, employeeId);
                        if (isApprove)
                        {
                            await log.AddAsync(new StatusLog() { TransactionId = entity.Id, Status = entity.Status, Description = entity.RemarkRejected });
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
                        entity.Status = StatusTransaction.StatusName(StatusTransaction.Draft);
                        context.Pces.Update(entity);
                        await log.AddAsync(new StatusLog() { TransactionId = entity.Id, Status = entity.Status, Description = entity.RemarkRejected });
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

        public async Task<int> ApproveAsync(Pce entity)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var save = await globalFcApproval.UnsafeProcessApproval(entity.Id, entity.StatusApproval, entity.RemarkRejected, entity);
                    var approval = context.TrTemplateApprovals.AsNoTracking().Where(x => x.RefId == entity.Id && !(x.IsDeleted ?? false)).FirstOrDefault();
                    if (approval != null)
                    {
                        entity.Status = StatusTransaction.StatusName(approval.StatusApproved.GetHashCode());
                        if (approval.StatusApproved == Models.Um.StatusApproved.Approved)
                        {
                            //entity.Status = PCEStatus.Open;

                            var job = await context.JobDetail.AsNoTracking().Where(x => x.Id == entity.JobId).FirstOrDefaultAsync();
                            if (job != null)
                            {
                                job.JobStatusId = "open";
                                context.JobDetail.Update(job);
                            }

                            //add new notification request by usay juni 2020
                            var pceid = await context.Pces.AsNoTracking().Where(x => x.JobId == entity.JobId).Select(x => x.Id).FirstOrDefaultAsync();
                            var notificationType = await context.NotificationTypes.AsNoTracking().Where(x => x.Href == $"/pm/pce/addedit/{pceid}").FirstOrDefaultAsync();
                            var jobName = entity.JobName;
                            if (string.IsNullOrEmpty(jobName)) { jobName = await context.JobDetail.AsNoTracking().Where(x => x.Id == entity.JobId).Select(x => x.JobName).FirstOrDefaultAsync(); }
                            if (notificationType == null)
                            {
                                notificationType = new NotificationType()
                                {
                                    Id = Guid.NewGuid().ToString().ToUpper(),
                                    ShortText = $"({jobName}) is started",
                                    LongText = $"({jobName}) is started",
                                    Href = $"/pm/pce/addedit/{pceid}"
                                };
                                await context.PhoenixAddAsync(notificationType);
                            }

                            var team = await (from a in context.PmTeams
                                              join b in context.PmTeamMembers on a.Id equals b.TeamId
                                              where !(a.IsDeleted ?? false) && !(b.IsDeleted ?? false) && a.JobId == entity.JobId
                                              select new { b.ApplicationUserId }).AsNoTracking().ToListAsync();

                            foreach (var item in team)
                            {
                                var notification = new Notification()
                                {
                                    Id = Guid.NewGuid().ToString().ToUpper(),
                                    NotificationTypeId = notificationType.Id,
                                    UserId = item.ApplicationUserId,
                                    ItemId = entity.Id,
                                    IsSeen = false,
                                    Module = entity.IsPM ? NotificationModuleType.ModulePM : NotificationModuleType.ModuleFinance
                                };
                                await context.PhoenixAddAsync(notification);
                            }
                        }
                        else if (approval.StatusApproved == Models.Um.StatusApproved.Rejected)
                        {
                            var pca = await context.Pcas.Where(x => x.Id == entity.PcaId).FirstOrDefaultAsync();
                            pca.Status = StatusTransactionName.Rejected;
                            context.PhoenixEdit(pca);

                            var trApproval = await context.TrTemplateApprovals.Where(x => x.RefId == entity.PcaId).Where(x => !(x.IsDeleted ?? false)).FirstOrDefaultAsync();
                            trApproval.StatusApproved = StatusApproved.Rejected;
                            context.PhoenixEdit(trApproval);
                        }

                        context.Pces.Update(entity);
                        await log.AddAsync(new StatusLog() { TransactionId = entity.Id, Status = entity.Status, Description = entity.RemarkRejected });
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

        public Task<int> AddRangeAsync(params Pce[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(Pce entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params Pce[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(Pce entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params Pce[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> EditRangeAsync(Pce entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Pce>> GetList(string status, bool isPM)
        {
            //yang bisa akses ke pce adalah pm team, creator dan approval
            var userId = context.GetUserId();
            var employeeId = context.GetEmployeeId();
            var groupId = await context.TmUserApps.Where(x => x.EmployeeBasicInfoId == employeeId).Select(x => x.GroupId).FirstOrDefaultAsync();
            var isAdministrator = (groupId == "1908b01c-688a-4ed4-813e-5f622de0b794");
            if (!isPM) { isAdministrator = true; }

            var team = (from a in context.PmTeams
                        join b in context.PmTeamMembers on a.Id equals b.TeamId
                        where !(a.IsDeleted ?? false) && !(b.IsDeleted ?? false) && b.ApplicationUserId == userId
                        select new { a.JobId });

            var approval = (from a in context.TrTemplateApprovals
                            join b in context.TrUserApprovals on a.Id equals b.TrTempApprovalId
                            where b.EmployeeBasicInfoId == employeeId
                            select new { a.RefId });

            var result = await (from a in context.Pces
                                join b in context.JobDetail on a.JobId equals b.Id
                                join d in context.CompanyBrands on b.BrandId equals d.Id
                                join e in context.Companys on b.CompanyId equals e.Id
                                join f in context.Pcas on a.PcaId equals f.Id
                                join g in context.Currencies on a.CurrencyId equals g.Id
                                where !(a.IsDeleted ?? false) && //e.Is_client == true &&
                                (string.IsNullOrEmpty(status) ? true : (a.Status ?? StatusTransactionName.Draft) == status)
                                select new Pce
                                {
                                    Id = a.Id,
                                    Code = a.Code,
                                    JobId = a.JobId,
                                    Status = a.Status,
                                    Revision = a.Revision,
                                    JobName = $"{a.JobId} - {b.JobName}",
                                    CompanyName = e.CompanyName,
                                    BrandName = d.BrandName,
                                    CreatedOn = a.CreatedOn,
                                    CreatedBy = a.CreatedBy,
                                    ModifiedOn = a.ModifiedOn,
                                    PcaId = a.PcaId,
                                    CurrencyId = a.CurrencyId,
                                    Subtotal = a.Subtotal,
                                    Vat = a.Vat,
                                    OtherFee = a.OtherFee,
                                    Total = a.Total,
                                    CurrencySymbol = g.CurrencySymbol,
                                    CurrencySymbolOriginal = (from x in context.PceTasks
                                                              join y in context.Currencies on x.CurrencyId equals y.Id
                                                              where x.PceId == a.Id
                                                              select y.CurrencySymbol).FirstOrDefault(),
                                    SubtotalOriginal = a.SubtotalOriginal,
                                    VatOriginal = a.VatOriginal,
                                    OtherFeeOriginal = a.OtherFeeOriginal,
                                    TotalOriginal = a.TotalOriginal,
                                    PceMarginOriginal = a.PceMarginOriginal
                                })
                                .Where(x => isAdministrator ? true : (team.Any(y => y.JobId == x.JobId) || x.CreatedBy == employeeId || approval.Any(y => y.RefId == x.Id)))
                                .OrderByDescending(x => x.ModifiedOn).ToListAsync();

            return result;
        }

        public async Task<Pce> Get(string Id)
        {
            var result = await context.Pces.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();

            if (result != null)
            {
                result.CurrencySymbol = context.Currencies.Where(x => x.Id == result.CurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault();
                result.CurrencySymbolOriginal = (from x in context.PceTasks
                                                 join y in context.Currencies on x.CurrencyId equals y.Id
                                                 where x.PceId == result.Id
                                                 select y.CurrencySymbol).FirstOrDefault();

                var client = await GetClientByJobId(result.JobId);
                if (client != null)
                {
                    result.CompanyName = client.CompanyName;
                    result.ClientBrand = client.BrandName;
                    result.JobName = client.JobName;
                    result.CompanyId = client.CompanyId;

                    if (!string.IsNullOrEmpty(client.MasterOutletId))
                    {
                        result.MasterOutletId = client.MasterOutletId;
                        var location = await (from a in context.MasterOutlets
                                              join b in context.Citys on a.CityId equals b.Id
                                              where a.Id == result.MasterOutletId
                                              select b.CityName).FirstOrDefaultAsync();
                        result.MasterOutletLocation = location;
                    }
                }

                //sudah di masukan ketika proses approve pca 16/8/2020
                //result.Balance = 0;
                //result.CurrentInvoice = 0;

                //if ((result.Status ?? StatusTransactionName.Draft) == StatusTransactionName.Draft)
                //{
                //    result.OtherFeePercentage = client.Asf;
                //    result.OtherFee = Math.Round(((result.Subtotal * result.OtherFeePercentage) / 100) ?? 0);
                //    result.Vat = Math.Round(Convert.ToDecimal(0.1) * ((result.Subtotal ?? 0) + (result.OtherFee ?? 0)));
                //    result.Total = (result.Subtotal + result.OtherFee) + result.Vat;

                //    var totalCost = context.PceTasks.Where(x => x.PceId == result.Id).Sum(x => (x.Cost ?? 0));
                //    result.PceMargin = (result.Subtotal + result.OtherFee) - totalCost;
                //}

                //var invoice = await context.InvoiceClients.Where(x => x.PceId == Id).ToListAsync();
                //if (invoice.Count > 0)
                //{
                //    result.Balance = result.Total - invoice.Sum(x => x.TotalAmount);
                //    result.CurrentInvoice = invoice.Sum(x => x.TotalAmount);
                //}
            }

            return result;
        }

        private async Task<dynamic> GetClientByJobId(string jobId)
        {
            //return await (from a in context.JobDetail
            //              join b in context.Companys on a.CompanyId equals b.Id
            //              where !(a.IsDeleted ?? false) && a.Id == jobId
            //              select new { b.CompanyName, a.MainServiceCategory }).FirstOrDefaultAsync();

            var CompanyName = "";
            var BrandName = "";
            var Asf = 0;
            var jobDetail = await context.JobDetail.Where(x => !(x.IsDeleted ?? false) && x.Id == jobId).FirstOrDefaultAsync();
            if (jobDetail != null)
            {
                var company = await context.Companys.Where(x => x.Id == jobDetail.CompanyId).FirstOrDefaultAsync();
                if (company != null) { CompanyName = company.CompanyName; }
                var brand = await context.CompanyBrands.Where(x => x.Id == jobDetail.BrandId).FirstOrDefaultAsync();
                if (brand != null)
                {
                    BrandName = brand.BrandName;
                    Asf = (brand.ASFValue ?? 0);
                }
            }

            return new { jobDetail.JobName, jobDetail.JobNumber, jobDetail.MainServiceCategory, jobDetail.CompanyId, CompanyName, BrandName, jobDetail.MasterOutletId, Asf };
        }

        public async Task<List<PceTask>> GetTaskList(string PceId)
        {
            var businessUnit = (from x in context.BusinessUnits
                                join y in context.BusinessUnitTypes on x.BusinessUnitTypeId equals y.Id
                                select new { x.Id, y.BusinessUnitLevel });
            var result = await (from a in context.PceTasks
                                    //join x in context.BusinessUnits on a.Departement equals x.Id
                                    //join y in context.BusinessUnitTypes on x.BusinessUnitTypeId equals y.Id
                                join b in context.Uom on a.UomId equals b.Id
                                join c in context.Pces on a.PceId equals c.Id
                                where a.PceId == PceId //&& y.BusinessUnitLevel == BusinessUnitLevelCode.Department
                                && (a.Departement == null ? true : businessUnit.Any(x => x.Id == a.Departement && x.BusinessUnitLevel == BusinessUnitLevelCode.Department))
                                select new PceTask
                                {
                                    Id = a.Id,
                                    PceId = a.PceId,
                                    TaskName = a.TaskName,
                                    Quantity = a.Quantity,
                                    UomId = a.UomId,
                                    //UomName = b.unit_name,
                                    UomSymbol = b.unit_symbol,
                                    Price = a.Price,
                                    Cost = a.Cost,
                                    Percentage = a.Percentage,
                                    Margin = a.Margin,
                                    UnitPrice = a.UnitPrice,
                                    Total = a.Total,
                                    IsPpn = a.IsPpn,
                                    Ppn = a.Ppn,
                                    IsAsf = a.IsAsf,
                                    Asf = a.Asf,
                                    Pph = a.Pph,
                                    Departement = a.Departement,
                                    ParentId = a.ParentId,
                                    //DepartementName = x.UnitName,
                                    DepartementName = context.BusinessUnits.Where(x => a.Departement == x.Id).Select(x => x.UnitName).FirstOrDefault(),
                                    IsRateCard = a.IsRateCard,
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
                                    CurrencyId = a.CurrencyId,
                                    ExchangeRate = a.ExchangeRate,
                                    Operator = a.Operator,
                                    TotalOriginal = a.TotalOriginal,
                                    CurrencySymbol = context.Currencies.Where(x => x.Id == c.CurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault(),
                                    CurrencySymbolRequest = context.Currencies.Where(x => x.Id == a.CurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault()
                                }).OrderByDescending(x => x.ModifiedOn).ToListAsync();
            return result;
        }


        public async Task<dynamic> GetInvoiceDetailList(string pceId)
        {
            var result = await context.InvoiceClients.AsNoTracking().Where(x => x.PceId == pceId).ToListAsync();
            foreach (var item in result)
            {
                item.PceBalance = result.Where(x => x.InvoiceDate <= item.InvoiceDate).Sum(x => x.TotalAmount);
                item.CurrencySymbol = await context.Currencies.Where(x => x.Id == item.CurrencyId).Select(x => x.CurrencySymbol).FirstOrDefaultAsync();
            }
            return result;
        }

        public Task<List<Pce>> Get()
        {
            throw new NotImplementedException();
        }

        public Task<List<Pce>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        //public async Task<Pce> GetByJobId(string jobId)
        //{
        //    var result = await context.Pces.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.JobId == jobId).FirstOrDefaultAsync();

        //    if (result != null)
        //    {
        //        var client = await GetClientByJobId(result.JobId);
        //        if (client != null)
        //        {
        //            result.CompanyName = client.CompanyName;
        //            result.ClientBrand = client.BrandName;
        //        }

        //        result.Balance = 0;
        //        result.CurrentInvoice = 0;

        //        if ((result.Status ?? StatusTransactionName.Draft) == StatusTransactionName.Draft)
        //        {
        //            result.OtherFeePercentage = client.Asf;
        //            result.OtherFee = Math.Round(((result.Subtotal * result.OtherFeePercentage) / 100) ?? 0);
        //            result.Vat = Math.Round(Convert.ToDecimal(0.1) * ((result.Subtotal ?? 0) + (result.OtherFee ?? 0)));
        //            result.Total = (result.Subtotal + result.OtherFee) + result.Vat;

        //            var totalCost = context.PceTasks.Where(x => x.PceId == result.Id).Sum(x => (x.Cost ?? 0));
        //            result.PceMargin = (result.Subtotal + result.OtherFee) - totalCost;
        //        }

        //        var invoice = await context.InvoiceClients.Where(x => x.PceId == result.Id).ToListAsync();
        //        if (invoice.Count > 0)
        //        {
        //            result.Balance = result.Total - invoice.Sum(x => x.TotalAmount);
        //            result.CurrentInvoice = invoice.Sum(x => x.TotalAmount);
        //        }
        //    }

        //    return result;
        //}

        public async Task<dynamic> Print(string pceId)
        {
            var result = await (from a in context.Pces
                                join b in context.JobDetail on a.JobId equals b.Id
                                join d in context.CompanyBrands on b.BrandId equals d.Id
                                join e in context.Companys on b.CompanyId equals e.Id
                                join f in context.Pcas on a.PcaId equals f.Id
                                join g in context.LegalEntity on b.LegalEntityId equals g.Id
                                join h in context.Currencies on a.CurrencyId equals h.Id
                                where !(a.IsDeleted ?? false) && a.Id == pceId
                                select new
                                {
                                    Client = e.CompanyName.ToUpper(),
                                    ClientAddress = (e.BillingAddress ?? "") != "" ? e.BillingAddress : e.OfficeAddress,
                                    ClientTelp = e.Phone,
                                    ClientFax = e.Fax,
                                    ClientEmail = e.Email,
                                    LegalEntity = g.LegalEntityName.ToUpper(),
                                    LegalEntityAddress = (g.BillingAddress ?? "") != "" ? g.BillingAddress : g.OfficeAddress,
                                    LegalEntityTelp = g.Phone,
                                    LegalEntityFax = g.Fax,
                                    LegalEntityEmail = g.Email,
                                    Date = a.CreatedOn,
                                    JobNo = b.JobNumber,
                                    RefNo = "",
                                    Brand = d.BrandName,
                                    BudgetNo = f.Code,
                                    JobTitle = b.JobName,
                                    b.JobDescription,
                                    CurrencyName = (from x in context.PceTasks
                                                    join y in context.Currencies on x.CurrencyId equals y.Id
                                                    where a.Id == x.PceId
                                                    select y.CurrencyName).FirstOrDefault() ?? h.CurrencyName,
                                    CurrencySymbol = (from x in context.PceTasks
                                                      join y in context.Currencies on x.CurrencyId equals y.Id
                                                      where a.Id == x.PceId
                                                      select y.CurrencySymbol).FirstOrDefault() ?? h.CurrencySymbol,
                                    Subtotal = a.SubtotalOriginal ?? a.Subtotal,
                                    OtherFee = a.OtherFeeOriginal ?? a.OtherFee,
                                    a.OtherFeeName,
                                    a.OtherFeePercentage,
                                    Vat = a.VatOriginal ?? a.Vat,
                                    Total = a.TotalOriginal ?? a.Total,
                                    PrintTasks = context.PceTasks.Where(x => x.PceId == a.Id).Select(x => new
                                    {
                                        TaskId = x.Id,
                                        x.TaskName,
                                        TotalPrice = x.TotalOriginal ?? (x.Total ?? 0)
                                    })
                                }).FirstOrDefaultAsync();
            return result;
        }
    }
}