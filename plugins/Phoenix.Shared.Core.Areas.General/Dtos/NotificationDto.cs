using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.General.Dtos
{
    public class NotificationDto
    {

        public string Id { get; set; }
        public string href { get; set; }
        public string shorttext { get; set; }
        public string longtext { get; set; }
        public string notification_type_id { get; set; }
        public string item_id { get; set; }
        public bool is_seen { get; set; }
        public DateTime? created_on { get; set; }

        public string date_on { get; set; }
        public string CreatedBy { get; set; }
        public string Module { get; set; }
    }
    public class NotificationFilter:SearchParameter
    {
        public bool search_is_seen { get; set; }
    }
}