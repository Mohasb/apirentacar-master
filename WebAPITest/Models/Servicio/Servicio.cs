using System.ComponentModel.DataAnnotations;

namespace WebAPITest.Models
{
    public class Servicio
    {
        [Key]
        public string? Code { get; set; }
        public int AmountPerDay { get; set; }
        public string? Name { get; set; }

        public Servicio(){}
        public Servicio(string Code, int AmountPerDay, string Name)
        {
            this.Code = Code;
            this.AmountPerDay = AmountPerDay;
            this.Name = Name;
        }
    }
}