
using Microsoft.EntityFrameworkCore;
using MovieApp.Data.DBContext;
using MovieApp.Data.Models;
using MovieApp.Data.Repository.BaseRepository;

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
