using System;
using System.Collections.Generic;
using System.Security.Cryptography.Pkcs;
using System.Text;

namespace GYM.DAL.Data.Models
{
    public class Booking :BaseEntity
    {

       public  DateTime BookingDate { get; set; }    
       public bool IsAttended { get; set; }
        public int MemberId { get; set; }
        public Member Member { get; set; } = default!; 
        public int SessionId { get; set; }
        public Session Session { get; set; } = default!;
    }
}
