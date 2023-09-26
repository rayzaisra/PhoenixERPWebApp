using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models;
using Phoenix.Data.RestApi;
using Phoenix.Shared.Core.Contexts;

namespace Phoenix.Data
{
    public interface IMapCoaTransaksiService
    {
        #region Setting Journal Job Closing
        Task<dynamic> GetMasterClosing();
        Task<dynamic> GetMasterClosingById(string mainServiceCategoryId, string businessUnitId);
        Task<List<MapCoaTransaksi>> GetMasterClosingAP(string mainServiceCategoryId, string businessUnitId, bool isEdit);
        Task<List<MapCoaTransaksi>> GetMasterClosingAR(string mainServiceCategoryId, string businessUnitId, bool isEdit);
        Task<int> DeleteSoftMasterClosingAsync(string mainServiceCategoryId, string businessUnitId);
        Task<int> AddMasterClosingAsync(List<MapCoaTransaksi> entity);
        #endregion

        #region Setting Journal AP
        Task<dynamic> GetMasterAP();
        Task<dynamic> GetMasterAPById(string mainServiceCategoryId, bool pceTypeCrossBilling);
        Task<List<MapCoaTransaksi>> GetMasterDetailAP(string mainServiceCategoryId, bool pceTypeCrossBilling, bool isEdit);
        Task<int> DeleteSoftAPAsync(string mainServiceCategoryId, bool pceTypeCrossBilling);
        Task<int> AddAPAsync(List<MapCoaTransaksi> entity);
        #endregion

        #region Setting Journal AR
        Task<dynamic> GetMasterAR();
        Task<dynamic> GetMasterARById(string mainServiceCategoryId, bool pceTypeCrossBilling, string invoiceType);
        Task<List<MapCoaTransaksi>> GetMasterDetailAR(string mainServiceCategoryId, bool pceTypeCrossBilling, string invoiceType, bool isEdit);
        Task<int> DeleteSoftARAsync(string mainServiceCategoryId, bool pceTypeCrossBilling, string invoiceType);
        Task<int> AddARAsync(List<MapCoaTransaksi> entity);
        #endregion

        #region Setting Journal Petty Cash
        Task<List<MapCoaTransaksi>> GetJournalPettyCash();
        Task<int> AddPettyCashAsync(List<MapCoaTransaksi> entity);
        #endregion

        #region Setting Journal Salary Payment Request
        Task<List<MapCoaTransaksi>> GetJournalSalaryPaymentRequest();
        Task<int> AddSalaryPaymentRequestAsync(List<MapCoaTransaksi> entity);
        #endregion
    }

    public class MapCoaTransaksiService : IMapCoaTransaksiService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage MapCoaTransaksi
        /// </summary>
        /// <param name="context">Database context</param>
        public MapCoaTransaksiService(DataContext context)
        {
            this.context = context;
        }

        #region Setting Journal Job Closing
        public async Task<dynamic> GetMasterClosing()
        {
            var result = await (from a in context.MapCoaTransaksi
                                join b in context.MainServiceCategorys on a.MainserviceCategoryId equals b.Id
                                join c in context.BusinessUnits on a.BusinessUnitId equals c.Id
                                where !(a.IsDeleted ?? false) && a.ModuleCode.Contains("CLOSING")
                                group a by new { a.MainserviceCategoryId, b.Name, a.BusinessUnitId, c.UnitName } into x
                                select new
                                {
                                    x.Key.MainserviceCategoryId,
                                    MainserviceCategoryName = x.Key.Name,
                                    x.Key.BusinessUnitId,
                                    BusinessUnitName = x.Key.UnitName
                                }).ToListAsync();
            return result;
        }

