using MovieApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Common.DTOs.Response
{
    public class ResponseMovieCategoryDto
    {
        public long MovieId { get; set; }
        public IEnumerable<ResponseCategoryDto> Category { get; set; }
    }

    public class ResponseCategoryMovieDto
    {
        public long CategoryId { get; set; }
        public IEnumerable<ResponseMovieDto> Movie { get; set; }
    }
}
