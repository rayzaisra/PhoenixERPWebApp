using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Finance
{
    public class OrderReceiptDetail : FinanceEntityBase
    {
        public string Id { get; set; }
        public string ReceiptOrderNumber { get; set; }
        public string GoodRequestNumber { get; set; }
    }
}
