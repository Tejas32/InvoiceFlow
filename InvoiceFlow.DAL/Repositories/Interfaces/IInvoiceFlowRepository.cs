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

        // ===== CLIENTS =====
        List<Client> GetClients(int userId);
        void AddClient(Client client);

        // ===== PRODUCTS =====
        List<Product> GetProducts(int userId);
        void AddProduct(Product product);

        // ===== INVOICES =====
        List<Invoice> GetInvoices(int userId);
        Invoice GetInvoiceById(int invoiceId);
        int CreateInvoice(Invoice invoice);
        void UpdateInvoice(Invoice invoice);

        // ===== INVOICE ITEMS =====
        void AddInvoiceItems(List<InvoiceItem> items);

        // ===== SAVE =====
        void Save();
    }
}