using GYM.DAL.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GYM.DAL.Data.Models
{
    public class Trainer :GymUser  
    {
     

        public Specialties Specialtiy { get; set; } = default!; 
        public ICollection<Session>? Sessions { get; set;}
 


    }
}
