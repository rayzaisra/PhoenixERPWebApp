using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.PhoenixERP.Finance
{
    public partial class RequestForQuotationDTO
    {
        public request_for_quotationDTO RequestForQuotation { get; set; }
        public string[] VendorList { get; set; }
        public List<request_for_quotation_detailDTO> Details { get; set; }
    }
    public partial class RequestForQuotationDetailsDTO
    {
        public request_for_quotationDTO RequestForQuotation { get; set; }
        public string[] VendorList { get; set; } 
        public List<request_for_quotation_detailDTO> Details { get; set; }
    }

    public partial class vw_request_for_quotation_detailsDto
    {
        public vw_request_for_quotationDTO RequestForQuotation { get; set; }
        //public List<vw_request_for_quotation_detailsDto> Details { get; set; }
    }
}
