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
}
