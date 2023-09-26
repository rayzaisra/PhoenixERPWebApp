using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class StatusAssignedDto
    {
        [Required]
        public string Id { get; set; }
        [StringLength(50)]
        public string status_assigned { get; set; }
    }
    public class StatusAssignedNew
    {
        [StringLength(50)]
        public string status_assigned { get; set; }
    }
    public class StatusAssignedSearch : SearchParameter
    {
        public string Search_status_assigned { get; set; }

    }

    #region AddonService
    public class SetStatusFileDto
    {
        public string FileId { get; set; }
        public string FileName { get; set; }
        public string DamId { get; set; }
        public string DamName { get; set; }
        public string DamTypeId { get; set; }
        public string DamTypeName { get; set; }
    }

    public class SetStatusDetailDto
    {
        public string Id { get; set; }
        public string JobNumber { get; set; }
        public string JobName { get; set; }
        public string ClientName { get; set; }
        public string CampaignName { get; set; }
        public IList<SetStatusFileDto> ListFileDAM { get; set; }
    }

    public class SetStatusResponseDto : SetStatusDetailDto
    {
        public bool UpdateDataResult { get; set; }
    }

    public class SetStatusParameterDto : SetStatusDetailDto
    {
        public string SetStatusId { get; set; }
    }

    #endregion

}