using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccessLayer.Entities
{
    public class GroupPostTable
    {
        [Key]
        public int GPID { get; set; }
        public int PostedIn { get; set; }
        public int PostedBy { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}
