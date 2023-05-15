using System.ComponentModel.DataAnnotations;

namespace WebAPITest.Models
{
    public class Client
    {
        [Key]
        public string? DNI { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public int? Age { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public int? Phone { get; set; }
        public string? Address { get; set; }
        public int? CP { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }

        public Client() {}
        public Client(string DNI, string Name, string Surname, int Age, string Email, string Password, int Phone, string Address, int CP, string City, string Country)
        {
            this.DNI = DNI;
            this.Name = Name;
            this.Surname = Surname;
            this.Age = Age;
            this.Email = Email;
            this.Password = Password;
            this.Phone = Phone;
            this.Address = Address;
            this.CP = CP;
            this.City = City;
            this.Country = Country;
        }
    }
}