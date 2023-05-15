using WebAPITest.Data;
using WebAPITest.Models;
using WebAPITest.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace WebAPITest.DataBase
{
    public class BookingDB : IBookingService
    {
        private readonly DataContext _context;
        private readonly ISaveChangesService _saveChanges;
        private readonly IBranchService _branch;
        private readonly IPlanningService _planning;
        public BookingDB(DataContext context, ISaveChangesService saveChanges, IPlanningService planning, IBranchService branch)
        {
            _context = context;
            _planning = planning;
            _saveChanges = saveChanges;
            _branch = branch;
        }
        public async Task<ActionResult<IEnumerable<Booking>>> GetAllBookings()
        {
            return await _context.Bookings.ToListAsync();
        }
        public async Task<ActionResult<Booking>> GetOneBooking(int bookingId)
        {
            return await _context.Bookings.FindAsync(bookingId) is Booking booking
                ? booking
                : new Booking();
        }
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookingsByClient(string client)
        {
            return await _context.Bookings.Where(booking => booking.Client == client).ToListAsync();
        }
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookingByStatus(EnumStatusBooking status)
        {
            return await _context.Bookings.Where(booking => booking.Status == status).ToListAsync();
        }

        public async Task<ActionResult<string>> GetAvailableCarsFromBooking(string branch, DateTime dateStart, DateTime dateEnd, string group)
        {
            var carList = await _context.Bookings.Where(booking => 
                    (booking.DateStart <= dateStart && dateStart < booking.DateEnd) ||
                    (booking.DateStart <= dateEnd && dateEnd <= booking.DateEnd) ||
                    (dateStart <= booking.DateStart && booking.DateEnd <= dateEnd)
                ).Select(booking => booking.LicensePlate
                ).Distinct().ToListAsync();

            var availableCars = await _context.Cars.Where(
                    car => car.Branch == branch
                    && !carList.Contains(car.LicensePlate)
                ).Select(
                    car => car.LicensePlate!
                ).ToListAsync();

            return availableCars.Count() != 0
                ? availableCars.First()
                : "";
        }

        public async Task<bool> BookingExist(int bookingId)
        {
            return await _context.Bookings.FindAsync(bookingId) is not null;
        }
        public void AddBoking(Booking booking)
        {
            _context.Add(booking);
            var sucursalInicio = _branch.GetOneBranch(booking.PickUp!).Result.Value!;
            if(sucursalInicio.Name is not null)
            {
                if(booking.PickUp == booking.DropOff)
                {
                    _planning.RestarDisponibleNuevaReservaMismaSucursal(sucursalInicio.Name!, booking.Group!, booking.DateStart, booking.DateEnd);
                }
                else
                {
                    var sucursalFinal = _branch.GetOneBranch(booking.DropOff!).Result.Value!;
                    if(sucursalFinal.Name is not null)
                        _planning.RestarDisponibleNuevaReservaDiferenteSucursal(sucursalInicio.Name!, sucursalFinal.Name!, booking.Group!, booking.DateStart, booking.DateEnd);
                } 
            }
            
            _saveChanges.SaveChangesDatabase();
        }
        public void UpdateBooking(Booking booking)
        {
            _context.Update(booking);
            _saveChanges.SaveChangesDatabase();
        }
        public void DeleteBooking(Booking booking)
        {
            _context.Remove(booking);
            _saveChanges.SaveChangesDatabase();
        }
    }
}