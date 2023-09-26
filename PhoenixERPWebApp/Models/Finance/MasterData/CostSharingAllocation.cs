using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class CostSharingAllocation : FinanceEntityBase
    {
        [DisplayName("Cost Allocatio Ratio ID")]
        public string Id { get; set; }

        [DisplayName("Cost Allocation Name")]
        public string CostSharingAllocationName { get; set; }

        [DisplayName("Total Cost Allocation Ratio")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? TotalCostAllocationSharing { get; set; }

        [DisplayName("Tax Payable VAT 100%")]
        public string BusinessUnitId { get; set; }

        public bool? IsMonthly { get; set; }

        public List<CostSharingAllocationDetail> CostSharingAllocationDetails { get; set; }
    }
}