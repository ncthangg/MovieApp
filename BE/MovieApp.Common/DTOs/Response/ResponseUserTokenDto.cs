using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Common.DTOs.Response
{
    public class ResponseUserTokenDto
    {
        public long TokenId { get; set; }

        public long? UserId { get; set; }

        public string RefreshToken { get; set; }

        public DateTime? RefreshTokenExpires { get; set; }

        public DateTime? LastLogin { get; set; }
    }
}
