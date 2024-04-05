using Aram.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            ViewBag.QLSanPham = true;
            var detail = _context.ChiTiet_SP.Where(x => x.SanPhamId == Id).ToList();
            return View(detail);
        }
        public IActionResult Create()
        {
            ViewBag.QLSanPham = true;
            return View();
        }
        [HttpPost]
        public IActionResult Create(ChiTiet_SP ct) 
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


                    if (ct.Gia == 0)
                    {
                        ViewBag.ktgia = "Vui lòng nhập giá!!";
                    }

                    if (ct.SoLuong == 0)
                    {
                        ViewBag.ktSoLuong = "Vui lòng nhập số lượng!!";
                    }

                    if (ct.KichThuoc == 0)
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
                    var ls = new LichSu
                    {
                        ThongTin_ThaoTac = $"Thêm chi tiết sản phẩm",
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
        public IActionResult Edit(int? id)
        {
            ViewBag.QLSanPham = true;
            var SPList = _context.SanPham.OrderBy(x => x.Id)
                                    .Select(x => new SelectListItem
                                    {
                                        Value = x.Id.ToString(),
                                        Text = x.Ten
                                    })
                                    .ToList();
            ViewBag.LoaiSPid = new SelectList(SPList, "Value", "Text");

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


                    if (ct.Gia == 0)
                    {
                        ViewBag.ktgia = "Vui lòng nhập giá!!";
                    }

                    if (ct.SoLuong == 0)
                    {
                        ViewBag.ktSoLuong = "Vui lòng nhập số lượng!!";
                    }

                    if (ct.KichThuoc == 0)
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
                        return RedirectToAction("Index", "QLSanPham");
                    }
                    return View(ct);
                }
                catch (Exception ex)
                {
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
                    return RedirectToAction("Index", "QLSanPham");
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
