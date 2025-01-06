namespace MovieApp.Common.DTOs.Request
{
    public class RequestMovieRateDto
    {
        public long MovieId { get; set; }

        public long UserId { get; set; }

        public long? Vote { get; set; }

        public string? Comment { get; set; }
    }
}
