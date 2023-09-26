using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models.Pm.MasterData
{
    public class MasterJobStatus : PmEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }
        public string name { get; set; }
        [DisplayName("Status Name")]
        public string StatusName { get; set; }
        [DisplayName("Status Order")]
        public string StatusOrder { get; set; }
        [DisplayName("Is Default")]
        public bool Is_Default { get; set; }
        [DisplayName("Is Locked")]
        public bool Is_Locked { get; set; }
        [DisplayName("Is showing on timesheet")]
        public bool? is_showing_on_timesheet { get; set; }

    }
}
