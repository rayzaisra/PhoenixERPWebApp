using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models
{
    public class Reconcile : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Period { get; set; }

        [DisplayName("Reference Id")]
        public string ReferenceId { get; set; }

        [DisplayName("Notes")]
        public string Notes { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }
    }

    public class ReconcileList
    {
        public string Id { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PaymentRequestDate { get; set; }
        public string PaymentRequestNumber { get; set; }
        public string ReferenceNumberId { get; set; }
        public string LegalEntityName { get; set; }
        public string VendorName { get; set; }
        public string JobName { get; set; }
        public string BusinessUnitName { get; set; }
        public string PaymentTypeName { get; set; }
        public string CurrencyName { get; set; }
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? AmountPay { get; set; }
        public string Status { get; set; }
        public string ReconcileId { get; set; }

        [DisplayName("Bank Account")]
        public string AccountName { get; set; }

        [DisplayName("Account No")]
        public string AccountNo { get; set; }

        [DisplayName("Date")]
        public DateTime? Period { get; set; }

        [DisplayName("Notes")]
        public string Notes { get; set; }
    }

    public class ReconcileSearch
    {
        [DisplayName("Legal Entity")]
        public string LegalEntityId { get; set; }

        //[DisplayName("Periode From")]
        //[DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        //public DateTime? PeriodeFrom { get; set; }

        //[DisplayName("Periode To")]
        //[DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        //public DateTime? PeriodeTo { get; set; }

        [DisplayName("Type")]
        public string Type { get; set; }

        [DisplayName("Reference No")]
        public string ReferenceNo { get; set; }

        //[DisplayName("Business Unit")]
        //public string BusinessUnitId { get; set; }

        //[DisplayName("COA Level")]
        //public int Level { get; set; }

        [DisplayName("Bank Account")]
        public string AccountId { get; set; }
    }
}
