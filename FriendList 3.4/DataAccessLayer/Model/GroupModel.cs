using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Model
{
    public class GroupModel
    {
        public int GID { get; set; }
        public int AdminID { get; set; }
        public string GroupName { get; set; }
        public bool IsAdmin { get; set; }
        public List<UserTable> UserTable { get; set; }
        public List<GroupPostModel> GroupPostModel { get; set; }
    }
}
