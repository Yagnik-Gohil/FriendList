using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccessLayer.Model
{
    public class UserProfile
    {
        /*public int UID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }*/
        public int TotalFriends { get; set; }
        public int FriendRequests { get; set; }
        public string ProfilePicture { get; set; }

        public UserTable UserTable { get; set; }
        public List<PostModel> PostModel { get; set; }

    }
}
