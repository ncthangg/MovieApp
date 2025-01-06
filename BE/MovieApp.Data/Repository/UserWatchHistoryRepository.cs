using Microsoft.EntityFrameworkCore;
using MovieApp.Data.DBContext;
using MovieApp.Data.Models;
using MovieApp.Data.Repository.BaseRepository;

namespace MovieApp.Data.Repository
{
    public class UserWatchHistoryRepository : GenericRepository<UserWatchHistory>
    {
        public UserWatchHistoryRepository()
        {
        }
        public UserWatchHistoryRepository(MovieAppDBContext context) => _context = context;

        public async Task<List<UserWatchHistory>> GetByUserIdAsync(long userId)
        {
            return await _context.Set<UserWatchHistory>()
                .Where(like => like.UserId == userId)
                .ToListAsync();
        }
        public async Task<List<UserWatchHistory>> GetByMovieIdAsync(long userId, long movieId)
        {
            return await _context.Set<UserWatchHistory>()
                .Where(like => like.UserId == userId && like.MovieId == movieId)
                .ToListAsync();
        }
        public async Task<List<UserWatchHistory>> GetBySeasonIdAsync(long userId, long movieId, long seasonId)
        {
            return await _context.Set<UserWatchHistory>()
                .Where(like => like.UserId == userId && like.MovieId == movieId && like.SeasonId == seasonId)
                .ToListAsync();
        }
        public async Task<List<UserWatchHistory>> GetByEpisodeIdAsync(long userId, long movieId, long seasonId, long episodeId)
        {
            return await _context.Set<UserWatchHistory>()
                .Where(like => like.UserId == userId && like.MovieId == movieId && like.SeasonId == seasonId && like.EpisodeId == episodeId)
                .ToListAsync();
        }
        public async Task<bool> MovieWatchedAsync(long movieId, long userId)
        {
            return await _context.Set<UserWatchHistory>()
                .AnyAsync(like => like.UserId == userId && like.MovieId == movieId);
        }
    }
}
