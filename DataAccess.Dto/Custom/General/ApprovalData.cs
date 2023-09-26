using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Dto.Custom.General
{
    public partial class approval_data
    {
        public string approval_status_id;

        public List<string> approval_ids;
    }

    public partial class approval_detail_data_by_groupDTO
    {
        public string approval_id;
        public int? approval_level;
        public List<approval_detail_dataDTO> details;
    }

    public partial class approval_detail_data_by_group_idDTO
    {
        public string approval_id;
        public List<approval_detail_data_by_group_levelDTO> data;
    }



    public partial class approval_detail_data_by_group_levelDTO
    {
        public int? approval_level;
        public List<approval_detail_dataDTO> details;
    }


    public partial class approval_detail_dataDTO
    {
        public string id { get; set; }
        public string previous_approval { get; set; }
        public int? approval_level { get; set; }
        public DateTime? expiry_datetime { get; set; }
        public bool? is_auto_approved { get; set; }
        public string approver_id { get; set; }
        public string app_fullname { get; set; }
        public string approval_status_id { get; set; }
        public string status_name { get; set; }
        public string approval_id { get; set; }
    }

}
