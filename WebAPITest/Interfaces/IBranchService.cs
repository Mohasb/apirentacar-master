using Microsoft.AspNetCore.Mvc;
using WebAPITest.Models;
using WebAPITest.Data;

namespace WebAPITest.Interfaces
{
    public interface IBranchService
    {
        Task<ActionResult<IEnumerable<Branch>>> GetAllBranches();
        Task<ActionResult<Branch>> GetOneBranch(string branch);
        Task<bool> BranchExist(string branch);
        void AddBranch(Branch branch);
        void UpdateBranch(Branch branch);
        void DeleteBranch(Branch branch);
    }
}