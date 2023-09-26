using System.Collections.Generic;

namespace PhoenixERPWebApp.Models.Finance.Transaction.ServiceReceipt
{
    public class ServiceReceiptDTO
    {
        public ServiceReceipt Header { get; set; }
        public List<ServiceReceiptDetail> Details { get; set; }
    }
}
