using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Phoenix.Data.Models.ViewModel
{
    public class GroupChatVm
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string GroupName { get; set; }
        public List<GroupChatMemberVm> Members { get; set; }
    }
    public class GroupChatMemberVm
    {
        public string GroupId { get; set; }
        [Required]
        public string MemberCode { get; set; }
        public string MemberName { get; set; }
    }


    public class VmEmailMsGraph
    {
        public List<EmailAddressTo> ListEmailAddessTo { get; set; }
        public string Subject { get; set; }
        public string ContentEmail { get; set; }
    }
    public class EmailAddressTo
    {
        public string To { get; set; }
    }

}
