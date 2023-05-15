using Microsoft.AspNetCore.Mvc;
using WebAPITest.Models;
using WebAPITest.Data;

namespace WebAPITest.Interfaces
{
    public interface ICarService
    {
        Task<ActionResult<IEnumerable<Car>>> GetAllCars();
        Task<ActionResult<Car>> GetOneCar(string licensePlate);
        Task<ActionResult<IEnumerable<Car>>> GetAllCarsFromGroup(string group);
        Task<ActionResult<IEnumerable<Car>>> GetAllCarsFromBranch(string branch);
        Task<int> GetAvailableCarsByGroupBySucursal(string branch, string group);
        Task<bool> CarExist(string licensePlate);
        void AddCar(Car car);
        void UpdateCar(Car car);
        void DeleteCar(Car car);
    }
}