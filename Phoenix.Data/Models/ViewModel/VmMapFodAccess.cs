using System.ComponentModel.DataAnnotations;

namespace Phoenix.Data.Models.ViewModel
{
    public class VmMapFodAccess
    {
        public string Id { get; set; }
        public int RefType { get; set; }
        public string RefDesc { get; set; }
        public string ItemId { get; set; }
        public string RefId { get; set; }
        public string RefName { get; set; }
    }

    public class VmFodUserList
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string AliasName { get; set; }
        public string FullPath { get; set; }
    }

    public class VmFileFodUserList : VmFodUserList
    {
        public string FileName { get; set; }
        public string JobId { get; set; }
    }
    public class VmFodGroupList
    {
        public string Id { get; set; }
        public string GroupName { get; set; }
    }

    public class VmAddFod
    {
        [Required]
        public int RefType { get; set; }
        [Required]
        public string ItemId { get; set; }
        [Required]
        public string RefId { get; set; }
    }

    public class TrainingSchedule
    {
        public string title { get; set; }
        public string start { get; set; }
        public string description { get; set; }
        public string className { get; set; }
        public string end { get; set; }

    }
}