        public async Task<dynamic> GetMasterClosingById(string mainServiceCategoryId, string businessUnitId)
        {
            var result = await (from a in context.MapCoaTransaksi
                                where !(a.IsDeleted ?? false) && a.ModuleCode.Contains("CLOSING") &&
                                a.MainserviceCategoryId == mainServiceCategoryId &&
                                a.BusinessUnitId == businessUnitId
                                group a by new { a.MainserviceCategoryId, a.NameSetting, a.BusinessUnitId } into x
                                select new
                                {
                                    x.Key.MainserviceCategoryId,
                                    x.Key.NameSetting,
                                    x.Key.BusinessUnitId
                                }).FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<MapCoaTransaksi>> GetMasterClosingAP(string mainServiceCategoryId, string businessUnitId, bool isEdit)
        {
            if (isEdit)
            {
                return await (from a in context.MapCoaTransaksi
                              where !(a.IsDeleted ?? false) &&
                                       a.MainserviceCategoryId == mainServiceCategoryId &&
                                       a.BusinessUnitId == businessUnitId &&
                                       a.ModuleCode.Contains("CLOSING AP")
                              select new MapCoaTransaksi
                              {
                                  Id = a.Id,
                                  ModuleCode = a.ModuleCode,
                                  MainserviceCategoryId = a.MainserviceCategoryId,
                                  BusinessUnitId = a.BusinessUnitId,
                                  LineCode = a.LineCode,
                                  Description = a.Description,
                                  CoaId = a.CoaId,
                                  DK = a.DK,
                                  AccountName = context.ChartOfAccounts.Where(x => x.Id == a.CoaId && x.Level == "5").Select(x => x.Name5).FirstOrDefault(),
                                  CodeRec = context.ChartOfAccounts.Where(x => x.Id == a.CoaId && x.Level == "5").Select(x => x.CodeRec).FirstOrDefault(),
                                  CreatedBy = a.CreatedBy,
                                  CreatedOn = a.CreatedOn
                              }).ToListAsync();
            }
            else
            {
                return new List<MapCoaTransaksi>()
                {
                    new MapCoaTransaksi(){Id=Guid.NewGuid().ToString(), ModuleCode= "CLOSING AP",LineCode="WIP", Description="Work in Progress" },
                    new MapCoaTransaksi(){Id=Guid.NewGuid().ToString(), ModuleCode= "CLOSING AP",LineCode="COB", Description="Cost of Billing" }
                };
            }
        }

        public async Task<List<MapCoaTransaksi>> GetMasterClosingAR(string mainServiceCategoryId, string businessUnitId, bool isEdit)
        {
            if (isEdit)
            {
                return await (from a in context.MapCoaTransaksi
                              where !(a.IsDeleted ?? false) &&
                                       a.MainserviceCategoryId == mainServiceCategoryId &&
                                       a.BusinessUnitId == businessUnitId &&
                                       a.ModuleCode.Contains("CLOSING AR")
                              select new MapCoaTransaksi
                              {
                                  Id = a.Id,
                                  ModuleCode = a.ModuleCode,
                                  MainserviceCategoryId = a.MainserviceCategoryId,
                                  BusinessUnitId = a.BusinessUnitId,
                                  LineCode = a.LineCode,
                                  Description = a.Description,
                                  CoaId = a.CoaId,
                                  DK = a.DK,
                                  AccountName = context.ChartOfAccounts.Where(x => x.Id == a.CoaId && x.Level == "5").Select(x => x.Name5).FirstOrDefault(),
                                  CodeRec = context.ChartOfAccounts.Where(x => x.Id == a.CoaId && x.Level == "5").Select(x => x.CodeRec).FirstOrDefault(),
                                  CreatedBy = a.CreatedBy,
                                  CreatedOn = a.CreatedOn
                              }).ToListAsync();
            }
            else
            {
                return new List<MapCoaTransaksi>()
                {
                    new MapCoaTransaksi(){Id=Guid.NewGuid().ToString(), ModuleCode= "CLOSING AR",LineCode="PRE BILLING", Description="PRE BILLING" },
                    new MapCoaTransaksi(){Id=Guid.NewGuid().ToString(), ModuleCode= "CLOSING AR",LineCode="BILLING", Description="BILLING" }
                };
            }
        }

        public async Task<int> DeleteSoftMasterClosingAsync(string mainServiceCategoryId, string businessUnitId)
        {
            var result = await context.MapCoaTransaksi.AsNoTracking().Where(x => x.MainserviceCategoryId == mainServiceCategoryId && x.BusinessUnitId == businessUnitId).ToListAsync();
            context.PhoenixDeleteRange(result.ToArray());
            return await context.SaveChangesAsync();
        }

        public async Task<int> AddMasterClosingAsync(List<MapCoaTransaksi> entity)
        {
            var single = entity.FirstOrDefault();
            if (single != null)
            {
                if (single.isEdit == false)
                {
                    var mapCoa = await context.MapCoaTransaksi.AsNoTracking().Where(x => (x.IsDeleted ?? false) && x.ModuleCode.Contains("CLOSING") && x.MainserviceCategoryId == single.MainserviceCategoryId && x.BusinessUnitId == single.BusinessUnitId).ToListAsync();
                    if (mapCoa.Count > 0)
                    {
                        context.RemoveRange(mapCoa.ToArray());
                    }
                }
            }

            foreach (var item in entity)
            {
                if (item.isEdit == true)
                {
                    context.PhoenixEdit(item);
                }
                else
                {
                    item.Id = Guid.NewGuid().ToString();
                    await context.PhoenixAddAsync(item);
                }
            }
            return await context.SaveChangesAsync();
        }
        #endregion

        #region Setting Journal AP
        public async Task<dynamic> GetMasterAP()
        {
            var result = (await (from a in context.MapCoaTransaksi
                                 join b in context.MainServiceCategorys on a.MainserviceCategoryId equals b.Id
                                 where !(a.IsDeleted ?? false) && a.ModuleCode == "AP"
                                 group a by new { a.MainserviceCategoryId, b.Name, a.NameSetting, a.PceTypeCrossBilling } into x
                                 select new
                                 {
                                     x.Key.MainserviceCategoryId,
                                     MainserviceCategoryName = x.Key.Name,
                                     x.Key.NameSetting,
                                     x.Key.PceTypeCrossBilling
                                 }).ToListAsync())
                          .Select(x => new
                          {
                              x.MainserviceCategoryId,
                              x.MainserviceCategoryName,
                              x.NameSetting,
                              PceTypeCrossBilling = (x.PceTypeCrossBilling ?? false),
                          }).ToList();
            return result;
        }

        public async Task<dynamic> GetMasterAPById(string mainServiceCategoryId, bool pceTypeCrossBilling)
        {
            var result = await (from a in context.MapCoaTransaksi
                                where !(a.IsDeleted ?? false) && a.ModuleCode == "AP" &&
                                a.MainserviceCategoryId == mainServiceCategoryId &&
                                (a.PceTypeCrossBilling ?? false) == pceTypeCrossBilling
                                group a by new { a.MainserviceCategoryId, a.NameSetting, a.PceTypeCrossBilling } into x
                                select new
                                {
                                    x.Key.MainserviceCategoryId,
                                    x.Key.NameSetting,
                                    x.Key.PceTypeCrossBilling
                                }).FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<MapCoaTransaksi>> GetMasterDetailAP(string mainServiceCategoryId, bool pceTypeCrossBilling, bool isEdit)
        {
            if (isEdit)
            {
                return await (from a in context.MapCoaTransaksi
                              where !(a.IsDeleted ?? false) &&
                                       a.MainserviceCategoryId == mainServiceCategoryId &&
                                       (a.PceTypeCrossBilling ?? false) == pceTypeCrossBilling &&
                                       a.ModuleCode == "AP"
                              select new MapCoaTransaksi
                              {
                                  Id = a.Id,
                                  ModuleCode = a.ModuleCode,
                                  MainserviceCategoryId = a.MainserviceCategoryId,
                                  LineCode = a.LineCode,
                                  Description = a.Description,
                                  CoaId = a.CoaId,
                                  DK = a.DK,
                                  IsAsf = a.IsAsf,
                                  AccountName = context.ChartOfAccounts.Where(x => x.Id == a.CoaId && x.Level == "5").Select(x => x.Name5).FirstOrDefault(),
                                  CodeRec = context.ChartOfAccounts.Where(x => x.Id == a.CoaId && x.Level == "5").Select(x => x.CodeRec).FirstOrDefault(),
                                  CreatedBy = a.CreatedBy,
                                  CreatedOn = a.CreatedOn
                              }).ToListAsync();
            }
            else
            {
                if (mainServiceCategoryId == MainServiceCategoryId.Others)
                {
                    return new List<MapCoaTransaksi>()
                    {
                        new MapCoaTransaksi(){Id=Guid.NewGuid().ToString(), ModuleCode= "AP",LineCode="VAT", Description="Tax Payable - VAT" },
                        new MapCoaTransaksi(){Id=Guid.NewGuid().ToString(), ModuleCode= "AP",LineCode="AP", Description="Account Payable - Others" }
                    };
                }
                else if (mainServiceCategoryId != "0")
                {
                    var mainservice = await context.MainServiceCategorys.AsNoTracking().Where(x => x.Id == mainServiceCategoryId).Select(x => x.Name).FirstOrDefaultAsync() ?? "";
                    return new List<MapCoaTransaksi>()
                    {
                        new MapCoaTransaksi(){Id=Guid.NewGuid().ToString(), ModuleCode= "AP",LineCode="VAT", Description="Tax Payable - VAT" },
                        new MapCoaTransaksi(){Id=Guid.NewGuid().ToString(), ModuleCode= "AP",LineCode="WIP", Description= $"Work in Progress - {mainservice}" },
                        new MapCoaTransaksi(){Id=Guid.NewGuid().ToString(), ModuleCode= "AP",LineCode="AP", Description=mainServiceCategoryId == MainServiceCategoryId.Production? "Account Payable Trade" : $"Account Payable - {mainservice}" }
                    };
                }
                else
                {
                    return new List<MapCoaTransaksi>();
                }
            }
        }

        public async Task<int> DeleteSoftAPAsync(string mainServiceCategoryId, bool pceTypeCrossBilling)
        {
            var result = await context.MapCoaTransaksi.AsNoTracking().Where(x => x.MainserviceCategoryId == mainServiceCategoryId && (x.PceTypeCrossBilling ?? false) == pceTypeCrossBilling).ToListAsync();
            context.PhoenixDeleteRange(result.ToArray());
            return await context.SaveChangesAsync();
        }

        public async Task<int> AddAPAsync(List<MapCoaTransaksi> entity)
        {
            var single = entity.FirstOrDefault();
            if (single != null)
            {
                if (single.isEdit == false)
                {
                    var mapCoa = await context.MapCoaTransaksi.AsNoTracking().Where(x => (x.IsDeleted ?? false) && x.ModuleCode == single.ModuleCode && x.MainserviceCategoryId == single.MainserviceCategoryId && x.PceTypeCrossBilling == single.PceTypeCrossBilling).ToListAsync();
                    if (mapCoa.Count > 0)
                    {
                        context.RemoveRange(mapCoa.ToArray());
                    }
                }
            }

            foreach (var item in entity)
            {
                if (item.isEdit == true)
                {
                    context.PhoenixEdit(item);
                }
                else
                {
                    item.Id = Guid.NewGuid().ToString();
                    await context.PhoenixAddAsync(item);
                }
            }
            return await context.SaveChangesAsync();
        }
        #endregion

        #region Setting Journal AR
        public async Task<dynamic> GetMasterAR()
        {
            var result = (await (from a in context.MapCoaTransaksi
                                 join b in context.MainServiceCategorys on a.MainserviceCategoryId equals b.Id
                                 where !(a.IsDeleted ?? false) && a.ModuleCode == "AR"
                                 group a by new { a.MainserviceCategoryId, b.Name, a.NameSetting, a.PceTypeCrossBilling, a.InvoiceType } into x
                                 select new
                                 {
                                     x.Key.MainserviceCategoryId,
                                     MainserviceCategoryName = x.Key.Name,
                                     x.Key.NameSetting,
                                     x.Key.PceTypeCrossBilling,
                                     x.Key.InvoiceType
                                 }).ToListAsync())
                          .Select(x => new
                          {
                              x.MainserviceCategoryId,
                              x.MainserviceCategoryName,
                              x.NameSetting,
                              PceTypeCrossBilling = (x.PceTypeCrossBilling ?? false),
                              x.InvoiceType
                          }).ToList(); ;

            return result;
        }

        public async Task<dynamic> GetMasterARById(string mainServiceCategoryId, bool pceTypeCrossBilling, string invoiceType)
        {
            var result = await (from a in context.MapCoaTransaksi
                                where !(a.IsDeleted ?? false) && a.ModuleCode == "AR" &&
                                       (a.PceTypeCrossBilling ?? false) == pceTypeCrossBilling &&
                                       a.InvoiceType == invoiceType &&
                                      a.MainserviceCategoryId == mainServiceCategoryId
                                group a by new { a.MainserviceCategoryId, a.NameSetting, a.PceTypeCrossBilling, a.InvoiceType } into x
                                select new
                                {
                                    x.Key.MainserviceCategoryId,
                                    x.Key.NameSetting,
                                    x.Key.PceTypeCrossBilling,
                                    x.Key.InvoiceType
                                }).FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<MapCoaTransaksi>> GetMasterDetailAR(string mainServiceCategoryId, bool pceTypeCrossBilling, string invoiceType, bool isEdit)
        {
            if (isEdit)
            {
                return await (from a in context.MapCoaTransaksi
                              where !(a.IsDeleted ?? false) &&
                                       a.MainserviceCategoryId == mainServiceCategoryId &&
                                       (a.PceTypeCrossBilling ?? false) == pceTypeCrossBilling &&
                                       a.InvoiceType == invoiceType &&
                                       a.ModuleCode == "AR"
                              select new MapCoaTransaksi
                              {
                                  Id = a.Id,
                                  ModuleCode = a.ModuleCode,
                                  MainserviceCategoryId = a.MainserviceCategoryId,
                                  LineCode = a.LineCode,
                                  Description = a.Description,
                                  CoaId = a.CoaId,
                                  DK = a.DK,
                                  IsAsf = a.IsAsf,
                                  AccountName = context.ChartOfAccounts.Where(x => x.Id == a.CoaId && x.Level == "5").Select(x => x.Name5).FirstOrDefault(),
                                  CodeRec = context.ChartOfAccounts.Where(x => x.Id == a.CoaId && x.Level == "5").Select(x => x.CodeRec).FirstOrDefault(),
                                  CreatedBy = a.CreatedBy,
                                  CreatedOn = a.CreatedOn
                              }).ToListAsync();
            }
            else
            {
                if (mainServiceCategoryId == MainServiceCategoryId.Media)
                {
                    return new List<MapCoaTransaksi>()
                    {
                        new MapCoaTransaksi(){Id=Guid.NewGuid().ToString(), ModuleCode= "AR",LineCode="COB", Description="COB Media TV" },
                        new MapCoaTransaksi(){Id=Guid.NewGuid().ToString(), ModuleCode= "AR",LineCode="BILLING", Description="Media Billing TV" },
                        new MapCoaTransaksi(){Id=Guid.NewGuid().ToString(), ModuleCode= "AR",LineCode="VAT", Description="Tax Payable VAT" },
                        new MapCoaTransaksi(){Id=Guid.NewGuid().ToString(), ModuleCode= "AR",LineCode="AR", Description="Account Receivable Trade" },
                        new MapCoaTransaksi(){Id=Guid.NewGuid().ToString(), ModuleCode= "AR",LineCode="WIP", Description="Work in Progress Media" }
                    };
                }
                else if (mainServiceCategoryId != "0")
                {
                    var mainservice = await context.MainServiceCategorys.AsNoTracking().Where(x => x.Id == mainServiceCategoryId).Select(x => x.Name).FirstOrDefaultAsync() ?? "";
                    //(mainServiceCategoryId == MainServiceCategoryId.Production || mainServiceCategoryId == MainServiceCategoryId.Others
                    return new List<MapCoaTransaksi>()
                    {
                        new MapCoaTransaksi(){Id=Guid.NewGuid().ToString(), ModuleCode= "AR",LineCode="BILLING", Description="Total" },//Description= mainServiceCategoryId == MainServiceCategoryId.Others?"Prepaid Others" : $"Client Prebilling {mainservice}" },
                        new MapCoaTransaksi(){Id=Guid.NewGuid().ToString(), ModuleCode= "AR",LineCode="VAT", Description="VAT" }, //Description="Tax Payable VAT" },
                        new MapCoaTransaksi(){Id=Guid.NewGuid().ToString(), ModuleCode= "AR",LineCode="AR", Description="Grand total" } //Description="Account Receivable Trade" }
                    };
                }
                else
                {
                    return new List<MapCoaTransaksi>();
                }
            }
        }

        public async Task<int> DeleteSoftARAsync(string mainServiceCategoryId, bool pceTypeCrossBilling, string invoiceType)
        {
            var result = await context.MapCoaTransaksi.AsNoTracking().Where(x => x.MainserviceCategoryId == mainServiceCategoryId && (x.PceTypeCrossBilling ?? false) == pceTypeCrossBilling && x.InvoiceType == invoiceType).ToListAsync();
            context.PhoenixDeleteRange(result.ToArray());
            return await context.SaveChangesAsync();
        }

        public async Task<int> AddARAsync(List<MapCoaTransaksi> entity)
        {
            var single = entity.FirstOrDefault();
            if (single != null)
            {
                if (single.isEdit == false)
                {
                    var mapCoa = await context.MapCoaTransaksi.AsNoTracking().Where(x => (x.IsDeleted ?? false) && x.ModuleCode == single.ModuleCode && x.MainserviceCategoryId == single.MainserviceCategoryId && x.PceTypeCrossBilling == single.PceTypeCrossBilling && x.InvoiceType == single.InvoiceType).ToListAsync();
                    if (mapCoa.Count > 0)
                    {
                        context.RemoveRange(mapCoa.ToArray());
                    }
                }
            }

            foreach (var item in entity)
            {
                if (item.isEdit == true)
                {
                    context.PhoenixEdit(item);
                }
                else
                {
                    item.Id = Guid.NewGuid().ToString();
                    await context.PhoenixAddAsync(item);
                }
            }
            return await context.SaveChangesAsync();
        }
        #endregion

        #region Setting Journal Petty Cash
        public async Task<List<MapCoaTransaksi>> GetJournalPettyCash()
        {
            var result = await (from a in context.MapCoaTransaksi
                                where !(a.IsDeleted ?? false) &&
                                         a.ModuleCode == "PC"
                                select new MapCoaTransaksi
                                {
                                    Id = a.Id,
                                    ModuleCode = a.ModuleCode,
                                    MainserviceCategoryId = a.MainserviceCategoryId,
                                    LineCode = a.LineCode,
                                    Description = a.Description,
                                    CoaId = a.CoaId,
                                    DK = a.DK,
                                    IsAsf = a.IsAsf,
                                    AccountName = context.ChartOfAccounts.Where(x => x.Id == a.CoaId && x.Level == "5").Select(x => x.Name5).FirstOrDefault(),
                                    CodeRec = context.ChartOfAccounts.Where(x => x.Id == a.CoaId && x.Level == "5").Select(x => x.CodeRec).FirstOrDefault(),
                                    CreatedBy = a.CreatedBy,
                                    CreatedOn = a.CreatedOn
                                }).ToListAsync();

            if (result.Count == 0)
            {
                result = new List<MapCoaTransaksi>()
                    {
                        new MapCoaTransaksi(){Id=Guid.NewGuid().ToString(), ModuleCode= "PC",LineCode="AP", Description= "Account Payable Trade", MainserviceCategoryId=MainServiceCategoryId.Others }
                    };
            }
            return result;
        }
        public async Task<int> AddPettyCashAsync(List<MapCoaTransaksi> entity)
        {
            var isEdit = context.MapCoaTransaksi.Where(x => !(x.IsDeleted ?? false) && entity.Any(z => z.ModuleCode == x.ModuleCode && z.MainserviceCategoryId == x.MainserviceCategoryId && z.LineCode == x.LineCode)).Count() > 0;
            foreach (var item in entity)
            {
                if (isEdit)
                {
                    context.PhoenixEdit(item);
                }
                else
                {
                    item.Id = Guid.NewGuid().ToString();
                    await context.PhoenixAddAsync(item);
                }
            }
            return await context.SaveChangesAsync();
        }
        #endregion

        #region Setting Journal Salary Payment Request
        public async Task<List<MapCoaTransaksi>> GetJournalSalaryPaymentRequest()
        {
            var result = await (from a in context.MapCoaTransaksi
                                where !(a.IsDeleted ?? false) &&
                                         a.ModuleCode == "SALARY PAYMENT REQUEST"
                                select new MapCoaTransaksi
                                {
                                    Id = a.Id,
                                    ModuleCode = a.ModuleCode,
                                    MainserviceCategoryId = a.MainserviceCategoryId,
                                    LineCode = a.LineCode,
                                    Description = a.Description,
                                    CoaId = a.CoaId,
                                    DK = a.DK,
                                    IsAsf = a.IsAsf,
                                    AccountName = context.ChartOfAccounts.Where(x => x.Id == a.CoaId && x.Level == "5").Select(x => x.Name5).FirstOrDefault(),
                                    CodeRec = context.ChartOfAccounts.Where(x => x.Id == a.CoaId && x.Level == "5").Select(x => x.CodeRec).FirstOrDefault(),
                                    CreatedBy = a.CreatedBy,
                                    CreatedOn = a.CreatedOn
                                }).ToListAsync();

            if (result.Count == 0)
            {
                result = new List<MapCoaTransaksi>()
                    {
                        new MapCoaTransaksi(){Id=Guid.NewGuid().ToString(), ModuleCode= "SALARY PAYMENT REQUEST",LineCode="AP", Description= "Account Payable Trade", MainserviceCategoryId=MainServiceCategoryId.Others },
                        new MapCoaTransaksi(){Id=Guid.NewGuid().ToString(), ModuleCode= "SALARY PAYMENT REQUEST",LineCode="SALARY", Description= "Salary", MainserviceCategoryId=MainServiceCategoryId.Others }
                    };
            }
            return result;
        }
        public async Task<int> AddSalaryPaymentRequestAsync(List<MapCoaTransaksi> entity)
        {
            var isEdit = context.MapCoaTransaksi.Where(x => !(x.IsDeleted ?? false) && entity.Any(z => z.ModuleCode == x.ModuleCode && z.MainserviceCategoryId == x.MainserviceCategoryId && z.LineCode == x.LineCode)).Count() > 0;
            foreach (var item in entity)
            {
                if (isEdit)
                {
                    context.PhoenixEdit(item);
                }
                else
                {
                    item.Id = Guid.NewGuid().ToString();
                    await context.PhoenixAddAsync(item);
                }
            }
            return await context.SaveChangesAsync();
        }
        #endregion
    }
}