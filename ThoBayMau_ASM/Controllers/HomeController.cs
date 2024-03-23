using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using ThoBayMau_ASM.Data;
using ThoBayMau_ASM.Models;
using Microsoft.AspNetCore;

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
            if (HttpContext.Session.GetString("UserName") != null)
            {
                // Nếu có dữ liệu người dùng trong Session, lấy nó ra
                string userName = HttpContext.Session.GetString("UserName");
                // Truyền userName vào ViewBag để sử dụng trong View
                ViewBag.UserName = userName;
            }


            var result = _db.SanPham.Include(x => x.ChiTietSPs).Include(x => x.Anhs).ToList();
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
            var result = _db.SanPham
                    .Include(x => x.ChiTietSPs)
                    .Include(x => x.Anhs)
                    .FirstOrDefault(x => x.Id == id_sp);
			ViewData["RelateProduct"] = _db.SanPham.Where(x => x.LoaiSPId == result.LoaiSPId).Include(x => x.ChiTietSPs).Include(x => x.Anhs).ToList();
			return View(result);
        }

        public IActionResult Cart()
        {
            return View();
        }
        public IActionResult Shop_list()
        {
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
                return View(result);
            }
            else
            {
                return View(null);
            }
        }
        public IActionResult List_Type(int id)
        {
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
                return View(result);
            }
            else
            {
                return View(null);
            }
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
    }
}
