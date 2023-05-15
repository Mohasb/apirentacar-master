using WebAPITest.Data;
using WebAPITest.Interfaces;

namespace WebAPITest.DataBase
{
    public class SaveChangesDB : ISaveChangesService
    {
        private readonly DataContext _context;
        public SaveChangesDB(DataContext context)
        {
            _context = context;
        }
        public async void SaveChangesDatabase()
        {
            await _context.SaveChangesAsync();
        }
    }
}