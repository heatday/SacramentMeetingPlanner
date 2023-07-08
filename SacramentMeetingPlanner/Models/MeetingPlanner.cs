using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SacramentMeetingPlanner.Models
{
    public class MeetingPlanner
    {
        public int Id { get; set; }
        public DateTime MeetingDate { get; set; }
        public string ConductingLeader { get; set; }
        public string OpeningSong { get; set; }
        public string SacramentHymn { get; set; }
        public string ClosingSong { get; set; }
        public string IntermediateNumber { get; set; }
        public string OpeningPrayer { get; set; }
        public string ClosingPrayer { get; set; }
        [ForeignKey("MeetingPlannerId")]
        public List<Speaker> Speakers { get; set; }
        [ForeignKey("MeetingPlannerId")]
        public List<Hymn> Hymns { get; set; }
    }
}
