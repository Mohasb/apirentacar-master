using Microsoft.AspNetCore.Mvc;
using WebAPITest.Models;

namespace WebAPITest.Interfaces
{
    public interface ISaveChangesService
    {
        void SaveChangesDatabase();
    }
}