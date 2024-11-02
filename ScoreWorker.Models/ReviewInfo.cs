using System.Text.Json.Serialization;

namespace ScoreWorker.Models;

public class ReviewInfo
{
    [JsonPropertyName("ID_reviewer")]
    public int IDReviewer { get; set; }
    [JsonPropertyName("ID_under_review")]
    public int IDUnderReview { get; set; }
    [JsonPropertyName("review")]
    public required string Review { get; set; }
}
