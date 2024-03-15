using Microsoft.AspNetCore.Mvc;
using ThoBayMau_ASM.Data;
using ThoBayMau_ASM.Models;

namespace ThoBayMau_ASM.Controllers
{
    public class TaiKhoanController : Controller
    {
        private readonly ThoBayMau_ASMContext _db;

        public TaiKhoanController(ThoBayMau_ASMContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View(_db.TaiKhoan.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TaiKhoan obj)
        {
            if (ModelState.IsValid)
            {
                obj.NgayDangKy = DateTime.Now;
                obj.TrangThai = true;
                _db.TaiKhoan.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound(); 
            }
            var obj = _db.TaiKhoan.FirstOrDefault(x => x.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TaiKhoan obj)
        {
            if (ModelState.IsValid)
            {
                _db.TaiKhoan.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }
    }
}
