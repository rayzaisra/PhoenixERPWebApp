using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.PhoenixERP.Finance
{
    public partial class account_cost_sharingDTO
    {
        public cost_sharing_allocationDTO cost_sharing_allocation { get; set; }
        public List<cost_sharing_allocation_detailDTO> cost_sharing_allocation_detail { get; set; }
    }
    public partial class vw_account_cost_sharingDTO
    {
        public vw_cost_sharing_allocationDTO cost_sharing_allocation { get; set; }
        public List<vw_cost_sharing_allocation_detailDTO> cost_sharing_allocation_detail { get; set; }
    }
    
}
