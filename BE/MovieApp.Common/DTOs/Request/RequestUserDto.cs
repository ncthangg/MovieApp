using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Common.DTOs.Request
{
    public class RequestUserDto
    {
        public string Name { get; set; }

        public long RoleId { get; set; }

        public long StatusId { get; set; }

    }
}
