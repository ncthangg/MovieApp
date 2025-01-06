namespace MovieApp.Common.Base
{
    public interface IServiceResult
    {
        int Status { get; set; }
        string? Message { get; set; }
        object? Data { get; set; }
        int? Count { get; set; }
    }

    public class ServiceResult : IServiceResult
    {
        public int Status { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
        public int? Count { get; set; }

        public ServiceResult()
        {
            Status = -1;
            Message = "Action fail";
        }

        public ServiceResult(int status, string message)
        {
            Status = status;
            Message = message;
        }
        public ServiceResult(int status, string message, object data)
        {
            Status = status;
            Message = message;
            Data = data;
        }
        public ServiceResult(int status, string message, object data, int count = 0)
        {
            Status = status;
            Message = message;
            Data = data;
            Count = count;
        }
    }
}
