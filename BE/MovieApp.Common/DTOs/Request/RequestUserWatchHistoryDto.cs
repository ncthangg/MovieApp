namespace MovieApp.Common.DTOs.Request
{
    public class RequestUserWatchHistoryDto
    {
        public long UserId { get; set; }

        public long? MovieId { get; set; }

        public long? SeasonId { get; set; }

        public long? EpisodeId { get; set; }

        public DateTime? LastWatch { get; set; }

        public long? TimeWatch { get; set; }
    }
}
