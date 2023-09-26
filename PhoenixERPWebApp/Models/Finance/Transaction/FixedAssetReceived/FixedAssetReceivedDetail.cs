using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Finance.Transaction.FixedAssetReceived
{
    public class FixedAssetReceivedDetail : FinanceEntityBase
    {
        public string Id { get; set; }

        public string FixedAssetReceivedId { get; set; }

        public string FixedAssetId { get;set ;}

        public string AccountId { get; set; }

        public string SkuCode { get; set; }

        public string SerialNumber { get; set; }

        public string ItemCategoryId { get; set; }

        public string ItemCategoryName { get; set; }

        public string UomId { get; set; }

        public string UnitName { get; set; }

        public string Brand { get; set; }

        public string Subbrand { get; set; }

        public string Description { get; set; }

        public decimal? Qty { get; set; }

        public string Remark { get; set; }

        public IFormFile FileAttachment { get; set; }
    }
}
