using InvoiceFlow.DAL.Models;
using InvoiceFlow.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceFlow.DAL.Repositories.Implementations
{
    public class InvoiceFlowRepository: IInvoiceFlowRepository
    {
        private readonly InvoiceFlowDbContext _context;

        public InvoiceFlowRepository (InvoiceFlowDbContext context)
        {
            _context = context;
        }

        // User
        public User GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(x => x.Email == email && x.IsDeleted == false);
            
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
        }

        // Clients

        public List<Client> GetClients(int userId)
        {
            return _context.Clients.Where(x => x.UserId == userId && x.IsDeleted == false).ToList();
        }

        public void AddClient(Client client)
        {
            _context.Clients.Add(client);
        }

        // Products 

        public List<Product> GetProducts(int userId)
        {
            return _context.Products.Where(x => x.UserId == userId && x.IsActive == true).ToList();
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
        }

        // Invoices

        public List<Invoice> GetInvoices(int userId)
        {
            return _context.Invoices.Where(x => x.UserId == userId && x.IsDeleted == false).ToList();
        }

        public Invoice GetInvoiceById(int invoiceId)
        {
            return _context.Invoices.Include(x=> x.InvoiceItems).FirstOrDefault(x => x.InvoiceId == invoiceId && x.IsDeleted == false);
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

        // Invoice Items

        public void AddInvoiceItems(List<InvoiceItem> items)
        {
            _context.InvoiceItems.AddRange(items);
        }

        // Save

        public void Save()
        {
            _context.SaveChanges();
        }

    }
}
