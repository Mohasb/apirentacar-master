using WebAPITest.Data;
using WebAPITest.Models;
using WebAPITest.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPITest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarController : ControllerBase
    {
        private readonly ILogger<CarController> _logger;
        private readonly ICarService _car;
        private readonly IBranchService _branch;
        private readonly IGroupService _group;

        public CarController(ILogger<CarController> logger,ICarService car, IBranchService branch, IGroupService group)
        {
            _logger = logger;
            _car = car;
            _branch = branch;
            _group = group;
        }

        [HttpGet(Name = "GetCars")]
        public Task<ActionResult<IEnumerable<Car>>> GetCars()
        {
            return _car.GetAllCars();
        }

        [HttpGet("licenseplate/{licensePlate}", Name = "GetCar")]
        public ActionResult<Car> GetCar(string licensePlate)
        {
            var car = _car.GetOneCar(licensePlate).Result;
            return car.Value?.LicensePlate == null
                ? NotFound()
                :car;
        }

        [HttpGet("group/{groupName}", Name = "GetCarsByGroup")]
        public Task<ActionResult<IEnumerable<Car>>> GetCarsByGroup(string groupName)
        {
            return _car.GetAllCarsFromGroup(groupName);
        }
        [HttpGet("branch/{branchName}", Name = "GetCarsFromBranch")]
        public Task<ActionResult<IEnumerable<Car>>> GetAllCarsFromBranch(string branchName)
        {
            var branch = _branch.GetOneBranch(branchName).Result.Value;
            return _car.GetAllCarsFromBranch(branch!.Name!);
        }

        [HttpPost]
        public async Task<ActionResult<Car>> Post(Car car)
        {
            if(car.LicensePlate is null || await _car.CarExist(car.LicensePlate)) return BadRequest();

            if(car.Branch is null || !await _branch.BranchExist(car.Branch)) return BadRequest();

            if(car.Group is null || !await _group.GroupExist(car.Group)) return BadRequest();

            _car.AddCar(car);

            return new CreatedAtRouteResult("GetCar", new { licensePlate = car.LicensePlate }, car);
        }

        [HttpPut("{licensePlate}")]
        public async Task<IActionResult> Put(string licensePlate, Car newCar)
        {
            if(!await _car.CarExist(licensePlate)) return BadRequest();
            var car = _car.GetOneCar(licensePlate).Result.Value!;
            
            if(newCar.Model is not null)
                car.Model = newCar.Model;
            if(newCar.Automatic != car.Automatic)
                car.Automatic = newCar.Automatic;
            if(newCar.Doors >= 0)
                car.Doors = newCar.Doors;
            if(newCar.Seats >= 0)
                car.Seats = newCar.Seats;
            if(newCar.GasType is not null)
                car.GasType = newCar.GasType;
            if(newCar.Suitcases >= 0)
                car.Suitcases = newCar.Suitcases;
            if(newCar.DayPrice >= 0)
                car.DayPrice = newCar.DayPrice;
            if(newCar.Branch is not null && await _branch.BranchExist(newCar.Branch))
                car.Branch = newCar.Branch;
            if(newCar.Group is not null && await _group.GroupExist(newCar.Group))
                car.Group = newCar.Group;

            _car.UpdateCar(car);

            return new CreatedAtRouteResult("GetCar", new { licensePlate = car.LicensePlate }, car);
        }

        [HttpDelete("{licensePlate}")]
        public async Task<ActionResult<Car>> Delete(string licensePlate)
        {
            if(!await _car.CarExist(licensePlate)) return NotFound();

            var car = _car.GetOneCar(licensePlate).Result.Value!;

            _car.DeleteCar(car);

            return car;
        }
    }
}