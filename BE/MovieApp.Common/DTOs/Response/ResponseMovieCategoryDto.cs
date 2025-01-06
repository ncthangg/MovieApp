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
