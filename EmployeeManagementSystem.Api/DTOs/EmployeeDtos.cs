using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Api.DTOs;

public record EmployeeDto(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    string? Phone,
    DateTime HireDate,
    decimal Salary,
    bool IsActive,
    int DepartmentId,
    string DepartmentName
);

public record CreateEmployeeDto(
    [Required][MaxLength(100)] string FirstName,
    [Required][MaxLength(100)] string LastName,
    [Required][EmailAddress][MaxLength(200)] string Email,
    [MaxLength(20)] string? Phone,
    [Required] DateTime HireDate,
    [Required][Range(0, double.MaxValue)] decimal Salary,
    [Required] int DepartmentId,
    bool IsActive = true
);

public record UpdateEmployeeDto(
    [Required][MaxLength(100)] string FirstName,
    [Required][MaxLength(100)] string LastName,
    [Required][EmailAddress][MaxLength(200)] string Email,
    [MaxLength(20)] string? Phone,
    [Required] DateTime HireDate,
    [Required][Range(0, double.MaxValue)] decimal Salary,
    bool IsActive,
    [Required] int DepartmentId
);
