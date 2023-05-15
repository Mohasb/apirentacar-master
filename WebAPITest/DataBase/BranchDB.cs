using WebAPITest.Data;
using WebAPITest.Models;
using WebAPITest.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace WebAPITest.DataBase
{
    public class BranchDB : IBranchService
    {
        private readonly DataContext _context;
        private readonly ISaveChangesService _saveChanges;
        public BranchDB(DataContext context, ISaveChangesService saveChanges)
        {
            _context = context;
            _saveChanges = saveChanges;
        }
        public async Task<ActionResult<IEnumerable<Branch>>> GetAllBranches()
        {
            return await _context.Branches.Select(branch => new Branch(branch.Name!)).ToListAsync();
        }

        public async Task<ActionResult<Branch>> GetOneBranch(string branchName)
        {
            return await _context.Branches.FindAsync(branchName) is Branch branch
                ? branch
                : new Branch();
        }
        public async Task<bool> BranchExist(string branch)
        {
            return await _context.Branches.FindAsync(branch) is not null;
        }
        public void AddBranch(Branch branch)
        {
            _context.Add(branch);
            _saveChanges.SaveChangesDatabase();
        }

        public void UpdateBranch(Branch branch)
        {
            _context.Update(branch);
            _saveChanges.SaveChangesDatabase();
        }

        public void DeleteBranch(Branch branch)
        {
            _context.Remove(branch);
            _saveChanges.SaveChangesDatabase();
        }
    }
}