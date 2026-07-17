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
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceFlowRepository _repository;

        public InvoiceController(IInvoiceFlowRepository repository)
        {
            _repository = repository;
        }

        // POST: api/Invoice
        [HttpPost]
        public IActionResult CreateInvoice(CreateInvoiceRequest request)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            // Validate Client
            var client = _repository.GetClientById(request.ClientId, userId);

            if (client == null)
            {
                return NotFound(new
                {
                    Message = "Client not found."
                });
            }

            decimal totalAmount = 0;
            List<InvoiceItem> invoiceItems = new();

            foreach (var item in request.Items)
            {
                var product = _repository.GetProductById(item.ProductId, userId);

                if (product == null)
                {
                    return NotFound(new
                    {
                        Message = $"Product with ID {item.ProductId} not found."
                    });
                }

                decimal price = product.Price ?? 0;
                decimal total = price * item.Quantity;

                totalAmount += total;

                invoiceItems.Add(new InvoiceItem
                {
                    ProductId = product.ProductId,
                    ItemName = product.Name,
                    Quantity = item.Quantity,
                    Price = price,
                    Total = total
                });
            }

            Invoice invoice = new Invoice
            {
                UserId = userId,
                ClientId = request.ClientId,
                InvoiceNumber = "INV-" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                TotalAmount = totalAmount,
                Status = "Pending",
                PaymentType = request.PaymentType,
                PaymentLink = null,
                CreatedDate = DateTime.Now,
                DueDate = request.DueDate,
                UpdatedDate = DateTime.Now,
                IsDeleted = false
            };

            int invoiceId = _repository.CreateInvoice(invoice);

            foreach (var item in invoiceItems)
            {
                item.InvoiceId = invoiceId;
            }

            _repository.AddInvoiceItems(invoiceItems);
            _repository.Save();

            return Ok(new
            {
                Message = "Invoice created successfully.",
                InvoiceId = invoiceId,
                InvoiceNumber = invoice.InvoiceNumber,
                TotalAmount = totalAmount
            });
        }

        // GET: api/Invoice
        [HttpGet]
        public IActionResult GetInvoices()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var invoices = _repository.GetInvoices(userId);

            return Ok(invoices);
        }

        // GET: api/Invoice/{id}
        [HttpGet("{id}")]
        public IActionResult GetInvoiceById(int id)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var invoice = _repository.GetInvoiceById(id, userId);

            if (invoice == null)
            {
                return NotFound(new
                {
                    Message = "Invoice not found."
                });
            }

            return Ok(invoice);
        }

        // PUT: api/Invoice/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateInvoice(int id, UpdateInvoiceRequest request)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var invoice = _repository.GetInvoiceById(id, userId);

            if (invoice == null)
            {
                return NotFound(new
                {
                    Message = "Invoice not found."
                });
            }

            var client = _repository.GetClientById(request.ClientId, userId);

            if (client == null)
            {
                return NotFound(new
                {
                    Message = "Client not found."
                });
            }

            invoice.ClientId = request.ClientId;
            invoice.PaymentType = request.PaymentType;
            invoice.DueDate = request.DueDate;
            invoice.Status = request.Status;
            invoice.UpdatedDate = DateTime.Now;

            _repository.UpdateInvoice(invoice);
            _repository.Save();

            return Ok(new
            {
                Message = "Invoice updated successfully."
            });
        }

        // DELETE: api/Invoice/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteInvoice(int id)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var invoice = _repository.GetInvoiceById(id, userId);

            if (invoice == null)
            {
                return NotFound(new
                {
                    Message = "Invoice not found."
                });
            }

            _repository.DeleteInvoice(invoice);
            _repository.Save();

            return Ok(new
            {
                Message = "Invoice deleted successfully."
            });
        }
    }
}