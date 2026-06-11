using System;
using System.Collections.Generic;
using System.Text;

namespace GYM.DAL.Data.Models
{
    public  class HealthRecord :  BaseEntity
    {
        public  decimal Height  { get; set; }
        public decimal Weight { get; set; }
        public string BloodType { get; set; } = default!;
        public string? Note { get; set; }
        public  int MemberId { get; set; }
        public  Member  Member { get; set; } = default!;


        // LastUpdate


    }
}
        
  