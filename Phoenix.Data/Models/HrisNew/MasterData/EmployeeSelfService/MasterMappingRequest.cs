using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("master_mapping_request", Schema = "hr")]
    public class MasterMappingRequest : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(255)]
        [Column("service_name"), StringLength(255)]
        public string ServiceName { get; set; }

        [MaxLength(255)]
        [Column("master_division_selfservice_id"), StringLength(255)]
        public string MasterDivisionSelfserviceId { get; set; }

        [Column("employee_basic_info_id")]
        public string EmployeeBasicInfoId { get; set; }

        [Column(nameof(Remarks)), StringLength(2147483647)]
        public string Remarks { get; set; }
    }
}