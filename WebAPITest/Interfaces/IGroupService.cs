using Microsoft.AspNetCore.Mvc;
using WebAPITest.Models;

namespace WebAPITest.Interfaces
{
    public interface IGroupService
    {
        Task<ActionResult<IEnumerable<Group>>> GetAllGroups();
        Task<ActionResult<Group>> GetOneGroup(string groupName);
        Task<bool> GroupExist(string groupName);
        void AddGroup(Group group);
        void UpdateGroup(Group group);
        void DeleteGroup(Group group);
    }
}