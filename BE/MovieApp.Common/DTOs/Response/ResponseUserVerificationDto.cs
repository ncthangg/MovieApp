namespace MovieApp.Common.DTOs.Response
{
    public class ResponseUserVerificationDto
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public string VerificationCode { get; set; }

        public bool IsUsed { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ExpiresAt { get; set; }
    }
}


