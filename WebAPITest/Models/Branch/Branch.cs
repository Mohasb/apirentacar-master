using System.ComponentModel.DataAnnotations;

namespace WebAPITest.Models
{
    public class Branch
    {
        [Key]
        public string? Name { get; set; }

        public Branch(){}
        public Branch(string Name)
        {
            this.Name = Name;
        }
    }
}