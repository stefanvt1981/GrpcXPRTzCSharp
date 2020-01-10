using GrpcXPRTzCSharp.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GrpcXPRTzCSharp.Repository
{
    public class XprtzContext : DbContext
    {
        public XprtzContext(DbContextOptions<XprtzContext> options) : base(options)
        { }

        public DbSet<XprtEntity> Xprtz { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<XprtEntity>()
                .HasKey(x => x.Id);
        }
    }
}
