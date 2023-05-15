using WebAPITest.Data;
using WebAPITest.Models;
using WebAPITest.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;


namespace WebAPITest.DataBase
{
    public class GroupDB : IGroupService
    {
        private readonly DataContext _context;
        private readonly ISaveChangesService _saveChanges;
        public GroupDB(DataContext context, ISaveChangesService saveChanges)
        {
            _context = context;
            _saveChanges = saveChanges;
        }
        public async Task<ActionResult<IEnumerable<Group>>> GetAllGroups()
        {
            return await _context.Groups.ToListAsync();
        }
        public async Task<ActionResult<Group>> GetOneGroup(string groupName)
        {
            return await _context.Groups.FindAsync(groupName) is Group group
                ? group
                : new Group();
        }

        public async Task<bool> GroupExist(string groupName)
        {
            return await _context.Groups.FindAsync(groupName) is not null;
        }

        public void AddGroup(Group group)
        {
            _context.Add(group);
            _saveChanges.SaveChangesDatabase();
        }

        public void UpdateGroup(Group group)
        {
            _context.Update(group);
            _saveChanges.SaveChangesDatabase();
        }

        public void DeleteGroup(Group group)
        {
            _context.Remove(group);
            _saveChanges.SaveChangesDatabase();
        }
    }
}