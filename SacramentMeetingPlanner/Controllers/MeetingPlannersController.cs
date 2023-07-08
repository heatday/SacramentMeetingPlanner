using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SacramentMeetingPlanner.Data;
using SacramentMeetingPlanner.Models;

namespace SacramentMeetingPlanner.Controllers
{
    public class MeetingPlannersController : Controller
    {
        private readonly SacramentMeetingPlannerContext _context;

        public MeetingPlannersController(SacramentMeetingPlannerContext context)
        {
            _context = context;
        }

        // GET: MeetingPlanners
        public async Task<IActionResult> Index()
        {
            var meetingPlanners = await _context.MeetingPlanner
         .Include(mp => mp.Speakers)
         .Include(mp => mp.Hymns)
         .ToListAsync();

            return View(meetingPlanners);
        }


        // GET: MeetingPlanners/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.MeetingPlanner == null)
            {
                return NotFound();
            }

            var meetingPlanner = await _context.MeetingPlanner
                .FirstOrDefaultAsync(m => m.Id == id);
            if (meetingPlanner == null)
            {
                return NotFound();
            }

            return View(meetingPlanner);
        }

        // GET: MeetingPlanners/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MeetingPlanners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MeetingDate,ConductingLeader,OpeningSong,SacramentHymn,ClosingSong,IntermediateNumber,OpeningPrayer,ClosingPrayer")] MeetingPlanner meetingPlanner)
        {
            if (ModelState.IsValid)
            {
                _context.Add(meetingPlanner);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(meetingPlanner);
        }

        // GET: MeetingPlanners/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.MeetingPlanner == null)
            {
                return NotFound();
            }

            var meetingPlanner = await _context.MeetingPlanner.FindAsync(id);
            if (meetingPlanner == null)
            {
                return NotFound();
            }
            return View(meetingPlanner);
        }

        // POST: MeetingPlanners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MeetingDate,ConductingLeader,OpeningSong,SacramentHymn,ClosingSong,IntermediateNumber,OpeningPrayer,ClosingPrayer")] MeetingPlanner meetingPlanner)
        {
            if (id != meetingPlanner.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(meetingPlanner);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MeetingPlannerExists(meetingPlanner.Id))
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
            return View(meetingPlanner);
        }

        // GET: MeetingPlanners/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.MeetingPlanner == null)
            {
                return NotFound();
            }

            var meetingPlanner = await _context.MeetingPlanner
                .FirstOrDefaultAsync(m => m.Id == id);
            if (meetingPlanner == null)
            {
                return NotFound();
            }

            return View(meetingPlanner);
        }

        // POST: MeetingPlanners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.MeetingPlanner == null)
            {
                return Problem("Entity set 'SacramentMeetingPlannerContext.MeetingPlanner'  is null.");
            }
            var meetingPlanner = await _context.MeetingPlanner.FindAsync(id);
            if (meetingPlanner != null)
            {
                _context.MeetingPlanner.Remove(meetingPlanner);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MeetingPlannerExists(int id)
        {
            return (_context.MeetingPlanner?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
