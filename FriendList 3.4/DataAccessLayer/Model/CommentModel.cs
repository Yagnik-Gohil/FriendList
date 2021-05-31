using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Model
{
    public class CommentModel
    {
        public int CID { get; set; }
        public string UserName { get; set; }
        public string Comment { get; set; }
    }
}
