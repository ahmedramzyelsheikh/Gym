using GYM.DAL.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GYM.DAL.Data.Models
{
    public  abstract class GymUser  :  BaseEntity
    {
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;

        public string Phone { get; set; } = default!;

        public DateTime DateOfBirth { get; set; } = default!;

        public Gender Gender { get; set; } = default!;

        public Address Address { get; set; } = default!;
    }
    [NotMapped]
     public  class  Address 
    {
        public int BuildingNomber  { get; set; } = default!;
        public string City { get; set; } = default!;
        public string Street { get; set; } = default!;
      
    }
}
