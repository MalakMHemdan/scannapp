using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    
        public interface IBranchService
        {
            Task<IEnumerable<Branches>> GetAllBranches();
            Task AddBranch(BranchDto branchDto);
            Task DeleteBranch(int branchId);
            Task<IEnumerable<(int BranchID, string BranchName)>> GetAllBranchesSummary();
        }
    



}
