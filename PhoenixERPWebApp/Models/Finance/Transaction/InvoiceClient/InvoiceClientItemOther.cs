using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models
{
    public class InvoiceClientItemOther
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Parent Id")]
        public string ParentId { get; set; }

        [DisplayName("Invoice Id")]
        public string InvoiceId { get; set; }

        //[DisplayName("Description")]
        //public string Description { get; set; }

        //[DisplayName("Remark")]
        //public string Remark { get; set; }

        [DisplayName("Total")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal Amount { get; set; }

        [DisplayName("Sequence")]
        public int? Sequence { get; set; }

        [DisplayName("Account Id")]
        public string AccountID { get; set; }

        public string PceId { get; set; }
        public string JobId { get; set; }

        public string TaskId { get; set; }

        [DisplayName("Bill Percent")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal AmountPercentage { get; set; }

        [DisplayName("Qty")]
        public int? Quantity { get; set; }

        [DisplayName("Task Name")]
        public string TaskName { get; set; }

        [DisplayName("Unit Price")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? UnitPrice { get; set; }

        [DisplayName("Billed")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? AmountBilled { get; set; }
        
        [DisplayName("Balance")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? Balance { get; set; }

        [DisplayName("Use ASF")]
        public bool? IsAsf { get; set; }

        public string CurrencySymbol { get; set; }
    }
}
