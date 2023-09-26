using PhoenixERPWebApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace PhoenixERPWebApp.Models.Finance.Transaction.FixedAssetOutMovement
{
    public class FixedAssetOutMovementDetail : FinanceEntityBase
    {
        public string Id { get; set; }
        [DisplayName("Fixed Asset Out Movement Id")]
        public string FixedAssetOutId { get; set; }
        [DisplayName("Fixed Asset Id")]
        public string FixedAssetId { get; set; }
        [DisplayName("Account Id")]
        public string AccountId { get; set; }
        [DisplayName("SKU Code")]
        public string SkuCode { get; set; }
        [DisplayName("Serial Number")]
        public string SerialNumber { get; set; }
        [DisplayName("Item Category Id")]
        public string ItemCategoryId { get; set; }
        [DisplayName("Item Category Name")]
        public string ItemCategoryName { get; set; }
        [DisplayName("UOM Id")]
        public string UomId { get; set; }
        [DisplayName("Unit Name")]
        public string UnitName { get; set; }
        [DisplayName("Brand")]
        public string Brand { get; set; }
        [DisplayName("Sub Brand")]
        public string SubBrand { get; set; }
        [DisplayName("Description")]
        public string Description { get; set; }
        [DisplayName("Quantity")]
        public decimal Qty { get; set; }
        [DisplayName("Remark")]
        public string Remark { get; set; }
        [DisplayName("File")]
        public string FileAttachment { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [DisplayName("Request Date")]
        public DateTime? RequestDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [DisplayName("Registration Date")]
        public DateTime? RegistrationDate { get; set; }
        [DisplayName("Location Id")]
        public string LocationId { get; set; }
        [DisplayName("Location Name")]
        public string LocationName { get; set; }
        [DisplayName("Condition Id")]
        public string Condition { get; set; }
        [DisplayName("Condition Name")]
        public string ConditionName { get; set; }
        [DisplayName("Legal Entity Id")]
        public string LegalEntityId { get; set; }
        [DisplayName("Legal Entity Name")]
        public string LegalEntityName { get; set; }
        [DisplayName("Url Link")]
        public string UrlLink { get; set; }
        [DisplayName("Request Status")]
        public string RequestStatus { get; set; }
        public decimal QtyMax { get; set; }

        public string RequestStatusName { get; set; }
    }
}
