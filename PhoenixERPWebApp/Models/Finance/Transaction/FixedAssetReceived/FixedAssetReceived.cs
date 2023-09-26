using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Finance.Transaction.FixedAssetReceived
{
    public class FixedAssetReceived : FinanceEntityBase
    {
        public string Id { get; set; }

        public string FarNumber { get; set; }

        public DateTime? FarDatetime { get; set; }

        public string VendorId { get; set; }

        public string VendorName { get; set; }

        public string PurchaseOrderId { get; set; }

        public string ReceivedBy { get; set; }

        public DateTime? ReceivedOn { get; set; }

        public string LocationId { get; set; }

        public string BusinessUnitId { get; set; }

        public string LegalEntityId { get; set; }

        public string AffiliationId { get; set; }

        public string Remark { get; set; }

        public string ApprovalId { get; set; }

    }
}
