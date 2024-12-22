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
    public class UserVerificationRepository : GenericRepository<UserVerification>
    {
        public UserVerificationRepository()
        {
        }
        public UserVerificationRepository(MovieAppDBContext context) => _context = context;
        public async Task<UserVerification> GetByUserId(long userId)
        {
            return await _context.Set<UserVerification>()
                .AsNoTracking()
                .FirstOrDefaultAsync(token => token.UserId == userId);
        }
    }
}
