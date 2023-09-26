using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class MppApprovedStatusDto
    {
        [Required]
            public string Id { get; set; }
        public DateTime? created_date { get; set; }
        [StringLength(50)]
        public string id_mpp { get; set; }
        [StringLength(2147483647)]
        public string reason { get; set; }
        public DateTime? updated_date { get; set; }
        public string approved_by { get; set; }
        public DateTime? created_on { get; set; }
    }
    public class MppApprovedStatusNew
    {
        public DateTime? created_date { get; set; }
        [StringLength(50)]
        public string id_mpp { get; set; }
        [StringLength(2147483647)]
        public string reason { get; set; }
        public DateTime? updated_date { get; set; }
    }            
    public class MppApprovedStatusSearch : SearchParameter
    {
        public DateTime? Search_created_date { get; set; }
        public string Search_id_mpp { get; set; }
        public string Search_reason { get; set; }
        public DateTime? Search_updated_date { get; set; }
        
    }
}