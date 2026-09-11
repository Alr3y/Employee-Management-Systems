using System;

namespace EmployeeManagementSystem.Api.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string PasswordHash { get; set; } = null!; // store hashed password
    public string? Role { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}