using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Common.DTOs;
using MovieApp.Common.DTOs.Request;
using MovieApp.Common.DTOs.Response;
using MovieApp.Service;
using MovieApp.Service.Services;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Net;

namespace MovieApp.API.Controllers
{
    [Route("category")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ServiceWrapper _serviceWrapper;

        public CategoryController(ServiceWrapper serviceWrapper)
        {
            _serviceWrapper = serviceWrapper;
        }

        // GET: api/category
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _serviceWrapper.CategoryService.GetAllCategory();
            if (result.Status < 0)
            {
                return NotFound(new ApiResponseDto<IEnumerable<ResponseCategoryDto>>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = result.Message,
                    Data = null
                });
            }
            return Ok(new ApiResponseDto<IEnumerable<ResponseCategoryDto>>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Data = (IEnumerable<ResponseCategoryDto>)result.Data
            });
        }

        // GET: api/category/{id}
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _serviceWrapper.CategoryService.GetByCategoryId(id);
            if (result.Status < 0)
            {
                return NotFound(new ApiResponseDto<IEnumerable<ResponseCategoryDto>>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = result.Message,
                    Data = null
                });
            }
            return Ok(new ApiResponseDto<ResponseCategoryDto>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Data = (ResponseCategoryDto)result.Data
            });
        }

        // GET: api/category/name={name}
        [HttpGet("search")]
        public async Task<IActionResult> GetByName([FromQuery] string name)
        {
            var result = await _serviceWrapper.CategoryService.GetByCategoryName(name);

            if (result.Status < 0)
            {
                return NotFound(new ApiResponseDto<IEnumerable<ResponseCategoryDto>>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = result.Message,
                    Data = null
                });
            }

            return Ok(new ApiResponseDto<IEnumerable<ResponseCategoryDto>>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Data = (IEnumerable<ResponseCategoryDto>)result.Data
            });
        }

        // POST: api/category
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RequestCategoryDto category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _serviceWrapper.CategoryService.Create(category);

            if (result.Status < 0)
            {
                return NotFound(new ApiResponseDto<ResponseCategoryDto>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = result.Message,
                    Data = null
                });
            }

            return Ok(new ApiResponseDto<ResponseCategoryDto>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Data = (ResponseCategoryDto)result.Data
            });
        }

        // PUT: api/category/{id}
        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] RequestCategoryDto category)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _serviceWrapper.CategoryService.Update(id, category);

            if (result.Status < 0)
            {
                return NotFound(new ApiResponseDto<ResponseCategoryDto>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = result.Message,
                    Data = null
                });
            }

            return Ok(new ApiResponseDto<ResponseCategoryDto>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Data = (ResponseCategoryDto)result.Data
            });
        }

        // DELETE: api/category/{id}
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _serviceWrapper.CategoryService.DeleteByCategoryId(id);

            if (result.Status < 0)
            {
                return NotFound(new ApiResponseDto<ResponseCategoryDto>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = result.Message,
                    Data = null
                });
            }

            return Ok(new ApiResponseDto<ResponseCategoryDto>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Data = (ResponseCategoryDto)result.Data
            });
        }


    }
}
