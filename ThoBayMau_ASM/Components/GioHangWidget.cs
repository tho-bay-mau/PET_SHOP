using ThoBayMau_ASM.Data;
using ThoBayMau_ASM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Aram.Infrastructure;

namespace ThoBayMau_ASM.Components
{
    public class GiohangWidget : ViewComponent
    {
        private readonly ThoBayMau_ASMContext _context;
		public GioHang? GioHang { get; set; }
		public GiohangWidget(ThoBayMau_ASMContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
			GioHang = HttpContext.Session.GetJson<GioHang>("giohang") ?? new GioHang();
            return View("Default", GioHang);
        }
    }
}
