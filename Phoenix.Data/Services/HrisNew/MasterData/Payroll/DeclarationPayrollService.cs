using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models;
using Phoenix.Data.Models.HrisNew.Global;
using Phoenix.Data.RestApi;
using Phoenix.Shared.Core.Contexts;
using Phoenix.Shared.Libraries.SnowFlake;

namespace Phoenix.Data
{
    public interface IDeclarationPayrollService : IDataService<DeclarationPayrollHeader>
    {
        Task<List<DeclarationPayrollDetail>> getDeclarationDetail(string month, string year, string componentId);
        Task<int> AddCustomAsync(DeclarationPayrollDto model);
        Task<int> EditCustomAsync(DeclarationPayrollDto model);
        Task<List<DeclarationPayrollHeader>> GetApprovalList();
    }

    public class DeclarationPayrollService : IDeclarationPayrollService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage AllowanceComponent
        /// </summary>
        /// <param name="context">Database context</param>
        public DeclarationPayrollService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(DeclarationPayrollHeader entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> AddCustomAsync(DeclarationPayrollDto model)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var code = await TransID.GetTransId(context, "DPR", DateTime.Today);
                    var HeaderMonthIsExist = await context.DeclarationPayrollHeader.AsNoTracking().Where(x => x.Month == Convert.ToInt32(model.Header.Month) && x.Year == Convert.ToInt32(model.Header.Year) && x.SubComponentId == model.Header.SubComponentId).FirstOrDefaultAsync();

                    if (HeaderMonthIsExist != null)
                    {
                        context.DeclarationPayrollHeader.Remove(HeaderMonthIsExist);
                        var detailHeaderMonth = await context.DeclarationPayrollDetail.AsNoTracking().Where(x => x.DeclarationPayrollHeaderId == HeaderMonthIsExist.Code).ToListAsync();

                        if (detailHeaderMonth.Count > 0)
                        {
                            context.DeclarationPayrollDetail.RemoveRange(detailHeaderMonth);
                        }
                        
                    }

                    DeclarationPayrollHeader dataHeader = new DeclarationPayrollHeader();
                    dataHeader.Id = code;
                    dataHeader.Code = code;
                    dataHeader.Month = Convert.ToInt32(model.Header.Month);
                    dataHeader.Year = Convert.ToInt32(model.Header.Year);
                    dataHeader.IsTmp = true;
                    dataHeader.DeclarationType = model.Header.DeclarationType;
                    dataHeader.SubComponentId = model.Header.SubComponentId;
                    dataHeader.Description = model.Header.Description;
                    dataHeader.Status = model.Header.Status;
                    await context.PhoenixAddAsync(dataHeader);

                    if (model.Details.Count > 0)
                    {
                        var detail = await context.DeclarationPayrollDetail.AsNoTracking().Where(x => x.DeclarationPayrollHeaderId == code).ToListAsync();

                        if (detail.Count > 0)
                        {
                            context.DeclarationPayrollDetail.RemoveRange(detail);
                        }
                        var snowflake = new IdWorker();
                        Parallel.ForEach(model.Details, (item) =>
                        {
                            
                            item.Id = snowflake.NextId().ToString();
                            item.DeclarationPayrollHeaderId = code;
                            item.Status = model.Header.Status;
                            context.PhoenixAddAsync(item);
                        });
                    }

                    var save = await context.SaveChangesAsync();
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

        public Task<int> AddRangeAsync(params DeclarationPayrollHeader[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(DeclarationPayrollHeader entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params DeclarationPayrollHeader[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(DeclarationPayrollHeader entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params DeclarationPayrollHeader[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(DeclarationPayrollHeader entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> EditCustomAsync(DeclarationPayrollDto model)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var HeaderMonthIsExist = await context.DeclarationPayrollHeader.AsNoTracking().Where(x => x.Id == model.Header.Id).FirstOrDefaultAsync();


                    HeaderMonthIsExist.Month = Convert.ToInt32(model.Header.Month);
                    HeaderMonthIsExist.Year = Convert.ToInt32(model.Header.Year);
                    HeaderMonthIsExist.IsTmp = true;
                    HeaderMonthIsExist.DeclarationType = model.Header.DeclarationType;
                    HeaderMonthIsExist.SubComponentId = model.Header.SubComponentId;
                    HeaderMonthIsExist.Description = model.Header.Description;
                    HeaderMonthIsExist.Status = model.Header.Status;
                    context.PhoenixEdit(HeaderMonthIsExist);

                    if (model.Details.Count > 0)
                    {
                        var detail = await context.DeclarationPayrollDetail.AsNoTracking().Where(x => x.DeclarationPayrollHeaderId == model.Header.Id).ToListAsync();

                        if (detail.Count > 0)
                        {
                            context.DeclarationPayrollDetail.RemoveRange(detail);
                        }
                        var snowflake = new IdWorker();
                        Parallel.ForEach(model.Details, (item) =>
                        {

                            item.Id = snowflake.NextId().ToString();
                            item.DeclarationPayrollHeaderId = model.Header.Id;
                            item.Status = model.Header.Status;
                            context.PhoenixAddAsync(item);
                        });
                    }

                    var save = await context.SaveChangesAsync();
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

        public Task<int> EditRangeAsync(DeclarationPayrollHeader entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<DeclarationPayrollHeader>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<DeclarationPayrollHeader> Get(string Id) => await context.DeclarationPayrollHeader.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();

        public async Task<List<DeclarationPayrollHeader>> Get()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var bpjsAdd = await context.BpjsComponents.Where(x => !(x.IsDeleted ?? false) && x.Type == 1).Select(x => new SelectListItem() { Text = x.SubComponentName, Value = x.Id }).ToListAsync();
            var allowance = await context.AllowanceComponents.Where(x => !(x.IsDeleted ?? false) && x.IsDefault == true).Select(x => new SelectListItem() { Text = x.SubComponentName, Value = x.Id }).ToListAsync();
            var bpjsDed = await context.BpjsComponents.Where(x => !(x.IsDeleted ?? false) && x.Type == 2).Select(x => new SelectListItem() { Text = x.SubComponentName, Value = x.Id }).ToListAsync();
            var deduction = await context.DeductionComponents.Where(x => !(x.IsDeleted ?? false) && x.IsDefault == true).Select(x => new SelectListItem() { Text = x.SubComponentName, Value = x.Id }).ToListAsync();
            list.AddRange(bpjsAdd);
            list.AddRange(allowance);
            list.AddRange(bpjsDed);
            list.AddRange(deduction);

            var query = await (from q in context.DeclarationPayrollHeader
                               where q.IsDeleted == false
                               select new DeclarationPayrollHeader()
                               {
                                   Id = q.Id,
                                   Code = q.Code,
                                   Month = q.Month,
                                   Year = q.Year,
                                   Description = q.Description,
                                   Status = q.Status,
                                   SubComponentId = q.SubComponentId,
                                   DeclarationType = q.DeclarationType,
                                   IsTmp = q.IsTmp,
                                   SubComponentName = list.Where(x => x.Value == q.SubComponentId).Select(x => x.Text).FirstOrDefault(),
                                   MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(q.Month ?? 1)
                               }).ToListAsync();

            //var query = await context.DeclarationPayrollHeader.Where(x => x.IsDeleted == false).ToListAsync();
            return query;
        }

        public async Task<List<DeclarationPayrollHeader>> GetApprovalList() {
            var query = await Get();
            var queryApproval = query.Where(x => x.Status == 2).ToList();

            return queryApproval;

        }

        public async Task<List<DeclarationPayrollDetail>> getDeclarationDetail(string month, string year, string componentId)
        {
            List<DeclarationPayrollDetail> result = new List<DeclarationPayrollDetail>();

            var isExists = await context.DeclarationPayrollHeader.Where(x => x.IsDeleted == false && x.Month == Convert.ToInt32(month) && x.Year == Convert.ToInt32(year) && x.SubComponentId == componentId).FirstOrDefaultAsync();
            if (isExists != null)
            {
                var query = await (from q in context.DeclarationPayrollDetail
                                   join r in context.EmployeeBasicInfos on q.EmployeeBasicInfoId equals r.Id
                                   join s in context.LegalEntity on r.LegalEntityId equals s.Id into lqs
                                   from qs in lqs.DefaultIfEmpty()
                                   where q.IsDeleted == false && q.DeclarationPayrollHeaderId == isExists.Id
                                   select new DeclarationPayrollDetail()
                                   {
                                       EmployeeBasicInfoId = q.EmployeeBasicInfoId,
                                       EmployeeName = r.NameEmployee,
                                       Amount = q.Amount,
                                       DeclarationPayrollHeaderId = isExists.Id,
                                       Remarks = q.Remarks,
                                       Id = q.Id,
                                       LegalEntityId = r.LegalEntityId,
                                       LegalEntityName = qs.LegalEntityName
                                   }).ToListAsync();
                result = query;
            }
           
            return result;
        }
    }
}