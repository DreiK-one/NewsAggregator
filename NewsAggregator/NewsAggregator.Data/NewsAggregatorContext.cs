﻿using Microsoft.EntityFrameworkCore;
using NewsAggregator.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Data
{
    public class NewsAggregatorContext : DbContext
    {
        public DbSet<Article> Articles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserActivity> UserActivities { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        public NewsAggregatorContext(DbContextOptions<NewsAggregatorContext> options) 
            : base(options)
        {

        }

    }
}
