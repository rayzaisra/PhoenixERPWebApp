using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Phoenix.WebExtension.Extentions;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models;
using PhoenixERPWebApp.Models.Finance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoenixERPWebApp
{
    public static class GlobalCache
    {
        private static string uniquePCA = "PCA";
        private static string uniqueCAList = "CAList";
        public static Pca GetPCA()
        {
            return CacheStore.Get<Pca>(uniquePCA);
        }

        public static List<CashAdvanceDetail> GetCashAdvanceList()
        {
            return CacheStore.Get<List<CashAdvanceDetail>>(uniqueCAList);
        }

        public static void SetPCA(Pca model)
        {
            ClearPCA();
            ClearCashAdvanceList();
            //model.PcaTasks = model.PcaTasks.Where(x => x.IsSelect == true && x.ParentId == null && x.Status == Models.PCAStatus.Open).ToList();
            model.PcaTasks = model.PcaTasks.Where(x => x.IsSelect == true && x.ParentId == null && (x.TotalPriceBalance > 0)).ToList();

            var caDetailList = new List<CashAdvanceDetail>();
            Parallel.ForEach(model.PcaTasks, (item) =>
             {
                 var caDetailModel = new CashAdvanceDetail();
                 caDetailModel.CashAdvanceId = Guid.NewGuid().ToString();//model.Id;
                 caDetailModel.TaskId = item.Id;
                 caDetailModel.TaskName = item.TaskName;
                 caDetailModel.UomId = item.UomId;
                 caDetailModel.UomName = item.UomSymbol;
                 caDetailModel.Qty = item.Quantity;
                 caDetailModel.UnitPrice = item.Price;
                 caDetailModel.Amount = (item.Quantity ?? 0) * (item.Price ?? 0); //item.Total;
                 caDetailModel.RemainingBudget = item.TotalPriceBalance;
                 caDetailModel.CurrencySymbol = item.CurrencySymbol;
                 caDetailModel.CurrencySymbolExchange = item.CurrencySymbol;
                 caDetailList.Add(caDetailModel);
             });

            model.PcaTasks = null;
            CacheStore.Add<Pca>(uniquePCA, model);
            CacheStore.Add<List<CashAdvanceDetail>>(uniqueCAList, caDetailList);
        }

        public static void ClearPCA()
        {
            CacheStore.Remove<Pca>(uniquePCA);
        }

        public static void ClearCashAdvanceList()
        {
            CacheStore.Remove<List<CashAdvanceDetail>>(uniqueCAList);
        }
    }
}