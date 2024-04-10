using Aram.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThoBayMau_ASM.Data;
using ThoBayMau_ASM.Models;

namespace ThoBayMau_ASM.Controllers
{
	public class AdminController : Controller
	{
        private readonly ThoBayMau_ASMContext _context;

        public AdminController(ThoBayMau_ASMContext context)
        {
            _context = context;
        }
        public IActionResult AccountName()
        {
            var user = HttpContext.Session.GetJson<TaiKhoan>("User");
            return Json(user);
        }
        public IActionResult Index()
		{
            var topProducts = _context.SPTop5.FromSqlRaw("EXEC SPTop5").ToList();
            ViewBag.SPTop5 = topProducts;
            return View();
		}
        
        [HttpGet]
        public IActionResult ThongKeDoanhThuTheoThang()
        {
            var data = _context.Set<ThongKeDoanhThu>().FromSqlInterpolated($"EXEC ThongKeDoanhThu").ToList();
            return Json(data);
        }
        [HttpGet]
        public IActionResult ThongKeDoanhThuTheoNgay()
        {
            var data = _context.Set<ThongKeDoanhThuTheoNgay>().FromSqlInterpolated($"EXEC ThongKeDoanhThuTheoNgay").ToList();
            return Json(data);
        }

    }
}
