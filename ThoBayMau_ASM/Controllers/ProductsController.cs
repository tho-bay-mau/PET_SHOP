using Microsoft.AspNetCore.Mvc;

namespace ThoBayMau_ASM.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
