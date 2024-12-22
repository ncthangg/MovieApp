using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Common.DTOs.Request;
using MovieApp.Common.DTOs.Response;
using MovieApp.Common.DTOs;
using MovieApp.Service.Services;
using System.Net;
using MovieApp.Service;

namespace MovieApp.API.Controllers
{
    [Route("status")]
    [ApiController]
    public class UserStatusController : Controller
    {
        private readonly ServiceWrapper _serviceWrapper;

        public UserStatusController(ServiceWrapper serviceWrapper)
        {
            _serviceWrapper = serviceWrapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _serviceWrapper.UserStatusService.GetAllUserStatus();
            if (result.Status < 0)
            {
                return NotFound(new ApiResponseDto<IEnumerable<ResponseUserStatusDto>>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = result.Message,
                    Data = null
                });
            }

            return Ok(new ApiResponseDto<IEnumerable<ResponseUserStatusDto>>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Data = (IEnumerable<ResponseUserStatusDto>)result.Data
            });
        }

        // GET: api/movie/{id}
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _serviceWrapper.UserStatusService.GetByStatusId(id);
            if (result.Status < 0)
            {
                return NotFound(new ApiResponseDto<ResponseUserStatusDto>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = result.Message,
                    Data = null
                });
            }

            return Ok(new ApiResponseDto<ResponseUserStatusDto>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Data = (ResponseUserStatusDto)result.Data
            });

        }

        // GET: api/movie/name={name}
        [HttpGet("search")]
        public async Task<IActionResult> GetByName([FromQuery] string name)
        {
            var result = await _serviceWrapper.UserStatusService.GetByStatusName(name);

            if (result.Status < 0)
            {
                return NotFound(new ApiResponseDto<IEnumerable<ResponseUserStatusDto>>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = result.Message,
                    Data = null
                });
            }

            return Ok(new ApiResponseDto<IEnumerable<ResponseUserStatusDto>>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Data = (IEnumerable<ResponseUserStatusDto>)result.Data
            });
        }

        // POST: api/movie
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RequestUserStatusDto status)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _serviceWrapper.UserStatusService.Create(status);
            if (result.Status < 0)
            {
                return NotFound(new ApiResponseDto<ResponseUserStatusDto>
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = result.Message,
                    Data = null
                });
            }

            return Ok(new ApiResponseDto<ResponseUserStatusDto>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Data = (ResponseUserStatusDto)result.Data
            });
        }

        // PUT: api/movie/{id}
        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] RequestUserStatusDto status)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _serviceWrapper.UserStatusService.Update(id, status);
            if (result.Status < 0)
            {
                return NotFound(new ApiResponseDto<ResponseUserStatusDto>
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = result.Message,
                    Data = null
                });
            }

            return Ok(new ApiResponseDto<ResponseUserStatusDto>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Data = (ResponseUserStatusDto)result.Data
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

            var result = await _serviceWrapper.UserStatusService.DeleteByStatusId(id);
            if (result.Status < 0)
            {
                return NotFound(new ApiResponseDto<ResponseUserStatusDto>
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = result.Message,
                    Data = null
                });
            }

            return Ok(new ApiResponseDto<ResponseUserStatusDto>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Data = (ResponseUserStatusDto)result.Data
            });
        }
    }
}
