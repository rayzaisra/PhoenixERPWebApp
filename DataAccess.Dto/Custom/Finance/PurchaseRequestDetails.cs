using DataAccess.PhoenixERP.Finance;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Dto.Custom.Finance
{
    public partial class PurchaseRequestDetails
    {
        public purchase_requestDTO purchase_request { get; set; }
        public List<purchase_request_detailDTO> purchase_request_details { get; set; }

    }

    public partial class vw_purchase_requestDetailsDTO
    {
        public vw_purchase_requestDTO purchase_request { get; set; }
    }
}
