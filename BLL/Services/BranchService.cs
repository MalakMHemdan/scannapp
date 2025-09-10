using BLL.DTOs;
using BLL.Interfaces;
using DAL.Models;
using DAL.Repositories.unitofwork;

namespace BLL.Services
{
    public class BranchService : IBranchService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BranchService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Branches>> GetAllBranches()
        {
            return await _unitOfWork.Branches.GetAllAsync();
        }

        public async Task AddBranch(BranchDto branchDto)
        {
            var branch = new Branches
            {
                BranchName = branchDto.BranchName,
                Address = branchDto.Address,
                City = branchDto.City,
                Phone = branchDto.Phone,
                SuperAdmainID = branchDto.SuperAdminID
            };

            await _unitOfWork.Branches.AddAsync(branch);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteBranch(int branchId)
        {
            var branch = await _unitOfWork.Branches.GetByIdAsync(branchId);
            if (branch != null)
            {
                _unitOfWork.Branches.Delete(branch);
                await _unitOfWork.SaveAsync();
            }
        }

        public async Task<IEnumerable<(int BranchID, string BranchName)>> GetAllBranchesSummary()
        {
            var branches = await _unitOfWork.Branches.GetAllAsync();
            return branches.Select(b => (b.BranchID, b.BranchName));
        }
    }
}
