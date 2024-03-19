using Aram.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;
using ThoBayMau_ASM.Data;
using ThoBayMau_ASM.Models;

namespace ThoBayMau_ASM.Controllers
{
    public class GioHangController : Controller
    {
		private readonly ThoBayMau_ASMContext _context;

		public GioHangController(ThoBayMau_ASMContext context)
		{
			_context = context;
		}
		public GioHang? GioHang { get; set; }
		public IActionResult Index()
        {
			GioHang = HttpContext.Session.GetJson<GioHang>("giohang") ?? new GioHang();
			return View(GioHang);
        }
		[HttpPost]
		public IActionResult AddToGioHang(int sanPhamId, int? quantity, int? kichThuoc, string? returnUrl)
		{
			/*SanPham? sanPham = _context.SanPham
				.Where(x => x.Id == sanPhamId)
				.Include(x => x.ChiTietSPs)
				.FirstOrDefault(x => x.ChiTietSPs.Any(x => x.KichThuoc == kichThuoc));*/
			ChiTiet_SP? chiTietSP = _context.ChiTiet_SP
				.Include(x => x.SanPham)
				.FirstOrDefault(s => s.KichThuoc == kichThuoc && s.SanPhamId == sanPhamId);
			if (chiTietSP != null)
			{
				GioHang = HttpContext.Session.GetJson<GioHang>("giohang") ?? new GioHang();
				if (quantity != null)
				{
					GioHang.AddItem(chiTietSP, (int)quantity);
				}
				else
				{
					GioHang.AddItem(chiTietSP, 1);
				}
				HttpContext.Session.SetJson("giohang", GioHang);
			}

			if (string.IsNullOrEmpty(returnUrl))
			{
				return View("Index", GioHang);
			}
			else
			{
				return Redirect(returnUrl);
			}
		}
	}
}
