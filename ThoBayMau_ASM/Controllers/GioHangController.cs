﻿using Aram.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
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

        public const int ITEM_PER_PAGE = 10;

        [BindProperty(SupportsGet = true, Name = "p")]
        public int currentpage { get; set; }

        public int countpages { get; set; }

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
        public IActionResult AddToDonHang(string HoTen, string SDT, string DiaChi, string GhiChu, TaiKhoan tk, string now, string payment = "COD")
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
                    if (Regex.IsMatch(HoTen, @"[\p{P}\p{S}]"))
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
                if (GioHang.Lines.Count > 0)
                {
                    var donHang = new DonHang();
                    donHang.TaiKhoanId = User.Id;
                    _context.Add(donHang);
                    _context.SaveChanges();

                    foreach (var item in GioHang.Lines)
                    {
                        var donHang_chiTiet = new DonHang_ChiTiet();
                        donHang_chiTiet.ChiTiet_SPId = item.ChiTiet_SP.Id;
                        var SP = _context.ChiTiet_SP.FirstOrDefault(p => p.Id == item.ChiTiet_SP.Id);
                        if (SP != null)
                        {
                            SP.SoLuong -= item.SoLuong;
                            _context.Update(SP);
                            _context.SaveChanges();
                        }
                        donHang_chiTiet.DonHangId = donHang.Id;
                        donHang_chiTiet.SoLuong = item.SoLuong;
                        _context.Add(donHang_chiTiet);
                        _context.SaveChanges();
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
                    if (now == "early")
                    {
                        return RedirectToAction("TTDH", "GioHang");
                    } else
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
                }
            }
            return RedirectToAction("Index", "GioHang");        
        }

        public IActionResult PaymentUser(int Id)
        {
            var User = HttpContext.Session.GetJson<TaiKhoan>("User");
            
            
                var vnPayModel = new VnPaymentRequestModel
                {
                    Amount = tinhTong(Id),
                    CreatedDate = DateTime.Now,
                    Description = $"{User.DiaChi}{User.SDT}",
                    OrderId = new Random().Next(1000, 10000),
                    ProductId = Id
                };
                return Redirect(_vnPayService.CreatePaymentUrl(HttpContext, vnPayModel));
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
                return RedirectToAction("TTDH","GioHang");
            }
            var ID = int.Parse(response.OrderId);
            var sp = _context.DonHang.FirstOrDefault(x => x.Id == ID);
            if (sp != null)
            {
                sp.TrangThaiThanhToan = true;
                _context.SaveChanges();
            }
            TempData["Sucess"] = $"Thanh toán VN Pay thành công";
            return RedirectToAction("TTDH", "GioHang");

        }
        public IActionResult TTDH()
        {
            var user = HttpContext.Session.GetJson<TaiKhoan>("User");
            if (user != null)
            {
                int total = _context.DonHang.Where(x => x.TaiKhoanId == user.Id).Count();
                countpages = (int)Math.Ceiling((double)total / ITEM_PER_PAGE);

                if (currentpage < 1)
                {
                    currentpage = 1;
                }
                if (currentpage > countpages)
                {
                    currentpage = countpages;
                }

                ViewBag.CurrentPage = currentpage;
                ViewBag.CountPages = countpages;
                if (total > 0)
                {
                    var result = _context.DonHang
                        .Include(x => x.TaiKhoan)
                        .Include(x => x.ThongTin_NhanHang)
                        .Include(x => x.DonHang_ChiTiets)
                        .ThenInclude(x => x.ChiTiet_SP)
                        .ThenInclude(x => x.SanPham)
                        .ThenInclude(x => x.LoaiSP)
                        .Where(x => x.TaiKhoanId == user.Id)
                        .OrderByDescending(x => x.ThoiGianTao)
                        .Skip((currentpage - 1) * ITEM_PER_PAGE).Take(ITEM_PER_PAGE).ToList();
                    return View(result);
                }
                else
                {
                    return View(null);
                }
            }
            else
            {
                return NotFound();
            }
        }
        public IActionResult DatLai(int txt_ID)
        {
            var currentDate = DateTime.Now.Date;
            //đếm số lần đặt lại
            var user = HttpContext.Session.GetJson<TaiKhoan>("User");

            var dem = _context.DonHang
            .Where(y => y.TrangThaiDonHang == "da huy"
                        && y.TaiKhoanId == user.Id
                        && y.ThoiGianHuy.HasValue
                        && y.ThoiGianHuy.Value.Date == currentDate)
            .Count();
            if (dem > 5)
            {
                TempData["Error"] = "Bạn đẵ huỷ đơn quá nhiều vào hôm nay, thử lại vào ngày mai!";
                return RedirectToAction("TTDH", "GioHang");
            } else
            {
                var dh = _context.DonHang.Include(x => x.DonHang_ChiTiets)
                .Include(x => x.ThongTin_NhanHang)
                .FirstOrDefault(x => x.Id == txt_ID);
                if (dh == null)
                {
                    return NotFound();
                }
                else
                {
                    var DH = new DonHang();
                    DH.TaiKhoanId = dh.TaiKhoanId;
                    _context.Add(DH);
                    if (_context.SaveChanges() > 0)
                    {
                        foreach (var item in dh.DonHang_ChiTiets)
                        {
                            var donHang_chiTiet = new DonHang_ChiTiet();
                            donHang_chiTiet.ChiTiet_SPId = item.ChiTiet_SPId;
                            donHang_chiTiet.DonHangId = DH.Id;
                            donHang_chiTiet.SoLuong = item.SoLuong;
                            var SP = _context.ChiTiet_SP.FirstOrDefault(p => p.Id == item.Id);
                            if (SP != null)
                            {
                                SP.SoLuong -= item.SoLuong;
                                _context.Update(SP);
                                _context.SaveChanges();
                            }
                            _context.Add(donHang_chiTiet);
                            _context.SaveChanges();
                        }
                        var tt_nh = dh.ThongTin_NhanHang;
                        var TT_NH = new ThongTin_NhanHang();
                        TT_NH.DonhangId = DH.Id;
                        TT_NH.HoTen = tt_nh.HoTen;
                        TT_NH.SDT = tt_nh.SDT;
                        TT_NH.DiaChi = tt_nh.DiaChi;
                        TT_NH.GhiChu = tt_nh.GhiChu;
                        _context.Add(TT_NH);
                        _context.SaveChanges();
                    }
                }
                TempData["Sucess"] = $"Đặt lại đơn thành công";
                return RedirectToAction("TTDH", "GioHang");
            }
            
        }

    }
}