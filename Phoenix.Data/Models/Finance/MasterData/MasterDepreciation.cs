using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models
{
    [Table("master_depreciation", Schema = "fn")]
    public class MasterDepreciation : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("depreciation_name"), StringLength(50)]
        public string DepreciationName { get; set; }

        [MaxLength(50)]
        [Column("status"), StringLength(50)]
        public string status { get; set; }

        [MaxLength(50)]
        [Column("legal_entity_id"), StringLength(50)]
        public string LegalEntityId { get; set; }
    }
}
