using Wedding_Planner.Models;
using System.ComponentModel.DataAnnotations;

public class RSVP
{
    [Key]
    public int RSVPId { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public int WeddingId { get; set; }
    public Wedding? Wedding { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
