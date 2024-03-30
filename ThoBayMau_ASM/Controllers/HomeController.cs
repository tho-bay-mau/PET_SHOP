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
            var topProducts = _db.SPTop5.FromSqlRaw("EXEC SPTop5").ToList();
            ViewBag.SPTop5 = topProducts;
            var result = _db.SanPham.Where(x => x.TrangThai == "Đang bán" || x.TrangThai == "Mới").Include(x => x.ChiTietSPs).Include(x => x.Anhs).ToList();
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
            var topProducts = _db.SPTop5.FromSqlRaw("EXEC SPTop5").ToList();
            ViewBag.SPTop5 = topProducts;
            var result = _db.SanPham
                    .Include(x => x.ChiTietSPs)
                    .Include(x => x.Anhs)
                    .FirstOrDefault(x => x.Id == id_sp);
            ViewData["RelateProduct"] = _db.SanPham.Where(x => (x.LoaiSPId == result.LoaiSPId) && (x.TrangThai == "Đang bán" || x.TrangThai == "Mới")).Include(x => x.ChiTietSPs).Include(x => x.Anhs).ToList();
            return View(result);
        }
        public IActionResult Shop_list()
        {
            var topProducts = _db.SPTop5.FromSqlRaw("EXEC SPTop5").ToList();
            ViewBag.SPTop5 = topProducts;
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
                var result = _db.SanPham.Where(x => x.TrangThai == "Đang bán" || x.TrangThai == "Mới").Include(x => x.ChiTietSPs).Include(x => x.Anhs).Skip((currentpage - 1) * ITEM_PER_PAGE).Take(ITEM_PER_PAGE).ToList();
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
            var topProducts = _db.SPTop5.FromSqlRaw("EXEC SPTop5").ToList();
            ViewBag.SPTop5 = topProducts;
            ViewBag.SPID = id;
            ViewBag.LoaiSP = _db.LoaiSP.OrderBy(x => x.Id).ToList();
            int total = _db.SanPham.Where(x => (x.TrangThai == "Đang bán" || x.TrangThai == "Mới") && x.LoaiSPId == id).Count();
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
                var result = _db.SanPham.Where(x => (x.TrangThai == "Đang bán" || x.TrangThai == "Mới") && (x.LoaiSPId == id)).Include(x => x.ChiTietSPs).Include(x => x.Anhs).Skip((currentpage - 1) * ITEM_PER_PAGE).Take(ITEM_PER_PAGE).ToList();
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
            ViewBag.About = true;
            return View();
        }
        public IActionResult Contact()
        {
            ViewBag.Contact = true;
            return View();
        }
        public IActionResult SearchProducts(string key)
        {
            if (key == null)
            {
                return View(null);
            }
            ViewBag.key = key;
            var topProducts = _db.SPTop5.FromSqlRaw("EXEC SPTop5").ToList();
            ViewBag.SPTop5 = topProducts;
            ViewData["RelateProduct"] = _db.SanPham.Where(x => x.TrangThai == "Đang bán" || x.TrangThai == "Mới").Include(x => x.ChiTietSPs).Include(x => x.Anhs).ToList();

            int total = _db.SanPham.Where(x => (x.Ten.ToLower().Contains(key.ToLower())) && (x.TrangThai == "Đang bán" || x.TrangThai == "Mới")).Count();
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
                var result = _db.SanPham
                            .Where(x => (x.Ten.ToLower().Contains(key.ToLower())) && (x.TrangThai == "Đang bán" || x.TrangThai == "Mới"))
                            .Include(x => x.ChiTietSPs)
                            .Include(x => x.Anhs)
                            .Skip((currentpage - 1) * ITEM_PER_PAGE)
                            .Take(ITEM_PER_PAGE)
                            .ToList();
                return View(result);
            }
            else
            {
                return View(null);
            }
        }
        public JsonResult Search()
        {
            var result = _db.SanPham
                        .Where(x => x.TrangThai == "Đang bán" || x.TrangThai == "Mới")
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
