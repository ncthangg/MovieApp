using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Common.DTOs.Request;
using MovieApp.Common.DTOs.Response;
using MovieApp.Common.DTOs;
using MovieApp.Data.Models;
using MovieApp.Service.Services;
using System.Net;
using MovieApp.Service;
using System.Collections.Generic;

namespace MovieApp.API.Controllers
{
    [Route("role")]
    [ApiController]
    public class UserRoleController : Controller
    {
        private readonly ServiceWrapper _serviceWrapper;

        public UserRoleController(ServiceWrapper serviceWrapper)
        {
            _serviceWrapper = serviceWrapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _serviceWrapper.UserRoleService.GetAllUserRole();
            if (result.Status < 0)
            {
                return NotFound(new ApiResponseDto<IEnumerable<ResponseUserRoleDto>>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = result.Message,
                    Data = null
                });
            }

            return Ok(new ApiResponseDto<IEnumerable<ResponseUserRoleDto>>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Data = (IEnumerable<ResponseUserRoleDto>)result.Data
            });
        }

        // GET: api/movie/{id}
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _serviceWrapper.UserRoleService.GetByRoleId(id);
            if (result.Status < 0)
            {
                return NotFound(new ApiResponseDto<IEnumerable<ResponseUserRoleDto>>
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = result.Message,
                    Data = null
                });
            }

            return Ok(new ApiResponseDto<ResponseUserRoleDto>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Data = (ResponseUserRoleDto)result.Data
            });

        }

        // GET: api/movie/name={name}
        [HttpGet("search")]
        public async Task<IActionResult> GetByName([FromQuery] string name)
        {
            var result = await _serviceWrapper.UserRoleService.GetByRoleName(name);
            if (result.Status < 0)
            {
                return NotFound(new ApiResponseDto<IEnumerable<ResponseUserRoleDto>>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = result.Message,
                    Data = null
                });
            }

            return Ok(new ApiResponseDto<IEnumerable<ResponseUserRoleDto>>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Data = (IEnumerable<ResponseUserRoleDto>)result.Data
            });
        }

        // POST: api/movie
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RequestUserRoleDto role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _serviceWrapper.UserRoleService.Create(role);
            if (result.Status < 0)
            {
                return NotFound(new ApiResponseDto<IEnumerable<ResponseUserRoleDto>>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = result.Message,
                    Data = null
                });
            }

            return Ok(new ApiResponseDto<ResponseUserRoleDto>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Data = (ResponseUserRoleDto)result.Data
            });
        }

        // PUT: api/movie/{id}
        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] RequestUserRoleDto role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _serviceWrapper.UserRoleService.Update(id, role);
            if (result.Status < 0)
            {
                return NotFound(new ApiResponseDto<IEnumerable<ResponseUserRoleDto>>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = result.Message,
                    Data = null
                });
            }

            return Ok(new ApiResponseDto<ResponseUserRoleDto>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Data = (ResponseUserRoleDto)result.Data
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

            var result = await _serviceWrapper.UserRoleService.DeleteByRoleId(id);
            if (result.Status < 0)
            {
                return NotFound(new ApiResponseDto<IEnumerable<ResponseUserRoleDto>>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = result.Message,
                    Data = null
                });
            }

            return Ok(new ApiResponseDto<ResponseUserRoleDto>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Data = (ResponseUserRoleDto)result.Data
            });

        }
    }
}
