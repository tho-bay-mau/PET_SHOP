
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThoBayMau_ASM.Data;
using ThoBayMau_ASM.Models;
using ThoBayMau_ASM.Models.ViewModel;
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
            var SanPham = _context.SanPham.ToList();
            var HinhAnh = _context.Anhs.ToList();
            var result = SanPham.Where(x => x.TrangThai != "Ngừng bán").Select(p => new SanPhamViewModel
            {
                Id = p.Id,
                Ten = p.Ten,
                Mota = p.Mota,
                TrangThai = p.TrangThai,
                LoaiSPId = p.LoaiSPId,
                /*TenAnh = HinhAnh.Where(x => x.Id == p.Id).Select(x => x.TenAnh).ToList(),*/
            }).ToList();
            return View(result);
        }
        public IActionResult Create()
        {

            List<string> listTrangThai = new List<string> { "Đang bán", "Ngừng bán", "Mới" };
            ViewBag.TrangThai = new SelectList(listTrangThai);
            return View();

        }
        [HttpPost]
        public IActionResult Create( SanPhamViewModel obj)
        {
            List<string> listTrangThai = new List<string> { "Đang bán", "Ngừng bán", "Mới" };
            ViewBag.TrangThai = new SelectList(listTrangThai);
            if (ModelState.IsValid)
            {
                
                foreach (var item in obj.TenAnh)
                {
                    string stringFileName = Uploadfile(item);
                    var productImage = new Anh
                    {
                        TenAnh = stringFileName,
                        SanPham = obj.SanPham,
                    };
                    _context.Anhs.Add(productImage);
                }
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
        public string Uploadfile(IFormFile file)
        {
            string fileName = null;
            if (file != null)
            {
                
                string uploadDir = Path.Combine(_webhost.WebRootPath, "images"); // đưa ảnh vào file
                fileName = Guid.NewGuid().ToString() + "-" + file.FileName; // đưa ảnh vào file
                string filePath = Path.Combine(uploadDir, fileName); // đưa ảnh vào file
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
            return fileName;
        }

    }
}
