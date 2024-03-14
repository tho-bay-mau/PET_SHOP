using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThoBayMau_ASM.Data;
using ThoBayMau_ASM.Models;

namespace ThoBayMau_ASM.Controllers
{
    public class LichSusController : Controller
    {
        private readonly ThoBayMau_ASMContext _context;

        public LichSusController(ThoBayMau_ASMContext context)
        {
            _context = context;
        }

        // GET: LichSus
        public async Task<IActionResult> Index()
        {
            var thoBayMau_ASMContext = _context.LichSu.Include(l => l.TaiKhoan);
            return View(await thoBayMau_ASMContext.ToListAsync());
        }

        // GET: LichSus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lichSu = await _context.LichSu
                .Include(l => l.TaiKhoan)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lichSu == null)
            {
                return NotFound();
            }

            return View(lichSu);
        }

        // GET: LichSus/Create
        public IActionResult Create()
        {
            ViewData["TaiKhoanId"] = new SelectList(_context.Set<TaiKhoan>(), "Id", "Id");
            return View();
        }

        // POST: LichSus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ThongTin_ThaoTac,NgayGio,ChiTiet,TaiKhoanId")] LichSu lichSu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lichSu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TaiKhoanId"] = new SelectList(_context.Set<TaiKhoan>(), "Id", "Id", lichSu.TaiKhoanId);
            return View(lichSu);
        }

        // GET: LichSus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lichSu = await _context.LichSu.FindAsync(id);
            if (lichSu == null)
            {
                return NotFound();
            }
            ViewData["TaiKhoanId"] = new SelectList(_context.Set<TaiKhoan>(), "Id", "Id", lichSu.TaiKhoanId);
            return View(lichSu);
        }

        // POST: LichSus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ThongTin_ThaoTac,NgayGio,ChiTiet,TaiKhoanId")] LichSu lichSu)
        {
            if (id != lichSu.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lichSu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LichSuExists(lichSu.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TaiKhoanId"] = new SelectList(_context.Set<TaiKhoan>(), "Id", "Id", lichSu.TaiKhoanId);
            return View(lichSu);
        }

        // GET: LichSus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lichSu = await _context.LichSu
                .Include(l => l.TaiKhoan)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lichSu == null)
            {
                return NotFound();
            }

            return View(lichSu);
        }

        // POST: LichSus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lichSu = await _context.LichSu.FindAsync(id);
            if (lichSu != null)
            {
                _context.LichSu.Remove(lichSu);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LichSuExists(int id)
        {
            return _context.LichSu.Any(e => e.Id == id);
        }
    }
}
