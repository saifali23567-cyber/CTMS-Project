using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProjectAPI.Models;
using ProjectAPI.DTOs;


namespace ProjectAPI.Data
{
    public class CtmsDbContext : DbContext
    {
        public CtmsDbContext(DbContextOptions<CtmsDbContext> options) : base(options) { }

        public DbSet<Contact> Contact => Set<Contact>();
        public DbSet<Customer> Customer => Set<Customer>();
        public DbSet<Courier> Courier => Set<Courier>();
        public DbSet<Admin> Admin => Set<Admin>();
        public DbSet<Package> Package => Set<Package>();
        public DbSet<Tracking> Tracking => Set<Tracking>();

        public DbSet<CustomerDemoDto> CustomerDemoDtos { get; set; } = null!;
public DbSet<CourierDemoDto> CourierDemoDtos { get; set; } = null!;
public DbSet<MyPackagesDemoDto> MyPackagesDemoDtos { get; set; } = null!;
public DbSet<AssignedDemoDto> AssignedDemoDtos { get; set; } = null!;

        public DbSet<AllPackagesDemoDto> AllPackagesDemoDtos { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Fix Phone as string (BIGINT in DB, but better as string in API)
            modelBuilder.Entity<Contact>()
                .Property(c => c.Phone)
                .HasConversion<long?>();

            // Ensure TrackingNumber is unique in Package
            modelBuilder.Entity<Package>()
                .HasIndex(p => p.TrackingNumber)
                .IsUnique();
            
            // Fix: Create proper converters for decimal to double
            var decimalToDoubleConverter = new ValueConverter<double, decimal>(
                v => (decimal)v,          // Convert double to decimal for DB
                v => (double)v            // Convert decimal to double for C#
            );
            
            modelBuilder.Entity<Package>()
                .Property(p => p.Cost)
                .HasConversion(decimalToDoubleConverter);
            
            modelBuilder.Entity<Package>()
                .Property(p => p.RateperKG)
                .HasConversion(decimalToDoubleConverter);
            
            modelBuilder.Entity<Package>()
                .Property(p => p.Weight)
                .HasConversion(decimalToDoubleConverter);

            // Composite key for Tracking if needed
            modelBuilder.Entity<Tracking>()
                .HasKey(t => t.TrackingID);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AllPackagesDemoDto>().HasNoKey().ToView(null);

            modelBuilder.Entity<CustomerDemoDto>().HasNoKey().ToView(null);
modelBuilder.Entity<CourierDemoDto>().HasNoKey().ToView(null);
modelBuilder.Entity<MyPackagesDemoDto>().HasNoKey().ToView(null);
modelBuilder.Entity<AssignedDemoDto>().HasNoKey().ToView(null);
        }
    }
}