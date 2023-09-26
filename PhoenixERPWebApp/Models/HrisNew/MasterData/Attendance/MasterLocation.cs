using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class MasterLocation : HrisEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Location Name")]
        public string LocationName { get; set; }

        [DisplayName("Latitude")]
        public string MapLat { get; set; }

        [DisplayName("Longitude")]
        public string MapLong { get; set; }
    }
}