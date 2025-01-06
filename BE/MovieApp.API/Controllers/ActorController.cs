using Microsoft.AspNetCore.Mvc;
using MovieApp.Common.DTOs.Request;
using MovieApp.Common.DTOs.Response;
using MovieApp.Common.DTOs;
using MovieApp.Service;
using System.Net;

namespace MovieApp.API.Controllers
{
    [Route("actor")]
    [ApiController]
    public class ActorController : Controller
    {
        private readonly ServiceWrapper _serviceWrapper;

        public ActorController(ServiceWrapper serviceWrapper)
        {
            _serviceWrapper = serviceWrapper;
        }

        // GET: api/actor
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _serviceWrapper.ActorService.GetAllActor();
            if (result.Status < 0)
            {
                return NotFound(new ApiResponseDto<IEnumerable<ResponseActorDto>>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = result.Message,
                    Count = result.Count,
                    Data = null
                });
            }
            return Ok(new ApiResponseDto<IEnumerable<ResponseActorDto>>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Count = result.Count,
                Data = (IEnumerable<ResponseActorDto>)result.Data
            });
        }

        // GET: api/actor/{id}
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _serviceWrapper.ActorService.GetByActorId(id);
            if (result.Status < 0)
            {
                return NotFound(new ApiResponseDto<IEnumerable<ResponseActorDto>>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = result.Message,
                    Data = null
                });
            }
            return Ok(new ApiResponseDto<ResponseActorDto>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Data = (ResponseActorDto)result.Data
            });
        }

        // GET: api/actor/name={name}
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string name)
        {
            var result = await _serviceWrapper.ActorService.Search(name);

            if (result.Status < 0)
            {
                return NotFound(new ApiResponseDto<IEnumerable<ResponseActorDto>>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = result.Message,
                    Count = result.Count,
                    Data = null
                });
            }

            return Ok(new ApiResponseDto<IEnumerable<ResponseActorDto>>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Count = result.Count,
                Data = (IEnumerable<ResponseActorDto>)result.Data
            });
        }

        // POST: api/actor
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RequestActorDto actor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _serviceWrapper.ActorService.Create(actor);

            if (result.Status < 0)
            {
                return NotFound(new ApiResponseDto<ResponseActorDto>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = result.Message,
                    Data = null
                });
            }

            return Ok(new ApiResponseDto<ResponseActorDto>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Data = (ResponseActorDto)result.Data
            });
        }

        // PUT: api/actor/{id}
        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] RequestActorDto actor)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _serviceWrapper.ActorService.Update(id, actor);

            if (result.Status < 0)
            {
                return NotFound(new ApiResponseDto<ResponseActorDto>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = result.Message,
                    Data = null
                });
            }

            return Ok(new ApiResponseDto<ResponseActorDto>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Data = (ResponseActorDto)result.Data
            });
        }

        // DELETE: api/actor/{id}
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _serviceWrapper.ActorService.DeleteByActorId(id);

            if (result.Status < 0)
            {
                return NotFound(new ApiResponseDto<ResponseActorDto>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = result.Message,
                    Data = null
                });
            }

            return Ok(new ApiResponseDto<ResponseActorDto>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Data = (ResponseActorDto)result.Data
            });
        }
    }
}
