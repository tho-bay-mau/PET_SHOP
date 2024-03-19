
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThoBayMau_ASM.Data;
using ThoBayMau_ASM.Models;
using System.IO;

namespace ThoBayMau_ASM.Controllers
{
    public class QLSanPhamController : Controller
    {
        private readonly ThoBayMau_ASMContext _context;
        private readonly IWebHostEnvironment _webhost;

        public QLSanPhamController(ThoBayMau_ASMContext context,IWebHostEnvironment webHostEnvironment)
        {
            // Khai báo constructor
            _webhost = webHostEnvironment;
            _context = context;
        }
        public IActionResult Index()
        {
            var result = _context.SanPham
                .Include(x => x.Anhs)
                .Include(x => x.ChiTietSPs)
                /*.Where(x => x.TrangThai == "Đang bán")*/
                .ToList();
            return View(result);
        }
        public IActionResult Create()
        {

            List<string> listTrangThai = new List<string> { "Đang bán", "Ngừng bán", "Mới" };
            ViewBag.TrangThai = new SelectList(listTrangThai);
            return View();

        }
        [HttpPost]
        public IActionResult Create( SanPham sp, IFormFile[] files)
        {
            List<string> listTrangThai = new List<string> { "Đang bán", "Ngừng bán", "Mới" };
            ViewBag.TrangThai = new SelectList(listTrangThai);
            
            if (ModelState.IsValid)
            {
                _context.SanPham.Add(sp);
                _context.SaveChanges();
                foreach (var item in files)
                {
                    Uploadfile(item);
                    Anh anh = new Anh();
                    anh.SanphamId = sp.Id;
                    anh.TenAnh = item.FileName;
                    _context.Anh.Add(anh);
                }
                _context.SaveChanges();
                TempData["Sucess"] = "Thêm sản phẩm thành công!!";
                return RedirectToAction("Index");
            }
            return View(sp);
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
        public IActionResult Edit(SanPham obj, IFormFile[] files)
        {
            List<string> listTrangThai = new List<string> { "Đang bán", "Ngừng bán", "Mới" };
            ViewBag.TrangThai = new SelectList(listTrangThai);
            if (ModelState.IsValid)
            {
                
                    _context.Update(obj);
                    _context.SaveChanges();
                    /*var anhdaco = _context.Anh.Where(x => x.SanphamId == obj.Id).ToList();
                    foreach (var existingImage in anhdaco)
                    {
                        _context.Anh.Remove(existingImage);
                        _context.SaveChanges();
                    }*/
                    foreach (var item in files)
                    {
                        Uploadfile(item);
                        Anh anh = new Anh();
                        anh.SanphamId = obj.Id;
                        anh.TenAnh = item.FileName;
                        _context.Anh.Add(anh);
                    }
                    _context.SaveChanges();
                    TempData["Sucess"] = "Sửa sản phẩm thành công!!";
                    return RedirectToAction("Index");
            }
            return View(obj);
        }
        public void Uploadfile(IFormFile file)
        {
            if (file != null)
            {
                string uploadDir = Path.Combine(_webhost.WebRootPath, "img"); // đưa ảnh vào file
                string filePath = Path.Combine(uploadDir,file.FileName); // đưa ảnh vào file
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
        }


    }
}
