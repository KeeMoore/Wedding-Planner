using Wedding_Planner.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Wedding_Planner.Attributes;

public class Wedding
{
    [Key]
    public int WeddingId { get; set; }
    [Required]
    public string? WedderOne { get; set; }
    [Required]
    public string? WedderTwo { get; set; }
    [Required]
    public DateTime Date { get; set; }
    [Required]
    public string? Address { get; set; }
    public int UserId { get; set; }

    public List<RSVP> RSVPs { get; set; } = new List<RSVP>();

    [NotMapped]
    public string Title => $"{WedderOne} & {WedderTwo}";

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public User? Creator { get; set; }
}
