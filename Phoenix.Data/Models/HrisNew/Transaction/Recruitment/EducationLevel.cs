using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models
{
	[Table("education_level", Schema = "hr")]
	public class EducationLevel : HrisEntityBase
	{
		[Key]
		[MaxLength(50)]
		[Column(nameof(Id)), StringLength(50)]
		public string Id { get; set; }

		[MaxLength(100)]
		[Column("education_name"), StringLength(100)]
		public string EducationName { get; set; }		
	}
}
