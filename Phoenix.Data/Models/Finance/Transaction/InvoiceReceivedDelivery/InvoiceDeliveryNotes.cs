using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("invoice_delivery_notes", Schema = "fn")]
    public class InvoiceDeliveryNotes : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [Column("invoice_number")]
        public string InvoiceNumber { get; set; }

        [Column("client_id")]
        public string ClientId { get; set; }

        [Column("attention")]
        public string Attention { get; set; }

        [Column("received_by")]
        public string ReceivedBy { get; set; }

        [Column("received_on")]
        public DateTime? ReceivedOn { get; set; }

        [Column("delivered_by")]
        public string DeliveredBy { get; set; }

        [Column("delivered_on")]
        public DateTime? DeliveredOn { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("legal_entity_id")]
        public string LegalEntityId { get; set; }

        [Column("remarks")]
        public string Remarks { get; set; }

        [Column("file_master_id")]
        public string FileMasterId { get; set; }
        [NotMapped]
        public string FileMasterName { get; set; }

        [NotMapped]
        public string ClientName { get; set; }

        [NotMapped]
        public string CreatedByName { get; set; }


        [NotMapped]
        public List<AccountReceivable> listAR { get; set; }

        [NotMapped]
        public IFormFile DocFile { get; set; }
    }
}
