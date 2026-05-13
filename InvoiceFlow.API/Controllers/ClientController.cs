using Microsoft.AspNetCore.Mvc;

namespace InvoiceFlow.API.Controllers
{
    public class ClientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
