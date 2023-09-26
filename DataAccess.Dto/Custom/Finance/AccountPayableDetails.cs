using DataAccess.PhoenixERP.Finance;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Dto.Custom.Finance
{
    public partial class AccountPayableDetails
    {
        public account_payableDTO account_payable { get; set; }
        public List<account_payable_detailDTO> account_payable_details { get; set; }
    }

    public partial class vw_account_payable_detailDTO
    {
        public vw_account_payableDTO account_payable { get; set; }
    }
}
