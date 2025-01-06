using Microsoft.EntityFrameworkCore;
using MovieApp.Data.DBContext;
using MovieApp.Data.Models;
using MovieApp.Data.Repository.BaseRepository;

namespace MovieApp.Data.Repository
{
    public class CategoryRepository : GenericRepository<Category>
    {
        public CategoryRepository()
        {
        }
        public CategoryRepository(MovieAppDBContext context) => _context = context;

        public async Task<List<Category>> GetByCategoryNameAsync(string name)
        {
            return await _context.Set<Category>()
                .AsNoTracking()
                .Where(u => EF.Functions.Like(u.CategoryName, $"%{name}%"))
                .ToListAsync();
        }

    }
}
