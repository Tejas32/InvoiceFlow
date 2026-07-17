namespace InvoiceFlow.API.DTOs
{
    public class InvoiceItemRequest
    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }
    }
}