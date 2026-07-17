namespace InvoiceFlow.API.DTOs
{
    public class CreateInvoiceRequest
    {
        public int ClientId { get; set; }

        public string PaymentType { get; set; } = string.Empty;

        public DateTime DueDate { get; set; }

        public List<InvoiceItemRequest> Items { get; set; } = new();
    }
}