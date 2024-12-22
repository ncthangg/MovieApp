using Microsoft.EntityFrameworkCore;
using MovieApp.Data.DBContext;
using MovieApp.Data.Models;
using MovieApp.Data.Repository.BaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Data.Repository
{
    public class UserRoleRepository : GenericRepository<UserRole>
    {
        public UserRoleRepository()
        {
        }
        public UserRoleRepository(MovieAppDBContext context) => _context = context;

        public async Task<UserRole> GetByRoleNameAsync(string name)
        {
            return await _context.Set<UserRole>()
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.RoleName == name);
        }

        public async Task<List<UserRole>> GetListByRoleNameAsync(string name)
        {
            return await _context.Set<UserRole>()
                .AsNoTracking()
                .Where(u => EF.Functions.Like(u.RoleName, $"%{name}%"))
                .ToListAsync();
        }

    }
}
