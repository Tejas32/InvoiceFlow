using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InvoiceFlow.DAL.Models;

namespace InvoiceFlow.DAL.Repositories.Interfaces
{
    public interface IInvoiceFlowRepository
    {
        User GetUserByEmail(string email);
        void AddUser(User user);

        // clients
        List<Client> GetClients(int userId);
        void AddClient(Client client);

        // Products
        List<Product> GetProducts(int userId);
        void AddProduct(Product product);

        // Invoices
        List<Invoice> GetInvoices(int userId);
        Invoice GetInvoiceById(int invoiceId);
        int CreateInvoice(Invoice invoice);
        void UpdateInvoice(Invoice invoice);

        // Invoice Items
        void AddInvoiceItems(List<InvoiceItem> items);

        // Save
        void Save();
    }
}