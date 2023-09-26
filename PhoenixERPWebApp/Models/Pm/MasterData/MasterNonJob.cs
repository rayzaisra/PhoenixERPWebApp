using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models.Pm.MasterData
{
    public class MasterNonJob : PmEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }
        [DisplayName("Non Job Name")]
        public string name { get; set; }
        [DisplayName("Description")]
        public string description { get; set; }
        [DisplayName("Business Unit")]
        public string BusinessUnitName { get; set; }
        [DisplayName("Is Default")]
        public bool Is_Default { get; set; }
        [DisplayName("Is Locked")]
        public bool Is_Locked { get; set; }


    }
}
