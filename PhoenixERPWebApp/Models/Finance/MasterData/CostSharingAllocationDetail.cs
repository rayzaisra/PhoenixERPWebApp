using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class CostSharingAllocationDetail : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Cost Sharing Allocation Id")]
        public string CostSharingAllocationId { get; set; }

        [DisplayName("Business Unit")]
        public string BusinessUnitId { get; set; }

        [DisplayName("Percentage")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        //[DataType(DataType.dec)]
        public decimal? CostSharingAllocationPercent { get; set; }
        public string MonthlyDescription { get; set; }

        //Not Mapped
        [DisplayName("Business Unit")]
        public string BusinessUnitName { get; set; }
    }
}