using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Common.DTOs.Request
{
    public class RequestMovieCategoryDto
    {
        public long MovieId { get; set; }
        public long[] CategoryIds { get; set; }
    }
}
