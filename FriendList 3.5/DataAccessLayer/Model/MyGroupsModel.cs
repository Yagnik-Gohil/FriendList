using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Model
{
    public class MyGroupsModel
    {
        public List<GroupTable> MyGroups { get; set; }
        public List<GroupTable> GroupInvites { get; set; }
    }
}
