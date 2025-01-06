using Microsoft.EntityFrameworkCore;
using MovieApp.Data.DBContext;
using MovieApp.Data.Models;
using MovieApp.Data.Repository.BaseRepository;

namespace MovieApp.Data.Repository
{
    public class MovieCategoryRepository : GenericRepository<MovieCategory>
    {
        public MovieCategoryRepository()
        {
        }
        public MovieCategoryRepository(MovieAppDBContext context) => _context = context;
        public async Task<List<MovieCategory>> GetAllAsync()
        {
            return await _context.Set<MovieCategory>()
                .Include(x => x.Category)
                .ToListAsync();
        }
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

        //Tìm Category mới
        public async Task<List<long>> GetCategoriesToAddAsync(long movieId, List<long> requestCategoryIds)
        {
            // Lấy danh sách các danh mục hiện tại của movie
            var existingCategories = await _context.MovieCategories
                .Where(mc => mc.MovieId == movieId)
                .Select(mc => mc.CategoryId)
                .ToListAsync();

            // Trả về các danh mục trong request nhưng chưa tồn tại trong database
            return requestCategoryIds
                .Where(catId => !existingCategories.Contains(catId))
                .ToList();
        }
        //Tìm Category xóa
        public async Task<List<long>> GetCategoriesToRemoveAsync(long movieId, List<long> requestCategoryIds)
        {
            // Lấy danh sách các danh mục hiện tại của movie
            var existingCategories = await _context.MovieCategories
                .Where(mc => mc.MovieId == movieId)
                .Select(mc => mc.CategoryId)
                .ToListAsync();

            // Trả về các danh mục có trong database nhưng không có trong request
            return existingCategories
                .Where(catId => !requestCategoryIds.Contains(catId))
                .ToList();
        }

        //Thêm Category mới
        public async Task CreateAsync(long movieId, List<long> categoryIds)
        {
            foreach (var categoryId in categoryIds)
            {
                var movieCategory = new MovieCategory
                {
                    MovieId = movieId,
                    CategoryId = categoryId
                };
                await _context.MovieCategories.AddAsync(movieCategory);
            }
            await _context.SaveChangesAsync();
        }
        //Xóa Category không còn trong Request
        public async Task RemoveCategoriesAsync(long movieId, List<long> categoryIdsToRemove)
        {
            var categoriesToRemove = await _context.MovieCategories
                .Where(mc => mc.MovieId == movieId && categoryIdsToRemove.Contains(mc.CategoryId))
                .ToListAsync();

            _context.MovieCategories.RemoveRange(categoriesToRemove);
            await _context.SaveChangesAsync();
        }

        #region Create and Update REPOSITORY OLD

        //public async Task CreateAsync(long movieId, List<long> categoryIds)
        //{
        //    // Kiểm tra từng Category có tồn tại trong database không
        //    foreach (var categoryId in categoryIds)
        //    {
        //        var movieCategory = new MovieCategory
        //        {
        //            MovieId = movieId,
        //            CategoryId = categoryId
        //        };

        //        // Thêm đối tượng MovieCategory vào DbContext
        //        await _context.MovieCategories.AddAsync(movieCategory);
        //    }

        //    // Lưu thay đổi vào cơ sở dữ liệu
        //    await _context.SaveChangesAsync();
        //}

        //public async Task UpdateAsync(long movieId, List<long> categoryIds)
        //{
        //    // Kiểm tra sự tồn tại của MovieCategory hiện tại trong cơ sở dữ liệu
        //    var existingMovieCategories = await _context.Set<MovieCategory>()
        //        .Where(mc => mc.MovieId == movieId)
        //        .ToListAsync();

        //    // Nếu Movie không có trong danh mục nào, trả về lỗi hoặc thông báo
        //    if (!existingMovieCategories.Any())
        //    {
        //        throw new InvalidOperationException("Movie does not have any categories associated with it.");
        //    }

        //    // Xóa tất cả các liên kết Movie-Category hiện tại (nếu cần thiết)
        //    _context.Set<MovieCategory>().RemoveRange(existingMovieCategories);

        //    // Thêm các liên kết mới từ danh sách Category
        //    foreach (var categoryId in categoryIds)
        //    {
        //        var movieCategory = new MovieCategory
        //        {
        //            MovieId = movieId,
        //            CategoryId = categoryId
        //        };
        //        await _context.Set<MovieCategory>().AddAsync(movieCategory);
        //    }

        //    // Lưu các thay đổi vào cơ sở dữ liệu
        //    await _context.SaveChangesAsync();
        //}

        #endregion

    }
}
