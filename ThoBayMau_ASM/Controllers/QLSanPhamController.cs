
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThoBayMau_ASM.Data;
using ThoBayMau_ASM.Models;
using System.IO;
using Aram.Infrastructure;

namespace ThoBayMau_ASM.Controllers
{
    public class QLSanPhamController : Controller
    {
        private readonly ThoBayMau_ASMContext _context;
        private readonly IWebHostEnvironment _webhost;

        public QLSanPhamController(ThoBayMau_ASMContext context, IWebHostEnvironment webHostEnvironment)
        {
            // Khai báo constructor
            _webhost = webHostEnvironment;
            _context = context;
        }
        public const int ITEM_PER_PAGE = 5;

        [BindProperty(SupportsGet = true, Name = "p")]
        public int currentpage { get; set; }

        public int countpages { get; set; }
        public IActionResult Index()
        {
            ViewBag.QLSanPham = true;
            int total = _context.SanPham.Include(x => x.Anhs).Where(x => x.TrangThai == "Đang bán" || x.TrangThai == "Mới").Count();
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
                var result = _context.SanPham.Include(x => x.Anhs).Where(x => x.TrangThai == "Đang bán" || x.TrangThai == "Mới").Skip((currentpage - 1) * ITEM_PER_PAGE).Take(ITEM_PER_PAGE).ToList();
                return View(result);
            }
            else
            {
                return View(null);
            }
        }
        public IActionResult Create()
        {
            ViewBag.QLSanPham = true;
            var loaiSPList = _context.LoaiSP.OrderBy(x => x.Id)
                                     .Select(x => new SelectListItem
                                     {
                                         Value = x.Id.ToString(),
                                         Text = x.Ten
                                     })
                                     .ToList();
            ViewBag.LoaiSPid = new SelectList(loaiSPList, "Value", "Text");
            List<string> listTrangThai = new List<string> { "Đang bán", "Ngừng bán", "Mới" };
            ViewBag.TrangThai = new SelectList(listTrangThai);
            return View();

        }
        [HttpPost]
        public IActionResult Create(SanPham sp, IFormFile[] files, int gia, int soluong, int kichthuoc, DateTime ngaysanxuat, DateTime hansudung)
        {

            var user = HttpContext.Session.GetJson<TaiKhoan>("User");
            if (user != null)
            {
                try
                {
                    var loaiSPList = _context.LoaiSP.OrderBy(x => x.Id)
                                     .Select(x => new SelectListItem
                                     {
                                         Value = x.Id.ToString(),
                                         Text = x.Ten
                                     })
                                     .ToList();
                    if (_context.SanPham.Any(x => x.Ten == sp.Ten))
                    {
                        ModelState.AddModelError("Ten", "Tên sản phẩm đã tồn tại");
                    }
                    ViewBag.LoaiSPid = new SelectList(loaiSPList, "Value", "Text");
                    List<string> listTrangThai = new List<string> { "Đang bán", "Ngừng bán", "Mới" };
                    ViewBag.TrangThai = new SelectList(listTrangThai);
                    if (gia <= 0)
                    {
                        ViewBag.ktGia = "Giá không hợp lệ";

                    }

                    if (soluong <= 0)
                    {
                        ViewBag.ktSoLuong = "Số lượng không hợp lệ";

                    }

                    if (kichthuoc <= 0)
                    {
                        ViewBag.ktKichThuoc = "Kích thước không hợp lệ";

                    }

                    if (ngaysanxuat >= hansudung)
                    {
                        ViewBag.ktNgay = "Ngày sản suất phải bé hơn hạn sử dụng";

                    }
                    if (ngaysanxuat == DateTime.MinValue && hansudung == DateTime.MinValue)
                    {
                        ViewBag.ktTrongNgay = "Vui lòng nhập ngày sản suất, hạn sử dụng";

                    }

                    if (ViewBag.ktGia != null || ViewBag.ktSoLuong != null || ViewBag.ktKichThuoc != null || ViewBag.ktNgay != null || ViewBag.ktTrongNgay != null)
                    {
                        return View(sp);
                    }
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
                            if (_context.Anh.Any(a => a.TenAnh == item.FileName))
                            {
                                ViewBag.ktAnh = "Ảnh đã tồn tại";
                                return View(sp);
                            }
                        }
                        _context.SaveChanges();
                        TempData["Sucess"] = "Thêm sản phẩm thành công!!";
                        var ls = new LichSu
                        {
                            ThongTin_ThaoTac = $"Thêm sản phẩm",
                            NgayGio = DateTime.Now,
                            ChiTiet = $"Sản phẩm: {sp.Id}, chi tiết sản phẩm: {SP_CT.Id}",
                            TaiKhoanId = user.Id
                        };
                        _context.LichSu.Add(ls);
                        _context.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    return View(sp);
                }
                catch (Exception ex)
                {
                    var ls = new LichSu
                    {
                        ThongTin_ThaoTac = $"Thêm sản phẩm",
                        NgayGio = DateTime.Now,
                        ChiTiet = $"Lỗi: {ex}",
                        TaiKhoanId = user.Id
                    };
                    _context.LichSu.Add(ls);
                    _context.SaveChanges();
                    TempData["Error"] = "Lỗi nghiêm trọng hãy báo IT để được hỗ trợ";
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }
        public IActionResult Edit(int? id)
        {
            ViewBag.QLSanPham = true;
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
            var danhSachAnh = _context.Anh.Where(x => x.SanphamId == id).ToList();
            ViewBag.DanhSachAnh = danhSachAnh;
            return View(SanPhamID);

        }
        [HttpPost]
        public IActionResult Edit(SanPham obj, IFormFile[] files)
        {
            List<string> listTrangThai = new List<string> { "Đang bán", "Ngừng bán", "Mới" };
            ViewBag.TrangThai = new SelectList(listTrangThai);

            var user = HttpContext.Session.GetJson<TaiKhoan>("User");
            if (user != null)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        _context.Update(obj);
                        _context.SaveChanges();
                        var anhdaco = _context.Anh.Where(x => x.SanphamId == obj.Id).ToList();
                        if (files.Length > 0)
                        {
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
                                if (_context.Anh.Any(a => a.TenAnh == item.FileName))
                                {
                                    ViewBag.ktAnh = "Ảnh đã tồn tại";
                                    return View(obj);
                                }
                            }
                            _context.SaveChanges(); 
                        }
                        TempData["Sucess"] = "Sửa sản phẩm thành công!!";
                        var ls = new LichSu
                        {
                            ThongTin_ThaoTac = $"Sửa sản phẩm",
                            NgayGio = DateTime.Now,
                            ChiTiet = $"Sản phẩm: {obj.Id}",
                            TaiKhoanId = user.Id
                        };
                        _context.LichSu.Add(ls);
                        _context.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    return View(obj);
                }
                catch (Exception ex)
                {
                    var ls = new LichSu
                    {
                        ThongTin_ThaoTac = $"Sửa sản phẩm",
                        NgayGio = DateTime.Now,
                        ChiTiet = $"Lỗi: {ex}",
                        TaiKhoanId = user.Id
                    };
                    _context.LichSu.Add(ls);
                    _context.SaveChanges();
                    TempData["Error"] = "Lỗi nghiêm trọng hãy báo IT để được hỗ trợ";
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }
        public void Uploadfile(IFormFile file)
        {
            if (file != null)
            {
                string uploadDir = Path.Combine(_webhost.WebRootPath, "img/products"); // đưa ảnh vào file
                string filePath = Path.Combine(uploadDir, file.FileName); // đưa ảnh vào file
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
        }
        public IActionResult Delete(int? txt_ID)
        {
            var user = HttpContext.Session.GetJson<TaiKhoan>("User");
            if (user != null)
            {
                try
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
                        var ls = new LichSu
                        {
                            ThongTin_ThaoTac = $"Xóa sản phẩm",
                            NgayGio = DateTime.Now,
                            ChiTiet = $"Sản phẩm: {obj.Id}",
                            TaiKhoanId = user.Id
                        };
                        _context.LichSu.Add(ls);
                        _context.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    var ls = new LichSu
                    {
                        ThongTin_ThaoTac = $"Xóa sản phẩm",
                        NgayGio = DateTime.Now,
                        ChiTiet = $"Lỗi: {ex}",
                        TaiKhoanId = user.Id
                    };
                    _context.LichSu.Add(ls);
                    _context.SaveChanges();
                    TempData["Error"] = "Lỗi nghiêm trọng hãy báo IT để được hỗ trợ";
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }
        public IActionResult Search(string Key)
        {
            ViewBag.QLSanPham = true;
            if (Key != null)
            {
                int total = _context.SanPham.Where(x => x.TrangThai == "Đang bán" || x.TrangThai == "Mới" && x.Id.ToString() == Key).Count();
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
                ViewBag.Search = Key;
                if (total > 0)
                {
                    var result = _context.SanPham.Where(x => x.TrangThai == "Đang bán" || x.TrangThai == "Mới" && x.Id.ToString() == Key).Skip((currentpage - 1) * ITEM_PER_PAGE).Take(ITEM_PER_PAGE).ToList();
                    return View("Index", result);
                }
                else
                {
                    return View("Index", null);
                }
            }
            else
            {
                int total = _context.SanPham.Where(x => x.TrangThai == "Đang bán" || x.TrangThai == "Mới").Count();

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
                ViewBag.Search = Key;
                if (total > 0)
                {
                    var result = _context.SanPham.Where(x => x.TrangThai == "Đang bán" || x.TrangThai == "Mới").Skip((currentpage - 1) * ITEM_PER_PAGE).Take(ITEM_PER_PAGE).ToList();
                    return View("Index", result);
                }
                else
                {
                    return View("Index", null);
                }
            }
        }
        public IActionResult GetImages(int? id_sp)
        {
            if (id_sp != null)
            {
                var danhSachHinh = _context.Anh
                    .Where(x => x.SanphamId == id_sp)
                    .ToList();

                if (danhSachHinh.Any())
                {
                    List<byte[]> danhSachByteHinh = new List<byte[]>();
                    foreach (var hinh in danhSachHinh)
                    {
                        var byteHinh = System.IO.File.ReadAllBytes("wwwroot/img/products/" + hinh.TenAnh);
                        danhSachByteHinh.Add(byteHinh);
                    }

                    return Ok(danhSachByteHinh); // Trả về danh sách các byte của hình ảnh
                }
                else
                {
                    return NotFound(); // Không tìm thấy hình ảnh cho ID sản phẩm đã cho
                }
            }
            else
            {
                return NotFound(); // ID sản phẩm không hợp lệ hoặc không tồn tại
            }
        }
    }
}
