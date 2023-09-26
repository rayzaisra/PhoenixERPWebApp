using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("reconcile", Schema = "fn")]
    public class Reconcile : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [Column("period")]
        public DateTime? Period { get; set; }

        [MaxLength(50)]
        [Column("reference_id"), StringLength(50)]
        public string ReferenceId { get; set; }

        [MaxLength(50)]
        [Column("notes"), StringLength(50)]
        public string Notes { get; set; }

        [MaxLength(50)]
        [Column("status"), StringLength(50)]
        public string Status { get; set; }
    }


    public class ReconcileList
    {
        public string Id { get; set; }
        public DateTime? PaymentRequestDate { get; set; }
        public string PaymentRequestNumber { get; set; }
        public string ReferenceNumberId { get; set; }
        public string LegalEntityName { get; set; }
        public string VendorName { get; set; }
        public string JobName { get; set; }
        public string BusinessUnitName { get; set; }
        public string PaymentTypeName { get; set; }
        public string CurrencyName { get; set; }
        public decimal? AmountPay { get; set; }
        public string Status { get; set; }
        public string ReconcileId { get; set; }
        public string AccountName { get; set; }
        public string AccountNo { get; set; }
        public DateTime? Period { get; set; }
        public string Notes { get; set; }
    }

    public class ReconcileSearch
    {
        public string LegalEntityId { get; set; }

        //public DateTime? PeriodeFrom { get; set; }

        //public DateTime? PeriodeTo { get; set; }

        public string Type { get; set; }

        public string ReferenceNo { get; set; }

        //public string BusinessUnitId { get; set; }

        //public int Level { get; set; }

        public string AccountId { get; set; }
    }
}