using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class Organization : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Organization Name")]
        public string OrganizationName { get; set; }

        [DisplayName("Parent Organization Id")]
        public string ParentOrganizationId { get; set; }

        [DisplayName("Is Default Organization")]
        public bool? IsDefaultOrganization { get; set; }

        [DisplayName("Smtp Server")]
        public string SmtpServer { get; set; }

        [DisplayName("Smtp Port")]
        public int? SmtpPort { get; set; }

        [DisplayName("Smtp Username")]
        public string SmtpUsername { get; set; }

        [DisplayName("Smtp Password")]
        public string SmtpPassword { get; set; }

        [DisplayName("Smtp From Address")]
        public string SmtpFromAddress { get; set; }
    }
}