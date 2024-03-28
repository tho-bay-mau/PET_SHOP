using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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

        public IActionResult Index(string? trangThai)
        {
            var donhang = _context.DonHang
                .Include(x => x.TaiKhoan)
                .Include(x => x.ThongTin_NhanHang)
                .Include(x => x.DonHang_ChiTiets)
                .ThenInclude(x => x.ChiTiet_SP)
                .ThenInclude(x => x.SanPham)
                .ThenInclude(x => x.LoaiSP)
                .ToList();
            if(trangThai == null)
            {
                trangThai = "cho duyet";
                ViewBag.TrangThai = trangThai;
            }
            var dh = donhang.Where(x => x.TrangThaiDonHang == trangThai).ToList();
            return View(dh);
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
