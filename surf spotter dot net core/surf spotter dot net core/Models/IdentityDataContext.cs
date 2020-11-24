using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace surf_spotter_dot_net_core.Models
{
    // Entityframework Database
    public class IdentityDataContext : IdentityDbContext<IdentityUser>
    {
        // "table" in EF core database
        public DbSet<Spot> Spots { get; set; }
        public DbSet<Comment> Comments { get; set; }
        // Constructor to ensure database creation
        public IdentityDataContext(DbContextOptions<IdentityDataContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
