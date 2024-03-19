using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using ThoBayMau_ASM.Data;
using ThoBayMau_ASM.Models;
using Microsoft.AspNetCore;

namespace ThoBayMau_ASM.Controllers
{
	public class HomeController : Controller
	{
        private readonly ThoBayMau_ASMContext _db;

        public HomeController(ThoBayMau_ASMContext db)
        {
            _db = db;
        }

        public IActionResult Index()
		{
            var result = _db.SanPham.Include(x => x.ChiTietSPs).Include(x => x.Anhs).ToList();
			return View(result);
		}
        public IActionResult product_detail()
        {
            return View();
        }
        public IActionResult Cart()
        {
            return View();
        }
        public IActionResult Shop_list()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
	}
}
