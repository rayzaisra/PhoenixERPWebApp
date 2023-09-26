using DataAccess.PhoenixERP.Finance;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Dto.Custom.Finance
{
    public partial class CashAdvanceWithApprovals
    {
        public cash_advanceDTO cashAdvance { get; set; }

        public List<cash_advance_detailDTO> cashAdvanceDetails { get; set; }

        //public string approverIds { get; set; }
    }
}
