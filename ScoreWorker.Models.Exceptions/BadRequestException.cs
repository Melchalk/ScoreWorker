using System.Net;

namespace ScoreWorker.Models.Exceptions;

public class BadRequestException(string message) : StatusCodeException(message, statusCode)
{
    private const HttpStatusCode statusCode = HttpStatusCode.BadRequest;
}
