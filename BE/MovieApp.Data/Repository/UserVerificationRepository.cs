using Microsoft.EntityFrameworkCore;
using MovieApp.Data.DBContext;
using MovieApp.Data.Models;
using MovieApp.Data.Repository.BaseRepository;

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
