using Microsoft.AspNetCore.Mvc;

namespace InvoiceFlow.API.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
