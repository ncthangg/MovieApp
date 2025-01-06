namespace MovieApp.Common.DTOs.Response
{
    public class ResponseMovieDto
    {
        public long MovieId { get; set; }
        public string MovieName { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; }
        public string PosterUrl { get; set; }
        public string Director { get; set; }
        public int ReleaseYear { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
