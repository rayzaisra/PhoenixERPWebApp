using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("mapping_jurnal", Schema = "fn")]
    public class MappingJurnal : HrisEntityBase
    {
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("employee_division_id"), StringLength(50)]
        public string EmployeeDivisionId { get; set; }

        [MaxLength(50)]
        [Column("employee_depertment_id"), StringLength(50)]
        public string EmployeeDepertmentId { get; set; }

        [MaxLength(50)]
        [Column(nameof(Debit)), StringLength(50)]
        public string Debit { get; set; }

        [MaxLength(50)]
        [Column(nameof(Credit)), StringLength(50)]
        public string Credit { get; set; }

        [MaxLength(50)]
        [Column("prosess_name"), StringLength(50)]
        public string ProsessName { get; set; }
    }
}