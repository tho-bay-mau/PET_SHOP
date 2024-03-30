using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                .Skip((currentpage - 1) * ITEM_PER_PAGE).Take(ITEM_PER_PAGE).ToList();
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
        public IActionResult DuyetDon(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var DonHang = _context.DonHang.FirstOrDefault(a => a.Id == id);
                DonHang.TrangThaiDonHang = "dang giao";
                _context.Update(DonHang);
                _context.SaveChanges();
                return RedirectToAction("Index", "DonHang");
            }
        }
        public IActionResult GiaoHangThanhCong(int id)
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
                return RedirectToAction("Index", "DonHang");
            }
        }
        public IActionResult HuyDon(int id, string? returnUrl)
        {
            
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var DonHang = _context.DonHang.FirstOrDefault(a => a.Id == id);
                DonHang.TrangThaiDonHang = "da huy";
                _context.Update(DonHang);
                _context.SaveChanges();

                if (returnUrl != null)
                {
                    return Redirect(returnUrl);
                } else
                {
                    return RedirectToAction("Index", "DonHang");
                }
                
            }
        }
        public IActionResult ThanhToan(string payment = "COD")
        {

            return View();
        }
    }
}
