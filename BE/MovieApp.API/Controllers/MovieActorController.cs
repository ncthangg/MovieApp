using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Common.DTOs.Request;
using MovieApp.Common.DTOs.Response;
using MovieApp.Common.DTOs;
using MovieApp.Service;
using System.Net;

namespace MovieApp.API.Controllers
{
    [Route("movieactor")]
    [ApiController] 
    public class MovieActorController : Controller
    {
        private readonly ServiceWrapper _serviceWrapper;
        private readonly IMapper _mapper;

        public MovieActorController(ServiceWrapper serviceWrapper, IMapper mapper)
        {
            _serviceWrapper = serviceWrapper;
            _mapper = mapper;
        }
        // GET: /movie/{movieId}/categories
        [HttpGet]
        public async Task<IActionResult> GetAllMovieCategory()
        {
            var result = await _serviceWrapper.MovieActorService.GetAllMovieActor();
            if (result.Status < 0)
            {
                return NotFound(new ApiResponseDto<IEnumerable<ResponseMovieActorDto>>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = result.Message,
                    Count = result.Count,
                    Data = null
                });
            }

            return Ok(new ApiResponseDto<IEnumerable<ResponseMovieActorDto>>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Count = result.Count,
                Data = (IEnumerable<ResponseMovieActorDto>)result.Data
            });

        }

        // GET: /movie/{movieId}/categories
        [HttpGet("movie/{movieId}/actors")]
        public async Task<IActionResult> GetActorsByMovieId(long movieId)
        {
            var result = await _serviceWrapper.MovieActorService.GetByMovieId(movieId);
            if (result.Status < 0)
            {
                return NotFound(new ApiResponseDto<ResponseMovieActorDto>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = result.Message,
                    Data = null
                });
            }

            return Ok(new ApiResponseDto<ResponseMovieActorDto>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Data = (ResponseMovieActorDto)result.Data
            });

        }

        // GET: /category/{categoryId}/movies
        [HttpGet("actor/{actorId}/movies")]
        public async Task<IActionResult> GetMoviesByActorId(long categoryId)
        {
            var result = await _serviceWrapper.MovieActorService.GetByMovieId(categoryId);
            if (result.Status < 0)
            {
                return NotFound(new ApiResponseDto<ResponseMovieActorDto>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = result.Message,
                    Data = null
                });
            }

            return Ok(new ApiResponseDto<ResponseMovieActorDto>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Data = (ResponseMovieActorDto)result.Data
            });
        }
        [HttpPost]
        public async Task<IActionResult> SetCategoriesToMovie([FromBody] RequestMovieActorDto RequestMovieActorDto)
        {
            var result = await _serviceWrapper.MovieActorService.Upsert(RequestMovieActorDto);
            var responseData = _mapper.Map<ResponseMovieActorDto>(result.Data);

            if (result.Status < 0)
            {
                return NotFound(new ApiResponseDto<ResponseMovieActorDto>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = result.Message,
                    Data = null
                });
            }

            return Ok(new ApiResponseDto<ResponseMovieActorDto>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Data = responseData
            });
        }
    }
}
