using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccessLayer.Entities
{
    public class GroupTable
    {
        [Key]
        public int GID { get; set; }
        public string GroupName { get; set; }
        public int GroupAdmin { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}
