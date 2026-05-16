namespace InvoiceFlow.API.DTOs
{
    public class RegisterRequest
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string BusinessName { get; set; }

        public string BusinessAddress { get; set; }

        public string Phone { get; set; }

        public string Industry { get; set; }

        public string Location { get; set; }
    }
}