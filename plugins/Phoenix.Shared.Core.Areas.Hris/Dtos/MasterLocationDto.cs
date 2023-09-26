using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{

    public class MasterLocationDto
    {
        [Required]
        public string Id { get; set; }
        public string location_name { get; set; }
        public string map_lat { get; set; }
        public string map_long { get; set; }
        public DateTime? created_on { get; set; }
    }
    public class MasterLocationNew
    {
        public string Id { get; set; }
        public string location_name { get; set; }
        public string map_lat { get; set; }
        public string map_long { get; set; }
    }
    public class MasterLocationSearch : SearchParameter
    {


    }
}
