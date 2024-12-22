using AutoMapper;
using Azure;
using MovieApp.Common.Base;
using MovieApp.Common.DTOs.Request;
using MovieApp.Common.DTOs.Response;
using MovieApp.Data;
using MovieApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace MovieApp.Service.Services.Low
{
    public interface IMovieService
    {
        Task<ServiceResult> GetAllMovieCategory();
        Task<ServiceResult> GetByMovieId(long id);
        Task<ServiceResult> GetByMovieName(string name);
        Task<ServiceResult> Create(RequestMovieDto requestRequestMovieDto);
        Task<ServiceResult> Update(long id, RequestMovieDto requestRequestMovieDto);
        Task<ServiceResult> DeleteByMovieId(long id);
    }
    public interface IMovieCategoryService
    {
        Task<ServiceResult> GetAllMovieCategory();
        Task<ServiceResult> GetByMovieId(long id);
        Task<ServiceResult> GetByCategoryId(long id);
        Task<ServiceResult> Create(RequestMovieCategoryDto requestMovieCategoryDto);
        Task<ServiceResult> Update(RequestMovieCategoryDto requestMovieCategoryDto);
        Task<ServiceResult> DeleteByMovieId(long id);
    }
    public interface IMovieRateService
    {
        Task<ServiceResult> GetByMovieId(long id);
        Task<ServiceResult> GetByMovieName(string name);
        Task<ServiceResult> Create(RequestMovieRateDto requestMovieRateDto);
        Task<ServiceResult> Update(RequestMovieRateDto requestMovieRateDto);
        Task<ServiceResult> DeleteByMovieId(long id);
    }
    public interface ICategoryService
    {
        Task<ServiceResult> GetAllCategory();
        Task<ServiceResult> GetByCategoryId(long id);
        Task<ServiceResult> GetByCategoryName(string name);
        Task<ServiceResult> Create(RequestCategoryDto request);
        Task<ServiceResult> Update(long id, RequestCategoryDto request);
        Task<ServiceResult> DeleteByCategoryId(long id);
    }

    public class MovieService : IMovieService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public MovieService(IMapper mapper)
        {
            _unitOfWork ??= new UnitOfWork();
            _mapper = mapper;
        }
        public async Task<ServiceResult> GetAllMovieCategory()
        {
            var movies = await _unitOfWork.MovieRepository.GetAllAsync();
            if (!movies.Any())
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                var response = _mapper.Map<IEnumerable<ResponseMovieDto>>(movies);
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, response);
            }
        }
        public async Task<ServiceResult> GetByMovieId(long movieId)
        {
            var movie = await _unitOfWork.MovieRepository.GetByIdAsync(movieId);
            if (movie == null)
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                var response = _mapper.Map<ResponseMovieDto>(movie);
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, response);
            }
        }
        public async Task<ServiceResult> GetByMovieName(string name)
        {
            var movies = await _unitOfWork.MovieRepository.GetByMovieNameAsync(name);
            if (!movies.Any())
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                var response = _mapper.Map<IEnumerable<ResponseMovieDto>>(movies);
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, response);
            }
        }
        public async Task<ServiceResult> Create(RequestMovieDto x)
        {
            var movieExist = await MovieExist(x.MovieName);
            if (movieExist)
            {
                return new ServiceResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
            }
            else
            {
                var createMovie = new Movie()
                {
                    MovieName = x.MovieName,
                    Description = x.Description,
                    VideoUrl = x.VideoUrl,
                    PosterUrl = x.PosterUrl,
                    Director = x.Director,
                    ReleaseYear = x.ReleaseYear,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };
                var result = await _unitOfWork.MovieRepository.CreateAsync(createMovie);

                var response = _mapper.Map<ResponseMovieDto>(result);
                return new ServiceResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, response);
            }
        }
        public async Task<ServiceResult> Update(long id, RequestMovieDto x)
        {
            var movieIdExist = await MovieExist(id);
            var movieNameExist = await MovieExist(x.MovieName);
            if (movieIdExist && !movieNameExist)
            {
                var movieInfoExist = (await GetByMovieId(id)).Data as Movie;
                var updateMovie = new Movie()
                {
                    MovieId = movieInfoExist.MovieId,
                    MovieName = x.MovieName,
                    Description = x.Description,
                    VideoUrl = x.VideoUrl,
                    PosterUrl = x.PosterUrl,
                    Director = x.Director,
                    ReleaseYear = x.ReleaseYear,
                    CreatedAt = movieInfoExist.CreatedAt,
                    UpdatedAt = DateTime.UtcNow,
                };
                var result = await _unitOfWork.MovieRepository.UpdateAsync(updateMovie);

                var response = _mapper.Map<ResponseMovieDto>(result);
                return new ServiceResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, response);
            }
            else
            {
                return new ServiceResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
            }
        }
        public async Task<ServiceResult> DeleteByMovieId(long id)
        {
            var movieExist = await MovieExist(id);
            if (movieExist)
            {
                var movieInfoExist = await _unitOfWork.MovieRepository.GetByIdAsync(id);
                if (movieInfoExist == null)
                {
                    return new ServiceResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG);
                }
                else
                {
                    await _unitOfWork.MovieRepository.RemoveAsync(movieInfoExist);

                    var response = _mapper.Map<ResponseMovieDto>(movieInfoExist);
                    return new ServiceResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG, response);
                }
            }
            else
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
        }
        private async Task<bool> MovieExist(string name)
        {
            return await _unitOfWork.MovieRepository.EntityExistsByPropertyAsync("MovieName", name);
        }
        private async Task<bool> MovieExist(long id)
        {
            return await _unitOfWork.MovieRepository.EntityExistsByPropertyAsync("MovieId", id);
        }
    }
    public class MovieCategoryService : IMovieCategoryService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public MovieCategoryService(IMapper mapper)
        {
            _unitOfWork ??= new UnitOfWork();
            _mapper = mapper;
        }
        public async Task<ServiceResult> GetAllMovieCategory()
        {
            var movieCategories = await _unitOfWork.MovieCategoryRepository.GetAllAsync();
            if (!movieCategories.Any())
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                var response = _mapper.Map<IEnumerable<ResponseMovieCategoryDto>>(movieCategories);
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, response);
            }
        }

        public async Task<ServiceResult> GetByMovieId(long movieId)
        {
            var movieCategories = await _unitOfWork.MovieCategoryRepository.GetByMovieIdAsync(movieId);
            if (!movieCategories.Any())
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                var response = _mapper.Map<ResponseMovieCategoryDto>(movieCategories);
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, response);
            }
        }

        public async Task<ServiceResult> GetByCategoryId(long categoryId)
        {
            var categoryMovies = await _unitOfWork.MovieCategoryRepository.GetByCategoryIdAsync(categoryId);
            if (!categoryMovies.Any())
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                var response = _mapper.Map<ResponseCategoryMovieDto>(categoryMovies);
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, response);
            }
        }

        public async Task<ServiceResult> Create(RequestMovieCategoryDto requestRequestMovieCategoryDto)
        {
            // Kiểm tra danh sách danh mục không rỗng
            if (requestRequestMovieCategoryDto.CategoryIds == null || !requestRequestMovieCategoryDto.CategoryIds.Any())
            {
                return new ServiceResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
            }

            var exists = await _unitOfWork.MovieRepository.GetByIdAsync(requestRequestMovieCategoryDto.MovieId);
            if (exists == null)
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                var categoryList = requestRequestMovieCategoryDto.CategoryIds != null
                                ? new List<long>(requestRequestMovieCategoryDto.CategoryIds)
                                : new List<long>();
                await _unitOfWork.MovieCategoryRepository.CreateAsync(requestRequestMovieCategoryDto.MovieId, categoryList);
                return new ServiceResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG);
            }
        }

        public async Task<ServiceResult> Update(RequestMovieCategoryDto requestRequestMovieCategoryDto)
        {
            // Kiểm tra danh sách danh mục không rỗng
            if (requestRequestMovieCategoryDto.CategoryIds == null || !requestRequestMovieCategoryDto.CategoryIds.Any())
            {
                return new ServiceResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
            }

            var exists = await _unitOfWork.MovieRepository.GetByIdAsync(requestRequestMovieCategoryDto.MovieId);
            if (exists == null)
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                var categoryList = requestRequestMovieCategoryDto.CategoryIds != null
                ? new List<long>(requestRequestMovieCategoryDto.CategoryIds)
                : new List<long>();
                await _unitOfWork.MovieCategoryRepository.UpdateAsync(requestRequestMovieCategoryDto.MovieId, categoryList);
                return new ServiceResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
            }
        }

        public async Task<ServiceResult> DeleteByMovieId(long movieId)
        {
            var movieCategories = await _unitOfWork.MovieCategoryRepository.GetByMovieIdAsync(movieId);
            if (!movieCategories.Any())
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                foreach (var movieCategory in movieCategories)
                {
                    await _unitOfWork.MovieCategoryRepository.RemoveAsync(movieCategory);
                }
                return new ServiceResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
            }
        }

        public async Task<ServiceResult> DeleteByCategoryId(long categoryId)
        {
            var movieCategories = await _unitOfWork.MovieCategoryRepository.GetByCategoryIdAsync(categoryId);
            if (!movieCategories.Any())
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                foreach (var movieCategory in movieCategories)
                {
                    await _unitOfWork.MovieCategoryRepository.RemoveAsync(movieCategory);
                }
                return new ServiceResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
            }
        }
    }
    public class MovieRateService : IMovieRateService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public MovieRateService(IMapper mapper)
        {
            _unitOfWork ??= new UnitOfWork();
            _mapper = mapper;
        }
        public Task<ServiceResult> GetByMovieId(long id)
        {
            throw new NotImplementedException();
        }
        public Task<ServiceResult> GetByMovieName(string name)
        {
            throw new NotImplementedException();
        }
        public Task<ServiceResult> Create(RequestMovieRateDto x)
        {
            throw new NotImplementedException();
        }
        public Task<ServiceResult> Update(RequestMovieRateDto x)
        {
            throw new NotImplementedException();
        }
        public Task<ServiceResult> DeleteByMovieId(long id)
        {
            throw new NotImplementedException();
        }
    }
    public class CategoryService : ICategoryService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CategoryService(IMapper mapper)
        {
            _unitOfWork ??= new UnitOfWork();
            _mapper = mapper;
        }
        public async Task<ServiceResult> GetAllCategory()
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllAsync();
            if (!categories.Any())
            {
                return new ServiceResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, null);
            }
            else
            {
                var response = _mapper.Map<IEnumerable<ResponseCategoryDto>>(categories);
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, response);
            }
        }
        public async Task<ServiceResult> GetByCategoryId(long id)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return new ServiceResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, null);
            }
            else
            {
                var response = _mapper.Map<ResponseCategoryDto>(category);
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, response);
            }
        }
        public async Task<ServiceResult> GetByCategoryName(string name)
        {
            var categories = await _unitOfWork.CategoryRepository.GetByCategoryNameAsync(name);
            if (!categories.Any())
            {
                return new ServiceResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, null);
            }
            else
            {
                var response = _mapper.Map<IEnumerable<ResponseCategoryDto>>(categories);
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, categories);
            }
        }
        public async Task<ServiceResult> Create(RequestCategoryDto request)
        {
            var categoryExist = await CategoryExist(request.CategoryName);
            if (categoryExist)
            {
                return new ServiceResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
            }
            else
            {
                var newCategory = new Category()
                {
                    CategoryName = request.CategoryName
                };
                var result = await _unitOfWork.CategoryRepository.CreateAsync(newCategory);

                var response = _mapper.Map<ResponseCategoryDto>(result);
                return new ServiceResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, response);
            }
        }
        public async Task<ServiceResult> Update(long id, RequestCategoryDto request)
        {
            var categoryIdExist = await CategoryExist(id);
            var categoryNameExist = await CategoryExist(request.CategoryName);
            if (categoryIdExist && !categoryNameExist)
            {
                var updateCategory = new Category()
                {
                    CategoryId = id,
                    CategoryName = request.CategoryName
                };
                var result = await _unitOfWork.CategoryRepository.UpdateAsync(updateCategory);

                var response = _mapper.Map<ResponseCategoryDto>(result);
                return new ServiceResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, response);
            }
            else
            {
                return new ServiceResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
            }
        }
        public async Task<ServiceResult> DeleteByCategoryId(long id)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return new ServiceResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG);
            }
            else
            {
                await _unitOfWork.CategoryRepository.RemoveAsync(category);
                var response = _mapper.Map<ResponseCategoryDto>(category);
                return new ServiceResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG, response);
            }
        }
        private async Task<bool> CategoryExist(long id)
        {
            return await _unitOfWork.CategoryRepository.EntityExistsByPropertyAsync("CategoryId", id);
        }
        private async Task<bool> CategoryExist(string name)
        {
            return await _unitOfWork.CategoryRepository.EntityExistsByPropertyAsync("CategoryName", name);
        }
    }

}
