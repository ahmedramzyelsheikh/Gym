using System.ComponentModel.DataAnnotations;

namespace GYM.BLL.ViewModels
{
    public class CreateBookingViewModel
    {
        public int SessionId { get; set; }

        [Required, Range(1, int.MaxValue, ErrorMessage = "Please select a member.")]
        [Display(Name = "Member")]
        public int MemberId { get; set; }
    }
}
