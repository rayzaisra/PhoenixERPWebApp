using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.PhoenixERP.Finance
{
    public partial class FinancialTransactionDetails
    {
        public financial_transactionDTO transaction { get; set; }
        public financial_transaction_detailDTO[] details { get; set; }
    }

    public partial class vw_FinancialTransactionDetails
    {
        public vw_financial_transactionDTO transaction { get; set; }
        public List<vw_financial_transaction_detailDTO> details { get; set; }
    }
}
