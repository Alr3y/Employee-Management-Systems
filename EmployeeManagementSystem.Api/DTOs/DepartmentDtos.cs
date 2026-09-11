using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Api.DTOs;

public record DepartmentDto(int Id, string Name, string? Description, int EmployeeCount);

public record CreateDepartmentDto(
    [Required][MaxLength(100)] string Name,
    [MaxLength(500)] string? Description
);

public record UpdateDepartmentDto(
    [Required][MaxLength(100)] string Name,
    [MaxLength(500)] string? Description
);
