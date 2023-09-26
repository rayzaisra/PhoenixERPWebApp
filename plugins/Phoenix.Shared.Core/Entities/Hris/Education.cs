using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("education",Schema ="hr")]
    public partial class Education : GenericEntity<string>
    {
        public string address { get; set; }
        public string education_level_id { get; set; }
        public string employee_basic_info_id { get; set; }
        public string end_education { get; set; }
        public string institution_id { get; set; }
        public string majors { get; set; }
        public string start_education { get; set; }
        public string qualification { get; set; }
    }
}