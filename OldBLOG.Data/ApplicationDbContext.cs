using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OldBLOG.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OldBLOG.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {}

        public DbSet<Post> Posts { get; set; }
		public DbSet<Comment> Comments { get; set; }
	}
}
