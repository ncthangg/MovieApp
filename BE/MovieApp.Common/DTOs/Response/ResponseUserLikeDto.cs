namespace MovieApp.Common.DTOs.Request
{
    public class ResponseUserLikeDto
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public long? MovieId { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
