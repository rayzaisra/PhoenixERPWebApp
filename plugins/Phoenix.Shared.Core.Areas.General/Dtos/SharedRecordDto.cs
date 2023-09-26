using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.General.Dtos
{
    public class SharedRecordDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string application_entity_id { get; set; }
        public bool can_read { get; set; }
        public bool can_write { get; set; }
        [StringLength(50)]
        public string record_id { get; set; }
        [StringLength(50)]
        public string shared_to { get; set; }
    }
    public class SharedRecordNew
    {
        [StringLength(50)]
        public string application_entity_id { get; set; }
        public bool can_read { get; set; }
        public bool can_write { get; set; }
        [StringLength(50)]
        public string record_id { get; set; }
        [StringLength(50)]
        public string shared_to { get; set; }
    }            
    public class SharedRecordSearch : SearchParameter
    {
        public string Search_application_entity_id { get; set; }
        public bool Search_can_read { get; set; }
        public bool Search_can_write { get; set; }
        public string Search_record_id { get; set; }
        public string Search_shared_to { get; set; }
        
    }
}