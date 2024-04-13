using Aram.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
        public IActionResult Index(int? SanPhamId)
        {
            ViewBag.QLSanPham = true;
            var detail = _context.ChiTiet_SP.Where(x => x.SanPhamId == SanPhamId).ToList();
            ViewBag.SanPhamId = SanPhamId;
            return View(detail);
        }
        public IActionResult Create(int? SanPhamId)
        {
            ViewBag.QLSanPham = true;
            var sp = _context.SanPham.FirstOrDefault(x => x.Id == SanPhamId);
            ViewBag.tenSP = sp.Ten;
            ViewBag.SanPhamId = sp.Id;
            return View();
        }
        [HttpPost]
        public IActionResult Create(ChiTiet_SP ct, int SanPhamId)
        {
            var user = HttpContext.Session.GetJson<TaiKhoan>("User");
            if (user != null)
            {
                try
                {
                    var SPList = _context.SanPham.OrderBy(x => x.Id)
                                    .Select(x => new SelectListItem
                                    {
                                        Value = x.Id.ToString(),
                                        Text = x.Ten
                                    })
                                    .ToList();
                    ViewBag.LoaiSPid = new SelectList(SPList, "Value", "Text");


                    if (ct.Gia == null)
                    {
                        ViewBag.ktgia = "Vui lòng nhập giá!!";
                    }

                    if (ct.SoLuong == null)
                    {
                        ViewBag.ktSoLuong = "Vui lòng nhập số lượng!!";
                    }

                    if (ct.KichThuoc == null)
                    {
                        ViewBag.ktKichThuoc = "Vui lòng nhập kích thước!!";
                    }

                    if (ct.NgaySanXuat >= ct.HanSuDung)
                    {
                        ModelState.AddModelError("NgaySanXuat", "Ngày sản suất phải bé hơn hạn sử dụng");
                    }

                    if (ct.NgaySanXuat == DateTime.MinValue)
                    {
                        ViewBag.ktNSX = "Vui lòng nhập ngày sản xuất!!";
                    }

                    if (ct.HanSuDung == DateTime.MinValue)
                    {
                        ViewBag.ktHSD = "Vui lòng nhập hạn sử dụng!!";
                    }
                    ct.SanPhamId = SanPhamId;
                    if (ModelState.IsValid)
                    {
                        _context.Add(ct);
                        _context.SaveChanges();
                        TempData["Sucess"] = "Thêm chi tiết sản phẩm thành công!!";
                        
                        var ls = new LichSu
                        {
                            ThongTin_ThaoTac = $"Thêm chi tiết sản phẩm",
                            NgayGio = DateTime.Now,
                            ChiTiet = $"Chi tiết sản phẩm: {ct.Id}",
                            TaiKhoanId = user.Id
                        };
                        _context.LichSu.Add(ls);
                        _context.SaveChanges();
                        return RedirectToAction("Index", "QLSanPham");
                    }
                    return View(ct);
                }
                catch (Exception ex)
                {
                    _context.Entry(ct).State = EntityState.Detached;
                    var ls = new LichSu
                    {
                        ThongTin_ThaoTac = $"Thêm chi tiết sản phẩm",
                        NgayGio = DateTime.Now,
                        ChiTiet = $"Lỗi: {ex}",
                        TaiKhoanId = user.Id
                    };
                    _context.Add(ls);
                    _context.SaveChanges();
                    TempData["Error"] = "Lỗi nghiêm trọng hãy báo IT để được hỗ trợ";
                    return RedirectToAction("Index", "QLSanPham");
                }
            }
            return NotFound();
        }
        public IActionResult Edit(int? id)
        {
            ViewBag.QLSanPham = true;
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var SanPhamID = _context.ChiTiet_SP.SingleOrDefault(x => x.Id == id);
            var SP = _context.SanPham.FirstOrDefault(X => X.Id == SanPhamID.SanPhamId);
            ViewBag.tenSP = SP.Ten;
            if (SanPhamID == null)
            {
                return NotFound();
            }

            return View(SanPhamID);
        }
        [HttpPost]
        public IActionResult Edit(ChiTiet_SP ct, int id)
        {
            var user = HttpContext.Session.GetJson<TaiKhoan>("User");
            if (user != null)
            {
                try
                { 
                    if (ct.Gia == null)
                    {
                        ViewBag.ktgia = "Vui lòng nhập giá!!";
                    }

                    if (ct.SoLuong == null)
                    {
                        ViewBag.ktSoLuong = "Vui lòng nhập số lượng!!";
                    }

                    if (ct.KichThuoc == null)
                    {
                        ViewBag.ktKichThuoc = "Vui lòng nhập kích thước!!";
                    }

                    if (ct.NgaySanXuat >= ct.HanSuDung)
                    {
                        ModelState.AddModelError("NgaySanXuat", "Ngày sản suất phải bé hơn hạn sử dụng");
                    }

                    if (ct.NgaySanXuat == DateTime.MinValue)
                    {
                        ViewBag.ktNSX = "Vui lòng nhập ngày sản xuất!!";
                    }

                    if (ct.HanSuDung == DateTime.MinValue)
                    {
                        ViewBag.ktHSD = "Vui lòng nhập hạn sử dụng!!";
                    }

                    if (ModelState.IsValid)
                    {
                        ct.SanPhamId = id;
                        _context.Update(ct);
                        _context.SaveChanges();
                        TempData["Sucess"] = "Sửa Chi tiết sản phẩm thành công!!";

                        var ls = new LichSu
                        {
                            ThongTin_ThaoTac = $"Sửa chi tiết sản phẩm",
                            NgayGio = DateTime.Now,
                            ChiTiet = $"Chi tiết sản phẩm: {ct.Id}",
                            TaiKhoanId = user.Id
                        };
                        _context.LichSu.Add(ls);
                        _context.SaveChanges();
                        string returnUrl = Url.Action("Index", "ChiTietSP", new { SanPhamId = ct.SanPhamId });
                        return Redirect(returnUrl);
                    }
                    return View(ct);
                }
                catch (Exception ex)
                {
                    _context.Entry(ct).State = EntityState.Detached;
                    var ls = new LichSu
                    {
                        ThongTin_ThaoTac = $"Sửa chi tiết sản phẩm",
                        NgayGio = DateTime.Now,
                        ChiTiet = $"Lỗi: {ex}",
                        TaiKhoanId = user.Id
                    };
                    _context.LichSu.Add(ls);
                    _context.SaveChanges();
                    TempData["Error"] = "Lỗi nghiêm trọng hãy báo IT để được hỗ trợ";
                    string returnUrl = Url.Action("Index", "ChiTietSP", new { SanPhamId = ct.SanPhamId });
                    return Redirect(returnUrl);
                }
            }
            return NotFound();
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

                        var ls = new LichSu
                        {
                            ThongTin_ThaoTac = $"Xóa chi tiết sản phẩm",
                            NgayGio = DateTime.Now,
                            ChiTiet = $"Chi tiết sản phẩm: {obj.Id}",
                            TaiKhoanId = user.Id
                        };
                        _context.LichSu.Add(ls);
                        _context.SaveChanges();
                        
                        return RedirectToAction("Index", "QLSanPham");
                    }
                }
                catch (Exception ex)
                {
                    var ls = new LichSu
                    {
                        ThongTin_ThaoTac = $"Xóa chi tiết sản phẩm",
                        NgayGio = DateTime.Now,
                        ChiTiet = $"Lỗi: {ex}",
                        TaiKhoanId = user.Id
                    };
                    _context.LichSu.Add(ls);
                    _context.SaveChanges();
                    TempData["Error"] = "Lỗi nghiêm trọng hãy báo IT để được hỗ trợ";
                    return RedirectToAction("Index", "QLSanPham");
                }
            }
            return NotFound();
        }
    }
}
