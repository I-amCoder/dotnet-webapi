using System.Runtime.InteropServices;

namespace api_with_auth.Responses
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public List<string> Errors { get; set; }

        public ApiResponse() { }

        public ApiResponse(T data, string message)
        {
            Success = true;
            Data = data;
            Message = message;
        }

        public ApiResponse(string message, bool success = true)
        {
            Success = success;
            Message = message;
            Data = default;
            Errors = new List<string>();
        }

        public ApiResponse(List<String> errors)
        {
            Success = false;
            Message = "Validation Error";
            Data = default;
            Errors = errors; 
        }
    }
}