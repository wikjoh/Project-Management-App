﻿using Data.Entities;
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
    public DbSet<UserRoleEntity> UserRoles { get; set; }
    public DbSet<RoleEntity> Roles { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Projects
        modelBuilder.Entity<ProjectEntity>()
            .Property(p => p.Id)
            .UseIdentityColumn(seed: 101, increment: 1);

        modelBuilder.Entity<ProjectEntity>()
            .HasOne(p => p.ProjectManager)
            .WithMany(pm => pm.Projects)
            .HasForeignKey(p => p.ProjectManagerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProjectEntity>()
            .HasOne(p => p.Customer)
            .WithMany(c => c.Projects)
            .HasForeignKey(p => p.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProjectEntity>()
            .HasOne(p => p.Service)
            .WithMany(s => s.Projects)
            .HasForeignKey(p => p.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProjectEntity>()
            .HasOne(p => p.Status)
            .WithMany(ps => ps.Projects)
            .HasForeignKey(p => p.StatusId)
            .OnDelete(DeleteBehavior.Restrict);


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
            .HasForeignKey(s => s.UnitId)
            .OnDelete(DeleteBehavior.Restrict);


        // Users (project managers)
        modelBuilder.Entity<UserEntity>()
            .Property(u => u.DisplayName)
            .HasComputedColumnSql(
                "CONCAT(FirstName, ' ', LastName)",
                stored: true
            );

        modelBuilder.Entity<UserEntity>()
            .HasMany(u => u.UserRoles)
            .WithOne(ur => ur.User)
            .HasForeignKey(u => u.UserId);


        // User roles
        modelBuilder.Entity<UserRoleEntity>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });

        modelBuilder.Entity<UserRoleEntity>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
