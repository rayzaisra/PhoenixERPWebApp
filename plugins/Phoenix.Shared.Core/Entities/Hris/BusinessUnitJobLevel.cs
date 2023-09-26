using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("business_unit_job_level", Schema ="hr")]
    public partial class BusinessUnitJobLevel : GenericEntity<string>
    {
        public string business_unit_id { get; set; }
        public string job_grade_id { get; set; }
        public string job_level_id { get; set; }
        public string parent_id { get; set; }
        public string job_title_id { get; set; }
        public string business_unit_group_id { get; set; }
        public string business_unit_subgroup_id { get; set; }
        public string business_unit_division_id { get; set; }
        public string business_unit_departement_id { get; set; }

        public string remarks { get; set; }
    }
}