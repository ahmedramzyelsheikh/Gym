using System.ComponentModel.DataAnnotations;

namespace GYM.BLL.ViewModels.Sessions
{
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
}
