using Microsoft.AspNetCore.Mvc;
using WebAPITest.Data;
using WebAPITest.Models;
using WebAPITest.Interfaces;

namespace WebAPITest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BranchController : ControllerBase
    {
        private readonly ILogger<BranchController> _logger;
        private readonly IBranchService _branch;
        private readonly IPlanningService _planning;

        public BranchController(ILogger<BranchController> logger, IBranchService branch, IPlanningService planning)
        {
            _logger = logger;
            _branch = branch;
            _planning = planning;
        }

        [HttpGet(Name = "GetBranches")]
        public Task<ActionResult<IEnumerable<Branch>>> GetBranches()
        {
            return _branch.GetAllBranches();
        }

        [HttpGet("{id}", Name = "GetBranch")]
        public ActionResult<Branch> GetBranch(string branchName)
        {
            var branch = _branch.GetOneBranch(branchName).Result;
            return branch.Value?.Name is null
                ? NotFound()
                : branch;
        }

        [HttpPost]
        public async Task<ActionResult<Branch>> Post(Branch branch)
        {
            if (await _branch.BranchExist(branch.Name!)) return BadRequest();

            _branch.AddBranch(branch);
            _planning.AddNewBranch(branch.Name!);

            return new CreatedAtRouteResult("GetBranch", branch);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string branchName, Branch newBranch)
        {
            if(!await _branch.BranchExist(branchName)) return BadRequest();

            var branch = _branch.GetOneBranch(branchName).Result.Value!;

            branch.Name = newBranch.Name;

            _branch.UpdateBranch(branch);

            return new CreatedAtRouteResult("GetBranch", branch);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Branch>> Delete(string branchName)
        {
            if(!await _branch.BranchExist(branchName)) return NotFound();

            var branch = _branch.GetOneBranch(branchName).Result.Value!;

            _branch.DeleteBranch(branch);

            return branch;
        }
    }
}