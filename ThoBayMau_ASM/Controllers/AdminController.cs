﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThoBayMau_ASM.Data;
using ThoBayMau_ASM.Models;

namespace ThoBayMau_ASM.Controllers
{
	public class AdminController : Controller
	{
        private readonly ThoBayMau_ASMContext _context;

        public AdminController(ThoBayMau_ASMContext context)
        {
            _context = context;
        }
        public IActionResult Index()
		{
			return View();
		}
		public IActionResult DonHang()
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
            } else
            {
                var DonHang = _context.DonHang.FirstOrDefault(a => a.Id == id);
                DonHang.TrangThaiDonHang = "dang giao";
                _context.Update(DonHang);
                _context.SaveChanges();
                return RedirectToAction("DonHang", "Admin");
            }
        }
    }
}
