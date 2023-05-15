using System.Text.Json.Serialization;

namespace WebAPITest.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EnumStatusBooking
    {
        Pending = 1,
        InProgress = 2,
        Done = 3,
        Cancelled = 4
    }
}