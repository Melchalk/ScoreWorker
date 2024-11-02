using System.Net;

namespace WebLibrary.Backend.Models.Exceptions;

public class BadRequestException(string message) : StatusCodeException(message, statusCode)
{
    private const HttpStatusCode statusCode = HttpStatusCode.BadRequest;
}
