using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class FixedAssetMovementDetail : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Item Id")]
        public string ItemId { get; set; }

        [DisplayName("Fixed Asset Name")]
        public string FixedAssetName { get; set; }

        [DisplayName("ID Number")]
        public string FixedAssetNumber { get; set; }

        [DisplayName("Fixed Number")]
        public string FixedAssetNumberManual { get; set; }

        public string FixedAssetMovementNumber { get; set; }

        [DisplayName("Unit Count")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? UnitCount { get; set; }

        [DisplayName("Purchase Order Id")]
        public string PurchaseOrderId { get; set; }

        [DisplayName("Invoice Id")]
        public string InvoiceId { get; set; }

        [DisplayName("Evident Number")]
        public string EvidentNumber { get; set; }

        [DisplayName("Current Price")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? CurrentPrice { get; set; }

        public string CurrencyId { get; set; }

        [DisplayName("Exchange Rate")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? ExchangeRate { get; set; }

        [DisplayName("Location")]
        public string LocationId { get; set; }

        [DisplayName("Account Id")]
        public string AccountId { get; set; }

        [DisplayName("Commercial Depreciation Id")]
        public string CommercialDepreciationId { get; set; }

        [DisplayName("Economic Period")]
        public int? EconomicPeriod { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Date Receipt")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? RegistrationDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? RegistrationDateDisplay { get; set; }



        [DisplayName("Item Type Id")]
        public string ItemTypeId { get; set; }

        [DisplayName("Item Group Id")]
        public string ItemGroupId { get; set; }

        [DisplayName("Item Category Id")]
        public string ItemCategoryId { get; set; }

        [DisplayName("Item Condition Id")]
        public string ItemConditionId { get; set; }

        [DisplayName("Serial Number")]
        public string SerialNumber { get; set; }

        [DisplayName("Gross Value")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? GrossValue { get; set; }

        [DisplayName("Residue")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? Residue { get; set; }

        [DisplayName("Acquisition Cost")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? AcquisitionCost { get; set; }


        [DisplayName("Current Estimation Price")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? CurrentEstimationPrice { get; set; }

        [DisplayName("Rounding")]
        public string Rounding { get; set; }

        [DisplayName("Coa Id Akumulasi")]
        public string CoaIdAkumulasi { get; set; }

        [DisplayName("Coa Id Depresiasi")]
        public string CoaIdDepresiasi { get; set; }

        [DisplayName("Good Received Number")]
        public string GoodReceivedId { get; set; }

        [DisplayName("Good Received Detail Id")]
        public string GoodReceivedDetailId { get; set; }

        [DisplayName("Manufacturer")]
        public string Manufacture { get; set; }

        [DisplayName("Brand")]
        public string Brand { get; set; }

        [DisplayName("Model")]
        public string Model { get; set; }

        [DisplayName("Product Number")]
        public string ProductNumber { get; set; }

        [DisplayName("SKU Code")]
        public string SkuCode { get; set; }

        [DisplayName("Color")]
        public string Color { get; set; }


        [DisplayName("Unit of Measure")]
        public string UomId { get; set; }

        [DisplayName("Unit Name")]
        public string UnitName { get; set; }

        [DisplayName("Condition")]
        public string Condition { get; set; }

        [DisplayName("Business Unit")]
        public string BusinessUnitId { get; set; }

        [DisplayName("Legal Entity")]
        public string LegalEntityId { get; set; }

        [DisplayName("Affiliation")]
        public string AffiliationId { get; set; }

        [DisplayName("Lifetime")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? Lifetime { get; set; }

        [DisplayName("Master Depreciation Id")]
        public string MasterDepreciationId { get; set; }

        [DisplayName("Status Depreciation")]
        public string StatusDepreciation { get; set; }

        [DisplayName("Remarks Depreciation")]
        public string RemarksDepreciation { get; set; }

        [DisplayName("COA")]
        public string CoaId { get; set; }

        //NOT Mapped
        [DisplayName("Condition")]
        public string ConditionName { get; set; }
        [DisplayName("Location")]
        public string LocationName { get; set; }
        [DisplayName("Business Unit")]
        public string BusinessUnitName { get; set; }
        [DisplayName("Legal Entity")]
        public string LegalEntityName { get; set; }
        [DisplayName("Affiliation")]
        public string AffiliationName { get; set; }
        [DisplayName("Vendor Name")]
        public string VendorName { get; set; }
        [DisplayName("Brand Name")]
        public string VendorBrand { get; set; }

        public string Uom { get; set; }

        public string VendorId { get; set; }

        public string DivisionId { get; set; }

        public string GetFrom { get; set; }

        public decimal? AmortizationValue { get; set; }
        public string AmortizationPeriod { get; set; }

        [DisplayName("Files")]
        public string FileUploadName { get; set; }
        [DisplayName("Upload Files")]
        public string FileUpload { get; set; }


        [NotMapped]
        public List<FixedAssetMovement> FixedAssetMovement { get; set; }
        [NotMapped]
        public decimal? Qty { get; set; }
        [NotMapped]
        public string Remarks { get; set; }
    }
}
