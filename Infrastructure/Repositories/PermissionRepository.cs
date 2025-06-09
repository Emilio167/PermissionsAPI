using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PermissionRepository(PermissionsDbContext context, IUnitOfWork unitOfWork) : IPermissionRepository
    {
        private readonly PermissionsDbContext _context = context;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Permission> GetByIdAsync(int id)
        {
            return await _context.Permissions.FindAsync(id) ?? throw new InvalidOperationException($"Permission with ID {id} not found.");
        }

        public async Task<IEnumerable<Permission>> GetAllAsync() => await _context.Permissions.ToListAsync();

        public async Task AddAsync(Permission permission)
        {
            await _context.Permissions.AddAsync(permission);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateAsync(Permission permission)
        {
            _context.Permissions.Update(permission);
            await _unitOfWork.CompleteAsync();
        }
    }
}
