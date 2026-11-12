using System.ComponentModel.DataAnnotations;

namespace GYM.BLL.ViewModels
{
    public class CreateMemberShipViewModel
    {
        [Required, Range(1, int.MaxValue, ErrorMessage = "Please select a member.")]
        [Display(Name = "Member")]
        public int MemberId { get; set; }

        [Required, Range(1, int.MaxValue, ErrorMessage = "Please select a plan.")]
        [Display(Name = "Plan")]
        public int PlanId { get; set; }
    }
}
