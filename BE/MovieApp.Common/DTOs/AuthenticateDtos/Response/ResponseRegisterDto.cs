using MovieApp.Common.DTOs.Response;
using MovieApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Common.DTOs.AuthenticateDtos.Response
{
    public class ResponseRegisterDto
    {
        public ResponseUserDto ResponseUserDto { get; set; }
        public ResponseUserVerificationDto ResponseUserVerificationDto { get; set; }

    }
}
