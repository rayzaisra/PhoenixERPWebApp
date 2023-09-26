using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class FiscalYearDto
    {
        public string Id { get; set; }
        public string LegalEntityId{ get; set; }
        public string FinancialYearNumber { get; set; }
        public string FinancialYearName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public bool? IsClose { get; set; }
    }
}

