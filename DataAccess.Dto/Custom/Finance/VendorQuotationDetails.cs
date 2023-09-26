using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.PhoenixERP.Finance
{
    public partial class vendor_quotation_detailsDTO
    {
        public vendor_quotationDTO VendorQuotation { get; set; }
        //public List<vendor_quotation_vendorDTO> VendorQuotationVendor { get; set; }
        public List<vendor_quotation_detailDTO> Details { get; set; }
    }

    public partial class vw_vendor_quotation_detailsDto
    {
        public vw_vendor_quotationDTO VendorQuotation { get; set; }
        //public List<vw_vendor_quotation_vendorDTO> VendorQuotationVendor { get; set; }
        public List<vw_vendor_quotation_detailDTO> Details { get; set; }

        //public string[] BiddingSelected { get; set; }
    }
}
