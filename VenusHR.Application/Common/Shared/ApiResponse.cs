 namespace VenusHR.Application.Common.DTOs.Shared
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public string? ErrorCode { get; set; }

         public static ApiResponse<T> Succeeded(T data, string? message = null)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Data = data,
                Message = message ?? "Operation completed successfully"
            };
        }

        public static ApiResponse<T> Failed(string message, string? errorCode = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                ErrorCode = errorCode
            };
        }
    }
}