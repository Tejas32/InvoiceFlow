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
    public class ProductController : ControllerBase
    {
        private readonly IInvoiceFlowRepository _repository;

        public ProductController(IInvoiceFlowRepository repository)
        {
            _repository = repository;
        }

        // POST: api/Product
        [HttpPost]
        public IActionResult AddProduct(AddProductRequest request)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            Product product = new Product
            {
                UserId = userId,
                Name = request.Name,
                Price = request.Price,
                Description = request.Description,
                CreatedDate = DateTime.Now,
                IsActive = true
            };

            _repository.AddProduct(product);
            _repository.Save();

            return Ok(new
            {
                Message = "Product added successfully."
            });
        }

        // GET: api/Product
        [HttpGet]
        public IActionResult GetProducts()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var products = _repository.GetProducts(userId);

            return Ok(products);
        }

        // GET: api/Product/5
        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var product = _repository.GetProductById(id, userId);

            if (product == null)
            {
                return NotFound(new
                {
                    Message = "Product not found."
                });
            }

            return Ok(product);
        }

        // PUT: api/Product/5
        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, UpdateProductRequest request)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var product = _repository.GetProductById(id, userId);

            if (product == null)
            {
                return NotFound(new
                {
                    Message = "Product not found."
                });
            }

            product.Name = request.Name;
            product.Price = request.Price;
            product.Description = request.Description;

            _repository.UpdateProduct(product);
            _repository.Save();

            return Ok(new
            {
                Message = "Product updated successfully."
            });
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var product = _repository.GetProductById(id, userId);

            if (product == null)
            {
                return NotFound(new
                {
                    Message = "Product not found."
                });
            }

            _repository.DeleteProduct(product);
            _repository.Save();

            return Ok(new
            {
                Message = "Product deleted successfully."
            });
        }
    }
}