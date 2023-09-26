using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("fixed_asset", Schema = "fn")]
    public class FixedAsset : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("item_id"), StringLength(50)]
        public string ItemId { get; set; }

        [MaxLength(50)]
        [Column("fixed_asset_name"), StringLength(50)]
        public string FixedAssetName { get; set; }

        [MaxLength(50)]
        [Column("fixed_asset_number"), StringLength(50)]
        public string FixedAssetNumber { get; set; }

        [MaxLength(50)]
        [Column("fixed_asset_number_manual"), StringLength(50)]
        public string FixedAssetNumberManual { get; set; }

        [Column("unit_count")]
        public decimal? UnitCount { get; set; }

        [MaxLength(50)]
        [Column("purchase_order_id"), StringLength(50)]
        public string PurchaseOrderId { get; set; }

        [MaxLength(50)]
        [Column("invoice_id"), StringLength(50)]
        public string InvoiceId { get; set; }

        [MaxLength(50)]
        [Column("evident_number"), StringLength(50)]
        public string EvidentNumber { get; set; }

        [Column("current_price")]
        public decimal? CurrentPrice { get; set; }

        [MaxLength(50)]
        [Column("currency_id"), StringLength(50)]
        public string CurrencyId { get; set; }

        [Column("exchange_rate")]
        public decimal? ExchangeRate { get; set; }

        [MaxLength(450)]
        [Column("current_location"), StringLength(450)]
        public string LocationId { get; set; }

        [MaxLength(50)]
        [Column("account_id"), StringLength(50)]
        public string AccountId { get; set; }

        [MaxLength(50)]
        [Column("commercial_depreciation_id"), StringLength(50)]
        public string CommercialDepreciationId { get; set; }

        [Column("economic_period")]
        public int? EconomicPeriod { get; set; }

        [MaxLength(450)]
        [Column(nameof(Description)), StringLength(450)]
        public string Description { get; set; }

        [Column("registration_date")]
        public DateTime? RegistrationDate { get; set; }

        [MaxLength(50)]
        [Column("item_type_id"), StringLength(50)]
        public string ItemTypeId { get; set; }

        [MaxLength(50)]
        [Column("item_group_id"), StringLength(50)]
        public string ItemGroupId { get; set; }

        [MaxLength(50)]
        [Column("item_category_id"), StringLength(50)]
        public string ItemCategoryId { get; set; }

        [MaxLength(50)]
        [Column("item_condition_id"), StringLength(50)]
        public string ItemConditionId { get; set; }

        [MaxLength(50)]
        [Column("serial_number"), StringLength(50)]
        public string SerialNumber { get; set; }

        [Column("gross_value")]
        public decimal? GrossValue { get; set; }

        [Column(nameof(Residue))]
        public decimal? Residue { get; set; }

        [Column("acquisition_cost")]
        public decimal? AcquisitionCost { get; set; }

        [Column("current_estimation_price")]
        public decimal? CurrentEstimationPrice { get; set; }

        [MaxLength(50)]
        [Column(nameof(Rounding)), StringLength(50)]
        public string Rounding { get; set; }

        [MaxLength(50)]
        [Column("coa_id_akumulasi"), StringLength(50)]
        public string CoaIdAkumulasi { get; set; }

        [MaxLength(50)]
        [Column("coa_id_depresiasi"), StringLength(50)]
        public string CoaIdDepresiasi { get; set; }

        [MaxLength(50)]
        [Column("good_received_id"), StringLength(50)]
        public string GoodReceivedId { get; set; }

        [MaxLength(50)]
        [Column("good_received_detail_id"), StringLength(50)]
        public string GoodReceivedDetailId { get; set; }

        [MaxLength(50)]
        [Column(nameof(Manufacture)), StringLength(50)]
        public string Manufacture { get; set; }

        [MaxLength(50)]
        [Column(nameof(Brand)), StringLength(50)]
        public string Brand { get; set; }

        [MaxLength(50)]
        [Column(nameof(Model)), StringLength(50)]
        public string Model { get; set; }

        [MaxLength(50)]
        [Column("product_number"), StringLength(50)]
        public string ProductNumber { get; set; }

        [MaxLength(50)]
        [Column("sku_code"), StringLength(50)]
        public string SkuCode { get; set; }

        [MaxLength(50)]
        [Column("coa_id"), StringLength(50)]
        public string CoaId { get; set; }

        [MaxLength(50)]
        [Column(nameof(Color)), StringLength(50)]
        public string Color { get; set; }

        [Column(nameof(Qty))]
        public decimal? Qty { get; set; }

        [MaxLength(50)]
        [Column("uom"), StringLength(50)]
        public string UomId { get; set; }

        [MaxLength(450)]
        [Column("unit_name"), StringLength(450)]
        public string UnitName { get; set; }

        [Column("condition")]
        public int? Condition { get; set; }

        [MaxLength(50)]
        [Column("business_unit"), StringLength(50)]
        public string BusinessUnitId { get; set; }

        [MaxLength(50)]
        [Column("legal_entity"), StringLength(50)]
        public string LegalEntityId { get; set; }

        [MaxLength(50)]
        [Column("affiliation"), StringLength(50)]
        public string AffiliationId { get; set; }

        [Column(nameof(Lifetime))]
        public decimal? Lifetime { get; set; }

        [MaxLength(50)]
        [Column("master_depreciation_id"), StringLength(50)]
        public string MasterDepreciationId { get; set; }

        [MaxLength(50)]
        [Column("status_depreciation"), StringLength(50)]
        public string StatusDepreciation { get; set; }

        [MaxLength(100)]
        [Column("remarks_depreciation"), StringLength(100)]
        public string RemarksDepreciation { get; set; }

        [MaxLength(50)]
        [Column("amortization_value"), StringLength(50)]
        public decimal? AmortizationValue { get; set; }

        [MaxLength(50)]
        [Column("amortization_period"), StringLength(50)]
        public string AmortizationPeriod { get; set; }

        [Column("vendor_id")]
        public string VendorId { get; set; }

        [Column("division_id")]
        public string DivisionId { get; set; }

        [Column("get_from")]
        public string GetFrom { get; set; }

        [MaxLength(100)]
        [Column("file_attachment"), StringLength(100)]
        public string FileUpload { get; set; }


        [NotMapped]
        public string FileUploadName { get; set; }

        [NotMapped]
        public string ConditionName { get; set; }

        [NotMapped]
        public string LocationName { get; set; }

        [NotMapped]
        public string BusinessUnitName { get; set; }

        [NotMapped]
        public string LegalEntityName { get; set; }

        [NotMapped]
        public string AffiliationName { get; set; }
        
        [NotMapped]
        public IFormFile file { get; set; }

        [NotMapped]
        public string ItemCategoryName { get; set; }

        [NotMapped]
        public string CurrencyName { get; set; }

        [NotMapped]
        public int? LifeTimeMonth { get; set; }

        [NotMapped]
        public string COADebit { get; set; }

        [NotMapped]
        public string COACredit { get; set; }

        //[NotMapped]
        //public string VendorName { get; set; }

        //[NotMapped]
        //public string VendorBrand { get; set; }

        //[NotMapped]
        //public DateTime? RegistrationDateDisplay { get; set; }

        //[NotMapped]
        //public List<FixedAssetMovement> FixedAssetMovement { get; set; }
    }
}
