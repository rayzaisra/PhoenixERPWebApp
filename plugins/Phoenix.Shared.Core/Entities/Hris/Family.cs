using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("family",Schema ="hr")]
    public partial class Family : GenericEntity<string>
    {
        public string address { get; set; }
        public DateTime? birthday { get; set; }
        public string description { get; set; }
        public string employee_basic_info_id { get; set; }
        public string family_status_id { get; set; }
        public string gender { get; set; }
        public string institution_id { get; set; }
        public string married_status_id { get; set; }
        public string name { get; set; }
        public string other_no { get; set; }
        public string phone_no { get; set; }
        public string place_birth { get; set; }
        public string religion_id { get; set; }
        public string education_level_id { get; set; }
    }
}