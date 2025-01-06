using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Common.DTOs.AuthenticateDtos.Request;
using MovieApp.Common.DTOs.AuthenticateDtos.Response;
using MovieApp.Common.DTOs;
using System.Net;
using MovieApp.Service;
using MovieApp.Service.Services.High;

namespace MovieApp.API.Controllers
{
    [Route("authenticate")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly ServiceWrapper _serviceWrapper;
        private readonly IAuthenticateService _authenticateService;

        public AuthController(
            ServiceWrapper serviceWrapper,
            IAuthenticateService authenticateService
            )
        {
            _serviceWrapper = serviceWrapper;
            _authenticateService = authenticateService;
        }
        //Login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponseDto<ResponseLoginDto>>> Login([FromBody] RequestLoginDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponseDto<string>
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Invalid input data"
                });
            }

            var result = await _authenticateService.Login(request);

            if (result.Status != 1)
                return NotFound(new ApiResponseDto<IEnumerable<ResponseLoginDto>>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = result.Message,
                    Data = null
                });

            return Ok(new ApiResponseDto<ResponseLoginDto>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Data = (ResponseLoginDto)result.Data
            });
        }

        //Register
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponseDto<ResponseRegisterDto>>> Register([FromBody] RequestRegisterDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponseDto<string>
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Invalid input data"
                });
            }

            var result = await _authenticateService.Register(request);

            if (result.Status != 1)
                return NotFound(new ApiResponseDto<IEnumerable<ResponseRegisterDto>>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = result.Message,
                    Data = null
                });

            return Ok(new ApiResponseDto<ResponseRegisterDto>
            {
                StatusCode = HttpStatusCode.OK,
                Message = result.Message,
                Data = (ResponseRegisterDto)result.Data
            });
        }




    }
}
