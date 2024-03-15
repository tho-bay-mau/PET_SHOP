using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public IActionResult Index()
        {
            var donhang = _context.DonHang
                .Include(x => x.TaiKhoan)
                .ToList();
            return View(donhang);
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
                return RedirectToAction("DonHang", "Admin");
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
                return RedirectToAction("DonHang", "Admin");
            }
        }
    }
}
