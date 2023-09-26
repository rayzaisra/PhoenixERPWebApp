using System.Collections.Generic;

namespace Phoenix.Data.Models.Finance.Transaction.ServiceReceipt
{
    public class ServiceReceiptDTO
    {
        public ServiceReceipt Header { get; set; }
        public List<ServiceReceiptDetail> Details { get; set; }
    }
}
