using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table(nameof(Pce), Schema = "pm")]
    public class Pce : PmEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column(nameof(Code)), StringLength(50)]
        public string Code { get; set; }

        [MaxLength(50)]
        [Column("job_id"), StringLength(50)]
        public string JobId { get; set; }

        [MaxLength(50)]
        [Column(nameof(Revision)), StringLength(50)]
        public string Revision { get; set; }

        [MaxLength(50)]
        [Column("currency_id"), StringLength(50)]
        public string CurrencyId { get; set; }

        [MaxLength(50)]
        [Column("mother_pca_id"), StringLength(50)]
        public string MotherPcaId { get; set; }

        [Column(nameof(Subtotal))]
        public decimal? Subtotal { get; set; }

        [Column("other_fee")]
        public decimal? OtherFee { get; set; }

        [MaxLength(18)]
        [Column("other_fee_name"), StringLength(18)]
        public string OtherFeeName { get; set; }

        [Column("other_fee_percentage")]
        public decimal? OtherFeePercentage { get; set; }

        [Column(nameof(Vat))]
        public decimal? Vat { get; set; }

        [Column(nameof(Total))]
        public decimal? Total { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [MaxLength(50)]
        [Column("type_of_expense_id"), StringLength(50)]
        public string TypeOfExpenseId { get; set; }

        [MaxLength(50)]
        [Column("shareservices_id"), StringLength(50)]
        public string ShareservicesId { get; set; }

        [MaxLength(50)]
        [Column("mainservice_category_id"), StringLength(50)]
        public string MainserviceCategoryId { get; set; }

        [Column(nameof(Termofpayment1))]
        public decimal? Termofpayment1 { get; set; }

        [Column(nameof(Termofpayment2))]
        public decimal? Termofpayment2 { get; set; }

        [Column(nameof(Termofpayment3))]
        public decimal? Termofpayment3 { get; set; }

        [MaxLength(50)]
        [Column("rate_card_id"), StringLength(50)]
        public string RateCardId { get; set; }

        [MaxLength(50)]
        [Column("pca_id"), StringLength(50)]
        public string PcaId { get; set; }

        //[Column("current_invoice")]
        //public decimal? CurrentInvoice { get; set; }

        //[Column(nameof(Balance))]
        //public decimal? Balance { get; set; }

        [MaxLength(50)]
        [Column("pce_reference"), StringLength(50)]
        public string PceReference { get; set; }

        [MaxLength(50)]
        [Column("po_client"), StringLength(50)]
        public string PoClient { get; set; }

        [Column("total_po_client")]
        public decimal? TotalPoClient { get; set; }

        [Column("pce_margin")]
        public decimal? PceMargin { get; set; }

        [Column("total_used_invoice")]
        public decimal? TotalUsedInvoice { get; set; }

        [Column("total_balance_invoice")]
        public decimal? TotalBalanceInvoice { get; set; }

        [Column("subtotal_original")]
        public decimal? SubtotalOriginal { get; set; }

        [Column("other_fee_original")]
        public decimal? OtherFeeOriginal { get; set; }

        [Column("vat_original")]
        public decimal? VatOriginal { get; set; }

        [Column("total_original")]
        public decimal? TotalOriginal { get; set; }

        [Column("pce_margin_original")]
        public decimal? PceMarginOriginal { get; set; }

        [NotMapped]
        public string CompanyId { get; set; }

        [NotMapped]
        public string CompanyName { get; set; }

        [NotMapped]
        public string ClientBrand { get; set; }

        [NotMapped]
        public string JobName { get; set; }

        [NotMapped]
        public string BrandName { get; set; }

        [NotMapped]
        public int? StatusApproval { get; set; }

        [NotMapped]
        public string RemarkRejected { get; set; }

        [NotMapped]
        public string external_brand_id { get; set; }
        [NotMapped]
        public string MainServicesName { get; set; }

        [NotMapped]
        public List<PceTask> PceTasks { get; set; }

        [NotMapped]
        public string MasterOutletId { get; set; }

        [NotMapped]
        public string MasterOutletLocation { get; set; }

        [NotMapped]
        public bool IsPM { get; set; }

        [NotMapped]
        public string CurrencySymbol { get; set; }

        [NotMapped]
        public string CurrencySymbolOriginal { get; set; }
    }
}