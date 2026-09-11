using EmployeeManagementSystem.Api.DTOs.Auth;
using EmployeeManagementSystem.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var result = await _authService.AuthenticateAsync(request.Username, request.Password);
        if (result is null) return Unauthorized(new { message = "Invalid username or password" });
        return Ok(result);
    }
}