using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class TypeOfExpense : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("PCE Type Name")]
        public string Name { get; set; }

        [DisplayName("Status")]
        public string IsActiveName { get { return (IsActive ?? true) ? "Active" : "Inactive"; } }
    }
}