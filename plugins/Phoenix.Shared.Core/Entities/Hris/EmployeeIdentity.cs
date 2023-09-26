using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("employee_identity",Schema ="hr")]
    public partial class EmployeeIdentity : GenericEntity<string>
    {
        public string employee_basic_info_id { get; set; }
        public DateTime? expired { get; set; }
        public string file_upload { get; set; }
        public string name_card { get; set; }
        public string no_card { get; set; }
        public DateTime? published { get; set; }
        public string published_by { get; set; }
    }
}