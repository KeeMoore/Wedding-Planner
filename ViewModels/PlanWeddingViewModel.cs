
using Wedding_Planner.Models;

namespace Wedding_Planner.ViewModels
{
    public class PlanWeddingViewModel
    {
        public string WeddingName { get; set; }
        public DateTime WeddingDate { get; set; }
        public string Location { get; set; }
        public List<Wedding> Weddings { get; set; }

        public PlanWeddingViewModel()
        {
            Weddings = new List<Wedding>();
        }
    }
}
