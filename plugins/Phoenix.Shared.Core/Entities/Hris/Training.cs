using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("training",Schema ="hr")]
    public partial class Training : GenericEntity<string>
    {
        public string name { get; set; }
        public string category { get; set; }
        public string subject { get; set; }
        public int training_cost { get; set; }
        public string training_type { get; set; }
        public string bonding_detail { get; set; }
		public string recruitment { get; set; }
		public string description { get; set; }
		public string target_audiance { get; set; }
		
    }
}