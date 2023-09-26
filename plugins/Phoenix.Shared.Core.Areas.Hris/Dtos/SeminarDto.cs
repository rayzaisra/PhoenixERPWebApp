using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class SeminarDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        [Required]
        public string competence_id { get; set; }
        [StringLength(50)]
        [Required]
        public string contact_no { get; set; }
        [StringLength(100)]
        [Required]
        public string cover_seminar { get; set; }
        [Required]
        public DateTime? end_date { get; set; }
        [StringLength(50)]
        [Required]
        public string person_in_charge { get; set; }
        [StringLength(2147483647)]
        [Required]
        public string seminar_address { get; set; }
        [Required]
        public DateTime? start_date { get; set; }
    }
    public class SeminarNew
    {
        [StringLength(50)]
        [Required]
        public string competence_id { get; set; }
        [StringLength(50)]
        [Required]
        public string contact_no { get; set; }
        [StringLength(100)]
        [Required]
        public string cover_seminar { get; set; }
        [Required]
        public DateTime? end_date { get; set; }
        [StringLength(50)]
        [Required]
        public string person_in_charge { get; set; }
        [StringLength(2147483647)]
        [Required]
        public string seminar_address { get; set; }
        [Required]
        public DateTime? start_date { get; set; }
    }            
    public class SeminarSearch : SearchParameter
    {
        public string Search_competence_id { get; set; }
        public string Search_contact_no { get; set; }
        public string Search_cover_seminar { get; set; }
        public DateTime? Search_end_date { get; set; }
        public string Search_person_in_charge { get; set; }
        public string Search_seminar_address { get; set; }
        public DateTime? Search_start_date { get; set; }
        
    }
}