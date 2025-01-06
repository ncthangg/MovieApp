using Microsoft.AspNetCore.Mvc;
using MovieApp.Common.DTOs.Request;
using MovieApp.Common.DTOs.Response;
using MovieApp.Common.DTOs;
using MovieApp.Service;
using System.Net;

namespace MovieApp.API.Controllers
{
    [Route("type")]
    [ApiController]
    public class TypeController : Controller
    {
        private readonly ServiceWrapper _serviceWrapper;

        public TypeController(ServiceWrapper serviceWrapper)
        {
            _serviceWrapper = serviceWrapper;
        }

        // GET: api/type
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _serviceWrapper.TypeService.GetAllType();
            if (result.Status < 0)
            {
                return NotFound(new ApiResponseDto<IEnumerable<ResponseTypeDto>>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = result.Message,
                    Count = result.Count,
                    Data = null
                });
            }
            return Ok(new ApiResponseDto<IEnumerable<ResponseTypeDto>>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Count = result.Count,
                Data = (IEnumerable<ResponseTypeDto>)result.Data
            });
        }

        // GET: api/type/{id}
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _serviceWrapper.TypeService.GetByTypeId(id);
            if (result.Status < 0)
            {
                return NotFound(new ApiResponseDto<IEnumerable<ResponseTypeDto>>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = result.Message,
                    Data = null
                });
            }
            return Ok(new ApiResponseDto<ResponseTypeDto>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Data = (ResponseTypeDto)result.Data
            });
        }

        // GET: api/type/name={name}
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string name)
        {
            var result = await _serviceWrapper.TypeService.Search(name);

            if (result.Status < 0)
            {
                return NotFound(new ApiResponseDto<IEnumerable<ResponseTypeDto>>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = result.Message,
                    Count = result.Count,
                    Data = null
                });
            }

            return Ok(new ApiResponseDto<IEnumerable<ResponseTypeDto>>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Count = result.Count,
                Data = (IEnumerable<ResponseTypeDto>)result.Data
            });
        }

        // POST: api/type
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RequestTypeDto type)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _serviceWrapper.TypeService.Create(type);

            if (result.Status < 0)
            {
                return NotFound(new ApiResponseDto<ResponseTypeDto>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = result.Message,
                    Data = null
                });
            }

            return Ok(new ApiResponseDto<ResponseTypeDto>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Data = (ResponseTypeDto)result.Data
            });
        }

        // PUT: api/type/{id}
        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] RequestTypeDto type)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _serviceWrapper.TypeService.Update(id, type);

            if (result.Status < 0)
            {
                return NotFound(new ApiResponseDto<ResponseTypeDto>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = result.Message,
                    Data = null
                });
            }

            return Ok(new ApiResponseDto<ResponseTypeDto>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Data = (ResponseTypeDto)result.Data
            });
        }

        // DELETE: api/type/{id}
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _serviceWrapper.TypeService.DeleteByTypeId(id);

            if (result.Status < 0)
            {
                return NotFound(new ApiResponseDto<ResponseTypeDto>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = result.Message,
                    Data = null
                });
            }

            return Ok(new ApiResponseDto<ResponseTypeDto>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Data = (ResponseTypeDto)result.Data
            });
        }

    }
}
