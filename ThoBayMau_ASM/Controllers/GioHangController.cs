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
                return RedirectToAction("Index", "GioHang");
            }
			else
			{
				return Redirect(returnUrl);
			}
		}
		// ===> tăng số lượng 
		[HttpGet]
		public JsonResult TangSoLuong(int id)
		{
			var GioHang = HttpContext.Session.GetJson<GioHang>("giohang");
			var GioHang_line = GioHang.Lines.FirstOrDefault(p => p.ChiTiet_SP.Id == id);
			GioHang_line.SoLuong++;
			HttpContext.Session.SetJson("giohang", GioHang);

			var DonGia = GioHang_line.ChiTiet_SP.Gia;

			var json = new
			{
				SoLuong = GioHang_line.SoLuong,
				TongTienSanPham = GioHang_line.TongTienSp(),
				TamTinh = GioHang.TamTinh(),
				TongTien = GioHang.TongTien(),
			};

			return Json(json);

		}
		// giảm số lượng
		[HttpGet]
		public JsonResult GiamSoLuong(int id)
		{
			var GioHang = HttpContext.Session.GetJson<GioHang>("giohang");
			var GioHang_line = GioHang.Lines.FirstOrDefault(p => p.ChiTiet_SP.Id == id);
			GioHang_line.SoLuong--;
			HttpContext.Session.SetJson("giohang", GioHang);

			var DonGia = GioHang_line.ChiTiet_SP.Gia;

			var json = new
			{
				SoLuong = GioHang_line.SoLuong,
				TongTienSanPham = GioHang_line.TongTienSp(),
				TamTinh = GioHang.TamTinh(),
				TongTien = GioHang.TongTien(),
			};

			return Json(json);

		}
		[HttpGet]
		public JsonResult InputSoLuong(int id, int SoLuong)
		{
			GioHang = HttpContext.Session.GetJson<GioHang>("giohang");
			var GioHang_line = GioHang.Lines.FirstOrDefault(p => p.ChiTiet_SP.Id == id);
			GioHang_line.SoLuong = SoLuong;
			HttpContext.Session.SetJson("giohang", GioHang);

			var DonGia = _context.ChiTiet_SP.FirstOrDefault(a => a.Id == id);
			var json = new
			{
				SoLuong = GioHang_line.SoLuong,
				TongTienSanPham = GioHang_line.TongTienSp(),
				TamTinh = GioHang.TamTinh(),
				TongTien = GioHang.TongTien(),
			};
			return Json(json);

		}
		public IActionResult XoaSPGioHang(int Id)
		{
			GioHang = HttpContext.Session.GetJson<GioHang>("giohang");
			GioHang.Lines.FirstOrDefault(p => p.ChiTiet_SP.Id == Id);
			if (GioHang.Lines != null)
			{
				GioHang.RemoveSanPham(Id);
				HttpContext.Session.SetJson("giohang", GioHang);
			}
			return RedirectToAction(nameof(Index), GioHang);
		}
		public IActionResult AddToDonHang(string HoTen, string SDT, string DiaChi, string GhiChu)
		{
            GioHang = HttpContext.Session.GetJson<GioHang>("giohang") ?? new GioHang();
            var donHang = new DonHang();
			donHang.TaiKhoanId = 4;
            _context.Add(donHang);
            _context.SaveChanges();
            GioHang = HttpContext.Session.GetJson<GioHang>("giohang");
            foreach (var item in GioHang.Lines)
            {
                var donHang_chiTiet = new DonHang_ChiTiet();
                donHang_chiTiet.ChiTiet_SPId = item.ChiTiet_SP.Id;
                donHang_chiTiet.DonHangId = donHang.Id;
                donHang_chiTiet.SoLuong = item.SoLuong;
                _context.Add(donHang_chiTiet);
            }
            var TT_NH = new ThongTin_NhanHang();
            TT_NH.DonhangId = donHang.Id;
            TT_NH.HoTen = HoTen;
            TT_NH.SDT = SDT;
            TT_NH.DiaChi = DiaChi;
            TT_NH.GhiChu = GhiChu;
            _context.Add(TT_NH);
            _context.SaveChanges();
            HttpContext.Session.Remove("giohang");
            return RedirectToAction("Index", "GioHang");
        }

    }
}
