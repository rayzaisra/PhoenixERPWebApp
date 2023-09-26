using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("salary_allocation", Schema = "fn")]
    public class SalaryAllocation : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("evident_number"), StringLength(50)]
        public string Code { get; set; }

        [Column("transaction_date")]
        public DateTime? TransactionDate { get; set; }

        [MaxLength(50)]
        [Column("type"), StringLength(50)]
        public string Type { get; set; }

        [MaxLength(50)]
        [Column("currency_id"), StringLength(50)]
        public string CurrencyId { get; set; }

        [Column("exchange_rate")]
        public decimal? ExchangeRate { get; set; }

        [MaxLength(50)]
        [Column("reference_id"), StringLength(50)]
        public string ReferenceId { get; set; }

        [MaxLength(50)]
        [Column("reference_number"), StringLength(50)]
        public string ReferenceNumber { get; set; }

        [MaxLength(450)]
        [Column(nameof(Description)), StringLength(450)]
        public string Description { get; set; }

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
        [Column("main_service_category_id"), StringLength(50)]
        public string MainServiceCategoryId { get; set; }

        [MaxLength(50)]
        [Column("job_id"), StringLength(50)]
        public string JobId { get; set; }

        [MaxLength(50)]
        [Column("pce_id"), StringLength(50)]
        public string PceId { get; set; }

        [MaxLength(50)]
        [Column("pca_id"), StringLength(50)]
        public string PcaId { get; set; }

        [MaxLength(50)]
        [Column("client_id"), StringLength(50)]
        public string ClientId { get; set; }

        [Column("financial_year_id")]
        public string FinancialYearId { get; set; }

        [Column("finance_period_id")]
        public string FinancePeriodId { get; set; }

        [MaxLength(50)]
        [Column(nameof(Status)), StringLength(50)]
        public string Status { get; set; }

        [NotMapped]
        public List<SalaryAllocationDetail> SalaryAllocationDetails { get; set; }

        [NotMapped]
        public string LegalEntityName { get; set; }

        [NotMapped]
        public string BusinessUnitName { get; set; }

        [NotMapped]
        public string AffiliationName { get; set; }

        [NotMapped]
        public string ClientName { get; set; }

        [NotMapped]
        public string MainServiceCategoryName { get; set; }

        [NotMapped]
        public string ViewFiscalYear { get; set; }
        [NotMapped]
        public string ViewPeriod { get; set; }
        [NotMapped]
        public DateTime? ViewDateTransaction { get; set; }

        //For List Search
        [NotMapped]
        public DateTime? TransactionDateEnd { get; set; }

        //For Approval
        [NotMapped]
        public int StatusApproval { get; set; }

        [NotMapped]
        [MaxLength(255)]
        public string RemarksProcess { get; set; }
    }

    public class SalaryAllocationSearchNumber
    {
        public string Type { get; set; }
        public int? FiscalYear { get; set; }
        public DateTime? StartPeriod { get; set; }
        public DateTime? EndPeriod { get; set; }
    }
}