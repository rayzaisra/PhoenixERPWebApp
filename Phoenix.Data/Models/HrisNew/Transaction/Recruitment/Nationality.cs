using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models
{
	[Table("nationality", Schema = "hr")]
	public class Nationality : HrisEntityBase
	{
		[Key]
		[MaxLength(50)]
		[Column(nameof(Id)), StringLength(50)]
		public string Id { get; set; }

		[Required]
		[MaxLength(50)]
		[Column("nationality_name"), StringLength(50)]
		public string NationalityName { get; set; }		
	}
}
