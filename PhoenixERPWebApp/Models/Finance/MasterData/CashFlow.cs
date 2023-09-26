using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class CashFlow : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Type")]
        public string Type { get; set; }

        [DisplayName("Bank")]
        public string BankId { get; set; }

        [DisplayName("Currency")]
        public string CurrencyId { get; set; }

        [DisplayName("Condition")]
        public string Condition { get; set; }

        [DisplayName("Amount")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Amount { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CFDate { get; set; }

        [DisplayName("Bank")]
        public string BankName { get; set; }

        [DisplayName("Currency")]
        public string CurrencyName { get; set; }
    }
}