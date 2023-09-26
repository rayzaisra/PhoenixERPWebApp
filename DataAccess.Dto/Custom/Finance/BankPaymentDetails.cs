using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.PhoenixERP.Finance
{
    public partial class BankPaymentDetailsDTO
    {
        public bank_paymentDTO bank_payment { get; set; }
        public financial_transaction_detailDTO[] details { get; set; }
    }

    public partial class vw_BankPaymentDetailsDTO
    {
        public vw_bank_paymentDTO bank_payment { get; set; }
        public vw_financial_transactionDTO transaction { get; set; }
        public List<vw_financial_transaction_detailDTO> details { get; set; }
    }
}
