using System;
using System.Collections.Generic;
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
    public interface IFinReportControlService
    {
        Task<int> AddEditAsync(VwFinReportControl entity);
        Task<VwFinReportControl> Get();
    }

    public class FinReportControlService : IFinReportControlService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage FinReportControl
        /// </summary>
        /// <param name="context">Database context</param>
        public FinReportControlService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddEditAsync(VwFinReportControl entity)
        {
            string CREDIT = "CREDIT";
            string DEBIT = "DEBIT";
            var list = await context.FinReportControls.AsNoTracking().ToListAsync();
            if (list.Count == 0)
            {
                await context.PhoenixAddAsync(new FinReportControl() { Id = Guid.NewGuid().ToString(), ItemCode = entity.ItemCodeCurrencyGain, CoaId = entity.CoaIdCurrencyGain, DK = CREDIT });
                await context.PhoenixAddAsync(new FinReportControl() { Id = Guid.NewGuid().ToString(), ItemCode = entity.ItemCodeCurrencyLoss, CoaId = entity.CoaIdCurrencyLoss, DK = DEBIT });
                await context.PhoenixAddAsync(new FinReportControl() { Id = Guid.NewGuid().ToString(), ItemCode = entity.ItemCodeYTD, CoaId = entity.CoaIdYTD, DK = CREDIT });
                await context.PhoenixAddAsync(new FinReportControl() { Id = Guid.NewGuid().ToString(), ItemCode = entity.ItemCodePAL, CoaId = entity.CoaIdPAL, DK = CREDIT });
                await context.PhoenixAddAsync(new FinReportControl() { Id = Guid.NewGuid().ToString(), ItemCode = entity.ItemCodeREPY, CoaId = entity.CoaIdREPY, DK = CREDIT });
                await context.PhoenixAddAsync(new FinReportControl() { Id = Guid.NewGuid().ToString(), ItemCode = entity.ItemCodeHistoricalBalance, CoaId = entity.CoaIdHistoricalBalance, DK = CREDIT });
            }
            else
            {
                foreach (var item in list)
                {
                    if (item.ItemCode == ReportControlItemCode.CurrencyLoss)
                    {
                        item.DK = item.DK ?? DEBIT;
                    }
                    else
                    {
                        item.DK = item.DK ?? CREDIT;
                    }

                    if (entity.ItemCodeYTD == item.ItemCode) item.CoaId = entity.CoaIdYTD;
                    else if (entity.ItemCodePAL == item.ItemCode) item.CoaId = entity.CoaIdPAL;
                    else if (entity.ItemCodeREPY == item.ItemCode) item.CoaId = entity.CoaIdREPY;

                    if (string.IsNullOrEmpty(item.Id))
                    {
                        item.Id = Guid.NewGuid().ToString();
                        await context.PhoenixAddAsync(item);
                    }
                    else
                    {
                        context.PhoenixEdit(item);
                    }
                }
            }
            return await context.SaveChangesAsync();
        }

        public async Task<VwFinReportControl> Get()
        {
            var models = await context.FinReportControls.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).ToListAsync();
            var result = new VwFinReportControl();

            result.ItemCodeCurrencyGain = ReportControlItemCode.CurrencyGain;
            result.ItemCodeCurrencyLoss = ReportControlItemCode.CurrencyLoss;
            result.ItemCodeYTD = ReportControlItemCode.YTD;
            result.ItemCodePAL = ReportControlItemCode.PAL;
            result.ItemCodeREPY = ReportControlItemCode.REPY;
            result.ItemCodeHistoricalBalance = ReportControlItemCode.HistoricalBalance;

            result.CoaIdCurrencyGain = models.Where(x => x.ItemCode == result.ItemCodeCurrencyGain).Select(x => x.CoaId).FirstOrDefault();
            result.CoaIdCurrencyLoss = models.Where(x => x.ItemCode == result.ItemCodeCurrencyLoss).Select(x => x.CoaId).FirstOrDefault();
            result.CoaIdYTD = models.Where(x => x.ItemCode == result.ItemCodeYTD).Select(x => x.CoaId).FirstOrDefault();
            result.CoaIdPAL = models.Where(x => x.ItemCode == result.ItemCodePAL).Select(x => x.CoaId).FirstOrDefault();
            result.CoaIdREPY = models.Where(x => x.ItemCode == result.ItemCodeREPY).Select(x => x.CoaId).FirstOrDefault();
            result.CoaIdHistoricalBalance = models.Where(x => x.ItemCode == result.ItemCodeHistoricalBalance).Select(x => x.CoaId).FirstOrDefault();
            return result;
        }
    }
}