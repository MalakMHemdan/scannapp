using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IPointsService
    {
        Task<int> GetUserPoints(int userId, int branchId);
        Task AddPoints(int userId, int branchId, decimal orderTotal);
        Task DeductPoints(int userId, int branchId, int pointsToUse);
        Task<IEnumerable<(int UserId, int Points)>> GetPointsByBranch(int branchId);

    }



}
