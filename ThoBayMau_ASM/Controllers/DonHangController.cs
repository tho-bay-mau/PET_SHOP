using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aram.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using ThoBayMau_ASM.Data;
using ThoBayMau_ASM.Models;

namespace ThoBayMau_ASM.Controllers
{
    public class DonHangController : Controller
    {
        private readonly ThoBayMau_ASMContext _context;

        public DonHangController(ThoBayMau_ASMContext context)
        {
            _context = context;
        }
        public const int ITEM_PER_PAGE = 5;

        [BindProperty(SupportsGet = true, Name = "p")]
        public int currentpage { get; set; }

        public int countpages { get; set; }
        public IActionResult Index(string? trangThai)
        {
            ViewBag.DonHang = true;
            if (trangThai == null)
            {
                trangThai = "cho duyet";
                ViewBag.TrangThai = trangThai;
            }
            else
            {
                ViewBag.TrangThai = trangThai;
            }

            int total = _context.DonHang
                .Where(x => x.TrangThaiDonHang == trangThai)
                .Count();
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
                .Where(x => x.TrangThaiDonHang == trangThai)
                .Include(x => x.TaiKhoan)
                .Include(x => x.ThongTin_NhanHang)
                .Include(x => x.DonHang_ChiTiets)
                .ThenInclude(x => x.ChiTiet_SP)
                .ThenInclude(x => x.SanPham)
                .ThenInclude(x => x.LoaiSP)
                .Skip((currentpage - 1) * ITEM_PER_PAGE).Take(ITEM_PER_PAGE)
                .OrderByDescending(x => x.ThoiGianHuy == null ? x.ThoiGianTao : x.ThoiGianHuy)
                .ToList();
                int count = _context.DonHang.Where(x => x.TrangThaiDonHang == "cho duyet").Count();
                ViewBag.Count = count;
                return View(result);
            }
            else
            {
                return View(null);
            }
        }
        // duyệt đơn
        public IActionResult DuyetDon(int id, string? returnUrl)
        {
            var user = HttpContext.Session.GetJson<TaiKhoan>("User");
            if (user != null)
            {
                try
                {
                    if (id == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        var DonHang = _context.DonHang.FirstOrDefault(a => a.Id == id);
                        if(DonHang.TrangThaiDonHang == "cho duyet")
                        {
                            DonHang.TrangThaiDonHang = "dang giao";
                            DonHang.ThoiGianHuy = DateTime.Now;
                        } else
                        {
                            DonHang.TrangThaiDonHang = "da giao";
                            DonHang.ThoiGianHuy = DateTime.Now;
                        }
                        _context.Update(DonHang);
                        _context.SaveChanges();
                        TempData["Sucess"] = "Thành công";

                        var ls = new LichSu
                        {
                            ThongTin_ThaoTac = $"Duyệt đơn",
                            NgayGio = DateTime.Now,
                            ChiTiet = $"Đơn hàng: {DonHang.Id}",
                            TaiKhoanId = user.Id
                        };
                        _context.Add(ls);
                        _context.SaveChanges();
                        if(returnUrl != null)
                        {
                            return Redirect(returnUrl);
                        } else
                        {
                            return RedirectToAction("Index", "DonHang");
                        }
                    }
                }
                catch (Exception ex)
                {
                    var ls = new LichSu
                    {
                        ThongTin_ThaoTac = $"Duyệt đơn",
                        NgayGio = DateTime.Now,
                        ChiTiet = $"Lỗi: {ex}",
                        TaiKhoanId = user.Id
                    };
                    _context.Add(ls);
                    _context.SaveChanges();
                    TempData["Error"] = "Lỗi nghiêm trọng hãy báo IT để được hỗ trợ";
                    return RedirectToAction("Index", "DonHang");
                }
            }
            return NotFound();
        }
        public IActionResult GiaoHangThanhCong(int id)
        {
            var user = HttpContext.Session.GetJson<TaiKhoan>("User");
            if (user != null)
            {
                try
                {
                    if (id == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        var DonHang = _context.DonHang.FirstOrDefault(a => a.Id == id);
                        DonHang.TrangThaiDonHang = "da giao";
                        _context.Update(DonHang);
                        _context.SaveChanges();
                        TempData["Sucess"] = "Thành công";

                        var ls = new LichSu
                        {
                            ThongTin_ThaoTac = $"Đã giao",
                            NgayGio = DateTime.Now,
                            ChiTiet = $"Đơn hàng: {DonHang.Id}",
                            TaiKhoanId = user.Id
                        };
                        _context.Add(ls);
                        _context.SaveChanges();
                        return RedirectToAction("Index", "DonHang");
                    }
                }
                catch (Exception ex)
                {
                    var ls = new LichSu
                    {
                        ThongTin_ThaoTac = $"Đã giao",
                        NgayGio = DateTime.Now,
                        ChiTiet = $"Lỗi: {ex}",
                        TaiKhoanId = user.Id
                    };
                    _context.Add(ls);
                    _context.SaveChanges();
                    TempData["Error"] = "Lỗi nghiêm trọng hãy báo IT để được hỗ trợ";
                    return RedirectToAction("Index", "DonHang");
                }
            }
            return NotFound();
        }
        public IActionResult HuyDon(int txt_ID, string? returnUrl)
        {
            var user = HttpContext.Session.GetJson<TaiKhoan>("User");
            if (user != null)
            {
                try
                {
                    if (txt_ID == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        var DonHang = _context.DonHang
                            .Include(x => x.DonHang_ChiTiets)
                            .FirstOrDefault(a => a.Id == txt_ID);
                        DonHang.TrangThaiDonHang = "da huy";
                        DonHang.ThoiGianHuy = DateTime.Now;
                        foreach (var item in DonHang.DonHang_ChiTiets)
                        {
                            var sp = _context.ChiTiet_SP.FirstOrDefault(a => a.Id == item.ChiTiet_SPId);
                            sp.SoLuong += item.SoLuong;
                            _context.Update(sp);
                            _context.SaveChanges();
                        }
                        _context.Update(DonHang);
                        _context.SaveChanges();
                        TempData["Sucess"] = "Thành công";

                        var ls = new LichSu
                        {
                            ThongTin_ThaoTac = $"Hủy đơn",
                            NgayGio = DateTime.Now,
                            ChiTiet = $"Đơn hàng: {DonHang.Id}",
                            TaiKhoanId = user.Id
                        };
                        _context.Add(ls);
                        _context.SaveChanges();

                        if (returnUrl != null)
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "DonHang");
                        }
                    }
                }
                catch (Exception ex)
                {
                    var ls = new LichSu
                    {
                        ThongTin_ThaoTac = $"Hủy đơn",
                        NgayGio = DateTime.Now,
                        ChiTiet = $"Lỗi: {ex}",
                        TaiKhoanId = user.Id
                    };
                    _context.Add(ls);
                    _context.SaveChanges();
                    TempData["Error"] = "Lỗi nghiêm trọng hãy báo IT để được hỗ trợ";

                    if (returnUrl != null)
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "DonHang");
                    }
                }
            }
            return NotFound();
        }
    }
}
