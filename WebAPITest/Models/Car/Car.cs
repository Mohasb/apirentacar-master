using System.ComponentModel.DataAnnotations;

namespace WebAPITest.Models
{
    public class Car
    {
        [Key]
        public string? LicensePlate { get; set; }
        public string? Model { get; set; }
        public bool? Automatic { get; set; }
        public int? Doors { get; set; }
        public int? Seats { get; set; }
        public string? GasType { get; set; }
        public int? Suitcases { get; set; }
        public int? DayPrice { get; set; }
        public string? Group { get; set; }
        public string? Branch { get; set; }

        public Car(){}
        public Car(string LicensePlate, string Model, bool Automatic, int Doors, int Seats, string GasType, int Suitcases, int DayPrice, string Group, string Branch)
        {
            this.LicensePlate = LicensePlate;
            this.Model = Model;
            this.Automatic = Automatic;
            this.Doors = Doors;
            this.Seats = Seats;
            this.GasType = GasType;
            this.Suitcases = Suitcases;
            this.DayPrice = DayPrice;
            this.Group = Group;
            this.Branch = Branch;
        }
    }
}