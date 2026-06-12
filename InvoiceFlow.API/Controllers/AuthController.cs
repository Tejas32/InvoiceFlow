using InvoiceFlow.API.DTOs;
using InvoiceFlow.DAL.Models;
using InvoiceFlow.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using InvoiceFlow.API.Services;

namespace InvoiceFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IInvoiceFlowRepository _repository;
        private readonly JwtService _jwtService;

        public AuthController (IInvoiceFlowRepository repository, JwtService jwtService)
        {
            _repository = repository;
            _jwtService = jwtService;
        }

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


        // Http Post Login 

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

    }
}