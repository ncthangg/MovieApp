using AutoMapper;
using MovieApp.Common.Base;
using MovieApp.Common.DTOs.Request;
using MovieApp.Common.DTOs.Response;
using MovieApp.Data;
using MovieApp.Data.Models;

namespace MovieApp.Service.Services.Low
{
    public interface IMovieService
    {
        Task<ServiceResult> GetAllMovie();
        Task<ServiceResult> GetByMovieId(long id);
        Task<ServiceResult> GetByMovieName(string name);
        Task<ServiceResult> Create(RequestMovieDto RequestMovieDto);
        Task<ServiceResult> Update(long id, RequestMovieDto RequestMovieDto);
        Task<ServiceResult> DeleteByMovieId(long id);
    }
    public interface IMovieSeasonService
    {
        Task<ServiceResult> GetAllSeason();
        Task<ServiceResult> GetBySeasonId(long id);
        Task<ServiceResult> GetBySeasonName(string name);
        Task<ServiceResult> Create(RequestMovieSeasonDto RequestMovieSeasonDto);
        Task<ServiceResult> Update(long id, RequestMovieSeasonDto RequestMovieSeasonDto);
        Task<ServiceResult> DeleteBySeasonId(long id);
    }
    public interface IMovieEpisodeService
    {
        Task<ServiceResult> GetAllEpisode();
        Task<ServiceResult> GetByEpisodeId(long id);
        Task<ServiceResult> GetByEpisodeName(string name);
        Task<ServiceResult> Create(RequestMovieEpisodeDto RequestMovieEpisodeDto);
        Task<ServiceResult> Update(long id, RequestMovieEpisodeDto RequestMovieEpisodeDto);
        Task<ServiceResult> DeleteByEpisodeId(long id);
    }
    public interface IMovieActorService
    {
        Task<ServiceResult> GetAllMovieActor();
        Task<ServiceResult> GetByMovieId(long id);
        Task<ServiceResult> GetByActorId(long id);
        Task<ServiceResult> Upsert(RequestMovieActorDto RequestMovieActorDto);
        //Task<ServiceResult> Create(RequestMovieActorDto RequestMovieActorDto);
        //Task<ServiceResult> Update(long id, RequestMovieActorDto RequestMovieActorDto);
        Task<ServiceResult> DeleteByMovieId(long id);
    }
    public interface IMovieCategoryService
    {
        Task<ServiceResult> GetAllMovieCategory();
        Task<ServiceResult> GetByMovieId(long id);
        Task<ServiceResult> GetByCategoryId(long id);
        Task<ServiceResult> Upsert(RequestMovieCategoryDto RequestMovieCategoryDto);
        //Task<ServiceResult> Create(RequestMovieCategoryDto requestMovieCategoryDto);
        //Task<ServiceResult> Update(RequestMovieCategoryDto requestMovieCategoryDto);
        Task<ServiceResult> DeleteByMovieId(long id);
    }
    public interface IMovieRateService
    {
        Task<ServiceResult> GetByMovieId(long id);
        Task<ServiceResult> GetByMovieName(string name);
        Task<ServiceResult> Create(RequestMovieRateDto RequestMovieRateDto);
        Task<ServiceResult> Update(RequestMovieRateDto RequestMovieRateDto);
        Task<ServiceResult> DeleteByMovieId(long id);
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
        public async Task<ServiceResult> GetAllMovie()
        {
            var movies = await _unitOfWork.MovieRepository.GetAllAsync();
            if (!movies.Any())
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                var response = _mapper.Map<IEnumerable<ResponseMovieDto>>(movies);
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, response, response.Count());
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
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, response, response.Count());
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
                    TypeId = x.TypeId,
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
    public class MovieSeasonService : IMovieSeasonService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public MovieSeasonService(IMapper mapper)
        {
            _unitOfWork ??= new UnitOfWork();
            _mapper = mapper;
        }
        public async Task<ServiceResult> GetAllSeason()
        {
            var movies = await _unitOfWork.MovieSeasonRepository.GetAllAsync();
            if (!movies.Any())
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                var response = _mapper.Map<IEnumerable<ResponseMovieSeasonDto>>(movies);
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, response, response.Count());
            }
        }
        public async Task<ServiceResult> GetBySeasonId(long movieId)
        {
            var movie = await _unitOfWork.MovieSeasonRepository.GetByIdAsync(movieId);
            if (movie == null)
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                var response = _mapper.Map<ResponseMovieSeasonDto>(movie);
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, response);
            }
        }
        public async Task<ServiceResult> GetBySeasonName(string name)
        {
            var movies = await _unitOfWork.MovieSeasonRepository.GetByMovieNameAsync(name);
            if (!movies.Any())
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                var response = _mapper.Map<IEnumerable<ResponseMovieSeasonDto>>(movies);
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, response, response.Count());
            }
        }
        public async Task<ServiceResult> Create(RequestMovieSeasonDto x)
        {
            var seasonExist = await SeasonExist(x.SeasonName);
            if (seasonExist)
            {
                return new ServiceResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
            }
            else
            {
                var createSeason = new MovieSeason()
                { 
                    MovieId = x.MovieId,
                    SeasonName = x.SeasonName,
                    PosterUrl = x.PosterUrl,
                    ReleaseYear = x.ReleaseYear,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };
                var result = await _unitOfWork.MovieSeasonRepository.CreateAsync(createSeason);

                var response = _mapper.Map<ResponseMovieSeasonDto>(result);
                return new ServiceResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, response);
            }
        }
        public async Task<ServiceResult> Update(long id, RequestMovieSeasonDto x)
        {
            var seasonIdExist = await SeasonExist(id);
            var seasonNameExist = await SeasonExist(x.SeasonName);
            if (seasonIdExist && !seasonNameExist)
            {
                var movieInfoExist = (await GetBySeasonId(id)).Data as Movie;
                var updateSeason = new MovieSeason()
                {
                    MovieId = movieInfoExist.MovieId,
                    SeasonName = x.SeasonName,
                    PosterUrl = x.PosterUrl,
                    ReleaseYear = x.ReleaseYear,
                    CreatedAt = movieInfoExist.CreatedAt,
                    UpdatedAt = DateTime.UtcNow,
                };
                var result = await _unitOfWork.MovieSeasonRepository.UpdateAsync(updateSeason);

                var response = _mapper.Map<ResponseMovieSeasonDto>(result);
                return new ServiceResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, response);
            }
            else
            {
                return new ServiceResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
            }
        }
        public async Task<ServiceResult> DeleteBySeasonId(long id)
        {
            var movieExist = await SeasonExist(id);
            if (movieExist)
            {
                var movieInfoExist = await _unitOfWork.MovieSeasonRepository.GetByIdAsync(id);
                if (movieInfoExist == null)
                {
                    return new ServiceResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG);
                }
                else
                {
                    await _unitOfWork.MovieSeasonRepository.RemoveAsync(movieInfoExist);

                    var response = _mapper.Map<ResponseMovieSeasonDto>(movieInfoExist);
                    return new ServiceResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG, response);
                }
            }
            else
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
        }
        private async Task<bool> SeasonExist(string name)
        {
            return await _unitOfWork.MovieSeasonRepository.EntityExistsByPropertyAsync("SeasonName", name);
        }
        private async Task<bool> SeasonExist(long id)
        {
            return await _unitOfWork.MovieSeasonRepository.EntityExistsByPropertyAsync("SeasonId", id);
        }
    }
    public class MovieEpisodeService : IMovieEpisodeService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public MovieEpisodeService(IMapper mapper)
        {
            _unitOfWork ??= new UnitOfWork();
            _mapper = mapper;
        }
        public async Task<ServiceResult> GetAllEpisode()
        {
            var movies = await _unitOfWork.MovieEpisodeRepository.GetAllAsync();
            if (!movies.Any())
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                var response = _mapper.Map<IEnumerable<ResponseMovieEpisodeDto>>(movies);
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, response, response.Count());
            }
        }
        public async Task<ServiceResult> GetByEpisodeId(long movieId)
        {
            var movie = await _unitOfWork.MovieEpisodeRepository.GetByIdAsync(movieId);
            if (movie == null)
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                var response = _mapper.Map<ResponseMovieEpisodeDto>(movie);
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, response);
            }
        }
        public async Task<ServiceResult> GetByEpisodeName(string name)
        {
            var movies = await _unitOfWork.MovieEpisodeRepository.GetByEpisodeNameAsync(name);
            if (!movies.Any())
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                var response = _mapper.Map<IEnumerable<ResponseMovieEpisodeDto>>(movies);
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, response, response.Count());
            }
        }
        public async Task<ServiceResult> Create(RequestMovieEpisodeDto x)
        {
            var movieExist = await EpisodeExist(x.EpisodeName);
            if (movieExist)
            {
                return new ServiceResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
            }
            else
            {
                var createEpisode = new MovieEpisode()
                {
                    SeasonId = x.SeasonId,
                    EpisodeName = x.EpisodeName,
                    Description = x.Description,
                    VideoUrl = x.VideoUrl,
                    PosterUrl = x.PosterUrl,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };
                var result = await _unitOfWork.MovieEpisodeRepository.CreateAsync(createEpisode);

                var response = _mapper.Map<ResponseMovieEpisodeDto>(result);
                return new ServiceResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, response);
            }
        }
        public async Task<ServiceResult> Update(long id, RequestMovieEpisodeDto x)
        {
            var movieIdExist = await EpisodeExist(id);
            var movieNameExist = await EpisodeExist(x.EpisodeName);
            if (movieIdExist && !movieNameExist)
            {
                var movieInfoExist = (await GetByEpisodeId(id)).Data as MovieEpisode;
                var updateEpisode = new MovieEpisode()
                {
                    EpisodeId = movieInfoExist.EpisodeId,
                    SeasonId = x.SeasonId,
                    EpisodeName = x.EpisodeName,
                    Description = x.Description,
                    VideoUrl = x.VideoUrl,
                    PosterUrl = x.PosterUrl, 
                    CreatedAt = movieInfoExist.CreatedAt,
                    UpdatedAt = DateTime.UtcNow,
                };
                var result = await _unitOfWork.MovieEpisodeRepository.UpdateAsync(updateEpisode);

                var response = _mapper.Map<ResponseMovieEpisodeDto>(result);
                return new ServiceResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, response);
            }
            else
            {
                return new ServiceResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
            }
        }
        public async Task<ServiceResult> DeleteByEpisodeId(long id)
        {
            var movieExist = await EpisodeExist(id);
            if (movieExist)
            {
                var movieInfoExist = await _unitOfWork.MovieEpisodeRepository.GetByIdAsync(id);
                if (movieInfoExist == null)
                {
                    return new ServiceResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG);
                }
                else
                {
                    await _unitOfWork.MovieEpisodeRepository.RemoveAsync(movieInfoExist);

                    var response = _mapper.Map<ResponseMovieEpisodeDto>(movieInfoExist);
                    return new ServiceResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG, response);
                }
            }
            else
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
        }
        private async Task<bool> EpisodeExist(string name)
        {
            return await _unitOfWork.MovieEpisodeRepository.EntityExistsByPropertyAsync("EpisodeName", name);
        }
        private async Task<bool> EpisodeExist(long id)
        {
            return await _unitOfWork.MovieEpisodeRepository.EntityExistsByPropertyAsync("EpisodeId", id);
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
                var groupedData = movieCategories
                           .GroupBy(mc => mc.MovieId)
                           .Select(group => new ResponseMovieCategoryDto
                           {
                               MovieId = group.Key,
                               Category = _mapper.Map<List<ResponseCategoryDto>>(group.Select(mc => mc.Category).ToList())
                           })
                           .ToList();
                //var response = _mapper.Map<IEnumerable<ResponseMovieCategoryDto>>(movieCategories);
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, groupedData, groupedData.Count);
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
        public async Task<ServiceResult> Upsert(RequestMovieCategoryDto requestMovieCategoryDto)
        {
            if (requestMovieCategoryDto.CategoryIds == null || !requestMovieCategoryDto.CategoryIds.Any())
            {
                return new ServiceResult(Const.FAIL_UPDATE_CODE, "Danh sách danh mục không hợp lệ.");
            }

            // Lấy danh sách hiện tại từ database
            //var existingCategories = await _unitOfWork.MovieCategoryRepository.GetByMovieIdAsync(requestMovieCategoryDto.MovieId);

            // Tìm các danh mục cần thêm mới
            var categoriesToAdd = await _unitOfWork.MovieCategoryRepository.GetCategoriesToAddAsync(requestMovieCategoryDto.MovieId, requestMovieCategoryDto.CategoryIds.ToList());

            // Tìm các danh mục cần xóa
            var categoriesToRemove = await _unitOfWork.MovieCategoryRepository.GetCategoriesToRemoveAsync(requestMovieCategoryDto.MovieId, requestMovieCategoryDto.CategoryIds.ToList());

            // Thực hiện thêm mới
            if (categoriesToAdd.Any())
            {
                await _unitOfWork.MovieCategoryRepository.CreateAsync(requestMovieCategoryDto.MovieId, categoriesToAdd);
            }

            // Thực hiện xóa
            if (categoriesToRemove.Any())
            {
                await _unitOfWork.MovieCategoryRepository.RemoveCategoriesAsync(requestMovieCategoryDto.MovieId, categoriesToRemove);
            }

            return new ServiceResult(Const.SUCCESS_UPDATE_CODE, "Cập nhật danh mục thành công.");
        }
        #region Create and Update SERVICE OLD
        //public async Task<ServiceResult> Create(RequestMovieCategoryDto requestRequestMovieCategoryDto)
        //{
        //    // Kiểm tra danh sách danh mục không rỗng
        //    if (requestRequestMovieCategoryDto.CategoryIds == null || !requestRequestMovieCategoryDto.CategoryIds.Any())
        //    {
        //        return new ServiceResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
        //    }

        //    var exists = await _unitOfWork.MovieRepository.GetByIdAsync(requestRequestMovieCategoryDto.MovieId);
        //    if (exists == null)
        //    {
        //        return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
        //    }
        //    else
        //    {
        //        var categoryList = requestRequestMovieCategoryDto.CategoryIds != null
        //                        ? new List<long>(requestRequestMovieCategoryDto.CategoryIds)
        //                        : new List<long>();
        //        await _unitOfWork.MovieCategoryRepository.CreateAsync(requestRequestMovieCategoryDto.MovieId, categoryList);
        //        return new ServiceResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG);
        //    }
        //}

        //public async Task<ServiceResult> Update(RequestMovieCategoryDto requestRequestMovieCategoryDto)
        //{
        //    // Kiểm tra danh sách danh mục không rỗng
        //    if (requestRequestMovieCategoryDto.CategoryIds == null || !requestRequestMovieCategoryDto.CategoryIds.Any())
        //    {
        //        return new ServiceResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
        //    }

        //    var exists = await _unitOfWork.MovieRepository.GetByIdAsync(requestRequestMovieCategoryDto.MovieId);
        //    if (exists == null)
        //    {
        //        return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
        //    }
        //    else
        //    {
        //        var categoryList = requestRequestMovieCategoryDto.CategoryIds != null
        //        ? new List<long>(requestRequestMovieCategoryDto.CategoryIds)
        //        : new List<long>();
        //        await _unitOfWork.MovieCategoryRepository.UpdateAsync(requestRequestMovieCategoryDto.MovieId, categoryList);
        //        return new ServiceResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
        //    }
        //}
        #endregion
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
    public class MovieActorService : IMovieActorService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public MovieActorService(IMapper mapper)
        {
            _unitOfWork ??= new UnitOfWork();
            _mapper = mapper;
        }
        public async Task<ServiceResult> GetAllMovieActor()
        {
            var movieCategories = await _unitOfWork.MovieActorRepository.GetAllAsync();
            if (!movieCategories.Any())
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                var groupedData = movieCategories
                           .GroupBy(mc => mc.MovieId)
                           .Select(group => new ResponseMovieActorDto
                           {
                               MovieId = group.Key,
                               Actor = _mapper.Map<List<ResponseActorDto>>(group.Select(mc => mc.Actor).ToList())
                           })
                           .ToList();
                //var response = _mapper.Map<IEnumerable<ResponseMovieCategoryDto>>(movieCategories);
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, groupedData, groupedData.Count);
            }
        }
        public async Task<ServiceResult> GetByMovieId(long movieId)
        {
            var movieCategories = await _unitOfWork.MovieActorRepository.GetByMovieIdAsync(movieId);
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
        public async Task<ServiceResult> GetByActorId(long actorId)
        {
            var actorMovies = await _unitOfWork.MovieActorRepository.GetByActorIdAsync(actorId);
            if (!actorMovies.Any())
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                var response = _mapper.Map<ResponseCategoryMovieDto>(actorMovies);
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, response);
            }
        }
        public async Task<ServiceResult> Upsert(RequestMovieActorDto requestMovieActorDto)
        {
            if (requestMovieActorDto.ActorIds == null || !requestMovieActorDto.ActorIds.Any())
            {
                return new ServiceResult(Const.FAIL_UPDATE_CODE, "Danh sách danh mục không hợp lệ.");
            }

            // Lấy danh sách hiện tại từ database
            //var existingCategories = await _unitOfWork.MovieCategoryRepository.GetByMovieIdAsync(requestMovieCategoryDto.MovieId);

            // Tìm các danh mục cần thêm mới
            var moviesToAdd = await _unitOfWork.MovieActorRepository.GetActorsToAddAsync(requestMovieActorDto.MovieId, requestMovieActorDto.ActorIds.ToList());

            // Tìm các danh mục cần xóa
            var moviesToRemove = await _unitOfWork.MovieActorRepository.GetActorsToRemoveAsync(requestMovieActorDto.MovieId, requestMovieActorDto.ActorIds.ToList());

            // Thực hiện thêm mới
            if (moviesToAdd.Any())
            {
                await _unitOfWork.MovieActorRepository.CreateAsync(requestMovieActorDto.MovieId, moviesToAdd);
            }

            // Thực hiện xóa
            if (moviesToRemove.Any())
            {
                await _unitOfWork.MovieActorRepository.RemoveActorAsync(requestMovieActorDto.MovieId, moviesToRemove);
            }

            return new ServiceResult(Const.SUCCESS_UPDATE_CODE, "Cập nhật danh mục thành công.");
        }
        #region Create and Update SERVICE OLD
        //public async Task<ServiceResult> Create(RequestMovieCategoryDto requestRequestMovieCategoryDto)
        //{
        //    // Kiểm tra danh sách danh mục không rỗng
        //    if (requestRequestMovieCategoryDto.CategoryIds == null || !requestRequestMovieCategoryDto.CategoryIds.Any())
        //    {
        //        return new ServiceResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
        //    }

        //    var exists = await _unitOfWork.MovieRepository.GetByIdAsync(requestRequestMovieCategoryDto.MovieId);
        //    if (exists == null)
        //    {
        //        return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
        //    }
        //    else
        //    {
        //        var categoryList = requestRequestMovieCategoryDto.CategoryIds != null
        //                        ? new List<long>(requestRequestMovieCategoryDto.CategoryIds)
        //                        : new List<long>();
        //        await _unitOfWork.MovieCategoryRepository.CreateAsync(requestRequestMovieCategoryDto.MovieId, categoryList);
        //        return new ServiceResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG);
        //    }
        //}

        //public async Task<ServiceResult> Update(RequestMovieCategoryDto requestRequestMovieCategoryDto)
        //{
        //    // Kiểm tra danh sách danh mục không rỗng
        //    if (requestRequestMovieCategoryDto.CategoryIds == null || !requestRequestMovieCategoryDto.CategoryIds.Any())
        //    {
        //        return new ServiceResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
        //    }

        //    var exists = await _unitOfWork.MovieRepository.GetByIdAsync(requestRequestMovieCategoryDto.MovieId);
        //    if (exists == null)
        //    {
        //        return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
        //    }
        //    else
        //    {
        //        var categoryList = requestRequestMovieCategoryDto.CategoryIds != null
        //        ? new List<long>(requestRequestMovieCategoryDto.CategoryIds)
        //        : new List<long>();
        //        await _unitOfWork.MovieCategoryRepository.UpdateAsync(requestRequestMovieCategoryDto.MovieId, categoryList);
        //        return new ServiceResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
        //    }
        //}
        #endregion
        public async Task<ServiceResult> DeleteByMovieId(long movieId)
        {
            var movieActors = await _unitOfWork.MovieActorRepository.GetByMovieIdAsync(movieId);
            if (!movieActors.Any())
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                foreach (var movieActor in movieActors)
                {
                    await _unitOfWork.MovieActorRepository.RemoveAsync(movieActor);
                }
                return new ServiceResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
            }
        }
        public async Task<ServiceResult> DeleteByCategoryId(long actorId)
        {
            var movieActors = await _unitOfWork.MovieActorRepository.GetByActorIdAsync(actorId);
            if (!movieActors.Any())
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                foreach (var movieActor in movieActors)
                {
                    await _unitOfWork.MovieActorRepository.RemoveAsync(movieActor);
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

}
