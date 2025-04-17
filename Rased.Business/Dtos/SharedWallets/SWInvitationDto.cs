using System.ComponentModel.DataAnnotations;

namespace Rased.Business.Dtos.SharedWallets
{
    public class SWInvitationDto
    {
        [Required]
        public string ReceiverEmail { get; set; } = null!;
        [Required]
        public int SWId { get; set; } // Shared Wallet Id
    }

    public class UpdateInvitationDto
    {
        [Required]
        public int SWId { get; set; } // Shared Wallet Id
        public bool Status { get; set; } // true for accept, false for decline
    }

    public class UpdateMemberRoleDto
    {
        public int SWId { get; set; } // Shared Wallet Id
        [Required]
        public string MemberId { get; set; } = null!;
        [Required]
        public string NewRole { get; set; } = null!;
    }

    public class RemoveMemberDto
    {
        [Required]
        public string MemberId { get; set; } = null!;
        [Required]
        public int SWId { get; set; } // Shared Wallet Id
    }
}
