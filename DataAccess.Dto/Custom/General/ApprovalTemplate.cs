using System;
using System.Collections.Generic;
using System.Text;
using CommonTool.QueryBuilder.Model;

namespace DataAccess.PhoenixERP.General
{
    public partial class approval_group_templateDTO
    {
        public approval_templateDTO approval_template { get; set; }
        public JsonRule details { get; set; }

    }

    public class Iapproval_template
    {
        public string approver_id { get; set; }
        public string app_fullname { get; set; }
        public int? approval_level { get; set; }
        public bool is_primary { get; set; }
        public List<Iapproval_template> Backup { get; set; }
    }

    public partial class approval_template_setupDTO 
    {
        public string id { get; set; } 
        public string template_name { get; set; }
        public string description { get; set; }
        public string application_entity_id { get; set; }
        public List<Iapproval_template> templates { get; set; }
    }

    public partial class approval_template_connector {
        public string From { get; set; }
        public string To { get; set; }
        public string Label { get; set; }
    }


}
