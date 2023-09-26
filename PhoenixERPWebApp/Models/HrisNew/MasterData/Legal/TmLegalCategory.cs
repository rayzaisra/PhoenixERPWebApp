using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class TmLegalCategory : EntityBase
    {
        [DisplayName("Id")]
        public Guid Id { get; set; }

        [DisplayName("Legal Category Name")]
        public string LegalCategoryName { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }
    }
}