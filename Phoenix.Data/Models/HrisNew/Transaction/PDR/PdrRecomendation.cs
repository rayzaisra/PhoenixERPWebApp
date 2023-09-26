using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("pdr_recomendation", Schema = "hr")]
    public class PdrRecomendation : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("pdr_employee_id"), StringLength(50)]
        public string PdrEmployeeId { get; set; }

        [Column("contract_extended")]
        public int? ContractExtended { get; set; }

        [Column(nameof(Terminated))]
        public bool Terminated { get; set; }

        [Column("effective_permanent")]
        public DateTime? EffectivePermanent { get; set; }

        [MaxLength(50)]
        [Column(nameof(Type)), StringLength(50)]
        public string Type { get; set; }

        //public PdrEmployee PdrEmployee { get; set; }
    }
}