using InvoiceFlow.DAL.Models;

namespace InvoiceFlow.DAL.Repositories.Interfaces
{
    public interface IInvoiceFlowRepository
    {
        // ===========================
        // Users
        // ===========================

        User GetUserByEmail(string email);

        User GetUserByEmailAndPassword(string email, string password);

        void AddUser(User user);

        // ===========================
        // Clients
        // ===========================

        List<Client> GetClients(int userId);

        Client? GetClientById(int clientId, int userId);

        void AddClient(Client client);

        void UpdateClient(Client client);

        void DeleteClient(Client client);

        // ===========================
        // Products
        // ===========================

        List<Product> GetProducts(int userId);

        Product? GetProductById(int productId, int userId);

        void AddProduct(Product product);

        void UpdateProduct(Product product);

        void DeleteProduct(Product product);

        // ===========================
        // Invoices
        // ===========================

        List<Invoice> GetInvoices(int userId);

        Invoice? GetInvoiceById(int invoiceId, int userId);

        int CreateInvoice(Invoice invoice);

        void UpdateInvoice(Invoice invoice);

        void DeleteInvoice(Invoice invoice);

        // ===========================
        // Invoice Items
        // ===========================

        void AddInvoiceItems(List<InvoiceItem> items);

        // ===========================
        // Save Changes
        // ===========================

        void Save();
    }
}