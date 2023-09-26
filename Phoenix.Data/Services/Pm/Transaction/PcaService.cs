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
using Phoenix.Data.Models.Enum;
using Phoenix.Data.Models.Finance.Master;
using Phoenix.Data.RestApi;
using Phoenix.Shared.Core.Contexts;

namespace Phoenix.Data
{
    public interface IPcaService : IDataService<Pca>
    {
        Task<List<PcaTask>> GetTaskList(string pcaId);
        Task<int> ApproveAsync(Pca entity);
        Task<RequestForQuotation> RFQAsync(Pca entity);
        Task<dynamic> GetClientByJobId(string jobId);
        Task<List<Pca>> GetList(string status);
    }

    public class PcaService : IPcaService
    {
        readonly DataContext context;
        readonly GlobalFunctionApproval globalFcApproval;
        readonly Services.Um.IManageMenuService menuService;
        readonly StatusLogService log;
        readonly FileService uploadFile;

        /// <summary>
        /// And endpoint to manage Pca
        /// </summary>
        /// <param name="context">Database context</param>
        public PcaService(DataContext context, GlobalFunctionApproval globalFcApproval, Services.Um.IManageMenuService menuService, StatusLogService log, FileService uploadFile)
        {
            this.context = context;
            this.globalFcApproval = globalFcApproval;
            this.menuService = menuService;
            this.log = log;
            this.uploadFile = uploadFile;
        }

        public async Task<int> AddAsync(Pca entity)
        {
            return await AddEdit(entity, false);
        }

        public async Task<int> EditAsync(Pca entity)
        {
            return await AddEdit(entity, true);
        }

        private async Task<int> AddEdit(Pca entity, bool isEdit)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var jobDetail = await context.JobDetail.AsNoTracking().Where(x => x.Id == entity.JobId).FirstOrDefaultAsync();
                    jobDetail.MasterOutletId = entity.MasterOutletId;
                    context.JobDetail.Update(jobDetail);

                    int revision;
                    bool success = Int32.TryParse(entity.Revision, out revision);
                    if (!success) revision = 0;
                    if (context.Pcas.Where(x => x.Id == entity.Id && x.Status == StatusTransactionName.Revised).Count() > 0) entity.Revision = (revision + 1).ToString();

                    entity.Status = StatusTransaction.StatusName(entity.StatusApproval);
                    if (isEdit)
                    {
                        context.PhoenixEdit(entity);
                    }
                    else
                    {
                        entity.Id = await TransID.GetTransId(context, Code.PCA, DateTime.Today);
                        entity.Code = entity.Id;
                        await context.PhoenixAddAsync(entity);
                        await context.SaveChangesAsync();
                    }

                    if (!string.IsNullOrEmpty(entity.Id))
                    {
                        var task = context.PcaTasks.AsNoTracking().Where(x => x.PcaId == entity.Id).ToList();
                        if (task.Count > 0)
                        {
                            context.PcaTasks.RemoveRange(task.ToArray());
                        }
                    }

                    if (entity.PcaTasks != null)
                    {
                        foreach (var item in entity.PcaTasks.Where(x => string.IsNullOrEmpty(x.ParentId)))
                        {
                            item.PcaId = entity.Id;
                            await context.PhoenixAddAsync(item);
                        }
                        await context.SaveChangesAsync();

                        foreach (var item in entity.PcaTasks.Where(x => !string.IsNullOrEmpty(x.ParentId)))
                        {
                            item.PcaId = entity.Id;
                            await context.PhoenixAddAsync(item);
                        };
                    }

