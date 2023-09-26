using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("mpp_detail",Schema ="hr")]
    public partial class MppDetail : GenericEntity<string>
    {
        public string employment_status { get; set; }
        public string job_tittle_id { get; set; }
        public string job_grade_id { get; set; }
        public decimal? mpp_budged { get; set; }
        public string mpp_id { get; set; }
        public string ttf { get; set; }
        public string status { get; set; }
        [Column("description")]
        public string Description { get; set; }
        public string project_name { get; set; }
        public string budget_souce { get; set; }
    }
}