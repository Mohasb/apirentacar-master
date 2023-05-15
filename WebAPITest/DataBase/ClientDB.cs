using WebAPITest.Data;
using WebAPITest.Models;
using WebAPITest.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace WebAPITest.DataBase
{
    public class ClientDB : IClientService
    {
        private readonly DataContext _context;
        private readonly ISaveChangesService _saveChanges;
        public ClientDB(DataContext context, ISaveChangesService saveChanges)
        {
            _context = context;
            _saveChanges = saveChanges;
        }
        public async Task<ActionResult<IEnumerable<Client>>> GetAllClients()
        {
            return await _context.Clients.ToListAsync();
        }

        public async Task<ActionResult<Client>> GetOneClient(string clientDNI)
        {
            return await _context.Clients.FindAsync(clientDNI) is Client client
                ? client
                : new Client();
        }
        public async Task<bool> ClientExist(string clientDNI)
        {
            return await _context.Clients.FindAsync(clientDNI) is not null;
        }

        public void AddClient(Client client)
        {
            _context.Add(client);
            _saveChanges.SaveChangesDatabase();
        }

        public void UpdateClient(Client client)
        {
            _context.Update(client);
            _saveChanges.SaveChangesDatabase();
        }

        public void DeleteClient(Client client)
        {
            _context.Remove(client);
            _saveChanges.SaveChangesDatabase();
        }
    }
}