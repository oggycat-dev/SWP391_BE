using CleanArchitectureTemplate.Application.Common.DTOs;
using CleanArchitectureTemplate.Application.Common.DTOs.Auth;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureTemplate.API.Controllers.CMS;

/// <summary>
/// CMS authentication API for Admin and EVM staff
/// </summary>
[ApiController]
[Route("api/cms/auth")]
[ApiExplorerSettings(GroupName = "cms")]
public class AuthController : ControllerBase
{
    private readonly IAuthenticationService _authService;
    private readonly ICurrentUserService _currentUserService;

    public AuthController(
        IAuthenticationService authService,
        ICurrentUserService currentUserService)
    {
        _authService = authService;
        _currentUserService = currentUserService;
    }

    /// <summary>
    /// CMS Login for Admin, EVMManager, and EVMStaff
    /// </summary>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<LoginResponse>>> Login([FromBody] LoginRequest request)
    {
        var loginResponse = await _authService.LoginAsync(request);
        
        // Validate that user has CMS access (Admin, EVMManager, or EVMStaff)
        if (loginResponse.Role != "Admin" && 
            loginResponse.Role != "EVMManager" && 
            loginResponse.Role != "EVMStaff")
        {
            return Unauthorized(ApiResponse<LoginResponse>.Unauthorized("Access denied. CMS login is only for Admin, EVMManager, and EVMStaff."));
        }
        
        var response = ApiResponse<LoginResponse>.Ok(loginResponse, "Login successful");
        return Ok(response);
    }

    /// <summary>
    /// Refresh access token
    /// </summary>
    [HttpPost("refresh-token")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<LoginResponse>>> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var loginResponse = await _authService.RefreshTokenAsync(request);
        var response = ApiResponse<LoginResponse>.Ok(loginResponse, "Token refreshed successfully");
        return Ok(response);
    }

    /// <summary>
    /// CMS Logout
    /// </summary>
    [HttpPost("logout")]
    [Authorize(Roles = "Admin,EVMManager,EVMStaff")]
    public async Task<ActionResult<ApiResponse<object>>> Logout()
    {
        await _authService.LogoutAsync(_currentUserService.UserId?.ToString() ?? string.Empty);
        var response = ApiResponse<object>.Ok(null, "Logout successful");
        return Ok(response);
    }
}
