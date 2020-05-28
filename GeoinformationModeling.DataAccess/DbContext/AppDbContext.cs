using GeoinformationModeling.DataAccess.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoinformationModeling.DataAccess.DbContext
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<RiverParams> RiverParamsSet { get; set; }
        public DbSet<MapParams> MapParams { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RiverParams>()
                .HasKey(g => g.Id);

            modelBuilder.Entity<RiverParams>()
               .HasOne(t => t.User).WithMany(k => k.RiverParamsList).HasForeignKey(l => l.UserId);

            modelBuilder.Entity<MapParams>()
               .HasKey(g => g.Id);

            modelBuilder.Entity<MapParams>()
               .HasOne(t => t.RiverParams).WithMany(k => k.MapParamsList).HasForeignKey(l => l.RiverParamsId);
        }
    }
}
