using CleanArchitectureTemplate.Application.Common.DTOs;
using CleanArchitectureTemplate.Application.Common.DTOs.Auth;
using CleanArchitectureTemplate.Application.Common.DTOs.Users;
using CleanArchitectureTemplate.Application.Features.Users.Commands.CreateUser;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureTemplate.API.Controllers.Customer;

/// <summary>
/// Customer authentication API
/// </summary>
[ApiController]
[Route("api/customer/auth")]
[ApiExplorerSettings(GroupName = "customer")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IAuthenticationService _authService;
    private readonly ICurrentUserService _currentUserService;

    public AuthController(
        IMediator mediator,
        IAuthenticationService authService,
        ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _authService = authService;
        _currentUserService = currentUserService;
    }

    /// <summary>
    /// Register a new customer account
    /// </summary>
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<UserDto>>> Register([FromBody] RegisterRequest request)
    {
        var command = new CreateUserCommand
        {
            Username = request.Username,
            Email = request.Email,
            Password = request.Password,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Role = UserRole.Customer.ToString()
        };

        var result = await _mediator.Send(command);
        if (result.Success && result.Data != null)
        {
            var response = ApiResponse<UserDto>.Created(result.Data, "Customer registered successfully");
            return StatusCode(response.StatusCode, response);
        }
        
        var errorResponse = ApiResponse<object>.BadRequest(result.Message, result.Errors);
        return StatusCode(errorResponse.StatusCode, errorResponse);
    }

    /// <summary>
    /// Customer login
    /// </summary>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<LoginResponse>>> Login([FromBody] LoginRequest request)
    {
        var loginResponse = await _authService.LoginAsync(request);
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
    /// Customer logout
    /// </summary>
    [HttpPost("logout")]
    [Authorize(Roles = "Customer")]
    public async Task<ActionResult<ApiResponse<object>>> Logout()
    {
        await _authService.LogoutAsync(_currentUserService.UserId?.ToString() ?? string.Empty);
        var response = ApiResponse<object>.Ok(null, "Logout successful");
        return Ok(response);
    }
}
