using InvoiceFlow.API.DTOs;
using InvoiceFlow.DAL.Models;
using InvoiceFlow.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InvoiceFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClientController : ControllerBase
    {
        private readonly IInvoiceFlowRepository _repository;

        public ClientController(IInvoiceFlowRepository repository)
        {
            _repository = repository;
        }

        // POST: api/client
        [HttpPost]
        public IActionResult AddClient(AddClientRequest request)
        {
            int userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            Client client = new Client
            {
                UserId = userId,
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone,
                Address = request.Address,
                CreatedDate = DateTime.Now,
                IsDeleted = false
            };

            _repository.AddClient(client);
            _repository.Save();

            return Ok(new
            {
                Message = "Client added successfully."
            });
        }

        // GET: api/client
        [HttpGet]
        public IActionResult GetClients()
        {
            int userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var clients = _repository.GetClients(userId);

            return Ok(clients);
        }

        // GET: api/client/5
        [HttpGet("{id}")]
        public IActionResult GetClientById(int id)
        {
            int userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var client = _repository.GetClientById(id, userId);

            if (client == null)
            {
                return NotFound(new
                {
                    Message = "Client not found."
                });
            }

            return Ok(client);
        }

        // PUT: api/client/5
        [HttpPut("{id}")]
        public IActionResult UpdateClient(int id, UpdateClientRequest request)
        {
            int userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var client = _repository.GetClientById(id, userId);

            if (client == null)
            {
                return NotFound(new
                {
                    Message = "Client not found."
                });
            }

            client.Name = request.Name;
            client.Email = request.Email;
            client.Phone = request.Phone;
            client.Address = request.Address;

            _repository.UpdateClient(client);
            _repository.Save();

            return Ok(new
            {
                Message = "Client updated successfully."
            });
        }

        // DELETE: api/client/5
        [HttpDelete("{id}")]
        public IActionResult DeleteClient(int id)
        {
            int userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var client = _repository.GetClientById(id, userId);

            if (client == null)
            {
                return NotFound(new
                {
                    Message = "Client not found."
                });
            }

            _repository.DeleteClient(client);
            _repository.Save();

            return Ok(new
            {
                Message = "Client deleted successfully."
            });
        }
    }
}