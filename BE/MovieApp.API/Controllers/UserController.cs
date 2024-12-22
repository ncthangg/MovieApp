using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Common.DTOs.Request;
using MovieApp.Common.DTOs.Response;
using MovieApp.Common.DTOs;
using MovieApp.Service.Services;
using System.Net;
using MovieApp.Data.Models;
using MovieApp.Service;

namespace MovieApp.API.Controllers
{
    [Route("user")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly ServiceWrapper _serviceWrapper;

        public UserController(ServiceWrapper serviceWrapper )
        {
            _serviceWrapper = serviceWrapper;
        }

        // GET: api/movie
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _serviceWrapper.UserService.GetAllUser();
            if (result.Status < 0)
            {
                return NotFound(new ApiResponseDto<IEnumerable<ResponseUserDto>>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = result.Message,
                    Data = null
                });
            }

            return Ok(new ApiResponseDto<IEnumerable<ResponseUserDto>>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Data = (IEnumerable<ResponseUserDto>)result.Data
            });
        }

        // GET: api/movie/{id}
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _serviceWrapper.UserService.GetByUserId(id);
            if (result.Status < 0)
            {
                return NotFound(new ApiResponseDto<ResponseUserDto>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = result.Message,
                    Data = null
                });
            }

            return Ok(new ApiResponseDto<ResponseUserDto>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Data = (ResponseUserDto)result.Data
            });

        }

        // GET: api/movie/name={name}
        [HttpGet("search")]
        public async Task<IActionResult> GetByName([FromQuery] string name)
        {
            var result = await _serviceWrapper.UserService.GetByUserName(name);

            if (result.Status < 0)
            {
                return NotFound(new ApiResponseDto<IEnumerable<ResponseUserDto>>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = result.Message,
                    Data = null
                });
            }

            return Ok(new ApiResponseDto<IEnumerable<ResponseUserDto>>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Data = (IEnumerable<ResponseUserDto>)result.Data
            });
        }

        // POST: api/movie
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _serviceWrapper.UserService.Create(user);
            if (result.Status < 0)
            {
                return NotFound(new ApiResponseDto<IEnumerable<ResponseUserDto>>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = result.Message,
                    Data = null
                });
            }

            return Ok(new ApiResponseDto<ResponseUserDto>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Data = (ResponseUserDto)result.Data
            });
        }

        // PUT: api/movie/{id}
        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] RequestUserDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _serviceWrapper.UserService.Update(id, user);
            if (result.Status < 0)
            {
                return NotFound(new ApiResponseDto<IEnumerable<ResponseUserDto>>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = result.Message,
                    Data = null
                });
            }

            return Ok(new ApiResponseDto<ResponseUserDto>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Data = (ResponseUserDto)result.Data
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

            var result = await _serviceWrapper.UserService.DeleteByUserId(id);
            if (result.Status < 0)
            {
                return NotFound(new ApiResponseDto<IEnumerable<ResponseUserDto>>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = result.Message,
                    Data = null
                });
            }

            return Ok(new ApiResponseDto<ResponseUserDto>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Data = (ResponseUserDto)result.Data
            });
        }

    }
}
