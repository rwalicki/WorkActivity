namespace Shared.Models
{
    public class ServiceResult<T>
    {
        public T Data { get; set; }
        public string Message { get; set; } = null;
        public bool Success { get; set; } = true;
    }
}