using DataAccess.PhoenixERP.Finance;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Dto.Custom.Finance
{
    public partial class InternalRfqDetailsDTO
    {
        public internal_rfqDTO InternalRfq { get; set; }
        public List<internal_rfq_detailDTO> Details { get; set; }
    }

    public partial class vw_internal_rfq_detailsDto
    {
        public vw_internal_rfqDTO InternalRfq { get; set; }
        //public List<vw_request_for_quotation_detailsDto> Details { get; set; }
    }
}
