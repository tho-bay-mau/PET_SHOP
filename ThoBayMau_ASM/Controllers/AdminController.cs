using Microsoft.AspNetCore.Mvc;

namespace ThoBayMau_ASM.Controllers
{
	public class AdminController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
