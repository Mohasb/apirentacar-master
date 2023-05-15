using System.ComponentModel.DataAnnotations;

namespace WebAPITest.Models
{
    public class Group
    {
        [Key]
        public string? Name { get; set; }
        public string? Photo { get; set; }
        public Group(){}
        public Group(string Name, string Photo)
        {
            this.Name = Name;
            this.Photo = Photo;
        }
    }
}