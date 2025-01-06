namespace MovieApp.Common.DTOs.Request
{
    public class RequestMovieCategoryDto
    {
        public long MovieId { get; set; }
        public long[] CategoryIds { get; set; }
    }
}
