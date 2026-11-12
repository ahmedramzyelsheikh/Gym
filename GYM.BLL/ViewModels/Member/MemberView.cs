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
}
