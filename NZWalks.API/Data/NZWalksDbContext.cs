using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Data
{
    public class NZWalksDbContext : DbContext
    {
        public NZWalksDbContext(DbContextOptions<NZWalksDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed data for Difficulties table
            List<Difficulty> difficulties = new List<Difficulty>()
            {
                new Difficulty() {
                    Id = Guid.Parse("d28888e9-2ba9-473a-a40f-e38cb54f9b35"),
                    Name = "Easy"
                },
                new Difficulty()
                {
                    Id = Guid.Parse("d28888e9-2ba9-473a-a40f-e38cb54f9b36"),
                    Name = "Medium"
                },
                new Difficulty()
                {
                    Id = Guid.Parse("d28888e9-2ba9-473a-a40f-e38cb54f9b37"),
                    Name = "Hard"
                }
            };

            // Save difficulties to the database
            modelBuilder.Entity<Difficulty>().HasData(difficulties);

            // Seed data for Regions table
            List<Region> regions = new List<Region>()
            {
                new Region()
                {
                    Id = Guid.Parse("d28888e9-2ba9-473a-a40f-e38cb54f9b01"),
                    Name = "Northland",
                    Code = "NTL",
                    RegionImageUrl = "https://images.unsplash.com/photo-1506744038136-46273834b3fb?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80"
                },
                new Region()
                {
                    Id = Guid.Parse("d28888e9-2ba9-473a-a40f-e38cb54f9b02"),
                    Name = "Auckland",
                    Code = "AKL",
                    RegionImageUrl = "https://images.unsplash.com/photo-1506748686214-e9df14d4d9d0?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80"
                },
                new Region()
                {
                    Id = Guid.Parse("d28888e9-2ba9-473a-a40f-e38cb54f9b03"),
                    Name = "Waikato",
                    Code = "WKO",
                    RegionImageUrl = "https://images.unsplash.com/photo-1500534623283-312aade485b7?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80"
                },
                new Region()
                {
                    Id = Guid.Parse("d28888e9-2ba9-473a-a40f-e38cb54f9b04"),
                    Name = "Bay of Plenty",
                    Code = "BOP"
                }
            };

            // Save regions to the database
            modelBuilder.Entity<Region>().HasData(regions);
        }
    }
}
