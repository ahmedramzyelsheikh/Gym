using System.ComponentModel.DataAnnotations;

namespace GYM.BLL.ViewModels
{
    public class SessionViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public int AvailableSlots { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string TrainerName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // "Upcoming", "Ongoing" or "Completed"
        public string Status { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public string DateDisplay { get; set; } = string.Empty;
        public string TimeRangeDisplay { get; set; } = string.Empty;
    }

    public class CreateSessionViewModel
    {
        [Required, StringLength(200, MinimumLength = 5)]
        public string Description { get; set; } = string.Empty;

        [Required, Range(1, 25, ErrorMessage = "Capacity must be between 1 and 25.")]
        public int Capacity { get; set; } = 10;

        [Required, Range(1, int.MaxValue, ErrorMessage = "Please select a category.")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Required, Range(1, int.MaxValue, ErrorMessage = "Please select a trainer.")]
        [Display(Name = "Trainer")]
        public int TrainerId { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; } = DateTime.Now.Date.AddDays(1).AddHours(10);

        [Required]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; } = DateTime.Now.Date.AddDays(1).AddHours(11);
    }

    public class UpdateSessionViewModel
    {
        public int Id { get; set; }

        [Required, StringLength(200, MinimumLength = 5)]
        public string Description { get; set; } = string.Empty;

        [Required, Range(1, int.MaxValue, ErrorMessage = "Please select a trainer.")]
        [Display(Name = "Trainer")]
        public int TrainerId { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
    }

    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class CreateBookingViewModel
    {
        public int SessionId { get; set; }

        [Required, Range(1, int.MaxValue, ErrorMessage = "Please select a member.")]
        [Display(Name = "Member")]
        public int MemberId { get; set; }
    }

    public class MemberForSessionViewModel
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; } = string.Empty;
        public int SessionId { get; set; }
        public string BookingDate { get; set; } = string.Empty;
        public bool IsAttended { get; set; }
    }
}
