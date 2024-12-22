using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Common.DTOs.Response
{
    public class ResponseUserDto
    {
        public long UserId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public long RoleId { get; set; }

        public long StatusId { get; set; }
        public bool IsVerified { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
