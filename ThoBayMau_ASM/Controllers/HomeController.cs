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
        private readonly ThoBayMau_ASMContext _context;

        public HomeController(ThoBayMau_ASMContext context)
        {
            _context = context;
        }

        public IActionResult Index()
		{
			var sp = _context.SanPham
				.Include(x => x.LoaiSP)
				.Include(x => x.Anhs)
				.Include(x => x.ChiTietSPs).ToList();
			return View(sp);
		}
        // Đọc hình ảnh trong foder wwwroot/img/products
        public IActionResult GetImage(int? id_sp,int? id_hinh)
        {
			if(id_hinh != null)
			{
                var hinh = _context.Anh
                .FirstOrDefault(x => x.Id == id_hinh);
                var imageBytes = System.IO.File.ReadAllBytes("wwwroot/img/products/" + hinh.TenAnh + ".jpg"); // Đọc file ảnh thành mảng byte
                return File(imageBytes, "image/jpeg");
            } else if (id_sp != null)
            {
                var hinh = _context.Anh
                    .FirstOrDefault(x => x.SanphamId == id_sp);
                if (hinh != null)
                {
                    var imageBytes = System.IO.File.ReadAllBytes("wwwroot/img/products/" + hinh.TenAnh + ".jpg"); // Đọc file ảnh thành mảng byte
                    return File(imageBytes, "image/jpeg");
                } else
                {
                    return NotFound();
                }
                
            } else 
            {
                return NotFound();
            }
        }

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
