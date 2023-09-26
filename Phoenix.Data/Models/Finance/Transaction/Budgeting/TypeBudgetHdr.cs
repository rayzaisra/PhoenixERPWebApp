using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models.Finance
{
    [Table("type_budget_hdr", Schema = "fn")]
    public class TypeBudgetHdr : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("employee_basic_info_id"), StringLength(50)]
        public string employee_basic_info_id { get; set; }

        [NotMapped]
        public string employee_name { get; set; }

        [Column("financial_year_id")]
        public string financial_year_id { get; set; }

        [Required]
        [Column("financialyear")]
        public int? financialyear { get; set; }

        [Required]
        [Column("bulan")]
        public string bulan { get; set; }

        [Column("revisi")]
        public int? revision { get; set; }

        [Column("upload_date")]
        public DateTime? upload_date { get; set; }

        [Column("organization_id")]
        public string organization_id { get; set; }
        
    }
}
