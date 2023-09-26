using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models.Finance
{
    [Table("order_receipt", Schema = "fn")]
    public class OrderReceipt : FinanceEntityBase
    {
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }
        [Column("receipt_order_number")]
        public string ReceiptOrderNumber { get; set; }
        [Column("good_request_number")]
        public string GoodRequestNumber { get; set; }
        [Column("date_receipt")]
        public DateTime? DateReceipt { get; set; }
        [Column("employee_id")]
        public string EmployeeId { get; set; }
        [Column("employee_name")]
        public string EmployeeName { get; set; }
        [Column("business_unit_id")]
        public string BusinessUnitId { get; set; }
        [Column("business_unit_name")]
        public string BusinessUnitName { get; set; }
        [Column("status")]
        public string Status { get; set; }
        [Column("legal_entity_id")]
        public string LegalEntityId { get; set; }
        [Column("legal_entity_name")]
        public string LegalEntityName { get; set; }
        [Column("upload_image")]
        public string UploadImage { get; set; }
        [Column("upload_doc")]
        public string UploadDoc { get; set; }
        [NotMapped]
        public IFormFile fileImage { get; set; }
        [NotMapped]
        public IFormFile fileDoc { get; set; }
        [NotMapped]
        public string ImageName { get; set; }
        [NotMapped]
        public string DocName { get; set; }
    }
}
