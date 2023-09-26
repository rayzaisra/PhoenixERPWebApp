using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("seminar",Schema ="hr")]
    public partial class Seminar : GenericEntity<string>
    {
        public string competence_id { get; set; }
        public string contact_no { get; set; }
        public string cover_seminar { get; set; }
        public DateTime? end_date { get; set; }
        public string person_in_charge { get; set; }
        public string seminar_address { get; set; }
        public DateTime? start_date { get; set; }
    }
}