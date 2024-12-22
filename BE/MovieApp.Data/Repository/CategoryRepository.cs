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
