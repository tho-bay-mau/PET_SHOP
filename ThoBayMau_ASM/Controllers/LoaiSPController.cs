using Microsoft.AspNetCore.Mvc;
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
                var result = _context.LoaiSP.Skip((currentpage - 1) * ITEM_PER_PAGE).Take(ITEM_PER_PAGE).ToList();
                return View(result);
            }
            else
            {
                return View(null);
            }
            /*var loaiSP = _context.LoaiSP.Where(x => x.TrangThai == true).ToList();
            return View(loaiSP);*/
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(LoaiSP Loai)
        {
            if (ModelState.IsValid)
            {
                _context.Add(Loai);
                _context.SaveChanges();
                TempData["Sucess"] = "Thêm loại sản phẩm thành công!!";
                return RedirectToAction("Index");
            }

            return View(Loai);
        }

        public IActionResult Edit(int? id)
        {
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
            if (ModelState.IsValid)
            {
                _context.Update(loai);
                _context.SaveChanges();
                TempData["Sucess"] = "Sửa loại sản phẩm thành công!!";
                return RedirectToAction("Index");
            }
            return View(loai);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? txt_ID)
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
                return RedirectToAction("Index");
            }

        }
        public IActionResult Search(string Key)
        {
            if (string.IsNullOrEmpty(Key))
            {
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
                ViewBag.Search = Key;
                if (total > 0)
                {
                    var result = _context.LoaiSP.Skip((currentpage - 1) * ITEM_PER_PAGE).Take(ITEM_PER_PAGE).ToList();
                    return View("Index", result);
                }
                else
                {
                    return View("Index", null);
                }
            }
            else
            {
                int total = _context.LoaiSP.Where(x => x.Ten == Key).Count();
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
                    var result = _context.LoaiSP.Where(x => x.Ten == Key).Skip((currentpage - 1) * ITEM_PER_PAGE).Take(ITEM_PER_PAGE).ToList();
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
