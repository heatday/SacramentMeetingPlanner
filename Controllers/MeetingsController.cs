using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SacramentPlanner.Data;
using SacramentPlanner.Models;
using static System.Reflection.Metadata.BlobBuilder;

namespace SacramentPlanner.Controllers
{
    public class MeetingsController : Controller
    {
        private readonly SacramentPlannerContext _context;

        public MeetingsController(SacramentPlannerContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var meetings = _context.Meeting
                                       .ToList();

            if (meetings.Count() > 0) 
            {
                foreach (var item in meetings) {
                    var speakers = _context.Speaker
                                               .Where(s => s.Meeting == item.Id)
                                               .ToList();

                    System.Diagnostics.Debug.WriteLine($"meetingId = {item.Id}, speakers = {speakers.Count}");
                    if (speakers.Count() != 0)
                    {
                        ViewData[item.Id.ToString()] = speakers;
                    }
                    else
                    {
                        ViewData[item.Id.ToString()] = "none";
                    }
                }
            }

            return _context.Meeting != null ? 
                          View(await _context.Meeting.ToListAsync()) :
                          Problem("Entity set 'SacramentPlannerContext.Meeting'  is null.");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Meeting == null)
            {
                return NotFound();
            }

            var meeting = await _context.Meeting
                .FirstOrDefaultAsync(m => m.Id == id);
            if (meeting == null)
            {
                return NotFound();
            }
            ViewData["Quote"] = getQuote(meeting.Topic);
            var speakers = _context.Speaker
                                       .Where(q => q.Meeting == id)
                                       .ToList();

            if (speakers.Count == 0)
            {
                ViewData["Speakers"] = "none";
            }
            else {
                ViewData["Speakers"] = speakers;
            }

            return View(meeting);
        }

        public IActionResult Create()
        {
            generateLists();

            var model = new Meeting();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection collection, [Bind("Id,Congregation,MeetingDate,Conducting,OpeningPrayer,ClosingPrayer,OpeningHymn,SacramentHymn,IntermediateHymn,ClosingHymn,Topic")] Meeting meeting)
        {
            if (ModelState.IsValid)
            {
                _context.Add(meeting);
                await _context.SaveChangesAsync();
                int id = meeting.Id;
                addSpeakers(id, collection);

                return RedirectToAction(nameof(Index));
            }

            return View(meeting);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Meeting == null)
            {
                return NotFound();
            }

            var meeting = await _context.Meeting.FindAsync(id);
            if (meeting == null)
            {
                return NotFound();
            }

            var speakers = _context.Speaker
                                       .Where(s => s.Meeting == id)
                                       .ToList();

            ViewData["Speakers"] = speakers;
            ViewData["Speaker_Count"] = speakers.Count + 1;
            generateLists();

            ViewData["Conducting"] = meeting.Conducting;
            ViewData["OpeningPrayer"] = meeting.OpeningPrayer;
            ViewData["ClosingPrayer"] = meeting.ClosingPrayer;
            ViewData["OpeningHymn"] = meeting.OpeningHymn;
            ViewData["SacramentHymn"] = meeting.SacramentHymn;
            ViewData["IntermediateHymn"] = meeting.IntermediateHymn;
            ViewData["ClosingHymn"] = meeting.ClosingHymn;
            ViewData["Topic"] = meeting.Topic;

            return View(meeting);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormCollection collection, [Bind("Id,Congregation,MeetingDate,Conducting,OpeningPrayer,ClosingPrayer,OpeningHymn,SacramentHymn,IntermediateHymn,ClosingHymn,Topic")] Meeting meeting)
        {
            if (id != meeting.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    deleteSpeakers(id);
                    addSpeakers(id, collection);

                    _context.Update(meeting);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MeetingExists(meeting.Id))
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
            return View(meeting);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Meeting == null)
            {
                return NotFound();
            }

            var meeting = await _context.Meeting
                .FirstOrDefaultAsync(m => m.Id == id);
            if (meeting == null)
            {
                return NotFound();
            }

            ViewData["Quote"] = getQuote(meeting.Topic);
            var speakers = _context.Speaker
                                       .Where(s => s.Meeting == id)
                                       .ToList();

            // https://www.tutorialsteacher.com/mvc/viewbag-in-asp.net-mvc
            if (speakers.Count == 0)
            {
                ViewData["Speakers"] = "none";
            }
            else
            {
                ViewData["Speakers"] = speakers;
            }

            return View(meeting);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Meeting == null)
            {
                return Problem("Entity set 'SacramentPlannerContext.Meeting'  is null.");
            }
            var meeting = await _context.Meeting.FindAsync(id);
            if (meeting != null)
            {
                deleteSpeakers(id);

                _context.Meeting.Remove(meeting);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MeetingExists(int id)
        {
          return (_context.Meeting?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private void deleteSpeakers(int id)
        {
            var speakersToDelete = _context.Speaker.Where(r => r.Meeting == id);
            var speakersList = speakersToDelete.ToList();
            foreach (var speaker in speakersList)
            {
                _context.Speaker.Remove(speaker);
            }
            _context.SaveChanges();
        }

        private void addSpeakers(int id, IFormCollection collection)
        {
            string newName = "";
            string newSubject = "";

            foreach (string key in collection.Keys)
            {
                if (key.Contains("Speaker"))
                {
                    System.Diagnostics.Debug.WriteLine($"Speaker = {collection[key]}");
                    newName = collection[key];
                }
                if (key.Contains("Subject"))
                {
                    System.Diagnostics.Debug.WriteLine($"Subject = {collection[key]}");
                    newSubject = collection[key];
                }

                if (newName != "" && newSubject != "")
                {
                    Speaker newSpeaker = new Speaker();
                    newSpeaker.Meeting = id;
                    newSpeaker.Name = newName;
                    newSpeaker.Subject = newSubject;
                    System.Diagnostics.Debug.WriteLine($"Meeting = {id}, Speaker = {newName}, Subject={newSubject}");
                    _context.Speaker.AddRange(newSpeaker);
                    newName = "";
                    newSubject = "";
                }
            }
            _context.SaveChanges();
        }

        private void generateLists()
        {
            var members = _context.Member
                                       .Where(q => q.Bishopric != true)
                                       .ToList();
            ViewData["Members"] = members;
            var bishopric = _context.Member
                                       .Where(q => q.Bishopric == true)
                                       .ToList();
            ViewData["Bishopric"] = bishopric;
            var hymns = _context.Hymn
                                       .Where(q => q.Sacrament != true)
                                       .ToList();
            ViewData["Hymns"] = hymns;
            var sacramentHymns = _context.Hymn
                           .Where(q => q.Sacrament == true)
                           .ToList();
            ViewData["SacramentHymns"] = sacramentHymns;
            var topics = _context.Topic
                           .ToList();
            ViewData["Topics"] = topics;
        }

        private string getQuote(string topic)
        {
            string quote = "";

            if (topic != null) 
            {
                var random = new Random();
                var topics = _context.Topic
                               .Where(q => q.Name == topic)
                               .ToList();

                foreach (var item in topics)
                {
                    quote = topic + ": " + item.Quote;
                }
            }

            return quote;
        }
    }
}
