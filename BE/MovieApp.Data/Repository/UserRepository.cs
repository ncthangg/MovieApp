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
    public class UserRepository : GenericRepository<User>
    {
        public UserRepository()
        {
        }
        public UserRepository(MovieAppDBContext context) => _context = context;

        public async Task<List<User>> GetByUserNameAsync(string name)
        {
            return await _context.Set<User>()
                      .AsNoTracking() 
                      .Include(x => x.Role)
                      .Where(u => EF.Functions.Like(u.Name, $"%{name}%"))
                      .ToListAsync();
        }
        public async Task<User> GetByUserEmailAsync(string email)
        {
            return await _context.Set<User>().Include(x => x.Role)
                .FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
