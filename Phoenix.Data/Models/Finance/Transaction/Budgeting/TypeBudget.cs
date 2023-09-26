using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models.Finance
{
    [Table("type_budget", Schema = "fn")]
    public class TypeBudget : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [Column("financialyear")]
        public int? financialyear { get; set; }

        [Column("type_budget_hdr_id")]
        public string type_budget_hdr_id { get; set; }

        [MaxLength(50)]
        [Column("bulan")]
        public string bulan { get; set; }

        [Column("revisi")]
        public int? revisi { get; set; }

        [MaxLength(200)]
        [Column("accountname")]
        public string accountname { get; set; }

        [Column("skor")]
        public decimal? skor { get; set; }

        [Column("interface")]
        public decimal? Interface { get; set; }

        [Column("tm_advis")]
        public decimal? tm_advis { get; set; }

        [Column("brandcoma")]
        public decimal? brandcoma { get; set; }

        [Column("brandcomb")]
        public decimal? brandcomb { get; set; }

        [Column("marketingservicesampling")]
        public decimal? marketingservicesampling { get; set; }

        [Column("marketingservicerm")]
        public decimal? marketingservicerm { get; set; }

        [Column("pr")]
        public decimal? pr { get; set; }

        [Column("pathfinder")]
        public decimal? pathfinder { get; set; }
        [Column("totaliris")]
        public decimal? totaliris { get; set; }
        [Column("bodcentral")]
        public decimal? bodcentral { get; set; }
        [Column("totalgroup")]
        public decimal? totalgroup { get; set; }
        [Column("organization_id")]
        public string organization_id { get; set; }

        [Column("is_upload")]
        public string is_upload { get; set; }
        


    }
}
