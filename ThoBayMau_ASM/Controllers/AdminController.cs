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
        public IActionResult Index()
		{
			return View();
		}
        [HttpGet]
        public IActionResult ThongKeDoanhThuTheoThang()
        {
            var data = _context.Set<ThongKeDoanhThu>().FromSqlRaw("EXEC ThongKeDoanhThu").ToList();
            return Json(data);
        }
        

    }
}
