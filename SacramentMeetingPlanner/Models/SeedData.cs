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
                new Speaker { Name = "Gerrit Gong", Subject = "Happy and Forever" },
                new Speaker { Name = "Ulises Soares", Subject = "In Awe of Christ and His Gospel" },
                new Speaker { Name = "Dallin H. Oaks", Subject = "The Plan of Salvation" },
                new Speaker { Name = "Henry B. Eyring", Subject = "The Atonement of Jesus Christ" },
                new Speaker { Name = "M. Russell Ballard", Subject = "The Role of the Holy Ghost" },
                new Speaker { Name = "Quentin L. Cook", Subject = "Faith in Jesus Christ" },
             
            },
                    Hymns = new List<Hymn>
            {
                new Hymn { Number = "123", Title = "Oh, May My Soul Commune with Thee" },
                new Hymn { Number = "203", Title = "Angels We Have Heard on High" },
                new Hymn { Number = "86", Title = "How Firm a Foundation" },
                new Hymn { Number = "168", Title = "O God, the Eternal Father" },
                new Hymn { Number = "293", Title = "If You Could Hie to Kolob" },
                new Hymn { Number = "46", Title = "God of Our Fathers, Whose Almighty Hand" },
               
            }
                };

                context.MeetingPlanner.Add(meetingPlanner);
                context.SaveChanges();
            }
        }

    }

}
