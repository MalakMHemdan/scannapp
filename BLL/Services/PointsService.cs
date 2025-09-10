using BLL.Interfaces;
using DAL.Repositories.unitofwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class PointsService : IPointsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private const decimal POINTS_RATE = 0.07m; // 7%

        public PointsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> GetUserPoints(int userId, int branchId)
        {
            var points = await _unitOfWork.Points.GetUserPointsAsync(userId);
            var branchPoints = points.FirstOrDefault(p => p.BranchId == branchId);
            return branchPoints?.AmountOfPoints ?? 0;
        }

        public async Task AddPoints(int userId, int branchId, decimal orderTotal)
        {
            int earnedPoints = (int)(orderTotal * POINTS_RATE);
            var points = (await _unitOfWork.Points.GetUserPointsAsync(userId))
                         .FirstOrDefault(p => p.BranchId == branchId);

            if (points != null)
            {
                points.AmountOfPoints += earnedPoints;
                _unitOfWork.Points.Update(points);
            }
            else
            {
                await _unitOfWork.Points.AddAsync(new Points
                {
                    UserId = userId,
                    BranchId = branchId,
                    AmountOfPoints = earnedPoints
                });
            }
            await _unitOfWork.SaveAsync();
        }

        public async Task DeductPoints(int userId, int branchId, int pointsToUse)
        {
            var points = (await _unitOfWork.Points.GetUserPointsAsync(userId))
                         .FirstOrDefault(p => p.BranchId == branchId);

            if (points != null && points.AmountOfPoints >= pointsToUse)
            {
                points.AmountOfPoints -= pointsToUse;
                _unitOfWork.Points.Update(points);
                await _unitOfWork.SaveAsync();
            }
        }
        public async Task<IEnumerable<(int UserId, int Points)>> GetPointsByBranch(int branchId)
        {
            var points = await _unitOfWork.Points.GetAllAsync();
            return points
                .Where(p => p.BranchId == branchId)
                .Select(p => (p.UserId, p.AmountOfPoints));
        }
    }


}
