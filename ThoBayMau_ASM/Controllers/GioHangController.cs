using Aram.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;
using System.Text.RegularExpressions;
using ThoBayMau_ASM.Data;
using ThoBayMau_ASM.Models;
using ThoBayMau_ASM.Services;
using ThoBayMau_ASM.ViewModel;

namespace ThoBayMau_ASM.Controllers
{
    public class GioHangController : Controller
    {
        private readonly ThoBayMau_ASMContext _context;
        private readonly IVnPayService _vnPayService;

        public GioHangController(ThoBayMau_ASMContext context, IVnPayService vnPayService)
        {
            _context = context;
            _vnPayService = vnPayService;
        }

        public GioHang? GioHang { get; set; }

        public IActionResult Index()
        {
            var user = HttpContext.Session.GetJson<TaiKhoan>("User");
            if (user != null)
            {
                ViewBag.User = user;
            }
            ViewBag.HoTen = TempData["HoTen"];
            ViewBag.SDT = TempData["SDT"];
            ViewBag.DiaChi = TempData["DiaChi"];
            ViewBag.GhiChu = TempData["GhiChu"];
            ViewBag.errHoTen = TempData["errHoTen"];
            ViewBag.errSDT = TempData["errSDT"];
            ViewBag.errDiaChi = TempData["errDiaChi"];
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
        public IActionResult AddToDonHang(string HoTen, string SDT, string DiaChi, string GhiChu, TaiKhoan tk, string payment = "COD")
        {
            ViewBag.HoTen = HoTen;
            TempData["HoTen"] = ViewBag.HoTen;
            ViewBag.SDT = SDT;
            TempData["SDT"] = ViewBag.SDT;
            ViewBag.DiaChi = DiaChi;
            TempData["DiaChi"] = ViewBag.DiaChi;
            ViewBag.GhiChu = GhiChu;
            TempData["GhiChu"] = ViewBag.GhiChu;
            var User = HttpContext.Session.GetJson<TaiKhoan>("User");
            string returnUrl = Url.Action("Index", "GioHang");
            if (User == null)
            {
                TempData["Warning"] = "Đăng nhập để sử dụng chức năng";
                return RedirectToAction("Login", "DangNhapDangKi", new { returnUrl = returnUrl });
            }
            else
            {
                if (string.IsNullOrEmpty(HoTen))
                {
                    ViewBag.errHoTen = "Họ tên không được bỏ trống";
                    TempData["errHoTen"] = ViewBag.errHoTen;
                }
                else
                {
                    if (HoTen.Any(char.IsDigit))
                    {
                        ViewBag.errHoTen = "Họ tên không có kí tự số";
                        TempData["errHoTen"] = ViewBag.errHoTen;
                    }
                    if (Regex.IsMatch(HoTen, @"[^A-Za-z\sđĐ]+$"))
                    {
                        ViewBag.errHoTen = "Họ tên không được có kí tự đặc biệt";
                        TempData["errHoTen"] = ViewBag.errHoTen;
                    }
                }
                if (string.IsNullOrEmpty(SDT))
                {
                    ViewBag.errSDT = "SDT không được bỏ trống";
                    TempData["errSDT"] = ViewBag.errSDT;
                }
                else
                {
                    if (!Regex.IsMatch(SDT, @"^0\d{9,10}$"))
                    {
                        ViewBag.errSDT = "SDT không hợp lệ";
                        TempData["errSDT"] = ViewBag.errSDT;
                    }
                }
                if (string.IsNullOrEmpty(DiaChi))
                {
                    ViewBag.errDiaChi = "Địa chỉ không được bỏ trống";
                    TempData["errDiaChi"] = ViewBag.errDiaChi;
                }
                if (ViewBag.errHoTen != null || ViewBag.errSDT != null || ViewBag.errDiaChi != null)
                {
                    return RedirectToAction("Index", "GioHang");
                }
                GioHang = HttpContext.Session.GetJson<GioHang>("giohang") ?? new GioHang();
                var donHang = new DonHang();
                donHang.TaiKhoanId = User.Id;
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
                if (payment == "Thanh toán VNPay")
                {
                    var vnPayModel = new VnPaymentRequestModel
                    {
                        Amount = tinhTong(donHang.Id),
                        CreatedDate = DateTime.Now,
                        Description = $"{tk.DiaChi}{tk.SDT}",
                        OrderId = new Random().Next(1000, 10000),
                        ProductId = donHang.Id
                    };
                    return Redirect(_vnPayService.CreatePaymentUrl(HttpContext, vnPayModel));
                }
                return RedirectToAction("Index", "GioHang");
            }

        }
        public int tinhTong(int? Id)
        {
            int tong = 0;

            int phiship = 20000;
            var sp = _context.DonHang.Include(x => x.DonHang_ChiTiets)
                .ThenInclude(y => y.ChiTiet_SP)
                .FirstOrDefault(x => x.Id == Id);
            if (sp != null)
            {
                foreach (var x in sp.DonHang_ChiTiets)
                {
                    int tamtinh = 0;
                    tamtinh = x.SoLuong * x.ChiTiet_SP.Gia;
                    tong += tamtinh;
                }
                tong = tong + phiship;
            }
            return (tong);

        }
        
        public IActionResult PaymentCallBack()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);
            if (response == null || response.VnPayResponseCode != "00")
            {
                TempData["Error"] = $"Lỗi thanh toán VN Pay: {response.VnPayResponseCode}";
                return RedirectToAction("Index","GioHang");
            }
            // Lưu đơn hàng vào database
            TempData["Sucess"] = $"Thanh toán VN Pay thành công";
            return RedirectToAction("Index", "GioHang");

        }
        public IActionResult TTDH()
        {
            var user = HttpContext.Session.GetJson<TaiKhoan>("User");
            if (user != null)
            {
                ViewBag.User = user;
                var result = _context.DonHang.Where(x => x.TaiKhoanId == user.Id).ToList();
                return View(result);
            }
            else
            {
                return View(null);
            }
        }

    }
}