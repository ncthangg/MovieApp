using MovieApp.Data.DBContext;
using MovieApp.Data.Models;
using MovieApp.Data.Repository.BaseRepository;
namespace MovieApp.Data.Repository
{
    public class MovieRateRepository : GenericRepository<MovieRate>
    {
        public MovieRateRepository()
        {
        }
        public MovieRateRepository(MovieAppDBContext context) => _context = context;
    }
}
