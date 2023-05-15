using System.Collections;

namespace WebAPITest.Models
{
    public class PlanningResponse
    {
        public string? Group {get; set; }
        public int Available {get; set; }

        public PlanningResponse(string? Group, int Available)
        {
            this.Group = Group;
            this.Available = Available;
        }
        
    }
}