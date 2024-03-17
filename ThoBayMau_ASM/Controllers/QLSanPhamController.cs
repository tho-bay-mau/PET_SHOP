
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThoBayMau_ASM.Data;
using ThoBayMau_ASM.Models;

namespace ThoBayMau_ASM.Controllers
{
    public class QLSanPhamController : Controller
    {
        private readonly ThoBayMau_ASMContext _context;

        public QLSanPhamController(ThoBayMau_ASMContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var spChiTiet = _context.SanPham
                .Include(x => x.ChiTietSPs)
                .Include(y => y.Anhs)
                .ToList();
            return View(spChiTiet);
        }
        public IActionResult Create()
        {
            List<string> listTrangThai = new List<string> { "Đang bán", "Ngừng bán", "Mới" };
            ViewBag.TrangThai = new SelectList(listTrangThai);

            return View();

        }
        [HttpPost]
        public IActionResult Create(SanPham obj)
        {
            List<string> listTrangThai = new List<string> { "Đang bán", "Ngừng bán", "Mới" };
            ViewBag.TrangThai = new SelectList(listTrangThai);
            if (ModelState.IsValid)
            {
                _context.Add(obj);
                _context.SaveChanges();
                TempData["Sucess"] = "Thêm sản phẩm thành công!!";
                return RedirectToAction("Index");
            }

            
            return View(obj);
        }
        public IActionResult Edit(int? id)
        {
            List<string> listTrangThai = new List<string> { "Đang bán", "Ngừng bán", "Mới" };
            ViewBag.TrangThai = new SelectList(listTrangThai);
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var SanPhamID = _context.SanPham.SingleOrDefault(x => x.Id == id);
            if (SanPhamID == null)
            {
                return NotFound();
            }
            return View(SanPhamID);

        }
        [HttpPost]
        public IActionResult Edit(SanPham obj)
        {
            List<string> listTrangThai = new List<string> { "Đang bán", "Ngừng bán", "Mới" };
            ViewBag.TrangThai = new SelectList(listTrangThai);
            if (ModelState.IsValid)
            {
                _context.Update(obj);
                _context.SaveChanges();
                TempData["Sucess"] = "Sửa sản phẩm thành công!!";
                return RedirectToAction("Index");
            }
            return View(obj);
        }


    }
}
