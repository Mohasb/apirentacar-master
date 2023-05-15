namespace WebAPITest.Models
{
    public class CarResponse
    {
        public string? LicensePlate { get; set; }

        public CarResponse(){}

        public CarResponse(string LicensePlate)
        {
            this.LicensePlate = LicensePlate;
        }
    }
}