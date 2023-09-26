using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.PhoenixERP.Finance
{
    public partial class CompanyDetailsDTO
    {
        public companyDTO company { get; set; }
        public List<company_addressDTO> company_address { get; set; }
        public List<company_bankDTO> company_bank { get; set; }
        public List<company_contactDTO> company_contact { get; set; }
    }

    public partial class vw_CompanyDetailsDTO
    {
        public vw_companyDTO company { get; set; }
        public List<vw_company_addressDTO> company_address { get; set; }
        public List<vw_company_bankDTO> company_bank { get; set; }
        public List<vw_company_contactDTO> company_contact { get; set; }
    }
}
