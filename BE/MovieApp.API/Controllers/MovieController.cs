using Microsoft.AspNetCore.Mvc;
using MovieApp.Common.DTOs;
using MovieApp.Common.DTOs.Request;
using MovieApp.Common.DTOs.Response;
using MovieApp.Service;
using System.Net;

namespace MovieApp.API.Controllers
{
    [Route("movie")]
    [ApiController]
    public class MovieController : Controller
    {
        private readonly ServiceWrapper _serviceWrapper;

        public MovieController(ServiceWrapper serviceWrapper)
        {
            _serviceWrapper = serviceWrapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _serviceWrapper.MovieService.GetAllMovie();
            if (result.Status < 0)
            {
                return NotFound(new ApiResponseDto<IEnumerable<ResponseMovieDto>>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = result.Message,
                    Count = result.Count,
                    Data = null
                });
            }

            return Ok(new ApiResponseDto<IEnumerable<ResponseMovieDto>>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Count = result.Count,
                Data = (IEnumerable<ResponseMovieDto>)result.Data
                
            });
        }

        // GET: api/movie/{id}
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _serviceWrapper.MovieService.GetByMovieId(id);

            if (result.Status < 0)
            {
                return NotFound(new ApiResponseDto<ResponseMovieDto>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = result.Message,
                    Data = null
                });
            }

            return Ok(new ApiResponseDto<ResponseMovieDto>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Data = (ResponseMovieDto)result.Data
            });

        }

        // GET: api/movie/name={name}
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string name)
        {
            var result = await _serviceWrapper.MovieService.GetByMovieName(name);

            if (result.Status < 0)
            {
                return NotFound(new ApiResponseDto<IEnumerable<ResponseMovieDto>>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = result.Message,
                    Count = result.Count,
                    Data = null
                });
            }

            return Ok(new ApiResponseDto<IEnumerable<ResponseMovieDto>>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Count = result.Count,
                Data = (IEnumerable<ResponseMovieDto>)result.Data
            });
        }

        // POST: api/movie
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RequestMovieDto movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _serviceWrapper.MovieService.Create(movie);

            if (result.Status < 0)
            {
                return NotFound(new ApiResponseDto<ResponseMovieDto>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = result.Message,
                    Data = null
                });
            }

            return Ok(new ApiResponseDto<ResponseMovieDto>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Data = (ResponseMovieDto)result.Data
            });
        }

        // PUT: api/movie/{id}
        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] RequestMovieDto Movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _serviceWrapper.MovieService.Update(id, Movie);

            if (result.Status < 0)
            {
                return NotFound(new ApiResponseDto<ResponseMovieDto>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = result.Message,
                    Data = null
                });
            }

            return Ok(new ApiResponseDto<ResponseMovieDto>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Data = (ResponseMovieDto)result.Data
            });
        }

        // DELETE: api/movie/{id}
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _serviceWrapper.MovieService.DeleteByMovieId(id);

            if (result.Status < 0)
            {
                return NotFound(new ApiResponseDto<ResponseMovieDto>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = result.Message,
                    Data = null
                });
            }

            return Ok(new ApiResponseDto<ResponseMovieDto>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Data = (ResponseMovieDto)result.Data
            });
        }
    }
}
