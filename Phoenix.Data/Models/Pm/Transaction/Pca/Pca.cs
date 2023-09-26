using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table(nameof(Pca), Schema = "pm")]
    public class Pca : PmEntityBase
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

        [MaxLength(53)]
        [Column("other_fee_name"), StringLength(53)]
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

        [Column(nameof(Margin))]
        public decimal? Margin { get; set; }

        [Column("cost_analysis")]
        public decimal? CostAnalysis { get; set; }

        [Column("total_task_price")]
        public decimal? TotalTaskPrice { get; set; }

        [Column("total_task_price_used")]
        public decimal? TotalTaskPriceUsed { get; set; }

        [Column("total_task_price_balance")]
        public decimal? TotalTaskPriceBalance { get; set; }

        [NotMapped]
        public string CompanyId { get; set; }

        [NotMapped]
        public string CompanyName { get; set; }

        [NotMapped]
        public string ClientBrand { get; set; }

        [NotMapped]
        public string JobNumber { get; set; }

        [NotMapped]
        public string JobName { get; set; }

        [NotMapped]
        public string BrandName { get; set; }

        [NotMapped]
        public int? StatusApproval { get; set; }

        [NotMapped]
        public string RemarkRejected { get; set; }

        [NotMapped]
        public string RemarkRFQ { get; set; }

        [NotMapped]
        public List<PcaTask> PcaTasks { get; set; }

        [NotMapped]
        public string StatusPCE { get; set; }

        [NotMapped]
        public bool IsPrefered { get; set; }

        [NotMapped]
        public DateTime? DueDate { get; set; }

        [NotMapped]
        public double? TotalMotherPCA { get; set; }

        [NotMapped]
        public string MasterOutletId { get; set; }

        [NotMapped]
        public string MasterOutletLocation { get; set; }

        [NotMapped]
        public string PceId { get; set; }
        [JsonConverter(typeof(ConcreteConverter<FileDetail>))]
        [NotMapped]
        public IFormFile file { get; set; }
        [NotMapped]
        public string DetailSpecFile { get; set; }
        [NotMapped]
        public string DetailSpecFileName { get; set; }

        [NotMapped]
        public string JobStatusId { get; set; }

        //FOR RFQ 
        [NotMapped]
        public string CurrencyRequestId { get; set; }

        [NotMapped]
        public decimal? ExchangeRateRequest { get; set; }

        [NotMapped]
        public string OperatorRequest { get; set; }

        [NotMapped]
        public List<PcaTaskRequest> PcaTaskRequest { get; set; }

        [NotMapped]
        public string DefaultCurrencyId { get; set; }

        [NotMapped]
        public string CurrencySymbol { get; set; }

        [NotMapped]
        public string CurrencySymbolExchange { get; set; }
        [NotMapped]
        public bool IsJobOwner { get; set; }
    }

    public class PcaTaskRequest
    {
        public string TaskId { get; set; }

        public string Task { get; set; }

        public int? Quantity { get; set; }

        public string UomId { get; set; }

        public decimal? UnitPrice { get; set; }

        public decimal? TotalPrice { get; set; }

        public string JobId { get; set; }

        public string UomName { get; set; }

        public decimal? RemainingBudget { get; set; }

        public decimal? TotalRequest { get; set; }

        public decimal? TotalExchangeRequest { get; set; }

        public string CurrencySymbol { get; set; }

        public string CurrencySymbolExchange { get; set; }
    }
}