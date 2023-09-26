using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models.ViewModel
{
    [Table("Vw_MappingBussinessUnitLevelCompletely", Schema = "dbo")]
    public class MappingBusinessUnit
    {
        [Key]
        [Column("IdGroup")]
        public string IdGroup { get; set; }

        [Column("IdSubGroup")]
        public string IdSubgroup { get; set; }

        [Column("IdDivisi")]
        public string IdDivisi { get; set; }

        [Column("IdDepartment")]
        public string IdDepartment { get; set; }

        [Column("GroupName")]
        public string GroupName { get; set; }

        [Column("SubGroupName")]
        public string SubGroupName { get; set; }

        [Column("DivisiName")]
        public string DivisiName { get; set; }

        [Column("DepartmentName")]
        public string DepartmentName { get; set; }

    }

    public class MaapingGroupDto {
        public string IdGroup { get; set; }
        public string GroupName { get; set; }
    }

    public class MaapingSubgroupDto
    {
        public string IdGroup { get; set; }
        public string GroupName { get; set; }
        public string IdSubgroup { get; set; }
        public string SubGroupName { get; set; }
    }

    public class MaapingDisivionDto
    {
        public string IdGroup { get; set; }
        public string GroupName { get; set; }
        public string IdSubgroup { get; set; }
        public string SubGroupName { get; set; }
        public string IdDivisi { get; set; }
        public string DivisiName { get; set; }
    }
}
