using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("employee_location", Schema = "hr")]
    public class EmployeeLocation : HrisEntityBase
    {
        [MaxLength(100)]
        [Column(nameof(Id)), StringLength(100)]
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column(nameof(EmployeeBasicInfoId)), StringLength(50)]
        public string EmployeeBasicInfoId { get; set; }

        [MaxLength(50)]
        [Column(nameof(LocationId)), StringLength(50)]
        public string LocationId { get; set; }
    }
}