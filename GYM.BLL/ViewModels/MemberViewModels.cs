using System.ComponentModel.DataAnnotations;
using GYM.DAL.Data.Enums;
using Microsoft.AspNetCore.Http;

namespace GYM.BLL.ViewModels
{
    public class MemberView
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? Photo { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string DateOfBirth { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        // Filled on the details page when the member has an active membership.
        public string? MembershipStartDate { get; set; }
        public string? MembershipEndDate { get; set; }
    }

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

    public class CreateMemberViewModel
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

        [Required(ErrorMessage = "Profile photo is required.")]
        [Display(Name = "Profile Photo")]
        public IFormFile? PhotoFile { get; set; }

        public HealthRecordViewModel HealthRecordViewModel { get; set; } = new();
    }

    public class MemberToUpdateViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required, RegularExpression(@"^01[0125][0-9]{8}$", ErrorMessage = "Enter a valid Egyptian phone number (e.g. 01012345678).")]
        public string Phone { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        public Gender Gender { get; set; }

        [Required, Range(1, int.MaxValue, ErrorMessage = "Building number must be greater than 0.")]
        [Display(Name = "Building Number")]
        public int BuildingNumber { get; set; }

        [Required, StringLength(30)]
        public string City { get; set; } = string.Empty;

        [Required, StringLength(30)]
        public string Street { get; set; } = string.Empty;

        public string? Photo { get; set; }

        [Display(Name = "Profile Photo")]
        public IFormFile? PhotoFile { get; set; }
    }
}
