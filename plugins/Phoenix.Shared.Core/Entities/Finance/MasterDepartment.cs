using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities.Finance
{
    [Table("master_department", Schema = "fn")]
    public partial class MasterDepartment : BaseEntity<string>
    {
        public string division_id { get; set; }
        public string name { get; set; }
        public bool is_deleted { get; set; }
    }
}
