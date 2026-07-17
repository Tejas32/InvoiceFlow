using InvoiceFlow.DAL.Models;
using InvoiceFlow.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InvoiceFlow.DAL.Repositories.Implementations
{
    public class InvoiceFlowRepository : IInvoiceFlowRepository
    {
        private readonly InvoiceFlowDbContext _context;

        public InvoiceFlowRepository(InvoiceFlowDbContext context)
        {
            _context = context;
        }

        // ===========================
        // Users
        // ===========================

        public User GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(x =>
                x.Email == email &&
                x.IsDeleted == false);
        }

        public User GetUserByEmailAndPassword(string email, string password)
        {
            return _context.Users.FirstOrDefault(x =>
                x.Email == email &&
                x.Password == password &&
                x.IsDeleted == false);
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
        }

        // ===========================
        // Clients
        // ===========================

        public List<Client> GetClients(int userId)
        {
            return _context.Clients
                .Where(x => x.UserId == userId && x.IsDeleted == false)
                .ToList();
        }

        public Client? GetClientById(int clientId, int userId)
        {
            return _context.Clients.FirstOrDefault(x =>
                x.ClientId == clientId &&
                x.UserId == userId &&
                x.IsDeleted == false);
        }

        public void AddClient(Client client)
        {
            _context.Clients.Add(client);
        }

        public void UpdateClient(Client client)
        {
            _context.Clients.Update(client);
        }

        public void DeleteClient(Client client)
        {
            client.IsDeleted = true;
            _context.Clients.Update(client);
        }

        // ===========================
        // Products
        // ===========================

        public List<Product> GetProducts(int userId)
        {
            return _context.Products
                .Where(x => x.UserId == userId && x.IsActive == true)
                .ToList();
        }

        public Product? GetProductById(int productId, int userId)
        {
            return _context.Products.FirstOrDefault(x =>
                x.ProductId == productId &&
                x.UserId == userId &&
                x.IsActive == true);
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
        }

        public void UpdateProduct(Product product)
        {
            _context.Products.Update(product);
        }

        public void DeleteProduct(Product product)
        {
            product.IsActive = false;
            _context.Products.Update(product);
        }

        // ===========================
        // Invoices
        // ===========================

        public List<Invoice> GetInvoices(int userId)
        {
            return _context.Invoices
                .Where(x => x.UserId == userId && x.IsDeleted == false)
                .ToList();
        }

        public Invoice? GetInvoiceById(int invoiceId, int userId)
        {
            return _context.Invoices
                .Include(x => x.InvoiceItems)
                .FirstOrDefault(x =>
                    x.InvoiceId == invoiceId &&
                    x.UserId == userId &&
                    x.IsDeleted == false);
        }

        public int CreateInvoice(Invoice invoice)
        {
            _context.Invoices.Add(invoice);
            _context.SaveChanges();

            return invoice.InvoiceId;
        }

        public void UpdateInvoice(Invoice invoice)
        {
            _context.Invoices.Update(invoice);
        }

        public void DeleteInvoice(Invoice invoice)
        {
            invoice.IsDeleted = true;
            invoice.UpdatedDate = DateTime.Now;

            _context.Invoices.Update(invoice);
        }

        // ===========================
        // Invoice Items
        // ===========================

        public void AddInvoiceItems(List<InvoiceItem> items)
        {
            _context.InvoiceItems.AddRange(items);
        }

        // ===========================
        // Common
        // ===========================

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}