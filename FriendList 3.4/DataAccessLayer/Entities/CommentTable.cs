using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccessLayer.Entities
{
    public class CommentTable
    {
        [Key]
        public int CID { get; set; }
        public int PID { get; set; }
        public string Comment { get; set; }
        public int CommentBy { get; set; }
    }
}
