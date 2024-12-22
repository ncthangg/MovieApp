using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Common.DTOs.Request
{
    public class RequestMovieDto
    {
        public string MovieName { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; }
        public string PosterUrl { get; set; }
        public string Director { get; set; }
        public int ReleaseYear { get; set; }
    }
}
