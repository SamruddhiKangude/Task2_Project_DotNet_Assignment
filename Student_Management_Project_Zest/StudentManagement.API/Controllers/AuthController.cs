using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Core.DTOs;
using StudentManagement.Core.Entities;
using StudentManagement.Core.Interfaces;
using StudentManagement.Infrastructure.Data;

namespace StudentManagement.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ITokenService _tokenService;

    public AuthController(AppDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        // For simplicity and assignment, plain text matching, but in Real World use PasswordHasher
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == loginDto.Username && x.PasswordHash == loginDto.Password);

        if (user == null)
        {
            return Unauthorized("Invalid Username or Password");
        }

        var token = _tokenService.CreateToken(user);
        return Ok(new { token });
    }
}
