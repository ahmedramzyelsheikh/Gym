using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace GYM.DAL.Data.Models
{
    public   abstract class BaseEntity
    {
        public  int Id { get; set; }
        #region  Audit Fields
        public DateTime CreateAt { get; set; }
        public  DateTime UpdateAt { get; set; }
        #endregion
    }
}
