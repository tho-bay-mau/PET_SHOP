using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThoBayMau_ASM.Data;
using ThoBayMau_ASM.Helpers;
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

        public const int ITEM_PER_PAGE = 5;

        [BindProperty(SupportsGet = true, Name = "p")]
        public int currentpage { get; set; }

        public int countpages { get; set; }

        public IActionResult Index()
        {
            ViewBag.TaiKhoan = true;
            int total = _db.TaiKhoan.Where(x => x.TrangThai == true).Count();
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
            if(total > 0)
            {
                var result = _db.TaiKhoan.Where(x => x.TrangThai == true).Skip((currentpage - 1) * ITEM_PER_PAGE).Take(ITEM_PER_PAGE).ToList();
                return View(result);
            } else
            {
                return View(null);
            }
        }

        public IActionResult Create()
        {
            ViewBag.TaiKhoan = true;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TaiKhoan obj)
        {
            if (_db.TaiKhoan.Any(x => x.TenTK == obj.TenTK))
            {
                ModelState.AddModelError("TenTK", "Tên tài khoản đã tồn tại");
            }
            if (_db.TaiKhoan.Any(x => x.SDT == obj.SDT))
            {
                ModelState.AddModelError("SDT", "SDT đã liên kết với tài khoản khác");
            }
            if (_db.TaiKhoan.Any(x => x.Email == obj.Email))
            {
                ModelState.AddModelError("Email", "Email đã liên kết với tài khoản khác");
            }
            if (ModelState.IsValid)
            {
                obj.NgayDangKy = DateTime.Now;
                obj.TrangThai = true;
                _db.TaiKhoan.Add(obj);
                _db.SaveChanges();
                TempData["Sucess"] = "Thêm tài khoản thành công";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Edit(int? id)
        {
            ViewBag.TaiKhoan = true;
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
            var tkNow = _db.TaiKhoan.FirstOrDefault(x => x.Id == obj.Id);
            if (tkNow == null)
            {
                return NotFound();
            }
            else
            {
                if (tkNow.SDT != obj.SDT)
                {
                    if (_db.TaiKhoan.Any(x => x.SDT == obj.SDT))
                    {
                        ModelState.AddModelError("SDT", "SDT đã liên kết với tài khoản khác");
                    }
                }
                if (tkNow.Email != obj.Email)
                {
                    if (_db.TaiKhoan.Any(x => x.Email == obj.Email))
                    {
                        ModelState.AddModelError("Email", "Email đã liên kết với tài khoản khác");
                    }
                }
            }
            if (ModelState.IsValid)
            {
                _db.Entry(tkNow).State = EntityState.Detached;
                _db.TaiKhoan.Update(obj);
                _db.SaveChanges();
                TempData["Sucess"] = "Chỉnh sửa tài khoản thành công";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? txt_ID)
        {
            if (txt_ID == null || txt_ID == 0)
            {
                return NotFound();
            }
            var obj = _db.TaiKhoan.FirstOrDefault(x => x.Id == txt_ID);
            if (obj == null)
            {
                return NotFound();
            }
            else
            {
                obj.TrangThai = false;
                _db.SaveChanges();
                TempData["Sucess"] = "Xóa tài khoản thành công";
                return RedirectToAction("Index");
            }
        }

        public IActionResult Search(string Key)
        {
            ViewBag.TaiKhoan = true;
            if (Key != null)
            {
                int total = _db.TaiKhoan.Where(x => x.TenTK.ToLower().Contains(Key.ToLower()) && x.TrangThai == true).Count();
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
                    var result = _db.TaiKhoan.Where(x => x.TenTK.ToLower().Contains(Key.ToLower()) && x.TrangThai == true).Skip((currentpage - 1) * ITEM_PER_PAGE).Take(ITEM_PER_PAGE).ToList();
                    return View("Index", result);
                }
                else
                {
                    return View("Index", null);
                }
            }
            else
            {
                int total = _db.TaiKhoan.Where(x => x.TrangThai == true).Count();
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
                    var result = _db.TaiKhoan.Where(x => x.TrangThai == true).Skip((currentpage - 1) * ITEM_PER_PAGE).Take(ITEM_PER_PAGE).ToList();
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
