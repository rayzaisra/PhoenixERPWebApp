using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.PhoenixERP.Finance;

namespace DataAccess.Dto.Custom.Finance
{
    public partial class InvoicePayableDetailDTO
    {
        public invoice_payableDTO Invoice { get; set; }

        public List<invoice_payable_attachmentDTO> Attachments { get; set; }
    }

    public partial class ViewInvoicePayableDetailDTO
    {
        public vw_invoice_payableDTO Invoice { get; set; }

        public List<vw_invoice_payable_attachmentDTO> Attachments { get; set; }
    }
}
