using MovieApp.Common.DTOs.Response;

namespace MovieApp.Common.DTOs.AuthenticateDtos.Response
{
    public class ResponseLoginDto
    {
        public ResponseUserDto ResponseUserDto { get; set; }
        public ResponseUserTokenDto ResponseUserTokenDto { get; set; }
        public string Token { get; set; }  // JWT Token
    }
}
