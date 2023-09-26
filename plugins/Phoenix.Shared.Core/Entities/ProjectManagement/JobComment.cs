using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("job_comment", Schema = "pm")]
    public partial class JobComment : GenericEntity<string>
    {
        public string job_number { get; set; }
        public string comment_message { get; set; }
        public DateTime? comment_date { get; set; }
        public string user_id { get; set; }
        public string user_name { get; set; }
    }
}
