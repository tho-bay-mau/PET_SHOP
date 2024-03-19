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
		public IActionResult GetImage(int? id_sp, int? id_hinh)
		{
			if (id_hinh != null)
			{
				var hinh = _db.Anh
				.FirstOrDefault(x => x.Id == id_hinh);
				var imageBytes = System.IO.File.ReadAllBytes("wwwroot/img/" + hinh.TenAnh); // Đọc file ảnh thành mảng byte
				return File(imageBytes, "image/jpeg");
			}
			else if (id_sp != null)
			{
				var hinh = _db.Anh
					.FirstOrDefault(x => x.SanphamId == id_sp);
				if (hinh != null)
				{
                    var imageBytes = System.IO.File.ReadAllBytes("wwwroot/img/" + hinh.TenAnh);
					// Đọc file ảnh thành mảng byte
					return File(imageBytes, "image/jpeg");
				}
				else
				{
					return NotFound();
				}

			}
			else
			{
				return NotFound();
			}
		}
		public IActionResult product_detail(int id_sp)
        {
            var result = _db.SanPham
					.Include(x => x.ChiTietSPs)
					.Include(x => x.Anhs)
					.Where(x => x.Id == id_sp)
					.ToList();
            return View(result);
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
