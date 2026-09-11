namespace EmployeeManagementSystem.Api.DTOs.Auth;

public sealed record AuthResponseDto(string Token, DateTime ExpiresAt);