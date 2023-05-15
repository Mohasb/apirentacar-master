using WebAPITest.Data;
using WebAPITest.Models;
using WebAPITest.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace WebAPITest.DataBase
{
    public class CarDB : ICarService
    {
        private readonly DataContext _context;
        private readonly IPlanningService _planning;
        private readonly ISaveChangesService _saveChanges;
        public CarDB(DataContext context, IPlanningService planning, ISaveChangesService saveChanges)
        {
            _context = context;
            _planning = planning;
            _saveChanges = saveChanges;
        }
        public async Task<ActionResult<IEnumerable<Car>>> GetAllCars()
        {
            return await _context.Cars.ToListAsync();
        }

        public async Task<ActionResult<Car>> GetOneCar(string licensePlate)
        {
            return await _context.Cars.FindAsync(licensePlate) is Car car
                ? car
                : new Car();
        }

        public async Task<ActionResult<IEnumerable<Car>>> GetAllCarsFromGroup(string group)
        {
            return await _context.Cars.Where(car => car.Group == group).ToListAsync();
        }

        public async Task<ActionResult<IEnumerable<Car>>> GetAllCarsFromBranch(string branch)
        {
            return await _context.Cars.Where(car => car.Branch == branch).ToListAsync();
        }

        public async Task<int> GetAvailableCarsByGroupBySucursal(string branch, string group)
        {
            return await _context.Cars.Where(car => car.Branch == branch && car.Group == group).CountAsync();
        }

        public async Task<bool> CarExist( string licensePlate)
        {
            return await _context.Cars.FindAsync(licensePlate) is not null;
        }

        public void AddCar( Car car)
        {
            _context.Add(car);
            _planning.SumarDisponibleNuevoCoche(car.Branch!, car.Group!);
            _saveChanges.SaveChangesDatabase();
        }

        public void UpdateCar(Car car)
        {
            _context.Update(car);
            _saveChanges.SaveChangesDatabase();
        }

        public void DeleteCar(Car car)
        {
            _context.Remove(car);
            _saveChanges.SaveChangesDatabase();
        }
    }
}