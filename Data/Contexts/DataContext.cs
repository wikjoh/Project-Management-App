using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<CustomerEntity> Customers { get; set; }
    public DbSet<CustomerPhoneNumberEntity> CustomerPhoneNumbers { get; set; }
    public DbSet<ProjectEntity> Projects { get; set; }
    public DbSet<ProjectStatusEntity> ProjectStatuses { get; set; }
    public DbSet<ServiceEntity> Services { get; set; }
    public DbSet<ServiceUnitEntity> ServiceUnits { get; set; }
    public DbSet<UserEntity> Users { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Projects
        modelBuilder.Entity<ProjectEntity>()
            .Property(p => p.Id)
            .UseIdentityColumn(seed: 101, increment: 1);

        modelBuilder.Entity<ProjectEntity>()
            .HasOne(p => p.ProjectManager)
            .WithMany(pm => pm.Projects)
            .HasForeignKey(p => p.ProjectManagerId);

        modelBuilder.Entity<ProjectEntity>()
            .HasOne(p => p.Customer)
            .WithMany(c => c.Projects)
            .HasForeignKey(p => p.CustomerId);

        modelBuilder.Entity<ProjectEntity>()
            .HasOne(p => p.Service)
            .WithMany(s => s.Projects)
            .HasForeignKey(p => p.ServiceId);

        modelBuilder.Entity<ProjectEntity>()
            .HasOne(p => p.Status)
            .WithMany(ps => ps.Projects)
            .HasForeignKey(p => p.StatusId);


        // Customers
        modelBuilder.Entity<CustomerEntity>()
            .HasMany(c => c.PhoneNumbers)
            .WithOne(cpn => cpn.Customer)
            .HasForeignKey(c => c.CustomerId);

        modelBuilder.Entity<CustomerEntity>()
            .Property(c => c.DisplayName)
            .HasComputedColumnSql(
                "CASE WHEN IsCompany = 1 THEN CompanyName ELSE CONCAT(FirstName, ' ', LastName) END",
                stored: true
            );


        // Customer phone numbers
        modelBuilder.Entity<CustomerPhoneNumberEntity>()
            .HasKey(cpn => new { cpn.CustomerId, cpn.PhoneNumber });


        // Services
        modelBuilder.Entity<ServiceEntity>()
            .HasOne(s => s.Unit)
            .WithMany(u => u.Services)
            .HasForeignKey(s => s.UnitId);


        // Users (project managers)
        modelBuilder.Entity<UserEntity>()
            .Property(u => u.DisplayName)
            .HasComputedColumnSql(
                "CONCAT(FirstName, ' ', LastName)",
                stored: true
            );
    }
}
