using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class ItemList : EntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        public string type_name { get; set; }

        public string organization_id { get; set; }

        public string ItemCategoryId { get; set; }

        public string ItemTypeId { get; set; }

        public string ItemName { get; set; }

        public string ItemCode { get; set; }

        public string UomId { get; set; }
    }
}