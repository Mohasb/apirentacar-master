using System.ComponentModel.DataAnnotations;

namespace WebAPITest.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }
        public string? PickUp { get; set; }
        public string? DropOff { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string? Group { get; set; }
        public string? LicensePlate { get; set; }
        public string? Client { get; set; }
        public EnumStatusBooking Status { get; set; } = EnumStatusBooking.Pending;
        public int Card { get; set; }

        public Booking(){}
        public Booking(string PickUp, string DropOff, DateTime DateStart, DateTime DateEnd, string Group, string LicensePlate, string Client, int Card)
        {
            this.PickUp = PickUp;
            this.DropOff = DropOff;
            this.DateStart = DateStart;
            this.DateEnd = DateEnd;
            this.Group = Group;
            this.LicensePlate = LicensePlate;
            this.Client = Client;
            this.Card = Card;
        }
    }
    
}