using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    public class RptCashFlowFilter
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndingDate { get; set; }
        public string CurrencyId { get; set; }
        public string BankId { get; set; }
    }
}