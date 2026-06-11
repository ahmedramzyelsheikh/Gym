using System;
using System.Collections.Generic;
using System.Text;

namespace GYM.DAL.Data.Models
{
    public class Member :  GymUser
    {
        public string? Photo { get; set;  }
        public ICollection<Membership> memberships { get; set; } = default!;
        public ICollection<Booking> Bookings { get; set; } = default!;
        public HealthRecord HealthRecord { get; set; } = default!; 
    }
}
