using InvoiceFlow.API.DTOs;
using InvoiceFlow.API.Services;
using InvoiceFlow.DAL.Models;
using InvoiceFlow.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InvoiceFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IInvoiceFlowRepository _repository;
        private readonly JwtService _jwtService;

        public AuthController(IInvoiceFlowRepository repository, JwtService jwtService)
        {
            _repository = repository;
            _jwtService = jwtService;
        }

        // Register User
        [HttpPost("register")]
        public IActionResult Register(RegisterRequest request)
        {
            User existingUser = _repository.GetUserByEmail(request.Email);

            if (existingUser != null)
            {
                return BadRequest("Email already exists");
            }

            User user = new User
            {
                Name = request.Name,
                Email = request.Email,
                Password = request.Password,
                BusinessName = request.BusinessName,
                BusinessAddress = request.BusinessAddress,
                Phone = request.Phone,
                Industry = request.Industry,
                Location = request.Location,
                CreatedDate = DateTime.Now,
                IsDeleted = false
            };

            _repository.AddUser(user);
            _repository.Save();

            return Ok("User registered successfully");
        }

        // Login User
        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            User user = _repository.GetUserByEmailAndPassword(
                request.Email,
                request.Password);

            if (user == null)
            {
                return Unauthorized("Invalid email or password");
            }

            string token = _jwtService.GenerateToken(user);

            return Ok(new
            {
                Token = token,
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                BusinessName = user.BusinessName
            });
        }

        // Protected API
        [Authorize]
        [HttpGet("profile")]
        public IActionResult Profile()
        {
            return Ok(new
            {
                Message = "JWT Authentication Successful",
                User = User.Identity?.Name,
                UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Email = User.FindFirst(ClaimTypes.Email)?.Value
            });
        }
    }
}