using Microsoft.AspNetCore.Mvc;

namespace InvoiceFlow.API.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
