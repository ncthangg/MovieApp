using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Common.DTOs;
using MovieApp.Common.DTOs.Request;
using MovieApp.Common.DTOs.Response;
using MovieApp.Service;
using System.Net;

namespace MovieApp.API.Controllers
{
    [Route("moviecategory")]
    [ApiController]
    public class MovieCategoryController : Controller
    {
        private readonly ServiceWrapper _serviceWrapper;
        private readonly IMapper _mapper;

        public MovieCategoryController(ServiceWrapper serviceWrapper, IMapper mapper)
        {
            _serviceWrapper = serviceWrapper;
            _mapper = mapper;
        }
        // GET: /movie/{movieId}/categories
        [HttpGet]
        public async Task<IActionResult> GetAllMovieCategory()
        {
            var result = await _serviceWrapper.MovieCategoryService.GetAllMovieCategory();
            if (result.Status < 0)
            {
                return NotFound(new ApiResponseDto<IEnumerable<ResponseMovieCategoryDto>>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = result.Message,
                    Data = null
                });
            }

            return Ok(new ApiResponseDto<IEnumerable<ResponseMovieCategoryDto>>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Count = result.Count,
                Data = (IEnumerable<ResponseMovieCategoryDto>)result.Data
            });

        }

        // GET: /movie/{movieId}/categories
        [HttpGet("movie/{movieId}/categories")]
        public async Task<IActionResult> GetCategoriesByMovieId(long movieId)
        {
            var result = await _serviceWrapper.MovieCategoryService.GetByMovieId(movieId);
            if (result.Status < 0)
            {
                return NotFound(new ApiResponseDto<ResponseMovieCategoryDto>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = result.Message,
                    Data = null
                });
            }

            return Ok(new ApiResponseDto<ResponseMovieCategoryDto>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Data = (ResponseMovieCategoryDto)result.Data
            });

        }

        // GET: /category/{categoryId}/movies
        [HttpGet("category/{categoryId}/movies")]
        public async Task<IActionResult> GetMoviesByCategoryId(long categoryId)
        {
            var result = await _serviceWrapper.MovieCategoryService.GetByCategoryId(categoryId);
            if (result.Status < 0)
            {
                return NotFound(new ApiResponseDto<ResponseCategoryMovieDto>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = result.Message,
                    Data = null
                });
            }

            return Ok(new ApiResponseDto<ResponseCategoryMovieDto>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Data = (ResponseCategoryMovieDto)result.Data
            });
        }
        [HttpPost]
        public async Task<IActionResult> SetCategoriesToMovie([FromBody] RequestMovieCategoryDto requestMovieCategoryDto)
        {
            var result = await _serviceWrapper.MovieCategoryService.Upsert(requestMovieCategoryDto);
            var responseData = _mapper.Map<ResponseMovieCategoryDto>(result.Data);

            if (result.Status < 0)
            {
                return NotFound(new ApiResponseDto<ResponseMovieCategoryDto>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = result.Message,
                    Data = null
                });
            }

            return Ok(new ApiResponseDto<ResponseMovieCategoryDto>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Data = responseData
            });
        }

        #region Create and Update CONTROLLER OLD
        //[HttpPost]
        //public async Task<IActionResult> AddCategoriesToMovie([FromBody] RequestMovieCategoryDto requestMovieCategoryDto)
        //{
        //    var result = await _serviceWrapper.MovieCategoryService.Create(requestMovieCategoryDto);
        //    var responseData = _mapper.Map<ResponseMovieCategoryDto>(result.Data);

        //    if (result.Status < 0)
        //    {
        //        return NotFound(new ApiResponseDto<ResponseMovieCategoryDto>
        //        {
        //            StatusCode = HttpStatusCode.NotFound,
        //            Message = result.Message,
        //            Data = null
        //        });
        //    }

        //    return Ok(new ApiResponseDto<ResponseMovieCategoryDto>
        //    {
        //        StatusCode = HttpStatusCode.OK,
        //        Message = result.Message,
        //        Data = responseData
        //    });
        //}

        //[HttpPut]
        //public async Task<IActionResult> UpdateCategoryFromMovie([FromBody] RequestMovieCategoryDto requestMovieCategoryDto)
        //{
        //    var result = await _serviceWrapper.MovieCategoryService.Update(requestMovieCategoryDto);
        //    var responseData = _mapper.Map<ResponseMovieCategoryDto>(result.Data);

        //    if (result.Status < 0)
        //    {
        //        return NotFound(new ApiResponseDto<ResponseMovieCategoryDto>
        //        {
        //            StatusCode = HttpStatusCode.NotFound,
        //            Message = result.Message,
        //            Data = null
        //        });
        //    }

        //    return Ok(new ApiResponseDto<ResponseMovieCategoryDto>
        //    {
        //        StatusCode = HttpStatusCode.OK,
        //        Message = result.Message,
        //        Data = responseData
        //    });
        //}
        #endregion

    }
}
