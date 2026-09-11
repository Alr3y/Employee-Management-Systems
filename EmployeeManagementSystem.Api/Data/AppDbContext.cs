using EmployeeManagementSystem.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(d => d.Id);
            entity.Property(d => d.Name).HasMaxLength(100).IsRequired();
            entity.Property(d => d.Description).HasMaxLength(500);
            entity.HasIndex(d => d.Name).IsUnique();
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.LastName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Email).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Salary).HasPrecision(18, 2);
            entity.HasIndex(e => e.Email).IsUnique();

            entity.HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Username).HasMaxLength(100).IsRequired();
            entity.HasIndex(u => u.Username).IsUnique();
            entity.Property(u => u.PasswordHash).IsRequired();
        });

        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        var seedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<Department>().HasData(
            new Department { Id = 1, Name = "Human Resources", Description = "HR and recruitment", CreatedAt = seedDate },
            new Department { Id = 2, Name = "Engineering", Description = "Software development", CreatedAt = seedDate },
            new Department { Id = 3, Name = "Finance", Description = "Financial operations", CreatedAt = seedDate }
        );

        modelBuilder.Entity<Employee>().HasData(
            new Employee
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@company.com",
                Phone = "+62-812-3456-7890",
                HireDate = new DateTime(2022, 3, 15, 0, 0, 0, DateTimeKind.Utc),
                Salary = 8500000m,
                IsActive = true,
                DepartmentId = 2,
                CreatedAt = seedDate
            },
            new Employee
            {
                Id = 2,
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@company.com",
                Phone = "+62-813-9876-5432",
                HireDate = new DateTime(2021, 7, 1, 0, 0, 0, DateTimeKind.Utc),
                Salary = 12000000m,
                IsActive = true,
                DepartmentId = 1,
                CreatedAt = seedDate
            },
            new Employee
            {
                Id = 3,
                FirstName = "Michael",
                LastName = "Johnson",
                Email = "michael.johnson@company.com",
                Phone = "+62-821-5555-1234",
                HireDate = new DateTime(2023, 1, 10, 0, 0, 0, DateTimeKind.Utc),
                Salary = 9500000m,
                IsActive = true,
                DepartmentId = 3,
                CreatedAt = seedDate
            }
        );
    }
}