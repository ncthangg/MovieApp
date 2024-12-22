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
        public async Task<List<UserWatchHistory>> GetByMovieIdAsync(long movieId)
        {
            return await _context.Set<UserWatchHistory>()
                .Where(like => like.MovieId == movieId)
                .ToListAsync();
        }
        public async Task<bool> MovieWatchedAsync(long movieId, long userId)
        {
            return await _context.Set<UserWatchHistory>()
                .AnyAsync(like => like.UserId == userId && like.MovieId == movieId);
        }
    }
}
