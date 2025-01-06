using Microsoft.EntityFrameworkCore;
using MovieApp.Data.DBContext;
using MovieApp.Data.Models;
using MovieApp.Data.Repository.BaseRepository;

namespace MovieApp.Data.Repository
{
    public class MovieSeasonRepository : GenericRepository<MovieSeason>
    {
        public MovieSeasonRepository()
        {
        }
        public MovieSeasonRepository(MovieAppDBContext context) => _context = context;
        public async Task<List<Movie>> GetByMovieNameAsync(string name)
        {
            return await _context.Set<Movie>()
                .AsNoTracking()
                .Where(u => EF.Functions.Like(u.MovieName, $"%{name}%"))
                .ToListAsync();
        }
    }
}
