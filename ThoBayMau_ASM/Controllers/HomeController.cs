using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using ThoBayMau_ASM.Data;
using ThoBayMau_ASM.Models;
using Microsoft.AspNetCore;
using Aram.Infrastructure;
using Newtonsoft.Json;

namespace ThoBayMau_ASM.Controllers
{
    public class HomeController : Controller
    {
        private readonly ThoBayMau_ASMContext _db;

        public HomeController(ThoBayMau_ASMContext db)
        {
            _db = db;
        }
        public const int ITEM_PER_PAGE = 12;

        [BindProperty(SupportsGet = true, Name = "p")]
        public int currentpage { get; set; }

        public int countpages { get; set; }

        public IActionResult Index()
        {
            var user = HttpContext.Session.GetJson<TaiKhoan>("User");
            if (user != null)
            {
                ViewBag.User = user;
            }
            var result = _db.SanPham.Include(x => x.ChiTietSPs).Include(x => x.Anhs).ToList();
            ViewBag.Index = true;
            return View(result);
        }
        public IActionResult GetImage(int? id_sp, int? id_hinh)
        {
            if (id_hinh != null)
            {
                var hinh = _db.Anh
                .FirstOrDefault(x => x.Id == id_hinh);
                var imageBytes = System.IO.File.ReadAllBytes("wwwroot/img/products/" + hinh.TenAnh); // Đọc file ảnh thành mảng byte
                return File(imageBytes, "image/jpeg");
            }
            else if (id_sp != null)
            {
                var hinh = _db.Anh
                    .FirstOrDefault(x => x.SanphamId == id_sp);
                if (hinh != null)
                {
                    var imageBytes = System.IO.File.ReadAllBytes("wwwroot/img/products/" + hinh.TenAnh);
                    // Đọc file ảnh thành mảng byte
                    return File(imageBytes, "image/jpeg");
                }
                else
                {
                    return NotFound();
                }

            }
            else
            {
                return NotFound();
            }
        }
        public IActionResult product_detail(int id_sp)
        {
            var user = HttpContext.Session.GetJson<TaiKhoan>("User");
            if (user != null)
            {
                ViewBag.User = user;
            }
            var result = _db.SanPham
                    .Include(x => x.ChiTietSPs)
                    .Include(x => x.Anhs)
                    .FirstOrDefault(x => x.Id == id_sp);
            ViewData["RelateProduct"] = _db.SanPham.Where(x => x.LoaiSPId == result.LoaiSPId).Include(x => x.ChiTietSPs).Include(x => x.Anhs).ToList();
            return View(result);
        }
        public IActionResult Shop_list()
        {
            var user = HttpContext.Session.GetJson<TaiKhoan>("User");
            if (user != null)
            {
                ViewBag.User = user;
            }
            ViewBag.LoaiSP = _db.LoaiSP.OrderBy(x => x.Id).ToList();
            int total = _db.SanPham.Where(x => x.TrangThai == "Đang bán").Count();
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
                var result = _db.SanPham.Where(x => x.TrangThai == "Đang bán").Include(x => x.ChiTietSPs).Include(x => x.Anhs).Skip((currentpage - 1) * ITEM_PER_PAGE).Take(ITEM_PER_PAGE).ToList();
                ViewBag.ProductList = true;
                return View(result);
            }
            else
            {
                ViewBag.ProductList = true;
                return View(null);
            }
        }
        public IActionResult List_Type(int id)
        {
            var user = HttpContext.Session.GetJson<TaiKhoan>("User");
            if (user != null)
            {
                ViewBag.User = user;
            }
            ViewBag.SPID = id;
            ViewBag.LoaiSP = _db.LoaiSP.OrderBy(x => x.Id).ToList();
            int total = _db.SanPham.Where(x => x.TrangThai == "Đang bán" && x.LoaiSPId == id).Count();
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
                var result = _db.SanPham.Where(x => x.TrangThai == "Đang bán" && x.LoaiSPId == id).Include(x => x.ChiTietSPs).Include(x => x.Anhs).Skip((currentpage - 1) * ITEM_PER_PAGE).Take(ITEM_PER_PAGE).ToList();
                ViewBag.ProductList = true;
                return View(result);
            }
            else
            {
                ViewBag.ProductList = true;
                return View(null);
            }
        }
        public IActionResult About()
        {
            var user = HttpContext.Session.GetJson<TaiKhoan>("User");
            if (user != null)
            {
                ViewBag.User = user;
            }
            ViewBag.About = true;
            return View();
        }
        public IActionResult Contact()
        {
            var user = HttpContext.Session.GetJson<TaiKhoan>("User");
            if (user != null)
            {
                ViewBag.User = user;
            }
            ViewBag.Contact = true;
            return View();
        }
        public JsonResult Search()
        {
            var result = _db.SanPham
                        .Where(x => x.TrangThai == "Đang bán")
                        .Include(x => x.Anhs)
                        .GroupBy(x => x.Id)
                        .Select(group => group.First())
                        .ToList();
            string value = string.Empty;
            value = JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value);
        }
    }
}
