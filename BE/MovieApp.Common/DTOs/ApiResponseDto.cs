using System.Net;

namespace MovieApp.Common.DTOs
{
    public class ApiResponseDto<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; } = null!;
        public int? Count { get; set; }
        public T? Data { get; set; }
        
    }
}
