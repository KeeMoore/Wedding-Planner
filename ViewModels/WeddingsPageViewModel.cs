using Wedding_Planner.Models;

namespace Wedding_Planner.ViewModels;

public class WeddingsPageViewModel
{
    public User? User { get; set; }
    public List<Wedding> Weddings { get; set; } = new List<Wedding>();
}