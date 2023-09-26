using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
	public class MasterTrainingRequest : HrisEntityBase
	{
		[DisplayName("Id")]
		public string Id { get; set; }

		[DisplayName("Request Name")]
		public string RequestName { get; set; }

		[DisplayName("Responsibility")]
		public string Responsibility { get; set; }

		[DisplayName("Date")]
		public bool? IsDate { get; set; }
	}
}
