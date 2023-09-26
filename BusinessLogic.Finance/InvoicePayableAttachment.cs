using BusinessLogic.Services.Finance;
using DataAccess.PhoenixERP.Finance;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Finance
{
    public partial class InvoicePayableAttachment : InvoicePayableAttachmentService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public InvoicePayableAttachment(DataContext AppUserData) : base(AppUserData)
        {

        }

        public override bool SaveEntity(invoice_payable_attachmentDTO RecordDto, ref bool isNew)
        {
            bool result = false;

            if (RecordDto == null) return result;

            try
            {
                var Exist = invoice_payable.Exists(RecordDto.invoice_payable_id);
                if (!Exist)
                {
                    throw new ArgumentException("Record doesnt have invoice payable");
                }
                var record = invoice_payable_attachment.FirstOrDefault(" WHERE invoice_payable_id= @0",RecordDto.invoice_payable_id);
                if (record != null)
                {
                    RecordDto.id = record.id;
                }
                result = Svc.SaveEntity(RecordDto, ref isNew);
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }

            return result;
        }
    }
}
