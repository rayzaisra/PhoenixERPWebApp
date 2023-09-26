using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table(nameof(Religion), Schema = "hr")]
    public class Religion : HrisEntityBase
    {
		[Key]
		[MaxLength(50)]
		[Column(nameof(Id)), StringLength(50)]
		[DisplayName("Id")]
		public string Id { get; set; }

		[Required]
		[MaxLength(50)]
		[Column("religion_name"), StringLength(50)]
		[DisplayName("Religion Name")]
		public string ReligionName { get; set; }
	}
}