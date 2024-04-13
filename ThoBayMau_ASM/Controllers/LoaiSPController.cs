using Aram.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThoBayMau_ASM.Data;
using ThoBayMau_ASM.Models;

namespace ThoBayMau_ASM.Controllers
{
    public class LoaiSPController : Controller
    {
        private readonly ThoBayMau_ASMContext _context;
        public LoaiSPController(ThoBayMau_ASMContext context)
        {
            _context = context;
        }
        public const int ITEM_PER_PAGE = 5;

        [BindProperty(SupportsGet = true, Name = "p")]
        public int currentpage { get; set; }

        public int countpages { get; set; }
        public IActionResult Index()
        {
            ViewBag.LoaiSP = true;
            int total = _context.LoaiSP.Count();
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
                var result = _context.LoaiSP.Where(x => x.TrangThai == true).Skip((currentpage - 1) * ITEM_PER_PAGE).Take(ITEM_PER_PAGE).ToList();
                return View(result);
            }
            else
            {
                return View(null);
            }
        }
        public IActionResult Create()
        {
            ViewBag.LoaiSP = true;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(LoaiSP Loai)
        {
            var user = HttpContext.Session.GetJson<TaiKhoan>("User");
            if (user != null)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        Loai.TrangThai = true;
                        _context.Add(Loai);
                        _context.SaveChanges();
                        TempData["Sucess"] = "Thêm loại sản phẩm thành công!!";
                        
                        var ls = new LichSu
                        {
                            ThongTin_ThaoTac = $"Thêm loại sản phẩm",
                            NgayGio = DateTime.Now,
                            ChiTiet = $"Loại sản phẩm: {Loai.Id}",
                            TaiKhoanId = user.Id
                        };
                        _context.LichSu.Add(ls);
                        _context.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    return View(Loai);
                }
                catch (Exception ex)
                {
                    _context.Entry(Loai).State = EntityState.Detached;
                    var ls = new LichSu
                    {
                        ThongTin_ThaoTac = $"Thêm loại sản phẩm",
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
            ViewBag.LoaiSP = true;
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var loaifromdb = _context.LoaiSP.FirstOrDefault(x => x.Id == id);
            if (loaifromdb == null)
            {
                return NotFound();
            }
            return View(loaifromdb);
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(LoaiSP loai)
        {
            var user = HttpContext.Session.GetJson<TaiKhoan>("User");
            if (user != null)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        _context.Update(loai);
                        _context.SaveChanges();
                        TempData["Sucess"] = "Sửa loại sản phẩm thành công!!";
                        var ls = new LichSu
                        {
                            ThongTin_ThaoTac = $"Sửa loại sản phẩm",
                            NgayGio = DateTime.Now,
                            ChiTiet = $"Loại sản phẩm: {loai.Id}",
                            TaiKhoanId = user.Id
                        };
                        _context.LichSu.Add(ls);
                        _context.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    return View(loai);
                }
                catch (Exception ex)
                {
                    _context.Entry(loai).State = EntityState.Detached;
                    var ls = new LichSu
                    {
                        ThongTin_ThaoTac = $"Sửa loại sản phẩm",
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
        [HttpPost]
        [ValidateAntiForgeryToken]
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
                    var obj = _context.LoaiSP.FirstOrDefault(x => x.Id == txt_ID);
                    if (obj == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        obj.TrangThai = false;
                        _context.SaveChanges();
                        TempData["Sucess"] = "Xóa loại sản phẩm thành công";
                        var ls = new LichSu
                        {
                            ThongTin_ThaoTac = $"Xóa loại sản phẩm",
                            NgayGio = DateTime.Now,
                            ChiTiet = $"Loại sản phẩm: {obj.Id}",
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
                        ThongTin_ThaoTac = $"Xóa loại sản phẩm",
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
            ViewBag.LoaiSP = true;
            if (Key != null)
            {
                int total = _context.LoaiSP.Where(x => x.TrangThai == true && x.Ten.ToLower().Contains(Key.ToLower())).Count();
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
                    var result = _context.LoaiSP.Where(x => x.TrangThai == true && x.Ten.ToLower().Contains(Key.ToLower())).Skip((currentpage - 1) * ITEM_PER_PAGE).Take(ITEM_PER_PAGE).ToList();
                    return View("Index", result);
                }
                else
                {
                    return View("Index", null);
                }
            }
            else
            {
                int total = _context.LoaiSP.Where(x => x.TrangThai == true).Count();
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
                    var result = _context.LoaiSP.Where(x => x.TrangThai == true).Skip((currentpage - 1) * ITEM_PER_PAGE).Take(ITEM_PER_PAGE).ToList();
                    return View("Index", result);
                }
                else
                {
                    return View("Index", null);
                }
            }
        }
    }
}
