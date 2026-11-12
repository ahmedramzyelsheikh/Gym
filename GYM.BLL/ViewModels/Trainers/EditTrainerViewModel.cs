using System.ComponentModel.DataAnnotations;
using GYM.DAL.Data.Enums;
using Microsoft.AspNetCore.Http;

namespace GYM.BLL.ViewModels.Trainers
{
    public class EditTrainerViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required, RegularExpression(@"^01[0125][0-9]{8}$", ErrorMessage = "Enter a valid Egyptian phone number (e.g. 01012345678).")]
        public string Phone { get; set; } = string.Empty;

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

        public string? Photo { get; set; }

        [Display(Name = "Profile Photo")]
        public IFormFile? PhotoFile { get; set; }
    }
}
