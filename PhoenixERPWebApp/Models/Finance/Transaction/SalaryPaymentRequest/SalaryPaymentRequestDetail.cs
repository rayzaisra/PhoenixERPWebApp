using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class SalaryPaymentRequestDetail : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Salary Payment Request Id")]
        public string SalaryPaymentRequestId { get; set; }

        [DisplayName("HRIS Division")]
        public string DivisionId { get; set; }

        [DisplayName("HRIS Division")]
        public string DivisionName { get; set; }

        [DisplayName("Amount")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Amount { get; set; }
    }
}