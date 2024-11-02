namespace WebLibrary.Backend.Models.Exceptions;

public class ExceptionInfo
{
    public int StatusCode { get; set; }
    public string? Message { get; set; }
    public string? StackTrace { get; set; }
}
