using Microsoft.AspNetCore.Mvc;

namespace InvoiceFlow.API.Controllers
{
    public class InvoiceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
