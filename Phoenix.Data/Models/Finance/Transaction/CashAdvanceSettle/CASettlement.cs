using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models.Finance
{
    [Table("ca_settlement", Schema = "fn")]
    public class CASettlement : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("cash_advance_id"), StringLength(50)]
        public string CashAdvanceId { get; set; }

        [MaxLength(50)]
        [Column("payment_request_id"), StringLength(50)]
        public string PaymentRequestId { get; set; }

        [MaxLength(50)]
        [Column("settlement_number"), StringLength(50)]
        public string SettlementNumber { get; set; }

        [Column("ca_settlement_date")]
        public DateTime? CaSettlementDate { get; set; }

        [Column(nameof(Amount))]
        public decimal? Amount { get; set; }

        [Column("total_amount")]
        public decimal? TotalAmount { get; set; }

        [Column("ca_balance")]
        public decimal? CaBalance { get; set; }

        [MaxLength(50)]
        [Column("from_account"), StringLength(50)]
        public string FromAccount { get; set; }

        [MaxLength(50)]
        [Column("to_account"), StringLength(50)]
        public string ToAccount { get; set; }

        [Column("file_receipt"), StringLength(150)]
        public string FileReceipt { get; set; }

        [Column("section_upload_evidence"), StringLength(150)]
        public string SectionUploadEvidence { get; set; }

        [MaxLength(50)]
        [Column("job_id"), StringLength(50)]
        public string JobId { get; set; }

        [MaxLength(50)]
        [Column("pca_id"), StringLength(50)]
        public string PcaId { get; set; }

        [MaxLength(50)]
        [Column("mainservice_category_id"), StringLength(50)]
        public string MainserviceCategoryId { get; set; }

        [MaxLength(50)]
        [Column("business_unit_id"), StringLength(50)]
        public string BusinessUnitId { get; set; }

        [MaxLength(50)]
        [Column("legal_entity_id"), StringLength(50)]
        public string LegalEntityId { get; set; }

        [MaxLength(50)]
        [Column("affiliation_id"), StringLength(50)]
        public string AffiliationId { get; set; }

        [MaxLength(50)]
        [Column("vendor_id"), StringLength(50)]
        public string VendorId { get; set; }

        [MaxLength(450)]
        [Column("vendor_name"), StringLength(450)]
        public string VendorName { get; set; }

        [MaxLength(50)]
        [Column("pce_type_id"), StringLength(50)]
        public string PceTypeId { get; set; }

        [MaxLength(50)]
        [Column("currency_id"), StringLength(50)]
        public string CurrencyId { get; set; }

        [Column("exchange_rate")]
        public decimal? ExchangeRate { get; set; }

        [Column(nameof(Vat))]
        public decimal? Vat { get; set; }

        [Column("Vatpercent")]
        public double? VatPercent { get; set; }

        [Column("Taxpayable")]
        public decimal? TaxPayable { get; set; }

        [Column("Taxpayablepercent")]
        public decimal? TaxPayablePercent { get; set; }

        [MaxLength(50)]
        [Column("tax_pay_coa_id"), StringLength(50)]
        public string TaxPayCoaId { get; set; }

        [MaxLength(50)]
        [Column("request_employee_id"), StringLength(50)]
        public string RequestEmployeeId { get; set; }

        [MaxLength(150)]
        [Column("request_employee_name"), StringLength(150)]
        public string RequestEmployeeName { get; set; }

        [MaxLength(50)]
        [Column("request_job_title_id"), StringLength(50)]
        public string RequestJobTitleId { get; set; }

        [MaxLength(150)]
        [Column("request_job_title_name"), StringLength(150)]
        public string RequestJobTitleName { get; set; }

        [MaxLength(50)]
        [Column("request_job_grade_id"), StringLength(50)]
        public string RequestJobGradeId { get; set; }

        [MaxLength(150)]
        [Column("request_job_grade_name"), StringLength(150)]
        public string RequestJobGradeName { get; set; }

        [MaxLength(150)]
        [Column("requested_by"), StringLength(150)]
        public string RequestedBy { get; set; }

        [Column("requested_on")]
        public DateTime? RequestedOn { get; set; }

        [MaxLength(50)]
        [Column("on_behalf_id"), StringLength(50)]
        public string OnBehalfId { get; set; }

        [MaxLength(50)]
        [Column("bank_id"), StringLength(50)]
        public string BankId { get; set; }

        [MaxLength(150)]
        [Column("account_number"), StringLength(150)]
        public string AccountNumber { get; set; }

        [MaxLength(50)]
        [Column("ca_status_id"), StringLength(50)]
        public string Status { get; set; }

        [MaxLength(50)]
        [Column("to_account_number"), StringLength(50)]
        public string ToAccountNumber { get; set; }

        [MaxLength(150)]
        [Column("transfer_to"), StringLength(150)]
        public string TransferTo { get; set; }

        //For Approval
        [NotMapped]
        public int StatusApproval { get; set; }
        [NotMapped]
        public string RemarksProcess { get; set; }


        //For Process Upload
        [NotMapped]
        public string FileReceiptName { get; set; }
        [NotMapped]
        public IFormFile receiptFile { get; set; }

        [NotMapped]
        public string SectionUploadEvidenceName { get; set; }

        [NotMapped]
        public IFormFile SectionUploadEvidenceFile { get; set; }


        //For Information CA
        [NotMapped]
        public CashAdvance CashAdvance { get; set; }

        //Detail 
        [NotMapped]
        public List<CASettlementDetail> CASettlementDetail { get; set; }

        [NotMapped]
        public string CurrencySymbol { get; set; }
    }
}
