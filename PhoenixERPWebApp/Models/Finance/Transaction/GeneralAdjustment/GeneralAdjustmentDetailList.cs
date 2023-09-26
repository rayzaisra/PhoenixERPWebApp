using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Finance.Transaction.GeneralAdjustment
{
    public class GeneralAdjustmentDetailList : FinanceEntityBase
    {
        public string Id { get; set; }
        public string ACCNo { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public string Division { get; set; }
        public string JobId { get; set; }
    }
}
