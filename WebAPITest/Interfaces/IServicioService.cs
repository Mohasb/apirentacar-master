using Microsoft.AspNetCore.Mvc;
using WebAPITest.Models;

namespace WebAPITest.Interfaces
{
    public interface IServicioService
    {
        Task<ActionResult<IEnumerable<Servicio>>> GetAllServices();
        Task<ActionResult<Servicio>> GetOneService(string code);
        Task<bool> ServiceExist(string code);
        void AddService(Servicio service);
        void UpdateService(Servicio service);
        void DeleteService(Servicio service);
    }
}