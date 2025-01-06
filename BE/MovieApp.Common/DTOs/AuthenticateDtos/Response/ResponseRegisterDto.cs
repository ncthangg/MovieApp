using MovieApp.Common.DTOs.Response;

namespace MovieApp.Common.DTOs.AuthenticateDtos.Response
{
    public class ResponseRegisterDto
    {
        public ResponseUserDto ResponseUserDto { get; set; }
        public ResponseUserVerificationDto ResponseUserVerificationDto { get; set; }

    }
}
