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
    public class MovieCategoryRepository : GenericRepository<MovieCategory>
    {
        public MovieCategoryRepository()
        {
        }
        public MovieCategoryRepository(MovieAppDBContext context) => _context = context;
        public async Task<List<MovieCategory>> GetByCategoryIdAsync(long categoryId)
        {
            return await _context.Set<MovieCategory>()
                .Include(x => x.Category)
                .Include(x => x.Movie)
                .Where(x => x.CategoryId == categoryId)
                .ToListAsync();
        }
        public async Task<List<MovieCategory>> GetByMovieIdAsync(long movieId)
        {
            return await _context.Set<MovieCategory>()
                .Include(x => x.Movie)
                .Include(x => x.Category)
                .Where(x => x.MovieId == movieId)
                .ToListAsync();
        }

        public async Task CreateAsync(long movieId, List<long> categoryIds)
        {
            // Kiểm tra từng Category có tồn tại trong database không
            foreach (var categoryId in categoryIds)
            {
                var movieCategory = new MovieCategory
                {
                    MovieId = movieId,
                    CategoryId = categoryId
                };

                // Thêm đối tượng MovieCategory vào DbContext
                await _context.MovieCategories.AddAsync(movieCategory);
            }

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(long movieId, List<long> categoryIds)
        {
            // Kiểm tra sự tồn tại của MovieCategory hiện tại trong cơ sở dữ liệu
            var existingMovieCategories = await _context.Set<MovieCategory>()
                .Where(mc => mc.MovieId == movieId)
                .ToListAsync();

            // Nếu Movie không có trong danh mục nào, trả về lỗi hoặc thông báo
            if (!existingMovieCategories.Any())
            {
                throw new InvalidOperationException("Movie does not have any categories associated with it.");
            }

            // Xóa tất cả các liên kết Movie-Category hiện tại (nếu cần thiết)
            _context.Set<MovieCategory>().RemoveRange(existingMovieCategories);

            // Thêm các liên kết mới từ danh sách Category
            foreach (var categoryId in categoryIds)
            {
                var movieCategory = new MovieCategory
                {
                    MovieId = movieId,
                    CategoryId = categoryId
                };
                await _context.Set<MovieCategory>().AddAsync(movieCategory);
            }

            // Lưu các thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();
        }

    }
}
