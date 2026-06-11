using GYM.DAL.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GYM.DAL.Data.Models
{
    public class Category :  BaseEntity
    {
        public string CategoryName { get; set; } = default!; 

        public ICollection<Session>? sessions { get; set; }
    }
}
