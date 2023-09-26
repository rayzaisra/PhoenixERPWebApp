using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models.Finance.Master
{
    [Table("chartofaccount", Schema = "fn")]
    public class ChartOfAccount : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(20)]
        [Column("code_rec"), StringLength(20)]
        public string CodeRec { get; set; }

        [MaxLength(50)]
        [Column(nameof(Name1)), StringLength(50)]
        public string Name1 { get; set; }

        [MaxLength(50)]
        [Column(nameof(Name2)), StringLength(50)]
        public string Name2 { get; set; }

        [MaxLength(50)]
        [Column(nameof(Name3)), StringLength(50)]
        public string Name3 { get; set; }

        [MaxLength(50)]
        [Column(nameof(Name4)), StringLength(50)]
        public string Name4 { get; set; }

        [MaxLength(80)]
        [Column(nameof(Name5)), StringLength(80)]
        public string Name5 { get; set; }

        [MaxLength(3)]
        [Column(nameof(Level)), StringLength(3)]
        public string Level { get; set; }

        [MaxLength(3)]
        [Column("group_coa"), StringLength(3)]
        public string Group { get; set; }

        [MaxLength(15)]
        [Column(nameof(Condition)), StringLength(15)]
        public string Condition { get; set; }

        [MaxLength(15)]
        [Column(nameof(Report)), StringLength(15)]
        public string Report { get; set; }

        [MaxLength(50)]
        [Column("reff_trx_code"), StringLength(50)]
        public string ReffTrxCode { get; set; }

        [MaxLength(50)]
        [Column("reff_trx_id"), StringLength(50)]
        public string ReffTrxId { get; set; }

        [MaxLength(50)]
        [Column("chartofaccount_report_id"), StringLength(50)]
        public string ChartofaccountReportId { get; set; }

        [NotMapped]
        public string CodeRec1 { get; set; }
        [NotMapped]
        public string CodeRec2 { get; set; }
        [NotMapped]
        public string CodeRec3 { get; set; }
        [NotMapped]
        public string CodeRec4 { get; set; }
    }
}
