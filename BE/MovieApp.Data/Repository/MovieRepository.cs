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
    public class MovieRepository : GenericRepository<Movie>
    {
        public MovieRepository()
        {
        }
        public MovieRepository(MovieAppDBContext context) => _context = context;

        public async Task<List<Movie>> GetByMovieNameAsync(string name)
        {
            return await _context.Set<Movie>()
                .AsNoTracking()
                .Where(u => EF.Functions.Like(u.MovieName, $"%{name}%"))
                .ToListAsync();
        }

    }
}
