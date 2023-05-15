using Microsoft.AspNetCore.Mvc;
using WebAPITest.Data;
using WebAPITest.Models;
using WebAPITest.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace WebAPITest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly ILogger<BookingController> _logger;
        private readonly IBookingService _booking;
        private readonly IBranchService _branch;
        private readonly IClientService _client;
        private readonly ICarService _car;
        private readonly ICardService _card;
        private readonly IPlanningService _planning;

        public BookingController(ILogger<BookingController> logger,IBookingService booking , IBranchService branch, IClientService client, ICarService car, ICardService card, IPlanningService planning)
        {
            _logger = logger;
            _booking = booking;
            _branch = branch;
            _client = client;
            _car = car;
            _card = card;
            _planning = planning;
        }

        [HttpGet(Name = "GetBookings")]
        public Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
            return _booking.GetAllBookings();
        }

        [HttpGet("{id}", Name = "GetBooking")]
        public ActionResult<Booking> GetBooking(int id)
        {
            var booking = _booking.GetOneBooking(id).Result;
            return booking.Value?.Id == 0
                ? NotFound()
                : booking;
        }
        
        [HttpGet("clientBookings/{clientDNI}", Name = "GetBookingsByDNI")]
        public ActionResult<IEnumerable<Booking>> GetBookingsByDNI(string clientDNI)
        {
            if(!_client.ClientExist(clientDNI).Result) return BadRequest();

            return _booking.GetBookingsByClient(clientDNI).Result;
        }

        [HttpGet("{branch}&{fInicio}&{fFinal}", Name = "GruposDisponibles")]
        public async Task<ActionResult<IEnumerable<PlanningResponse>>> GetBookingsFromSucursal(string branch, DateTime fInicio, DateTime fFinal)
        {
            if(!await _branch.BranchExist(branch)) return BadRequest();
            var sucursal = _branch.GetOneBranch(branch).Result.Value!;

            return _planning.GetAvailableByBranchAndDates(sucursal.Name!, fInicio, fFinal).Result;
        }

        [HttpGet("status/{statusName}", Name = "GetBookingByStatus")]
        public ActionResult<IEnumerable<Booking>> GetBookingsByStatus(string statusName)
        {
            if(!Enum.IsDefined(typeof(EnumStatusBooking), statusName)) return NotFound();
            var status = (EnumStatusBooking) Enum.Parse(typeof(EnumStatusBooking), statusName);
            return _booking.GetBookingByStatus(status).Result;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Booking>> Post(Booking booking)
        {
            if (await _booking.BookingExist(booking.Id) || !_client.ClientExist(booking.Client!).Result || !_card.CardExist(booking.Card!).Result) return BadRequest();
            if(booking.Group is null) return BadRequest();
            
            var licensePlate =  _booking.GetAvailableCarsFromBooking(booking.PickUp!, booking.DateStart, booking.DateEnd, booking.Group).Result.Value;
            Console.WriteLine("MATRICULA" + licensePlate);
            if(licensePlate == "") return BadRequest();
            booking.LicensePlate = licensePlate;
            _booking.AddBoking(booking);

            return new CreatedAtRouteResult("GetBooking", new { id = booking.Id }, booking);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Booking newBooking)
        {
            if(!await _booking.BookingExist(id)) return BadRequest();
            var booking = _booking.GetOneBooking(id).Result.Value!;

            if(newBooking.Client != null && await _client.ClientExist(newBooking.Client!))
                booking.Client = newBooking.Client;
            if(newBooking.LicensePlate != null && await _car.CarExist(newBooking.LicensePlate))
                booking.LicensePlate = newBooking.LicensePlate;
            if(newBooking.Status != booking.Status && Enum.IsDefined(typeof(EnumStatusBooking), newBooking.Status) && booking.Status != EnumStatusBooking.Cancelled)
            {
                booking.Status = newBooking.Status;
                if(booking.Status == EnumStatusBooking.Cancelled)
                {
                    var sucursalInicio = _branch.GetOneBranch(booking.PickUp!).Result.Value!;
                    if(booking.PickUp == booking.DropOff)
                    {
                        _planning.SumarDisponibleCanceladoMismaSucursal(sucursalInicio.Name!, booking.Group!, booking.DateStart, booking.DateEnd);
                    }
                    else
                    {
                        var sucursalFinal = _branch.GetOneBranch(booking.DropOff!).Result.Value!;
                        _planning.SumarDisponibleCanceladoDiferenteSucursal(sucursalInicio.Name!, sucursalFinal.Name!, booking.Group!, booking.DateStart, booking.DateEnd);
                    }
                }
            }

            _booking.UpdateBooking(booking);

            return new CreatedAtRouteResult("GetBooking", new { id = booking.Id }, booking);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Booking>> Delete(int id)
        {
            if(!await _booking.BookingExist(id)) return NotFound();

            var booking = _booking.GetOneBooking(id).Result.Value!;

            _booking.DeleteBooking(booking);

            return booking;
        }
    }
}