using System.ComponentModel.DataAnnotations;

namespace GYM.BLL.ViewModels
{
    public class PlanView
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Description { get; set; } = string.Empty;

        [Required, Range(1, 365, ErrorMessage = "Duration must be between 1 and 365 days.")]
        [Display(Name = "Duration (days)")]
        public int DurationDays { get; set; }

        [Required, Range(0.01, 100000, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;
    }

    public class MemberShipViewModel
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public string MemberName { get; set; } = string.Empty;
        public string PlanName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

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
