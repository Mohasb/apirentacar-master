using WebAPITest.Data;
using WebAPITest.Models;
using WebAPITest.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPITest.Controllers
{
    [Route("[controller]")]
    public class ServicioController : Controller
    {
        private readonly ILogger<ServicioController> _logger;
        private readonly IServicioService _service;

        public ServicioController(ILogger<ServicioController> logger, IServicioService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet(Name = "GetServicios")]
        public Task<ActionResult<IEnumerable<Servicio>>> GetAllServices()
        {
            return _service.GetAllServices();
        }
        
        [HttpGet("{code}", Name = "GetService")]
        public ActionResult<Servicio> GetService(string code)
        {
            var service = _service.GetOneService(code).Result;
            return service.Value?.Name is null
                ? NotFound()
                : service;
        }

        [HttpPost]
        public async Task<ActionResult<Servicio>> Post(Servicio service)
        {
            if(!await _service.ServiceExist(service.Code!)) return BadRequest();

            _service.AddService(service);

            return new CreatedAtRouteResult("GetService", service);
        }

        [HttpPut("{code}")]
        public async Task<IActionResult> Put(string code, Servicio newServicio)
        {
            if(!await _service.ServiceExist(code)) return NotFound();
            var servicio = _service.GetOneService(code).Result.Value!;

            if(newServicio.Name is not null)
                servicio.Name = newServicio.Name;
            if(newServicio.AmountPerDay >= 0)
                servicio.AmountPerDay = newServicio.AmountPerDay;

            _service.UpdateService(servicio);

            return new CreatedAtRouteResult("GetSerbice", servicio);
        }

        [HttpDelete("{code}")]
        public async Task<ActionResult<Servicio>> Delete(string code)
        {
            if(!await _service.ServiceExist(code)) return NotFound();

            var servicio = _service.GetOneService(code).Result.Value!;

            _service.DeleteService(servicio);

            return servicio;
        }
    }
}