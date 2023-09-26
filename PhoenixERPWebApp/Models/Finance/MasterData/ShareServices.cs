using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class ShareServices : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Share Service Name")]
        public string ShareserviceName { get; set; }

        [DisplayName("Detail Share Services")]
        public string DetailShareservices { get; set; }

        [DisplayName("Main Service Category")]
        public string MainserviceCategoryId { get; set; }

        [DisplayName("Main Service Category")]
        public string MainserviceCategoryName { get; set; }
    }
}