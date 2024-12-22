
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
    public class UserStatusRepository : GenericRepository<UserStatus>
    {
        public UserStatusRepository()
        {
        }
        public UserStatusRepository(MovieAppDBContext context) => _context = context;

        public async Task<UserStatus> GetByStatusNameAsync(string name)
        {
            return await _context.Set<UserStatus>()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.StatusName == name);

        }
        public async Task<List<UserStatus>> GetListByStatusNameAsync(string name)
        {
            return await _context.Set<UserStatus>()
                .AsNoTracking()
                .Where(u => EF.Functions.Like(u.StatusName, $"%{name}%"))
                .ToListAsync();
        }
    }
}
