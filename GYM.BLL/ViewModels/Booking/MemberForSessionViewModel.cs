namespace GYM.BLL.ViewModels
{
    public class MemberForSessionViewModel
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; } = string.Empty;
        public int SessionId { get; set; }
        public string BookingDate { get; set; } = string.Empty;
        public bool IsAttended { get; set; }
    }
}
