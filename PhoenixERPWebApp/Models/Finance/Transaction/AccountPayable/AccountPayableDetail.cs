using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class AccountPayableDetail : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Account Payable Id")]
        public string AccountPayableId { get; set; }

        [DisplayName("Evident Number")]
        public string EvidentNumber { get; set; }

        [DisplayName("Payable Balance")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? PayableBalance { get; set; }

        [DisplayName("Amount Paid")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? AmountPaid { get; set; }

        [DisplayName("Discount")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Discount { get; set; }

        [DisplayName("Tax")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Tax { get; set; }

        [DisplayName("Ending Balance")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? EndingBalance { get; set; }

        [DisplayName("Row Index")]
        public int? RowIndex { get; set; }

        [DisplayName("Item Name")]
        public string ItemName { get; set; }

        [DisplayName("Item Description")]
        public string ItemDescription { get; set; }

        [DisplayName("Qty")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Qty { get; set; }

        [DisplayName("Unit Price")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal UnitPrice { get; set; }

        [DisplayName("Amount")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Amount { get; set; }

        [DisplayName("Uom Id")]
        public string UomId { get; set; }

        [DisplayName("Item Type Id")]
        public string ItemTypeId { get; set; }

        [DisplayName("D K")]
        public string DK { get; set; }

        [DisplayName("Account Id")]
        public string AccountId { get; set; }

        [DisplayName("Line Code")]
        public string LineCode { get; set; }

        [DisplayName("Tax Pay Coa Id")]
        public string TaxPayCoaId { get; set; }

        [DisplayName("Tax Payable Pct")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? TaxPayablePct { get; set; }

        public AccountPayable AccountPayable { get; set; }
    }
}