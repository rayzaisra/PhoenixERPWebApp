using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Phoenix.Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Data
{
    public static class TransID
    {
        public static async Task<string> GetTransId(DataContext context, string code, DateTime reqDate)
        {
            TransactionId model = null;
            reqDate = Convert.ToDateTime($"{reqDate.Year.ToString()}/{reqDate.Month.ToString()}/01");
            var result = await context.TransactionIds.AsNoTracking().Where(x => x.TransId == code && x.RequestDate.Date == reqDate.Date).FirstOrDefaultAsync();
            if (result == null)
            {
                model = new TransactionId() { TransId = code, RequestDate = reqDate, Sequence = 1 };
                context.TransactionIds.Add(model);
            }
            else
            {
                model = new TransactionId() { TransId = result.TransId, RequestDate = result.RequestDate, Sequence = result.Sequence + 1 };
                bool tracking = context.ChangeTracker.Entries<TransactionId>().Any(x => x.Entity.TransId == code && x.Entity.RequestDate == reqDate.Date);
                if (!tracking)
                {
                    context.TransactionIds.Update(model);
                }
                //context.TransactionIds.Update(model);
            }
            await context.SaveChangesAsync();
            context.Entry<TransactionId>(model).State = EntityState.Detached;
            
            int seq = model.Sequence == 1 ? model.Sequence : model.Sequence + 1;
            return $"{model.TransId}{model.RequestDate.ToString("yyyyMM")}{model.Sequence.ToString().PadLeft(5, '0')}";
        }


        public static async Task<string> GetTransIdMediaPlanTV(DataContext context, string code, DateTime reqDate, string lastCode)
        {
            var result = await context.TransactionIds.AsNoTracking().Where(x => x.TransId == code).FirstOrDefaultAsync();

            if (result == null)
            {
                result = new TransactionId() { TransId = code, RequestDate = reqDate, Sequence = 1 };
                context.TransactionIds.Add(result);
            }
            else
            {
                if (result.Sequence == 99999)
                {
                    result.Sequence = 1;
                }
                else
                {
                    result.Sequence += 1;
                }
                context.TransactionIds.Update(result);
            }

            await context.SaveChangesAsync();
            int seq = result.Sequence == 1 ? result.Sequence : result.Sequence + 1;
            return $"{result.Sequence.ToString("00000")}/OPT/MEDIA/{lastCode}";
        }
    }
}
