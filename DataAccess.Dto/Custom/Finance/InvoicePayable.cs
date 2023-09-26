using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.PhoenixERP.Finance
{

    public partial class InvoicePayableDTO
    {
        public invoice_payableDTO invoice_payable { get; set; }
        public List<invoice_payable_attachmentDTO> invoice_attachment { get; set; }
        public invoice_payable_approvalDTO invoice_approval { get; set; }
    }

    public partial class vw_InvoicePayableDTO
    {
        public vw_invoice_payableDTO Details { get; set; }
        public List<vw_invoice_payable_attachmentDTO> attachment { get; set; }
        public List<vw_invoice_payable_approvalDTO> approval { get; set; }
    }
}
