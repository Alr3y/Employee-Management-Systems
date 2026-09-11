using EmployeeManagementSystem.Api.DTOs.Auth;

namespace EmployeeManagementSystem.Api.Services;

public interface IAuthService
{
    Task<AuthResponseDto?> AuthenticateAsync(string username, string password);
}