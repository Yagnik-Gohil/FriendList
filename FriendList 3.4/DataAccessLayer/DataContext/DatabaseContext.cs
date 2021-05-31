using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.DataContext
{
    public class DatabaseContext : DbContext
    {
        public class OptionsBuild
        {
            public OptionsBuild()
            {
                settings = new AppConfiguration();
                optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
                dbOptions = optionsBuilder.Options;
            }
            public DbContextOptionsBuilder<DatabaseContext> optionsBuilder { get; set; }
            public DbContextOptions<DatabaseContext> dbOptions { get; set; }
            private AppConfiguration settings { get; set; }
        }

        public static OptionsBuild ops = new OptionsBuild();
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }
        public DbSet<UserTable> UserTable { get; set; }
        public DbSet<StatusTable> StatusTable { get; set; }
        public DbSet<PostTable> PostTable { get; set; }
        public DbSet<CommentTable> CommentTable { get; set; }
        public DbSet<GroupTable> GroupTable { get; set; }
        public DbSet<GroupManagerTable> GroupManagerTable { get; set; }
        public DbSet<GroupPostTable> GroupPostTable { get; set; }
    }
}
