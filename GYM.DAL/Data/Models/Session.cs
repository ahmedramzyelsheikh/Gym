using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GYM.DAL.Data.Models
{
    public class Session : BaseEntity
    {
        public string Description { get; set; } = default!;
        public int Capacity { get; set; }
        //public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ICollection<Booking> bookings { get; set; } = default!;

        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; } = default!;

        public int CategoryId;

        public Category Category { get; set; } = default!;
     }
}
