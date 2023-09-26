using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class SalaryPaymentRequest : FinanceEntityBase
    {
        [DisplayName("Salary Payment No")]
        public string Id { get; set; }

        [DisplayName("Legal Entity")]
        public string LegalEntityId { get; set; }

        [DisplayName("Payroll ID")]
        public string PayrollId { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Month & Year")]
        [DisplayFormat(DataFormatString = "{0:MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? SalaryPaymentDate { get; set; }

        [DisplayName("Transaction Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? TransactionDate { get; set; }

        [DisplayName("Currency")]
        public string CurrencyId { get; set; }

        [DisplayName("Exchange Rate")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? ExchangeRate { get; set; }

        [DisplayName("Total Amount")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? TotalAmount { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }

        //For List 
        [DisplayName("Legal Entity")]
        public string LegalEntityName { get; set; }

        [DisplayName("Request By")]
        public string RequestBy { get; set; }

        [DisplayName("Date Request")]
        public string RequestDate { get; set; }

        //For Approval  
        [DisplayName("Remarks")]
        public string LogDescription { get; set; }

        public List<SalaryPaymentRequestDetail> Details { get; set; }
    }
}