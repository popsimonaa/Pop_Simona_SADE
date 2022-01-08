using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pop_Simona_SADE.Models;
using Microsoft.EntityFrameworkCore;

namespace Pop_Simona_SADE.Data
{
    public class ExhibitionContext : DbContext
    {
        public ExhibitionContext(DbContextOptions<ExhibitionContext> options) :
            base(options)
        { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Painting> Paintings { get; set; }
        public DbSet<Current> Currents { get; set; }
        public DbSet<CurrentPainting> CurrentPaintings { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<Order>().ToTable("Order");
            modelBuilder.Entity<Painting>().ToTable("Painting");
            modelBuilder.Entity<Current>().ToTable("Current");
            modelBuilder.Entity<CurrentPainting>().ToTable("CurrentPainting");
            modelBuilder.Entity<CurrentPainting>().HasKey(c => new { c.PaintingID, c.CurrentID });//configureaza cheia primara compusa
        }
    }
}

