using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenix.Data.Models.Pm.Report
{
   public class ReportPCEDiego
    {
        public DateTime? Month { get; set; }
        public string Nature { get; set; }
        public string PeNumber { get; set; }
        public string Status { get; set; }
        public string Brand { get; set; }
        public string PIC { get; set; }
        public string BudgetOwner { get; set; }
        public string CoreActivityDescription { get; set; }
        public string Description { get; set; }
        public string JobName { get; set; }
        public decimal? Total { get; set; }
        public decimal? Vat { get; set; }
        public decimal Asf { get; set; }
        public decimal? GrandTotalIdr { get; set; }
        public decimal? GrandTOtalUsd { get; set; }
    }
}
