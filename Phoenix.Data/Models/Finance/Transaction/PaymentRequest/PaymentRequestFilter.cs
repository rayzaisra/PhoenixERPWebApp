using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models
{
    public class PaymentRequestFilter
    {
        public string PaymentRequestId { get; set; }
        public string FilterInvoice { get; set; }

        public string FilterReferenceNumber { get; set; }

        public string FilterLegalEntity { get; set; }

        public DateTime? FilterStartDate { get; set; }

        public DateTime? FilterEndDate { get; set; }

        public string FilterBusinessUnitId { get; set; }

        public string FilterStatus { get; set; }

        public string FilterPaymentRequestType { get; set; }

        public string MainServiceCategory { get; set; }

        public string CurrencyId { get; set; }

        public string VendorId { get; set; }
        public bool isBankPayment { get; set; }
        public string FilterARNumber { get; set; }
        public string FilterApNum { get; set; }
    }
}
