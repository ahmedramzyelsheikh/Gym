using System.ComponentModel.DataAnnotations;
using GYM.DAL.Data.Enums;
using Microsoft.AspNetCore.Http;

namespace GYM.BLL.ViewModels.Trainers
{
    public class CreateTrainerViewModel
    {
        [Required, StringLength(50, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required, RegularExpression(@"^01[0125][0-9]{8}$", ErrorMessage = "Enter a valid Egyptian phone number (e.g. 01012345678).")]
        public string Phone { get; set; } = string.Empty;

        [Required, DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public Gender? Gender { get; set; }

        [Required, Range(1, int.MaxValue, ErrorMessage = "Building number must be greater than 0.")]
        [Display(Name = "Building Number")]
        public int BuildingNumber { get; set; }

        [Required, StringLength(30)]
        public string City { get; set; } = string.Empty;

        [Required, StringLength(30)]
        public string Street { get; set; } = string.Empty;

        [Required(ErrorMessage = "Specialty is required.")]
        [Display(Name = "Specialty")]
        public Specialties? Specialties { get; set; }

        [Display(Name = "Profile Photo")]
        public IFormFile? PhotoFile { get; set; }
    }
}
