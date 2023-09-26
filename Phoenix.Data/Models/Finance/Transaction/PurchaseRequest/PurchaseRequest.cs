using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models
{
    [Table("purchase_request", Schema = "fn")]
    public class PurchaseRequest : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("request_number"), StringLength(50)]
        public string RequestNumber { get; set; }

        [Column("request_datetime")]
        public DateTime? RequestDatetime { get; set; }

        [MaxLength(50)]
        [Column("legal_entity_id"), StringLength(50)]
        public string LegalEntityId { get; set; }

        [MaxLength(50)]
        [Column("business_unit_id"), StringLength(50)]
        public string BusinessUnitId { get; set; }

        [MaxLength(50)]
        [Column("affiliation_id"), StringLength(50)]
        public string AffiliationId { get; set; }

        [MaxLength(50)]
        [Column("currency_id"), StringLength(50)]
        public string CurrencyId { get; set; }

        [Column("exchange_rate")]
        public decimal? ExchangeRate { get; set; }

        [Column("operator"), StringLength(50)]
        public string Operator { get; set; }

        [Column("sub_total")]
        public decimal? SubTotal { get; set; }

        [Column("vat_percent")]
        public decimal? VatPercent { get; set; }

        [Column(nameof(Vat))]
        public decimal? Vat { get; set; }

        [Column("grand_total")]
        public decimal? GrandTotal { get; set; }

        [Column("due_date")]
        public DateTime? DueDate { get; set; }

        [MaxLength(50)]
        [Column(nameof(Status)), StringLength(50)]
        public string Status { get; set; }

        [Column("budget_year")]
        public int? BudgetYear { get; set; }

        [MaxLength(50)]
        [Column("budget_owner_id"), StringLength(50)]
        public string BudgetOwnerId { get; set; }

        [MaxLength(50)]
        [Column("requestor_division_id"), StringLength(50)]
        public string RequestorDivisionId { get; set; }

        [MaxLength(50)]
        [Column("document_specification_file"), StringLength(50)]
        public string DocumentSpecificationFile { get; set; }

        //Not Mapped
        [NotMapped]
        public string Requestor { get; set; }

        [NotMapped]
        public string RequestorDivisionName { get; set; }

        [NotMapped]
        public string BusinessUnitName { get; set; }
        [NotMapped]
        public List<PurchaseRequestDetail> PurchaseRequestDetails { get; set; }

        [NotMapped]
        public List<PaymentRequestDetailJournal> JournalEntry { get; set; }

        //For Approval
        [NotMapped]
        public int StatusApproval { get; set; }

        [NotMapped]
        [MaxLength(255)]
        public string RemarksProcess { get; set; }

        //for file input detail
        [NotMapped]
        public IFormFile file { get; set; }

        [NotMapped]
        public string DocumentSpecificationFileName { get; set; }

        [NotMapped]
        public string CurrencySymbol { get; set; }

        [NotMapped]
        public string LegalEntityName { get; set; }
    }
}
