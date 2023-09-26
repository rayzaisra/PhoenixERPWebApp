using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class GetFrom : FinanceEntityBase
    {

        [DisplayName("ID Get From")]
        public string Id { get; set; }

        [DisplayName("Get From Name")]
        public string GetFromName { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }
    }
}
