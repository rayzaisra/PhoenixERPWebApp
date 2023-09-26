using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    //public class RptProfitAndLoss
    //{
    //    public string account_id { get; set; }
    //    public string code_rec { get; set; }
    //    public string name1 { get; set; }
    //    public string name2 { get; set; }
    //    public string name5 { get; set; }
    //    public decimal? balance { get; set; }
    //}

    public class RptProfitAndLossFilter
    {
        public string LegalEntityId { get; set; }
        public string Type { get; set; }
        public string BusinessUnitId { get; set; }
        public DateTime? StartingDate { get; set; }
        public DateTime? EndingDate { get; set; }
        public string CurrencyId { get; set; }
        public bool DisplayZeroBalance { get; set; }
        public string CoaLevel { get; set; }
    }
}