using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccessLayer.Entities
{
    public class GroupManagerTable
    {
        [Key]
        public int GMID { get; set; }
        public int UserID { get; set; }
        public int GroupID { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsMember { get; set; }
        public int InvitedBy { get; set; }
    }
}
