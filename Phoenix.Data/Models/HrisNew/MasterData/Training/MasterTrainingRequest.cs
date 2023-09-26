using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models.HrisNew.MasterData.Training
{
	[Table("master_training_request", Schema = "hr")]
	public class MasterTrainingRequest : HrisEntityBase
	{
		[Key]
		[MaxLength(50)]
		[Column(nameof(Id)), StringLength(50)]
		public string Id { get; set; }

		[MaxLength(200)]
		[Column("request_name"), StringLength(200)]
		public string RequestName { get; set; }

		[MaxLength(15)]
		[Column(nameof(Responsibility)), StringLength(15)]
		public string Responsibility { get; set; }

		[Column("is_date")]
		public bool? IsDate { get; set; }
	}
}
