using Microsoft.EntityFrameworkCore;
using MovieApp.Data.DBContext;
using MovieApp.Data.Models;
using MovieApp.Data.Repository.BaseRepository;

namespace MovieApp.Data.Repository
{
    public class UserLikeRepository : GenericRepository<UserLike>
    {
        public UserLikeRepository()
        {
        }
        public UserLikeRepository(MovieAppDBContext context) => _context = context;

        public async Task<List<UserLike>> GetByUserIdAsync(long userId)
        {
            return await _context.Set<UserLike>()
                .Where(like => like.UserId == userId)
                .ToListAsync();
        }
        public async Task<List<UserLike>> GetByMovieIdAsync(long movieId)
        {
            return await _context.Set<UserLike>()
                .Where(like => like.MovieId == movieId)
                .ToListAsync();
        }
        public async Task<bool> LikeExistAsync(long movieId, long userId)
        {
            return await _context.Set<UserLike>()
                .AnyAsync(like => like.UserId == userId && like.MovieId == movieId);
        }
    }
}
