using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class LoanCategory : HrisEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Interest per Month (%)")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal InterestMonth { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }
       
        public bool? is_Corporate_Loan { get; set; }
        [DisplayName("Jenis Pinjaman")]
        public string IsCorporateLoan { get; set; }

        public string CorporateLoanTypeDesc { get; set; }
    }
}