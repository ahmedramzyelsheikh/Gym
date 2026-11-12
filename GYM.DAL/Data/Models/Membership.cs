using System;
using System.Collections.Generic;
using System.Text;

namespace GYM.DAL.Data.Models
{
    public class Membership : BaseEntity
    {

        public  int MemberId { get; set; }
        public Member Member { get; set; } = default!;

        public  int PlanId { get; set; }
        public Plan Plan { get; set; } = default!;

        // Start date is CreateAt (mapped to the "Start Date" column).
        public DateTime EndDate { get; set; }
    }
}
