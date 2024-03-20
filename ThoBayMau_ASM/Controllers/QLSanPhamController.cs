
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
            var detail = _context.ChiTiet_SP.ToList();
            
            return View(result);

        }
        public IActionResult Create()
        {
            ViewBag.LoaiSPid = new SelectList(_context.LoaiSP.OrderBy(x => x.Id), "Id", "Ten");
            List<string> listTrangThai = new List<string> { "Đang bán", "Ngừng bán", "Mới" };
            ViewBag.TrangThai = new SelectList(listTrangThai);
            return View();

        }
        [HttpPost]
        public IActionResult Create( SanPham sp, IFormFile[] files,int gia, int soluong, int kichthuoc, DateTime ngaysanxuat, DateTime hansudung)
        {
            List<string> listTrangThai = new List<string> { "Đang bán", "Ngừng bán", "Mới" };
            ViewBag.TrangThai = new SelectList(listTrangThai);
            
            if (ModelState.IsValid)
            {
                _context.Add(sp);
                _context.SaveChanges();
                var SP_CT = new ChiTiet_SP();
                SP_CT.SanPhamId = sp.Id;
                SP_CT.Gia = gia;
                SP_CT.SoLuong = soluong;
                SP_CT.KichThuoc = kichthuoc;
                SP_CT.NgaySanXuat = ngaysanxuat;
                SP_CT.HanSuDung = hansudung;
                _context.Add(SP_CT);
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
            var SanPhamID = _context.SanPham.FirstOrDefault(x => x.Id == id);
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
                var anhdaco = _context.Anh.Where(x => x.SanphamId == obj.Id).ToList();
                foreach (var existingImage in anhdaco)
                {
                    _context.Anh.Remove(existingImage);
                    _context.SaveChanges();
                }
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
        public IActionResult Delete(int? txt_ID)
        {
            if (txt_ID == null || txt_ID == 0)
            {
                return NotFound();
            }
            var obj = _context.SanPham.FirstOrDefault(x => x.Id == txt_ID);
            if (obj == null)
            {
                return NotFound();
            }
            else
            {
                obj.TrangThai = "Ngừng bán";
                _context.SaveChanges();
                TempData["Sucess"] = "Xóa sản phẩm thành công";
                return RedirectToAction("Index");
            }
        }

    }
}
