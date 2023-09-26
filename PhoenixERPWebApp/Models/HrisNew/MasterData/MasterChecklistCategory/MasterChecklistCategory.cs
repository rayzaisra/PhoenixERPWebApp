using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PhoenixERPWebApp.Models.HrisNew.MasterData.MasterChecklistCategory
{
    public class MasterChecklistCategory : HrisEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }
        [DisplayName("Checklist Category")]
        public string ChecklistCategory { get; set; }
        [DisplayName("Is Default")]
        public bool Is_Default { get; set; }
        [DisplayName("Is Locked")]
        public bool Is_Locked { get; set; }
    }
}
