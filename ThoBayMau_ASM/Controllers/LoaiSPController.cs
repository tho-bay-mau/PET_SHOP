using Microsoft.AspNetCore.Mvc;
using ThoBayMau_ASM.Data;

namespace ThoBayMau_ASM.Controllers
{
    public class LoaiSPController : Controller
    {
        private readonly ThoBayMau_ASMContext _context;
        public LoaiSPController(ThoBayMau_ASMContext context)
        {
            // Khai báo constructor
            
            _context = context;
        }
        public IActionResult Index()
        {
            var loaiSP = _context.LoaiSP.ToList();
            return View(loaiSP);
        }

    }
}
