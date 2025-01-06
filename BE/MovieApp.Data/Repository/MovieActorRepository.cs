using Microsoft.EntityFrameworkCore;
using MovieApp.Data.DBContext;
using MovieApp.Data.Models;
using MovieApp.Data.Repository.BaseRepository;

namespace MovieApp.Data.Repository
{
    public class MovieActorRepository : GenericRepository<MovieActor>
    {
        public MovieActorRepository()
        {
        }
        public MovieActorRepository(MovieAppDBContext context) => _context = context;

        public async Task<List<MovieActor>> GetAllAsync()
        {
            return await _context.Set<MovieActor>()
                .Include(x => x.Actor)
                .ToListAsync();
        }
        public async Task<List<MovieActor>> GetByActorIdAsync(long actorId)
        {
            return await _context.Set<MovieActor>()
                .Include(x => x.Actor)
                .Include(x => x.Movie)
                .Where(x => x.ActorId == actorId)
                .ToListAsync();
        }
        public async Task<List<MovieActor>> GetByMovieIdAsync(long movieId)
        {
            return await _context.Set<MovieActor>()
                .Include(x => x.Movie)
                .Include(x => x.Actor)
                .Where(x => x.MovieId == movieId)
                .ToListAsync();
        }

        //Tìm Category mới
        public async Task<List<long>> GetActorsToAddAsync(long movieId, List<long> requestActorIds)
        {
            // Lấy danh sách các danh mục hiện tại của movie
            var existingActors = await _context.MovieActors
                .Where(mc => mc.MovieId == movieId)
                .Select(mc => mc.ActorId)
                .ToListAsync();

            // Trả về các danh mục trong request nhưng chưa tồn tại trong database
            return requestActorIds
                .Where(actorId => !existingActors.Contains(actorId))
                .ToList();
        }
        //Tìm Category xóa
        public async Task<List<long>> GetActorsToRemoveAsync(long movieId, List<long> requestActorIds)
        {
            // Lấy danh sách các danh mục hiện tại của movie
            var existingActors = await _context.MovieActors
                .Where(mc => mc.MovieId == movieId)
                .Select(mc => mc.ActorId)
                .ToListAsync();

            // Trả về các danh mục có trong database nhưng không có trong request
            return existingActors
                .Where(actorId => !requestActorIds.Contains(actorId))
                .ToList();
        }

        //Thêm Actor mới
        public async Task CreateAsync(long movieId, List<long> actorIds)
        {
            foreach (var actorId in actorIds)
            {
                var MovieActor = new MovieActor
                {
                    MovieId = movieId,
                    ActorId = actorId
                };
                await _context.MovieActors.AddAsync(MovieActor);
            }
            await _context.SaveChangesAsync();
        }
        //Xóa Actor không còn trong Request
        public async Task RemoveActorAsync(long movieId, List<long> actorIdsToRemove)
        {
            var actorsToRemove = await _context.MovieActors
                .Where(mc => mc.MovieId == movieId && actorIdsToRemove.Contains(mc.ActorId))
                .ToListAsync();

            _context.MovieActors.RemoveRange(actorsToRemove);
            await _context.SaveChangesAsync();
        }
    }
}
