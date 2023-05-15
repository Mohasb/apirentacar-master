using System.ComponentModel.DataAnnotations;

namespace WebAPITest.Models
{
    public class Card
    {
        [Key]
        public int Id { get; set; }
        public string? NumberCode { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string? OwnerName { get; set; }
        public string? Client { get; set; }

        public Card(){}
        public Card(string NumberCode, DateTime ExpirationDate, string OwnerName, string Client)
        {
            this.NumberCode = NumberCode;
            this.ExpirationDate = ExpirationDate;
            this.OwnerName = OwnerName;
            this.Client = Client;
        }
    }
}