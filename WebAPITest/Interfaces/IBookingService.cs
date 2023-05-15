using Microsoft.AspNetCore.Mvc;
using WebAPITest.Models;
using WebAPITest.Data;

namespace WebAPITest.Interfaces
{
    public interface IBookingService
    {
        Task<ActionResult<IEnumerable<Booking>>> GetAllBookings();
        Task<ActionResult<Booking>> GetOneBooking(int bookingId);
        Task<ActionResult<IEnumerable<Booking>>> GetBookingsByClient(string client);
        Task<ActionResult<IEnumerable<Booking>>> GetBookingByStatus(EnumStatusBooking status);
        Task<ActionResult<string>> GetAvailableCarsFromBooking(string branch, DateTime dateStart, DateTime dateEnd, string group);
        Task<bool> BookingExist(int bookingId);
        void AddBoking(Booking booking);
        void UpdateBooking(Booking booking);
        void DeleteBooking(Booking booking);
    }
}