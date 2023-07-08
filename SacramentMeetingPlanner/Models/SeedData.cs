using Microsoft.EntityFrameworkCore;
using SacramentMeetingPlanner.Data;
using SacramentMeetingPlanner.Models;

namespace SacramentMeetingPlanner.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new SacramentMeetingPlannerContext(
                serviceProvider.GetRequiredService<DbContextOptions<SacramentMeetingPlannerContext>>()))
            {
                // Check if data already exists
                if (context.MeetingPlanner.Any())
                {
                    return; // Data already seeded
                }

                // Add initial meeting planner data
                var meetingPlanner = new MeetingPlanner
                {
                    MeetingDate = DateTime.Now,
                    ConductingLeader = "Dallin H. Oaks",
                    OpeningSong = "Awake and Arise",
                    SacramentHymn = "Battle Hymn of the Republic",
                    ClosingSong = "Come, Rejoice",
                    IntermediateNumber = "25",
                    OpeningPrayer = "Russell Nelson",
                    ClosingPrayer = "Henry Eyring",
                    Speakers = new List<Speaker>
            {
                new Speaker { Name = "Speaker 1", Subject = "Subject 1" },
                new Speaker { Name = "Speaker 2", Subject = "Subject 2" },
                // Add more speakers if needed
            },
                    Hymns = new List<Hymn>
            {
                new Hymn { Number = "123", Title = "Oh, May My Soul Commune with Thee" },
                new Hymn { Number = "203", Title = "Angels We Have Heard on High" },
                // Add more hymns if needed
            }
                };

                context.MeetingPlanner.Add(meetingPlanner);
                context.SaveChanges();
            }
        }

    }

}
