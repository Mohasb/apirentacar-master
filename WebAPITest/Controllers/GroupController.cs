using WebAPITest.Models;
using WebAPITest.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPITest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupController : ControllerBase
    {
        private readonly ILogger<GroupController> _logger;
        private readonly IGroupService _group;
        private readonly IPlanningService _planning;
        public GroupController(ILogger<GroupController> logger, IGroupService group, IPlanningService planning)
        {
            _logger = logger;
            _group = group;
            _planning = planning;
        }

        [HttpGet(Name = "GetGroups")]
        public Task<ActionResult<IEnumerable<Group>>> GetGroups()
        {
            return _group.GetAllGroups();
        }

        [HttpGet("{groupName}", Name = "GetGroup")]
        public ActionResult<Group> GetGroup(string groupName)
        {
            var group = _group.GetOneGroup(groupName).Result;
            return group.Value?.Name == null
                ? NotFound()
                : group;
        }

        [HttpPost]
        public async Task<ActionResult<Group>> Post(Group group)
        {
            if(await _group.GroupExist(group.Name!)) return BadRequest();

            _group.AddGroup(group);
            _planning.AddNewGroup(group.Name!);

            return new CreatedAtRouteResult("GetGroup", new { groupName = group.Name}, group);
        }

        [HttpPut("{groupName}")]
        public async Task<IActionResult> Put(string groupName, Group newGroup)
        {
            if(!await _group.GroupExist(groupName) || await _group.GroupExist(newGroup.Name!)) return BadRequest();
            var group = _group.GetOneGroup(groupName).Result.Value!;

            if(newGroup.Photo != null)
                group.Photo = newGroup.Photo;

            _group.UpdateGroup(group);

            return new CreatedAtRouteResult("GetGroup", new { groupName = newGroup.Name}, newGroup);
        }

        [HttpDelete("{groupName}")]
        public async Task<ActionResult<Group>> Delete(string groupName)
        {
            if(!await _group.GroupExist(groupName)) return NotFound();

            var group = _group.GetOneGroup(groupName).Result.Value!;

            _group.DeleteGroup(group);

            return group;
        }
    }
}