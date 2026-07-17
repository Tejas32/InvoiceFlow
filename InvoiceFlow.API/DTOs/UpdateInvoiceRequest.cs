namespace InvoiceFlow.API.DTOs
{
    public class UpdateInvoiceRequest
    {
        public int ClientId { get; set; }

        public string PaymentType { get; set; } = string.Empty;

        public DateTime DueDate { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}