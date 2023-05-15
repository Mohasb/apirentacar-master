using WebAPITest.Data;
using WebAPITest.Models;
using WebAPITest.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace WebAPITest.DataBase
{
    public class ServicioDB : IServicioService
    {
        private readonly DataContext _context;
        private readonly ISaveChangesService _saveChanges;
        public ServicioDB(DataContext context, ISaveChangesService saveChanges)
        {
            _context = context;
            _saveChanges = saveChanges;
        }
        public async Task<ActionResult<IEnumerable<Servicio>>> GetAllServices()
        {
            return await _context.Servicios.ToListAsync();
        }
        public async Task<ActionResult<Servicio>> GetOneService(string code)
        {
            return await _context.Servicios.FindAsync(code) is Servicio service
                ? service
                : new Servicio();
        }
        public async Task<bool> ServiceExist(string code)
        {
            return await _context.Servicios.FindAsync(code) is not null;
        }
        public void AddService(Servicio service)
        {
            _context.Add(service);
            _saveChanges.SaveChangesDatabase();
        }
        public void UpdateService(Servicio service)
        {
            _context.Update(service);
            _saveChanges.SaveChangesDatabase();
        }
        public void DeleteService(Servicio service)
        {
            _context.Remove(service);
            _saveChanges.SaveChangesDatabase();
        }
    }
}