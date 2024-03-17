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
    public class LoaiSPsController : Controller
    {
        private readonly ThoBayMau_ASMContext _context;

        public LoaiSPsController(ThoBayMau_ASMContext context)
        {
            _context = context;
        }

        // GET: LoaiSPs
        public async Task<IActionResult> Index()
        {
            return View(await _context.LoaiSP.ToListAsync());
        }

        // GET: LoaiSPs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loaiSP = await _context.LoaiSP
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loaiSP == null)
            {
                return NotFound();
            }

            return View(loaiSP);
        }

        // GET: LoaiSPs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LoaiSPs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Ten,TrangThai")] LoaiSP loaiSP)
        {
            if (ModelState.IsValid)
            {
                _context.Add(loaiSP);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(loaiSP);
        }

        // GET: LoaiSPs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loaiSP = await _context.LoaiSP.FindAsync(id);
            if (loaiSP == null)
            {
                return NotFound();
            }
            return View(loaiSP);
        }

        // POST: LoaiSPs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ten,TrangThai")] LoaiSP loaiSP)
        {
            if (id != loaiSP.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loaiSP);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoaiSPExists(loaiSP.Id))
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
            return View(loaiSP);
        }

        // GET: LoaiSPs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loaiSP = await _context.LoaiSP
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loaiSP == null)
            {
                return NotFound();
            }

            return View(loaiSP);
        }

        // POST: LoaiSPs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loaiSP = await _context.LoaiSP.FindAsync(id);
            if (loaiSP != null)
            {
                _context.LoaiSP.Remove(loaiSP);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoaiSPExists(int id)
        {
            return _context.LoaiSP.Any(e => e.Id == id);
        }
    }
}
