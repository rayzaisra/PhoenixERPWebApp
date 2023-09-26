using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("married_status", Schema = "hr")]
	public class MarriedStatus : HrisEntityBase
	{
		[Key]
		[MaxLength(50)]
		[Column(nameof(Id)), StringLength(50)]
		[DisplayName("Id")]
		public string Id { get; set; }

		[MaxLength(100)]
		[Column("married_name"), StringLength(100)]
		[DisplayName("Married Name")]
		public string MarriedName { get; set; }
	}
}