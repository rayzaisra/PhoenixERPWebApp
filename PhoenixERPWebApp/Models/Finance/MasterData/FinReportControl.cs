using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class FinReportControl : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Item Code")]
        public string ItemCode { get; set; }

        [DisplayName("Coa Id")]
        public string CoaId { get; set; }

        [DisplayName("D K")]
        public string DK { get; set; }
    }

    public class VwFinReportControl
    {
        public string ItemCodeCurrencyGain { get; set; }
        public string CoaIdCurrencyGain { get; set; }
        public string ItemCodeCurrencyLoss { get; set; }
        public string CoaIdCurrencyLoss { get; set; }
        public string ItemCodeYTD { get; set; }
        public string CoaIdYTD { get; set; }
        public string ItemCodePAL { get; set; }
        public string CoaIdPAL { get; set; }
        public string ItemCodeREPY { get; set; }
        public string CoaIdREPY { get; set; }
        public string ItemCodeHistoricalBalance { get; set; }
        public string CoaIdHistoricalBalance { get; set; }
    }
}