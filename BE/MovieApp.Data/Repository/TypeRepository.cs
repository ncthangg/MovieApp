using Microsoft.EntityFrameworkCore;
using MovieApp.Data.DBContext;
using MovieApp.Data.Models;
using MovieApp.Data.Repository.BaseRepository;

namespace MovieApp.Data.Repository
{
    public class TypeRepository : GenericRepository<MovieType>
    {
        public TypeRepository()
        {
        }
        public TypeRepository(MovieAppDBContext context) => _context = context;

        public async Task<List<MovieType>> GetByTypeNameAsync(string name)
        {
            return await _context.Set<MovieType>()
                .AsNoTracking()
                .Where(u => EF.Functions.Like(u.TypeName, $"%{name}%"))
                .ToListAsync();
        }

    }
}
