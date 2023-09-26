using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("map_coa_transaksi", Schema = "fn")]
    public class MapCoaTransaksi : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("module_code"), StringLength(50)]
        public string ModuleCode { get; set; }

        [MaxLength(50)]
        [Column("mainservice_category_id"), StringLength(50)]
        public string MainserviceCategoryId { get; set; }

        //[MaxLength(50)]
        //[Column("sharedservice_id"), StringLength(50)]
        //public string SharedserviceId { get; set; }

        [MaxLength(50)]
        [Column("line_code"), StringLength(50)]
        public string LineCode { get; set; }

        [MaxLength(50)]
        [Column(nameof(Description)), StringLength(50)]
        public string Description { get; set; }

        [MaxLength(50)]
        [Column("coa_id"), StringLength(50)]
        public string CoaId { get; set; }

        [MaxLength(50)]
        [Column("d_k"), StringLength(50)]
        public string DK { get; set; }

        [Column("is_asf")]
        public bool? IsAsf { get; set; }

        [MaxLength(250)]
        [Column("name_setting"), StringLength(250)]
        public string NameSetting { get; set; }

        [Column("pce_type_cross_billing")]
        public bool? PceTypeCrossBilling { get; set; }

        [MaxLength(50)]
        [Column("invoice_type"), StringLength(50)]
        public string InvoiceType { get; set; }

        [MaxLength(50)]
        [Column("business_unit_id"), StringLength(50)]
        public string BusinessUnitId { get; set; }

        [NotMapped]
        public string AccountName { get; set; }

        [NotMapped]
        public string CodeRec { get; set; }

        [NotMapped]
        public bool? isEdit { get; set; }
    }
}