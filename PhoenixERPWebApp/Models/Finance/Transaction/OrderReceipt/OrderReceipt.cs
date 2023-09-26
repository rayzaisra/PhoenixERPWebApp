using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models.Finance
{
    public class OrderReceipt : FinanceEntityBase
    {
        public string Id { get; set; }

        public string ReceiptOrderNumber { get; set; }
        public string GoodRequestNumber { get; set; }
        public DateTime? DateReceipt { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string BusinessUnitId { get; set; }
        public string BusinessUnitName { get; set; }
        public string Status { get; set; }
        public string LegalEntityId { get; set; }
        public string LegalEntityName { get; set; }
        public string UploadImage { get; set; }
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
