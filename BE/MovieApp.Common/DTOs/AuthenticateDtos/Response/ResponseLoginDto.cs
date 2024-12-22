using MovieApp.Common.DTOs.Response;
using MovieApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Common.DTOs.AuthenticateDtos.Response
{
    public class ResponseLoginDto
    {
        public ResponseUserDto ResponseUserDto { get; set; }
        public ResponseUserTokenDto ResponseUserTokenDto { get; set; }
        public string Token { get; set; }  // JWT Token
    }
}
