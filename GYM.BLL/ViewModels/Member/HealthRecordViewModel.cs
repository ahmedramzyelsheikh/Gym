using System.ComponentModel.DataAnnotations;

namespace GYM.BLL.ViewModels
{
    public class HealthRecordViewModel
    {
        [Required, Range(50, 250, ErrorMessage = "Height must be between 50 and 250 cm.")]
        [Display(Name = "Height (cm)")]
        public decimal Height { get; set; }

        [Required, Range(20, 300, ErrorMessage = "Weight must be between 20 and 300 kg.")]
        [Display(Name = "Weight (kg)")]
        public decimal Weight { get; set; }

        [Required(ErrorMessage = "Blood type is required.")]
        [Display(Name = "Blood Type")]
        public string BloodType { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Note { get; set; }
    }
}
