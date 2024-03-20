using Microsoft.AspNetCore.Mvc;
using ThoBayMau_ASM.Data;
using ThoBayMau_ASM.Models;

namespace ThoBayMau_ASM.Controllers
{
    public class ChiTietSPController : Controller
    {
        private readonly ThoBayMau_ASMContext _context;
        private readonly IWebHostEnvironment _webhost;

        public ChiTietSPController(ThoBayMau_ASMContext context, IWebHostEnvironment webHostEnvironment)
        {
            // Khai báo constructor
            _webhost = webHostEnvironment;
            _context = context;
        }
        public IActionResult Index(int? Id)
        {
            var detail = _context.ChiTiet_SP.Where(x => x.SanPhamId == Id).ToList();
            return View(detail);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(ChiTiet_SP ct) 
        {
            if (ModelState.IsValid)
            {
                _context.Add(ct);
                _context.SaveChanges();
                TempData["Sucess"] = "Thêm chi tiết sản phẩm thành công!!";
                return RedirectToAction("Index");
            }
            return View(ct);
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var SanPhamID = _context.ChiTiet_SP.SingleOrDefault(x => x.Id == id);
            if (SanPhamID == null)
            {
                return NotFound();
            }
            return View(SanPhamID);
        }
        [HttpPost]
        public IActionResult Edit(ChiTiet_SP ct)
        {
            if (ModelState.IsValid)
            {
                _context.Update(ct);
                _context.SaveChanges();
                TempData["Sucess"] = "Sửa Chi tiết sản phẩm thành công!!";
                return RedirectToAction("Index");
            }
            return View(ct);
        }
        public IActionResult Delete(int? txt_ID)
        {
            if (txt_ID == null || txt_ID == 0)
            {
                return NotFound();
            }
            var obj = _context.ChiTiet_SP.FirstOrDefault(x => x.Id == txt_ID);
            if (obj == null)
            {
                return NotFound();
            }
            else
            {
                obj.TrangThai = false;
                _context.SaveChanges();
                TempData["Sucess"] = "Xóa chi tiết sản phẩm thành công";
                return RedirectToAction("Index");
            }
        }

    }
}
