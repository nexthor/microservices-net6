namespace Mango.Web.Models
{
    public class ResponseDto
    {
        public string? DisplayMessage { get; set; }
        public bool IsSuccess { get; set; } = true;
        public object? Result { get; set; }
        public string Message { get; set; } = "";
        public IEnumerable<string>? Errors { get; set; }
    }
}
