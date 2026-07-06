namespace ChatApp.API.Meddilware
{
    public class ApiResponse<T>
    {
        public T? Data { get; set; }
        public bool IsSuccess { get; set; }
        public string? Errors { get; set; }
        public string? ErrorCode { get; set; }
        public int StatusCode { get; set; }
        public string? TraceId { get; set; }
    }
}
