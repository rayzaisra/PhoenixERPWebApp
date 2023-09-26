using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("warehouse", Schema = "dbo")]
    public class Warehouse : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }
        [Column("organization_id")]
        public string OrganizationId { get; set; }
        [Column("warehouse_name")]
        public string WarehouseName { get; set; }
        [Column("country_id")]
        public string CountryId{ get; set; }
        [Column("city_id")]
        public string CityId { get; set; }
        [Column("address")]
        public string Address { get; set; }
        [Column("zip_code")]
        public string ZipCode { get; set; }
        [Column("phone")]
        public string Phone { get; set; }
        [Column("fax")]
        public string Fax { get; set; }
        [Column("manager_id")]
        public string ManagerId { get; set; }

    }
}
