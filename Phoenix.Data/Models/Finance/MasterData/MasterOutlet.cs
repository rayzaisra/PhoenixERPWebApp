using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("master_outlet", Schema = "fn")]
    public class MasterOutlet : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("client_id"), StringLength(50)]
        public string ClientId { get; set; }

        [MaxLength(50)]
        [Column("outlet_name"), StringLength(50)]
        public string OutletName { get; set; }

        [MaxLength(350)]
        [Column(nameof(Address)), StringLength(350)]
        public string Address { get; set; }

        [MaxLength(50)]
        [Column("country_id"), StringLength(50)]
        public string CountryId { get; set; }

        [MaxLength(50)]
        [Column("province_id"), StringLength(50)]
        public string ProvinceId { get; set; }

        [MaxLength(50)]
        [Column("city_id"), StringLength(50)]
        public string CityId { get; set; }

        [MaxLength(50)]
        [Column("zip_code"), StringLength(50)]
        public string ZipCode { get; set; }

        [MaxLength(50)]
        [Column(nameof(Phone)), StringLength(50)]
        public string Phone { get; set; }

        [MaxLength(50)]
        [Column("mobile_phone"), StringLength(50)]
        public string MobilePhone { get; set; }

        [MaxLength(10)]
        [Column(nameof(Email)), StringLength(10)]
        public string Email { get; set; }

        [MaxLength(150)]
        [Column("pic_name"), StringLength(150)]
        public string PicName { get; set; }

        [MaxLength(50)]
        [Column("type_outlet"), StringLength(50)]
        public string TypeOutlet { get; set; }

        [NotMapped]
        public string ClientName { get; set; }
        [NotMapped]
        public string CountryName { get; set; }

        [NotMapped]
        public string ProvinceName { get; set; }

        [NotMapped]
        public string CityName { get; set; }

    }
}