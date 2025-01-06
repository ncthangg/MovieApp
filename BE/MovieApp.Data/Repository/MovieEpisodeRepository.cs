using Microsoft.EntityFrameworkCore;
using MovieApp.Data.DBContext;
using MovieApp.Data.Models;
using MovieApp.Data.Repository.BaseRepository;

namespace MovieApp.Data.Repository
{
    public class MovieEpisodeRepository : GenericRepository<MovieEpisode>
    {
        public MovieEpisodeRepository()
        {
        }
        public MovieEpisodeRepository(MovieAppDBContext context) => _context = context;
        public async Task<List<MovieEpisode>> GetByEpisodeNameAsync(string name)
        {
            return await _context.Set<MovieEpisode>()
                .AsNoTracking()
                .Where(u => EF.Functions.Like(u.EpisodeName, $"%{name}%"))
                .ToListAsync();
        }
    }
}
