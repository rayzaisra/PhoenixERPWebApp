using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("legal_entity", Schema = "dbo")]
    public class LegalEntity : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("registration_code"), StringLength(50)]
        public string RegistrationCode { get; set; }

        [Required]
        [MaxLength(400)]
        [Column("legal_entity_name"), StringLength(400)]
        public string LegalEntityName { get; set; }

        [MaxLength(50)]
        [Column(nameof(Website)), StringLength(50)]
        public string Website { get; set; }

        [MaxLength(50)]
        [Column("director_name"), StringLength(50)]
        public string DirectorName { get; set; }

        [MaxLength(50)]
        [Column("npwp_number"), StringLength(50)]
        public string NPWPNumber { get; set; }

        [MaxLength(50)]
        [Column("office_country_id"), StringLength(50)]
        public string OfficeCountryId { get; set; }

        [MaxLength(50)]
        [Column("office_province_id"), StringLength(50)]
        public string OfficeProvinceId { get; set; }

        [MaxLength(50)]
        [Column("office_city_id"), StringLength(50)]
        public string OfficeCityId { get; set; }

        [MaxLength(350)]
        [Column("office_address"), StringLength(350)]
        public string OfficeAddress { get; set; }

        [MaxLength(50)]
        [Column("office_zip_code"), StringLength(50)]
        public string OfficeZipCode { get; set; }

        [Column("is_from_office_address")]
        public bool? IsFromOfficeAddress { get; set; }

        [MaxLength(50)]
        [Column("billing_country_id"), StringLength(50)]
        public string BillingCountryId { get; set; }

        [MaxLength(50)]
        [Column("billing_province_id"), StringLength(50)]
        public string BillingProvinceId { get; set; }

        [MaxLength(50)]
        [Column("billing_city_id"), StringLength(50)]
        public string BillingCityId { get; set; }

        [MaxLength(350)]
        [Column("billing_address"), StringLength(350)]
        public string BillingAddress { get; set; }

        [MaxLength(50)]
        [Column("billing_zip_code"), StringLength(50)]
        public string BillingZipCode { get; set; }

        [MaxLength(50)]
        [Column("image_tax_scan"), StringLength(50)]
        public string ImageTaxScan { get; set; }

        [MaxLength(50)]
        [Column("image_legal_scan"), StringLength(50)]
        public string ImageLegalScan { get; set; }

        [MaxLength(50)]
        [Column("contact_name"), StringLength(50)]
        public string ContactName { get; set; }

        [MaxLength(50)]
        [Column(nameof(Phone)), StringLength(50)]
        public string Phone { get; set; }

        [MaxLength(50)]
        [Column("mobile_phone"), StringLength(50)]
        public string MobilePhone { get; set; }

        [MaxLength(50)]
        [Column(nameof(Fax)), StringLength(50)]
        public string Fax { get; set; }

        [MaxLength(50)]
        [Column(nameof(Email)), StringLength(50)]
        public string Email { get; set; }

        [MaxLength(50)]
        [Column(nameof(Twitter)), StringLength(50)]
        public string Twitter { get; set; }

        [MaxLength(50)]
        [Column(nameof(Facebook)), StringLength(50)]
        public string Facebook { get; set; }

        [MaxLength(50)]
        [Column(nameof(Instagram)), StringLength(50)]
        public string Instagram { get; set; }


        [NotMapped]
        public string OfficeCountryName { get; set; }

        [NotMapped]
        public string OfficeProvinceName { get; set; }

        [NotMapped]
        public string OfficeCityName { get; set; }

        [NotMapped]
        public IFormFile fileImageTaxScan { get; set; }
        [NotMapped]
        public IFormFile fileImageLegalScan { get; set; }
        [NotMapped]
        public string ImageTaxScanName { get; set; }
        [NotMapped]
        public string ImageLegalScanName { get; set; }
    }
}