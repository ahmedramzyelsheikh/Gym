using GYM.DAL.Data.Models;

namespace GYM.BLL.Mapping.Common
{
    // Small formatting helpers shared by the mapping profiles.
    public static class MappingHelpers
    {
        public static string GetStatus(DateTime start, DateTime end)
        {
            var now = DateTime.Now;
            if (now < start) return "Upcoming";
            if (now <= end) return "Ongoing";
            return "Completed";
        }

        public static string FormatDuration(TimeSpan duration)
        {
            if (duration.TotalMinutes <= 0) return "0m";
            var hours = (int)duration.TotalHours;
            return hours > 0
                ? (duration.Minutes > 0 ? $"{hours}h {duration.Minutes}m" : $"{hours}h")
                : $"{duration.Minutes}m";
        }

        public static string FormatAddress(Address? address)
            => address is null ? string.Empty : $"{address.BuildingNomber} {address.Street}, {address.City}";
    }
}
