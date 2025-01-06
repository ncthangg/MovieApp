namespace MovieApp.Common.DTOs.Request
{
    public class ResponseUserWatchHistoryDto
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public long? MovieId { get; set; }

        public long? SeasonId { get; set; }

        public long? EpisodeId { get; set; }

        public DateTime? LastWatch { get; set; }

        public long? TimeWatch { get; set; }

    }
}
