namespace GYM.BLL.ViewModels
{
    public class TrainerView
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? Photo { get; set; }
        public string Specialties { get; set; } = string.Empty;
        public string DateOfBirth { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}
