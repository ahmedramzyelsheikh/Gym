using System.ComponentModel.DataAnnotations;

namespace GYM.BLL.ViewModels.Sessions
{
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
}
