using System;
using System.Collections.Generic;
using ExampleApp.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.WebApi.Data;

public partial class ExampleDbContext : DbContext
{
    public ExampleDbContext()
    {
    }

    public ExampleDbContext(DbContextOptions<ExampleDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<PersonRole> PersonRoles { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite("Name=ConnectionStrings:SqliteExampleDb");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>(entity =>
        {
            entity.ToTable("Person");

            entity.Property(e => e.DateOfBirth).HasColumnType("DATE");
        });

        modelBuilder.Entity<PersonRole>(entity =>
        {
            entity.HasKey(e => new { e.PersonId, e.RoleId });

            entity.ToTable("PersonRole");

            entity.Property(e => e.ExpiresOn).HasColumnType("DATETIME");
            entity.Property(e => e.GivenOn).HasColumnType("DATETIME");

            entity.HasOne(d => d.Person).WithMany(p => p.PersonRoles)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Role).WithMany(p => p.PersonRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.HasIndex(e => e.Name, "IX_Role_Name").IsUnique();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
