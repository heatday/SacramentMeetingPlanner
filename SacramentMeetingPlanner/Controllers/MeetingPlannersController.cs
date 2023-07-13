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
            if (id == null)
            {
                return NotFound();
            }

            var meetingPlanner = await _context.MeetingPlanner
                .Include(mp => mp.Speakers)
                .Include(mp => mp.Hymns)
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
            ViewData["Speakers"] = _context.Speaker.ToList();
            ViewData["Hymns"] = _context.Hymn.ToList();

            return View();
        }


        // POST: MeetingPlanners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MeetingDate,ConductingLeader,OpeningSong,SacramentHymn,ClosingSong,IntermediateNumber,OpeningPrayer,ClosingPrayer")] MeetingPlanner meetingPlanner, int[] Speakers, int[] Hymns)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the selected speakers and hymns from the provided IDs
                if (Speakers != null)
                {
                    meetingPlanner.Speakers = _context.Speaker.Where(s => Speakers.Contains(s.Id)).ToList();
                }

                if (Hymns != null)
                {
                    meetingPlanner.Hymns = _context.Hymn.Where(h => Hymns.Contains(h.Id)).ToList();
                }

                _context.Add(meetingPlanner);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Speakers"] = _context.Speaker.ToList();
            ViewData["Hymns"] = _context.Hymn.ToList();
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

            // Get the list of speakers and hymns
            var speakers = await _context.Speaker.ToListAsync();
            var hymns = await _context.Hymn.ToListAsync();

            // Assign the speaker and hymn data to ViewData
            ViewData["Speakers"] = speakers;
            ViewData["Hymns"] = hymns;

            return View(meetingPlanner);
        }


        // POST: MeetingPlanners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MeetingDate,ConductingLeader,OpeningSong,SacramentHymn,ClosingSong,IntermediateNumber,OpeningPrayer,ClosingPrayer")] MeetingPlanner meetingPlanner, int[] Speakers, int[] Hymns)
        {
            if (id != meetingPlanner.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingMeetingPlanner = await _context.MeetingPlanner
                        .Include(mp => mp.Speakers)
                        .Include(mp => mp.Hymns)
                        .FirstOrDefaultAsync(mp => mp.Id == id);

                    if (existingMeetingPlanner == null)
                    {
                        return NotFound();
                    }

                    // Update the scalar properties
                    existingMeetingPlanner.MeetingDate = meetingPlanner.MeetingDate;
                    existingMeetingPlanner.ConductingLeader = meetingPlanner.ConductingLeader;
                    existingMeetingPlanner.OpeningSong = meetingPlanner.OpeningSong;
                    existingMeetingPlanner.SacramentHymn = meetingPlanner.SacramentHymn;
                    existingMeetingPlanner.ClosingSong = meetingPlanner.ClosingSong;
                    existingMeetingPlanner.IntermediateNumber = meetingPlanner.IntermediateNumber;
                    existingMeetingPlanner.OpeningPrayer = meetingPlanner.OpeningPrayer;
                    existingMeetingPlanner.ClosingPrayer = meetingPlanner.ClosingPrayer;

                    // Update the speakers
                    if (Speakers != null)
                    {
                        var selectedSpeakers = await _context.Speaker.Where(s => Speakers.Contains(s.Id)).ToListAsync();
                        existingMeetingPlanner.Speakers.Clear();
                        existingMeetingPlanner.Speakers.AddRange(selectedSpeakers);
                    }

                    // Update the hymns
                    if (Hymns != null)
                    {
                        var selectedHymns = await _context.Hymn.Where(h => Hymns.Contains(h.Id)).ToListAsync();
                        existingMeetingPlanner.Hymns.Clear();
                        existingMeetingPlanner.Hymns.AddRange(selectedHymns);
                    }

                    _context.Update(existingMeetingPlanner);
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

            ViewData["Speakers"] = await _context.Speaker.ToListAsync();
            ViewData["Hymns"] = await _context.Hymn.ToListAsync();
            return View(meetingPlanner);
        }



        // GET: MeetingPlanners/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meetingPlanner = await _context.MeetingPlanner
                .Include(mp => mp.Speakers)
                .Include(mp => mp.Hymns)
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
            var meetingPlanner = await _context.MeetingPlanner
                .Include(mp => mp.Speakers)  // Include the Speakers
                .Include(mp => mp.Hymns)  // Include the Hymns
                .FirstOrDefaultAsync(mp => mp.Id == id);

            if (meetingPlanner == null)
            {
                return NotFound();
            }

            // Remove the meeting planner from the database
            _context.MeetingPlanner.Remove(meetingPlanner);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool MeetingPlannerExists(int id)
        {
            return (_context.MeetingPlanner?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
