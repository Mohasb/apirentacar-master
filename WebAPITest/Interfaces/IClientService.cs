using Microsoft.AspNetCore.Mvc;
using WebAPITest.Models;
using WebAPITest.Data;

namespace WebAPITest.Interfaces
{
    public interface IClientService
    {
        Task<ActionResult<IEnumerable<Client>>> GetAllClients();
        Task<ActionResult<Client>> GetOneClient(string clientDNI);
        Task<bool> ClientExist(string clientDNI);
        void AddClient(Client client);
        void UpdateClient(Client client);
        void DeleteClient(Client client);
    }
}