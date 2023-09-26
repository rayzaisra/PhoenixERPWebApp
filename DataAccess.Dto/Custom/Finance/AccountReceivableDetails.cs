using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.PhoenixERP.Finance
{
    public partial class account_receivable_detailDTO
    {
        public account_receivableDTO account_receivable { get; set; }
        public financial_transaction_detailDTO[] details { get; set; }
    }

    public partial class vw_account_receivable_detailDTO
    {
        public vw_account_receivableDTO account_receivable { get; set; }
        public vw_financial_transactionDTO transaction { get; set; }
        public List<vw_financial_transaction_detailDTO> details { get; set; }
    }
}
