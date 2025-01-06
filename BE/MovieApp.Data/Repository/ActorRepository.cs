using Microsoft.EntityFrameworkCore;
using MovieApp.Data.DBContext;
using MovieApp.Data.Models;
using MovieApp.Data.Repository.BaseRepository;

namespace MovieApp.Data.Repository
{
    public class ActorRepository : GenericRepository<Actor>
    {
        public ActorRepository()
        {
        }
        public ActorRepository(MovieAppDBContext context) => _context = context;
        public async Task<List<Actor>> GetByActorNameAsync(string name)
        {
            return await _context.Set<Actor>()
                .AsNoTracking()
                .Where(u => EF.Functions.Like(u.ActorName, $"%{name}%"))
                .ToListAsync();
        }
    }
}
