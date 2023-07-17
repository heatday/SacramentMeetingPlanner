using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SacramentPlanner.Data;
using SacramentPlanner.Models;

namespace SacramentPlanner.Controllers
{
    public class HymnsController : Controller
    {
        private readonly SacramentPlannerContext _context;

        public HymnsController(SacramentPlannerContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
              return _context.Hymn != null ? 
                          View(await _context.Hymn.ToListAsync()) :
                          Problem("Entity set 'SacramentPlannerContext.Hymn'  is null.");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Page,Sacrament")] Hymn hymn)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hymn);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hymn);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Hymn == null)
            {
                return NotFound();
            }

            var hymn = await _context.Hymn.FindAsync(id);
            if (hymn == null)
            {
                return NotFound();
            }
            return View(hymn);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Page,Sacrament")] Hymn hymn)
        {
            if (id != hymn.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hymn);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HymnExists(hymn.Id))
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
            return View(hymn);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Hymn == null)
            {
                return NotFound();
            }

            var hymn = await _context.Hymn
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hymn == null)
            {
                return NotFound();
            }

            return View(hymn);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Hymn == null)
            {
                return Problem("Entity set 'SacramentPlannerContext.Hymn'  is null.");
            }
            var hymn = await _context.Hymn.FindAsync(id);
            if (hymn != null)
            {
                _context.Hymn.Remove(hymn);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HymnExists(int id)
        {
          return (_context.Hymn?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
