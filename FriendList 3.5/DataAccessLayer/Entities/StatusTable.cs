using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccessLayer.Entities
{
    public class StatusTable
    {
        [Key]
        public int SID { get; set; }
        [Required]
        public int RequestBy { get; set; }
        [Required]
        public int RequestTo { get; set; }
        [Required]
        public string Status { get; set; }
    }
}