                    var isApprove = true;
                    if (entity.Status != StatusTransactionName.Draft)
                    {
                        await globalFcApproval.clearApproval(entity.Id);

                        var dataMenu = await menuService.GetByUniqueName(MenuUnique.PCA).ConfigureAwait(false);
                        var dataTempate = context.TmTemplateApprovals.Where(x => x.MenuId == dataMenu.Id && x.IsDeleted == false).FirstOrDefault();
                        if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
                        var vm = new Models.ViewModel.TransApprovalHrisVm()
                        {
                            IdTemplate = dataTempate.Id,
                            MenuId = dataMenu.Id,
                            RefId = entity.Id,
                            DetailLink = $"{ApprovalLink.PCA}?Id={entity.Id}",
                            NotificationType = NotificationTypeCode.PCANotification,
                            Module = NotificationModuleType.ModulePM
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
                        context.Pcas.Update(entity); await log.AddAsync(new StatusLog() { TransactionId = entity.Id, Status = entity.Status, Description = entity.RemarkRejected });
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

        public async Task<int> ApproveAsync(Pca entity)
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
                            var id = await TransID.GetTransId(context, Code.PCE, DateTime.Today);
                            var model = await context.Pces.AsNoTracking().Where(x => x.PcaId == entity.Id).FirstOrDefaultAsync();
                            if (model != null) { id = model.Id; }
                            var asf = await (from a in context.CompanyBrands
                                             join b in context.JobDetail on a.Id equals b.BrandId
                                             where b.Id == entity.JobId
                                             select a.ASFValue).FirstOrDefaultAsync();

                            var subTotalASF = (context.PcaTasks.Where(x => x.PcaId == entity.Id && !(x.IsDeleted ?? false) && x.IsAsf == true).Sum(x => x.Total) ?? 0);
                            var Subtotal = (context.PcaTasks.Where(x => x.PcaId == entity.Id && !(x.IsDeleted ?? false)).Sum(x => x.Total) ?? 0);//(entity.Subtotal ?? 0);
                            var OtherFeePercentage = Convert.ToDecimal(asf ?? 0);
                            var OtherFee = (subTotalASF * (OtherFeePercentage / 100));//Math.Round(((subTotalASF * OtherFeePercentage) / 100));
                            var Vat = Convert.ToDecimal(0.1) * (Subtotal + OtherFee);//Math.Round(Convert.ToDecimal(0.1) * (Subtotal + OtherFee));
                            var Total = (Subtotal + OtherFee) + Vat;

                            //var totalCost = context.PceTasks.Where(x => x.PceId == result.Id).Sum(x => (x.Cost ?? 0));
                            var PceMargin = (Subtotal + OtherFee);// - totalCost;

                            var subTotalASF_Original = (context.PcaTasks.Where(x => x.PcaId == entity.Id && !(x.IsDeleted ?? false) && x.IsAsf == true).Sum(x => x.TotalOriginal) ?? 0);
                            var Subtotal_Original = (context.PcaTasks.Where(x => x.PcaId == entity.Id && !(x.IsDeleted ?? false)).Sum(x => x.TotalOriginal) ?? 0);
                            var OtherFee_Original = (subTotalASF_Original * (OtherFeePercentage / 100));//Math.Round(((subTotalASF * OtherFeePercentage) / 100));
                            var Vat_Original = Convert.ToDecimal(0.1) * (Subtotal_Original + OtherFee_Original);//Math.Round(Convert.ToDecimal(0.1) * (Subtotal + OtherFee));
                            var Total_Original = (Subtotal_Original + OtherFee_Original) + Vat_Original;

                            var PceMargin_Original = (Subtotal_Original + OtherFee_Original);// - totalCost;

                            var pce = new Pce()
                            {
                                Id = id,
                                Code = id,
                                JobId = entity.JobId,
                                Revision = entity.Revision,
                                CurrencyId = entity.CurrencyId,
                                MotherPcaId = entity.MotherPcaId,
                                Subtotal = Subtotal,
                                OtherFee = OtherFee,
                                OtherFeePercentage = OtherFeePercentage,
                                Vat = Vat,
                                Total = Total,
                                Status = StatusTransactionName.Draft,
                                TypeOfExpenseId = entity.TypeOfExpenseId,
                                ShareservicesId = entity.ShareservicesId,
                                MainserviceCategoryId = entity.MainserviceCategoryId,
                                Termofpayment1 = entity.Termofpayment1,
                                Termofpayment2 = entity.Termofpayment2,
                                Termofpayment3 = entity.Termofpayment3,
                                RateCardId = entity.RateCardId,
                                PcaId = entity.Id,
                                PceMargin = PceMargin,
                                TotalBalanceInvoice = Total_Original != 0 ? Total_Original : Total,
                                TotalUsedInvoice = 0,
                                CreatedBy = entity.CreatedBy,
                                CreatedOn = entity.CreatedOn,
                                ModifiedBy = entity.ModifiedBy,
                                ModifiedOn = entity.ModifiedOn,
                                SubtotalOriginal = Subtotal_Original == 0 ? null : (decimal?)Subtotal_Original,
                                OtherFeeOriginal = Subtotal_Original == 0 ? null : (decimal?)OtherFee_Original,
                                VatOriginal = Subtotal_Original == 0 ? null : (decimal?)Vat_Original,
                                TotalOriginal = Subtotal_Original == 0 ? null : (decimal?)Total_Original,
                                PceMarginOriginal = Subtotal_Original == 0 ? null : (decimal?)PceMargin_Original
                            };

                            if (model == null)
                            {
                                await context.AddAsync(pce);
                                await context.SaveChangesAsync();
                            }
                            else
                            {
                                context.Update(pce);
                                var task = context.PceTasks.AsNoTracking().Where(x => x.PceId == pce.Id).ToList();
                                if (task.Count > 0) { context.PceTasks.RemoveRange(task.ToArray()); }
                            }

                            var pceTask = entity.PcaTasks.Select(x => new PceTask
                            {
                                Id = x.Id,
                                PceId = pce.Id,
                                TaskName = x.TaskName,
                                Quantity = x.Quantity,
                                UomId = x.UomId,
                                Price = x.Price,
                                IsPpn = x.IsPpn,
                                Ppn = x.Ppn,
                                IsAsf = x.IsAsf,
                                Asf = x.Asf,
                                Pph = x.Pph,
                                Cost = x.Cost,
                                Percentage = x.Percentage,
                                Margin = x.Margin,
                                UnitPrice = x.UnitPrice,
                                Total = x.Total,
                                Departement = x.Departement,
                                IsRateCard = x.IsRateCard,
                                ParentId = x.ParentId,
                                TotalBalanceInvoice = (x.TotalOriginal ?? x.Total),
                                TotalUsedInvoice = 0,
                                CurrencyId = x.CurrencyId,
                                ExchangeRate = x.ExchangeRate,
                                Operator = x.Operator,
                                TotalOriginal = x.TotalOriginal,
                                CreatedBy = entity.CreatedBy,
                                CreatedOn = entity.CreatedOn,
                                ModifiedBy = entity.ModifiedBy,
                                ModifiedOn = entity.ModifiedOn
                            });

                            if (pceTask != null)
                            {
                                foreach (var item in pceTask.Where(x => string.IsNullOrEmpty(x.ParentId)))
                                {
                                    item.PceId = pce.Id;
                                    await context.AddAsync(item);
                                };
                                await context.SaveChangesAsync();

                                foreach (var item in pceTask.Where(x => !string.IsNullOrEmpty(x.ParentId)))
                                {
                                    item.PceId = pce.Id;
                                    await context.AddAsync(item);
                                };
                            }

                            var job = await context.JobDetail.AsNoTracking().FirstOrDefaultAsync(x => x.Id == entity.JobId);
                            job.JobStatusId = "PCA Approved";
                            context.JobDetail.Update(job);
                        }

                        context.Pcas.Update(entity);
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

        public Task<int> AddRangeAsync(params Pca[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(Pca entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params Pca[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(Pca entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params Pca[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> EditRangeAsync(Pca entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Pca>> GetList(string status)
        {
            //yang bisa akses ke pca adalah pm team, creator dan approval
            var userId = context.GetUserId();
            var employeeId = context.GetEmployeeId();
            var groupId = await context.TmUserApps.Where(x => x.EmployeeBasicInfoId == employeeId).Select(x => x.GroupId).FirstOrDefaultAsync();
            var isAdministrator = groupId == "1908b01c-688a-4ed4-813e-5f622de0b794";

            var team = (from a in context.PmTeams
                        join b in context.PmTeamMembers on a.Id equals b.TeamId
                        where !(a.IsDeleted ?? false) && !(b.IsDeleted ?? false) && b.ApplicationUserId == userId
                        select new { a.JobId });

            var approval = (from a in context.TrTemplateApprovals
                            join b in context.TrUserApprovals on a.Id equals b.TrTempApprovalId
                            where b.EmployeeBasicInfoId == employeeId
                            select new { a.RefId });

            var result = await (from a in context.Pcas
                                join b in context.JobDetail on a.JobId equals b.Id
                                join d in context.CompanyBrands on b.BrandId equals d.Id
                                join e in context.Companys on b.CompanyId equals e.Id
                                where !(a.IsDeleted ?? false) && //e.Is_client == true &&
                                (string.IsNullOrEmpty(status) ? true : (a.Status ?? StatusTransactionName.Draft) == status)
                                select new Pca
                                {
                                    Id = a.Id,
                                    Code = a.Code,
                                    JobId = a.JobId,
                                    Total = a.Total,
                                    CostAnalysis = a.CostAnalysis,
                                    Status = a.Status,
                                    Revision = a.Revision,
                                    JobName = $"{a.JobId} - {b.JobName}",
                                    CompanyName = e.CompanyName,
                                    BrandName = d.BrandName,
                                    PceId = context.Pces.Where(x => x.PcaId == a.Id).Select(x => x.Id).FirstOrDefault(),
                                    CreatedOn = a.CreatedOn,
                                    CreatedBy = a.CreatedBy,
                                    ModifiedOn = a.ModifiedOn,
                                    CurrencySymbol = context.Currencies.Where(x => x.Id == a.CurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault()
                                })
                                .Where(x => isAdministrator ? true : (team.Any(y => y.JobId == x.JobId) || x.CreatedBy == employeeId || approval.Any(y => y.RefId == x.Id)))
                                .OrderByDescending(x => x.ModifiedOn).ToListAsync();

            return result;
        }

        public async Task<Pca> Get(string Id)
        {
            var result = await context.Pcas.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
            var isJobOwner = false;
            if (result != null)
            {
                var jobId = result.JobId;
                var teamId = await context.PmTeams.Where(x => !(x.IsDeleted ?? false) && x.JobId == jobId).Select(x => x.Id).FirstOrDefaultAsync();
                if (teamId != null)
                {
                    var userId = context.GetUserId();
                    var isJobOwnerExists = await context.PmTeamMembers.Where(x => !(x.IsDeleted ?? false) && x.TeamId == teamId && x.ApplicationUserId == userId && x.ActAs == "JO").FirstOrDefaultAsync();
                    if (isJobOwnerExists != null)
                    {
                        isJobOwner = true;
                    }
                }
                result.IsJobOwner = isJobOwner;
                result.DefaultCurrencyId = context.Currencies.Where(x => !(x.IsDeleted ?? false) && x.IsDefault == true).Select(x => x.Id).FirstOrDefault();
                result.CurrencySymbol = await context.Currencies.Where(x => x.Id == result.CurrencyId).Select(x => x.CurrencySymbol).FirstOrDefaultAsync();
                var client = await GetClientByJobId(result.JobId);
                if (client != null)
                {
                    result.CompanyName = client.CompanyName;
                    result.ClientBrand = client.BrandName;
                    result.JobNumber = client.JobNumber;
                    result.JobName = client.JobName;
                    result.CompanyId = client.CompanyId;
                    result.JobStatusId = client.JobStatusId;

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

                var mother = await context.MotherPcas.Where(x => x.Id == result.MotherPcaId).FirstOrDefaultAsync();
                if (mother != null)
                {
                    result.TotalMotherPCA = mother.Total;
                }

                var pce = await context.Pces.Where(x => x.PcaId == Id).FirstOrDefaultAsync();
                if (pce != null)
                {
                    result.StatusPCE = pce.Status;
                    result.PceId = pce.Id;
                }
            }

            return result;
        }

        public async Task<dynamic> GetClientByJobId(string jobId)
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

            return new { jobDetail.JobStatusId, jobDetail.JobName, jobDetail.JobNumber, jobDetail.MainServiceCategory, jobDetail.CompanyId, CompanyName, BrandName, jobDetail.MasterOutletId, Asf };
        }

        public async Task<List<PcaTask>> GetTaskList(string pcaId)
        {
            // var result = (from a in context.PcaTasks
            // join x in context.BusinessUnits on a.Departement equals x.Id
            // join y in context.BusinessUnitTypes on x.BusinessUnitTypeId equals y.Id
            // join b in context.Uom on a.UomId equals b.Id
            // where y.BusinessUnitLevel == BusinessUnitLevelCode.Department && a.PcaId == pcaId

            var businessUnit = (from x in context.BusinessUnits
                                join y in context.BusinessUnitTypes on x.BusinessUnitTypeId equals y.Id
                                select new { x.Id, y.BusinessUnitLevel });
            var result = await (from a in context.PcaTasks
                                    //join x in context.BusinessUnits on a.Departement equals x.Id
                                    //join y in context.BusinessUnitTypes on x.BusinessUnitTypeId equals y.Id
                                join b in context.Uom on a.UomId equals b.Id
                                join c in context.Pcas on a.PcaId equals c.Id
                                where a.PcaId == pcaId //&& y.BusinessUnitLevel == BusinessUnitLevelCode.Department
                                && (a.Departement == null ? true : businessUnit.Any(x => x.Id == a.Departement && x.BusinessUnitLevel == BusinessUnitLevelCode.Department))
                                select new PcaTask
                                {
                                    Id = a.Id,
                                    PcaId = a.PcaId,
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
                                    TotalPrice = a.TotalPrice,
                                    TotalPriceUsed = a.TotalPriceUsed,
                                    TotalPriceBalance = a.TotalPriceBalance,
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
                                    Status = a.Status,
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

        public async Task<RequestForQuotation> RFQAsync(Pca entity)
        {
            //await validateRFQCA(entity);
            string pcaId = null;
            string affiliationId = null;
            string businessUnitId = null;
            string legalEntityId = null;

            var file = await uploadFile.Upload(entity.file, entity.DetailSpecFile);
            //if (!string.IsNullOrEmpty(file))
            //{
            //    entity.DetailSpecFile = file;
            //}

            var jobDetail = await context.JobDetail.AsNoTracking().Where(x => x.Id == entity.JobId).FirstOrDefaultAsync();
            if (jobDetail != null)
            {
                legalEntityId = jobDetail.LegalEntityId;
                affiliationId = jobDetail.AffiliationId;
                businessUnitId = jobDetail.BusinessUnitDivisionId;
            }

            var pce = await context.Pces.AsNoTracking().Where(x => x.PcaId == entity.Id && x.Status.ToUpper() == PCEStatus.Approved.ToUpper()).FirstOrDefaultAsync();
            if (pce == null) throw new Exception($"can't request RFQ because it's still in the PCE process");
            if (pce != null) pcaId = pce.Id;

            //var task = entity.PcaTasks.Where(x => x.ParentId == null && x.Status == PCAStatus.Open).ToList();
            var task = entity.PcaTaskRequest.ToList();
            if (task.Count > 0)
            {
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var rfqId = await TransID.GetTransId(context, Code.RFQ, DateTime.Today);
                        var RFQ = new RequestForQuotation
                        {
                            Id = rfqId,
                            RequestForQuotationNumber = rfqId,
                            JobId = entity.JobId,
                            JobPaId = entity.Id,
                            JobPeId = pcaId,
                            AffiliationId = affiliationId,
                            LegalEntityId = legalEntityId,
                            BusinessUnitId = businessUnitId,
                            RequestForQuotationDate = DateTime.Now,
                            Status = StatusTransactionName.Draft,
                            MainserviceCategoryId = entity.MainserviceCategoryId,
                            IsPreferedVendor = entity.IsPrefered,
                            DueDate = entity.DueDate,
                            //CurrencyId = entity.CurrencyId,
                            CurrencyId = entity.CurrencyId,
                            RequestCurrencyId = entity.CurrencyRequestId,
                            TypeOfExpenseId = entity.TypeOfExpenseId,
                            Remarks = entity.RemarkRFQ,
                            InternalOrProject = "Project",
                            DetailSpecFile = file
                        };

                        await context.PhoenixAddAsync(RFQ);
                        await context.SaveChangesAsync();

                        foreach (var item in task)
                        {
                            //var RFQTask = new RequestForQuotationTask
                            //{
                            //    Id = Guid.NewGuid().ToString(),
                            //    RequestForQuotationId = RFQ.Id,
                            //    Task = item.TaskName,
                            //    Quantity = item.Quantity,
                            //    UomId = item.UomId,
                            //    TaskId = item.Id,
                            //    JobId = entity.JobId,
                            //    UnitPrice = item.Price,
                            //    TotalPrice = item.Price * item.Quantity
                            //};

                            var RFQTask = new RequestForQuotationTask
                            {
                                Id = Guid.NewGuid().ToString(),
                                RequestForQuotationId = RFQ.Id,
                                Task = item.Task,
                                Quantity = item.Quantity,
                                UomId = item.UomId,
                                TaskId = item.TaskId,
                                JobId = entity.JobId,
                                UnitPrice = item.UnitPrice,
                                TotalPrice = item.TotalPrice,
                                TotalRequest = item.TotalRequest,
                                TotalExchange = item.TotalExchangeRequest,
                                CurrencyId = entity.CurrencyRequestId,
                                ExchangeRate = entity.ExchangeRateRequest,
                                Operator = entity.OperatorRequest,
                                //ItemTypeId = ItemTypeId.NonInventory
                            };
                            context.PhoenixAdd(RFQTask);

                            //context.Database.ExecuteSqlCommand($"UPDATE pm.pca_task SET status={PCAStatus.RFQ} WHERE Id={item.TaskId}");

                            var taskDtl = await context.PcaTasks.AsNoTracking().Where(x => x.PcaId == entity.Id && x.Id == item.TaskId).FirstOrDefaultAsync();
                            taskDtl.Status = PCAStatus.RFQ;
                            taskDtl.TotalPriceUsed = (taskDtl.TotalPriceUsed ?? 0) + item.TotalExchangeRequest;
                            taskDtl.TotalPriceBalance = (taskDtl.TotalPrice ?? 0) - taskDtl.TotalPriceUsed;

                            if (taskDtl.TotalPriceBalance == null)
                            {
                                throw new Exception("Remaining Budget Cannot be Empty.");
                            }

                            if (taskDtl.TotalPriceBalance < 0)
                            {
                                throw new Exception("the total exchange requests must be less than or equal to the remaining budget");
                            }

                            context.PhoenixEdit(taskDtl);
                        }

                        await context.SaveChangesAsync();
                        var pca = await context.Pcas.AsNoTracking().Where(x => x.Id == entity.Id).FirstOrDefaultAsync();
                        if (pca != null)
                        {
                            pca.TotalTaskPriceUsed = context.PcaTasks.Where(x => !(x.IsDeleted ?? false) && x.PcaId == pca.Id).Sum(x => (x.TotalPriceUsed ?? 0));
                            pca.TotalTaskPriceBalance = context.PcaTasks.Where(x => !(x.IsDeleted ?? false) && x.PcaId == pca.Id).Sum(x => (x.TotalPriceBalance ?? 0));
                            context.Pcas.Update(pca);
                        }

                        var save = await context.SaveChangesAsync();
                        transaction.Commit();
                        return RFQ;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception(ex.Message);
                    }
                }
            }
            else
            {
                throw new Exception($"for this task is in the process...");
            }
        }

        //public async Task validateRFQCA(Pca entity)
        //{
        //    var taskAmount = await (from a in context.RequestForQuotationDetail
        //                            join b in context.RequestForQuotationVendor on new { a.RequestForQuotationId, a.VendorId } equals new { b.RequestForQuotationId, b.VendorId }
        //                            join c in context.RequestForQuotation on b.RequestForQuotationId equals c.Id
        //                            where (b.IsRequestPurchaseOrder ?? false) && c.JobPaId == entity.Id && c.Status == StatusTransactionName.Approved
        //                            group a by new { a.TaskId, a.Task } into g
        //                            select new
        //                            {
        //                                TaskId = g.Key.TaskId,
        //                                TaskName = g.Key.Task,
        //                                TotalAmount = g.Sum(x => (x.Amount ?? 0))
        //                            }).ToListAsync();

        //    var taskAmountCA = await (from a in context.CashAdvanceDetails
        //                              join c in context.CashAdvances on a.CashAdvanceId equals c.Id
        //                              where c.PcaId == entity.Id && c.Status == StatusTransactionName.Approved
        //                              group a by new { a.TaskId, a.TaskName } into g
        //                              select new
        //                              {
        //                                  TaskId = g.Key.TaskId,
        //                                  TaskName = g.Key.TaskName,
        //                                  TotalAmount = g.Sum(x => (x.Amount ?? 0))
        //                              }).ToListAsync();

        //    var taskAmountReq = entity.PcaTasks.Where(x => !(x.IsDeleted ?? false) && x.PcaId == entity.Id).GroupBy(x => new { x.Id, x.TaskName }).Select(x => new { TaskId = x.Key.Id, TaskName = x.Key.TaskName, TotalAmount = x.Sum(y => (y.Total ?? 0)) }).ToList();

        //    taskAmount.AddRange(taskAmountCA);

        //    var existsRFQ = (from a in taskAmount
        //                     join b in taskAmountReq on new { a.TaskId, a.TotalAmount } equals new { b.TaskId, b.TotalAmount }
        //                     select a).ToList();

        //    var existsCA = (from a in taskAmountCA
        //                    join b in taskAmountReq on new { a.TaskId, a.TotalAmount } equals new { b.TaskId, b.TotalAmount }
        //                    select a).ToList();

        //    var message = "";
        //    var rfqca = "RFQ";

        //    if (existsRFQ.Count > 0)
        //    {
        //        Parallel.ForEach(existsRFQ, (item) =>
        //        {
        //            message += item.TaskName;
        //        });
        //    }

        //    if (existsCA.Count > 0)
        //    {
        //        Parallel.ForEach(existsCA, (item) =>
        //        {
        //            message += item.TaskName;
        //        });
        //    }

        //    if (existsRFQ.Count > 0 && existsCA.Count > 0) rfqca = "RFQ and CA";
        //    else if (existsRFQ.Count > 0) rfqca = "RFQ";
        //    else if (existsCA.Count > 0) rfqca = "CA";

        //    if (message != "") throw new Exception($"task {message} cannot be processed, because they have already been processed in {rfqca}.");
        //}

        public Task<List<Pca>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public Task<List<Pca>> Get()
        {
            throw new NotImplementedException();
        }
    }
}