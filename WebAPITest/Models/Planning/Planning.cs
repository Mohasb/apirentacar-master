using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPITest.Models
{
    public class Planning
    {
        public DateTime Dia { get; set; }
        public string Branch { get; set; }
        public string? GroupName { get; set; }
        public int Available { get; set; }
        public Planning(DateTime Dia, string Branch, string GroupName, int Available)
        {
            this.Dia = Dia;
            this.Branch = Branch;
            this.GroupName = GroupName;
            this.Available = Available;
        }
    }
}