#pragma warning disable CS8618
using Wedding_Planner.Models;

namespace Wedding_Planner.ViewModels
{
    public class WeddingDetailsViewModel
    {
        public int UserId { get; set; }
        public Wedding? Wedding { get; set; } 
        public RSVP RSVP { get; set; } 
    }
}
