using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models
{
    public class InvoiceDeliveryNotes : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        public string InvoiceNumber { get; set; }
        public string ClientId { get; set; }
        public string Attention { get; set; }
        public string ReceivedBy { get; set; }

        [DisplayName("Created Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ReceivedOn { get; set; }

        public string DeliveredBy { get; set; }

        [DisplayName("Delivered Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DeliveredOn { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }

        [DisplayName("Client Name")]
        public string ClientName { get; set; }

        [DisplayName("Legal Entity")]
        public string LegalEntityId { get; set; }

        public string Remarks { get; set; }
        public string SignIn { get; set; }
        public string CreatedByName { get; set; }
        public string FileMasterId { get; set; }
        public string FileMasterName { get; set; }
        public List<AccountReceivable> listAR { get; set; }
    }
}
