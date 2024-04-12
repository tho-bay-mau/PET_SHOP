using Microsoft.AspNetCore.Mvc;
using ThoBayMau_ASM.Data;

namespace ThoBayMau_ASM.Controllers
{
    public class HistoryController : Controller
    {
        private readonly ThoBayMau_ASMContext _db;

        public HistoryController(ThoBayMau_ASMContext db)
        {
            _db = db;
        }

        public const int ITEM_PER_PAGE = 7;

        [BindProperty(SupportsGet = true, Name = "p")]
        public int currentpage { get; set; }

        public int countpages { get; set; }
        public IActionResult Index()
        {
            ViewBag.LichSu = true;
            int total = _db.LichSu.Count();
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
                var result = _db.LichSu.OrderByDescending(x => x.NgayGio).Skip((currentpage - 1) * ITEM_PER_PAGE).Take(ITEM_PER_PAGE).ToList();
                return View(result);
            }
            else
            {
                return View(null);
            }
        }
        public IActionResult Search(string Key)
        {
            ViewBag.LichSu = true;
            if (Key != null)
            {
                int total = _db.LichSu.Where(x => x.ThongTin_ThaoTac.ToLower().Contains(Key.ToLower())).Count();
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
                    var result = _db.LichSu.Where(x => x.ThongTin_ThaoTac.ToLower().Contains(Key.ToLower())).OrderByDescending(x => x.NgayGio).Skip((currentpage - 1) * ITEM_PER_PAGE).Take(ITEM_PER_PAGE).ToList();
                    return View("Index", result);
                }
                else
                {
                    return View("Index", null);
                }
            }
            else
            {
                int total = _db.LichSu.Count();
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
                    var result = _db.LichSu.OrderByDescending(x => x.NgayGio).Skip((currentpage - 1) * ITEM_PER_PAGE).Take(ITEM_PER_PAGE).ToList();
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
