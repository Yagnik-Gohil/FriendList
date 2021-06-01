using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccessLayer.Model
{
    public class PostModel
    {
        public int PID { get; set; }
        public string UserName { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public List<CommentModel> CommentModel { get; set; }
    }
}
